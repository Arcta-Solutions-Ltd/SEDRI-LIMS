using arc.common.Models.Specimen;
using arc.data.Utils;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Specimen;

internal class GetCultureCommentsForSelectorQuery : IQueryReturningType<List<CommentListModel>>
{
    public async Task<List<CommentListModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var securityClause = QuerySecurity.GetWhereClause(queryFilters);
        var securityJoin = string.IsNullOrWhiteSpace(securityClause) ? string.Empty : $" and s.{securityClause}";
        var cultureId = queryFilters.GetIntegerValue("cultureid");

        var sql = $"""
                    select case when sp.comment is null then li.value else sp.comment end as comment, sp.id, sp.displayonreport, sp.lastmodifieddate, sp.addedby, lj.value as commenttype from specimencomment sp
                    inner join specimen s on s.id = sp.specimenid{securityJoin}
                    left outer join listitem li on li.id = sp.cannedcommentid
                    left outer join listitem lj on lj.id = sp.commenttypeid
                    where sp.cultureid = @CultureId order by sp.lastmodifieddate
                   """;

        var result = await connect.QueryAsync<CommentListModel>(sql, new { CultureId = cultureId });

        return result.ToList();
    }
}
