using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Organisation
{
    internal class GetOrganisationForValidationByIdQuery : IQueryReturningInteger
    {
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var organisationName = queryFilters.Parameters.Where(p => p.Key.ToLower() == "organisationname").First().Value;
            var parentOrganisationInterim = queryFilters.Parameters.Where(p => p.Key.ToLower() == "parentorganisationid").FirstOrDefault();

            var sql = @"select count(id) from Organisation s where s.organisationname = @organisationName and s.parentorganisationid is null";
            var parentOrganisationId = 0;

            if (parentOrganisationInterim != null)
            {
                parentOrganisationId = int.Parse(parentOrganisationInterim.Value);
                sql = @"select count(id) from Organisation s where s.organisationname = @organisationName and s.parentorganisationid = @parentOrganisationId";
            }
            var init = await connect.QueryFirstAsync<long>(sql, new { organisationName, parentOrganisationId });

            var result = Convert.ToInt32(init);

            return result;
        }
    }
}