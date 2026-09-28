using arc.app.Common;
using arc.data.model.Coding;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Coding.ExpertRules;

/// <summary>
/// Inserts a single expert rule action row.
/// </summary>
internal class AddExpertRuleActionCommand : ICommandWithTypeReturningInteger<ExpertRuleActionDataModel>
{
    /// <summary>
    /// Inserts the action and returns the new row id.
    /// </summary>
    /// <param name="connect">Active database connection.</param>
    /// <param name="command">Row values to insert.</param>
    /// <param name="logWriter">Logger for installed-system diagnostics.</param>
    /// <returns>The new action id.</returns>
    public async Task<int> ExecuteAsync(NpgsqlConnection connect, ExpertRuleActionDataModel command, ILogWriter logWriter)
    {
        const string sql = """
            insert into expertruleaction
                (expertruleid, antibioticid, antibioticgroupid, susceptibilityid, displayonreport, lastmodifieddate)
            values
                (@ExpertRuleId, @AntibioticId, @AntibioticGroupId, @SusceptibilityId, @DisplayOnReport, now())
            returning id
            """;

        var newId = await connect.ExecuteScalarAsync<int>(sql, command);

        logWriter.LogInfo(
            $"Added expert rule action id={newId} expertRuleId={command.ExpertRuleId} antibioticId={command.AntibioticId} antibioticGroupId={command.AntibioticGroupId}",
            nameof(AddExpertRuleActionCommand),
            nameof(ExecuteAsync));

        return newId;
    }
}
