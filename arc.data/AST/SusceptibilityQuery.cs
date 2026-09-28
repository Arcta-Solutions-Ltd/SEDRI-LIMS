using arc.common.Models.AST;
using arc.domain.Configuration.QueryConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.AST;

/// <summary>
/// Retrieves susceptibilities (breakpoints) for AST rows.
/// </summary>
/// <remarks>
/// <para><b>Custom / non-taxonomic organisms:</b> Organisms without genus (or higher taxonomy) still produce a row in organismvalues (culture organism id only).
/// Matching uses stable ids: direct <c>breakpoint.organismid</c> and organism-group <c>organismcoding.OrganismId</c>. Hierarchy expansion runs only when <c>genusid</c> (and lower levels) are present.</para>
/// <para><b>Organism hierarchy matching:</b> Organisms follow Order → Family → Genus → Species (with subspecies or serotype).
/// Matching considers breakpoints scoped at the organism itself or any higher level (e.g. genus breakpoints apply to species in that genus).</para>
/// <para><b>Organism group matching:</b> If the organism belongs to an organism group (via organismcoding), breakpoints scoped to that group are included.
/// Direct membership matches when oc.OrganismId equals the culture organism or any ancestor taxon organism id (genus/species/subspecies/serotype rows from organismvalues), or taxonomy alignment of organisms in the group with the current organism's hierarchy.</para>
/// <para>Behaviour must stay aligned with <see cref="Coding.BreakpointsForOrganismQuery"/> (id-based matching, not translated labels).</para>
/// <para><b>Dosage:</b> For Disk (TestMethodId != 680), dosage must match. For MIC (TestMethodId 680), dosage is not required for matching per business rules.</para>
/// </remarks>
internal class SusceptibilityQuery : IQueryReturningType<List<SusceptibilityModel>>
{
    private const int MicTestMethodId = 680;

