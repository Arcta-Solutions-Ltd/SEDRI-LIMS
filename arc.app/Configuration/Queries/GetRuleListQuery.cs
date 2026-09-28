using arc.app.Common;
using arc.app.Config.Forms;
using arc.app.Coding;
using arc.app.SystemConfig;
using arc.common.Models.Laboratory;
using arc.common.Models.SystemConfig;
using arc.common.Utils;
using arc.domain.Configuration.QueryConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Threading.Tasks;

namespace arc.app.Configuration.Queries;
/// <summary>
/// Query for retrieving a list of rules associated with laboratory configurations.
/// </summary>
internal class GetRuleListQuery : ISingleConfig
{
    private readonly IServiceProvider _serviceProvider;
    private readonly string _configName;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetRuleListQuery"/> class.
    /// </summary>
    /// <param name="configName">The name of the configuration to retrieve rules for.</param>
    /// <param name="serviceProvider">The service provider used to resolve dependencies.</param>
    internal GetRuleListQuery(string configName, IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _configName = configName;
    }

    /// <summary>
    /// Executes the query to retrieve rules associated with the given configuration.
    /// </summary>
    /// <param name="queryFilters">Filters to apply to the query.</param>
    /// <param name="queryData">Additional query data and parameters.</param>
    /// <returns>
    /// A JSON string representation of the list of rules, including details like group descriptions
    /// and associated forms.
    /// </returns>
    public async Task<string> GetAsync(QueryFilterConfig queryFilters, QueryConfig queryData)
    {
        queryFilters.AddString("configname", _configName);
        queryFilters.Parameters[0].Key = "id";

        var laboratoryConfigRepository = _serviceProvider.GetService<ILaboratoryConfigRepository>();
        var configList = await laboratoryConfigRepository.GetLaboratoryConfigListAsync(queryFilters);

        var listRepository = _serviceProvider.GetService<IListRepository>();
        var returnList = new List<object>();

        var formAdapter = _serviceProvider.GetService<IFormConfigAdapter>();
        if (configList != null)
        {
            foreach (var def in configList)
            {
                object groupDescription;
                if (_configName == "organismscopeculturetestoption")
                {
                    var organismDefValues = ArcJson.Deserialize<OrganismScopeCultureTestOptionModel>(def.Contents);
                    var organismRepository = _serviceProvider.GetService<IOrganismRepository>();
                    groupDescription = await organismRepository.GetOrganismScopeDescriptionAsync(
                        organismDefValues.OrderId, organismDefValues.FamilyId, organismDefValues.GenusId,
                        organismDefValues.SpeciesId, organismDefValues.SubspeciesId, organismDefValues.SerotypeId,
                        organismDefValues.OrgGroupCodingId, organismDefValues.OrganismId);
                    var organismDescriptionList = "";
                    foreach (var form in (organismDefValues.AssociatedListId ?? "").Split(",", StringSplitOptions.RemoveEmptyEntries))
                    {
                        var newForm = await formAdapter.GetFormAsync(form.Trim());
                        if (newForm != null) { organismDescriptionList += organismDescriptionList == "" ? newForm.Title : ", " + newForm.Title; }
                    }
                    var newOrganismItem = new { GroupDescription = groupDescription, AssociatedList = organismDescriptionList, def.Id };
                    returnList.Add(newOrganismItem);
                    continue;
                }

                if (_configName == "formspecimentypeoption")
                {
                    var formDefValues = ArcJson.Deserialize<LaboratoryFormGroupingModel>(def.Contents);
                    var groupForm = await formAdapter.GetFormAsync(formDefValues.GroupId);
                    var specimenTypeList = "";
                    foreach (var specimenType in (formDefValues.AssociatedListId ?? "").Split(",", StringSplitOptions.RemoveEmptyEntries))
                    {
                        var newDescription = await listRepository.GetValueFromIdAsync(int.Parse(specimenType.Trim()));
                        specimenTypeList += specimenTypeList == "" ? newDescription : ", " + newDescription;
                    }
                    var newFormItem = new { GroupDescription = groupForm?.Title ?? formDefValues.GroupId, AssociatedList = specimenTypeList, def.Id };
                    returnList.Add(newFormItem);
                    continue;
                }

                var defValues = ArcJson.Deserialize<LaboratoryListGroupingModel>(def.Contents);
                groupDescription = "";

                if (_configName == "specimentypeworkflow")
                {
                    var configRepository = _serviceProvider.GetService<IConfigRepository>();
                    var queryFilter = new QueryFilterConfig().AddInteger("id",defValues.GroupId);
                    var configRecord = await configRepository.SingleConfigByIdAsync(queryFilter);
                    var record = ArcJson.Deserialize<WorkflowListModel>(configRecord.Contents);
                    groupDescription = record.Description;
                } else
                {
                    groupDescription = await listRepository.GetValueFromIdAsync(defValues.GroupId);
                }

                var descriptionList = "";
                foreach (var form in defValues.AssociatedListId.Split(","))
                {
                    if (_configName == "culturetypecategory" || _configName == "specimentypeculturetypedefault" || _configName == "specimentypeculturetypeoption" || _configName == "specimentypeworkflow")
                    {
                        var newDescription = await listRepository.GetValueFromIdAsync(int.Parse(form));
                        descriptionList += descriptionList == "" ? newDescription : ", " + newDescription;
                    } else
                    {
                        var newForm = await formAdapter.GetFormAsync(form);
                        if (newForm != null) { descriptionList += descriptionList == "" ? newForm.Title : ", " + newForm.Title; }
                    }
                }
                var newItem = new { GroupDescription = groupDescription, AssociatedList = descriptionList, def.Id };
                returnList.Add(newItem);
            }
        }

        return JsonConvert.SerializeObject(returnList);
    }
}
