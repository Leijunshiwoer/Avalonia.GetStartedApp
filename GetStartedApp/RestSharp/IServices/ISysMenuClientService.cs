using GetStartedApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.RestSharp.IServices
{
    public interface ISysMenuClientService
    {
        Task<ApiResponse<List<MenuDto>>> GetMenuTreeAsync();
    }
}
