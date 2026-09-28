using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Specimen;

/// <summary>
/// Returns the specimen type of a specimen, keyed on the specimen id.
/// </summary>
internal class SpecimenTypeBySpecimenIdQuery : IQueryReturningInteger
{
    /// <summary>
    /// Runs the query against the supplied connection.
    /// </summary>
    /// <param name="connect">An open connection to the database.</param>
    /// <param name="queryFilters">Filter configuration carrying the id parameter.</param>
    /// <returns>The specimen type list item id.</returns>
    public async Task<int> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var id = queryFilters.GetIntegerValue("id");

        var sql = "select s.specimentypeid from specimen s where s.id = @id";

        return await connect.QueryFirstAsync<int>(sql, new { id });
    }
}
