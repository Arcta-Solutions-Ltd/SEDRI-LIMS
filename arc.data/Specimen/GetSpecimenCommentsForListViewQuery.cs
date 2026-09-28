using arc.common.Models.Specimen;
using arc.data.Utils;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.data.Specimen;

internal class GetSpecimenCommentsForListViewQuery : IQueryReturningType<IEnumerable<CommentListModel>>
{
    public async Task<IEnumerable<CommentListModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var securityClause = QuerySecurity.GetWhereClause(queryFilters);

        var sql = $"""
                    select case when sp.comment is null then li.value else sp.comment end, sp.id, sp.displayonreport, sp.lastmodifieddate AT TIME ZONE 'UTC' As lastmodifieddate, sp.addedby, lj.value from specimencomment sp
                    inner join specimen s on s.id = sp.specimenid and s.{securityClause}
                    left outer join listitem li on li.id = sp.cannedcommentid
                    left outer join listitem lj on lj.id = sp.commenttypeid
                    where sp.specimenid = @SpecimenId and sp.cultureid is null order by sp.lastmodifieddate
                   """;

        return await connect.QueryAsync<CommentListModel>(sql, new { SpecimenId = int.Parse(queryFilters.Parameters[0].Value) });

    }
}
