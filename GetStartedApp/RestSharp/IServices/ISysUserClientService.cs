using GetStartedApp.Models;
using GetStartedApp.RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GetStartedApp.RestSharp.IServices
{
    public interface ISysUserClientService
    {
        Task<ApiResponse<UserDto>> LoginAsync(string userName, string password);
        Task<ApiResponse<List<UserDto>>> GetUsersAsync();
        Task<ApiResponse> InsertOrUpdateUserAsync(UserDto user);
        Task<ApiResponse<UserDto>> GetUserByIdAsync(int id);
    }
}

