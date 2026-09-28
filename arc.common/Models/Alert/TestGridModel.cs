using System;
using System.Collections.Generic;
using System.Text;

namespace arc.common.Models.Alert
{
    public class TestGridModel
    {
        public string Test { get; set; }
        public string Field { get; set; }
        public string Comparison { get; set; }
        public string ListValue { get; set; }
        public string NumberValue { get; set; }
        public string StringValue { get; set; }
        public bool Matched { get; set; }
    }
}
