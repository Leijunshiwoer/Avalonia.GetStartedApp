using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.Models
{
    public class RecipeDto : BaseDto
    {
        public int RecipeNo { get; set; }
        public string Name { get; set; }
        public int SecondId { get; set; }
        public List<RecipeMaterialDto> Materials { get; set; }
        public List<RecipeSTDto> STs { get; set; }
    }
}
