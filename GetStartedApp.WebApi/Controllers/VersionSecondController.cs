using System;
using System.Collections.Generic;
using System.Linq;
using GetStartedApp.SqlSugar.IServices;
using GetStartedApp.SqlSugar.Tables;
using GetStartedApp.WebApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace GetStartedApp.WebApi.Controllers
{
    [ApiController]
    [Route("api/version-second")]
    [Tags("版本二级配置")]
    public class VersionSecondController : ApiControllerBase
    {
        private readonly ILogger<VersionSecondController> _logger;
        private readonly IBase_Version_Second_Config_Service _versionSecondService;

        public VersionSecondController(
            ILogger<VersionSecondController> logger,
            IBase_Version_Second_Config_Service versionSecondService)
        {
            _logger = logger;
            _versionSecondService = versionSecondService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var list = _versionSecondService.GetAll();
                return Success(list, "获取二级版本成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取二级版本失败");
                return Failure("获取二级版本失败");
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var entity = _versionSecondService.GetAll().FirstOrDefault(x => x.Id == id);
                return entity == null ? Failure("未找到对应二级版本") : Success(entity, "获取二级版本成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "根据 Id={Id} 获取二级版本失败", id);
                return Failure("获取二级版本失败");
            }
        }

        [HttpGet("paged")]
        public IActionResult GetPaged([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 50, [FromQuery] bool desc = false)
        {
            try
            {
                int total = 0;
                var items = desc
                    ? _versionSecondService.GetAllPageDesc(ref total, pageIndex, pageSize)
                    : _versionSecondService.GetAllPage(ref total, pageIndex, pageSize);
                return Success(new { total, items }, "获取二级版本分页数据成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "分页获取二级版本失败");
                return Failure("分页获取二级版本失败");
            }
        }

        [HttpGet("with-primary")]
        public IActionResult GetWithPrimary()
        {
            try
            {
                var list = _versionSecondService.GetVersionSeconds();
                return Success(list, "获取二级版本（含一级信息）成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取二级版本（含一级信息）失败");
                return Failure("获取数据失败");
            }
        }

        [HttpGet("without-routes")]
        public IActionResult GetWithoutRoutes([FromQuery] List<int> routeIds)
        {
            try
            {
                var ids = routeIds ?? new List<int>();
                var list = ids.Count == 0
                    ? _versionSecondService.GetVersionSeconds()
                    : _versionSecondService.GetVersionSecondsByNotContainsRouteId(ids);
                return Success(list, "获取二级版本成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取未绑定工艺路线的二级版本失败");
                return Failure("获取数据失败");
            }
        }

        [HttpGet("route/{routeId:int}")]
        public IActionResult GetByRoute(int routeId)
        {
            try
            {
                var list = _versionSecondService.GetVersionSecondsByRouteId(routeId);
                return Success(list, "获取工艺路线关联的二级版本成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "根据工艺路线获取二级版本失败");
                return Failure("获取数据失败");
            }
        }

        [HttpPost("group-by-primary")]
        public IActionResult GroupByPrimary([FromBody] List<int> secondIds)
        {
            if (secondIds == null || secondIds.Count == 0)
            {
                return Failure("二级版本 Id 列表不能为空");
            }

            try
            {
                var list = _versionSecondService.GetGroupByPrimaryIdsByContainsSecondId(secondIds);
                return Success(list, "按一级版本分组成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "根据二级版本 Id 列表获取一级版本失败");
                return Failure("获取数据失败");
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
                var result = _versionSecondService.IsExist(code, name, id);
                return Success(new { exists = result }, "校验成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "校验二级版本唯一性失败");
                return Failure("校验失败");
            }
        }

        [HttpGet("exist-by-parent")]
        public IActionResult CheckExistByParent([FromQuery] int primaryId, [FromQuery] string code, [FromQuery] string name, [FromQuery] int id = 0)
        {
            if (primaryId <= 0 || string.IsNullOrWhiteSpace(code))
            {
                return Failure("父级 Id 和编码不能为空");
            }

            try
            {
                var result = _versionSecondService.IsExistByParentId(primaryId, code, name, id);
                return Success(new { exists = result }, "校验成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "校验二级版本（包含父级）失败");
                return Failure("校验失败");
            }
        }

        [HttpPost]
        public IActionResult Save([FromBody] Base_Version_Second_Config entity)
        {
            if (entity == null)
            {
                return Failure("保存内容不能为空");
            }

            try
            {
                var id = _versionSecondService.InsertOrUpdate(entity);
                var target = _versionSecondService.GetAll().FirstOrDefault(x => x.Id == id);
                return Success(target, "保存二级版本成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "保存二级版本失败");
                return Failure("保存失败");
            }
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id, [FromQuery] bool utterly = false)
        {
            try
            {
                _versionSecondService.DeletedById(id, utterly);
                return Success(null, "删除二级版本成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "删除二级版本失败");
                return Failure("删除失败");
            }
        }

        [HttpPost("route")]
        public IActionResult UpdateRoute([FromBody] SecondRouteUpdateRequest request)
        {
            if (request == null || request.SecondId <= 0)
            {
                return Failure("二级版本 Id 不可为空");
            }

            try
            {
                var rows = _versionSecondService.UpdateSecondRouteIdById(request.SecondId, request.RouteId);
                return Success(new { rows }, "更新二级版本路线成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新二级版本路线失败");
                return Failure("更新失败");
            }
        }

        [HttpPost("route/swap")]
        public IActionResult SwapRoute([FromBody] RouteMappingAdjustRequest request)
        {
            if (request == null || request.OldRouteId <= 0 || request.NewRouteId <= 0)
            {
                return Failure("旧路线与新路线必须有效");
            }

            try
            {
                var rows = _versionSecondService.UpdateSecondRouteIdByRouteId(request.OldRouteId, request.NewRouteId);
                return Success(new { rows }, "批量更新二级版本路线成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "批量更新二级版本路线失败");
                return Failure("更新失败");
            }
        }
    }
}

