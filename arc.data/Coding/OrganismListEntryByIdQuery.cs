using arc.common.Models.Coding;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Coding
{
    internal class OrganismListEntryByIdQuery : IQueryReturningType<OrganismListModel>
    {
        public async Task<OrganismListModel> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var id = queryFilters.Parameters.Where(p => p.Key.ToLower() == "id").First();

            var codeId = 0;
            var codeSQL = "";
            var selectCodeSQL = "";
            if(queryFilters.Parameters.Any(p => p.Key.ToLower() == "metafcodingid"))
            {
                codeId = int.Parse(queryFilters.Parameters.Where(p => p.Key.ToLower() == "metafcodingid").First().Value);
                codeSQL = "inner join organismcoding oc on o.Id = oc.organismId and oc.codingId = @codeId";
                selectCodeSQL = ", oc.Code";
            }


            var sql = $"""
                        select o.Id, TRIM(CONCAT(g.name,case when g.name is not null and s.name is null and a.name is null then ' spp.' else '' end, ' ',s.name,' ',ss.name,' ',TRIM(se.name), a.name)) as description,
                        Case When a.name is NULL Then 'No' Else 'Yes' End As custom, li.Value as Gram{selectCodeSQL},
                        Case When a.familyid is NULL or a.familyid = 0 Then ord.name Else orda.name End as ordername,
                        Case When a.familyid is NULL or a.familyid = 0 Then f.name Else fa.name End as familyname
                        from organism o
                        {codeSQL}
                        left outer join genus g on g.Id = o.genusId
                        left outer join species s on s.Id = o.speciesId
                        left outer join family f on g.familyid = f.id
                        left outer join ordercat ord on f.orderid = ord.id
                        left outer join subspecies ss on ss.id = o.subspeciesId
                        left outer join serotype se on se.Id = o.serotypeId
                        left outer join additional a on a.Id = o.additionalId
                        left outer join family fa on a.familyid = fa.id
                        left outer join ordercat orda on fa.orderid = orda.id
                        left outer join ListItem li on li.Id = o.gramId
                        where o.Id = @Id
                    """;

            return await connect.QueryFirstAsync<OrganismListModel>(sql, new { Id = int.Parse(id.Value), CodeId = codeId });
        }
    }
}
