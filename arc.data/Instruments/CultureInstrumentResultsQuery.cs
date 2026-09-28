using arc.common.Models.Instruments;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Instruments
{
    /// <summary>
    /// Represents a query to retrieve culture instrument results.
    /// </summary>
    internal class CultureInstrumentResultsQuery : IQueryReturningType<List<InstrumentResultsListModel>>
    {
        /// <summary>
        /// Executes the query asynchronously to retrieve culture instrument results based on the specified query filters.
        /// </summary>
        /// <param name="connect">The Npgsql connection to use for the query.</param>
        /// <param name="queryFilters">The query filter configuration.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the list of instrument results.</returns>
        public async Task<List<InstrumentResultsListModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var cultureId = queryFilters.Parameters.FirstOrDefault(p => p.Key.Equals("cultureid", StringComparison.OrdinalIgnoreCase))?.Value;

            if (cultureId == null)
            {
                throw new ArgumentException("CultureId is missing in query filters.");
            }

            var sql = @"SELECT 
                        ir.id::text AS id,
                        ir.instrumentprofile AS instrumentprofile, 
                        li1.Value AS status, 
                        CASE WHEN ir.resultreceived IS NULL THEN '' ELSE TO_CHAR(ir.resultreceived AT TIME ZONE 'UTC', 'YYYY-MM-DD HH24:MI:SS') END AS ResultReceived,
                        CASE WHEN ir.requestmade IS NULL THEN '' ELSE TO_CHAR(ir.requestmade AT TIME ZONE 'UTC', 'YYYY-MM-DD HH24:MI:SS') END AS RequestMade
                    FROM instrumentresults ir
                    INNER JOIN listitem li1 ON ir.statusid = li1.id
                    WHERE cultureid = @cultureid
                    ORDER BY ir.requestmade";

            var result = await connect.QueryAsync<InstrumentResultsListModel>(sql, new { cultureid = int.Parse(cultureId) });
            return result.ToList();
        }
    }

}
