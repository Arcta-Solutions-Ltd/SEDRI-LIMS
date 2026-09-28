using arc.common.Models.Alert;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Alert
{
    internal class StandardSpecimenAlertQuery : IQueryReturningType<List<AlertDetailsModel>>
    {
        public async Task<List<AlertDetailsModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var sql = @"select distinct a.* from alert a where moredata is not null";

            var result = await connect.QueryAsync<AlertDetailsModel>(sql);

            return result.ToList();
        }
    }
}
