using arc.common.Models.Tests;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Tests;

/// <summary>
/// Query to retrieve culture test list for a culture, including LaboratoryId for TAT enrichment.
/// </summary>
internal class TestListForCultureListQuery : IQueryReturningType<List<TestListResultModel>>
{
    public async Task<List<TestListResultModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var cultureId = queryFilters.GetIntegerValue("CultureId");

        const string sql = """
            select
                t.id,
                t.testname,
                t.status,
                t.Requested AT TIME ZONE 'UTC' As Requested,
                t.completed AT TIME ZONE 'UTC' As completed,
                t.testresults,
                al.colour,
                al.alertcategoryid,
                s.id as specimenid,
                s.laboratoryid
            from
                CultureTests t
                inner join culture c on c.id = t.cultureid
                inner join specimen s on s.id = c.specimenid
                left outer join alerttype al on al.id = t.alerttypeid
            where t.cultureid = @cultureId
            order by t.requested
            """;

        var result = await connect.QueryAsync<TestListResultModel>(sql, new { cultureId });
        return result.ToList();
    }
}
