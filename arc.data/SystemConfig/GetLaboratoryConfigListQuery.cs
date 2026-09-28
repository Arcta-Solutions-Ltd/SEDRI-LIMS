using arc.data.model.Configuration;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.SystemConfig;

/// <summary>
/// Query for retrieving a list of laboratory configurations.
/// </summary>
internal class GetLaboratoryConfigListQuery : IQueryReturningType<List<LaboratoryConfigsDataModel>>
{
    /// <summary>
    /// Executes the query to retrieve a list of laboratory configurations based on the provided filters.
    /// </summary>
    /// <param name="connect">The database connection used to execute the query.</param>
    /// <param name="queryFilters">The query filters containing parameters such as LaboratoryId and ConfigName.</param>
    /// <returns>
    /// A list of <see cref="LaboratoryConfigsDataModel"/> objects representing the laboratory configurations.
    /// </returns>
    public async Task<List<LaboratoryConfigsDataModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var laboratoryid = queryFilters.GetStringValue("id");
        var configname = queryFilters.GetStringValue("configname");

        var sql = @"select * from laboratoryconfigs where LaboratoryId = @LaboratoryId 
                    and ConfigName = @ConfigName";
        var result = await connect.QueryAsync<LaboratoryConfigsDataModel>(sql, new { laboratoryId = int.Parse(laboratoryid), configname });
        return result.ToList();
    }
}
