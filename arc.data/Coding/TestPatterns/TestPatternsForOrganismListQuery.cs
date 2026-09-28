using arc.common.Models.Coding;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Coding;

/// <summary>
/// Returns test patterns matching a culture organism by direct taxonomic scope or organism-group membership.
/// Each result includes <see cref="TestPatternScopeModel.IsOrganismGroupMatch"/> for default-selection logic.
/// </summary>
internal class TestPatternsForOrganismListQuery : IQueryReturningType<List<TestPatternScopeModel>>
{
    public async Task<List<TestPatternScopeModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        _ = queryFilters.TryParseIntegerValue("organismid", out var organismId, 0);
        _ = queryFilters.TryParseIntegerValue("specimentypeid", out var specimenTypeId, 0);

        var sql = @"select o.id as organismid, o.genusid, o.speciesid, g.familyid, f.orderid, o.additionalid, o.serotypeid, o.subspeciesid
                        from Organism o
                        left outer join genus g on g.id = o.genusid
                        left outer join family f on f.id = g.familyid
                        where o.Id = @OrganismId";

        var queryResult = await connect.QueryAsync<OrganismDescriptorModel>(sql, new { OrganismId = organismId });
        var organismDescription = queryResult.FirstOrDefault();

        //Return an empty list of test patterns if no organism is present
        var result = new List<TestPatternScopeModel>();
        if (organismDescription != null)
        {
            // Get direct taxonomic matches (i.e. to test patterns with taxonomic scope).
           sql = """
            select tp.id, tp.orderid, tp.familyid, o.genusid, o.speciesid, o.subspeciesid, o.serotypeid, o.additionalid,
                tp.testpatternname, tp.makedefault, 0 as orggroupcodingid, tp.organismid as scopeorganismid, false as isorganismgroupmatch
            	from testpattern tp
            	left outer join organism o on o.id = tp.organismid
                left outer join specimentypetestpattern s on tp.id = s.testpatternid
                left outer join (select testpatternid, count(id) as counttotal from specimentypetestpattern
            	                  group by testpatternid) stb on tp.id = stb.testpatternid
            	left outer join genus g on o.genusid = g.id
            	where ((tp.orderid = @OrderId) and
            	(tp.familyid = @FamilyId or tp.familyId is null or tp.familyId = 0) and
            	(o.speciesid = @SpeciesId or o.speciesId is null or o.speciesId = 0) and
            	(o.genusid = @GenusId or o.genusId is null or o.genusId = 0) and
            	(o.subspeciesid = @SubspeciesId or o.subspeciesId is null or o.subspeciesId = 0) and
            	(o.serotypeid = @SerotypeId or o.serotypeId is null or o.serotypeId = 0) and
            	(o.additionalid = @AdditionalId or o.additionalId is null or o.additionalId = 0)) and 
            	(tp.orggroupcodingid is null or tp.orggroupcodingid = 0) and
                (s.specimentypeid = @SpecimenTypeId or stb.counttotal is null)
            union		
            select tp.id, f.orderid, g.familyid, o.genusid, o.speciesid, o.subspeciesid, o.serotypeid, o.additionalid,
                tp.testpatternname, tp.makedefault, tp.orggroupcodingid, coalesce(oc.organismid, 0) as scopeorganismid, true as isorganismgroupmatch
            	from testpattern tp	
            	inner join organismcoding oc on tp.orggroupcodingid = oc.CodingId
            	left outer join organism o on o.id = oc.organismid
            	left outer join genus g on o.genusid = g.id
            	left outer join family f on g.familyid = f.id
                left outer join specimentypetestpattern s on tp.id = s.testpatternid
                left outer join (select testpatternid, count(id) as counttotal from specimentypetestpattern
            	         group by testpatternid) stb on tp.id = stb.testpatternid
                where (((f.orderid = @OrderId or f.orderid = 0 or f.orderid is null) and
                (g.familyid = @FamilyId or g.familyid = 0 or g.familyid is null) and
                (o.genusid = @GenusId or o.genusid = 0 or o.genusid is null) and
                (o.speciesid = @SpeciesId or o.speciesid = 0 or o.speciesid is null) and
                (o.serotypeid = @SerotypeId or o.serotypeid = 0 or o.serotypeid is null) and
                (o.subspeciesid = @SubspeciesId or o.subspeciesid = 0 or o.subspeciesid is null) and
                (tp.orggroupcodingid != 0) and
                (tp.orggroupcodingid is not null)) or
                    oc.OrganismId = @OrganismId) and
                (s.specimentypeid = @SpecimenTypeId or stb.counttotal is null)
            """;

            var res = await connect.QueryAsync<TestPatternScopeModel>(sql, new
            {
                OrganismId = organismId,
                SpecimenTypeId = specimenTypeId,
                OrderId = organismDescription != null ? organismDescription.OrderId : 0,
                FamilyId = organismDescription != null ? organismDescription.FamilyId : 0,
                GenusId = organismDescription != null ? organismDescription.GenusId : 0,
                SpeciesId = organismDescription != null ? organismDescription.SpeciesId : 0,
                SubspeciesId = organismDescription != null ? organismDescription.SubspeciesId : 0,
                SerotypeId = organismDescription != null ? organismDescription.SerotypeId : 0,
                AdditionalId = organismDescription != null ? organismDescription.AdditionalId : 0
            });
            result = [.. res];
        };

        return result;
    }
}
