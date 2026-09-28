using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Linq;

namespace arc.domain.Configuration.QueryConfig
{
    public class QueryJoin
    {
        public string Table { get; set; }
        public List<QueryField> Fields { get; set; }
        public string ListItems { get; set; }
        public string Type { get; set; }
        public string On { get; set; }
        public string From { get; set; }
        public string ConditionOn { get; set; }
        public string CompareField { get; set; }
        public string LastUsedAlias { get; set; }

        internal string GetJoinSyntax(string alias, QueryFilterConfig filters)
        {
            var onClause = "s." + Table + "Id";
            if (! string.IsNullOrEmpty(On))
            {
                onClause = On.Contains(".") ? On : "s." + On;
            }

            var fromClause = alias + ".Id";
            if (!string.IsNullOrEmpty(From))
            {
                fromClause = alias + "." + From;
            }

            var whereClause = "";
            if (CompareField != null)
            {
                var affectedFilter = filters.Parameters.Where((f) => f.Key.ToLower() == ConditionOn.ToLower()).First();
                whereClause = " and " + CompareField + " in (" + affectedFilter.Value + ")";
            }

            if (Type != null && Type.ToLower() == "left")
            {
                return "left outer join " + Table + " " + alias + " on " + fromClause + " = " + onClause + whereClause;
            }
            return "inner join " + Table + " " + alias + " on " + fromClause + " = " + onClause + whereClause;
        }

        internal string GetFieldList(string alias)
        {
            LastUsedAlias = alias;
            var returnSql = "";
            if (Fields != null)
            {
                foreach (var field in Fields)
                {
                    var name = field.Name.Trim();
                    var fieldName = field.FromMoreData
                        ? $"COALESCE({alias}.moredata::jsonb->>'{name}', {alias}.moredata::jsonb->>'{name.ToLower()}')"
                        : alias + "." + name;

                    var knownAs = string.IsNullOrEmpty(field.KnownAs) ? name : field.KnownAs.Trim();
                    if (field.FromMoreData || !string.IsNullOrEmpty(field.KnownAs))
                    {
                        fieldName += " As " + knownAs;
                    }
                    if (!string.IsNullOrWhiteSpace(fieldName))
                    {
                        returnSql += returnSql.Length > 0 ? ", " + fieldName : fieldName;
                    }
                }
            }

            return returnSql;
        }
    }
}

