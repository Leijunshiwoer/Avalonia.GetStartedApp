using System;
using System.Collections.Generic;
using GetStartedApp.SqlSugar.IServices;
using GetStartedApp.SqlSugar.Tables;
using Microsoft.AspNetCore.Mvc;

namespace GetStartedApp.WebApi.Controllers
{
    [ApiController]
    [Route("api/routes")]
    [Tags("工艺路线配置")]
    public class RouteConfigController : ApiControllerBase
    {
        private readonly ILogger<RouteConfigController> _logger;
        private readonly IBase_Route_Config_Service _routeService;

        public RouteConfigController(
            ILogger<RouteConfigController> logger,
            IBase_Route_Config_Service routeService)
        {
            _logger = logger;
            _routeService = routeService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var list = _routeService.GetAll();
                return Success(list, "获取工艺路线成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取工艺路线列表失败");
                return Failure("获取工艺路线列表失败");
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var entity = _routeService.GetById(id);
                return Success(entity, "获取工艺路线成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "根据 Id 获取工艺路线失败");
                return Failure("获取工艺路线失败");
            }
        }

        [HttpGet("second/{secondId:int}")]
        public IActionResult GetBySecondId(int secondId)
        {
            try
            {
                var route = _routeService.GetBySecondId(secondId);
                return Success(route, "获取二级版本绑定路线成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "根据二级版本获取工艺路线失败");
                return Failure("获取工艺路线失败");
            }
        }

        [HttpGet("paged")]
        public IActionResult GetPaged([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 50)
        {
            try
            {
                long total = 0;
                var items = _routeService.GetRoutePage(ref total, pageIndex, pageSize);
                return Success(new { total, items }, "获取工艺路线分页数据成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "分页获取工艺路线失败");
                return Failure("分页获取工艺路线失败");
            }
        }

        [HttpGet("task/{taskId:int}/paged")]
        public IActionResult GetPagedByTask(int taskId, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 50)
        {
            try
            {
                long total = 0;
                var items = _routeService.GetRoutePageByTaskId(taskId, ref total, pageIndex, pageSize);
                return Success(new { total, items }, "获取任务关联工艺路线成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "分页获取任务工艺路线失败");
                return Failure("分页获取工艺路线失败");
            }
        }

        [HttpGet("ids")]
        public IActionResult GetIds()
        {
            try
            {
                var ids = _routeService.GetRouteIdList();
                return Success(ids, "获取工艺路线 Id 列表成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取工艺路线 Id 列表失败");
                return Failure("获取工艺路线 Id 列表失败");
            }
        }

        [HttpGet("contains")]
        public IActionResult GetContains([FromQuery] List<int> ids, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 50)
        {
            if (ids == null || ids.Count == 0)
            {
                return Failure("Id 列表不能为空");
            }

            try
            {
                long total = 0;
                var items = _routeService.GetPageAllContains(ids, ref total, pageIndex, pageSize);
                return Success(new { total, items }, "获取指定工艺路线成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取指定工艺路线失败");
                return Failure("获取失败");
            }
        }

        [HttpGet("not-contains")]
        public IActionResult GetNotContains([FromQuery] List<int> ids, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 50)
        {
            ids ??= new List<int>();

            try
            {
                long total = 0;
                var items = ids.Count == 0
                    ? _routeService.GetRoutePage(ref total, pageIndex, pageSize)
                    : _routeService.GetPageAllNotContains(ids, ref total, pageIndex, pageSize);
                return Success(new { total, items }, "获取未包含的工艺路线成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取未包含的工艺路线失败");
                return Failure("获取失败");
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
                var exists = _routeService.IsExist(code, name, id);
                return Success(new { exists }, "校验成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "校验工艺路线唯一性失败");
                return Failure("校验失败");
            }
        }

        [HttpPost]
        public IActionResult Save([FromBody] Base_Route_Config route)
        {
            if (route == null)
            {
                return Failure("保存内容不能为空");
            }

            try
            {
                var id = _routeService.InsertOrUpdateReturnIdentity(route);
                var entity = _routeService.GetById(id);
                return Success(entity, "保存工艺路线成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "保存工艺路线失败");
                return Failure("保存工艺路线失败");
            }
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id, [FromQuery] bool utterly = false)
        {
            try
            {
                _routeService.DeletedById(id, utterly);
                return Success(null, "删除工艺路线成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "删除工艺路线失败");
                return Failure("删除工艺路线失败");
            }
        }
    }
}

