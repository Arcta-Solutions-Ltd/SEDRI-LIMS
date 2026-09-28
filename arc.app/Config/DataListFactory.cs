using arc.app.Alert;
using arc.app.Asset;
using arc.app.Coding;
using arc.app.Common;
using arc.app.Configuration;
using arc.app.Config.Forms;
using arc.app.Configuration.Queries;
using arc.app.Exports;
using arc.app.Import;
using arc.app.Instruments;
using arc.app.Location;
using arc.app.Quality;
using arc.app.Roles;
using arc.app.Security;
using arc.app.Specification;
using arc.app.SystemConfig;
using arc.common.Models;
using arc.domain.Configuration.ListsConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Config;

/// <summary>
/// Factory class that creates list configurations based on the specified list name.
/// This factory aggregates various repository and data handler dependencies to construct
/// a <see cref="ListConfig"/> object containing the options for different list types.
/// </summary>
public class DataListFactory : IDataListFactory
{
    private readonly IListRepository _listRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IOrganisationRepository _organisationRepository;
    private readonly ILaboratoryRepository _laboratoryRepository;
    private readonly IAntibioticRepository _antibioticRepository;
    private readonly IOrganismRepository _organismRepository;
    private readonly ILocationRepository _locationRepository;
    private readonly IConfigListDataHandler _configListDataHandler;
    private readonly IAlertRepository _alertRepository;
    private readonly ISpecimenEventListQuery _specimenEventListQuery;
    private readonly IListFilterHandler _listFilterHandler;
    private readonly IQualityRepository _qualityRepository;
    private readonly IExportProfileConfigHandler _exportProfileConfigHandler;
    private readonly IExportProfileRepository _exportProfileRepository;
    private readonly ITestPatternRepository _testPatternRepository;
    private readonly IFieldListHandler _fieldListHandler;
    private readonly IProfileListHandler _profileListHandler;
    private readonly ICommentRepository _commentRepository;
    private readonly IConfigRepository _configRepository;
    private readonly IStorageRepository _storageRepository;
    private readonly ISpecificationRepository _specificationRepository;
    private readonly IFormConfigAdapter _formConfigAdapter;

    public DataListFactory(IListRepository listRepository, IRoleRepository roleRepository, IOrganisationRepository organisationRepository, IConfigListDataHandler configListDataHandler,
        ILaboratoryRepository laboratoryRepository, IAntibioticRepository antibioticRepository, IOrganismRepository organismRepository, IAlertRepository alertRepository, ILocationRepository locationRepository,
        ISpecimenEventListQuery specimenEventListQuery, IListFilterHandler listFilterHandler, IQualityRepository qualityRepository, IExportProfileConfigHandler exportProfileConfigHandler,
        IExportProfileRepository exportProfileRepository, ITestPatternRepository testPatternRepository, IFieldListHandler fieldListHandler, IProfileListHandler profileListHandler, ICommentRepository commentRepository, IStorageRepository storageRepository,
        IConfigRepository configRepository, ISpecificationRepository specificationRepository, IFormConfigAdapter formConfigAdapter)

    
    {
        _formConfigAdapter = formConfigAdapter;
        _listRepository = listRepository;
        _roleRepository = roleRepository;
        _organisationRepository = organisationRepository;
        _laboratoryRepository = laboratoryRepository;
        _antibioticRepository = antibioticRepository;
        _organismRepository = organismRepository;
        _configListDataHandler = configListDataHandler;
        _alertRepository = alertRepository;
        _locationRepository = locationRepository;
        _specimenEventListQuery = specimenEventListQuery;
        _listFilterHandler = listFilterHandler;
        _qualityRepository = qualityRepository;
        _exportProfileConfigHandler = exportProfileConfigHandler;
        _exportProfileRepository = exportProfileRepository;
        _testPatternRepository = testPatternRepository;
        _fieldListHandler = fieldListHandler;
        _profileListHandler = profileListHandler;
        _commentRepository = commentRepository;
        _storageRepository = storageRepository;
        _configRepository = configRepository;
        _specificationRepository = specificationRepository;
    }

