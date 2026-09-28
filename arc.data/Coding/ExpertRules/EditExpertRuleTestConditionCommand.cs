using arc.app.Common;
using arc.data.model.Coding;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Coding.ExpertRules;

/// <summary>
/// Updates a single expert rule test condition row by id.
/// </summary>
internal class EditExpertRuleTestConditionCommand : ICommandWithTypeReturningInteger<ExpertRuleTestConditionDataModel>
{
    /// <summary>
    /// Persists the test condition row.
    /// </summary>
    /// <param name="connect">Active database connection.</param>
    /// <param name="command">Updated row values including id.</param>
    /// <param name="logWriter">Logger for installed-system diagnostics.</param>
    /// <returns>The updated test condition id.</returns>
    public async Task<int> ExecuteAsync(NpgsqlConnection connect, ExpertRuleTestConditionDataModel command, ILogWriter logWriter)
    {
        const string sql = """
            update expertruletestcondition
            set
                testname = @TestName,
                fieldname = @FieldName,
                comparison = @Comparison,
                compvalue = @CompValue,
                lastmodifieddate = now()
            where id = @Id
            """;

        await connect.ExecuteAsync(sql, command);
        logWriter.LogInfo(
            $"Updated expert rule test condition id={command.Id} expertRuleId={command.ExpertRuleId} hasCriteria={!string.IsNullOrWhiteSpace(command.FieldName)}",
            nameof(EditExpertRuleTestConditionCommand),
            nameof(ExecuteAsync));
        return command.Id;
    }
}
