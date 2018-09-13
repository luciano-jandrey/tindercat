using System;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;

namespace Tindercat.Bases
{
    public class RepositoryBase
    {
        public async Task<T> ExecuteAsync<T>(IRestRequest request) where T : class, new()
        {
            var client = new RestClient
            {
                BaseUrl = new Uri(AppConsts.ApiBaseUrl),
                Timeout = 10000
            };
            
            client.AddDefaultParameter("x-api-key", AppConsts.ApiKey);

            var tcs = new TaskCompletionSource<T>();

            client.ExecuteAsync<T>(request, response =>
            {
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        tcs.SetResult(JsonConvert.DeserializeObject<T>(response.Content));
                        break;
                    case HttpStatusCode.BadRequest:
                        tcs.SetResult(JsonConvert.DeserializeObject<T>(response.Content));
                        break;
                    default:
                        tcs.SetException(new ApplicationException(response.Content, response.ErrorException));
                        break;
                }
            });

            return await tcs.Task;
        }
    }
}