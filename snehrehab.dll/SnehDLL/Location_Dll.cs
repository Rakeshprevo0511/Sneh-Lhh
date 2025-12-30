using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnehDLL
{
    public class Location_Dll
    {
        public int location_id { get; set; }
        public string name { get; set; }
        public int location_type { get; set; }
        public int parent_id { get; set; }
        public int is_visible { get; set; }
    }
}
