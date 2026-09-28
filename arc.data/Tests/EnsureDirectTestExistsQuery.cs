using arc.common.Models.Tests;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Tests;

/// <summary>
/// Finds a direct test by specimen and trimmed test name, or inserts a requested row.
/// </summary>
internal class EnsureDirectTestExistsQuery : IQueryReturningType<EnsureTestRowResult>
{
    /// <inheritdoc />
    public async Task<EnsureTestRowResult> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var specimenId = queryFilters.GetIntegerValue("specimenid");
        var testName = queryFilters.GetStringValue("testname") ?? string.Empty;

        const string findSql = @"select id from Tests where specimenid = @SpecimenId and lower(trim(testname)) = lower(trim(@TestName))";
        var existing = await connect.QueryFirstOrDefaultAsync<int?>(findSql, new { SpecimenId = specimenId, TestName = testName });
        if (existing.HasValue)
            return new EnsureTestRowResult { Id = existing.Value, WasCreated = false };

        const string insertSql = @"insert into Tests(specimenid, testname, status, lastmodifieddate, requested)
                values(@SpecimenId, @TestName, 'Requested', now(), now()) returning id";
        var newId = await connect.QueryFirstAsync<int>(insertSql, new { SpecimenId = specimenId, TestName = testName });
        return new EnsureTestRowResult { Id = newId, WasCreated = true };
    }
}
