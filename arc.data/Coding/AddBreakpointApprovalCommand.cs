using arc.app.Common;
using arc.domain.Coding;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Coding;

/// <summary>
/// Command to insert a new breakpoint approval record.
/// When the status is Rejected, also sets the parent breakpoint's Enabled to 'No'.
/// </summary>
internal class AddBreakpointApprovalCommand : ICommandWithTypeReturningInteger<BreakpointApproval>
{
    private const int CodingStatusRejected = 146;

    /// <summary>
    /// Executes the insert operation for a breakpoint approval.
    /// When CodingStatusId is Rejected (146), updates the breakpoint to set Enabled = 'No'.
    /// </summary>
    public async Task<int> ExecuteAsync(NpgsqlConnection connect, BreakpointApproval command, ILogWriter logWriter = null)
    {
        var sql = @"INSERT INTO breakpointapproval (BreakpointId, DateRecorded, RecordedBy, CodingStatusId, LastModifiedDate)
                    VALUES (@BreakpointId, now(), @RecordedBy, @CodingStatusId, now())
                    RETURNING Id";
        var id = await connect.QueryFirstAsync(sql, command);

        if (command.CodingStatusId == CodingStatusRejected)
        {
            logWriter?.LogInfo(
                $"Breakpoint rejection: forcing Enabled=No for BreakpointId={command.BreakpointId}",
                nameof(AddBreakpointApprovalCommand),
                nameof(ExecuteAsync));

            await connect.ExecuteAsync(
                "UPDATE breakpoint SET enabled = 'No', lastmodifieddate = now() WHERE id = @BreakpointId",
                new { command.BreakpointId });
        }

        return (int)id.id;
    }
}
