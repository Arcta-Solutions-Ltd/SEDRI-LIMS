using arc.common.Models.Coding;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Coding
{
    internal class CustomEntryByOrganismIdQuery : IQueryReturningType<EditCustomEntryModel>
    {

        public async Task<EditCustomEntryModel> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var id = queryFilters.Parameters.Where(p => p.Key.ToLower() == "id").First();
            var codingId = queryFilters.Parameters.Where(p => p.Key.ToLower() == "metafcodingid").First();

            var sql = @"select o.additionalid from organism o where o.Id = @Id";
            var additionalId = await connect.QueryFirstAsync<string>(sql, new { Id = int.Parse(id.Value) });

            EditCustomEntryModel result;
            if (additionalId == null || additionalId == "0")
            {
                sql = @"select o.Id, oc.Id As OrganismCodingId, o.additionalId, oc.code,
                        TRIM(CONCAT(g.name,case when g.name is not null and s.name is null then ' spp.' else '' end,' ',s.name,' ',ss.name,' ',TRIM(se.name))) as description
                        from organism o
                        inner join organismcoding oc on oc.OrganismId = o.Id and oc.codingId = @CodingId
                        left outer join genus g on g.Id = o.genusId
                        left outer join species s on s.Id = o.speciesId
                        left outer join subspecies ss on ss.id = o.subspeciesId
                        left outer join serotype se on se.Id = o.serotypeId
                        where o.Id = @Id";

                result = await connect.QueryFirstAsync<EditCustomEntryModel>(sql, new { Id = int.Parse(id.Value), CodingId = int.Parse(codingId.Value) });
            }
            else
            {
                sql = @"select o.Id, oc.Id As OrganismCodingId, a.Id as AdditionalId,
                        Case When a.familyid = 0 Then f.orderid Else fa.orderid end,
                        Case When a.familyid = 0 Then g.familyid Else a.familyid end,
                        o.genusid, o.speciesid, a.name as description, oc.code
                        from additional a
                        inner join organism o on o.additionalid = a.id
                        inner join organismcoding oc on oc.OrganismId = o.Id
                        left outer join genus g on g.id = o.genusid
                        left outer join family f on f.id = g.familyid
                        left outer join family fa on fa.id = a.familyid
                        where o.Id = @Id";

                result = await connect.QueryFirstAsync<EditCustomEntryModel>(sql, new { Id = int.Parse(id.Value) });
            }

            return result;
        }
    }
}

//public async Task<JustCraftedPages> Execute(NpgsqlConnection connect, QueryFilterConfig queryFilters)
//{
//    var id = queryFilters.Parameters.Where(p => p.Key.ToLower() == "id").First();
//    var codingId = queryFilters.Parameters.Where(p => p.Key.ToLower() == "metafcodingid").First();

//    var sql = @"select o.additionalid from organism o where o.Id = @Id";
//    var additionalId = await connect.QueryFirstAsync<string>(sql, new { Id = int.Parse(id.Value) });

//    EditCustomEntryModel result;
//    if (additionalId == null || additionalId == "0")
//    {
//        sql = @"select o.Id, oc.Id As OrganismCodingId, o.additionalId, oc.code,
//                        TRIM(CONCAT(g.name,case when g.name is not null and s.name is null then ' spp.' else '' end,' ',s.name,' ',ss.name,' ',TRIM(se.name))) as description
//                        from organism o
//                        inner join organismcoding oc on oc.OrganismId = o.Id and oc.codingId = @CodingId
//                        left outer join genus g on g.Id = o.genusId
//                        left outer join species s on s.Id = o.speciesId
//                        left outer join subspecies ss on ss.id = o.subspeciesId
//                        left outer join serotype se on se.Id = o.serotypeId
//                        where o.Id = @Id";

//        result = await connect.QueryFirstAsync<EditCustomEntryModel>(sql, new { Id = int.Parse(id.Value), CodingId = int.Parse(codingId.Value) });
//    }
//    else
//    {
//        sql = @"select o.Id, oc.Id As OrganismCodingId, a.Id as AdditionalId,
//                        Case When a.familyid = 0 Then f.orderid Else fa.orderid end,
//                        Case When a.familyid = 0 Then g.familyid Else a.familyid end,
//                        o.genusid, o.speciesid, a.name as description, oc.code
//                        from additional a
//                        inner join organism o on o.additionalid = a.id
//                        inner join organismcoding oc on oc.OrganismId = o.Id
//                        left outer join genus g on g.id = o.genusid
//                        left outer join family f on f.id = g.familyid
//                        left outer join family fa on fa.id = a.familyid
//                        where o.Id = @Id";

//        result = await connect.QueryFirstAsync<EditCustomEntryModel>(sql, new { Id = int.Parse(id.Value) });
//    }

//    var craftedModels = new List<CraftedModel>();
//    craftedModels.Add(new CraftedModel { Name = "editcustompage", Contents = JsonConvert.SerializeObject(result) });
//    var returnModel = new JustCraftedPages { Crafted = craftedModels };
//    return returnModel;
//}
