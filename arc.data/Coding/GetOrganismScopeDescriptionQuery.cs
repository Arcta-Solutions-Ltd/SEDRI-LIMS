using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Coding;

/// <summary>
/// Query that returns a human-readable description for an organism scope
/// (OrderId, FamilyId, GenusId, SpeciesId, SubspeciesId, SerotypeId, OrgGroupCodingId, OrganismId).
/// Used for displaying organism scope in laboratory config list views.
/// </summary>
internal class GetOrganismScopeDescriptionQuery : IQueryReturningString
{
    public async Task<string> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var organismId = GetIntParam(queryFilters, "organismid", 0);
        var orgGroupCodingId = GetIntParam(queryFilters, "orggroupcodingid", 0);
        var orderId = GetIntParam(queryFilters, "orderid", 0);
        var familyId = GetIntParam(queryFilters, "familyid", 0);
        var genusId = GetIntParam(queryFilters, "genusid", 0);
        var speciesId = GetIntParam(queryFilters, "speciesid", 0);
        var subspeciesId = GetIntParam(queryFilters, "subspeciesid", 0);
        var serotypeId = GetIntParam(queryFilters, "serotypeid", 0);

        if (organismId > 0)
        {
            var sql = @"
                select TRIM(CONCAT(g.name, case when g.name is not null and s.name is null and a.name is null then ' spp.' else '' end, ' ', s.name, ' ', ss.name, ' ', TRIM(se.name), a.name))
                from organism o
                left outer join genus g on g.Id = o.genusId
                left outer join species s on s.Id = o.speciesId
                left outer join subspecies ss on ss.id = o.subspeciesId
                left outer join serotype se on se.Id = o.serotypeId
                left outer join additional a on a.Id = o.additionalId
                where o.Id = @OrganismId";
            var result = await connect.QueryFirstOrDefaultAsync<string>(sql, new { OrganismId = organismId });
            return result ?? "";
        }

        if (orgGroupCodingId > 0)
        {
            var sql = "select Value from ListItem where Id = @Id";
            var result = await connect.QueryFirstOrDefaultAsync<string>(sql, new { Id = orgGroupCodingId });
            return result ?? "";
        }

        var parts = new System.Collections.Generic.List<string>();

        if (genusId > 0 || speciesId > 0 || subspeciesId > 0 || serotypeId > 0)
        {
            if (genusId > 0)
            {
                var sql = "select name from genus where id = @Id";
                var name = await connect.QueryFirstOrDefaultAsync<string>(sql, new { Id = genusId });
                if (!string.IsNullOrEmpty(name)) parts.Add(name);
            }
            if (speciesId > 0)
            {
                var sql = "select name from species where id = @Id";
                var name = await connect.QueryFirstOrDefaultAsync<string>(sql, new { Id = speciesId });
                if (!string.IsNullOrEmpty(name)) parts.Add(name);
            }
            if (subspeciesId > 0)
            {
                var sql = "select name from subspecies where id = @Id";
                var name = await connect.QueryFirstOrDefaultAsync<string>(sql, new { Id = subspeciesId });
                if (!string.IsNullOrEmpty(name)) parts.Add(name);
            }
            if (serotypeId > 0)
            {
                var sql = "select name from serotype where id = @Id";
                var name = await connect.QueryFirstOrDefaultAsync<string>(sql, new { Id = serotypeId });
                if (!string.IsNullOrEmpty(name)) parts.Add(name);
            }
        }
        else if (familyId > 0)
        {
            var sql = "select name from family where id = @Id";
            var name = await connect.QueryFirstOrDefaultAsync<string>(sql, new { Id = familyId });
            if (!string.IsNullOrEmpty(name)) parts.Add(name);
        }
        else if (orderId > 0)
        {
            var sql = "select name from ordercat where id = @Id";
            var name = await connect.QueryFirstOrDefaultAsync<string>(sql, new { Id = orderId });
            if (!string.IsNullOrEmpty(name)) parts.Add(name);
        }

        return string.Join(" ", parts.Where(p => !string.IsNullOrWhiteSpace(p)));
    }

    private static int GetIntParam(QueryFilterConfig queryFilters, string key, int defaultValue)
    {
        var param = queryFilters.Parameters?.FirstOrDefault(p => p.Key?.ToLower() == key.ToLower());
        if (param == null || string.IsNullOrEmpty(param.Value)) return defaultValue;
        return int.TryParse(param.Value, out var v) ? v : defaultValue;
    }
}
