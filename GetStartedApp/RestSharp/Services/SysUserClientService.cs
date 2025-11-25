using GetStartedApp.Models;
using GetStartedApp.RestSharp.IServices;
using RestSharp;
using System.Collections.Generic;
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

        public async Task<ApiResponse<List<UserDto>>> GetUsersAsync()
        {
            BaseRequest request = new BaseRequest();
            request.Method = Method.Get;
            request.Route = $"api/{serviceName}";
            var result = await client.ExcuteAsync<List<UserDto>>(request);
            return result;
        }

        public async Task<ApiResponse> InsertOrUpdateUserAsync(UserDto user)
        {
            BaseRequest request = new BaseRequest();
            request.Method = Method.Post;
            request.Route = $"api/{serviceName}";
            request.Parameter = user;
            var result = await client.ExcuteAsync(request);
            return result;
        }

        public async Task<ApiResponse<UserDto>> GetUserByIdAsync(int id)
        {
            BaseRequest request = new BaseRequest();
            request.Method = Method.Get;
            request.Route = $"api/{serviceName}/{id}";
            var result = await client.ExcuteAsync<UserDto>(request);
            return result;
        }
    }
}

