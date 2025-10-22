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

    public class public_r
    {
        public int[] stationStatus { get; set; } = new int[16];
        public int[] eventTrigger { get; set; } = new int[64];
        public int[] sequnceRId { get; set; } = new int[64];
        public int[] sequnceWId { get; set; } = new int[64];
        public bool[] warnings { get; set; } = new bool[1024];
    }

    public class EventWClass
    {
        public int SequenceId { get; set; }
        public int ReultCode { get; set; }
        public int E01 { get; set; }
        public int E02 { get; set; }
        public int E03 { get; set; }
        public int E04 { get; set; }
        public int E05 { get; set; }
        public int E06 { get; set; }
        public int E07 { get; set; }
        public int E08 { get; set; }
        public int E09 { get; set; }
        public int E10 { get; set; }
        public int E11 { get; set; }
        public int E12 { get; set; }
        public int E13 { get; set; }
        public int E14 { get; set; }
        public int E15 { get; set; }
        public int E16 { get; set; }
        public int E17 { get; set; }
        public int E18 { get; set; }
        public int E19 { get; set; }
        public int E20 { get; set; }
        public int E21 { get; set; }
        public int E22 { get; set; }
        public int E23 { get; set; }
        public int E24 { get; set; }
        public int E25 { get; set; }
        public int E26 { get; set; }
        public int E27 { get; set; }
        public int E28 { get; set; }
        public int E29 { get; set; }
        public int E30 { get; set; }
        public int E31 { get; set; }
        public int E32 { get; set; }
        public int E33 { get; set; }
        public int E34 { get; set; }
        public int E35 { get; set; }
        public int E36 { get; set; }
        public int E37 { get; set; }
        public int E38 { get; set; }
        public int E39 { get; set; }
        public int E40 { get; set; }
    }


    public class EventRClass
    {
        public bool SequenceId { get; set; }
        public float E01 { get; set; }
        public float E02 { get; set; }
        public float E03 { get; set; }
        public float E04 { get; set; }
        public float E05 { get; set; }
        public float E06 { get; set; }
        public float E07 { get; set; }
        public float E08 { get; set; }
        public float E09 { get; set; }
        public float E10 { get; set; }
        public float E11 { get; set; }
        public float E12 { get; set; }
        public float E13 { get; set; }
        public float E14 { get; set; }
        public float E15 { get; set; }
        public float E16 { get; set; }
        public float E17 { get; set; }
        public float E18 { get; set; }
        public float E19 { get; set; }
        public float E20 { get; set; }
        public float E21 { get; set; }
        public float E22 { get; set; }
        public float E23 { get; set; }
        public float E24 { get; set; }
        public float E25 { get; set; }
        public float E26 { get; set; }
        public float E27 { get; set; }
        public float E28 { get; set; }
        public float E29 { get; set; }
        public float E30 { get; set; }
        public float E31 { get; set; }
        public float E32 { get; set; }
        public float E33 { get; set; }
        public float E34 { get; set; }
        public float E35 { get; set; }
        public float E36 { get; set; }
        public float E37 { get; set; }
        public float E38 { get; set; }
        public float E39 { get; set; }
        public float E40 { get; set; }
    }
}
