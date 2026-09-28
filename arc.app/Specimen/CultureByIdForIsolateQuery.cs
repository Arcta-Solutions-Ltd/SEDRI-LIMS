using arc.app.Common;
using arc.common.Models;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Specimen;

/// <summary>
/// Executes a specialized query to retrieve culture data by ID for isolate workflows.
/// Implements <see cref="IQueryRun"/> to support asynchronous query execution.
/// </summary>
internal class CultureByIdForIsolateQuery : IQueryRun
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="CultureByIdForIsolateQuery"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider used to resolve dependencies.</param>
    public CultureByIdForIsolateQuery(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Executes the query using the provided filter and token, retrieves culture data,
    /// clears the <c>OrganismId</c> field, and returns the result as a JSON string.
    /// </summary>
    /// <param name="queryFilter">The filter configuration used to identify the culture record.</param>
    /// <param name="token">The token containing user or session context.</param>
    /// <returns>A JSON-formatted string representing the culture data with <c>OrganismId</c> cleared.</returns>
    public async Task<string> RunAsync(QueryFilterConfig queryFilter, TokenInfoModel token)
    {
        var cultureRepository = _serviceProvider.GetService<ICultureRepository>();
        var result = await cultureRepository.GetCultureByIdAsync(queryFilter);
        result.OrganismId = string.Empty;
        result.ManufacturersBarcode = string.Empty;
        result.Quantity = string.Empty;
        result.IdPercentage = string.Empty;
        result.AliquotID = string.Empty;

        var craftedKeyValuePairs = new List<JsonKeyValuePairModel>
            {
                new() { Key = "culturetypeid", value = result.CultureType }
            };

        var craftedModels = new List<CraftedModel>
        {
            new() { Name = "cultureorganismpage", Contents = JsonConvert.SerializeObject(craftedKeyValuePairs) }
        };

        result.Crafted = craftedModels;
        return JsonConvert.SerializeObject(result);
    }
}
