using arc.common.ExtensionMethods;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace arc.data.Common;
/// <summary>
/// Provides methods to build SQL WHERE clauses and corresponding dynamic parameters.
/// </summary>
internal static class ParameterBuilder
{
    /// <summary>
    /// Builds a SQL WHERE clause and dynamic parameters based on the given query filters and parameter definitions.
    /// </summary>
    /// <param name="queryFilters">The configuration for query filters, containing parameters to filter by.</param>
    /// <param name="parameterDefinitions">The definitions of query parameters and their SQL expressions.</param>
    /// <returns>
    /// A tuple containing the constructed WHERE clause as a string and the associated <see cref="DynamicParameters"/>.
    /// </returns>
    internal static (string whereClause, DynamicParameters parameters) BuildWhereClause(
        QueryFilterConfig queryFilters,
        List<ParameterInfo> parameterDefinitions)
    {
        var whereClauses = new List<string>();
        var parameters = new DynamicParameters();

        foreach (var queryFilter in queryFilters.Parameters)
        {
            if (string.IsNullOrEmpty(queryFilter.Value)) continue;

            var parameterDefinition = parameterDefinitions.FirstOrDefault(
                p => string.Equals(p.Key, queryFilter.Key, StringComparison.OrdinalIgnoreCase));

            if (parameterDefinition == null)
                continue;

            int? parsedInt = null;
            if (string.Equals(parameterDefinition.Type, "int", StringComparison.OrdinalIgnoreCase))
            {
                if (!int.TryParse(queryFilter.Value.Trim(), out var parsed))
                    continue;
                parsedInt = parsed;
            }

            parameterDefinition.SqlExpression = string.Equals(parameterDefinition.Type, "list", StringComparison.OrdinalIgnoreCase) && queryFilter.Value.ContainsOnlyListOfNumbers()
                ? parameterDefinition.SqlExpression.ToLower().Replace($"@{parameterDefinition.Key.ToLower()}", queryFilter.Value)
                : parameterDefinition.SqlExpression;

            whereClauses.Add(parameterDefinition.SqlExpression);

            if (!string.Equals(parameterDefinition.Type, "list", StringComparison.OrdinalIgnoreCase))
            {
                if (string.Equals(parameterDefinition.Type, "date", StringComparison.OrdinalIgnoreCase))
                {
                    parameters.Add($"@{parameterDefinition.Key}", DateTime.Parse(queryFilter.Value));
                }
                else if (string.Equals(parameterDefinition.Type, "int", StringComparison.OrdinalIgnoreCase))
                {
                    parameters.Add($"@{parameterDefinition.Key}", parsedInt!.Value);
                }
                else
                {
                    parameters.Add($"@{parameterDefinition.Key}", queryFilter.Value);
                }
            }
        }

        var whereClause = whereClauses.Any()
            ? " WHERE " + string.Join(" AND ", whereClauses)
            : string.Empty;

        return (whereClause, parameters);
    }
}

/// <summary>
/// Represents information about a query parameter, including its key, SQL expression, and whether it accepts a list of values.
/// </summary>
public class ParameterInfo
{
    /// <summary>
    /// Gets or sets the key identifying the query parameter.
    /// </summary>
    internal string Key { get; set; }

    /// <summary>
    /// Gets or sets the SQL expression associated with the query parameter.
    /// </summary>
    internal string SqlExpression { get; set; }

    /// <summary>
    /// Gets or sets a value indicating the type of the parameter.
    /// </summary>
    internal string Type { get; set; } = "string";
}

