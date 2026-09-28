using arc.common.Models.Tests;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Tests;

/// <summary>
/// Finds a culture/isolate test by culture and trimmed test name, or inserts a requested row.
/// </summary>
internal class EnsureCultureTestExistsQuery : IQueryReturningType<EnsureTestRowResult>
{
    /// <inheritdoc />
    public async Task<EnsureTestRowResult> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var cultureId = queryFilters.GetIntegerValue("cultureid");
        var testName = queryFilters.GetStringValue("testname") ?? string.Empty;

        const string findSql = @"select id from CultureTests where cultureid = @CultureId and lower(trim(testname)) = lower(trim(@TestName))";
        var existing = await connect.QueryFirstOrDefaultAsync<int?>(findSql, new { CultureId = cultureId, TestName = testName });
        if (existing.HasValue)
            return new EnsureTestRowResult { Id = existing.Value, WasCreated = false };

        const string insertSql = @"insert into CultureTests(cultureid, testname, status, lastmodifieddate, requested)
                values(@CultureId, @TestName, 'Requested', now(), now()) returning id";
        var newId = await connect.QueryFirstAsync<int>(insertSql, new { CultureId = cultureId, TestName = testName });
        return new EnsureTestRowResult { Id = newId, WasCreated = true };
    }
}
