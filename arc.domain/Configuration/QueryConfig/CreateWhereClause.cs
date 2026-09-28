using arc.common.Models;
using arc.common.Utils;
using arc.domain.Configuration.QueryConfig.WhereClauses;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Linq;

namespace arc.domain.Configuration.QueryConfig
{
    /// <summary>
    /// Represents the CreateWhereClause class that builds a WHERE clause for SQL queries.
    /// </summary>
    internal class CreateWhereClause
    {
        private readonly TokenInfoModel _token;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateWhereClause"/> class.
        /// </summary>
        /// <param name="token">The token information model.</param>
        public CreateWhereClause(TokenInfoModel token)
        {
            _token = token;
        }

        /// <summary>
        /// Creates a WHERE clause based on the provided parameters and field list.
        /// </summary>
        /// <param name="parameters">The list of query value configurations.</param>
        /// <param name="fieldListSql">The SQL field list as a string.</param>
        /// <param name="where">The list of query where field configurations.</param>
        /// <param name="tableFields">The list of table fields.</param>
        /// <returns>A string representing the WHERE clause.</returns>
        internal string Create(List<QueryValuesConfig> parameters, string fieldListSql, List<QueryWhereFieldConfig> where, string tableName, List<KeyValuePair<string, List<string>>> tableFields, 
            List<QueryJoin> joins, List<QueryJoin> listJoins)
        {
            var fieldList = "";
            var orGroup = new OrGroup();

            parameters = AddToListOfParametersForQueriesWithAValue(parameters, where);

            foreach (var clause in where)
            {
                string alias = "s";
                var fieldInstances = fieldListSql.AllMatchingEntriesInFieldList(clause.Field);

                if (fieldInstances.Count() > 0)
                {
                    foreach (var instance in fieldInstances)
                    {
                        alias = instance.Key;

                        var value = parameters.Where(v => v.Key.ToLower() == clause.Field.ToLower());
                        if (value.Count() > 0 && !string.IsNullOrEmpty(value.First().Value))
                        {
                            var newWhere = GetWhereClauseForField(clause, value, alias, tableName, tableFields, joins, listJoins);

                            if (clause.OrGroup != null && clause.OrGroup != "")
                            {
                                orGroup.Add(clause.OrGroup, newWhere);
                            }
                            else
                            {
                                fieldList += fieldList == "" ? newWhere : " and " + newWhere;
                            }
                        }
                    }
                }
                else
                {
                    alias = "s";

                    var value = clause.Comparison?.ToLower() == "agerange"
                        ? parameters.Where(v => IsAgeRangeParam(v.Key))
                        : clause.Comparison?.ToLower() == "daterange"
                        ? parameters.Where(v => IsDateRangeParam(v.Key))
                        : parameters.Where(v => v.Key.ToLower() == clause.Field.ToLower());
                    var valueList = value.ToList();
                    var hasValue = clause.Comparison?.ToLower() == "agerange"
                        ? valueList.Any(v => !string.IsNullOrEmpty(v.Value))
                        : clause.Comparison?.ToLower() == "daterange"
                        ? valueList.Any(v => !string.IsNullOrEmpty(v.Value))
                        : valueList.Count > 0 && !string.IsNullOrEmpty(valueList.First().Value) && valueList.First().Value != "null";
                    if (hasValue)
                    {
                        var paramsToPass = (clause.Comparison?.ToLower() == "agerange" || clause.Comparison?.ToLower() == "daterange") ? parameters : valueList;
                        var newWhere = GetWhereClauseForField(clause, paramsToPass, alias, tableName, tableFields, joins, listJoins);

                        if (clause.OrGroup != null && clause.OrGroup != "")
                        {
                            orGroup.Add(clause.OrGroup, newWhere);
                        }
                        else
                        {
                            fieldList += fieldList == "" ? newWhere : " and " + newWhere;
                        }
                    }
                }
            }

            var orGroups = orGroup.GetString();
            var combinedString = fieldList != "" ? fieldList + (orGroups == "" ? "" : " and " + orGroups) : orGroups;

            return combinedString == "" ? "" : "where " + combinedString;
        }

        private static bool IsAgeRangeParam(string key)
        {
            var k = key?.ToLower() ?? "";
            return k == "agefromyears" || k == "agefrommonths" || k == "agefromdays"
                || k == "agetoyears" || k == "agetomonths" || k == "agetodays";
        }

        private static bool IsDateRangeParam(string key)
        {
            var k = key?.ToLower() ?? "";
            return k == "startdate" || k == "enddate";
        }

        /// <summary>
        /// Creates a WHERE clause for a specific field based on the clause configuration.
        /// </summary>
        /// <param name="clause">The query where field configuration.</param>
        /// <param name="value">The query values configuration.</param>
        /// <param name="alias">The alias for the field.</param>
        /// <param name="tableName">The default table the query is searchin on.</param>
        /// <param name="tableFields">The list of fields arranged by table.</param>
        /// <param name="joins">The list of joins.</param>
        /// <returns>A string representing the WHERE clause for the specific field.</returns>
        private string GetWhereClauseForField(QueryWhereFieldConfig clause, IEnumerable<QueryValuesConfig> value, string alias ,string tableName, List<KeyValuePair<string, List<string>>> tableFields, 
            List<QueryJoin> joins, List<QueryJoin> listJoins)
        {
            var factory = new WhereClauseFactory(_token);
            var clauseProcessor = factory.Create(clause.Comparison);

            clause.FieldToMatch = string.IsNullOrEmpty(clause.FieldToMatch) ? clause.Field : clause.FieldToMatch;
            var fieldMatch = true;

            var joinTable = joins?.FirstOrDefault(p=>p.LastUsedAlias == alias);
            var fields = new List<string>();
            if (joinTable == null) 
            {
                var listJoinTable = listJoins?.FirstOrDefault(p => p.LastUsedAlias == alias);

                if (listJoinTable == null)
                {
                    fields = tableFields.FirstOrDefault(p => p.Key == tableName).Value;
                }
                else
                {
                    if (listJoinTable.Fields.Any(p=>p.Name == clause.FieldToMatch)) 
                    {
                        fields.Add("Value");
                    }
                    clause.FieldToMatch = "Value";
                }
            }
            else
            {
                fields = tableFields.FirstOrDefault(p => p.Key == joinTable.Table).Value;
            }
            fieldMatch = fields.Any(p=>p.ToLower() == clause.FieldToMatch.ToLower());

            if (fields.Count > 0 && fieldMatch == false)
            {
                clause.FieldToMatch = "moredata::jsonb->>'" + clause.FieldToMatch.Trim() + "'";
            }

            return clauseProcessor.Get(clause, value, alias);
        }

        /// <summary>
        /// Adds query values with non-empty values to the list of parameters.
        /// </summary>
        /// <param name="parameters">The list of query value configurations.</param>
        /// <param name="where">The list of query where field configurations.</param>
        /// <returns>The updated list of query value configurations.</returns>
        private List<QueryValuesConfig> AddToListOfParametersForQueriesWithAValue(List<QueryValuesConfig> parameters, List<QueryWhereFieldConfig> where)
        {
            foreach (var param in where.Where(p => !string.IsNullOrEmpty(p.Value)))
            {
                parameters.Add(new QueryValuesConfig { Key = param.Field, Value = param.Value });
            }
            return parameters;
        }

    }
}
