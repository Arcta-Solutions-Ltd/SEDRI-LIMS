using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Specimen;

/// <summary>
/// Looks up a specimen Id by exact accession number match.
/// </summary>
internal class IdByAccessionNumberQuery : IQueryReturningString
{
    /// <summary>
    /// Executes the query to return the specimen Id for the provided accession number.
    /// </summary>
    /// <param name="connect">An open database connection.</param>
    /// <param name="queryFilters">Filter bag containing <c>accessionnumber</c>.</param>
    /// <returns>The specimen Id as a string.</returns>
    public async Task<string> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var accessionNumber = queryFilters.GetStringValue("accessionnumber");

        var sql = """
            select Id from Specimen
            where AccessionNumber = @accessionnumber
            """;

        return await connect.QueryFirstAsync<string>(sql, new { accessionNumber });
    }
}
