using arc.common.Models.Specimen;
using arc.data.Utils;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Specimen;

/// <summary>
/// Query to fetch specimen-level comments for selector UIs.
/// Returns user-entered or canned comment text plus metadata.
/// </summary>
internal class GetCommentsForSelectorQuery : IQueryReturningType<List<CommentListModel>>
{
    /// <summary>
    /// Executes the query to retrieve comments for a given specimen id.
    /// Applies security filtering based on <paramref name="queryFilters"/>.
    /// </summary>
    /// <param name="connect">Open database connection.</param>
    /// <param name="queryFilters">Filters containing parameter "specimenid" and security context.</param>
    /// <returns>List of <see cref="CommentListModel"/> rows for selector display.</returns>
    public async Task<List<CommentListModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var securityClause = QuerySecurity.GetWhereClause(queryFilters);
        var securityJoin = string.IsNullOrWhiteSpace(securityClause) ? string.Empty : $" and s.{securityClause}";
        var specimenId = queryFilters.GetIntegerValue("specimenid");

        var sql = $"""
                select case when sp.comment is null then li.value else sp.comment end as comment, sp.id, sp.displayonreport, sp.lastmodifieddate AT TIME ZONE 'UTC' As lastmodifieddate, sp.addedby, lj.value as commenttype from specimencomment sp
                inner join specimen s on s.id = sp.specimenid{securityJoin}
                left outer join listitem li on li.id = sp.cannedcommentid
                left outer join listitem lj on lj.id = sp.commenttypeid
                where sp.specimenid = @SpecimenId and sp.cultureid is null order by sp.id
               """;

        var result = await connect.QueryAsync<CommentListModel>(sql, new {SpecimenId = specimenId });

        return result.ToList();
    }
}
