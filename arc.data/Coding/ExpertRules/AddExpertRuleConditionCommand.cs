using arc.app.Common;
using arc.data.model.Coding;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Coding.ExpertRules;

/// <summary>
/// Inserts a single expert rule condition row.
/// </summary>
internal class AddExpertRuleConditionCommand : ICommandWithTypeReturningInteger<ExpertRuleConditionDataModel>
{
    /// <summary>
    /// Inserts the condition and returns the new row id.
    /// </summary>
    /// <param name="connect">Active database connection.</param>
    /// <param name="command">Row values to insert.</param>
    /// <param name="logWriter">Logger for installed-system diagnostics.</param>
    /// <returns>The new condition id.</returns>
    public async Task<int> ExecuteAsync(NpgsqlConnection connect, ExpertRuleConditionDataModel command, ILogWriter logWriter)
    {
        const string sql = """
            insert into expertrulecondition
                (expertruleid, antibioticid, antibioticgroupid, testmethodid, susceptibilityid, specialconsiderationid, startval, endval, lastmodifieddate)
            values
                (@ExpertRuleId, @AntibioticId, @AntibioticGroupId, @TestMethodId, @SusceptibilityId, @SpecialConsiderationId, @StartVal, @EndVal, now())
            returning id
            """;

        var newId = await connect.ExecuteScalarAsync<int>(sql, command);
        logWriter.LogInfo(
            $"Added expert rule condition id={newId} expertRuleId={command.ExpertRuleId} startVal={command.StartVal} endVal={command.EndVal}",
            nameof(AddExpertRuleConditionCommand),
            nameof(ExecuteAsync));
        return newId;
    }
}
