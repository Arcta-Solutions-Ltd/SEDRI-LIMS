using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Newtonsoft.Json;
using Npgsql;
using System.Text;
using System.Threading.Tasks;

namespace arc.data.Specimen;

internal class GetReportCommentsQuery : IQueryReturningString
{
    public async Task<string> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var specimenId = queryFilters.GetIntegerValue("id");
        var queryFiltersHasDisplayOnReport = queryFilters.TryGetStringValue("displayonreport", out var _);

        var whereClause = new StringBuilder();

        if (queryFiltersHasDisplayOnReport)
        {
            whereClause.AppendLine("and s.displayonreport = 'Yes'");
        }

        var sql = $"""
            select
                case
                    when s.comment is null then l.value
                    else s.comment
                end,
                s.id,
                s.specimenid,
                s.commenttypeid,
                s.cultureid,
                s.displayonreport,
                li.value
            from
                specimencomment s
                left outer join listitem l on s.cannedcommentid = l.id
                left outer join listitem li on s.commenttypeid = li.id
            where
                s.specimenid = @specimenId
                {whereClause}
            """;

        var result = await connect.QueryAsync(sql, new { specimenId });

        return JsonConvert.SerializeObject(result);
    }
}
