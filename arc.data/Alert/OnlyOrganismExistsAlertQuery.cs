using arc.common.Models.Alert;
using arc.domain.Configuration.QueryFiltersConfig;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.data.Alert
{
    internal class OnlyOrganismExistsAlertQuery : IQueryReturningType<List<AlertDetailsModel>>
    {
        public async Task<List<AlertDetailsModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var alertUtils = new AlertQueryUtils(connect, queryFilters);
            return await alertUtils.GetAlerts();
        }
    }
}
