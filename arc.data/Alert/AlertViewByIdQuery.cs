using arc.common.Models.Alert;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Alert;

/// <summary>
/// Data query that loads a single alert by id for the alert record view.
/// Returns text for all list-item fields (Source, OrgGroup, SusceptibilityAndOr, TestAndOr).
/// </summary>
internal class AlertViewByIdQuery : IQueryReturningType<AlertViewModel>
{
    public async Task<AlertViewModel> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var alertId = queryFilters.GetIntegerValue("id");

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
            )
            select
                {organismDescriptionSql},
                al.Id,
                al.AlertName,
                al.AlertMessage,
                al.enabled,
                at.name as alerttype,
                specdisplay.value as specification,
                o.name as ordername,
                f.name as familyname,
                li3.value as orggroupname,
                li4.value as susceptibilityandor,
                li5.value as testandor
            from
                alert al
                left outer join organism og on og.id = al.organismId
                left outer join organismsynonyms os on og.Id = os.organismId
                and os.PreferredName = true
                left outer join alerttype at on at.id = al.alerttypeid
                left outer join specdisplay on specdisplay.id = al.specificationid
                left outer join ordercat o on al.orderid = o.id
                left outer join family f on al.familyid = f.id
                left outer join genus g on g.Id = og.genusId
                left outer join species s on s.Id = og.speciesId
                left outer join subspecies ss on ss.id = og.subspeciesId
                left outer join serotype se on se.Id = og.serotypeId
                left outer join additional ad on ad.Id = og.additionalId
                left outer join listitem li3 on al.orggroupcodingid = li3.id
                left outer join listitem li4 on li4.id::text = al.susceptibilityandor
                left outer join listitem li5 on li5.id::text = al.testandor
            where
                al.Id = @alertId
            """;

        return await connect.QueryFirstAsync<AlertViewModel>(sql, new { alertId });
    }
}
