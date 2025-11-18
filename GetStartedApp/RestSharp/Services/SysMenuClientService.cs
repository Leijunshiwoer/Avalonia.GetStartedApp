
using GetStartedApp.Models;
using GetStartedApp.RestSharp.IServices;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.RestSharp.Services
{
    public class SysMenuClientService : ISysMenuClientService
    {
        private readonly HttpRestClient client;
        private readonly string serviceName = "SysMenu";

        public SysMenuClientService(HttpRestClient client)
        {
            this.client = client;
        }

        public async Task<ApiResponse<List<MenuDto>>> GetMenuTreeAsync()
        {
            BaseRequest request = new BaseRequest();
            request.Method = Method.Get;
            request.Route = $"api/{serviceName}/";
            var result = await client.ExcuteAsync<List<MenuDto>>(request);
            return result;
        }
    }
}
