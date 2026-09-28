using arc.app.Alert.AlertDefinitions;
using arc.app.Common;
using arc.app.Coding;
using arc.app.Specimen;
using arc.app.Tests;
using arc.common;
using arc.common.Models;
using arc.common.Models.Alert;
using arc.common.Models.Coding;
using arc.common.Utils;
using arc.domain.Alert;
using arc.domain.Coding;
using arc.domain.Configuration.QueryFiltersConfig;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.ExpertRule;

public class ExpertRuleHandler : IExpertRuleHandler
{
    private readonly ICultureRepository _cultureRepository;
    private readonly IExpertRuleRepository _expertRuleRepository;
    private readonly IAntibioticRepository _antibioticRepository;
    private readonly IOrganismRepository _organismRepository;
    private readonly IMapType<arc.domain.Coding.ExpertRule, ExpertRuleDetailsModel> _expertRuleMapper;
    private readonly ICopyProperties _copyProperties;
    private readonly ITestRepository _testRepository;
    private readonly ISpecimenRepository _specimenRepository;
    private readonly ILogWriter _logWriter;

    public ExpertRuleHandler(ICultureRepository cultureRepository, IExpertRuleRepository expertRuleRepository, IAntibioticRepository antibioticRepository, ILogWriter logWriter, IOrganismRepository organismRepository, ISpecimenRepository specimenRepository,
                        IMapType<arc.domain.Coding.ExpertRule, ExpertRuleDetailsModel> expertRuleMapper, ICopyProperties copyProperties, ITestRepository testRepository, IStandardSpecimenAlerts standardSpecimenAlerts)
    {
        _cultureRepository = cultureRepository;
        _antibioticRepository = antibioticRepository;
        _expertRuleMapper = expertRuleMapper;
        _organismRepository = organismRepository;
        _copyProperties = copyProperties;
        _expertRuleRepository = expertRuleRepository;
        _testRepository = testRepository;
        _specimenRepository = specimenRepository;
        _logWriter = logWriter;
    }

    public async Task<string> GetExpertRuleAsync(QueryFilterConfig queryFilters)
    {
        var expertRule = await _expertRuleRepository.EditExpertRuleQueryAsync(queryFilters);

        var craftedKeyValuePairs = new List<JsonKeyValuePairModel>
        {
            new() { Key = "orderid", value = expertRule.OrderId.ToString() },
            new() { Key = "familyid", value = expertRule.FamilyId.ToString() },
            new() { Key = "orggroupcodingid", value = expertRule.OrgGroupCodingId.ToString() }
        };

        var organismPairs = new List<JsonKeyValuePairModel>
        {
            new() { Key = "order", value = expertRule.Order },
            new() { Key = "family", value = expertRule.Family },
        };

        if (expertRule.OrganismId > 0)
        {
            var organismFilter = new QueryFilterConfig().AddInteger("id", expertRule.OrganismId);
            var organism = await _organismRepository.GetOrganismListEntrByIdAsync(organismFilter);
            organismPairs.Add(new JsonKeyValuePairModel { Key = "organism", value = organism.Description });
            organismPairs.Add(new JsonKeyValuePairModel { Key = "organismid", value = organism.Id.ToString() });
            var hierarchy = await _organismRepository.GetOrganismHierarchyAsync(expertRule.OrganismId);
            craftedKeyValuePairs.Add(new JsonKeyValuePairModel { Key = "genusid", value = hierarchy.GenusId.ToString() });
            craftedKeyValuePairs.Add(new JsonKeyValuePairModel { Key = "speciesid", value = hierarchy.SpeciesId.ToString() });
            craftedKeyValuePairs.Add(new JsonKeyValuePairModel { Key = "subspeciesid", value = hierarchy.SubSpeciesId.ToString() });
            craftedKeyValuePairs.Add(new JsonKeyValuePairModel { Key = "serotypeid", value = hierarchy.SerotypeId.ToString() });
        }

        if (!string.IsNullOrEmpty(expertRule.OrganismGroup))
        {
            organismPairs.Add(new JsonKeyValuePairModel { Key = "orggroup", value = expertRule.OrganismGroup });
        }
        organismPairs.AddRange(craftedKeyValuePairs);

        var craftedModels = new List<CraftedModel>
        {
            new() { Name = "editorganismscopepage", Contents = JsonConvert.SerializeObject(craftedKeyValuePairs) },
            new() { Name = "changeorganismselectorpage", Contents = JsonConvert.SerializeObject(organismPairs) }
        };

        var expertRuleDetailsModel = _expertRuleMapper.Map(expertRule);
        var fullExpertRuleModel = new ExpertRuleDetailsWithCraftedModel();
        _copyProperties.CopyAll(expertRuleDetailsModel, fullExpertRuleModel);
        fullExpertRuleModel.Crafted = craftedModels;
        fullExpertRuleModel.ContainsOrganism = 0;

        return JsonConvert.SerializeObject(fullExpertRuleModel);
    }
}
