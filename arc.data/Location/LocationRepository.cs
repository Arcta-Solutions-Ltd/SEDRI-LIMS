using arc.app.Common;
using arc.app.Location;
using arc.common.Data;
using arc.common.Utils;
using arc.data.Common;
using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Location;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.data.Location;

/// <summary>
/// Represents the repository for managing location-related data operations.
/// </summary>
public class LocationRepository(ISqlCommand sqlCommand,ISqlQuery sqlQuery,ILogWriter logWriter,IGenerateMoreData moreDataGenerator) : GeneralRepository(sqlQuery, logWriter, sqlCommand), ILocationRepository
{
    private readonly IGenerateMoreData _moreDataGenerator = moreDataGenerator;

    /// <summary>
    /// Adds a new location record asynchronously.
    /// </summary>
    /// <param name="dataToSave">The location data in JSON format to be added.</param>
    /// <returns>The ID of the newly added location record.</returns>
    public async Task<int> AddAsync(string dataToSave)
    {
        var location = JsonConvert.DeserializeObject<LocationModel>(dataToSave);
        location.MoreData = _moreDataGenerator.GetMoreDataJsonString("location", dataToSave);
        _logWriter.LogInfo("Run add location command", "LocationRepository", "Add");
        return await _sqlCommand.CommandWithTypeQueryAsync(new AddLocationCommand(), "Add Location", location);
    }

    /// <summary>
    /// Edits an existing location record asynchronously.
    /// </summary>
    /// <param name="dataToSave">The location data in JSON format to be updated.</param>
    /// <returns>The ID of the edited location record.</returns>
    public async Task<int> EditAsync(string dataToSave)
    {
        var location = ArcJson.Deserialize<LocationModel>(dataToSave);
        location.MoreData = _moreDataGenerator.GetMoreDataJsonString("location", dataToSave);
        _logWriter.LogInfo("Run edit location command", "LocationRepository", "Edit");
        return await _sqlCommand.CommandWithTypeQueryAsync(new EditLocationCommand(), "Edit Location", location);
    }

    /// <summary>
    /// Retrieves a list of location options for dropdown selection asynchronously.
    /// </summary>
    /// <returns>A collection of location options.</returns>
    public async Task<IEnumerable<OptionsConfig>> GetLocationsForListAsync()
    {
        _logWriter.LogInfo("Run get location for dropdown list query", "LocationRepository", "GetLocationsForList");
        return await _sqlQuery.QueryReturningTypeAsync(new GetLocationsForListQuery(), "Get Locations for list Query", new QueryFilterConfig());
    }

    /// <summary>
    /// Retrieves the location hierarchy for a specified location ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the location for which the hierarchy is to be retrieved.</param>
    /// <returns>A string representation of the location hierarchy.</returns>
    public async Task<string> GetLocationHierarchyAsync(string id)
    {
        var queryFilter = new QueryFilterConfig
        {
            Parameters = new List<QueryValuesConfig>
            {
                new QueryValuesConfig { Key = "locationid", Value = id }
            }
        };
        _logWriter.LogInfo("Run location hierarchy query", "LocationRepository", "GetLocationHierarchy");
        return await _sqlQuery.QueryReturningStringAsync(new LocationHierarchyListQuery(), "Get Location Hierarchy Query", queryFilter);
    }
}

