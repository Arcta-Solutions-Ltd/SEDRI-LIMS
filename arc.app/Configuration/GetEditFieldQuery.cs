using arc.app.Common;
using arc.app.Config.Pages;
using arc.common.Models.Config;
using arc.common.Models.Lists;
using arc.domain.Configuration.PagesConfig;
using arc.domain.Configuration.QueryConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Configuration
{
    /// <summary>
    /// Retrieves field configuration data for editing a specific field within a form page.
    /// This query loads the field definition from the form configuration and converts it to a format suitable for the edit field form,
    /// including field properties, grid definitions, validation rules, and button visibility settings.
    /// </summary>
    internal class GetEditFieldQuery : ISingleConfig
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetEditFieldQuery"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider for dependency resolution.</param>
        internal GetEditFieldQuery(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Retrieves the field configuration for editing as a JSON string.
        /// </summary>
        /// <param name="queryFilters">Query filters containing the field identifier in the format "formId|pageName|fieldId".</param>
        /// <param name="queryData">Additional query configuration data (not used in this implementation).</param>
        /// <returns>A JSON string containing the field configuration including label, type, placeholder, validation rules, grid fields (with GridTitle for each column), button visibility settings, ParentList, ChildLists, and PageListFields for parent-link editing.</returns>
        /// <remarks>
        /// The method expects an "id" parameter in <paramref name="queryFilters"/> with a pipe-separated value containing:
        /// form identifier, page name, and field identifier. It loads the form and page configuration, finds the specified field,
        /// resolves list references, processes grid fields if present (including GridTitle for column headers), and returns a serialized configuration object.
        /// </remarks>
        public async Task<string> GetAsync(QueryFilterConfig queryFilters, QueryConfig queryData)
        {
            var id = queryFilters.Parameters.Where(p => p.Key.ToLower() == "id").First().Value;
            var idList = id.Split("|");

            var formConfigDefinition = _serviceProvider.GetService<IFormConfigDefinition>();
            var formToUpdate = await formConfigDefinition.LoadFormAsync(idList[0]);

            var pageAdapter = _serviceProvider.GetService<IPageConfigAdapter>();
            var page = await pageAdapter.GetPageAsync(idList[1]);

            var fieldTypeUtils = _serviceProvider.GetService<IFieldTypeList>();
            var listRepository = _serviceProvider.GetService<IListRepository>();
            var logWriter = _serviceProvider.GetService<ILogWriter>();

            foreach (var column in page.Columns)
            {
                foreach(var formgroup in column.FormGroups)
                {
                    foreach(var field in formgroup.Fields)
                    {
                        if (field.Id == idList[2])
                        {
                            if (!fieldTypeUtils.TryGetIdFromName(field.Type, out var fieldTypeId))
                            {
                                logWriter?.LogError(
                                    $"GetEditFieldQuery: unsupported field type '{field.Type}' for fieldId={field.Id} on page {idList[1]}",
                                    nameof(GetEditFieldQuery),
                                    nameof(GetAsync));
                                return JsonConvert.SerializeObject("{}");
                            }

                            var listId = new ListByIdQueryModel { Id = 0 };

                            if (! string.IsNullOrWhiteSpace(field.OptionsName))
                            {
                                var listParam = new QueryFilterConfig();
                                listParam.AddString("Value", field.OptionsName);
                                listId = await listRepository.GetListByValueAsync(listParam)
                                    ?? new ListByIdQueryModel { Id = 0 };
                            }

                            var gridValues = new List<GridDefinitionModel>();
                            if (field.GridFields != null)
                            {
                                foreach(var gridLine in field.GridFields)
                                {
                                    if (!fieldTypeUtils.TryGetIdFromName(gridLine.Type, out var gridType))
                                    {
                                        logWriter?.LogError(
                                            $"GetEditFieldQuery: unsupported grid field type '{gridLine.Type}' on fieldId={field.Id}",
                                            nameof(GetEditFieldQuery),
                                            nameof(GetAsync));
                                        continue;
                                    }

                                    var gridWidth = fieldTypeUtils.GetGridIdFromName(gridLine.Width);
                                    gridWidth = gridWidth == 0 ? 467 : gridWidth;
                                    var gridlistId = new ListByIdQueryModel { Id = 0 };

                                    if (!string.IsNullOrWhiteSpace(gridLine.OptionsName))
                                    {
                                        var listParam = new QueryFilterConfig();
                                        listParam.AddString("Value", gridLine.OptionsName);
                                        gridlistId = await listRepository.GetListByValueAsync(listParam)
                                            ?? new ListByIdQueryModel { Id = 0 };
                                    }

                                    var gridValue = new GridDefinitionModel
                                    {
                                        GridId = gridLine.Id,
                                        GridTitle = gridLine.GridTitle ?? string.Empty,
                                        GridType = gridType.ToString(),
                                        GridWidth = gridWidth.ToString(),
                                        GridOption = gridlistId.Id.ToString(),
                                        MultiSelect = gridLine.MultiSelect ? "Yes" : "No",
                                        Rules = (gridLine.Rules ?? new List<arc.domain.Configuration.PagesConfig.RuleConfig>())
                                            .Select(r => new arc.common.Models.Config.FormGroupRuleModel
                                            {
                                                Effect = r.Effect,
                                                Field = r.Field,
                                                Rule = r.Rule,
                                                Value = r.Value
                                            }).ToList()
                                    };
                                    gridValues.Add(gridValue);
                                }
                                logWriter?.LogInfo($"GetEditFieldQuery: loading fieldgrid for edit, fieldId={field.Id}, gridColumns={gridValues?.Count ?? 0}, RemoveGridAddButton={field.RemoveGridAddButton}", "GetEditFieldQuery", "GetAsync");
                            }

                            var errorRule = formToUpdate.SaveEventConfig.GetValidationRuleForField(field.Id);
                            var parentLinkUtils = new FieldParentLinkUtils(formConfigDefinition, listRepository, _serviceProvider.GetService<ILogWriter>());
                            var parentLinkData = await parentLinkUtils.BuildAsync(idList[0], idList[2], field.ParentList ?? "");
                            var otherDetailsLabel = "";
                            if (field.AllowOther)
                            {
                                var companion = formToUpdate.GetFieldsForForm()
                                    .FirstOrDefault(f => !string.IsNullOrWhiteSpace(f.OtherDetailsFor)
                                        && f.OtherDetailsFor.Equals(field.Id, StringComparison.OrdinalIgnoreCase));
                                if (companion != null
                                    && !string.Equals(companion.Label, OtherOptionConstants.DetailsLabelTag, StringComparison.OrdinalIgnoreCase))
                                {
                                    otherDetailsLabel = companion.Label ?? "";
                                }
                            }

                            object returnValue;
                            if (field.Type == "upload")
                            {
                                logWriter?.LogInfo($"GetEditFieldQuery: loading upload field for edit, fieldId={field.Id}, ContentTypes count={field.ContentTypes?.Count ?? 0}", "GetEditFieldQuery", "GetAsync");
                                var contentTypeIds = new List<string>();
                                if (field.ContentTypes != null && field.ContentTypes.Count > 0)
                                {
                                    foreach (var ct in field.ContentTypes)
                                    {
                                        if (!string.IsNullOrWhiteSpace(ct))
                                        {
                                            var ctParam = new QueryFilterConfig();
                                            ctParam.AddString("value", ct);
                                            ctParam.AddString("name", "ContentTypeList");
                                            try
                                            {
                                                var ctItem = await listRepository.GetListItemByValueAsync(ctParam);
                                                if (ctItem != null)
                                                {
                                                    contentTypeIds.Add(ctItem.Id.ToString());
                                                }
                                            }
                                            catch
                                            {
                                                // Content type not found in list; skip
                                            }
                                        }
                                    }
                                }
                                returnValue = new {
                                    FieldId = field.Id,
                                    Label = field.Label,
                                    TypeId = fieldTypeId,
                                    Required = field.Required ? "Yes" : "No",
                                    List = listId.Id,
                                    ContentTypeIds = string.Join(",", contentTypeIds),
                                    RequiredErrorMessage = errorRule == null ? "" : errorRule.Message,
                                    Placeholder = field.Placeholder ?? "",
                                    MultiSelect = field.MultiSelect ? "Yes" : "No",
                                    ReadOnly = field.ReadOnly ? "Yes" : "No",
                                    IncludeAddButton = field.RemoveGridAddButton ? "No" : "Yes",
                                    IncludeDeleteButton = field.RemoveGridDeleteButton ? "No" : "Yes"
                                };
                            }
                            else
                            {
                                returnValue = new {
                                    FieldId = field.Id,
                                    Label = field.Label,
                                    TypeId = fieldTypeId,
                                    Required = field.Required ? "Yes" : "No",
                                    List = listId.Id,
                                    Min = field.Min,
                                    Max = field.Max,
                                    dpts = field.MaxDPs,
                                    DefaultToNow = field.DefaultToNow ? "Yes" : "No",
                                    ToggleDefault = field.DefaultValue == "Yes" ? 460 : 461,
                                    FieldGrid = gridValues,
                                    RequiredErrorMessage = errorRule == null ? "" : errorRule.Message,
                                    Placeholder = field.Placeholder ?? "",
                                    MultiSelect = field.MultiSelect ? "Yes" : "No",
                                    ReadOnly = field.ReadOnly ? "Yes" : "No",
                                    IncludeAddButton = field.RemoveGridAddButton ? "No" : "Yes",
                                    IncludeDeleteButton = field.RemoveGridDeleteButton ? "No" : "Yes",
                                    ParentList = field.ParentList ?? "",
                                    AllowOther = field.AllowOther ? "Yes" : "No",
                                    OtherDetailsLabel = otherDetailsLabel,
                                    ChildLists = parentLinkData.ChildLists,
                                    PageListFields = parentLinkData.PageListFields,
                                };
                            }

                            logWriter?.LogInfo(
                                $"GetEditFieldQuery: fieldId={field.Id}, ParentList='{field.ParentList ?? ""}', pageListFields={parentLinkData.PageListFields?.Count ?? 0}",
                                nameof(GetEditFieldQuery),
                                nameof(GetAsync));

                            return JsonConvert.SerializeObject(returnValue);
                        }
                    }
                }
            }


            return JsonConvert.SerializeObject("{}");
        }
    }
}
