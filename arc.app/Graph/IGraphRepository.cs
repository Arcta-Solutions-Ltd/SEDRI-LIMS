using arc.common.Models;
using arc.common.Models.Graph;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Graph;

/// <summary>
/// Defines methods for retrieving graph data across various specimen-related dimensions
/// based on dynamic filter configurations.
/// </summary>
public interface IGraphRepository
{
    /// <summary>
    /// Retrieves graph data aggregated by specimen type.
    /// </summary>
    /// <param name="queryFilters">
    /// Configuration parameters used to filter and group the specimen type data.
    /// </param>
    /// <returns>
    /// A list of <see cref="GraphModel"/> items representing specimen type graph points.
    /// </returns>
    Task<List<GraphModel>> GetSpecimenTypeGraphDataAsync(QueryFilterConfig queryFilters);

    /// <summary>
    /// Retrieves graph data aggregated by location.
    /// </summary>
    /// <param name="queryFilters">
    /// Configuration parameters used to filter and group the location data.
    /// </param>
    /// <returns>
    /// A list of <see cref="GraphModel"/> items representing location graph points.
    /// </returns>
    Task<List<GraphModel>> GetLocationGraphDataAsync(QueryFilterConfig queryFilters);

    /// <summary>
    /// Retrieves graph data aggregated by tag values.
    /// </summary>
    /// <param name="queryFilters">
    /// Configuration parameters used to filter and group the tag data.
    /// </param>
    /// <returns>
    /// A list of <see cref="GraphModel"/> items representing tag graph points.
    /// </returns>
    Task<List<GraphModel>> GetTagGraphDataAsync(QueryFilterConfig queryFilters);

    /// <summary>
    /// Retrieves graph data aggregated by organisation.
    /// </summary>
    /// <param name="queryFilters">
    /// Configuration parameters used to filter and group the organisation data.
    /// </param>
    /// <returns>
    /// A list of <see cref="GraphModel"/> items representing organisation graph points.
    /// </returns>
    Task<List<GraphModel>> GetOrganisationGraphDataAsync(QueryFilterConfig queryFilters);

    /// <summary>
    /// Retrieves graph data aggregated by organism.
    /// </summary>
    /// <param name="queryFilters">
    /// Configuration parameters used to filter and group the organism data.
    /// </param>
    /// <returns>
    /// A list of <see cref="GraphModel"/> items representing organism graph points.
    /// </returns>
    Task<List<GraphModel>> GetOrganismGraphDataAsync(QueryFilterConfig queryFilters);

    /// <summary>
    /// Retrieves graph data aggregated by susceptibility results.
    /// </summary>
    /// <param name="queryFilters">
    /// Configuration parameters used to filter and group the susceptibility data.
    /// </param>
    /// <returns>
    /// A list of <see cref="GraphModel"/> items representing susceptibility graph points.
    /// </returns>
    Task<List<GraphModel>> GetSusceptibilityGraphDataAsync(QueryFilterConfig queryFilters);

    /// <summary>
    /// Retrieves graph data aggregated by test names.
    /// </summary>
    /// <param name="queryFilters">
    /// Configuration parameters used to filter and group the test data.
    /// </param>
    /// <returns>
    /// A list of <see cref="GraphModel"/> items representing test graph points.
    /// </returns>
    Task<List<GraphModel>> GetTestGraphDataAsync(QueryFilterConfig queryFilters);

    /// <summary>
    /// Retrieves graph data aggregated by gender.
    /// </summary>
    /// <param name="queryFilters">
    /// Configuration parameters used to filter and group the gender data.
    /// </param>
    /// <returns>
    /// A list of <see cref="GraphModel"/> items representing gender graph points.
    /// </returns>
    Task<List<GraphModel>> GetGenderGraphDataAsync(QueryFilterConfig queryFilters);

    /// <summary>
    /// Retrieves graph data aggregated by specimen workflow state.
    /// </summary>
    /// <param name="queryFilters">Filter configuration for the specimen state graph query.</param>
    /// <returns>Graph points for each state.</returns>
    Task<List<GraphModel>> GetSpecimenStateGraphDataAsync(QueryFilterConfig queryFilters);
}
