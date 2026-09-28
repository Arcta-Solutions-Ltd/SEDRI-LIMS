using arc.common.Models.Coding;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Coding;

/// <summary>
/// Data query that loads a single breakpoint by id for the breakpoint view/record screen.
/// Returns a <see cref="BreakpointListModel"/> with organism name, taxonomy (order/family), antibiotic,
/// dosage, host, test method, specification, special consideration, and related list item values.
/// </summary>
internal class BreakpointViewByIdQuery : IQueryReturningType<BreakpointListModel>
{
    /// <summary>
    /// Executes the breakpoint-by-id query and returns the result as a single <see cref="BreakpointListModel"/>.
    /// </summary>
    /// <param name="connect">Active database connection.</param>
    /// <param name="queryFilters">Filter config; expects an integer "id" for the breakpoint.</param>
    /// <returns>The breakpoint row mapped to <see cref="BreakpointListModel"/>, or throws if not found.</returns>
    public async Task<BreakpointListModel> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var breakpointId = queryFilters.GetIntegerValue("id");

        // Organism display name: preferred synonym if set, otherwise genus/species/subspecies/serotype/additional
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

        // Select breakpoint row with joined organism taxonomy, antibiotic, specification display, and list item labels
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
            where b.Id = @Id 
            """;

        return await connect.QueryFirstAsync<BreakpointListModel>(sql, new { Id = breakpointId });
    }
}
