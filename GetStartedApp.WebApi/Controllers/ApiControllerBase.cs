using GetStartedApp.WebApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace GetStartedApp.WebApi.Controllers
{
    /// <summary>
    /// 提供统一的响应封装方法
    /// </summary>
    public abstract class ApiControllerBase : ControllerBase
    {
        protected IActionResult Success(object? data = null, string message = "操作成功")
        {
            return Ok(ApiResponse.Success(data, message));
        }

        protected IActionResult Failure(string message = "操作失败", object? data = null)
        {
            return BadRequest(ApiResponse.Fail(message, data));
        }
    }
}

