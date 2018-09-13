using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tindercat.Cats
{
    public interface ICatService
    {
        Task<List<Cat>> GetAllAsync(int page = 0, int limit = AppConsts.ApiDefaultLimit);

        Task<Cat> GetAsync(string id);
    }
}