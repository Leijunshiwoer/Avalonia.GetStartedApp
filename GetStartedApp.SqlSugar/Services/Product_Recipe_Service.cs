using GetStartedApp.SqlSugar.IServices;
using GetStartedApp.SqlSugar.Repositories;
using GetStartedApp.SqlSugar.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.SqlSugar.Services
{
    public class Product_Recipe_Service : IProduct_Recipe_Service
    {
        private readonly ISqlSugarRepository<Product_Recipe_Config> _recipeRep;
        private readonly ISqlSugarRepository<Product_Recipe_Material_Config> _recipeMaterialRep;
        private readonly ISqlSugarRepository<Product_Recipe_ST_Config> _recipeSTRep;
        private readonly ISqlSugarRepository<Product_Recipe_ST_Parameter_Config> _recipeSTParameterRep;
        private readonly IBase_Version_Attribute_Config_Service _attribute_Config_Service;

        public Product_Recipe_Service(
            ISqlSugarRepository<Product_Recipe_Config> recipeRep,
            ISqlSugarRepository<Product_Recipe_Material_Config> recipeMaterialRep,
            ISqlSugarRepository<Product_Recipe_ST_Config> recipeSTRep,
            ISqlSugarRepository<Product_Recipe_ST_Parameter_Config> recipeSTParameterRep,
            IBase_Version_Attribute_Config_Service attribute_Config_Service
            )
        {
            _recipeRep = recipeRep;
            _recipeMaterialRep = recipeMaterialRep;
            _recipeSTRep = recipeSTRep;
            _recipeSTParameterRep = recipeSTParameterRep;
            _attribute_Config_Service = attribute_Config_Service;
        }

        public int InsertReturnIdentityRecipe(Product_Recipe_Config recipe_Config)
        {
            return _recipeRep.InsertReturnIdentity(recipe_Config);
        }

        public int InsertReturnIdentityRecipeMaterial(Product_Recipe_Material_Config recipe_Material_Config)
        {
            return _recipeMaterialRep.InsertReturnIdentity(recipe_Material_Config);
        }

        public int InsertReturnIdentityRecipeST(Product_Recipe_ST_Config recipe_ST_Config)
        {
            return _recipeSTRep.InsertReturnIdentity(recipe_ST_Config);
        }

        public int InsertReturnIdentityRecipeSTParameter(Product_Recipe_ST_Parameter_Config recipe_ST_Parameter_Config)
        {
            return _recipeSTParameterRep.InsertReturnIdentity(recipe_ST_Parameter_Config);
        }

        /// <summary>
        /// 创建PLC配方
        /// </summary>
        /// <param name="count"></param>
        public void InitRecipe(int count, int SecondId)
        {
            for (int i = 0; i < count; i++)
            {
                //创建配方

                #region 第一个配方

                int recipeId = InsertReturnIdentityRecipe(new Product_Recipe_Config()
                {
                    RecipeNo = 1,
                    Name = "配方" + (i + 1).ToString().PadLeft(2, '0'),
                    SecondId = SecondId
                });
                //一个配方有10个物料
                for (int n = 0; n < 10; n++)
                {
                    InsertReturnIdentityRecipeMaterial(new Product_Recipe_Material_Config()
                    {
                        Name = "物料名称" + (n + 1).ToString().PadLeft(2, '0'),
                        //Value = "参数值" + (n + 1).ToString().PadLeft(2, '0'),
                        RecipeConfigId = recipeId
                    });
                }
                //一个配方20个ST
                for (int n = 0; n < 20; n++)
                {
                    var STs = GetSTs(i, n + 1);
                    if (STs != null)
                    {
                        int stId = InsertReturnIdentityRecipeST(new Product_Recipe_ST_Config()
                        {
                            Name = STs.Name,
                            Remark = STs.Remark,
                            RecipeConfigId = recipeId
                        });
                        for (int m = 0; m < 10; m++)
                        {
                            var result = GetSTParameter(i, n, m + 1);
                            if (result != null)
                            {
                                InsertReturnIdentityRecipeSTParameter(new Product_Recipe_ST_Parameter_Config
                                {
                                    Name = result.Name,
                                    Value = result.Value,
                                    RecipeSTId = stId,
                                    Remark = result.Remark
                                });
                            }
                            else
                            {
                                InsertReturnIdentityRecipeSTParameter(new Product_Recipe_ST_Parameter_Config
                                {
                                    Name = "参数" + m,
                                    Value = 0f,
                                    RecipeSTId = stId
                                });
                            }
                        }
                    }
                    else
                    {
                        int stId = InsertReturnIdentityRecipeST(new Product_Recipe_ST_Config()
                        {
                            Name = "ST" + (n + 1).ToString().PadLeft(2, '0'),
                            RecipeConfigId = recipeId
                        });
                        for (int m = 0; m < 10; m++)
                        {
                            var result = GetSTParameter(i, n, m + 1);
                            if (result != null)
                            {
                                InsertReturnIdentityRecipeSTParameter(new Product_Recipe_ST_Parameter_Config
                                {
                                    Name = result.Name,
                                    Value = result.Value,
                                    RecipeSTId = stId
                                });
                            }
                            else
                            {
                                InsertReturnIdentityRecipeSTParameter(new Product_Recipe_ST_Parameter_Config
                                {
                                    Name = "参数" + m,
                                    Value = 0f,
                                    RecipeSTId = stId
                                });
                            }
                        }
                    }
                }

                #endregion 第一个配方
            }
        }

        public Product_Recipe_ST_Parameter_Config GetSTParameter(int op, int st, int count)
        {
            int id;
            if (op == 0)
            {
                id = st * 10 + count;
            }
            else if (op == 1)
            {
                id = 200 + st * 10 + count;
            }
            else
            {
                id = 400 + st * 10 + count;
            }

            return _recipeSTParameterRep.Single(id);
        }

        public Product_Recipe_ST_Config GetSTs(int op, int count)
        {
            int id;
            if (op == 0)
            {
                id = count;
            }
            else if (op == 1)
            {
                id = 20 + count;
            }
            else
            {
                id = 40 + count;
            }

            return _recipeSTRep.Single(id);
        }

        public List<Product_Recipe_Config> GetRecipeBySecondId(int secondId)
        {
            return _recipeRep.Context.Queryable<Product_Recipe_Config>()
                .Includes(x => x.Materials)
                .Includes(x => x.STs, y => y.Parameters)
                .Where(x => x.SecondId == secondId)
                .ToList();
        }

        /// <summary>
        /// 跟新配方并包含子项
        /// </summary>
        public void UpdateRecipes(List<Product_Recipe_Config> recipes)
        {
            //都跟新一遍
            //保存 Product_Recipe_Config
            foreach (var pr in recipes)
            {
                //保存 Product_Recipe_ST_Config
                _recipeSTRep.Update(pr.STs);
                foreach (var st in pr.STs)
                {
                    //跟新 Product_Recipe_ST_Parameter_Config
                    _recipeSTParameterRep.Update(st.Parameters);
                    foreach (var p in st.Parameters)
                    {
                    }
                }
            }
        }

        public List<Product_Recipe_Config> GetRecipeByRecipeNo(int recipeNo)
        {
            var at = _attribute_Config_Service.GetAttributeByRecipeNo(recipeNo).FirstOrDefault();
            if (at != null)
            {
                return GetRecipeBySecondId(at.SecondId);
            }
            return new List<Product_Recipe_Config>();
        }
    }
}
