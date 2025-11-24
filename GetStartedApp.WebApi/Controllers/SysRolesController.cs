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

        [HttpGet("less-than/{sort:int}")]
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

        [HttpGet("user/{userId:int}/less-than/{sort:int}")]
        public IActionResult GetByUser(int userId, int sort)
        {
            try
            {
                var roles = _sysRoleService.GetRoleLessSortByUserId(userId, sort);
                return Success(roles, "获取用户角色成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "根据用户获取角色失败");
                return Failure("获取用户角色失败");
            }
        }
    }
}

