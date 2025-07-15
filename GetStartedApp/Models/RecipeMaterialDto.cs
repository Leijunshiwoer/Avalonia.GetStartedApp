using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.Models
{
    public class RecipeMaterialDto : BaseDto
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public int? RecipeConfigId { get; set; }
    }
}
