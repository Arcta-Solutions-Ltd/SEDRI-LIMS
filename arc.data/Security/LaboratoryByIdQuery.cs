using arc.common.Models.Laboratory;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Security;

/// <summary>
/// Represents a query that retrieves a laboratory and its coding list by identifier.
/// Used for the list view single-item display (e.g. SingleLaboratoryForLaboratoryList).
/// Resolves CodingListId and AntibioticGroupIds to display names for display in the UI.
/// </summary>
internal class LaboratoryByIdQuery : IQueryReturningType<LaboratoryListModel>
{
    /// <summary>
    /// Executes the SQL query asynchronously to fetch a laboratory record by ID.
    /// Returns CodingListId and AntibioticGroupIds as comma-separated display names.
    /// </summary>
    /// <param name="connect">
    /// The NpgsqlConnection used to execute the query against PostgreSQL.
    /// </param>
    /// <param name="queryFilters">
    /// The configuration containing query parameters; expects an "id" entry.
    /// </param>
    /// <returns>
    /// A task that resolves to a LaboratoryListModel populated from the database.
    /// </returns>
    public async Task<LaboratoryListModel> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var id = queryFilters.Parameters.Where(p => p.Key.ToLower() == "id").First();

        var sql = @"with
                        lablist as (select l.id, l.laboratoryname, l.approvereports, l.antibioticgroupids,
                            unnest(string_to_array(nullif(trim(codinglistid), ''), ',')) as codinglist, li.value As language
                            from laboratory l left outer join listitem li on li.id = l.languageid where l.id = @Id),
                        lablistagg as (select ll.id, ll.laboratoryname, ll.language, ll.approvereports, ll.antibioticgroupids,
                            string_agg(li.value, ',') as codinglistid from lablist ll
                            left outer join listitem li on li.id = cast(nullif(trim(ll.codinglist), '') As int)
                            group by ll.id, ll.laboratoryname, ll.language, ll.approvereports, ll.antibioticgroupids),
                        labantilist as (select lla.id, lla.laboratoryname, lla.language, lla.codinglistid, lla.approvereports, antiblist.id as antibid
                            from lablistagg lla
                            left join lateral unnest(coalesce(string_to_array(nullif(trim(lla.antibioticgroupids), ''), ','), array[]::text[])) as antiblist(id) on true),
                        labantilistagg as (select ll2.id, ll2.laboratoryname, ll2.language, ll2.codinglistid, ll2.approvereports,
                            string_agg(li2.value, ',') as antibioticgroupids from labantilist ll2
                            left outer join listitem li2 on li2.id = nullif(trim(ll2.antibid), '')::int
                            group by ll2.id, ll2.laboratoryname, ll2.language, ll2.codinglistid, ll2.approvereports)
                        select id, laboratoryname, language, codinglistid, approvereports, antibioticgroupids from labantilistagg
                        union
                        (select l.id, l.laboratoryname, li.value as language, coalesce(l.codinglistid, '') as codinglistid, l.approvereports,
                            coalesce((select string_agg(li3.value, ',') from listitem li3
                                join unnest(coalesce(string_to_array(nullif(trim(l.antibioticgroupids), ''), ','), array[]::text[])) as aid(id) on li3.id = aid.id::int), '') as antibioticgroupids
                            from laboratory l left outer join listitem li on li.id = l.languageid
                            where (l.codinglistid = '' or l.codinglistid is null) and l.id = @Id)
                        order by laboratoryname";

        return await connect.QueryFirstAsync<LaboratoryListModel>(
            sql,
            new { Id = int.Parse(id.Value) }
        );
    }
}
