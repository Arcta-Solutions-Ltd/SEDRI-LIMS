using arc.app.Alert.AlertDefinitions;
using arc.app.Coding;
using arc.app.Common;
using arc.app.Specimen;
using arc.app.Tests;
using arc.common;
using arc.common.Models;
using arc.common.Models.Alert;
using arc.common.Utils;
using arc.domain.Alert;
using arc.domain.Configuration.QueryFiltersConfig;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Alert;

public class AlertHandler : IAlertHandler
{
    private readonly ISpecimenAlertRepository _specimenAlertRepository;
    private readonly ICultureRepository _cultureRepository;
    private readonly IAlertRepository _alertRepository;
    private readonly IOrganismExistsAlerts _organismExistsAlerts;
    private readonly IDirectTestAndCultureTestAlerts _directTestAndCultureTestAlerts;
    private readonly IOrganismTestsASTTestAlerts _organismTestsASTTestAlerts;
    private readonly IOrganismTestAlerts _organismTestAlerts;
    private readonly IOrganismASTTestAlerts _organismASTTestAlerts;
    private readonly IAntibioticRepository _antibioticRepository;
    private readonly IOrganismRepository _organismRepository;
    private readonly IMapType<AlertDetails, AlertDetailsModel> _alertMapper;
    private readonly ICopyProperties _copyProperties;
    private readonly ITestRepository _testRepository;
    private readonly ISpecimenRepository _specimenRepository;
    private readonly ILogWriter _logWriter;
    private readonly IStandardSpecimenAlerts _standardSpecimenAlerts;

    public AlertHandler(ICultureRepository cultureRepository, ISpecimenAlertRepository specimenAlertRepository, IOrganismExistsAlerts organismExistsAlerts,
                        IDirectTestAndCultureTestAlerts directTestAndCultureTestAlerts, IOrganismTestsASTTestAlerts organismTestsASTTestAlerts,
                        IOrganismTestAlerts organismTestAlerts, IOrganismASTTestAlerts organismASTTestAlerts, IAlertRepository alertRepository,
                        IAntibioticRepository antibioticRepository, ILogWriter logWriter, IOrganismRepository organismRepository, ISpecimenRepository specimenRepository,
                        IMapType<AlertDetails, AlertDetailsModel> alertMapper, ICopyProperties copyProperties, ITestRepository testRepository, IStandardSpecimenAlerts standardSpecimenAlerts)
    {
        _cultureRepository = cultureRepository;
        _specimenAlertRepository = specimenAlertRepository;
        _organismExistsAlerts = organismExistsAlerts;
        _directTestAndCultureTestAlerts = directTestAndCultureTestAlerts;
        _organismTestsASTTestAlerts = organismTestsASTTestAlerts;
        _organismTestAlerts = organismTestAlerts;
        _organismASTTestAlerts = organismASTTestAlerts;
        _antibioticRepository = antibioticRepository;
        _alertRepository = alertRepository;
        _organismRepository = organismRepository;
        _alertMapper = alertMapper;
        _copyProperties = copyProperties;
        _testRepository = testRepository;
        _specimenRepository = specimenRepository;
        _logWriter = logWriter;
        _standardSpecimenAlerts = standardSpecimenAlerts;
    }

