using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Quality;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Quality.Queries
{
    internal class IqcTestByIdQuery : IQueryReturningType<IqcTest>
    {
        public async Task<IqcTest> ExecuteAsync(NpgsqlConnection connection, QueryFilterConfig queryFilters)
        {
            var id = queryFilters.GetIntegerValue("id");

            var sql = @"SELECT 
              it.*, ir.*
              FROM 
			  iqctests it 
              LEFT JOIN iqcresults ir ON ir.iqctestid = it.id
              WHERE it.id = @id;
			  ";

            IqcTest iqcTest = null;

            await connection.QueryAsync<IqcTest, IqcResult, IqcTest>(sql,
            map: (it, ir) =>
            {
                iqcTest ??= it;
                iqcTest.Results ??= new List<IqcResult>();
                if (ir != null && !iqcTest.Results.Any(x => x.Id == ir.Id))
                {
                    iqcTest.Results.Add(ir);
                }
                return it;
            },
            param: new { id });

            return iqcTest;
        }
    }
}
