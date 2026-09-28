using arc.common.Models.Laboratory;
using arc.data.Extensions;
using arc.data.Utils;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Security;

/// <summary>
/// Represents a query to retrieve a list of laboratories.
/// Resolves CodingListId and AntibioticGroupIds to comma-separated display names for list display.
/// </summary>
internal class LaboratoryListQuery : IQueryReturningType<List<LaboratoryListModel>>
{
    /// <summary>
    /// Executes the query asynchronously to retrieve a list of laboratories based on the provided query filters.
    /// Returns CodingListId and AntibioticGroupIds as resolved display names.
    /// </summary>
    /// <param name="connect">The NpgsqlConnection to the database.</param>
    /// <param name="queryFilters">The configuration for the query filters.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of LaboratoryListModel.</returns>
    public async Task<List<LaboratoryListModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var startsWithLabName = queryFilters.GetStringValue("laboratoryname").ToSqlStartsWith();

        var sql = @"with
                        lablist as (select l.id, l.laboratoryname, l.antibioticgroupids, unnest(string_to_array(codinglistid, ',')) as codinglist, li.value As language from laboratory l 
			            left outer join listitem li on li.id = l.languageid),
                        lablistagg as (select ll.id, ll.laboratoryname, ll.language, ll.antibioticgroupids,
                            string_agg(li.value, ',') as codinglistid from lablist ll
                            left outer join listitem li on li.id = cast(ll.codinglist As int)
                            group by ll.id, ll.laboratoryname, ll.language, ll.antibioticgroupids),
                        labantilist as (select lla.id, lla.laboratoryname, lla.language, lla.codinglistid, antiblist.id as antibid
                            from lablistagg lla
                            left join lateral unnest(coalesce(string_to_array(nullif(trim(lla.antibioticgroupids), ''), ','), array[]::text[])) as antiblist(id) on true),
                        labantilistagg as (select ll2.id, ll2.laboratoryname, ll2.language, ll2.codinglistid,
                            string_agg(li2.value, ',') as antibioticgroupids from labantilist ll2
                            left outer join listitem li2 on li2.id = nullif(trim(ll2.antibid), '')::int
                            group by ll2.id, ll2.laboratoryname, ll2.language, ll2.codinglistid)
                        select ll3.id, ll3.laboratoryname, ll3.language, ll3.codinglistid, ll3.antibioticgroupids from labantilistagg ll3
                        where ll3.laboratoryname ilike @startsWithLabName or ll3.language ilike @startsWithLabName
                        union (select l.id, l.laboratoryname, li.value as language, coalesce(l.codinglistid, '') as codinglistid,
                            coalesce((select string_agg(li3.value, ',') from listitem li3
                                join unnest(coalesce(string_to_array(nullif(trim(l.antibioticgroupids), ''), ','), array[]::text[])) as aid(id) on li3.id = aid.id::int), '') as antibioticgroupids
                            from laboratory l left outer join listitem li on li.id = l.languageid
                            where (l.codinglistid = '' or l.codinglistid is null))
                        order by " + ListQueryOrderByUtil.GetOrderByClause(queryFilters, "laboratoryname");

        var result = await connect.QueryAsync<LaboratoryListModel>(sql, new { startsWithLabName });

        return result.ToList();
    }
}
