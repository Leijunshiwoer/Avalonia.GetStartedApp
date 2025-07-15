using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.Models

{
    public class RecipeSTDto : BaseDto
    {
        public string Name { get; set; }
        public int? RecipeConfigId { get; set; }
        public List<RecipeSTParameterDto> Parameters { get; set; }
    }
}
