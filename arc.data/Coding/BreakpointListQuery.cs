using arc.common.ExtensionMethods;
using arc.common.Models.Coding;
using arc.data.Extensions;
using arc.data.Utils;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Coding;

/// <summary>
/// Data query that loads the breakpoint list with optional filters. Returns a list of
/// <see cref="BreakpointListModel"/> for the breakpoint list screen.
/// </summary>
/// <remarks>
/// Supports filtering by antibiotic name (wildcard/text), organism id, host id, test method id, and specification id.
/// Organism display name uses preferred synonym when set; otherwise genus/species/subspecies/serotype/additional.
/// Results are ordered per query filters and limited to 500 rows.
/// </remarks>
internal class BreakpointListQuery : IQueryReturningType<List<BreakpointListModel>>
{
    /// <summary>
    /// Executes the breakpoint list query with the given filters and returns matching breakpoints.
    /// </summary>
    /// <param name="connect">Active database connection.</param>
    /// <param name="queryFilters">Optional filters: "antibioticname", "organismid", "hostid", "testmethodid", "specificationid".</param>
    /// <returns>List of breakpoint rows mapped to <see cref="BreakpointListModel"/> (max 500).</returns>
    public async Task<List<BreakpointListModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var queryFilterHasTestPatternName = queryFilters.TryGetStringValue("antibioticname", out var wildcardAntibioticName);

        var queryFilterHasOrganismId = queryFilters.TryGetStringValue("organismid", out var organismId, "");
        var queryFilterHasHostId = queryFilters.TryGetStringValue("hostid", out var hostId, "");
        var queryFilterHasTestMethodId = queryFilters.TryGetStringValue("testmethodid", out var testMethodId, "");
        var queryFilterHasSpecificationId = queryFilters.TryGetStringValue("specificationid", out var specificationId, "");

        var organismIds = organismId.ToIntList().ToArray();
        var specificationIds = specificationId.ToIntList().ToArray();
        var hostIds = hostId.ToIntList().ToArray();
        var testMethodIds = testMethodId.ToIntList().ToArray();

        var whereClauseSql = "";

        if (queryFilterHasTestPatternName)
        {
            wildcardAntibioticName = wildcardAntibioticName.Trim();
            var orgWhere = OrganismSearchWhereClauseUtil.GenerateWhereClause(wildcardAntibioticName.ToSqlWildcard(), true, false);
            whereClauseSql = whereClauseSql + $"""
                (
                    a.antibioticname iLike '{wildcardAntibioticName.ToSqlStartsWith()}'
                    or {orgWhere}
                    or li1.value ilike '{wildcardAntibioticName.ToSqlStartsWith()}'
                    or specdisplay.value ilike '{wildcardAntibioticName.ToSqlStartsWith()}'
                    or li4.value ilike '{wildcardAntibioticName.ToSqlStartsWith()}'
                    or li5.value ilike '{wildcardAntibioticName.ToSqlStartsWith()}'
                    or b.dosage ilike '{wildcardAntibioticName.ToSqlStartsWith()}'
                )
                """ ;

        }

        if (queryFilterHasOrganismId && organismIds.Length != 0)
        {
            whereClauseSql = whereClauseSql.ConcatWithBlankStringCheck(" b.organismid = ANY(@organismIds)", " and b.organismid = ANY(@organismIds)");
        }

        if (queryFilterHasTestMethodId && testMethodIds.Length != 0)
        {
            whereClauseSql = whereClauseSql.ConcatWithBlankStringCheck(" li1.id = ANY(@testMethodIds)", " and li1.id  = ANY(@testMethodIds)");
        }

        if (queryFilterHasHostId && hostIds.Length != 0)
        {
            whereClauseSql = whereClauseSql.ConcatWithBlankStringCheck(" li2.id = ANY(@hostIds)", " and li2.id = ANY(@hostIds)");
        }

        if (queryFilterHasSpecificationId && specificationIds.Length != 0)
        {
            whereClauseSql = whereClauseSql.ConcatWithBlankStringCheck(" b.specificationid = ANY(@specificationIds)", " and b.specificationid = ANY(@specificationIds)");
        }

        if (whereClauseSql != "")
        {
            whereClauseSql = "where " + whereClauseSql;
        }

        // Organism display name: preferred synonym if set, else genus/species/subspecies/serotype/additional
        var organismDescriptionSql = $"""
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
            	Case When ad.familyid is NULL or ad.familyid = 0 Then ord.name Else orda.name End as ordername,
            	Case When ad.familyid is NULL or ad.familyid = 0 Then f.name Else fa.name End as familyname,
                li5.value As orggroupname,
                b.id,
                b.dosage as antibioticdosage,
                b.enabled as enabled,
                a.antibioticname as antibioticname,
                li2.value as Host,
                li1.value As testmethod,
                specdisplay.value As specification,
                li4.value As specialconsider
            from
                breakpoint b
                inner join antibiotic a on b.antibioticid = a.id
                left outer join specdisplay on specdisplay.id = b.specificationid
                left outer join organism o on o.id = b.organismId
                left outer join organismsynonyms os on o.Id = os.organismId
                and os.PreferredName = true
                left outer join ordercat ord on ord.id = b.orderid
                left outer join family f on f.id = b.familyid
                left outer join genus g on g.Id = o.genusId
                left outer join species s on s.Id = o.speciesId
                left outer join subspecies ss on ss.id = o.subspeciesId
                left outer join serotype se on se.Id = o.serotypeId
                left outer join additional ad on ad.Id = o.additionalId
                left outer join family fa on ad.familyid = fa.id
                left outer join ordercat orda on fa.orderid = orda.id
                left outer join listitem li5 on b.orggroupcodingid = li5.id
                left outer join listitem li2 on b.hostid = li2.id
                left outer join listitem li1 on b.testmethodid = li1.id
                left outer join listitem li4 on b.specialconsiderid = li4.id
                {whereClauseSql} 
            order by 
                {ListQueryOrderByUtil.GetOrderByClause(queryFilters, "antibioticname")}
            limit '500';
            """;

        var result = await connect.QueryAsync<BreakpointListModel>(sql, new { organismIds, hostIds, testMethodIds, specificationIds});

        return result.ToList();
    }
}
