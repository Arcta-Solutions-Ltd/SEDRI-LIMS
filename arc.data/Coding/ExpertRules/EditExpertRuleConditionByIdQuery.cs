using arc.data.model.Coding;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Coding.ExpertRules;

/// <summary>
/// Loads a single expert rule condition row by id for the record-view edit form.
/// </summary>
internal class EditExpertRuleConditionByIdQuery : IQueryReturningType<ExpertRuleConditionDataModel>
{
    /// <summary>
    /// Executes the query for the given condition id.
    /// </summary>
    /// <param name="connect">Active database connection.</param>
    /// <param name="queryFilters">Filter config; expects integer "id".</param>
    /// <returns>The condition row, or an empty model when not found.</returns>
    public async Task<ExpertRuleConditionDataModel> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var id = queryFilters.GetIntegerValue("id");

        const string sql = """
            select
                id,
                expertruleid,
                antibioticid,
                antibioticgroupid,
                testmethodid,
                susceptibilityid,
                specialconsiderationid,
                startval,
                endval,
                lastmodifieddate
            from expertrulecondition
            where id = @id
            """;

        var row = await connect.QueryFirstOrDefaultAsync<ExpertRuleConditionDataModel>(sql, new { id });
        return row ?? new ExpertRuleConditionDataModel();
    }
}
