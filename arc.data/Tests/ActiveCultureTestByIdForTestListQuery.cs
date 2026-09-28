using arc.common.Models.Tests;
using arc.data.Utils;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Tests;

internal class ActiveCultureTestByIdForTestListQuery : IQueryReturningType<TestListResultModel>
{
    public async Task<TestListResultModel> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var cultureTestId = queryFilters.GetIntegerValue("id");
        var securityClause = QuerySecurity.GetWhereClause(queryFilters);

        var sql = $"""
            select
                t.id,
                t.testname,
                t.status,
                t.Requested AT TIME ZONE 'UTC' As Requested,
                t.completed AT TIME ZONE 'UTC' As completed,
                t.testresults,
                al.colour,
                al.alertcategoryid,
                s.accessionnumber,
                p.firstname,
                p.surname,
                p.patientref,
                li.value as specimentype,
                li2.value as culturetype
            from
                CultureTests t
                inner join culture c on c.id = t.cultureid
                inner join specimen s on s.id = c.specimenid
                inner join patient p on p.id = s.patientid
                inner join listitem li on li.id = s.specimentypeid
                left outer join alerttype al on al.id = t.alerttypeid
                left outer join listitem li2 on li2.id = c.typeid
            where
                t.Id = @cultureTestId
                and s.{securityClause}
            """;

        return await connect.QueryFirstAsync<TestListResultModel>(sql, new { cultureTestId });
    }
}
