using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Instruments;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Instruments
{
    /// <summary>
    /// Represents a query for retrieving an instrument profile by specimen ID.
    /// </summary>
    internal class InstrumentProfileBySpecimenIdQuery : IQueryReturningType<SingleInstrumentConfig>
    {
        /// <summary>
        /// Executes the query asynchronously and returns the instrument profile that matches the specified specimen ID.
        /// </summary>
        /// <param name="connect">The Npgsql connection to use for the query.</param>
        /// <param name="queryFilters">The query filter configuration containing the filter values.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the single instrument configuration.</returns>
        public async Task<SingleInstrumentConfig> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var id = queryFilters.Parameters.First(p => p.Key.Equals("id", System.StringComparison.CurrentCultureIgnoreCase));
            var sql = @"select s.laboratoryid, s.specimentypeid from specimen s where s.id = @Id";
            return await connect.QueryFirstAsync<SingleInstrumentConfig>(sql, new { Id = int.Parse(id.Value) });
        }
    }

}
