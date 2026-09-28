using arc.common.ExtensionMethods;
using arc.common.Models.Specimen;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arc.data.Specimen;

/// <summary>
/// Query to retrieve results for the Batched Specimen Reports list view.
/// All user input is sanitized to prevent SQL injection attacks.
/// </summary>
internal class SpecimenBatchListQuery : IQueryReturningType<List<SpecimenBatchListModel>>
{
    private const int _published = 869;
    private const int _notPublished = 870;
    private const int _printed = 871;

    /// <summary>
    /// Executes the query to get all the data from a table.
    /// </summary>
    /// <param name="connect">Database connection.</param>
    /// <param name="queryFilters">Query filters for; reportid, laboratoryid, specimentypeid, stateid (csv), organisationid, printstatusid (csv), startdate, enddate and orderby.</param>
    /// <returns>List of specimen batch records.</returns>
    public async Task<List<SpecimenBatchListModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        _ = queryFilters.TryParseIntegerValue("reportid", out var reportId, 1);

        var laboratoryId = queryFilters.GetIntegerValue("laboratoryid");
        var queryFilterHasSpecimenTypeId = queryFilters.TryGetStringValue("specimentypeid", out var specimenTypeId);
        var queryFilterHasStateId = queryFilters.TryGetStringValue("stateid", out var stateId);
        var queryFilterHasOrganisationId = queryFilters.TryGetStringValue("organisationid", out var organisationIdsCsv);
        var queryFilterHasPrintStatusIds = queryFilters.TryGetStringValue("printstatusid", out var printStatusIdsCsv);
        var queryFilterHasStartDate = queryFilters.TryParseDateValue("startdate", out var startDate);
        var queryFilterHasEndDate = queryFilters.TryParseDateValue("endDate", out var endDate);

        var whereClause = new StringBuilder();

        if (queryFilterHasSpecimenTypeId)
        {
            // Sanitize ID list to only allow valid integers
            var sanitizedSpecimenTypeId = SqlSanitizer.SanitizeIdList(specimenTypeId);
            whereClause.AppendLine($" and s.SpecimenTypeId in ({sanitizedSpecimenTypeId}) ");
        }

        if (queryFilterHasStateId)
        {
            // Sanitize ID list to only allow valid integers
            var sanitizedStateId = SqlSanitizer.SanitizeIdList(stateId);
            whereClause.AppendLine($"and s.StateId in ({sanitizedStateId})");
        }

        if (queryFilterHasOrganisationId)
        {
            // Sanitize ID list to only allow valid integers
            var sanitizedOrganisationIds = SqlSanitizer.SanitizeIdList(organisationIdsCsv);
            whereClause.AppendLine($"and s.OrganisationId in ({sanitizedOrganisationIds})");
        }

        if (queryFilterHasStartDate)
        {
            whereClause.AppendLine("and s.ReceivedDate >= @startDate");
        }

        if (queryFilterHasEndDate)
        {
            whereClause.AppendLine("and s.ReceivedDate <= @endDate");
        }

        if (queryFilterHasPrintStatusIds)
        {
            // Sanitize and validate the print status IDs
            var sanitizedPrintStatusIds = SqlSanitizer.SanitizeIdList(printStatusIdsCsv);

            // Parse the sanitized IDs for logic checks
            var printStatusIds = sanitizedPrintStatusIds.Split(",")
                .Where(s => int.TryParse(s, out _))
                .Select(int.Parse);

            if (printStatusIds.Contains(_published) || printStatusIds.Contains(_printed))
            {
                whereClause.AppendLine($"and e.PrintStatusId in ({sanitizedPrintStatusIds})");
            }
            else if (printStatusIds.Contains(_notPublished))
            {
                whereClause.AppendLine("and e.PrintStatusId is null");
            }
        }

        var order = "accessionnumber desc";
        if (!string.IsNullOrWhiteSpace(queryFilters.OrderBy))
        {
            var desc = queryFilters.OrderDescending ? " desc" : "";
            order = queryFilters.OrderBy + desc;
        }

        var sql = $"""
            with historylist as
            (
                select
                    SpecimenId,
                    max(PrintStatusId) As PrintStatusId
                from
                    reporthistory
                where
                    ReportId = @reportId
                group by
                    SpecimenId
            )
            select
                a.SurName,
                c.Value As SpecimenType,
                d.Value As State,
                s.Id,
                s.StateId,
                s.AccessionNumber,
                b.OrganisationName,
                s.SpecimenTypeId,
                s.OrganisationId,
                case
                    when e.PrintStatusId is null then 'No'
                    Else 'Yes'
                end as Published,
                sh.LastModifiedDate As DateFinalised,
                case
                    when e.PrintStatusId is null then 'No'
                    Else case
                        when e.PrintStatusId = 869 then 'No'
                        Else 'Yes'
                    end
                end as Printed
            from
                Specimen s
                inner join Patient a on a.Id = s.PatientId
                inner join organisation b on b.Id = s.OrganisationId
                left outer join ListItem c on c.Id = s.SpecimenTypeId
                left outer join ListItem d on d.Id = s.StateId
                left outer join HistoryList e on e.SpecimenId = s.Id
                left outer join SpecimenStateHistory sh on sh.SpecimenId = s.Id
                and sh.StateId = 534
            where
                s.stateid = 534
                and s.LaboratoryId = @laboratoryId
                {whereClause}
                order by {order}
            """;

        var result = await connect.QueryAsync<SpecimenBatchListModel>(sql, new { laboratoryId, reportId, startDate, endDate });
        return result.ToList();
    }
}
