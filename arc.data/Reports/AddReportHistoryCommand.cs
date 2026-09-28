using arc.app.Common;
using arc.domain.Reports;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Reports;

/// <summary>
/// Represents the command to insert a report history record and return its generated identifier.
/// </summary>
internal class AddReportHistoryCommand : ICommandWithTypeReturningInteger<ReportHistory>
{
    /// <summary>
    /// Executes the insert operation asynchronously to persist the report history and retrieve its ID.
    /// </summary>
    /// <param name="connect">The NpgsqlConnection for database access.</param>
    /// <param name="command">The ReportHistory object containing data to be saved.</param>
    /// <param name="logWriter">The logger for recording operation details.</param>
    /// <returns>A task that resolves to the newly generated report history identifier.</returns>
    public async Task<int> ExecuteAsync(NpgsqlConnection connect, ReportHistory command, ILogWriter logWriter)
    {
        command.Contents = command.Contents.Replace("\r\n", "");
        var sql = @"insert into ReportHistory(specimenid, reportid, contents, lastmodifieddate, reportconfig, name, PrintStatusId, reportApprovalId, requesteddate)  
                        values(@specimenid, @reportid, to_json(@contents::jsonb), now(), @reportconfig, @name, @printstatusid, @ReportApprovalId, now()) returning id";
        if (command.IncludeApprovalInfo)
        {
            sql = @"insert into ReportHistory(specimenid, reportid, contents, lastmodifieddate, reportconfig, name, PrintStatusId, reportApprovalId, requesteddate, approvaldate, approvedby)  
                        values(@specimenid, @reportid, to_json(@contents::jsonb), now(), @reportconfig, @name, @printstatusid, @ReportApprovalId, now(), now(), @Username) returning id";
        }
        var id = await connect.QueryFirstAsync(sql, command);
        var historyId = (int)id.id;

        return historyId;
    }
}
