using arc.domain.Configuration.QueryFiltersConfig;

namespace arc.domain.Configuration.QueryConfig
{
    public class QueryWhereFieldConfig
    {
        public string Field { get; set; }
        public string Comparison { get; set; }
        public string FieldToMatch { get; set; }
        public string OrGroup { get; set; }
        public string Units { get; set; }
        public string Value { get; set; }
        public QueryConfig SubQuery { get; set; }
        public QueryFilterConfig Filters { get; set; }
    }
}
