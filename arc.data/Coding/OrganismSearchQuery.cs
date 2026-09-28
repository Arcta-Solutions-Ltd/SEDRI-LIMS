using arc.common.Models.Coding;
using arc.data.Utils;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Coding
{
    internal class OrganismSearchQuery : IQueryReturningType<List<OrganismSearchModel>>
    {
        public async Task<List<OrganismSearchModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var genusIdVal = 0;
            var speciesIdVal = 0;
            var subspeciesIdVal = 0;
            var serotypeIdVal = 0;

            QueryValuesConfig speciesId = null;
            QueryValuesConfig subspeciesId = null;
            QueryValuesConfig serotypeId = null;
            var search = queryFilters.Parameters.Where(p => p.Key.ToLower() == "search");
            var paramList = queryFilters.Parameters.Where(p => p.Key.ToLower() == "genusid");
            var genusId = paramList.Count() == 0 ? null : paramList.First();

            //            var textMatch = search.Count() > 0 && ! string.IsNullOrWhiteSpace(search.First().Value) ? "%" + search.First().Value.ToLower() + "%" : "%";

            ////// Process space separated string from search box //////
            var searchString = search.Count() > 0 ? search.First().Value.ToLower() : "%";

            var whereSearchClause = OrganismSearchWhereClauseUtil.GenerateWhereClause(searchString);          

            ////////////////////////////////////////////////////////////


            if (genusId != null)
            {
                paramList = queryFilters.Parameters.Where(p => p.Key.ToLower() == "speciesid");
                speciesId = paramList.Count() == 0 ? null : paramList.First();

                if (speciesId != null)
                {
                    paramList = queryFilters.Parameters.Where(p => p.Key.ToLower() == "subspeciesid");
                    subspeciesId = paramList.Count() == 0 ? null : paramList.First();
                    paramList = queryFilters.Parameters.Where(p => p.Key.ToLower() == "serotypeid");
                    serotypeId = paramList.Count() == 0 ? null : paramList.First();
                }
            }

            var whereClause = "";

            if (genusId != null && !string.IsNullOrWhiteSpace(genusId.Value) && int.Parse(genusId.Value) > 0)
            {
                whereClause += "g.Id = @genusId";
                genusIdVal = int.Parse(genusId.Value);
            }

            if (speciesId != null && !string.IsNullOrWhiteSpace(speciesId.Value) && int.Parse(speciesId.Value) > 0)
            {
                whereClause += " and s.Id = @speciesId";
                speciesIdVal = int.Parse(speciesId.Value);
            }

            if (subspeciesId != null && !string.IsNullOrWhiteSpace(subspeciesId.Value) && int.Parse(subspeciesId.Value) > 0)
            {
                whereClause += " and ss.Id = @subspeciesId";
                subspeciesIdVal = int.Parse(subspeciesId.Value);
            }

            if (serotypeId != null && !string.IsNullOrWhiteSpace(serotypeId.Value) && int.Parse(serotypeId.Value) > 0)
            {
                whereClause += " and se.Id = @serotypeId";
                serotypeIdVal = int.Parse(serotypeId.Value);
            }

            var sql = "";
            var organismDescription = "Case When os.synonym is NULL then Case When ad.name is NULL Then TRIM(CONCAT(TRIM(g.name),case when g.name is not null and s.name is null then ' spp.' else '' end,' ',TRIM(s.name),' ',TRIM(ss.name),' ',TRIM(se.name))) else ad.name end else os.synonym end as description";

            var whereClauseToAdd = whereClause != "" || whereSearchClause != "" ? " where " : "";
            whereClauseToAdd += whereClause != "" ? whereClause : "";
            whereClauseToAdd += whereClause != "" && whereSearchClause != "" ? " and " : "";
            whereClauseToAdd += whereSearchClause != "" ? whereSearchClause : "";

            sql = @"select o.Id, " + organismDescription + @", g.name As genus, 
                    s.name as species, ss.name as subspecies, se.name as serotype, sy.synonyms
                    from organism o
                    left outer join genus g on g.Id = o.genusId 
                    left outer join species s on s.Id = o.speciesId 
                    left outer join subspecies ss on ss.id = o.subspeciesId 
                    left outer join serotype se on se.Id = o.serotypeId 
                    left outer join additional ad on ad.Id = o.additionalId 
                    left outer join organismsynonyms os on o.Id = os.organismId and os.PreferredName = true 
                    left outer join (select organismid, string_agg(synonym, ', ') As synonyms from organismsynonyms where PreferredName = false group by organismid ) sy on o.Id = sy.OrganismId "
                    + whereClauseToAdd + @"
                    order by CONCAT(g.name, s.name, ss.name, se.name)
                    limit 500";

            var criteria = new { genusId = genusIdVal, speciesId = speciesIdVal, subspeciesId = subspeciesIdVal, serotypeId = serotypeIdVal };
            var result = await connect.QueryAsync<OrganismSearchModel>(sql, criteria);

            return result.ToList();
        }
    }
}
