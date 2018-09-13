using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;
using Tindercat.Bases;

namespace Tindercat.Cats
{
    public class CatService : RepositoryBase, ICatService
    {
        public async Task<List<Cat>> GetAllAsync(int page = 0, int limit = 10)
        {
            var request = new RestRequest
            {
                Resource = "/images/search",
                Method = Method.GET,
                RequestFormat = DataFormat.Json,
            };

            request.AddParameter("page", page.ToString());
            request.AddParameter("limit", limit.ToString());

            return await ExecuteAsync<List<Cat>>(request);
        }

        public async Task<Cat> GetAsync(string id)
        {
            var request = new RestRequest
            {
                Resource = $"/images/{id}",
                Method = Method.GET,
                RequestFormat = DataFormat.Json
            };
            
            return await ExecuteAsync<Cat>(request);
        }
    }
}