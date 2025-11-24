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
    [Route("api/route-steps")]
    [Tags("工艺路线-工位关联")]
    public class RouteProcessStepController : ApiControllerBase
    {
        private readonly ILogger<RouteProcessStepController> _logger;
        private readonly IBase_Route_ProcessStep_Config_Service _routeStepService;

        public RouteProcessStepController(
            ILogger<RouteProcessStepController> logger,
            IBase_Route_ProcessStep_Config_Service routeStepService)
        {
            _logger = logger;
            _routeStepService = routeStepService;
        }

        [HttpGet("{routeId:int}")]
        public IActionResult Get(int routeId)
        {
            try
            {
                var ids = _routeStepService.GetRouteProcessStepIdList(routeId);
                return Success(ids, "获取工艺路线关联工位成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取工艺路线工位关联失败");
                return Failure("获取工位关联失败");
            }
        }

        [HttpPost]
        public IActionResult Save([FromBody] RouteProcessStepUpdateRequest request)
        {
            if (request == null || request.RouteId <= 0)
            {
                return Failure("工艺路线 Id 不可为空");
            }

            try
            {
                var relations = (request.ProcessStepIds ?? new List<int>())
                    .Select(stepId => new Base_Route_ProcessStep_Config
                    {
                        RouteId = request.RouteId,
                        ProcessStepId = stepId
                    }).ToList();

                _routeStepService.UpdateRouteProcessStep(request.RouteId, relations);
                return Success(null, "保存工艺路线工位关系成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "保存工艺路线工位关系失败");
                return Failure("保存工艺路线工位关系失败");
            }
        }
    }
}

