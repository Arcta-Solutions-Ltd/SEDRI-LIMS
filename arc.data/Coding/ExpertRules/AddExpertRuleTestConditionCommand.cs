using arc.app.Common;
using arc.data.model.Coding;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Coding.ExpertRules;

/// <summary>
/// Inserts a single expert rule test condition row.
/// </summary>
internal class AddExpertRuleTestConditionCommand : ICommandWithTypeReturningInteger<ExpertRuleTestConditionDataModel>
{
    /// <summary>
    /// Inserts the test condition and returns the new row id.
    /// </summary>
    /// <param name="connect">Active database connection.</param>
    /// <param name="command">Row values to insert.</param>
    /// <param name="logWriter">Logger for installed-system diagnostics.</param>
    /// <returns>The new test condition id.</returns>
    public async Task<int> ExecuteAsync(NpgsqlConnection connect, ExpertRuleTestConditionDataModel command, ILogWriter logWriter)
    {
        const string sql = """
            insert into expertruletestcondition
                (expertruleid, testname, fieldname, comparison, compvalue, lastmodifieddate)
            values
                (@ExpertRuleId, @TestName, @FieldName, @Comparison, @CompValue, now())
            returning id
            """;

        var newId = await connect.ExecuteScalarAsync<int>(sql, command);
        logWriter.LogInfo(
            $"Added expert rule test condition id={newId} expertRuleId={command.ExpertRuleId} hasCriteria={!string.IsNullOrWhiteSpace(command.FieldName)}",
            nameof(AddExpertRuleTestConditionCommand),
            nameof(ExecuteAsync));
        return newId;
    }
}
