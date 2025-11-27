using GetStartedApp.Models;
using GetStartedApp.RestSharp.IServices;
using GetStartedApp.SqlSugar.Tables;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GetStartedApp.RestSharp.Services
{
    public class SysRoleClientService : ISysRoleClientService
    {
        private readonly HttpRestClient client;
        private readonly string serviceName = "SysRoles";

        public SysRoleClientService(HttpRestClient client)
        {
            this.client = client;
        }

        public async Task<ApiResponse<List<RoleDto>>> GetRoleLessSortAsync(int sort)
        {
            BaseRequest request = new BaseRequest();
            request.Method = Method.Get;
            request.Route = $"api/{serviceName}/lesssort/{sort}";
            var result = await client.ExcuteAsync<List<RoleDto>>(request);
            return result;
        }

        public async Task<ApiResponse<List<RoleDto>>> GetRoleLessSortByRoleIdAsync(int roleId)
        {
            BaseRequest request = new BaseRequest();
            request.Method = Method.Get;
            request.Route = $"api/{serviceName}/lesssortbyroleid/{roleId}";
            var result = await client.ExcuteAsync<List<RoleDto>>(request);
            return result;
        }
    }
}