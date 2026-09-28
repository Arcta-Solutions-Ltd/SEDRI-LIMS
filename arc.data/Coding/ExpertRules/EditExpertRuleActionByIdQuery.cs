using arc.data.model.Coding;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Coding.ExpertRules;

/// <summary>
/// Loads a single expert rule action row by id for the record-view edit form.
/// </summary>
internal class EditExpertRuleActionByIdQuery : IQueryReturningType<ExpertRuleActionDataModel>
{
    /// <summary>
    /// Executes the query for the given action id.
    /// </summary>
    /// <param name="connect">Active database connection.</param>
    /// <param name="queryFilters">Filter config; expects integer "id".</param>
    /// <returns>The action row, or an empty model when not found.</returns>
    public async Task<ExpertRuleActionDataModel> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var id = queryFilters.GetIntegerValue("id");

        const string sql = """
            select
                id,
                expertruleid,
                antibioticid,
                antibioticgroupid,
                susceptibilityid,
                displayonreport,
                lastmodifieddate
            from expertruleaction
            where id = @id
            """;

        var row = await connect.QueryFirstOrDefaultAsync<ExpertRuleActionDataModel>(sql, new { id });
        return row ?? new ExpertRuleActionDataModel();
    }
}
