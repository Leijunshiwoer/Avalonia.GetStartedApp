using System;
using System.Linq;
using GetStartedApp.SqlSugar.IServices;
using GetStartedApp.SqlSugar.Tables;
using Microsoft.AspNetCore.Mvc;

namespace GetStartedApp.WebApi.Controllers
{
    [ApiController]
    [Route("api/version-primary")]
    [Tags("版本一级配置")]
    public class VersionPrimaryController : ApiControllerBase
    {
        private readonly ILogger<VersionPrimaryController> _logger;
        private readonly IBase_Version_Primary_Config_Service _versionPrimaryService;

        public VersionPrimaryController(
            ILogger<VersionPrimaryController> logger,
            IBase_Version_Primary_Config_Service versionPrimaryService)
        {
            _logger = logger;
            _versionPrimaryService = versionPrimaryService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var list = _versionPrimaryService.GetAll();
                return Success(list, "获取一级版本成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取一级版本列表失败");
                return Failure("获取一级版本列表失败");
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var entity = _versionPrimaryService.GetAll().FirstOrDefault(x => x.Id == id);
                return entity == null ? Failure("未找到对应的一级版本") : Success(entity, "获取一级版本成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "根据 Id={Id} 获取一级版本失败", id);
                return Failure("获取一级版本失败");
            }
        }

        [HttpGet("paged")]
        public IActionResult GetPaged([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 50)
        {
            try
            {
                long total = 0;
                var items = _versionPrimaryService.GetVersionPrimayPageList(ref total, pageIndex, pageSize);
                return Success(new { total, items }, "获取一级版本分页数据成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "分页获取一级版本失败");
                return Failure("分页获取一级版本失败");
            }
        }

        [HttpGet("tree")]
        public IActionResult GetTree()
        {
            try
            {
                var tree = _versionPrimaryService.GetVersionPrimayTree();
                return Success(tree, "获取一级版本树成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取一级版本树失败");
                return Failure("获取一级版本树失败");
            }
        }

        [HttpGet("exist")]
        public IActionResult CheckExist([FromQuery] string code, [FromQuery] string name, [FromQuery] int id = 0)
        {
            if (string.IsNullOrWhiteSpace(code) && string.IsNullOrWhiteSpace(name))
            {
                return Failure("编码或名称至少提供一个");
            }

            try
            {
                var result = _versionPrimaryService.IsExist(code, name, id);
                return Success(new { exists = result }, "校验成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "校验一级版本唯一性失败");
                return Failure("校验失败");
            }
        }

        [HttpPost]
        public IActionResult Save([FromBody] Base_Version_Primary_Config entity)
        {
            if (entity == null)
            {
                return Failure("保存内容不能为空");
            }

            try
            {
                var id = _versionPrimaryService.InsertOrUpdate(entity);
                var target = _versionPrimaryService.GetAll().FirstOrDefault(x => x.Id == id);
                return Success(target, "保存一级版本成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "保存一级版本失败");
                return Failure("保存一级版本失败");
            }
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id, [FromQuery] bool utterly = false)
        {
            try
            {
                _versionPrimaryService.DeletedById(id, utterly);
                return Success(null, "删除一级版本成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "删除一级版本失败");
                return Failure("删除一级版本失败");
            }
        }
    }
}

