using arc.common.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace arc.domain.Configuration.QueryFiltersConfig;

/// <summary>
/// Represents the configuration for a query filter, including name, parameters, order, and sorting direction.
/// </summary>
public class QueryFilterConfig
{
    /// <summary>
    /// Gets or sets the name of the query filter.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the list of parameters for the query filter.
    /// </summary>
    public List<QueryValuesConfig> Parameters { get; set; } = [];

    /// <summary>
    /// Gets or sets the column by which to order the query results.
    /// </summary>
    public string OrderBy { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the query results should be ordered in descending order.
    /// </summary>
    public bool OrderDescending { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryFilterConfig"/> class with the specified arguments.
    /// </summary>
    /// <param name="args">An array of strings representing parameter keys and values.</param>
    public QueryFilterConfig(params string[] args)
    {
        if (args == null) return;

        for (var i = 0; i < args.Length; i += 2)
        {
            if (i + 1 < args.Length)
            {
                Parameters.Add(new QueryValuesConfig { Key = args[i], Value = args[i + 1] });
            }
        }
    }

    /// <summary>
    /// Retrieves the string value associated with the specified parameter key.
    /// </summary>
    /// <param name="parameter">The key of the parameter to retrieve.</param>
    /// <returns>The string value associated with the specified key, or an empty string if not found.</returns>
    public string GetStringValue(string parameter)
    {
        return Parameters.FirstOrDefault(p => p.Key.Equals(parameter, StringComparison.CurrentCultureIgnoreCase))?.Value;
    }

    /// <summary>
    /// Retrieves the integer value associated with the specified key.
    /// </summary>
    /// <param name="key">The key of the parameter to retrieve.</param>
    /// <returns>The integer value associated with the specified key, or -1 if not found.</returns>
    public int GetIntegerValue(string key)
    {
        return int.Parse(Parameters.FirstOrDefault(p => p.Key.Equals(key, StringComparison.OrdinalIgnoreCase))?.Value);
    }

    /// <summary>
    /// Tries to parse the integer value associated with the specified key.
    /// </summary>
    /// <param name="key">The key of the parameter to retrieve.</param>
    /// <param name="value">When this method returns, contains the integer value associated with the specified key, if the key is found and parsed successfully; otherwise, -1.</param>
    /// <param name="defaultReturnValue">The integer to be returned if parsing is unsuccessful.</param>
    /// <returns><c>true</c> if the key is found and the value is parsed successfully; otherwise, <c>false</c>.</returns>
    public bool TryParseIntegerValue(string key, out int value, int defaultReturnValue = -1)
    {
        value = defaultReturnValue;
        return TryGetStringValue(key, out var stringValue) && int.TryParse(stringValue, out value);
    }

    /// <summary>
    /// Tries to parse the date value associated with the specified key.
    /// </summary>
    /// <param name="key">The key of the parameter to retrieve.</param>
    /// <param name="value">When this method returns, contains the date value associated with the specified key, if the key is found and parsed successfully; otherwise, the default value of <c>DateTime</c>.</param>
    /// <param name="defaultReturnValue">The DateTime to be returned if parsing is unsuccessful.</param>
    /// <returns><c>true</c> if the key is found and the value is parsed successfully; otherwise, <c>false</c>.</returns>
    public bool TryParseDateValue(string key, out DateTime value, DateTime defaultReturnValue = default)
    {
        value = defaultReturnValue;
        return TryGetStringValue(key, out var stringValue) && DateTime.TryParse(stringValue, out value);
    }

    /// <summary>
    /// Tries to get the string value associated with the specified key from the parameters.
    /// </summary>
    /// <param name="key">The key of the parameter to retrieve.</param>
    /// <param name="value">When this method returns, contains the string value associated with the specified key, if the key is found; otherwise, an empty string.</param>
    /// <param name="defaultReturnValue">The string to be returned if the key is not found.</param>
    /// <returns><c>true</c> if the key is found and the value is not null or empty; otherwise, <c>false</c>.</returns>
    public bool TryGetStringValue(string key, out string value, string defaultReturnValue = default)
    {
        var parameter = Parameters
            .FirstOrDefault(p => string.Equals(p.Key, key, StringComparison.OrdinalIgnoreCase));

        value = parameter?.Value ?? defaultReturnValue;

        return !string.IsNullOrEmpty(value);
    }

    /// <summary>
    /// Adds an integer parameter to the query filter configuration.
    /// </summary>
    /// <param name="key">The key of the parameter to add.</param>
    /// <param name="value">The value of the parameter to add.</param>
    /// <returns>The current instance of QueryFilterConfig with the added parameter.</returns>
    public QueryFilterConfig AddInteger(string key, int value)
    {
        return AddString(key, value.ToString());
    }

    /// <summary>
    /// Adds a string parameter to the query filter configuration.
    /// </summary>
    /// <param name="key">The key of the parameter to add.</param>
    /// <param name="value">The value of the parameter to add.</param>
    /// <returns>The current instance of QueryFilterConfig with the added or updated parameter.</returns>
    public QueryFilterConfig AddString(string key, string value)
    {
        Parameters ??= [];

        var existingParameter = Parameters.FirstOrDefault(x => string.Equals(x.Key, key, StringComparison.OrdinalIgnoreCase));
        if (existingParameter == null)
        {
            Parameters.Add(new QueryValuesConfig { Key = key, Value = value });
        }
        else
        {
            existingParameter.Value = value;
        }

        return this;
    }

    /// <summary>
    /// Removes specificed parameter from the query filter configuration.
    /// </summary>
    /// <param name="key">The key of the parameter to remove.</param>
    /// <returns>The current instance of QueryFilterConfig with the parameter removed.</returns>
    public QueryFilterConfig Remove(string key)
    {
        Parameters.RemoveAll(x => string.Equals(x.Key, key, StringComparison.OrdinalIgnoreCase));
        return this;
    }

    /// <summary>
    /// Nulls the specificed parameter's value from the query filter configuration.
    /// </summary>
    /// <param name="key">The key of the parameter value to null.</param>
    /// <returns>The current instance of QueryFilterConfig with the parameter value nulled.</returns>
    public QueryFilterConfig Null(string key)
    {
        var existingParameter = Parameters.FirstOrDefault(x => string.Equals(x.Key, key, StringComparison.OrdinalIgnoreCase));
        if (existingParameter != null)
        {
            existingParameter.Value = null;
        }
        return this;
    }

    /// <summary>
    /// Clears all parameters from the query filter configuration.
    /// </summary>
    /// <returns>The current instance of QueryFilterConfig with cleared parameters.</returns>
    public QueryFilterConfig Clear()
    {
        Parameters.Clear();
        return this;
    }

    /// <summary>
    /// Adds token filter parameters to the query based on the provided token information.
    /// </summary>
    /// <param name="tableName">The name of the table to apply the filter to.</param>
    /// <param name="token">The token information model containing filter values.</param>
    public void AddTokenFilter(string tableName, TokenInfoModel token)
    {
        Parameters ??= [];

        //if (!string.Equals(tableName, "specimen", StringComparison.OrdinalIgnoreCase) && ! string.Equals(tableName, "patient", StringComparison.OrdinalIgnoreCase))
        //{
        //    return;
        //}

        if (token != null)
        {
            AddParameterIfValid(token.LaboratoryId, "LaboratoryId");
            AddParameterIfValid(token.OrganisationId, "OrganisationId");
        }
    }

    /// <summary>
    /// Adds a parameter to the list if the value is valid.
    /// </summary>
    /// <param name="value">The value to be added.</param>
    /// <param name="key">The key for the parameter.</param>
    private void AddParameterIfValid(string value, string key)
    {
        if (!string.IsNullOrEmpty(value) && value != "0")
        {
            Parameters.Add(new QueryValuesConfig { Key = key, Value = value });
        }
    }

    /// <summary>
    /// Constructs a WHERE clause from the provided parameters.
    /// </summary>
    /// <returns>A string representing the WHERE clause for SQL queries.</returns>
    public string CreateWhereClauseFromParameters()
    {
        var whereClauses = new List<string>();
        foreach (var parameter in Parameters)
        {
            if (!string.IsNullOrEmpty(parameter.Value))
            {
                whereClauses.Add($"{parameter.Key} = @{parameter.Key}");
            }
        }
        return string.Join(" and ", whereClauses);
    }
}
