using arc.common.Models.Tests;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Tests;

/// <summary>
/// Query to retrieve direct test list for a specimen, including LaboratoryId for TAT enrichment.
/// </summary>
internal class TestListForSpecimenListQuery : IQueryReturningType<List<TestListResultModel>>
{
    public async Task<List<TestListResultModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var specimenId = queryFilters.GetIntegerValue("SpecimenId");

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
                Tests t
                inner join specimen s on s.id = t.specimenid
                left outer join alerttype al on al.id = t.alerttypeid
            where t.specimenid = @specimenId
            order by t.requested, t.id
            """;

        var result = await connect.QueryAsync<TestListResultModel>(sql, new { specimenId });
        return result.ToList();
    }
}
