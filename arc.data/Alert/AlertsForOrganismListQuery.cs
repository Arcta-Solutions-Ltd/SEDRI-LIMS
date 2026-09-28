using arc.common.Models.Alert;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Alert
{
    /// <summary>
    /// Data query that returns alerts configured for a specific organism.
    /// </summary>
    internal class AlertsForOrganismListQuery : IQueryReturningType<List<OrganismAlertListModel>>
    {
        public async Task<List<OrganismAlertListModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var organismId = queryFilters.Parameters.Where(p => p.Key.ToLower() == "organismid").First();

            var sql = @"select al.Id, al.AlertName, al.enabled, al.alerttypeid, al.specificationid from alert al where organismid = @OrganismId";

            var result = await connect.QueryAsync<OrganismAlertListModel>(sql, new { organismId = int.Parse(organismId.Value) });

            return result.ToList();
        }
    }
}
