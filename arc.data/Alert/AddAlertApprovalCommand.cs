using arc.app.Common;
using arc.domain.Alert;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Alert;

/// <summary>
/// Command to insert a new alert approval record.
/// When the status is Rejected, also sets the parent alert's Enabled to 'No'.
/// </summary>
internal class AddAlertApprovalCommand : ICommandWithTypeReturningInteger<AlertApproval>
{
    private const int CodingStatusRejected = 146;

    /// <summary>
    /// Executes the insert operation for an alert approval.
    /// When CodingStatusId is Rejected (146), updates the alert to set Enabled = 'No'.
    /// </summary>
    public async Task<int> ExecuteAsync(NpgsqlConnection connect, AlertApproval command, ILogWriter logWriter = null)
    {
        var sql = @"INSERT INTO alertapproval (AlertId, DateRecorded, RecordedBy, CodingStatusId, LastModifiedDate)
                    VALUES (@AlertId, now(), @RecordedBy, @CodingStatusId, now())
                    RETURNING Id";
        var id = await connect.QueryFirstAsync(sql, command);

        if (command.CodingStatusId == CodingStatusRejected)
        {
            await connect.ExecuteAsync(
                "UPDATE Alert SET enabled = 'No', lastmodifieddate = now() WHERE id = @AlertId",
                new { command.AlertId });
        }

        return (int)id.id;
    }
}
