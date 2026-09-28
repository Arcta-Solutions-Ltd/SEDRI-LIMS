using arc.common.Data;
using arc.common.Utils;
using System.Collections.Generic;

namespace arc.domain.StatementBuilder
{
    internal class SqlJoin
    {
        internal string Prefix {  get; set; }
        internal string Table { get; set; }
        internal string LinkPrefix { get; set; }
        internal string LinkField { get; set; } 
        internal string LinkTable { get; set; }
        internal string TestField { get; set; }
        internal string ListPrefix { get; set; }
        internal bool IsTest { get; set; }
        internal string PrefixField { get; set; } = "id";
        internal List<string> JoinClauses { get; set; } = null;
        internal bool IsCustom { get; set; }

        public string GetJoinSyntax()
        {
            var jsonConverter = new JsonWholeStructureFieldsCollector();
            var jsonRemover = new JsonElementRemover();
            var moreData = new GenerateMoreData(jsonConverter, jsonRemover);
            var tableFields = moreData.GetFieldList(LinkTable);

            var sql = "left outer join " + Table + " " + Prefix + " on " + Prefix + "." + PrefixField + " = " + LinkPrefix + "." + LinkField;
            if (tableFields.Count > 0 && !tableFields.Contains(LinkField.ToLower()) && !IsCustom)
            {
                sql = "left outer join " + Table + " " + Prefix + " on " + Prefix + "." + PrefixField + " = cast(" + LinkPrefix + ".moredata::jsonb->>'" + LinkField + "' As Int)";
            }
            if (JoinClauses != null) {
                foreach (var clause in JoinClauses)
                {
                    sql += " and " + Prefix + "." + clause;
                }
            }
            return sql;
        }

        public string GetTestJoinSyntax()
        {

            var sql = "left outer join " + Table + " " + Prefix + " on " + Prefix + "." + PrefixField + " = " + LinkPrefix + "." + LinkField + " and lower(" + Prefix + ".key) = '" + TestField.ToLower() + "' and " + Prefix + ".value != ''";
            if (! string.IsNullOrEmpty(ListPrefix))
            {
                sql += " left outer join listitem " + ListPrefix + " on " + ListPrefix + ".id = " + Prefix + ".Value::integer and " + Prefix + ".value ~ '^[0-9]+$'";
            }
            if (JoinClauses != null)
            {
                foreach (var clause in JoinClauses)
                {
                    sql += " and " + Prefix + "." + clause;
                }
            }
            return sql;
        }
    }
}