    public async Task RaiseAlertAsync(int specimenId)
    {
        var cultures = await _cultureRepository.GetCultureListBySpecimenIdAsync(new QueryFilterConfig().AddInteger("SpecimenId", specimenId));
        var directTestList = await _testRepository.GetTestsForSpecimenAsync(specimenId);

        var specimenDetails = await _specimenRepository.SpecimenByIdAsync(new QueryFilterConfig().AddInteger("id", specimenId));
        var antibioticOptions = (await _antibioticRepository.GetAntibioticListWithGroupsAsync()).ToList();

        _logWriter.LogInfo("Get Direct Test alerts", "AlertHandler", "RaiseAlert");
        var alertList = new List<SpecimenAlert>();
        var newAlerts = await _directTestAndCultureTestAlerts.GetAsync(cultures, specimenId);
        alertList.AddRange(newAlerts);

        // Organism exists alerts
        var existAlerts = await _organismExistsAlerts.GetAsync(cultures, specimenId);
        alertList.AddRange(existAlerts);

        // Organism + ast tests
        newAlerts = await _organismASTTestAlerts.GetAsync(cultures, specimenId, antibioticOptions);
        alertList.AddRange(newAlerts);

        // Organism + tests
        newAlerts = await _organismTestAlerts.GetAsync(cultures, specimenId);
        alertList.AddRange(newAlerts);

        // Organism + tests + ast tests
        newAlerts = await _organismTestsASTTestAlerts.GetAsync(cultures, specimenId, antibioticOptions);
        alertList.AddRange(newAlerts);

        //Standard specimen alerts
        newAlerts = await _standardSpecimenAlerts.GetAsync(cultures, directTestList.ToList(), specimenDetails);
        alertList.AddRange(newAlerts);

        var distinctAlerts = alertList.DistinctBy(alert => alert.AlertId).ToList();

        var alertCommand = new SpecimenAlertCommandModel { Alerts = distinctAlerts, SpecimenId = specimenId };
        _logWriter.LogInfo("Save specimen alert", "AlertHandler", "RaiseAlert");
        await _specimenAlertRepository.SaveSpecimenAlertAsync(alertCommand);
    }

    public async Task<string> GetAlertAsync(QueryFilterConfig queryFilters)
    {
        var alert = await _alertRepository.EditAlertQueryAsync(queryFilters);

        var craftedKeyValuePairs = new List<JsonKeyValuePairModel>
        {
            new() { Key = "orderid", value = alert.OrderId.ToString() },
            new() { Key = "familyid", value = alert.FamilyId.ToString() },
            new() { Key = "orggroupcodingid", value = alert.OrgGroupCodingId.ToString() }
        };

        var organismPairs = new List<JsonKeyValuePairModel>
        {
            new() { Key = "order", value = alert.Order },
            new() { Key = "family", value = alert.Family },
        };

        if (alert.OrganismId > 0)
        {
            var organismFilter = new QueryFilterConfig().AddInteger("id", alert.OrganismId);
            var organism = await _organismRepository.GetOrganismListEntrByIdAsync(organismFilter);
            organismPairs.Add(new JsonKeyValuePairModel { Key = "organism", value = organism.Description });
            organismPairs.Add(new JsonKeyValuePairModel { Key = "organismid", value = organism.Id.ToString() });
            var hierarchy = await _organismRepository.GetOrganismHierarchyAsync(alert.OrganismId);
            craftedKeyValuePairs.Add(new JsonKeyValuePairModel { Key = "genusid", value = hierarchy.GenusId.ToString() });
            craftedKeyValuePairs.Add(new JsonKeyValuePairModel { Key = "speciesid", value = hierarchy.SpeciesId.ToString() });
            craftedKeyValuePairs.Add(new JsonKeyValuePairModel { Key = "subspeciesid", value = hierarchy.SubSpeciesId.ToString() });
            craftedKeyValuePairs.Add(new JsonKeyValuePairModel { Key = "serotypeid", value = hierarchy.SerotypeId.ToString() });
        }

        if (!string.IsNullOrEmpty(alert.OrganismGroup))
        {
            organismPairs.Add(new JsonKeyValuePairModel { Key = "orggroup", value = alert.OrganismGroup });
        }
        organismPairs.AddRange(craftedKeyValuePairs);

        var craftedModels = new List<CraftedModel>
        {
            new() { Name = "editorganismscopepage", Contents = JsonConvert.SerializeObject(craftedKeyValuePairs) },
            new() { Name = "changeorganismselectorpage", Contents = JsonConvert.SerializeObject(organismPairs) }
        };

        var alertDetailsModel = _alertMapper.Map(alert);
        var fullAlertModel = new AlertDetailsWithCraftedModel();
        _copyProperties.CopyAll(alertDetailsModel, fullAlertModel);
        fullAlertModel.Crafted = craftedModels;
        fullAlertModel.ContainsOrganism = 0;

        return JsonConvert.SerializeObject(fullAlertModel);
    }
}
