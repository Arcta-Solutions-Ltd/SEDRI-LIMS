using arc.app.Common;
using arc.data.model.Coding;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Coding.ExpertRules;

/// <summary>
/// Updates a single expert rule action row by id.
/// </summary>
internal class EditExpertRuleActionCommand : ICommandWithTypeReturningInteger<ExpertRuleActionDataModel>
{
    /// <summary>
    /// Persists the action row.
    /// </summary>
    /// <param name="connect">Active database connection.</param>
    /// <param name="command">Updated row values including id.</param>
    /// <param name="logWriter">Logger for installed-system diagnostics.</param>
    /// <returns>The updated action id.</returns>
    public async Task<int> ExecuteAsync(NpgsqlConnection connect, ExpertRuleActionDataModel command, ILogWriter logWriter)
    {
        const string sql = """
            update expertruleaction
            set
                antibioticid = @AntibioticId,
                antibioticgroupid = @AntibioticGroupId,
                susceptibilityid = @SusceptibilityId,
                displayonreport = @DisplayOnReport,
                lastmodifieddate = now()
            where id = @Id
            """;

        await connect.ExecuteAsync(sql, command);

        logWriter.LogInfo(
            $"Updated expert rule action id={command.Id} antibioticId={command.AntibioticId} antibioticGroupId={command.AntibioticGroupId}",
            nameof(EditExpertRuleActionCommand),
            nameof(ExecuteAsync));

        return command.Id;
    }
}
