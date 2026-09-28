using arc.common.Models.Alert;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Alert;

internal class SpecimenAlertListQuery : IQueryReturningType<List<SpecimenAlertListModel>>
{
    public async Task<List<SpecimenAlertListModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var sql = @"select s.AccessionNumber, a.AlertName, p.Firstname, p.Surname, s.ReceivedDate, li.value as state from specimenalert sa
                        inner join specimen s on s.id = sa.specimenid
                        inner join patient p on p.id = s.patientid
                        inner join alert a on a.id = sa.alertid
                        inner join ListItem li on li.Id = s.stateid
                        order by s.ReceivedDate desc
                        limit 500";

        var result = await connect.QueryAsync<SpecimenAlertListModel>(sql);

        return result.ToList();
    }
}
