using System;
using System.Collections.Generic;
using System.Linq;
using GetStartedApp.SqlSugar.IServices;
using GetStartedApp.SqlSugar.Tables;
using Microsoft.AspNetCore.Mvc;

namespace GetStartedApp.WebApi.Controllers
{
    [ApiController]
    [Route("api/version-attribute")]
    [Tags("版本属性配置")]
    public class VersionAttributeController : ApiControllerBase
    {
        private readonly ILogger<VersionAttributeController> _logger;
        private readonly IBase_Version_Attribute_Config_Service _attributeService;

        public VersionAttributeController(
            ILogger<VersionAttributeController> logger,
            IBase_Version_Attribute_Config_Service attributeService)
        {
            _logger = logger;
            _attributeService = attributeService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var list = _attributeService.GetAll();
                return Success(list, "获取属性列表成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取属性列表失败");
                return Failure("获取属性列表失败");
            }
        }

        [HttpGet("paged")]
        public IActionResult GetPaged([FromQuery] List<int> secondIds, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 50)
        {
            if (secondIds == null || secondIds.Count == 0)
            {
                return Failure("二级版本 Id 列表不能为空");
            }

            try
            {
                int total = 0;
                var items = _attributeService.GetPageAttributeBySecondIds(secondIds, ref total, pageIndex, pageSize);
                return Success(new { total, items }, "获取属性分页数据成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "分页获取属性失败");
                return Failure("分页获取属性失败");
            }
        }

        [HttpGet("second/{secondId:int}")]
        public IActionResult GetBySecond(int secondId)
        {
            try
            {
                var list = _attributeService.GetAttributeBySecondId(secondId);
                return Success(list, "获取属性成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "根据二级版本获取属性失败");
                return Failure("获取属性失败");
            }
        }

        [HttpGet("recipe/{recipeNo:int}")]
        public IActionResult GetByRecipe(int recipeNo)
        {
            try
            {
                var list = _attributeService.GetAttributeByRecipeNo(recipeNo);
                return Success(list, "根据配方号获取属性成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "根据配方号获取属性失败");
                return Failure("获取属性失败");
            }
        }

        [HttpGet("exist")]
        public IActionResult CheckExist([FromQuery] string code, [FromQuery] string name, [FromQuery] int id = 0)
        {
            if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(name))
            {
                return Failure("编码和名称不能为空");
            }

            try
            {
                var exists = _attributeService.IsExist(code, name, id);
                return Success(new { exists }, "校验成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "校验属性唯一性失败");
                return Failure("校验失败");
            }
        }

        [HttpPost("bulk")]
        public IActionResult AddAttributes([FromBody] List<Base_Version_Attribute_Config> attributes)
        {
            if (attributes == null || attributes.Count == 0)
            {
                return Failure("属性列表不能为空");
            }

            try
            {
                var rows = _attributeService.AddAttributes(attributes);
                return Success(new { rows }, "批量新增属性成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "批量新增属性失败");
                return Failure("批量新增属性失败");
            }
        }

        [HttpPost]
        public IActionResult Save([FromBody] Base_Version_Attribute_Config entity)
        {
            if (entity == null)
            {
                return Failure("保存内容不能为空");
            }

            try
            {
                var id = _attributeService.InsertOrUpdate(entity);
                var target = _attributeService.GetAll().FirstOrDefault(x => x.Id == id);
                return Success(target, "保存属性成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "保存属性失败");
                return Failure("保存属性失败");
            }
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id, [FromQuery] bool utterly = false)
        {
            try
            {
                _attributeService.DeletedById(id, utterly);
                return Success(null, "删除属性成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "删除属性失败");
                return Failure("删除属性失败");
            }
        }
    }
}

