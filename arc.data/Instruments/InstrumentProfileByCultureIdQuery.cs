using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Instruments;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Instruments;

/// <summary>
/// Represents a query to retrieve an instrument profile by culture ID.
/// Implements the <see cref="IQueryReturningType{T}"/> interface.
/// </summary>
internal class InstrumentProfileByCultureIdQuery : IQueryReturningType<SingleInstrumentConfig>
{
    /// <summary>
    /// Executes the query asynchronously to retrieve an instrument profile configuration by culture ID.
    /// </summary>
    /// <param name="connect">The PostgreSQL database connection.</param>
    /// <param name="queryFilters">The query filter configuration containing the culture ID parameter.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the 
    /// <see cref="SingleInstrumentConfig"/> object with the retrieved data.
    /// </returns>
    public async Task<SingleInstrumentConfig> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var id = queryFilters.Parameters.Where(p => p.Key.ToLower() == "id").First();

        var sql = @"select s.laboratoryid, c.typeid as culturetypeid, s.specimentypeid,
                        c.moredata::jsonb->>'ManufacturersBarcode' as Barcode,
                        c.orggroupcodingid::text as cultureorggroupcodingid
                    from culture c
                    inner join specimen s on c.specimenid = s.id where c.id = @Id";

        return await connect.QueryFirstAsync<SingleInstrumentConfig>(sql, new { Id = int.Parse(id.Value) });
    }
}
