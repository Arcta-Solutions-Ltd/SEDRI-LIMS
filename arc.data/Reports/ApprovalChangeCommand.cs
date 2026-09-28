using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Reports;

/// <summary>
/// Implements a command to change the approval status of a report history record.
/// </summary>
internal class ApprovalChangeCommand : ICommand
{
    /// <summary>
    /// Executes the approval status update against the <c>reporthistory</c> table,
    /// recording who made the decision and setting the approval date to the current UTC time.
    /// Executes the approval update against the <c>reporthistory</c> table.
    /// Updates <c>reportapprovalid</c> (<b>123</b> approved, <b>124</b> rejected/unapproved), <c>approvedby</c>,
    /// <c>lastmodifieddate</c>, and <c>approvaldate</c> for both outcomes. Adjusts <c>contents</c> JSONB only when
    /// approving (<b>123</b>) and eligible: upserts <c>ReportApprovedBy</c> and <c>ReportApprovedDate</c> on the
    /// top-level <c>Standard</c> array. For <b>124</b>, <c>contents</c> is unchanged. If <c>Standard</c> exists but is not
    /// a JSON array, <c>contents</c> is left unchanged on approve (columns still update).
    /// </summary>
    /// <param name="connect">
    /// An open <see cref="NpgsqlConnection"/> to the PostgreSQL database.
    /// </param>
    /// <param name="args">
    /// Command arguments in order:
    /// <list type="bullet">
    /// <item><description>args[0] – The report history record Id to update (parseable as <see cref="int"/>).</description></item>
    /// <item><description>args[1] – The decision string: <c>"Yes"</c> maps to state 123 (Approved); any other value maps to state 124 (Rejected).</description></item>
    /// <item><description>args[2] – The username of the person who approved or rejected the report. Written to <c>reporthistory.approvedby</c> and displayed as "Decision By" in the list view.</description></item>
    /// </list>
    /// </param>
    /// <returns>
    /// A task that completes with the Id of the updated record.
    /// </returns>
    public async Task<int> ExecuteAsync(NpgsqlConnection connect, params string[] args)
    {
        var id = int.Parse(args[0]);
        var approval = args[1] == "Yes" ? 123 : 124;
        var approvedBy = args[2];

        const string sql = """
            UPDATE reporthistory AS rh
               SET reportapprovalid = @ReportApprovalId,
                   approvedby = @ApprovedBy,
                   lastmodifieddate = now(),
                   approvaldate = now(),
                   contents = CASE
                     WHEN @ReportApprovalId = 123
                          AND (
                            rh.contents IS NULL
                            OR NOT (rh.contents ? 'Standard')
                            OR jsonb_typeof(rh.contents -> 'Standard') = 'array'
                          )
                     THEN jsonb_set(
                            COALESCE(rh.contents, '{}'::jsonb),
                            '{Standard}',
                            COALESCE(
                              (
                                SELECT jsonb_agg(st.elem ORDER BY st.ord)
                                  FROM jsonb_array_elements(
                                         COALESCE(rh.contents -> 'Standard', '[]'::jsonb)
                                       ) WITH ORDINALITY AS st (elem, ord)
                                 WHERE lower(trim(BOTH FROM coalesce(st.elem ->> 'Key', st.elem ->> 'key', '')))
                                       NOT IN ('reportapprovedby', 'reportapproveddate')
                              ),
                              '[]'::jsonb
                            )
                            || jsonb_build_array(
                                 jsonb_build_object(
                                   'Key', 'ReportApprovedBy',
                                   'Value',
                                   COALESCE(
                                     NULLIF(
                                       trim(BOTH FROM (
                                         SELECT concat_ws(
                                                  chr(32),
                                                  NULLIF(trim(BOTH FROM coalesce(u.firstname, '')), ''),
                                                  NULLIF(trim(BOTH FROM coalesce(u.lastname, '')), '')
                                                )
                                           FROM users u
                                          WHERE lower(u.username) = lower(trim(COALESCE(@ApprovedBy, '')))
                                          LIMIT 1
                                       )),
                                       ''
                                     ),
                                     NULLIF(trim(BOTH FROM COALESCE(@ApprovedBy, '')), ''),
                                     ''
                                   )
                                 ),
                                 jsonb_build_object(
                                   'Key', 'ReportApprovedDate',
                                   'Value',
                                   to_char(
                                     clock_timestamp() AT TIME ZONE 'UTC',
                                     'YYYY-MM-DD"T"HH24:MI:SS.MS"Z"'
                                   )
                                 )
                               ),
                            true
                          )
                     ELSE rh.contents
                   END
             WHERE rh.id = @Id
            """;

        await connect.ExecuteAsync(sql, new { ReportApprovalId = approval, ApprovedBy = approvedBy, Id = id });

        return id;
    }
}
