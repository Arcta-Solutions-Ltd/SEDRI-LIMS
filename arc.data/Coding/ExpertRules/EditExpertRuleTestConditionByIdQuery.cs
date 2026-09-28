using arc.data.model.Coding;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Coding.ExpertRules;

/// <summary>
/// Loads a single expert rule test condition row by id for the record-view edit form.
/// </summary>
internal class EditExpertRuleTestConditionByIdQuery : IQueryReturningType<ExpertRuleTestConditionDataModel>
{
    /// <summary>
    /// Executes the query for the given test condition id.
    /// </summary>
    /// <param name="connect">Active database connection.</param>
    /// <param name="queryFilters">Filter config; expects integer "id".</param>
    /// <returns>The test condition row, or an empty model when not found.</returns>
    public async Task<ExpertRuleTestConditionDataModel> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var id = queryFilters.GetIntegerValue("id");

        const string sql = """
            select
                id,
                expertruleid,
                testname,
                fieldname,
                comparison,
                compvalue,
                lastmodifieddate
            from expertruletestcondition
            where id = @id
            """;

        var row = await connect.QueryFirstOrDefaultAsync<ExpertRuleTestConditionDataModel>(sql, new { id });
        return row ?? new ExpertRuleTestConditionDataModel();
    }
}
