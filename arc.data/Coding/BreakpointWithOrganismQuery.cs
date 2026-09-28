using arc.domain.Coding;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Coding
{
    class BreakpointWithOrganismQuery : IQueryReturningType<Breakpoint>
    {
        public async Task<Breakpoint> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var id = queryFilters.Parameters.Where(p => p.Key.ToLower() == "id").First();

            var sql = @"select b.*, st.specimentypeid, od.name as order, f.name as family, l.value as organismgroup, og.genusid, og.speciesid, og.subspeciesid, og.serotypeid from breakpoint b
                        left outer join organism og on og.id = b.organismid
                        left outer join ordercat od on od.id = b.orderid
                        left outer join family f on f.id = b.familyid
                        left outer join listitem l on l.id = b.orggroupcodingid
                        left outer join 
                        (select breakpointid, STRING_AGG(cast(specimentypeid as varchar(7)), ',') as specimentypeid from specimentypebreakpoint group by breakpointid) st
                        on st.breakpointId = b.id where b.Id = @Id";

            return await connect.QueryFirstAsync<Breakpoint>(sql, new { Id = int.Parse(id.Value) });
        }
    }
}
