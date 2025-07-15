using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.Models
{
    public class RecipeSTParameterDto : BaseDto
    {
        public string Name { get; set; }
        public float Value { get; set; }
        public int? RecipeSTId { get; set; }
        public List<string> Items { get; set; } =  new List<string>();
    }
}
