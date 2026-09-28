using arc.common.Models.Reports;
using arc.data.Organisation;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Text;
using System.Threading.Tasks;

namespace arc.data.Reports;

/// <summary>
/// Query to fetch an approved reports list model by report history ID,
/// with optional organisation and laboratory filters.
/// </summary>
internal class ApprovedReportListByIdQuery : IQueryReturningType<ApprovedReportsListModel>
{
    /// <summary>
    /// Executes the query asynchronously against the given PostgreSQL connection.
    /// </summary>
    /// <param name="connect">An open NpgsqlConnection.</param>
    /// <param name="queryFilters">
    /// Configuration object providing filter values:
    /// must include "id", may include "organisationid" and "laboratoryid".
    /// </param>
    /// <returns>
    /// An <see cref="ApprovedReportsListModel"/> instance for the specified report ID.
    /// </returns>
    public async Task<ApprovedReportsListModel> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var id = queryFilters.GetIntegerValue("id");
        var queryFilterHasOrganisationId = queryFilters.TryParseIntegerValue("organisationid", out var organisationId) && organisationId > 0;
        var queryFilterHasLaboratoryId = queryFilters.TryParseIntegerValue("laboratoryid", out var laboratoryId) && laboratoryId > 0;

        var whereClause = new StringBuilder();

        if (queryFilterHasOrganisationId)
        {
            var organisationIds = await new OrganisationHierarchyListQuery().ExecuteAsync(connect, queryFilters);
            whereClause.AppendLine($"and s.OrganisationId in ({organisationIds})");
        }

        if (queryFilterHasLaboratoryId)
        {
            whereClause.AppendLine($" and s.LaboratoryId in ({laboratoryId}) ");
        }

        var sql = $"""
            select rh.id, s.accessionnumber, pa.surname, li.value as specimentype,og.organisationname,
            rh.approvaldate AT TIME ZONE 'UTC' As approvaldate,
            rh.approvedby, rh.requesteddate AT TIME ZONE 'UTC' As requesteddate,
            li2.value as approved, rh.reportapprovalid as stateid from reporthistory rh
            inner join specimen s on rh.specimenid = s.id
            inner join patient pa on s.patientid = pa.id
            inner join organisation og on s.organisationid = og.id
            inner join listitem li on s.specimentypeid = li.id
            inner join listitem li2 on li2.id = rh.reportapprovalid
            where rh.id = @Id {whereClause}
            """;

        var result = await connect.QueryFirstAsync<ApprovedReportsListModel>(sql, new { Id = id });
        return result;
    }
}
