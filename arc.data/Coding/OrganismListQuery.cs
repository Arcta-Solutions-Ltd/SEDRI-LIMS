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
    internal class OrganismListQuery : IQueryReturningType<List<OrganismListModel>>
    {
        public async Task<List<OrganismListModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var codeId = queryFilters.Parameters[0].Value != "" ? int.Parse(queryFilters.Parameters[0].Value) : 0;
            var textMatch = queryFilters.Parameters.Count > 1 ? queryFilters.Parameters[1].Value.ToLower() : "%";

            var where = OrganismSearchWhereClauseUtil.GenerateWhereClause(textMatch.Trim(), true);
            var whereClause = where != "" ? " where " + where : "";

            var sql = @"select o.Id, TRIM(CONCAT(g.name,case when g.name is not null and s.name is null and ad.name is null then ' spp.' else '' end, ' ',s.name,' ',ss.name,' ',TRIM(se.name), ad.name)) as description,
                        Case When ad.name is NULL Then 'No' Else 'Yes' End As custom, li.Value as Gram, oc.Code,
						Case When ad.familyid is NULL or ad.familyid = 0 Then ord.name Else orda.name End as ordername,
						Case When ad.familyid is NULL or ad.familyid = 0 Then f.name Else fa.name End as familyname,
                        os.synonym as preferredname,
                        sy.synonyms
                        from organism o
                        inner join organismcoding oc on o.Id = oc.organismId and oc.codingId = @codeId
                        left outer join genus g on g.Id = o.genusId
                        left outer join species s on s.Id = o.speciesId
                        left outer join family f on g.familyid = f.id
                        left outer join ordercat ord on f.orderid = ord.id
                        left outer join subspecies ss on ss.id = o.subspeciesId
                        left outer join serotype se on se.Id = o.serotypeId
                        left outer join additional ad on ad.Id = o.additionalId
                        left outer join family fa on ad.familyid = fa.id
                        left outer join ordercat orda on fa.orderid = orda.id
                        left outer join ListItem li on li.Id = o.gramId 
                        left outer join organismsynonyms os on o.Id = os.organismId and os.PreferredName = true 
                        left outer join (select organismid, string_agg(synonym, ', ') As synonyms from organismsynonyms where PreferredName = false group by organismid ) sy on o.Id = sy.OrganismId " 
                        + whereClause + 
                        "order by " + ListQueryOrderByUtil.GetOrderByClause(queryFilters, "CONCAT(g.name, s.name, ss.name, se.name, ad.name)") + " limit 500";

            var result = await connect.QueryAsync<OrganismListModel>(sql, new { codeId, tx = textMatch });

            return result.ToList();
        }
    }
}
