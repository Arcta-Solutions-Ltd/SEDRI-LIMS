using arc.domain.Coding;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Coding
{
    internal class EditTestPatternQuery : IQueryReturningType<TestPattern>
    {
        public async Task<TestPattern> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var id = queryFilters.Parameters.Where(p => p.Key.ToLower() == "id").First();

            var sql = @"select tp.*, st.specimentypeid, od.name as order, f.name as family, l.value as organismgroup from testpattern tp
                        left outer join ordercat od on od.id = tp.orderid
                        left outer join family f on f.id = tp.familyid
                        left outer join listitem l on l.id = tp.orggroupcodingid
                        left outer join 
                        (select testpatternid, STRING_AGG(cast(specimentypeid as varchar(7)), ',') as specimentypeid from specimentypetestpattern group by testpatternid) st
                        on st.testpatternId = tp.id
                        where tp.id = @Id";

            var testPattern = await connect.QueryFirstAsync<TestPattern>(sql, new { Id = int.Parse(id.Value) });

            sql = @"select * from testpatternline where testpatternid = @TestPatternId";
            var lines = await connect.QueryAsync<TestPatternLine>(sql, new { TestPatternId = int.Parse(id.Value) });
            testPattern.AntibioticGrid = lines.ToList();

            return testPattern;
        }
    }
}
