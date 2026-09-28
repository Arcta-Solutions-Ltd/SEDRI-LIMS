using arc.common.Models.Coding;
using arc.domain.Coding;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Coding;

internal class BreakpointsForOrganismQuery : IQueryReturningType<List<Breakpoint>>
{
    public async Task<List<Breakpoint>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        _ = queryFilters.TryParseIntegerValue("specimentypeid", out var specimenTypeId, 0);
        _ = queryFilters.TryParseIntegerValue("organismid", out var organismId, 0);
        _ = queryFilters.TryParseIntegerValue("antibioticid", out var antibioticId, 0);
        _ = queryFilters.TryParseIntegerValue("testmethodid", out var testMethodId, 0);
        _ = queryFilters.TryParseIntegerValue("guidelinesid", out var guidelinesId, 0);
        var dosage = queryFilters.GetStringValue("dosage");

        var sql = @"select o.id as organismid, o.genusid, o.speciesid, g.familyid, f.orderid, o.additionalid, o.serotypeid, o.subspeciesid
                        from Organism o
                        left outer join genus g on g.id = o.genusid
                        left outer join family f on f.id = g.familyid
                        where o.Id = @OrganismId";

        var queryResult = await connect.QueryAsync<OrganismDescriptorModel>(sql, new { OrganismId = organismId });
        var organismDescription = queryResult.FirstOrDefault();

        sql = @"select b.id, b.orderid, b.familyid, o.genusid, o.speciesid, o.subspeciesid, o.serotypeid, b.specialconsiderid, b.orggroupcodingid from breakpoint b
	                    inner join specification sp on b.specificationid = sp.id and sp.guidelinesid = @GuidelinesId
	                    left outer join organism o on o.id = b.organismid
                        left outer join specimentypebreakpoint s on b.id = s.breakpointid
                        left outer join (select breakpointid, count(id) as counttotal from specimentypebreakpoint
			                                    group by breakpointid) stb on b.id = stb.breakpointid
                        where ((b.orderid = @OrderId) and
						(b.familyid = @FamilyId or b.familyId is null or b.familyId = 0) and
						(o.speciesid = @SpeciesId or o.speciesId is null or o.speciesId = 0) and
	                    (o.genusid = @GenusId or o.genusId is null or o.genusId = 0) and
	                    (o.subspeciesid = @SubspeciesId or o.subspeciesId is null or o.subspeciesId = 0) and
	                    (o.serotypeid = @SerotypeId or o.serotypeId is null or o.serotypeId = 0) and
	                    (o.additionalid = @AdditionalId or o.additionalId is null or o.additionalId = 0)) and 
	                    (b.orggroupcodingid is null or b.orggroupcodingid = 0) and 
                        b.antibioticid = @AntibioticId and
                        b.testmethodid = @TestMethodId and
                        ((b.dosage = @Dosage) or (@TestMethodId = 680)) and
                        b.enabled = 'Yes' and
                        (s.specimentypeid = @SpecimenTypeId or stb.counttotal is null)
                        union		
                        select b.id, b.orderid, b.familyid, o.genusid, o.speciesid, o.subspeciesid, o.serotypeid, b.specialconsiderid, b.orggroupcodingid from breakpoint b
                        inner join specification sp on b.specificationid = sp.id and sp.guidelinesid = @GuidelinesId
                        inner join organismcoding oc on b.orggroupcodingid = oc.CodingId
                        left outer join organism o on o.id = oc.organismid
                        left outer join genus g on o.genusid = g.id
                        left outer join family f on g.familyid = f.id
                        left outer join specimentypebreakpoint s on b.id = s.breakpointid
                        left outer join (select breakpointid, count(id) as counttotal from specimentypebreakpoint
						                        group by breakpointid) stb on b.id = stb.breakpointid
                        where (((f.orderid = @OrderId or f.orderid = 0 or f.orderid is null) and
                        (g.familyid = @FamilyId or g.familyid = 0 or g.familyid is null) and
                        (o.genusid = @GenusId or o.genusid = 0 or o.genusid is null) and
                        (o.speciesid = @SpeciesId or o.speciesid = 0 or o.speciesid is null) and
                        (o.serotypeid = @SerotypeId or o.serotypeid = 0 or o.serotypeid is null) and
                        (o.subspeciesid = @SubspeciesId or o.subspeciesid = 0 or o.subspeciesid is null) and
                        (b.orggroupcodingid != 0) and (b.orggroupcodingid is not null))
                        or oc.OrganismId = @OrganismId) and
                        b.antibioticid = @AntibioticId and
                        b.testmethodid = @TestMethodId and
                        ((b.dosage = @Dosage) or (@TestMethodId = 680)) and
                        b.enabled = 'Yes' and
                        (s.specimentypeid = @SpecimenTypeId or stb.counttotal is null)";

        var breakpoints = await connect.QueryAsync<Breakpoint>(sql, new
        {
            OrganismId = organismId,
            AntibioticId = antibioticId,
            TestMethodId = testMethodId,
            GuidelinesId = guidelinesId,
            Dosage = dosage,
            SpecimenTypeId = specimenTypeId,
            OrderId = organismDescription != null ? organismDescription.OrderId : 0,
            FamilyId = organismDescription != null ? organismDescription.FamilyId : 0,
            GenusId = organismDescription != null ? organismDescription.GenusId : 0,
            SpeciesId = organismDescription != null ? organismDescription.SpeciesId : 0,
            SubspeciesId = organismDescription != null ? organismDescription.SubspeciesId : 0,
            SerotypeId = organismDescription != null ? organismDescription.SerotypeId : 0,
            AdditionalId = organismDescription != null ? organismDescription.AdditionalId : 0
        });

        var breakpointList = new List<Breakpoint>();

        var specialConsiderationList = breakpoints.Select(v => v.SpecialConsiderId).Distinct();
        foreach (var specialConsideration in specialConsiderationList)
        {
            var specialBreakpoints = breakpoints.Where(b => b.SpecialConsiderId == specialConsideration).ToList();

            var breakpoint = GetLongestMatch(specialBreakpoints);

            if (breakpoint == null)
            {
                breakpoint = breakpoints.FirstOrDefault(b => b.OrgGroupCodingId > 0);
            }

            if (breakpoint != null)
            {
                breakpointList.Add(breakpoint);
            }
        }


        foreach (var breakpoint in breakpointList)
        {
            if (breakpoint != null)
            {
                sql = @"select * from resultline where breakpointid = @BreakpointId";
                var resultLines = await connect.QueryAsync<BreakpointLine>(sql, new { BreakpointId = breakpoint.Id });
                breakpoint.BreakpointGrid = resultLines.ToList();
            }
        }
        return breakpointList;
    }

    private Breakpoint GetLongestMatch(List<Breakpoint> breakpoints)
    {
        if (breakpoints.Count == 0)
        {
            return null;
        }
        foreach (var breakpoint in breakpoints)
        {
            if (breakpoint.SerotypeId != 0)
            {
                return breakpoint;
            }
        }
        foreach (var breakpoint in breakpoints)
        {
            if (breakpoint.SubSpeciesId != 0)
            {
                return breakpoint;
            }
        }
        foreach (var breakpoint in breakpoints)
        {
            if (breakpoint.SpeciesId != 0)
            {
                return breakpoint;
            }
        }
        foreach (var breakpoint in breakpoints)
        {
            if (breakpoint.GenusId != 0)
            {
                return breakpoint;
            }
        }
        foreach (var breakpoint in breakpoints)
        {
            if (breakpoint.FamilyId != 0)
            {
                return breakpoint;
            }
        }
        return breakpoints.First();
    }
}
