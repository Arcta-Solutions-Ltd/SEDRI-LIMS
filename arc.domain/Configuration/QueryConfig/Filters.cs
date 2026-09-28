using arc.common.Models;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;

namespace arc.domain.Configuration.QueryConfig
{
    internal class Filters
    {
        internal List<QueryValuesConfig> Parameters { get; set; }
        internal List<QueryWhereFieldConfig> Where { get; set; }
        internal string OrderBy { get; set; }
        internal bool Descending { get; set; }

        internal Filters(QueryFilterConfig filter, List<QueryWhereFieldConfig> where)
        {
            if (filter != null)
            {
                Parameters = filter.Parameters ?? new List<QueryValuesConfig>();
                OrderBy = filter.OrderBy;
                Descending = filter.OrderDescending;
                Where = where ?? new List<QueryWhereFieldConfig>();
            }
        }

        internal string GetSqlOrderString(string defaultOrderBy, bool defaultDescending)
        {
            if (string.IsNullOrWhiteSpace(OrderBy))
            {
                OrderBy = defaultOrderBy;
                Descending = defaultDescending;
            }
            var orderBy = !(string.IsNullOrEmpty(OrderBy)) ? " order by " + OrderBy : "";
            var descending = Descending ? " desc" : "";

            return orderBy + descending;
        }

        internal void AddTokenFilter(string tableName, TokenInfoModel token)
        {
            Parameters ??= new List<QueryValuesConfig>();
            Where ??= new List<QueryWhereFieldConfig>();

            if (tableName.ToLower() == "specimen")
            {
                if (! string.IsNullOrEmpty(token.LaboratoryId) && token.LaboratoryId != "0")
                {
                    Parameters.Add(new QueryValuesConfig { Key = "LaboratoryId", Value = token.LaboratoryId });
                    Where.Add(new QueryWhereFieldConfig { Comparison = "=", Field = "LaboratoryId" });
                }
                if (!string.IsNullOrEmpty(token.OrganisationId) && token.OrganisationId != "0")
                {
                    Parameters.Add(new QueryValuesConfig { Key = "OrganisationId", Value = token.OrganisationId });
                    Where.Add(new QueryWhereFieldConfig { Comparison = "in", Field = "OrganisationId" });
                }
            }

            var subQueryQueryName = "";
            var parameterKey = "";
            var queryWhereField = "";
            var comparison = "childexists";

            switch (tableName.ToLower())
            {
                case "patient":
                    subQueryQueryName = "PatientId";
                    parameterKey = "Id";
                    queryWhereField = "Id";
                    break;
                case "queue":
                    comparison = "childexistsornull";
                    subQueryQueryName = "Id";
                    parameterKey = "SpecimenId";
                    queryWhereField = "SpecimenId";
                    break;
                case "specimencomment":
                    subQueryQueryName = "Id";
                    parameterKey = "SpecimenId";
                    queryWhereField = "SpecimenId";
                    break;
                case "patientcomment":
                    subQueryQueryName = "PatientId";
                    parameterKey = "Id";
                    queryWhereField = "PatientId";
                    break;
            }

            if (!string.IsNullOrEmpty(subQueryQueryName) && !string.IsNullOrEmpty(parameterKey) && !string.IsNullOrEmpty(queryWhereField))
            {
                var subQuery = new QueryConfig { TableName = "Specimen", Type = "Select", Fields = new List<QueryField> { new QueryField { Name = subQueryQueryName } } };
                Parameters.Add(new QueryValuesConfig { Key = parameterKey, Value = "dynamic" });
                Where.Add(new QueryWhereFieldConfig { Comparison = comparison, Field = queryWhereField, SubQuery = subQuery });
            }
        }
    }
}
