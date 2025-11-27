using System;
using GetStartedApp.SqlSugar.IServices;
using Microsoft.AspNetCore.Mvc;

namespace GetStartedApp.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Tags("系统角色管理")]
    public class SysRolesController : ApiControllerBase
    {
        private readonly ILogger<SysRolesController> _logger;
        private readonly ISysRoleService _sysRoleService;

        public SysRolesController(ILogger<SysRolesController> logger, ISysRoleService sysRoleService)
        {
            _logger = logger;
            _sysRoleService = sysRoleService;
        }

        [HttpGet("lesssort/{sort:int}")]
        public IActionResult GetBySort(int sort)
        {
            try
            {
                var roles = _sysRoleService.GetRoleLessSort(sort);
                return Success(roles, "获取角色成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取角色列表失败");
                return Failure("获取角色列表失败");
            }
        }

     
    }
}

