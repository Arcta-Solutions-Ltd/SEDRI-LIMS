using arc.common.Data;
using arc.common.ExtensionMethods;
using arc.common.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace arc.data
{
    /// <summary>
    /// Represents the result of building a parameterized SQL query.
    /// </summary>
    internal class SqlBuildResult
    {
        /// <summary>
        /// Gets the SQL query template with parameter placeholders.
        /// </summary>
        public string Sql { get; init; }

        /// <summary>
        /// Gets the dictionary of parameter names and values.
        /// </summary>
        public Dictionary<string, object> Parameters { get; init; }

        /// <summary>
        /// Gets the payload keys excluded because they are client metadata, not columns on the target table.
        /// </summary>
        public IReadOnlyList<string> ExcludedMetadataFields { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlBuildResult"/> class.
        /// </summary>
        /// <param name="sql">The SQL query template.</param>
        /// <param name="parameters">The parameters dictionary.</param>
        /// <param name="excludedMetadataFields">Payload keys dropped because they are client metadata rather than columns.</param>
        public SqlBuildResult(string sql, Dictionary<string, object> parameters, IReadOnlyList<string> excludedMetadataFields = null)
        {
            Sql = sql;
            Parameters = parameters;
            ExcludedMetadataFields = excludedMetadataFields ?? [];
        }
    }

    /// <summary>
    /// Provides functionality to build SQL queries for inserting and updating records.
    /// Uses parameterized queries to prevent SQL injection attacks.
    /// </summary>
    internal class SqlBuilder
    {
        private Dictionary<string, string> _fields = [];
        private static readonly string[] sourceArray = ["event", "id", "view"];
        private static readonly string[] fieldsHandledSeparately = ["moredata", "lastmodifieddate"];

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlBuilder"/> class.
        /// </summary>
        /// <param name="contents">The JSON string containing the fields.</param>
        /// <param name="stringFields">Optional parameter for additional string fields.</param>
        internal SqlBuilder(string contents, string stringFields = "")
        {
            JToken json;

            // Parse the JSON string to a JToken with no date parsing
            using (var sr = new StringReader(contents))
            using (var jr = new JsonTextReader(sr) { DateParseHandling = DateParseHandling.None })
            {
                json = JToken.ReadFrom(jr);
            }

            var fieldsCollector = new JsonFieldsCollector(string.Empty, stringFields);
            var fieldList = fieldsCollector.GetAllFields(json);

            // Add fields to _fields dictionary, excluding "event", "id", and "view"
            foreach (var field in fieldList.Where(f => !sourceArray.Contains(f.Key.ToLower())))
            {
                _fields.Add(field.Key, field.Value);
            }
        }

        /// <summary>
        /// Returns true when a payload key is client metadata that the target table does not declare as a column.
        /// </summary>
        /// <param name="tableName">The name of the table being written to.</param>
        /// <param name="fieldName">The payload key under consideration.</param>
        /// <returns>True when the key must not be emitted as a column.</returns>
        private static bool IsExcludedMetadataField(string tableName, string fieldName)
        {
            return ClientPayloadMetadataFields.Contains(fieldName)
                && !SqlParameterTypeResolver.TryGetFieldTypeInfo(tableName, fieldName, out _);
        }

        /// <summary>
        /// Returns true when a payload key names a column the builder appends itself.
        /// </summary>
        /// <param name="fieldName">The payload key under consideration.</param>
        /// <returns>True when the key would duplicate a column the builder already writes.</returns>
        private static bool IsHandledSeparately(string fieldName)
        {
            return fieldsHandledSeparately.Contains(fieldName.ToLower());
        }

        /// <summary>
        /// Builds a parameterized SQL insert query for the specified table.
        /// </summary>
        /// <param name="tableName">The name of the table.</param>
        /// <param name="moreData">Additional JSON data.</param>
        /// <returns>A <see cref="SqlBuildResult"/> containing the SQL template and parameters.</returns>
        internal SqlBuildResult BuildInsertSqlParameterized(string tableName, string moreData)
        {
            var selectClauseBuilder = new StringBuilder();
            var valueClauseBuilder = new StringBuilder();
            var parameters = new Dictionary<string, object>();
            var excludedMetadataFields = new List<string>();
            var isFirstField = true;
            var paramIndex = 0;

            foreach (var field in _fields)
            {
                if (IsExcludedMetadataField(tableName, field.Key))
                {
                    excludedMetadataFields.Add(field.Key);
                    continue;
                }

                if (IsHandledSeparately(field.Key))
                {
                    continue;
                }

                if (!isFirstField)
                {
                    selectClauseBuilder.Append(',');
                    valueClauseBuilder.Append(',');
                }

                selectClauseBuilder.Append(field.Key);

                var value = field.Value;
                if (value == null || value == "null")
                {
                    valueClauseBuilder.Append("null");
                }
                else
                {
                    var paramName = $"@p{paramIndex++}";
                    valueClauseBuilder.Append(paramName);

                    if (SqlParameterTypeResolver.TryGetFieldTypeInfo(tableName, field.Key, out var fieldTypeInfo))
                    {
                        if (SqlParameterTypeResolver.TryConvert(value, fieldTypeInfo, out var typedValue))
                        {
                            if (typedValue == null)
                            {
                                valueClauseBuilder.Remove(valueClauseBuilder.Length - paramName.Length, paramName.Length);
                                valueClauseBuilder.Append("null");
                                paramIndex--;
                            }
                            else
                            {
                                if (fieldTypeInfo.IsJsonb)
                                {
                                    // Ensure JSONB cast in SQL for JSONB-annotated columns.
                                    valueClauseBuilder.Remove(valueClauseBuilder.Length - paramName.Length, paramName.Length);
                                    valueClauseBuilder.Append($"regexp_replace({paramName}, E'[\\n\\r]+', ' ', 'g')::jsonb");
                                }

                                parameters[paramName.TrimStart('@')] = typedValue;
                            }
                        }
                        else
                        {
                            // If parsing fails, set to null
                            valueClauseBuilder.Remove(valueClauseBuilder.Length - paramName.Length, paramName.Length);
                            valueClauseBuilder.Append("null");
                            paramIndex--;
                        }
                    }
                    else
                    {
                        // Unknown field types default to string binding.
                        parameters[paramName.TrimStart('@')] = value.Trim();
                    }
                }

                isFirstField = false;
            }

            if (moreData != "{}")
            {
                selectClauseBuilder.Append(",MoreData");
                var moreDataParam = $"p{paramIndex++}";
                valueClauseBuilder.Append($",cast(@{moreDataParam} as json)");
                parameters[moreDataParam] = moreData;
            }

            selectClauseBuilder.Append(",LastModifiedDate");
            valueClauseBuilder.Append(",").Append("now()");

            var sql = $"Insert into {tableName}({selectClauseBuilder}) Values({valueClauseBuilder}) returning id";
            return new SqlBuildResult(sql, parameters, excludedMetadataFields);
        }

        /// <summary>
        /// Builds a parameterized SQL update query for the specified table.
        /// </summary>
        /// <param name="tableName">The name of the table.</param>
        /// <param name="id">The ID of the record to update (must be a valid integer).</param>
        /// <param name="moreData">Additional JSON data.</param>
        /// <returns>A <see cref="SqlBuildResult"/> containing the SQL template and parameters.</returns>
        /// <exception cref="ArgumentException">Thrown when the ID is not a valid integer.</exception>
        internal SqlBuildResult BuildUpdateSqlParameterized(string tableName, string id, string moreData)
        {
            // SECURITY: Validate that Id is a valid integer to prevent SQL injection
            if (!int.TryParse(id, out var validatedId))
            {
                throw new ArgumentException($"Invalid ID value: '{id}'. ID must be a valid integer.", nameof(id));
            }

            var sqlBuilder = new StringBuilder($"Update {tableName} set ");
            var parameters = new Dictionary<string, object>();
            var excludedMetadataFields = new List<string>();
            var isFirstField = true;
            var paramIndex = 0;

            foreach (var field in _fields)
            {
                if (field.Key.Equals("id", StringComparison.OrdinalIgnoreCase) || field.Key.Contains("metaf", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (IsExcludedMetadataField(tableName, field.Key))
                {
                    excludedMetadataFields.Add(field.Key);
                    continue;
                }

                if (IsHandledSeparately(field.Key))
                {
                    continue;
                }

                if (SqlParameterTypeResolver.IsImmutableOnUpdate(tableName, field.Key))
                {
                    continue;
                }

                var value = field.Value;
                if (!isFirstField)
                {
                    sqlBuilder.Append(", ");
                }

                if (value == null || value == "null")
                {
                    sqlBuilder.Append($"{field.Key} = null");
                }
                else
                {
                    var paramName = $"p{paramIndex++}";

                    if (field.Key.Equals("testresults", StringComparison.OrdinalIgnoreCase))
                    {
                        // For testresults, we need special JSON handling
                        var cleanedValue = value.RemoveNullsFromJsonString().Replace("'", "\"");
                        sqlBuilder.Append($"{field.Key} = regexp_replace(@{paramName}, E'[\\n\\r]+', ' ', 'g')::jsonb");
                        parameters[paramName] = cleanedValue;
                    }
                    else if (SqlParameterTypeResolver.TryGetFieldTypeInfo(tableName, field.Key, out var fieldTypeInfo))
                    {
                        if (SqlParameterTypeResolver.TryConvert(value, fieldTypeInfo, out var typedValue))
                        {
                            if (typedValue == null)
                            {
                                sqlBuilder.Append($"{field.Key} = null");
                            }
                            else
                            {
                                if (fieldTypeInfo.IsJsonb)
                                {
                                    sqlBuilder.Append($"{field.Key} = regexp_replace(@{paramName}, E'[\\n\\r]+', ' ', 'g')::jsonb");
                                }
                                else
                                {
                                    sqlBuilder.Append($"{field.Key} = @{paramName}");
                                }

                                parameters[paramName] = typedValue;
                            }
                        }
                        else
                        {
                            // If parsing fails, treat as null
                            sqlBuilder.Append($"{field.Key} = null");
                        }
                    }
                    else
                    {
                        sqlBuilder.Append($"{field.Key} = @{paramName}");
                        parameters[paramName] = value.Trim();
                    }
                }

                isFirstField = false;
            }

            if (moreData != "{}")
            {
                var cleanedMoreData = moreData.RemoveNullsFromJsonString();
                var comma = isFirstField ? "" : ", ";
                var moreDataParam = $"p{paramIndex++}";
                sqlBuilder.Append($"{comma}MoreData = cast(@{moreDataParam} as json)");
                parameters[moreDataParam] = cleanedMoreData;
                isFirstField = false;
            }

            if (isFirstField)
            {
                return new SqlBuildResult(string.Empty, parameters, excludedMetadataFields);
            }

            // Use parameterized Id in WHERE clause
            sqlBuilder.Append($", LastModifiedDate = now() Where Id = @recordId");
            parameters["recordId"] = validatedId;

            return new SqlBuildResult(sqlBuilder.ToString(), parameters, excludedMetadataFields);
        }

        /// <summary>
        /// Builds an SQL insert query for the specified table.
        /// </summary>
        /// <param name="tableName">The name of the table.</param>
        /// <param name="dateFieldList">A comma-separated list of date fields.</param>
        /// <param name="moreData">Additional JSON data.</param>
        /// <returns>The SQL insert query string.</returns>
        [Obsolete("Use BuildInsertSqlParameterized for SQL injection protection. This method will be removed in a future version.")]
        internal string BuildInsertSql(string tableName, string dateFieldList, string moreData)
        {
            var result = BuildInsertSqlParameterized(tableName, moreData);

            // Convert parameterized query back to string format for backward compatibility
            var sql = result.Sql;
            foreach (var param in result.Parameters)
            {
                var value = param.Value?.ToString() ?? "null";
                // Escape single quotes for SQL safety
                var sanitizedValue = SqlSanitizer.SanitizeValue(value);
                sql = sql.Replace($"@{param.Key}", $"'{sanitizedValue}'");
            }

            return sql;
        }

        /// <summary>
        /// Builds an SQL update query for the specified table.
        /// </summary>
        /// <param name="tableName">The name of the table.</param>
        /// <param name="Id">The ID of the record to update.</param>
        /// <param name="dateFieldList">A comma-separated list of date fields.</param>
        /// <param name="moreData">Additional JSON data.</param>
        /// <returns>The SQL update query string.</returns>
        [Obsolete("Use BuildUpdateSqlParameterized for SQL injection protection. This method will be removed in a future version.")]
        internal string BuildUpdateSql(string tableName, string Id, string dateFieldList, string moreData)
        {
            var result = BuildUpdateSqlParameterized(tableName, Id, moreData);

            if (string.IsNullOrEmpty(result.Sql))
            {
                return string.Empty;
            }

            // Convert parameterized query back to string format for backward compatibility
            var sql = result.Sql;
            foreach (var param in result.Parameters)
            {
                var value = param.Value?.ToString() ?? "null";

                if (param.Key == "recordId")
                {
                    // ID is already validated as integer, use directly without quotes
                    sql = sql.Replace($"@{param.Key}", value);
                }
                else
                {
                    // Escape single quotes for SQL safety
                    var sanitizedValue = SqlSanitizer.SanitizeValue(value);
                    sql = sql.Replace($"@{param.Key}", $"'{sanitizedValue}'");
                }
            }

            return sql;
        }

    }
}
