using GetStartedApp.RestSharp;
using System.Threading.Tasks;

namespace GetStartedApp.RestSharp.IServices
{
    public interface ISysUserClientService
    {
        Task<ApiResponse> LoginAsync(string userName, string password);
    }
}

