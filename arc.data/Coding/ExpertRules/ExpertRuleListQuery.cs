using arc.common.Models.Coding;
using arc.data.Extensions;
using arc.data.Utils;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arc.data.Coding;

internal class ExpertRuleListQuery : IQueryReturningType<List<ExpertRuleListModel>>
{
    /// <summary>
    /// Loads the expert rule list with optional filters. Returns rows for the expert rules list screen.
    /// </summary>
    /// <param name="connect">Active database connection.</param>
    /// <param name="queryFilters">Optional filters: "searchtext", "specificationid" (comma-separated).</param>
    /// <returns>List of expert rule rows (max 500).</returns>
    public async Task<List<ExpertRuleListModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var queryFilterHasSpecificationId = queryFilters.TryGetStringValue("specificationid", out var specificationIdCsv);
        var searchRaw = queryFilters.GetStringValue("searchtext") ?? string.Empty;
        var wildcardSearchText = searchRaw.Trim().ToLower().ToSqlWildcard();

        var whereClauseSql = new StringBuilder();

        if (queryFilterHasSpecificationId)
        {
            whereClauseSql.AppendLine($"and e.specificationid in ({specificationIdCsv})");
        }

        var organismDescriptionSql = """
            case
                when os.synonym is NULL then TRIM(
                    CONCAT (
                        g.name,
                        case
                            when g.name is not null
                            and s.name is null
                            then ' spp.'
                            else ''
                        end,
                        ' ',
                        s.name,
                        ' ',
                        ss.name,
                        ' ',
                        TRIM(se.name)
                    )
                )
                else os.synonym
            end as organismname
            """;

        var sql = $"""
            with specdisplay as (
                select s.id,
                    TRIM(CONCAT(
                        COALESCE(l1.value, ''),
                        ' - ',
                        COALESCE(l2.value, ''),
                        CASE WHEN l3.value IS NOT NULL OR l4.value IS NOT NULL
                            THEN CONCAT(' (', COALESCE(l3.value, ''), CASE WHEN l3.value IS NOT NULL AND l4.value IS NOT NULL THEN ', ' ELSE '' END, COALESCE(l4.value, ''), ')')
                            ELSE ''
                        END
                    )) as value
                from specification s
                left outer join listitem l1 on l1.id = s.guidelinesid
                left outer join listitem l2 on l2.id = s.documentid
                left outer join listitem l3 on l3.id = s.versionnumberid
                left outer join listitem l4 on l4.id = s.publicationyearid
            )
            select 
                {organismDescriptionSql},
                e.Id,
                e.ExpertRuleName,
                e.RuleText,
                e.Enabled,
                specdisplay.value as specification,
                o.name as ordername,
                f.name as familyname,
                li2.value as orggroupname
            from expertrule e
                left outer join specdisplay on specdisplay.id = e.specificationid
                left outer join organism og on og.id = e.organismId
                left outer join organismsynonyms os on og.Id = os.organismId and os.PreferredName = true
                left outer join ordercat o on e.orderid = o.id 
                left outer join family f on e.familyid = f.id
                left outer join genus g on g.Id = og.genusId
                left outer join species s on s.Id = og.speciesId
                left outer join subspecies ss on ss.id = og.subspeciesId
                left outer join serotype se on se.Id = og.serotypeId
                left outer join listitem li2 on e.orggroupcodingid = li2.id
            where 
                (
                    e.ExpertRuleName ilike @wildcardSearchText
                    or e.RuleText ilike @wildcardSearchText
                    or COALESCE(e.Enabled::text, '') ilike @wildcardSearchText
                    or specdisplay.value ilike @wildcardSearchText
                    or o.name ilike @wildcardSearchText
                    or f.name ilike @wildcardSearchText
                    or li2.value ilike @wildcardSearchText
                )
            {whereClauseSql}
            order by 
                {ListQueryOrderByUtil.GetOrderByClause(queryFilters, "e.ExpertRuleName")}
            limit 500
            """;

        var result = await connect.QueryAsync<ExpertRuleListModel>(sql, new { wildcardSearchText });

        return result.ToList();
    }
}
