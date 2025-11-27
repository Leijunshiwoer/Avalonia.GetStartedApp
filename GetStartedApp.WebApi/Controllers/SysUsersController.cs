using System;
using System.ComponentModel.DataAnnotations;
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

        /// <summary>
        /// 获取所有用户列表
        /// </summary>
        /// <returns>用户列表</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAll()
        {
            try
            {
                _logger.LogInformation("开始获取所有用户列表");
                var users = _sysUserService.GetUsers();
                _logger.LogInformation("获取用户列表成功，共{Count}条记录", users?.Count ?? 0);
                return Success(users, "获取用户列表成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取用户列表失败");
                return Failure("获取用户列表失败，请稍后重试");
            }
        }

        /// <summary>
        /// 根据ID获取用户信息
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns>用户信息</returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetById([Required(ErrorMessage = "用户ID不能为空")] int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("获取用户信息失败：无效的用户ID {Id}", id);
                    return Failure("无效的用户ID");
                }

                _logger.LogInformation("开始获取ID为{Id}的用户信息", id);
                var user = _sysUserService.GetUserById(id);

                if (user == null)
                {
                    _logger.LogWarning("未找到ID为{Id}的用户", id);
                    return Failure("未找到该用户");
                }

                _logger.LogInformation("获取ID为{Id}的用户信息成功", id);
                return Success(user, "获取用户信息成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "根据ID={Id}获取用户信息失败", id);
                return Failure("获取用户信息失败，请稍后重试");
            }
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="sysUser">用户信息</param>
        /// <returns>新增的用户信息</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Create([FromBody, Required(ErrorMessage = "用户信息不能为空")] SysUser sysUser)
        {
            try
            {
                // 参数验证
                if (string.IsNullOrWhiteSpace(sysUser.Name))
                {
                    return Failure("用户名不能为空");
                }

                if (string.IsNullOrWhiteSpace(sysUser.Password))
                {
                    return Failure("密码不能为空");
                }

                if (string.IsNullOrWhiteSpace(sysUser.JobNumber))
                {
                    return Failure("工号不能为空");
                }

                _logger.LogInformation("开始新增用户：{UserName}", sysUser.Name);
                var id = _sysUserService.InserOrUpdateUser(sysUser);
                var createdUser = _sysUserService.GetUserById(id);
                _logger.LogInformation("新增用户成功，用户ID：{Id}", id);
                return Success(createdUser, "新增用户成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "新增用户失败");
                return Failure("新增用户失败，请稍后重试");
            }
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <param name="sysUser">用户信息</param>
        /// <returns>更新后的用户信息</returns>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Update([Required(ErrorMessage = "用户ID不能为空")] int id, [FromBody, Required(ErrorMessage = "用户信息不能为空")] SysUser sysUser)
        {
            try
            {
                if (id <= 0)
                {
                    return Failure("无效的用户ID");
                }

                // 检查用户是否存在
                var existingUser = _sysUserService.GetUserById(id);
                if (existingUser == null)
                {
                    return Failure("未找到该用户");
                }

                // 参数验证
                if (string.IsNullOrWhiteSpace(sysUser.Name))
                {
                    return Failure("用户名不能为空");
                }

                if (string.IsNullOrWhiteSpace(sysUser.JobNumber))
                {
                    return Failure("工号不能为空");
                }

                // 确保ID一致
                sysUser.Id = id;

                _logger.LogInformation("开始更新用户：{Id}", id);
                var updatedId = _sysUserService.InserOrUpdateUser(sysUser);
                var updatedUser = _sysUserService.GetUserById(updatedId);
                _logger.LogInformation("更新用户成功，用户ID：{Id}", updatedId);
                return Success(updatedUser, "更新用户成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新用户失败，用户ID：{Id}", id);
                return Failure("更新用户失败，请稍后重试");
            }
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns>删除结果</returns>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Delete([Required(ErrorMessage = "用户ID不能为空")] int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("删除用户失败：无效的用户ID {Id}", id);
                    return Failure("无效的用户ID");
                }

                // 检查用户是否存在
                var existingUser = _sysUserService.GetUserById(id);
                if (existingUser == null)
                {
                    _logger.LogWarning("删除用户失败：未找到ID为{Id}的用户", id);
                    return Failure("未找到该用户");
                }

                _logger.LogInformation("开始删除用户：{Id}", id);
                var result = _sysUserService.DeleteUser(id);
                if (result)
                {
                    _logger.LogInformation("删除用户成功，用户ID：{Id}", id);
                    return Success(null, "删除用户成功");
                }
                else
                {
                    _logger.LogWarning("删除用户失败：删除操作未影响任何记录，用户ID：{Id}", id);
                    return Failure("删除用户失败，请稍后重试");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "删除用户失败，用户ID：{Id}", id);
                return Failure("删除用户失败，请稍后重试");
            }
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="request">登录请求</param>
        /// <returns>登录结果</returns>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Login([FromBody, Required(ErrorMessage = "登录信息不能为空")] LoginRequest request)
        {
            try
            {
                // 参数验证
                if (string.IsNullOrWhiteSpace(request.UserName))
                {
                    return Failure("用户名不能为空");
                }

                if (string.IsNullOrWhiteSpace(request.Password))
                {
                    return Failure("密码不能为空");
                }

                _logger.LogInformation("用户{UserName}尝试登录", request.UserName);
                var user = _sysUserService.Login(request.UserName, request.Password);

                if (user == null)
                {
                    _logger.LogWarning("用户{UserName}登录失败：用户名或密码错误", request.UserName);
                    return Failure("用户名或密码错误");
                }

                _logger.LogInformation("用户{UserName}登录成功", request.UserName);
                return Success(user, "登录成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "用户登录失败");
                return Failure("登录失败，请稍后重试");
            }
        }
    }
}

