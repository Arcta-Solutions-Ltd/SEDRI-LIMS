using arc.common.Models.Coding;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Coding;

/// <summary>
/// Data query that loads expert rule test conditions for a given expert rule.
/// Columns (TestName, FieldName, Comparison, CompValue) store stable ids; list display resolves titles and labels server-side.
/// </summary>
internal class ExpertRuleTestConditionListByExpertRuleIdQuery : IQueryReturningType<List<ExpertRuleTestConditionListModel>>
{
    /// <summary>
    /// Executes the query and returns the list of test conditions for the given expert rule.
    /// </summary>
    /// <param name="connect">Active database connection.</param>
    /// <param name="queryFilters">Filter config; expects "ExpertRuleId" with the expert rule ID.</param>
    /// <returns>List of test condition rows.</returns>
    public async Task<List<ExpertRuleTestConditionListModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var expertRuleId = queryFilters.GetIntegerValue("ExpertRuleId");

        var sql = """
            select
                Id,
                TestName,
                FieldName,
                Comparison,
                CompValue
            from expertruletestcondition
            where expertruleid = @expertRuleId
            order by id
            """;

        var results = await connect.QueryAsync<ExpertRuleTestConditionListModel>(sql, new { expertRuleId });
        return results.ToList();
    }
}
