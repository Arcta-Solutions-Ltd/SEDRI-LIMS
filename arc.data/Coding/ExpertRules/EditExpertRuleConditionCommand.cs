using arc.app.Common;
using arc.data.model.Coding;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Coding.ExpertRules;

/// <summary>
/// Updates a single expert rule condition row by id.
/// </summary>
internal class EditExpertRuleConditionCommand : ICommandWithTypeReturningInteger<ExpertRuleConditionDataModel>
{
    /// <summary>
    /// Persists the condition row.
    /// </summary>
    /// <param name="connect">Active database connection.</param>
    /// <param name="command">Updated row values including id.</param>
    /// <param name="logWriter">Logger for installed-system diagnostics.</param>
    /// <returns>The updated condition id.</returns>
    public async Task<int> ExecuteAsync(NpgsqlConnection connect, ExpertRuleConditionDataModel command, ILogWriter logWriter)
    {
        const string sql = """
            update expertrulecondition
            set
                antibioticid = @AntibioticId,
                antibioticgroupid = @AntibioticGroupId,
                testmethodid = @TestMethodId,
                susceptibilityid = @SusceptibilityId,
                specialconsiderationid = @SpecialConsiderationId,
                startval = @StartVal,
                endval = @EndVal,
                lastmodifieddate = now()
            where id = @Id
            """;

        await connect.ExecuteAsync(sql, command);
        logWriter.LogInfo(
            $"Updated expert rule condition id={command.Id} startVal={command.StartVal} endVal={command.EndVal}",
            nameof(EditExpertRuleConditionCommand),
            nameof(ExecuteAsync));
        return command.Id;
    }
}
