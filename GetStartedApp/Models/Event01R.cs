using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.Models
{
    public class Event01R
    {
        public ushort event01R_sequnceId { get; set; }
        public float event01R_data01 { get; set; }
        public int[] event01R_data02 { get; set; }

    }

    public class Event01W
    {
        public ushort event01W_sequnceId { get; set; }
       

    }

}
