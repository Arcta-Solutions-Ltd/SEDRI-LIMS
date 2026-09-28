using arc.common.Models.Coding;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Coding;

/// <summary>
/// Data query that loads a single expert rule by id for the expert rule record view.
/// Returns text for all list-item fields. Multiselect specimen types are returned as comma-separated strings.
/// </summary>
internal class ExpertRuleViewByIdQuery : IQueryReturningType<ExpertRuleViewModel>
{
    /// <summary>
    /// Executes the expert rule-by-id query and returns the result as a single <see cref="ExpertRuleViewModel"/>.
    /// </summary>
    /// <param name="connect">Active database connection.</param>
    /// <param name="queryFilters">Filter config; expects an integer "id" for the expert rule.</param>
    /// <returns>The expert rule row mapped to <see cref="ExpertRuleViewModel"/>, or throws if not found.</returns>
    public async Task<ExpertRuleViewModel> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
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
                            and ad.name is null then ' spp.'
                            else ''
                        end,
                        ' ',
                        s.name,
                        ' ',
                        ss.name,
                        ' ',
                        TRIM(se.name),
                        ad.name
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
            ),
            specimentypeinclude as (
                select ert.expertruleid,
                    STRING_AGG(li.value, ', ' order by li.value) as value
                from expertrulespecimentype ert
                inner join listitem li on li.id = ert.specimentypeid
                where ert.included = true
                group by ert.expertruleid
            ),
            specimentypeexclude as (
                select ert.expertruleid,
                    STRING_AGG(li.value, ', ' order by li.value) as value
                from expertrulespecimentype ert
                inner join listitem li on li.id = ert.specimentypeid
                where ert.included = false
                group by ert.expertruleid
            )
            select
                {organismDescriptionSql},
                e.Id,
                e.ExpertRuleName,
                e.RuleText,
                e.CombinationRule,
                e.Enabled,
                e.AlertOnRule,
                e.TagId,
                specdisplay.value as specification,
                o.name as ordername,
                f.name as familyname,
                li2.value as orggroupname,
                sti.value as specimentypestoinclude,
                ste.value as specimentypestoexclude
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
                left outer join additional ad on ad.Id = og.additionalId
                left outer join listitem li2 on e.orggroupcodingid = li2.id
                left outer join specimentypeinclude sti on sti.expertruleid = e.id
                left outer join specimentypeexclude ste on ste.expertruleid = e.id
            where
                e.Id = @ruleId
            """;

        return await connect.QueryFirstAsync<ExpertRuleViewModel>(sql, new { ruleId });
    }
}
