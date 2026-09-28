using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;
using arc.common.Models.Organisation;

namespace arc.data.Organisation;

/// <summary>
/// Retrieves an organisation based on Id with hierarchy information.
/// </summary>
internal class GetOrganisationForHierarchy : IQueryReturningType<OrganisationModel>
{
    /// <summary>
    /// Executes the query to return the organisation hierarchy information for a specific organisation.
    /// </summary>
    /// <param name="connect">
    /// An open <see cref="NpgsqlConnection"/> against which the query will run.
    /// </param>
    /// <param name="queryFilters">
    /// A <see cref="QueryFilterConfig"/> containing parameters.
    /// Must include an "parentOrganisationId" integer parameter.
    /// </param>
    /// <returns>
    /// A <see cref="Task{OrganisationModel}"/> that resolves to an OrganisationModel.
    /// </returns>
    public async Task<OrganisationModel> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {

        var parentOrganisationId = queryFilters.Parameters.Where(p => p.Key.ToLower() == "parentorganisationid").First().Value;

        return await connect.QueryFirstAsync<OrganisationModel>(@"select id, organisationname, fullyqualifiedname, parentorganisationid, moredata ->> 'organisationcodehierarchy' as organisationcodehierarchy from organisation where id = @ParentOrganisationId", 
            new { ParentOrganisationId = !string.IsNullOrEmpty(parentOrganisationId) ? int.Parse(parentOrganisationId) : (int?)null });
    }
}
