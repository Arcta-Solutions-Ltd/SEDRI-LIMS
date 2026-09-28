using arc.common.Data;
using arc.data.model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace arc.data;

/// <summary>
/// Resolves physical columns vs MoreData fields by reflecting <c>arc.data.model</c> types
/// registered in <see cref="DataModelUtils.DataModelList"/>.
/// </summary>
public sealed class DataModelColumnResolver : IDataModelColumnResolver
{
    private static readonly ConcurrentDictionary<string, HashSet<string>> PhysicalColumnsByTable =
        new(StringComparer.OrdinalIgnoreCase);

    private static readonly Dictionary<string, Dictionary<string, string>> FieldAliasesByTable =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["patient"] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "Gender", "GenderId" }
            }
        };

    /// <inheritdoc />
    public bool IsPhysicalColumn(string tableName, string fieldId)
    {
        if (string.IsNullOrWhiteSpace(tableName) || string.IsNullOrWhiteSpace(fieldId))
        {
            return false;
        }

        var resolvedFieldId = ResolveAlias(tableName, fieldId);
        var columns = PhysicalColumnsByTable.GetOrAdd(tableName, BuildPhysicalColumnSet);
        return columns.Contains(resolvedFieldId);
    }

    /// <inheritdoc />
    public bool IsMoreDataField(string tableName, string fieldId) =>
        !IsPhysicalColumn(tableName, fieldId);

    private static string ResolveAlias(string tableName, string fieldId)
    {
        if (FieldAliasesByTable.TryGetValue(tableName, out var aliases)
            && aliases.TryGetValue(fieldId, out var mapped))
        {
            return mapped;
        }

        return fieldId;
    }

    private static HashSet<string> BuildPhysicalColumnSet(string tableName)
    {
        var modelType = ResolveModelType(tableName);
        if (modelType == null)
        {
            return new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        }

        var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        for (var type = modelType; type != null && type != typeof(object); type = type.BaseType)
        {
            foreach (var prop in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
            {
                if (prop.GetCustomAttributes(inherit: true).Any(a => a.GetType() == typeof(JsonbAttribute))
                    && prop.Name.Equals("MoreData", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                set.Add(prop.Name);
            }
        }

        return set;
    }

    private static Type? ResolveModelType(string tableName)
    {
        var match = DataModelUtils.DataModelList()
            .FirstOrDefault(d => d.Value.Equals(tableName, StringComparison.OrdinalIgnoreCase));
        return match.Key;
    }
}
