using arc.common.Models.Specimen;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Specimen;

internal class SpecimenApprovalQuery : IQueryReturningType<SpecimenApprovalModel>
{
    public async Task<SpecimenApprovalModel> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var specimenId = queryFilters.GetStringValue("specimenid");

        var submissionQuerySql = $"""
            select TRIM(CONCAT(u.firstname, ' ', u.lastname)) as SubmittedBy, (q.added at TIME ZONE 'UTC') as SubmittedDate from queue q
            inner join users u on u.username = q.username
            where q.eventid = 638 and q.specimenid = {specimenId}
            order by q.added desc
            """;

        var submissionInfo = await connect.QueryFirstOrDefaultAsync<SpecimenApprovalModel>(submissionQuerySql);

        var approvalQuerySql = $"""
            select TRIM(CONCAT(u.firstname, ' ', u.lastname)) as ApprovedBy, (q.added at TIME ZONE 'UTC') as ApprovedDate from queue q
            inner join users u on u.username = q.username
            where(q.eventid = 634 or q.eventid = 635) and q.stateid = 534 and q.specimenid = {specimenId}
            order by added desc
            """;

        var approvalInfo = await connect.QueryFirstOrDefaultAsync<SpecimenApprovalModel>(approvalQuerySql);

        return new SpecimenApprovalModel()
        {
            SubmittedBy = submissionInfo?.SubmittedBy ?? string.Empty,
            SubmittedDate = submissionInfo?.SubmittedDate,
            ApprovedBy = approvalInfo?.ApprovedBy ?? string.Empty,
            ApprovedDate = approvalInfo?.ApprovedDate,
        };
    }
}
