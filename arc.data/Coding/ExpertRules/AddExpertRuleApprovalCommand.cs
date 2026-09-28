using arc.app.Common;
using arc.domain.Coding;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Coding;

/// <summary>
/// Command to insert a new expert rule approval record.
/// When the status is Rejected, also sets the parent expert rule's Enabled to 'No'.
/// </summary>
internal class AddExpertRuleApprovalCommand : ICommandWithTypeReturningInteger<ExpertRuleApproval>
{
    private const int CodingStatusRejected = 146;

    /// <summary>
    /// Executes the insert operation for an expert rule approval.
    /// When CodingStatusId is Rejected (146), updates the expert rule to set Enabled = 'No'.
    /// </summary>
    public async Task<int> ExecuteAsync(NpgsqlConnection connect, ExpertRuleApproval command, ILogWriter logWriter = null)
    {
        var sql = @"INSERT INTO expertruleapproval (ExpertRuleId, DateRecorded, RecordedBy, CodingStatusId, LastModifiedDate)
                    VALUES (@ExpertRuleId, now(), @RecordedBy, @CodingStatusId, now())
                    RETURNING Id";
        var id = await connect.QueryFirstAsync(sql, command);

        if (command.CodingStatusId == CodingStatusRejected)
        {
            logWriter?.LogInfo(
                $"Expert rule rejection: forcing Enabled=No for ExpertRuleId={command.ExpertRuleId}",
                nameof(AddExpertRuleApprovalCommand),
                nameof(ExecuteAsync));

            await connect.ExecuteAsync(
                "UPDATE expertrule SET enabled = 'No', lastmodifieddate = now() WHERE id = @ExpertRuleId",
                new { command.ExpertRuleId });
        }

        return (int)id.id;
    }
}
