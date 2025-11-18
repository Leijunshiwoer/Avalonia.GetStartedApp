using System.Collections.Generic;
using System.Threading.Tasks;

namespace GetStartedApp.RestSharp
{
    public interface IRestSharpApiClient<TEntity> where TEntity : class
    {
        Task<ApiResponse<TEntity>> AddAsync(TEntity entity);

        Task<ApiResponse<TEntity>> UpdateAsync(TEntity entity);

        Task<ApiResponse> DeleteByIdAsync(int id);

        Task<ApiResponse> DeleteByGuidAsync(string guid);

        Task<ApiResponse> DeleteMultipleAsync(IList<TEntity> entities);

        Task<ApiResponse<TEntity>> GetSingleByCodeAsync(string code);

        Task<ApiResponse<TEntity>> GetSingleByIdAsync(int id);

        Task<ApiResponse<TEntity>> GetSingleByGuidAsync(string guid);
    }
}