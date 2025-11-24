using System;
using GetStartedApp.SqlSugar.IServices;
using GetStartedApp.SqlSugar.Tables;
using GetStartedApp.WebApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace GetStartedApp.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Tags("系统用户管理")]
    public class SysUsersController : ApiControllerBase
    {
        private readonly ILogger<SysUsersController> _logger;
        private readonly ISysUserService _sysUserService;

        public SysUsersController(ILogger<SysUsersController> logger, ISysUserService sysUserService)
        {
            _logger = logger;
            _sysUserService = sysUserService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var users = _sysUserService.GetUsers();
                return Success(users, "获取用户列表成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取用户列表失败");
                return Failure("获取用户列表失败");
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var user = _sysUserService.GetUserById(id);
                return Success(user, "获取用户信息成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "根据 Id={Id} 获取用户失败", id);
                return Failure("获取用户信息失败");
            }
        }

        [HttpPost]
        public IActionResult Save([FromBody] SysUser sysUser)
        {
            if (sysUser == null)
            {
                return Failure("用户内容不能为空");
            }

            try
            {
                var id = _sysUserService.InserOrUpdateUser(sysUser);
                var user = _sysUserService.GetUserById(id);
                return Success(user, "保存用户成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "保存用户失败");
                return Failure("保存用户失败");
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password))
            {
                return Failure("用户名和密码不能为空");
            }

            try
            {
                var result = _sysUserService.Login(request.UserName, request.Password);
                return result ? Success(null, "登录成功") : Failure("用户名或密码错误");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "用户登录失败");
                return Failure("登录失败");
            }
        }
    }
}

