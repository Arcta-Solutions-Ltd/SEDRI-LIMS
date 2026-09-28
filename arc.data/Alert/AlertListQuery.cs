using arc.common.Models.Alert;
using arc.data.Extensions;
using arc.data.Utils;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arc.data.Alert;

/// <summary>
/// Data query that returns the alert list for the alert list view. Supports filtering by alert type and specification.
/// </summary>
internal class AlertListQuery : IQueryReturningType<List<AlertListModel>>
{
    public async Task<List<AlertListModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var queryFilterHasAlertTypeId = queryFilters.TryGetStringValue("alerttypeid", out var alertTypeIdCsv);
        var queryFilterHasSpecificationId = queryFilters.TryGetStringValue("specificationid", out var specificationIdCsv);
        var queryFilterHasSearchText = queryFilters.TryGetStringValue("searchtext", out var wildcardSearchText);

        var whereClauseSql = new StringBuilder("where ");

        if (queryFilterHasSearchText)
        {
            wildcardSearchText = wildcardSearchText.Trim().ToSqlWildcard();
            var orgWhere = OrganismSearchWhereClauseUtil.GenerateWhereClause(wildcardSearchText, true, false);
            whereClauseSql.AppendLine($"""
                (
                    al.AlertName iLike '{wildcardSearchText}'
                    or {orgWhere}
                    or at.name ilike '{wildcardSearchText}'
                    or specdisplay.value ilike '{wildcardSearchText}'
                    or li3.value ilike '{wildcardSearchText}'
                )
                """);

        }

        if (queryFilterHasAlertTypeId)
        {
            whereClauseSql.AppendLine($"and al.alerttypeid in ({alertTypeIdCsv})");
        }

        if (queryFilterHasSpecificationId)
        {
            whereClauseSql.AppendLine($"and al.specificationid in ({specificationIdCsv})");
        }

        if(whereClauseSql.Equals("where "))
        {
            whereClauseSql = new StringBuilder();
        }

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
                al.enabled,
                at.name as alerttype,
                specdisplay.value as specification,
            	Case When ad.familyid is NULL or ad.familyid = 0 Then ord.name Else orda.name End as ordername,
            	Case When ad.familyid is NULL or ad.familyid = 0 Then f.name Else fa.name End as familyname,
                li3.value as orggroupname
            from alert al
                left outer join organism o on o.id = al.organismId
                left outer join organismsynonyms os on o.Id = os.organismId and os.PreferredName = true
                left outer join alerttype at on at.id = al.alerttypeid
                left outer join specdisplay on specdisplay.id = al.specificationid
                left outer join ordercat ord on al.orderid = ord.id 
                left outer join family f on al.familyid = f.id
                left outer join genus g on g.Id = o.genusId
                left outer join species s on s.Id = o.speciesId
                left outer join subspecies ss on ss.id = o.subspeciesId
                left outer join serotype se on se.Id = o.serotypeId
                left outer join additional ad on at.Id = o.additionalId
                left outer join family fa on ad.familyid = fa.id
                left outer join ordercat orda on fa.orderid = orda.id
                left outer join listitem li3 on al.orggroupcodingid = li3.id
            {whereClauseSql}
            order by 
                {ListQueryOrderByUtil.GetOrderByClause(queryFilters, "al.Alertname")}
            limit 500
            """;

        var result = await connect.QueryAsync<AlertListModel>(sql);

        return result.ToList();
    }
}
