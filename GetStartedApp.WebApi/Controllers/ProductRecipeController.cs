using System;
using System.Collections.Generic;
using GetStartedApp.SqlSugar.IServices;
using GetStartedApp.SqlSugar.Tables;
using GetStartedApp.WebApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace GetStartedApp.WebApi.Controllers
{
    [ApiController]
    [Route("api/product-recipes")]
    [Tags("产品配方管理")]
    public class ProductRecipeController : ApiControllerBase
    {
        private readonly ILogger<ProductRecipeController> _logger;
        private readonly IProduct_Recipe_Service _recipeService;

        public ProductRecipeController(
            ILogger<ProductRecipeController> logger,
            IProduct_Recipe_Service recipeService)
        {
            _logger = logger;
            _recipeService = recipeService;
        }

        [HttpPost]
        public IActionResult CreateRecipe([FromBody] Product_Recipe_Config recipe)
        {
            if (recipe == null)
            {
                return Failure("配方内容不能为空");
            }

            try
            {
                var id = _recipeService.InsertReturnIdentityRecipe(recipe);
                return Success(id, "新增配方成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "新增配方失败");
                return Failure("新增配方失败");
            }
        }

        [HttpPost("materials")]
        public IActionResult CreateMaterial([FromBody] Product_Recipe_Material_Config material)
        {
            if (material == null)
            {
                return Failure("配方物料不能为空");
            }

            try
            {
                var id = _recipeService.InsertReturnIdentityRecipeMaterial(material);
                return Success(id, "新增配方物料成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "新增配方物料失败");
                return Failure("新增配方物料失败");
            }
        }

        [HttpPost("sts")]
        public IActionResult CreateST([FromBody] Product_Recipe_ST_Config st)
        {
            if (st == null)
            {
                return Failure("配方 ST 不能为空");
            }

            try
            {
                var id = _recipeService.InsertReturnIdentityRecipeST(st);
                return Success(id, "新增配方 ST 成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "新增配方 ST 失败");
                return Failure("新增配方 ST 失败");
            }
        }

        [HttpPost("st-parameters")]
        public IActionResult CreateSTParameter([FromBody] Product_Recipe_ST_Parameter_Config parameter)
        {
            if (parameter == null)
            {
                return Failure("配方 ST 参数不能为空");
            }

            try
            {
                var id = _recipeService.InsertReturnIdentityRecipeSTParameter(parameter);
                return Success(id, "新增配方 ST 参数成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "新增配方 ST 参数失败");
                return Failure("新增配方 ST 参数失败");
            }
        }

        [HttpPost("init")]
        public IActionResult InitRecipes([FromBody] RecipeInitRequest request)
        {
            if (request == null || request.SecondId <= 0)
            {
                return Failure("二级版本 Id 不能为空");
            }

            try
            {
                _recipeService.InitRecipe(Math.Max(1, request.Count), request.SecondId);
                return Success(null, "初始化配方成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "初始化配方失败");
                return Failure("初始化配方失败");
            }
        }

        [HttpGet("second/{secondId:int}")]
        public IActionResult GetBySecond(int secondId)
        {
            try
            {
                var list = _recipeService.GetRecipeBySecondId(secondId);
                return Success(list, "获取二级版本配方成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "根据二级版本获取配方失败");
                return Failure("获取配方失败");
            }
        }

        [HttpGet("recipe-no/{recipeNo:int}")]
        public IActionResult GetByRecipeNo(int recipeNo)
        {
            try
            {
                var list = _recipeService.GetRecipeByRecipeNo(recipeNo);
                return Success(list, "根据配方号获取配方成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "根据配方号获取配方失败");
                return Failure("获取配方失败");
            }
        }

        [HttpGet("st-parameters")]
        public IActionResult GetSTParameter([FromQuery] int op, [FromQuery] int st, [FromQuery] int count)
        {
            try
            {
                var parameter = _recipeService.GetSTParameter(op, st, count);
                return parameter == null ? Failure("未找到对应参数") : Success(parameter, "获取配方 ST 参数成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取配方 ST 参数失败");
                return Failure("获取配方 ST 参数失败");
            }
        }

        [HttpGet("sts")]
        public IActionResult GetSTs([FromQuery] int op, [FromQuery] int count)
        {
            try
            {
                var st = _recipeService.GetSTs(op, count);
                return st == null ? Failure("未找到对应 ST") : Success(st, "获取配方 ST 成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取配方 ST 失败");
                return Failure("获取配方 ST 失败");
            }
        }

        [HttpPut]
        public IActionResult UpdateRecipes([FromBody] List<Product_Recipe_Config> recipes)
        {
            if (recipes == null || recipes.Count == 0)
            {
                return Failure("配方列表不能为空");
            }

            try
            {
                _recipeService.UpdateRecipes(recipes);
                return Success(null, "更新配方成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新配方失败");
                return Failure("更新配方失败");
            }
        }
    }
}

