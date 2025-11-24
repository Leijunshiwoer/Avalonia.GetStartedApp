using System;
using GetStartedApp.SqlSugar.IServices;
using GetStartedApp.SqlSugar.Tables;
using Microsoft.AspNetCore.Mvc;

namespace GetStartedApp.WebApi.Controllers
{
    [ApiController]
    [Route("api/processes")]
    [Tags("工序配置")]
    public class ProcessConfigController : ApiControllerBase
    {
        private readonly ILogger<ProcessConfigController> _logger;
        private readonly IBase_Process_Config_Service _processService;

        public ProcessConfigController(
            ILogger<ProcessConfigController> logger,
            IBase_Process_Config_Service processService)
        {
            _logger = logger;
            _processService = processService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var list = _processService.GetProcesss();
                return Success(list, "获取工序成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取工序列表失败");
                return Failure("获取工序列表失败");
            }
        }

        [HttpGet("paged")]
        public IActionResult GetPaged([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 50)
        {
            try
            {
                long total = 0;
                var items = _processService.GetProcessPage(ref total, pageIndex, pageSize);
                return Success(new { total, items }, "获取工序分页数据成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "分页获取工序失败");
                return Failure("分页获取工序失败");
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
                var exists = _processService.IsExist(code, name, id);
                return Success(new { exists }, "校验成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "校验工序唯一性失败");
                return Failure("校验失败");
            }
        }

        [HttpPost]
        public IActionResult Save([FromBody] Base_Process_Config process)
        {
            if (process == null)
            {
                return Failure("保存内容不能为空");
            }

            try
            {
                var id = _processService.InsertOrUpdate(process);
                return Success(id, "保存工序成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "保存工序失败");
                return Failure("保存工序失败");
            }
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id, [FromQuery] bool utterly = false)
        {
            try
            {
                _processService.DeletedById(id, utterly);
                return Success(null, "删除工序成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "删除工序失败");
                return Failure("删除工序失败");
            }
        }
    }
}

