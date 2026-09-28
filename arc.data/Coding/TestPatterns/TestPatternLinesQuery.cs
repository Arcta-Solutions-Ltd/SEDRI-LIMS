using arc.common.Models.Coding;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Coding;

public class TestPatternLinesQuery : IQueryReturningType<List<TestPatternLineModel>>
{
    public async Task<List<TestPatternLineModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var sql = @"SELECT * FROM TestPatternLine WHERE TestPatternId = @Id";

        var testpatternlines = await connect.QueryAsync<TestPatternLineModel>(sql, new { Id = int.Parse(queryFilters.Parameters[0].Value) });


        return testpatternlines.ToList();
    }
}