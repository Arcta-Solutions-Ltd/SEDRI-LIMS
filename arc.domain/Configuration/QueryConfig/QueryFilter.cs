using System.Collections.Generic;

namespace arc.domain.Configuration.QueryConfig
{
    public class QueryFilter
    {
        public string Field { get; set; }
        public string Comparison { get; set; }
        public List<string> Values { get; set; }
        public string GroupType { get; set; } = "and";

        public string GetFilterString()
        {
            var sql = "";
            foreach(var value in Values)
            {
                var newComp = "s." + Field + " " + Comparison + " " + value;
                sql = sql == "" ? newComp : sql + " " + GroupType + " " + newComp;
            }

            if (sql == "")
            {
                return "";
            } else
            {
                return "(" + sql + ")";
            }
        }
    }
}
