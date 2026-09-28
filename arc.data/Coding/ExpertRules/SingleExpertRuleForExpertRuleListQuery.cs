using arc.common.Models.Coding;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Coding;

internal class SingleExpertRuleForExpertRuleListQuery : IQueryReturningType<ExpertRuleListModel>
{
    /// <summary>
    /// Loads a single expert rule by ID for the list view (e.g. when selecting a row).
    /// </summary>
    /// <param name="connect">Active database connection.</param>
    /// <param name="queryFilters">Must contain "id" with the expert rule ID.</param>
    /// <returns>The expert rule row with specification display text.</returns>
    public async Task<ExpertRuleListModel> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var ruleId = queryFilters.GetIntegerValue("id");

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
                li2.value As orggroupname
            from
                expertrule e
                left outer join specdisplay on specdisplay.id = e.specificationid
                left outer join organism og on og.id = e.organismId
                left outer join organismsynonyms os on og.Id = os.organismId
                and os.PreferredName = true
                left outer join ordercat o on e.orderid = o.id
                left outer join family f on e.familyid = f.id
                left outer join genus g on g.Id = og.genusId
                left outer join species s on s.Id = og.speciesId
                left outer join subspecies ss on ss.id = og.subspeciesId
                left outer join serotype se on se.Id = og.serotypeId
                left outer join listitem li2 on e.orggroupcodingid = li2.id
            where
                e.Id = @ruleId
            """;

        var result = await connect.QueryFirstAsync<ExpertRuleListModel>(sql, new { ruleId });

        return result;
    }
}
