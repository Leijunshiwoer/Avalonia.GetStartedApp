using GetStartedApp.SqlSugar.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.SqlSugar.IServices
{
    public interface IProduct_Recipe_Service
    {
        int InsertReturnIdentityRecipe(Product_Recipe_Config recipe_Config);
        int InsertReturnIdentityRecipeMaterial(Product_Recipe_Material_Config recipe_Material_Config);
        int InsertReturnIdentityRecipeST(Product_Recipe_ST_Config recipe_ST_Config);
        int InsertReturnIdentityRecipeSTParameter(Product_Recipe_ST_Parameter_Config recipe_ST_Parameter_Config);
        void InitRecipe(int count, int SecondId);
        Product_Recipe_ST_Parameter_Config GetSTParameter(int op, int st, int count);
        Product_Recipe_ST_Config GetSTs(int op, int count);
        List<Product_Recipe_Config> GetRecipeBySecondId(int secondId);
        void UpdateRecipes(List<Product_Recipe_Config> recipes);
        List<Product_Recipe_Config> GetRecipeByRecipeNo(int recipeNo);
    }
}
