using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Configuration
{
    /// <summary>
    /// Query to retrieve a filtered subset of state list options (excludes specific IDs).
    /// </summary>
    internal class ShortStateListQuery : IQueryReturningType<List<OptionsConfig>>
    {
        /// <summary>
        /// Executes the query for the short state list.
        /// </summary>
        public async Task<List<OptionsConfig>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var sql = @"select id as key, value as text from listitem where listid = 60 and id != 534 and id != 528 and id != 537 and deleted = false order by value";

            var result = await connect.QueryAsync<OptionsConfig>(sql);

            return result.ToList();
        }
    }
}
