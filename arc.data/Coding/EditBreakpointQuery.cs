using arc.domain.Coding;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Coding
{
    internal class EditBreakpointQuery : IQueryReturningType<Breakpoint>
    {
        public async Task<Breakpoint> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var id = queryFilters.GetIntegerValue("id");

            var sql = @"select b.*, st.specimentypeid, od.name as order, f.name as family, l.value as organismgroup from breakpoint b
                        left outer join ordercat od on od.id = b.orderid
                        left outer join family f on f.id = b.familyid
                        left outer join listitem l on l.id = b.orggroupcodingid
                        left outer join 
                        (select breakpointid, STRING_AGG(cast(specimentypeid as varchar(7)), ',') as specimentypeid from specimentypebreakpoint group by breakpointid) st
                        on st.breakpointId = b.id where b.Id = @Id";

            var breakpoint = await connect.QueryFirstAsync<Breakpoint>(sql, new { Id = id });

            sql = @"select * from resultline where breakpointid = @BreakpointId";
            var resultLines = await connect.QueryAsync<BreakpointLine>(sql, new { BreakpointId = id });
            breakpoint.BreakpointGrid = resultLines.ToList();
            return breakpoint;
        }
    }
}

