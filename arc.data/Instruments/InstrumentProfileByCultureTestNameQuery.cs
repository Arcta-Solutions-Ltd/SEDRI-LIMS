using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Instruments;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Instruments;

/// <summary>
/// Represents a query to retrieve an instrument profile based on a culture ID and test name.
/// Implements the <see cref="IQueryReturningType{T}"/> interface.
/// </summary>
internal class InstrumentProfileByCultureTestNameQuery : IQueryReturningType<SingleInstrumentConfig>
{
    /// <summary>
    /// Executes the query asynchronously to retrieve an instrument profile configuration
    /// for a given culture ID and test name.
    /// </summary>
    /// <param name="connect">The PostgreSQL database connection.</param>
    /// <param name="queryFilters">The query filter configuration containing the culture ID and test name parameters.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the
    /// <see cref="SingleInstrumentConfig"/> object with the retrieved data.
    /// </returns>
    public async Task<SingleInstrumentConfig> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var id = queryFilters.Parameters.Where(p => p.Key.ToLower() == "cultureid").First();
        var testName = queryFilters.Parameters.Where(p => p.Key.ToLower() == "testname").First();

        var sql = @"select s.laboratoryid, c.typeid as culturetypeid, s.specimentypeid,
                        c.orggroupcodingid::text as cultureorggroupcodingid
                    from culture c
                        inner join specimen s on c.specimenid = s.id
                        inner join culturetests ct on ct.cultureid = c.id
                        where c.id = @CultureId and lower(trim(ct.testname)) = lower(trim(@testname))";

        return await connect.QueryFirstAsync<SingleInstrumentConfig>(sql, new { CultureId = int.Parse(id.Value), testname = testName.Value });
    }
}
