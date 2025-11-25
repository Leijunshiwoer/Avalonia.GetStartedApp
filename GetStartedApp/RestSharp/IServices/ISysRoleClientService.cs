using GetStartedApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.RestSharp.IServices
{
    public interface ISysRoleClientService
    {
        Task<ApiResponse<List<RoleDto>>> GetRoleLessSortAsync(int sort);
        Task<ApiResponse<List<RoleDto>>> GetRoleLessSortByUserIdAsync(int userId, int sort);
    }
}