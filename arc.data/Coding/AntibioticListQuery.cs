using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Coding;

/// <summary>
/// Query class that retrieves a list of antibiotics as options for dropdowns or selections.
/// </summary>
internal class AntibioticListQuery : IQueryReturningType<List<OptionsConfig>>
{
    /// <summary>
    /// Executes the query asynchronously to fetch the list of antibiotics.
    /// </summary>
    /// <param name="connect">An open <see cref="NpgsqlConnection"/> used to execute the SQL query.</param>
    /// <param name="queryFilters">
    /// A <see cref="QueryFilterConfig"/> containing any query filters (not used in this implementation).
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of
    /// <see cref="OptionsConfig"/> objects with the antibiotic id as the key and the antibiotic name as the text.
    /// </returns>
    public async Task<List<OptionsConfig>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var sql = @"select id as key, antibioticname As text from antibiotic order by antibioticname";

        var result = await connect.QueryAsync<OptionsConfig>(sql);
        return result.ToList();
    }
}
