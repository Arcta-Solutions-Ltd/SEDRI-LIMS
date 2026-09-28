using arc.app.Common;
using arc.app.Coding;
using arc.common.Models.Laboratory;
using arc.common.Utils;
using arc.data.model.Laboratory;
using arc.data.model.Specimen;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Laboratory;

/// <summary>
/// Handles the loading and processing of laboratory configurations.
/// </summary>
public class LaboratoryConfigurationHandler : ILaboratoryConfigurationHandler
{
    private readonly IGeneralRepository _generalRepository;
    private readonly IListRepository _listRepository;
    private readonly IOrganismRepository _organismRepository;
    private readonly IApplicableIsolateTestsResolver _applicableIsolateTestsResolver;
    private readonly ILogger<LaboratoryConfigurationHandler> _logger;
    private IEnumerable<LaboratoryConfigsModel> _configList;

    /// <summary>
    /// Gets the list of processed laboratory configurations.
    /// </summary>
    public LaboratoryConfigurationListModel LaboratoryConfigurationList { get; } = new LaboratoryConfigurationListModel { LaboratoryList = new List<LaboratoryConfigurationModel>() };

    /// <summary>
    /// Initializes a new instance of the <see cref="LaboratoryConfigurationHandler"/> class.
    /// </summary>
    /// <param name="generalRepository">The repository for general data access.</param>
    /// <param name="listRepository">The repository for list-based data access.</param>
    /// <param name="organismRepository">The organism repository for organism scope description resolution.</param>
    /// <param name="applicableIsolateTestsResolver">The resolver for applicable isolate tests by culture type and organism scope.</param>
    /// <param name="logger">The logger used to record isolate test resolution diagnostics.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="generalRepository"/>, <paramref name="listRepository"/>, <paramref name="organismRepository"/>, <paramref name="applicableIsolateTestsResolver"/>, or <paramref name="logger"/> is null.
    /// </exception>
    public LaboratoryConfigurationHandler(IGeneralRepository generalRepository, IListRepository listRepository, IOrganismRepository organismRepository, IApplicableIsolateTestsResolver applicableIsolateTestsResolver, ILogger<LaboratoryConfigurationHandler> logger)
    {
        _generalRepository = generalRepository ?? throw new ArgumentNullException(nameof(generalRepository));
        _listRepository = listRepository ?? throw new ArgumentNullException(nameof(listRepository));
        _organismRepository = organismRepository ?? throw new ArgumentNullException(nameof(organismRepository));
        _applicableIsolateTestsResolver = applicableIsolateTestsResolver ?? throw new ArgumentNullException(nameof(applicableIsolateTestsResolver));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Loads configuration details for a single laboratory identified by <paramref name="laboratoryId"/>.
    /// </summary>
    /// <param name="laboratoryId">The identifier of the laboratory to load configurations for.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    public async Task LoadSingleLaboratoryConfigurationAsync(int laboratoryId)
    {
        await GetConfigsFromDatabaseAsync();

        var filteredList = _configList.Where(config => config.LaboratoryId == laboratoryId).ToList();
        var laboratoryRecord = await _generalRepository.GetByIdAsync<LaboratoryDataModel>("laboratory", laboratoryId);
        var returnList = new List<LaboratoryConfigsModel>();

        foreach (var config in filteredList)
        {
            if (config.ConfigName == "organismscopeculturetestoption")
            {
                var scopeDetails = ArcJson.Deserialize<OrganismScopeCultureTestOptionModel>(config.Contents);
                config.GroupText = await _organismRepository.GetOrganismScopeDescriptionAsync(
                    scopeDetails.OrderId, scopeDetails.FamilyId, scopeDetails.GenusId, scopeDetails.SpeciesId,
                    scopeDetails.SubspeciesId, scopeDetails.SerotypeId, scopeDetails.OrgGroupCodingId, scopeDetails.OrganismId);
                config.GroupId = scopeDetails.OrganismId > 0 ? scopeDetails.OrganismId.ToString() : "organismscope";
                config.AssociatedListId = scopeDetails.AssociatedListId ?? "";
            }
            else
            {
                var details = ArcJson.Deserialize<LaboratoryDetailsModel>(config.Contents);

                config.GroupText = (config.ConfigName is "testcategory" or "culturetypecategory")
                    ? await _listRepository.GetValueFromIdAsync(int.Parse(details.GroupId))
                    : "";

                config.GroupId = details.GroupId;
                config.AssociatedListId = details.AssociatedListId;
            }
            returnList.Add(config);
        }

        LaboratoryConfigurationList.LaboratoryList.Add(new LaboratoryConfigurationModel
        {
            Configuration = returnList,
            LaboratoryId = laboratoryId,
            DefaultWorkflowId = laboratoryRecord.DefaultWorkflowId,
            ApproveReports = laboratoryRecord.ApproveReports,
            RecordSusceptibilityChangeAudit = laboratoryRecord.RecordSusceptibilityChangeAudit
        });
    }

    /// <summary>
    /// Loads configurations for all distinct laboratories.
    /// </summary>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    public async Task LoadConfigurationForAllLaboratoriesAsync()
    {
        await GetConfigsFromDatabaseAsync();
        var laboratoryRecords = await _generalRepository.GetAllAsync<LaboratoryDataModel>("laboratory");

        var laboratoryList = laboratoryRecords.Select(lab => lab.Id).Distinct();
        var loadTasks = laboratoryList.Select(LoadSingleLaboratoryConfigurationAsync);

        await Task.WhenAll(loadTasks);
    }

    /// <summary>
    /// Loads the configuration for a specified specimen by retrieving its record 
    /// and applying the appropriate laboratory configuration.
    /// </summary>
    /// <param name="specimenId">The unique identifier of the specimen.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task LoadConfigurationForSpecimenAsync(int specimenId)
    {
        var specimenRecord = await _generalRepository.GetByIdAsync<SpecimenDataModel>("specimen", specimenId);
        await LoadSingleLaboratoryConfigurationAsync(specimenRecord.LaboratoryId);
    }

    /// <summary>
    /// Returns the list of isolate test form names applicable for the given culture type and organism.
    /// Filters by culture type first, then by organism scope when an organism is set.
    /// If the laboratory has a non-empty resistance-mechanism isolate test name list, intersects with that whitelist.
    /// </summary>
    public async Task<IReadOnlyList<string>> GetApplicableIsolateTestsForCultureAsync(int laboratoryId, int cultureTypeId, int organismId, int orgGroupCodingId)
    {
        await GetConfigsFromDatabaseAsync();

        var labConfigs = _configList
            .Where(c => c.LaboratoryId == laboratoryId)
            .ToList();

        var baseList = await _applicableIsolateTestsResolver.GetApplicableIsolateTestsForCultureAsync(
            labConfigs, cultureTypeId, organismId, orgGroupCodingId);

        var laboratory = await _generalRepository.GetByIdAsync<LaboratoryDataModel>("laboratory", laboratoryId);
        var labWhitelist = laboratory?.ResistanceMechanismIsolateTestNames;

        if (string.IsNullOrWhiteSpace(labWhitelist))
            return baseList;

        // Empty base: intersection is empty (resolver found no applicable tests).
        if (baseList != null && baseList.Count == 0)
            return baseList;

        // Resolver returned null (e.g. no culture-type / organism-scope match). Still honour the lab multiselect
        // so AST does not fall back to showing every culture-type test from Redux.
        if (baseList == null)
            return IsolateTestNameWhitelist.ParseCommaSeparatedNames(labWhitelist);

        return IsolateTestNameWhitelist.IntersectWithLaboratorySelection(baseList, labWhitelist);
    }

    /// <summary>
    /// Returns the list of isolate test form names selectable on the Select Isolate Tests form for the
    /// given culture type and organism. Applies the culture type and organism scope configuration only.
    /// The laboratory <c>ResistanceMechanismIsolateTestNames</c> whitelist is deliberately not applied
    /// here because it restricts the AST screen alone.
    /// </summary>
    /// <param name="laboratoryId">The laboratory ID.</param>
    /// <param name="cultureTypeId">The culture type ID.</param>
    /// <param name="organismId">The organism ID (0 if not set).</param>
    /// <param name="orgGroupCodingId">The organism group coding ID (0 if using specific organism).</param>
    /// <returns>List of applicable isolate test form names, or null when no configuration applies and every isolate test should be selectable.</returns>
    public async Task<IReadOnlyList<string>> GetApplicableIsolateTestsForSelectionAsync(int laboratoryId, int cultureTypeId, int organismId, int orgGroupCodingId)
    {
        await GetConfigsFromDatabaseAsync();

        var labConfigs = _configList
            .Where(c => c.LaboratoryId == laboratoryId)
            .ToList();

        var applicableTests = await _applicableIsolateTestsResolver.GetApplicableIsolateTestsForCultureAsync(
            labConfigs, cultureTypeId, organismId, orgGroupCodingId);

        _logger.LogInformation(
            "GetApplicableIsolateTestsForSelection resolved for LaboratoryId={LaboratoryId}, CultureTypeId={CultureTypeId}, OrganismId={OrganismId}, OrgGroupCodingId={OrgGroupCodingId}. LaboratoryConfigCount={LaboratoryConfigCount}, ApplicableTestCount={ApplicableTestCount}, ConfigurationApplies={ConfigurationApplies}.",
            laboratoryId,
            cultureTypeId,
            organismId,
            orgGroupCodingId,
            labConfigs.Count,
            applicableTests?.Count ?? 0,
            applicableTests != null);

        if (applicableTests != null)
        {
            _logger.LogDebug(
                "GetApplicableIsolateTestsForSelection applicable isolate test names for LaboratoryId={LaboratoryId}, CultureTypeId={CultureTypeId}: {ApplicableTestNames}.",
                laboratoryId,
                cultureTypeId,
                string.Join(", ", applicableTests));
        }

        return applicableTests;
    }

    /// <summary>
    /// Retrieves configuration data from the database if not already loaded.
    /// </summary>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    private async Task GetConfigsFromDatabaseAsync()
    {
        _configList ??= await _generalRepository.GetAllAsync<LaboratoryConfigsModel>("LaboratoryConfigs");
    }
}
