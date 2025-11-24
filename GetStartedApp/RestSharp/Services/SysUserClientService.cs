using GetStartedApp.RestSharp.IServices;
using RestSharp;
using System.Threading.Tasks;

namespace GetStartedApp.RestSharp.Services
{
    public class SysUserClientService : ISysUserClientService
    {
        private readonly HttpRestClient client;
        private readonly string serviceName = "SysUsers";

        public SysUserClientService(HttpRestClient client)
        {
            this.client = client;
        }

        public async Task<ApiResponse> LoginAsync(string userName, string password)
        {
            BaseRequest request = new BaseRequest();
            request.Method = Method.Post;
            request.Route = $"api/{serviceName}/login";
            request.Parameter = new { UserName = userName, Password = password };
            var result = await client.ExcuteAsync(request);
            return result;
        }
    }
}

