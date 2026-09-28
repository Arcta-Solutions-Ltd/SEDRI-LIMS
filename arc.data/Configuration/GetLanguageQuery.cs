using arc.domain.Configuration.LanguageConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Configuration;

/// <summary>
/// Retrieves a list of language items for a given translation identifier, 
/// optionally filtering by value.
/// </summary>
internal class GetLanguageQuery : IQueryReturningType<List<LanguageItem>>
{
    /// <summary>
    /// Executes the query asynchronously to fetch language entries.
    /// </summary>
    /// <param name="connect">The NpgsqlConnection used to execute the SQL.</param>
    /// <param name="queryFilters">
    /// Contains the parameters for the query:
    /// - index 0: translation identifier
    /// - optional "value" filter for language text
    /// </param>
    /// <returns>
    /// A task that resolves to a list of <see cref="LanguageItem"/> instances,
    /// filtered by the provided value if specified.
    /// </returns>
    public async Task<List<LanguageItem>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var translationid = queryFilters.Parameters[0].Value;

        var sql = @"select s.pack->> 'Key' as key , s.pack->> 'Value' as value from (select jsonb_array_elements(pack) as pack from language where translationid = "
                    + translationid
                    + ") As s order by s.pack->> 'value'";

        var result = await connect.QueryAsync<LanguageItem>(sql);

        var valueFilter = queryFilters.Parameters.Where(q => q.Key.ToLower() == "value");

        if (valueFilter != null && valueFilter.Count() > 0)
        {
            var filter = valueFilter.First();
            result = result.Where(l => l.Value.ToLower().Contains(filter.Value)).ToList();
        }

        return result.ToList();
    }
}
