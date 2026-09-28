using arc.common.Models.Specimen;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Specimen;

/// <summary>
/// Represents a database query to retrieve a SpecimenModel by its ID.
/// Implements IQueryReturningType<T> for structured query execution.
/// </summary>
internal class SpecimenByIdQuery : IQueryReturningType<SpecimenModel>
{
    /// <summary>
    /// Executes the query asynchronously to fetch a specimen record by ID.
    /// </summary>
    /// <param name="connect">Active PostgreSQL connection.</param>
    /// <param name="queryFilters">Query filter configuration containing parameters.</param>
    /// <returns>A SpecimenModel instance if found; otherwise, null.</returns>
    public async Task<SpecimenModel> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var id = queryFilters.GetIntegerValue("id");

        var sql = """
            SELECT Id, SpecimenTypeId, LaboratoryId FROM Specimen
            WHERE Id = @id
            """;

        return await connect.QueryFirstAsync<SpecimenModel>(sql, new { id });
    }
}
