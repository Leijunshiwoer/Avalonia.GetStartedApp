using System;
using System.Collections.Generic;
using GetStartedApp.SqlSugar.IServices;
using GetStartedApp.SqlSugar.Tables;
using Microsoft.AspNetCore.Mvc;

namespace GetStartedApp.WebApi.Controllers
{
    [ApiController]
    [Route("api/process-steps")]
    [Tags("工位配置")]
    public class ProcessStepConfigController : ApiControllerBase
    {
        private readonly ILogger<ProcessStepConfigController> _logger;
        private readonly IBase_Process_Step_Config_Service _stepService;

        public ProcessStepConfigController(
            ILogger<ProcessStepConfigController> logger,
            IBase_Process_Step_Config_Service stepService)
        {
            _logger = logger;
            _stepService = stepService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var list = _stepService.GetAll();
                return Success(list, "获取工位成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取工位列表失败");
                return Failure("获取工位列表失败");
            }
        }

        [HttpGet("paged")]
        public IActionResult GetPaged([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 50)
        {
            try
            {
                long total = 0;
                var items = _stepService.GetProcessStepPage(ref total, pageIndex, pageSize);
                return Success(new { total, items }, "获取工位分页数据成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "分页获取工位失败");
                return Failure("分页获取工位失败");
            }
        }

        [HttpGet("process/{processId:int}")]
        public IActionResult GetByProcess(int processId)
        {
            try
            {
                var list = _stepService.GetProcessStepsById(processId);
                return Success(list, "获取工序下的工位成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "根据工序获取工位失败");
                return Failure("获取工位失败");
            }
        }

        [HttpGet("in-plc")]
        public IActionResult GetInPlc()
        {
            try
            {
                var list = _stepService.GetProcessStepsByInPLC();
                return Success(list, "获取上报 PLC 的工位成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取 PLC 工位失败");
                return Failure("获取工位失败");
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
                var exists = _stepService.IsExist(code, name, id);
                return Success(new { exists }, "校验成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "校验工位唯一性失败");
                return Failure("校验失败");
            }
        }

        [HttpPost]
        public IActionResult Save([FromBody] Base_Process_Step_Config step)
        {
            if (step == null)
            {
                return Failure("保存内容不能为空");
            }

            try
            {
                var id = _stepService.InsertOrUpdate(step);
                return Success(id, "保存工位成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "保存工位失败");
                return Failure("保存工位失败");
            }
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _stepService.DeletedProcessStepById(id);
                return Success(null, "删除工位成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "删除工位失败");
                return Failure("删除工位失败");
            }
        }
    }
}

