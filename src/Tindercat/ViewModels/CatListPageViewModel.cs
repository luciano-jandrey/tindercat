using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;
using Prism.Logging;
using Prism.Services;
using Tindercat.Cats;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Plugin.Share;
using Plugin.Share.Abstractions;

namespace Tindercat.ViewModels
{
    public class CatListPageViewModel : ViewModelBase
    {
        private readonly ICatService _catService;

        public CatListPageViewModel(INavigationService navigationService, 
                                    IPageDialogService pageDialogService, 
                                    IDeviceService deviceService,
                                    ICatService catService) : base(navigationService, pageDialogService, deviceService)
        {
            _catService = catService;

            Cats = new ObservableCollection<Cat>();
        }

        private int _lastIndex;

        public ObservableCollection<Cat> Cats { get; set; }

		public int CardIndex { get; set; }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);

            LoadMoreDataAsync();
        }

        private void LoadMoreDataAsync()
        {
            if (IsBusy) return;

            IsBusy = true;

            _deviceService.BeginInvokeOnMainThread(async () => {
                try
                {
                    foreach (var cat in await _catService.GetAllAsync())
                    {
                        Cats.Add(cat);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
                finally
                {
                    IsBusy = false;
                }
            });
        }

        private DelegateCommand _cardSelectedCommand;
        public DelegateCommand CardSelectedCommand =>
        _cardSelectedCommand ?? (_cardSelectedCommand = new DelegateCommand(ExecuteCardSelectedCommand));

        private void ExecuteCardSelectedCommand()
        {
            if (_lastIndex >= CardIndex) return;

            if (CardIndex >= (Cats.Count - 3))
            {
                LoadMoreDataAsync();
            }

            _lastIndex = CardIndex;
        }

        private DelegateCommand _shareCommand;
        public DelegateCommand ShareCommand =>
        _shareCommand ?? (_shareCommand = new DelegateCommand(ExecuteShareCommand));

        private async void ExecuteShareCommand()
        {
            if (Cats == null) return;

            var cat = Cats.ElementAt(CardIndex);

            if (cat == null) return;

            if (CrossShare.IsSupported)
            {
                if (await CrossShare.Current.Share(new ShareMessage
                {
                    Title = "Miauuu!",
                    Url = cat.Url
                }))
                {
                    Debug.WriteLine($"{cat.Url} shared");
                };
            }
        }
    }
}
