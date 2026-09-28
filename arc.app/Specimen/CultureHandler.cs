using arc.common.Models;
using arc.common.Models.Specimen;
using arc.domain.Configuration.QueryFiltersConfig;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Specimen;

/// <summary>
/// Handles operations related to culture data retrieval and transformation.
/// </summary>
public class CultureHandler : ICultureHandler
{
    /// <summary>
    /// Repository for accessing culture-related data.
    /// </summary>
    private readonly ICultureRepository _cultureRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="CultureHandler"/> class.
    /// </summary>
    /// <param name="cultureRepository">The culture repository to use for data access.</param>
    public CultureHandler(ICultureRepository cultureRepository)
    {
        _cultureRepository = cultureRepository;
    }

    /// <summary>
    /// Retrieves a culture by ID using the provided query filters, and returns it as a JSON string.
    /// </summary>
    /// <param name="queryFilters">The query filters containing parameters for the lookup.</param>
    /// <returns>A JSON string representing the culture data.</returns>
    public async Task<string> GetCultureByIdAsync(QueryFilterConfig queryFilters)
    {
        var id = queryFilters.Parameters.Where(p => p.Key.ToLower() == "id").First();

        var culture = await _cultureRepository.GetCultureByIdAsync(queryFilters);

        if (culture != null)
        {

            var craftedKeyValuePairs = new List<JsonKeyValuePairModel>
            {
                new() { Key = "culturetypeid", value = culture.CultureType },
                new() { Key = "OrganismId", value = culture.OrganismId },
                new() { Key = "organismname", value = culture.OrganismName },
                new() { Key = "orggroupcodingid", value = culture.OrgGroupCoding }
            };

            var craftedModels = new List<CraftedModel>
            {
                new() { Name = "cultureorganismpage", Contents = JsonConvert.SerializeObject(craftedKeyValuePairs) }
            };

            culture.Crafted = craftedModels;
        }

        return JsonConvert.SerializeObject(culture ?? new CultureModel());
    }
}
