using Xamarin.Forms;

namespace Tindercat.Views
{
    public partial class CatListPage : ContentPage
    {
        public CatListPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}