    /// <summary>
    /// Retrieves a list configuration for the specified list name.
    /// The method evaluates the provided list name (converted to lowercase) and returns
    /// the corresponding <see cref="ListConfig"/> object with its options populated by the 
    /// appropriate repository or data handler. If no matching list name is found, it defaults 
    /// to retrieving the list values from the general list repository.
    /// </summary>
    /// <param name="listName">The name of the list for which the configuration is requested.</param>
    /// <param name="includeFixed">
    /// A boolean flag indicating whether fixed list items should be included
    /// in the result for lists that support this option.
    /// </param>
    /// <param name="token">
    /// A <see cref="TokenInfoModel"/> object containing authentication and contextual information.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a <see cref="ListConfig"/>
    /// object with the name and the list of options.
    /// </returns>
    public async Task<ListConfig> GetListAsync(string listName, bool includeFixed, TokenInfoModel token, bool parentNodesOnly = false)
    {
        return listName.ToLower() switch
        {
            "alertcategory" => new ListConfig { Name = listName, Options = await _alertRepository.GetAlertCategoryListAsync() },
            "antibiotic" => new ListConfig { Name = listName, Options = (List<OptionsConfig>)await _antibioticRepository.GetAntibioticListAsync() },
            "antibioticlist" => new ListConfig { Name = listName, Options = (List<OptionsConfig>)await _antibioticRepository.GetAntibioticListWithGroupsAsync() },
            "archivestatelist" => new ListConfig { Name = listName, Options = (List<OptionsConfig>)await _listRepository.GetArchiveStateListAsync() },
            "astcannedcommentslist" => new ListConfig { Name = listName, Options = (List<OptionsConfig>)await _commentRepository.GetCannedASTCommentsAsync() },
            "astsusceptibilityoverridecannedcommentslist" => new ListConfig { Name = listName, Options = (List<OptionsConfig>)await _commentRepository.GetCannedASTSusceptibilityOverrideCommentsAsync() },
            "commonlist" => new ListConfig { Name = listName, Options = (List<OptionsConfig>)await _listRepository.GetCommonListAsync() },
            "completelaborglist" => new ListConfig { Name = "completelaborglist", Options = await new LabAndOrgListQuery(_laboratoryRepository, _organisationRepository).GetCompleteListAsync() },
            "culturecannedcommentslist" => new ListConfig { Name = listName, Options = (List<OptionsConfig>)await _commentRepository.GetCannedCultureCommentsAsync() },
            "culturefieldlist" => new ListConfig { Name = listName, Options = await _fieldListHandler.GetFieldsAsync(token, "addcultureform") },
            "culturetestconfiglist" => new ListConfig { Name = listName, Options = await _configListDataHandler.GetCultureTestListAsync() },
            "customtablelist" => new ListConfig { Name = listName, Options = (List<OptionsConfig>)await _listRepository.GetCustomListAsync() },
            "datasourcelist" => new ListConfig { Name = listName, Options = await _configListDataHandler.GetDataSectionListAsync() },
            "directtestconfiglist" => new ListConfig { Name = listName, Options = await _configListDataHandler.GetDirectTestListAsync() },
            "exportprofilefieldlist" => new ListConfig { Name = listName, Options = await _exportProfileConfigHandler.GetFieldsAsync(token) },
            "exportprofilelist" => new ListConfig { Name = listName, Options = (List<OptionsConfig>)await _exportProfileRepository.GetExportProfileOptionsForListAsync() },
            "familylist" => new ListConfig { Name = listName, Options = (List<OptionsConfig>)await _organismRepository.GetFamilyListAsync(null) },
            "genuslist" => new ListConfig { Name = listName, Options = (List<OptionsConfig>)await _organismRepository.GetGenusListAsync(null) },
            "gridfieldtypelist" => new ListConfig { Name = listName, Options = await _listFilterHandler.GetGridFieldListAsync() },
            "laboratorylist" => new ListConfig { Name = listName, Options = (List<OptionsConfig>)await _laboratoryRepository.GetLaboratoriesForListAsync(token) },
            "iqctestprofiles" => new ListConfig { Name = listName, Options = (List<OptionsConfig>)await _qualityRepository.GetIqcTestProfileNamesListAsync() },
            "laboratorylistunfiltered" => new ListConfig { Name = listName, Options = (List<OptionsConfig>)await _laboratoryRepository.GetLaboratoriesForListUnfilteredAsync() },
            "locationlist" => new ListConfig { Name = listName, Options = (List<OptionsConfig>)await _locationRepository.GetLocationsForListAsync() },
            "mappinglist" => new ListConfig { Name = listName, Options = await _configListDataHandler.GetMappingListAsync() },
            "orderlist" => new ListConfig { Name = listName, Options = (List<OptionsConfig>)await _organismRepository.GetOrderListAsync() },
            "organisationlist" => new ListConfig { Name = listName, Options = (List<OptionsConfig>)await _organisationRepository.GetOrganisationsForListAsync(token) },
            "organisationlistunfiltered" => new ListConfig { Name = listName, Options = (List<OptionsConfig>)await _organisationRepository.GetOrganisationsForListUnfilteredAsync() },
            "patientfieldlist" => new ListConfig { Name = listName, Options = await _fieldListHandler.GetFieldsAsync(token, "addpatientform") },
            "profilelist" => new ListConfig { Name = listName, Options = await _profileListHandler.GetListAsync(token) },
            "reportconfiglist" => new ListConfig { Name = listName, Options = await _configListDataHandler.GetReportListAsync() },
            "requestformlist" => new ListConfig { Name = listName, Options = await new RequestFormListQuery(_listRepository, _formConfigAdapter).GetListAsync() },
            "rolelist" => new ListConfig { Name = listName, Options = (List<OptionsConfig>)await _roleRepository.GetRolesForListAsync() },
            "shortstatelist" => new ListConfig { Name = listName, Options = (List<OptionsConfig>)await _listRepository.GetShortStateListAsync() },
            "specieslist" => new ListConfig { Name = listName, Options = (List<OptionsConfig>)await _organismRepository.GetSpeciesListAsync(null) },
            "specimenevent" => new ListConfig { Name = listName, Options = await _specimenEventListQuery.GetListAsync() },
            "specimenfieldlist" => new ListConfig { Name = listName, Options = await _fieldListHandler.GetFieldsAsync(token, "createspecimenreceivedform") },
            "specimenorganism" => new ListConfig { Name = listName, Options = (List<OptionsConfig>)await _organismRepository.GetOrganismDropdownAsync(token) },
            "specimenorganismcode" => new ListConfig { Name = listName, Options = (List<OptionsConfig>)await _organismRepository.GetOrganismCodeDropdownAsync(token) },
            "statelist" => new ListConfig { Name = listName, Options = (List<OptionsConfig>)await _listRepository.GetStateListAsync() },
            "storagelist" => new ListConfig { Name = listName, Options = (List<OptionsConfig>)await _storageRepository.GetStorageForListAsync(token) },
            "testconfiglist" => new ListConfig { Name = listName, Options = await _configListDataHandler.GetTestListAsync() },
            "testpatternnameslist" => new ListConfig { Name = listName, Options = (List<OptionsConfig>)await _testPatternRepository.GetTestPatternNamesListAsync() },
            "specification" => new ListConfig { Name = listName, Options = (List<OptionsConfig>)await _specificationRepository.GetSpecificationOptionsForListAsync() },
            "specimencannedcommentslist" => new ListConfig { Name = listName, Options = (List<OptionsConfig>)await _commentRepository.GetCannedSpecimenCommentsAsync() },
            "workflowlist" => new ListConfig { Name = listName, Options = (List<OptionsConfig>)await _configRepository.GetWorkflowListForDropdownAsync() },
            _ => new ListConfig { Name = listName, Options = (List<OptionsConfig>)await _listRepository.GetListValuesAsync(listName, includeFixed, parentNodesOnly) }
        };
    }
}
