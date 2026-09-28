using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Instruments;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Instruments;

/// <summary>
/// Represents a query to retrieve an instrument profile based on a specimen ID and test name.
/// Implements the <see cref="IQueryReturningType{T}"/> interface.
/// </summary>
internal class InstrumentProfileByDirectNameQuery : IQueryReturningType<SingleInstrumentConfig>
{
    /// <summary>
    /// Executes the query asynchronously to retrieve an instrument profile configuration
    /// for a given specimen ID and test name.
    /// </summary>
    /// <param name="connect">The PostgreSQL database connection.</param>
    /// <param name="queryFilters">The query filter configuration containing the specimen ID and test name parameters.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the
    /// <see cref="SingleInstrumentConfig"/> object with the retrieved data.
    /// </returns>
    public async Task<SingleInstrumentConfig> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var id = queryFilters.Parameters.Where(p => p.Key.ToLower() == "specimenid").First();
        var testName = queryFilters.Parameters.Where(p => p.Key.ToLower() == "testname").First();

        var sql = @"select s.laboratoryid, s.specimentypeid from specimen s
                        inner join tests t on t.specimenid = s.id
                        where s.id = @specimenid and lower(trim(t.testname)) = lower(trim(@testname))";

        var row = await connect.QueryFirstOrDefaultAsync<SingleInstrumentConfig>(sql, new { SpecimenId = int.Parse(id.Value), testname = testName.Value });
        return row ?? new SingleInstrumentConfig();
    }
}
