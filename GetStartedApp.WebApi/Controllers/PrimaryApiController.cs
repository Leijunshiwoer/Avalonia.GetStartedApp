using GetStartedApp.SqlSugar.IServices;
using GetStartedApp.SqlSugar.Services;
using GetStartedApp.WebApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace GetStartedApp.WebApi.Controllers
{
    [ApiController]
    [Tags("1.主要API控制器")]
    public class PrimaryApiController : ControllerBase
    {
        private readonly ILogger<PrimaryApiController> _logger;
        private readonly ISysMenuService _sysMenuService;

        public PrimaryApiController(ILogger<PrimaryApiController> logger,
            ISysMenuService sysMenuService)
        {
            _logger = logger;
            _sysMenuService = sysMenuService;
        }


        #region 菜单管理

        [HttpGet]
        [Route("/api/SysMenu/")]
        public virtual async Task<IActionResult> ApiSysMenuGetAsync()
        {
            var list = await _sysMenuService.GetMenuTreeAsync();
            var obj = new ApiResponse()
            {
                Status=true,
                Message="获取菜单列表成功!",
                Data= list
            };
            return new ObjectResult(obj);
        }

        #endregion
    }
}
