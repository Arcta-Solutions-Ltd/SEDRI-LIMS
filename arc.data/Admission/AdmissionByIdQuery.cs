using arc.common.Models.Admissions;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Admission;

/// <summary>
/// Returns a single admission by its primary key.
/// </summary>
internal class AdmissionByIdQuery : IQueryReturningType<AdmissionModel>
{
    /// <summary>
    /// Runs the query against the supplied connection.
    /// </summary>
    /// <param name="connect">An open connection to the database.</param>
    /// <param name="queryFilters">Filter configuration carrying the id parameter.</param>
    /// <returns>The admission, or an empty model when no row matches.</returns>
    public async Task<AdmissionModel> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var id = queryFilters.GetIntegerValue("id");

        var sql = "SELECT Id, PatientId, MoreData, LastModifiedDate FROM Admission WHERE Id = @Id";

        var result = await connect.QueryFirstOrDefaultAsync<AdmissionModel>(sql, new { Id = id });

        return result ?? new AdmissionModel();
    }
}