    /// <summary>
    /// Executes the susceptibility query to retrieve breakpoints matching the organism, antibiotic, guideline and dosage (Disk only).
    /// Uses organism hierarchy and organism group matching to find applicable breakpoints.
    /// </summary>
    /// <param name="connect">The database connection.</param>
    /// <param name="queryFilters">Query parameters including OrganismId, AntibioticId, TestMethodId, SourceId (guidelines), Dosage, and optionally ZoneDiameter.</param>
    /// <returns>A list of susceptibility models (breakpoints) applicable to the given criteria.</returns>
    public async Task<List<SusceptibilityModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        // bool measurementExists = queryFilters.Parameters.Any(p => p.Key.ToLower() == "zonediameter" || queryFilters.Parameters.Any(p => p.Key.ToLower() == "mic"));
        var measurementParam = queryFilters.Parameters.FirstOrDefault(p => p.Key.Equals("zonediameter", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(p.Value) && p.Value != "0") ?? queryFilters.Parameters.FirstOrDefault(p => p.Key.Equals("mic", StringComparison.OrdinalIgnoreCase));
        bool measurementExists = measurementParam != null;

        // Then: get a single "measurement" value if it exists
        decimal? Measurement = null;

        var OrganismId = int.Parse(queryFilters.Parameters.Where(p => p.Key.ToLower() == "organismid").First().Value);

        var AntibioticIdEntry = queryFilters.Parameters.FirstOrDefault(p => p.Key.Equals("antibioticid", StringComparison.OrdinalIgnoreCase));
        var AntibioticId = AntibioticIdEntry != null ? int.Parse(AntibioticIdEntry.Value) : int.Parse(queryFilters.Parameters.First(p => p.Key.Equals("antibiotic", StringComparison.OrdinalIgnoreCase)).Value);

        var DosageEntry = queryFilters.Parameters.FirstOrDefault(p => p.Key.ToLower() == "dosage");
        var Dosage = DosageEntry?.Value ?? string.Empty;

        var TestMethoodIdEntry = queryFilters.Parameters.FirstOrDefault(p => p.Key.Equals("testmethodid", StringComparison.OrdinalIgnoreCase));
        var TestMethodId = TestMethoodIdEntry != null ? int.Parse(TestMethoodIdEntry.Value) : int.Parse(queryFilters.Parameters.First(p => p.Key.Equals("testmethod", StringComparison.OrdinalIgnoreCase)).Value);

        var SourceIdEntry = queryFilters.Parameters.FirstOrDefault(p => p.Key.Equals("sourceid", StringComparison.OrdinalIgnoreCase));
        var SourceId = SourceIdEntry != null ? int.Parse(SourceIdEntry.Value) : int.Parse(queryFilters.Parameters.First(p => p.Key.Equals("guidelines", StringComparison.OrdinalIgnoreCase)).Value);

        if (SourceId == 0)
        {
            return new List<SusceptibilityModel>();
        }

        var dosageCondition = TestMethodId == MicTestMethodId ? "((b.dosage = @Dosage) or (@TestMethodId = 680))" : "b.dosage = @Dosage";

        var measurementCondition = measurementExists ? "left outer join resultline rl on b.id = rl.breakpointid and rl.startval <= @Measurement and rl.endval >= @Measurement" : "";
        var breakpointJoinCondition = measurementExists ? "select b.*, rl.resultid from breakpoint b" : "select b.* from breakpoint b";
        var breakpointReturnCondition = measurementExists ? ", resultid as susceptibilityid" : "";

        var sql = "";         
        {
                if (measurementExists) 
                {
                    Measurement = (measurementParam.Key.Equals("zonediameter", StringComparison.OrdinalIgnoreCase) ?  int.Parse(measurementParam.Value) : decimal.Parse(measurementParam.Value));
                } 
                    
                sql = @"with bpoint as (with organismvalues as (with org as (select o.id as organismid, o.genusid, o.speciesid, o.subspeciesid, o.serotypeid, f.id as familyid, ord.id as orderid from organism o 
                left outer join genus g on g.id = o.genusid
                left outer join family f on f.id = g.familyid
                left outer join ordercat ord on ord.id = f.orderid
                where o.id = @OrganismId)
                select org.organismid, org1.id as genusid, org2.id as speciesid, org3.id as subspeciesid, org4.id as serotypeid, org.familyid, org.orderid from org
                left outer join organism org1 on org.genusid is not null and org1.genusid = org.genusid and org1.speciesid is null
                left outer join organism org2 on org.genusid is not null and org.speciesid is not null and org2.genusid = org.genusid and org2.speciesid = org.speciesid and org2.subspeciesid is null and org2.serotypeid is null
                left outer join organism org3 on org.genusid is not null and org.speciesid is not null and org.subspeciesid is not null and org3.genusid = org.genusid and org3.speciesid = org.speciesid and org3.subspeciesid = org.subspeciesid and org3.serotypeid is null
                left outer join organism org4 on org.genusid is not null and org.speciesid is not null and org.serotypeid is not null and org4.genusid = org.genusid and org4.speciesid = org.speciesid and org4.serotypeid = org.serotypeid and org4.subspeciesid is null)
                " + breakpointJoinCondition + @"
		        inner join specification s on b.specificationid = s.id and s.guidelinesid = @SourceId
		        " + measurementCondition + @"
                inner join organismvalues org on b.organismid = org.organismid
                where b.antibioticid = @AntibioticId and " + dosageCondition + @" and b.testmethodid = @TestMethodId and enabled = 'Yes'
			    union all
			    " + breakpointJoinCondition + @"
		        inner join specification s on b.specificationid = s.id and s.guidelinesid = @SourceId
		        " + measurementCondition + @"
                inner join organismvalues org on b.organismid = org.serotypeid
                where b.antibioticid = @AntibioticId and " + dosageCondition + @" and b.testmethodid = @TestMethodId and enabled = 'Yes'
			    union all
                " + breakpointJoinCondition + @"
		        inner join specification s on b.specificationid = s.id and s.guidelinesid = @SourceId
		        " + measurementCondition + @"
                inner join organismvalues org on b.organismid = org.subspeciesid
                where b.antibioticid = @AntibioticId and " + dosageCondition + @" and b.testmethodid = @TestMethodId and enabled = 'Yes'
			    union all
                " + breakpointJoinCondition + @"
		        inner join specification s on b.specificationid = s.id and s.guidelinesid = @SourceId
		        " + measurementCondition + @"
                inner join organismvalues org on b.organismid = org.speciesid
                where b.antibioticid = @AntibioticId and " + dosageCondition + @" and b.testmethodid = @TestMethodId and enabled = 'Yes'
			    union all
                " + breakpointJoinCondition + @"
		        inner join specification s on b.specificationid = s.id and s.guidelinesid = @SourceId
		        " + measurementCondition + @"
                inner join organismvalues org on b.organismid = org.genusid
                where b.antibioticid = @AntibioticId and " + dosageCondition + @" and b.testmethodid = @TestMethodId and enabled = 'Yes'
			    union all
			    " + breakpointJoinCondition + @"
		        inner join specification s on b.specificationid = s.id and s.guidelinesid = @SourceId
		        " + measurementCondition + @"
                inner join organismvalues org on b.familyid = org.familyid and (b.organismid = 0 or b.organismid is null)
                where b.antibioticid = @AntibioticId and " + dosageCondition + @" and b.testmethodid = @TestMethodId and enabled = 'Yes'            
			    union all
                " + breakpointJoinCondition + @"
		        inner join specification s on b.specificationid = s.id and s.guidelinesid = @SourceId
		        " + measurementCondition + @"
                inner join organismvalues org on b.orderid = org.orderid and (b.familyid = 0 or b.familyid is null)
                where b.antibioticid = @AntibioticId and " + dosageCondition + @" and b.testmethodid = @TestMethodId and enabled = 'Yes'  
			    union all
                " + breakpointJoinCondition + @"
		        inner join specification s on b.specificationid = s.id and s.guidelinesid = @SourceId
		        " + measurementCondition + @"
                inner join organismcoding oc on b.orggroupcodingid = oc.CodingId
                left outer join organism o on o.id = oc.organismid
                left outer join genus g on o.genusid = g.id
                left outer join family f on g.familyid = f.id
                inner join organismvalues org on 1=1
                where (
                    (oc.OrganismId = org.organismid
                        or (org.genusid is not null and oc.OrganismId = org.genusid)
                        or (org.speciesid is not null and oc.OrganismId = org.speciesid)
                        or (org.subspeciesid is not null and oc.OrganismId = org.subspeciesid)
                        or (org.serotypeid is not null and oc.OrganismId = org.serotypeid))
                    or (
                        (f.orderid = org.orderid or f.orderid = 0 or f.orderid is null) and
                        (g.familyid = org.familyid or g.familyid = 0 or g.familyid is null) and
                        (o.genusid = org.genusid or o.genusid = 0 or o.genusid is null) and
                        (o.speciesid = org.speciesid or o.speciesid = 0 or o.speciesid is null) and
                        (o.subspeciesid = org.subspeciesid or o.subspeciesid = 0 or o.subspeciesid is null) and
                        (o.serotypeid = org.serotypeid or o.serotypeid = 0 or o.serotypeid is null) and
                        (b.orggroupcodingid != 0) and (b.orggroupcodingid is not null)
                    )
                )
                and b.antibioticid = @AntibioticId and " + dosageCondition + @" and b.testmethodid = @TestMethodId and enabled = 'Yes')						
			    select distinct on (specialconsiderid) antibioticid, dosage, testmethodid, sp.guidelinesid as guidelinesid, '' as displayonreport, specialconsiderid as specialconsiderationid, li.value as specialconsideration, bpoint.id as breakpointid " + breakpointReturnCondition + @" from bpoint
                inner join specification sp on bpoint.specificationid = sp.id
                left outer join ListItem li on li.Id = specialconsiderid
                order by specialconsiderid, bpoint.id";

                var result = await connect.QueryAsync<SusceptibilityModel>(sql, new { OrganismId, AntibioticId, Dosage, TestMethodId, SourceId, Measurement });
                return result.ToList();
            }
        }          
}
