using arc.app.Config;
using arc.app.Tests;
using arc.common.Models;
using arc.common.Models.Specimen;
using arc.domain.Configuration.QueryFiltersConfig;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Specimen;

/// <summary>
/// Loads and crafts acknowledgment receipt data for specimens.
/// </summary>
public class ACKReceiptLoader : IACKReceiptLoader
{
    private readonly ISpecimenRepository _specimenRepository;
    private readonly ITestSelectionHandler _testSelectionHandler;
    private readonly IListViewConfigFactory _viewConfigFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="ACKReceiptLoader"/> class.
    /// </summary>
    /// <param name="specimenRepository">Repository for retrieving specimen data.</param>
    /// <param name="testSelectionHandler">Handler for selecting and configuring tests.</param>
    /// <param name="viewConfigFactory">Factory to obtain list view configurations.</param>
    public ACKReceiptLoader(
        ISpecimenRepository specimenRepository,
        ITestSelectionHandler testSelectionHandler,
        IListViewConfigFactory viewConfigFactory)
    {
        _specimenRepository = specimenRepository;
        _testSelectionHandler = testSelectionHandler;
        _viewConfigFactory = viewConfigFactory;
    }

    /// <summary>
    /// Asynchronously loads and crafts the acknowledgment receipt pages data based on the provided filter and user token.
    /// </summary>
    /// <param name="queryFilter">Configuration containing query parameters for loading specimen data.</param>
    /// <param name="token">User token information for authentication and authorization.</param>
    /// <returns>
    /// A JSON string representing the finalized <see cref="AcknowledgeReceiptCraftedPages"/> with defaults applied.
    /// </returns>
    public async Task<string> LoadDataAsync(QueryFilterConfig queryFilter, TokenInfoModel token)
    {
        var id = queryFilter.Parameters
            .Where(p => p.Key.Equals("id", StringComparison.OrdinalIgnoreCase))
            .First();

        var view = await _viewConfigFactory.GetViewAsync("specimens");

        var specimenData = await _specimenRepository.SpecimenByIdAsync(queryFilter);

        var testsForSpecimen = await _testSelectionHandler
            .GetTestListWithUsedTestsEnabledAsync(id.Value, "tests", token);

        var cultureTypeList = await _testSelectionHandler.GetCultureTypeListAsync();

        var returnModel = _testSelectionHandler.CreateReturnModel(testsForSpecimen, "tests");

        returnModel.Crafted.Add(new CraftedModel
        {
            Name = "culturetypeselectionpage",
            Contents = JsonConvert.SerializeObject(cultureTypeList.OrderBy(o => o.Name))
        });

        var craftedString = JsonConvert.SerializeObject(returnModel);

        var craftedData = JsonConvert.DeserializeObject<AcknowledgeReceiptCraftedPages>(craftedString);

        craftedData.ApplyDefaults = "Yes";
        craftedData.Id = specimenData.Id;
        craftedData.LaboratoryId = specimenData.LaboratoryId;
        craftedData.SpecimenTypeId = specimenData.SpecimenTypeId.ToString();

        return JsonConvert.SerializeObject(craftedData);
    }
}
