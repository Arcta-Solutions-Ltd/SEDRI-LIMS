using arc.common.Models.Instruments;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Instruments;

/// <summary>
/// Returns <see cref="InstrumentResultsListModel"/> rows for the test record view: direct tests match
/// <c>instrumentresults.moredata</c> <c>directTestName</c> to <c>Tests.TestName</c>; culture/isolate tests match
/// <c>cultureTestName</c> to <c>CultureTests.TestName</c> for the given culture.
/// </summary>
internal class TestInstrumentResultsQuery : IQueryReturningType<List<InstrumentResultsListModel>>
{
    /// <summary>
    /// Executes the query. Required parameters: <c>id</c> (direct test row id in <c>tests</c> or culture test row id in <c>culturetests</c>),
    /// <c>source</c> (<c>direct</c> or <c>culture</c>).
    /// </summary>
    public async Task<List<InstrumentResultsListModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var idParam = queryFilters.Parameters.FirstOrDefault(p => p.Key.Equals("id", StringComparison.OrdinalIgnoreCase))?.Value;
        var sourceParam = queryFilters.Parameters.FirstOrDefault(p => p.Key.Equals("source", StringComparison.OrdinalIgnoreCase))?.Value;

        if (string.IsNullOrWhiteSpace(idParam))
            throw new ArgumentException("Id is missing in query filters.");
        if (string.IsNullOrWhiteSpace(sourceParam))
            throw new ArgumentException("Source is missing in query filters.");

        var testId = int.Parse(idParam);
        var source = sourceParam.Trim().ToLowerInvariant();

        string sql;
        object param;

        if (source == "culture")
        {
            sql = @"SELECT 
                        ir.id::text AS id,
                        ir.instrumentprofile AS instrumentprofile, 
                        li1.value AS status, 
                        CASE WHEN ir.resultreceived IS NULL THEN '' ELSE TO_CHAR(ir.resultreceived AT TIME ZONE 'UTC', 'YYYY-MM-DD HH24:MI:SS') END AS ResultReceived,
                        CASE WHEN ir.requestmade IS NULL THEN '' ELSE TO_CHAR(ir.requestmade AT TIME ZONE 'UTC', 'YYYY-MM-DD HH24:MI:SS') END AS RequestMade
                    FROM instrumentresults ir
                    INNER JOIN listitem li1 ON ir.statusid = li1.id
                    INNER JOIN culturetests ct ON ct.id = @testid
                    WHERE ir.cultureid = ct.cultureid
                      AND lower(trim(COALESCE(ir.moredata::jsonb->>'cultureTestName', ''))) = lower(trim(ct.testname))
                    ORDER BY ir.requestmade";
            param = new { testid = testId };
        }
        else if (source == "direct")
        {
            sql = @"SELECT 
                        ir.id::text AS id,
                        ir.instrumentprofile AS instrumentprofile, 
                        li1.value AS status, 
                        CASE WHEN ir.resultreceived IS NULL THEN '' ELSE TO_CHAR(ir.resultreceived AT TIME ZONE 'UTC', 'YYYY-MM-DD HH24:MI:SS') END AS ResultReceived,
                        CASE WHEN ir.requestmade IS NULL THEN '' ELSE TO_CHAR(ir.requestmade AT TIME ZONE 'UTC', 'YYYY-MM-DD HH24:MI:SS') END AS RequestMade,
                        ir.rawresult AS testresults 
                    FROM instrumentresults ir
                    INNER JOIN listitem li1 ON ir.statusid = li1.id
                    INNER JOIN tests t ON t.id = @testid
                    WHERE ir.specimenid = t.specimenid
                      AND (ir.cultureid IS NULL OR ir.cultureid = 0)
                      AND lower(trim(COALESCE(ir.moredata::jsonb->>'directTestName', ''))) = lower(trim(t.testname))
                    ORDER BY ir.requestmade";
            param = new { testid = testId };
        }
        else
        {
            throw new ArgumentException($"Source must be 'direct' or 'culture'; got '{sourceParam}'.");
        }

        var result = await connect.QueryAsync<InstrumentResultsListModel>(sql, param);
        return result.ToList();
    }
}

