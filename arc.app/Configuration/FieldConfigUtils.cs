using arc.app.Common;
using arc.app.Config.Reports.SectionFormats;
using arc.app.Reports;
using arc.app.SystemConfig;
using arc.common.ExtensionMethods;
using arc.common.Models.Config;
using arc.domain.Configuration.FormStructureConfig;
using arc.domain.Configuration.PagesConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Configuration;

/// <summary>
/// Utilities for creating, updating, and removing field configuration entries across
/// form definitions, including record view/query adjustments and report section sync.
/// </summary>
public class FieldConfigUtils : IFieldConfigUtils
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of <see cref="FieldConfigUtils"/> with required dependencies.
    /// </summary>
    /// <param name="serviceProvider">Service provider used to resolve configuration services.</param>
    public FieldConfigUtils(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Adds a field to a form configuration and propagates related updates (report sections, record view query and mapper).
    /// For fieldgrid types, maps IncludeAdd/DeleteButton inputs into RemoveGridAdd/DeleteButton flags on the stored <see cref="FieldConfig"/>.
    /// </summary>
    /// <param name="newFieldName">Normalized field name (identifier) to persist.</param>
    /// <param name="newFieldInfo">Client-provided edit model describing the field to add.</param>
    /// <param name="formToUpdate">Form configuration to mutate.</param>
    /// <param name="dataToSave">Original JSON payload for downstream consumers.</param>
    /// <param name="pageToAddFieldTo">Target page identifier where the field will be placed.</param>
    /// <param name="orderIndex">Optional insertion index within the page; -1 appends to the end.</param>
    /// <returns>The updated form configuration.</returns>
    public async Task<FullFormConfig> AddFieldToSystemAsync(string newFieldName, EditFieldModel newFieldInfo, FullFormConfig formToUpdate, string dataToSave, string pageToAddFieldTo, int orderIndex = -1)
    {

        var fieldTypeUtils = _serviceProvider.GetService<IFieldTypeList>();
        var listRepository = _serviceProvider.GetService<IListRepository>();
        var formConfigRepository = _serviceProvider.GetService<IFormConfigRepository>();
        var sectionFormatAdapter = _serviceProvider.GetService<IReportTranslator>();

        if ((newFieldInfo.TypeId == 453 || newFieldInfo.TypeId == 454) && newFieldName.ToLower().EndsWith("id"))
        {
            newFieldName = newFieldName.ToLower().Substring(0, newFieldName.ToLower().LastIndexOf("id"));
        }

        var newField = await AddFieldToFormAsync(dataToSave, formToUpdate, pageToAddFieldTo, formConfigRepository, fieldTypeUtils, listRepository, newFieldName, orderIndex);

        await ReconcileOtherDetailsCompanionAsync(newField, newFieldName, newFieldInfo, formToUpdate, pageToAddFieldTo, sectionFormatAdapter);

        await PropagateNewFieldAsync(newField, newFieldName, formToUpdate, sectionFormatAdapter);

        return formToUpdate;
    }

    /// <summary>
    /// Adds an existing field to a form by reference. The supplied <paramref name="fieldToReference"/>
    /// is cloned onto the target page keeping its <c>Id</c> verbatim, so the referenced field shares
    /// the same physical column or MoreData key as the source. Unlike
    /// <see cref="AddFieldToSystemAsync"/> this method does not reserve a new field name, because the
    /// identifier already exists in <c>namelist</c>.
    /// </summary>
    /// <param name="fieldToReference">Field configuration cloned from the source form.</param>
    /// <param name="formToUpdate">Target form configuration to mutate.</param>
    /// <param name="pageToAddFieldTo">Target page name id.</param>
    /// <param name="requiredErrorMessage">
    /// Required validation message carried over from the source form, so a required field keeps its
    /// message on the target form. Ignored when the field is not required.
    /// </param>
    /// <param name="formGroupKey">Target form group key, or null to append to the last form group.</param>
    /// <param name="columnKey">Target column key, used with <paramref name="formGroupKey"/>.</param>
    /// <param name="orderIndex">Optional insertion index within the form group; -1 appends.</param>
    /// <returns>The updated form configuration.</returns>
    public async Task<FullFormConfig> AddExistingFieldToSystemAsync(
        FieldConfig fieldToReference,
        FullFormConfig formToUpdate,
        string pageToAddFieldTo,
        string requiredErrorMessage = null,
        string formGroupKey = null,
        string columnKey = null,
        int orderIndex = -1)
    {
        var sectionFormatAdapter = _serviceProvider.GetService<IReportTranslator>();
        var logWriter = _serviceProvider.GetService<ILogWriter>();

        var newField = CloneField(fieldToReference);

        // A field may already be on another page of this form, when a reference copy is being placed
        // alongside the entry field. The report sections and record view key on the field id, so
        // propagating a second time would duplicate the row rather than add anything.
        var alreadyElsewhereOnForm = formToUpdate.GetFieldsForForm()
            .Any(f => f?.Id != null && f.Id.Equals(newField.Id, StringComparison.OrdinalIgnoreCase));

        formToUpdate.AddField(newField, pageToAddFieldTo, requiredErrorMessage, orderIndex);

        var relocated = RelocateFieldToFormGroup(formToUpdate, newField.Id, pageToAddFieldTo, columnKey, formGroupKey);

        logWriter?.LogInfo(
            $"AddExistingFieldToSystemAsync: field={newField.Id} added to form={formToUpdate.Name} page={pageToAddFieldTo} formGroup={formGroupKey ?? "last"} relocated={relocated} alreadyElsewhereOnForm={alreadyElsewhereOnForm}; name reservation skipped (existing id)",
            nameof(FieldConfigUtils),
            nameof(AddExistingFieldToSystemAsync));

        if (!alreadyElsewhereOnForm)
        {
            var recordViewFieldName = GetRecordViewFieldName(newField);
            await PropagateNewFieldAsync(newField, recordViewFieldName, formToUpdate, sectionFormatAdapter);
        }

        return formToUpdate;
    }

    /// <summary>
    /// Adds a newly placed field to the form's report sections and record view query and mapper.
    /// Shared by the add field and add existing field paths so both keep the same dependent
    /// configuration in step.
    /// </summary>
    /// <param name="newField">The field that was added to the form.</param>
    /// <param name="recordViewFieldName">
    /// Name used in the record view query. For list fields this is the field id without its trailing
    /// <c>Id</c> suffix, because the query joins append that suffix themselves.
    /// </param>
    /// <param name="formToUpdate">Form configuration to mutate.</param>
    /// <param name="sectionFormatAdapter">Adapter used to resolve report section column counts.</param>
    private async Task PropagateNewFieldAsync(
        FieldConfig newField,
        string recordViewFieldName,
        FullFormConfig formToUpdate,
        IReportTranslator sectionFormatAdapter)
    {
        var logWriter = _serviceProvider.GetService<ILogWriter>();
        var sectionsUpdated = 0;

        var sectionsToUpdate = formToUpdate.ReportSectionConfigList.ToList();
        if (sectionsToUpdate.Count == 0
            && formToUpdate.NewSectionConfig != null
            && formToUpdate.NewSectionConfig.DataSection.IsSameConfigName(formToUpdate.DataSection))
        {
            sectionsToUpdate.Add(formToUpdate.NewSectionConfig);
            logWriter?.LogWarning(
                $"PropagateNewFieldAsync: ReportSectionConfigList empty for form={formToUpdate.Name} dataSection={formToUpdate.DataSection}; propagating field={newField.Id} to NewSectionConfig {formToUpdate.NewSectionConfig.Name}",
                nameof(FieldConfigUtils),
                nameof(PropagateNewFieldAsync));
        }

        foreach (var section in sectionsToUpdate)
        {
            // The section's current grid capacity is read before the field is added, so a section already
            // using a format that places several grids keeps it instead of dropping back to the single
            // grid default.
            var currentFormat = section.Format == null ? null : await sectionFormatAdapter.GetSectionDefinitionAsync(section.Format);
            var gridPositionsInCurrentFormat = currentFormat?.Grids?.Count ?? 0;

            section.AddField(newField.Label, newField.Id, newField.Type, gridPositionsInCurrentFormat: gridPositionsInCurrentFormat);
            section.Format = section.Format == null ? "DoubleColumnFour" : section.Format;
            var reportSection = await sectionFormatAdapter.GetSectionDefinitionAsync(section.Format);
            section.ResetColumns(reportSection.Columns.Count());

            var boundGrids = section.Grids?.Count ?? 0;
            var formatGridPositions = reportSection.Grids?.Count ?? 0;
            if (boundGrids > formatGridPositions)
            {
                logWriter?.LogInfo(
                    $"PropagateNewFieldAsync: section={section.Name} now binds {boundGrids} grid(s) but format={section.Format} defines {formatGridPositions} grid position(s); choose a format with more positions in the report designer",
                    nameof(FieldConfigUtils),
                    nameof(PropagateNewFieldAsync));
            }

            sectionsUpdated++;
        }

        var gridBindingCount = sectionsToUpdate.Sum(s => s.Grids?.Count ?? 0);
        logWriter?.LogInfo(
            $"PropagateNewFieldAsync: report sections updated={sectionsUpdated} field={newField.Id} type={newField.Type} gridBindings={gridBindingCount}",
            nameof(FieldConfigUtils),
            nameof(PropagateNewFieldAsync));

        if (formToUpdate.RecordViewQueryConfig == null)
        {
            logWriter?.LogInfo(
                $"PropagateNewFieldAsync: record view query absent, mapper unchanged for field={newField.Id}",
                nameof(FieldConfigUtils),
                nameof(PropagateNewFieldAsync));
            return;
        }

        if (newField.Type == "combobox" || newField.Type == "dropdown" || newField.Type == "hierarchicalpicker")
        {
            formToUpdate.RecordViewQueryConfig.AddListItem(recordViewFieldName);
        }
        else
        {
            var mapType = newField.Type == "date" ? "date" : newField.Type == "number" ? "numeric" : "string";
            formToUpdate.RecordViewQueryConfig.AddField(recordViewFieldName, mapType);
        }

        var targetStructure = JsonConvert.DeserializeObject<RecordViewTargetModel>(formToUpdate.RecordViewQueryConfig.ResultMapperConfig.GetTarget().ToString());
        var ruleNumber = formToUpdate.RecordViewQueryConfig.ResultMapperConfig.AddRule(recordViewFieldName);

        var newTargetField = new RecordViewTargetFieldModel
        {
            Id = recordViewFieldName,
            Label = newField.Label,
            Value = "<:" + ruleNumber + ":>"
        };
        targetStructure.Add(newTargetField, 0, 2);
        var targetAsString = JsonConvert.SerializeObject(targetStructure);
        formToUpdate.RecordViewQueryConfig.ResultMapperConfig.SetTarget(targetAsString);

        logWriter?.LogInfo(
            $"PropagateNewFieldAsync: record view mapper rule {ruleNumber} added for field={recordViewFieldName}",
            nameof(FieldConfigUtils),
            nameof(PropagateNewFieldAsync));
    }

    /// <summary>
    /// Returns the name a field is known by inside the record view query. List fields are stored in
    /// <c>ListItems</c> without the trailing <c>Id</c> suffix, because the generated joins append it.
    /// </summary>
    /// <param name="field">The field being added.</param>
    /// <returns>The record view query field name.</returns>
    private static string GetRecordViewFieldName(FieldConfig field)
    {
        var isListField = field.Type == "combobox" || field.Type == "dropdown" || field.Type == "hierarchicalpicker";
        if (!isListField || string.IsNullOrEmpty(field.Id) || !field.Id.EndsWith("Id", StringComparison.OrdinalIgnoreCase))
        {
            return field.Id;
        }

        return field.Id[..^2];
    }

    /// <summary>
    /// Produces an independent copy of a field configuration so that editing the referenced field on
    /// one form cannot mutate the source form's stored definition. Grid columns are copied with it.
    /// </summary>
    /// <param name="field">Field configuration to clone.</param>
    /// <returns>A deep copy of the field.</returns>
    private static FieldConfig CloneField(FieldConfig field)
    {
        var serialized = JsonConvert.SerializeObject(field);
        return JsonConvert.DeserializeObject<FieldConfig>(serialized);
    }

    /// <summary>
    /// Moves a field that was appended by <see cref="FullFormConfig.AddField"/> into the specific
    /// form group the user chose. Does nothing when no form group was requested, when the field is
    /// already there, or when the target form group cannot be found.
    /// </summary>
    /// <param name="formToUpdate">Form configuration to mutate.</param>
    /// <param name="fieldId">Id of the field to relocate.</param>
    /// <param name="pageName">Page holding the target form group.</param>
    /// <param name="columnKey">Column key of the target form group.</param>
    /// <param name="formGroupKey">Key of the target form group.</param>
    /// <returns>True when the field was moved.</returns>
    private static bool RelocateFieldToFormGroup(
        FullFormConfig formToUpdate,
        string fieldId,
        string pageName,
        string columnKey,
        string formGroupKey)
    {
        if (string.IsNullOrWhiteSpace(formGroupKey))
        {
            return false;
        }

        var page = formToUpdate.PagesConfig?
            .FirstOrDefault(p => p?.Name?.Equals(pageName, StringComparison.OrdinalIgnoreCase) == true);
        if (page?.Columns == null)
        {
            return false;
        }

        FormGroupConfig targetFormGroup = null;
        foreach (var column in page.Columns)
        {
            if (!string.IsNullOrWhiteSpace(columnKey)
                && column?.Key?.Equals(columnKey, StringComparison.OrdinalIgnoreCase) != true)
            {
                continue;
            }

            targetFormGroup = column?.FormGroups?
                .FirstOrDefault(fg => fg?.Key?.Equals(formGroupKey, StringComparison.OrdinalIgnoreCase) == true);
            if (targetFormGroup != null)
            {
                break;
            }
        }

        if (targetFormGroup == null)
        {
            return false;
        }

        if (targetFormGroup.Fields?.Any(f => f?.Id?.Equals(fieldId, StringComparison.OrdinalIgnoreCase) == true) == true)
        {
            return false;
        }

        FieldConfig fieldToMove = null;
        foreach (var column in page.Columns)
        {
            foreach (var formGroup in column?.FormGroups ?? [])
            {
                var match = formGroup?.Fields?
                    .FirstOrDefault(f => f?.Id?.Equals(fieldId, StringComparison.OrdinalIgnoreCase) == true);
                if (match == null)
                {
                    continue;
                }

                fieldToMove = match;
                formGroup.Fields = formGroup.Fields
                    .Where(f => f?.Id?.Equals(fieldId, StringComparison.OrdinalIgnoreCase) != true)
                    .ToList();
                break;
            }

            if (fieldToMove != null)
            {
                break;
            }
        }

        if (fieldToMove == null)
        {
            return false;
        }

        targetFormGroup.Fields ??= [];
        targetFormGroup.Fields.Add(fieldToMove);
        return true;
    }

    /// <summary>
    /// Deletes a field from a form configuration and updates dependent structures (report sections, record view query/mapper).
    /// </summary>
    /// <param name="fieldToDelete">Identifier of the field to remove.</param>
    /// <param name="formToUpdate">Form configuration to mutate.</param>
    /// <returns>The updated form configuration.</returns>
    public async Task<FullFormConfig> DeleteFieldFromSystemAsync(string fieldToDelete, FullFormConfig formToUpdate)
    {
        var sectionFormatAdapter = _serviceProvider.GetService<ISectionFormatAdapter>();

        var resolvedFieldId = ResolveFieldIdOnForm(formToUpdate, fieldToDelete);
        if (!string.IsNullOrWhiteSpace(resolvedFieldId))
        {
            var companionIds = formToUpdate.GetFieldsForForm()
                .Where(f => !string.IsNullOrWhiteSpace(f.OtherDetailsFor)
                    && f.OtherDetailsFor.Equals(resolvedFieldId, StringComparison.OrdinalIgnoreCase))
                .Select(f => f.Id)
                .ToList();

            foreach (var companionId in companionIds)
            {
                formToUpdate.DeleteField(companionId);
                if (formToUpdate.RecordViewQueryConfig != null)
                {
                    formToUpdate.RecordViewQueryConfig.DeleteField(companionId);
                    var targetStructure = JsonConvert.DeserializeObject<RecordViewTargetModel>(
                        formToUpdate.RecordViewQueryConfig.ResultMapperConfig.GetTarget().ToString());
                    targetStructure.Delete(companionId);
                    formToUpdate.RecordViewQueryConfig.ResultMapperConfig.SetTarget(JsonConvert.SerializeObject(targetStructure));
                }
            }
        }

        formToUpdate.DeleteField(fieldToDelete);

        if (formToUpdate.RecordViewQueryConfig != null)
        {
            formToUpdate.RecordViewQueryConfig.DeleteField(fieldToDelete);
            foreach (var section in formToUpdate.ReportSectionConfigList)
            {
                var reportSection = await sectionFormatAdapter.GetFormatAsync(section.Format);
                section.ResetColumns(reportSection.Columns.Count());
            }
            var targetStructure = JsonConvert.DeserializeObject<RecordViewTargetModel>(formToUpdate.RecordViewQueryConfig.ResultMapperConfig.GetTarget().ToString());
            targetStructure.Delete(fieldToDelete);

            var targetAsString = JsonConvert.SerializeObject(targetStructure);
            formToUpdate.RecordViewQueryConfig.ResultMapperConfig.SetTarget(targetAsString);
        }

        return formToUpdate;
    }

    /// <summary>
    /// Creates a <see cref="FieldConfig"/> from the provided payload and appends it to the specified page
    /// within the form. Handles type-specific initialization (lists, numeric bounds, fieldgrid setup),
    /// grid button visibility mapping for fieldgrid, and GridTitle mapping from <see cref="GridDefinitionModel"/> to <see cref="FieldGridConfig"/>.
    /// New fields are added to the last form group on the page (respecting user-defined form groups;
    /// form structure is managed via Form Definition add/edit/delete form group).
    /// </summary>
    private async Task<FieldConfig> AddFieldToFormAsync(string dataToSave, FullFormConfig formToUpdate, string pageToAddFieldTo, IFormConfigRepository formConfigRepository, IFieldTypeList fieldTypeUtils, IListRepository listRepository, string newFieldName, int orderIndex = -1)
    {
        var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore };
        var newFieldInfo = JsonConvert.DeserializeObject<EditFieldModel>(dataToSave, settings);
        newFieldInfo.FieldId = newFieldName;

        string toggleDefault = null;
        if (newFieldInfo.ToggleDefault != 0)
        {
            var param = new QueryFilterConfig();
            param.AddString("id", newFieldInfo.ToggleDefault.ToString());
            var toggleRecord = await listRepository.GetListItemByIdAsync(param);
            toggleDefault = toggleRecord.Value;
        }

        var newField = new FieldConfig
        {
            Id = newFieldInfo.TypeId == 453 || newFieldInfo.TypeId == 454 ? newFieldName + "Id" : newFieldName,
            Label = newFieldInfo.Label,
            Type = fieldTypeUtils.GetNameFromId(newFieldInfo.TypeId),
            Required = newFieldInfo.Required == "Yes" ? true : false,
            DefaultToNow = (newFieldInfo.TypeId == 455 || newFieldInfo.TypeId == 456) && (newFieldInfo.DefaultToNow == "Yes"),
            DefaultValue = toggleDefault,
            ReadOnly = newFieldInfo.ReadOnly == "Yes",
        };

        if (!string.IsNullOrWhiteSpace(newFieldInfo.Placeholder))
        {
            newField.Placeholder = newFieldInfo.Placeholder.Trim();
        }

        var logWriterPlaceholder = _serviceProvider.GetService<ILogWriter>();
        logWriterPlaceholder?.LogInfo(
            $"AddFieldToFormAsync: field={newField.Id}, Placeholder set={!string.IsNullOrWhiteSpace(newField.Placeholder)}",
            nameof(FieldConfigUtils),
            nameof(AddFieldToFormAsync));

        if (newFieldInfo.TypeId == 149)
        {
            newField.MultiSelect = newFieldInfo.MultiSelect != null && newFieldInfo.MultiSelect == "Yes";
            var contentTypes = new List<string>();
            if (!string.IsNullOrWhiteSpace(newFieldInfo.ContentTypeIds))
            {
                var ids = newFieldInfo.ContentTypeIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var idStr in ids)
                {
                    if (int.TryParse(idStr.Trim(), out var id) && id > 0)
                    {
                        var param = new QueryFilterConfig();
                        param.AddString("id", id.ToString());
                        var item = await listRepository.GetListItemByIdAsync(param);
                        if (item != null && !string.IsNullOrWhiteSpace(item.Value))
                        {
                            contentTypes.Add(item.Value);
                        }
                    }
                }
            }
            newField.ContentTypes = contentTypes;
            var logWriter = _serviceProvider.GetService<ILogWriter>();
            logWriter?.LogInfo($"AddFieldToFormAsync: resolved upload field ContentTypes=[{string.Join(",", contentTypes ?? new List<string>())}] for field {newFieldName}", "FieldConfigUtils", "AddFieldToFormAsync");
        }

        var listParam = new QueryFilterConfig();
        listParam.AddString("metaflistid", newFieldInfo.List.ToString());
        var listValue = newFieldInfo.List != 0 ? await listRepository.GetListByIdAsync(listParam) : null;
        if (listValue != null)
        {
            newField.OptionsName = listValue.Name;
            newField.MultiSelect = newFieldInfo.MultiSelect != null && newFieldInfo.MultiSelect == "Yes";
        };

        if ((newFieldInfo.TypeId == 453 || newFieldInfo.TypeId == 454) && !string.IsNullOrWhiteSpace(newFieldInfo.ParentList))
        {
            newField.ParentList = newFieldInfo.ParentList.Trim();
            var logWriterParent = _serviceProvider.GetService<ILogWriter>();
            logWriterParent?.LogInfo(
                $"AddFieldToFormAsync: ParentList={newField.ParentList} for field {newField.Id}, childListId={newFieldInfo.List}",
                nameof(FieldConfigUtils),
                nameof(AddFieldToFormAsync));
        }

        if (IsAllowOtherEligible(newFieldInfo))
        {
            newField.AllowOther = true;
            newField.OtherOptionKey = OtherOptionConstants.Key;
        }

        if (newFieldInfo.TypeId == 458)
        {
            newField.Min = newFieldInfo.Min == null ? null : newFieldInfo.Min.ToString();
            newField.Max = newFieldInfo.Max == null ? null : newFieldInfo.Max.ToString();
            newField.MaxDPs = newFieldInfo.Dpts.ToString();
        }

        if (newFieldInfo.TypeId == 459)
        {
            // Compute grid button flags from Include* inputs; default to showing buttons when unspecified
            var includeAdd = string.Equals(newFieldInfo.IncludeAddButton ?? string.Empty, "Yes", StringComparison.OrdinalIgnoreCase);
            var includeDelete = string.Equals(newFieldInfo.IncludeDeleteButton ?? string.Empty, "Yes", StringComparison.OrdinalIgnoreCase);

            newField.RemoveGridAddButton = !includeAdd;
            newField.RemoveGridDeleteButton = !includeDelete;

            var gridItems = new List<FieldGridConfig>();
            var currentLine = 1;
            var gridIdList = formToUpdate.GetGridFieldList().Select(r => r.Id.ToLower()).ToList();
            foreach (var grid in newFieldInfo.FieldGrid)
            {
                var option = new QueryFilterConfig();
                option.AddString("metaflistid", grid.GridOption);
                var isDropdownOrComboBox = grid.GridType == "453" || grid.GridType == "454";
                var optionValue = isDropdownOrComboBox ? await listRepository.GetListByIdAsync(option) : null;

                var validRules = RuleValidationHelper.FilterValidRules(grid.Rules ?? new List<FormGroupRuleModel>());
                var rulesFilteredCount = (grid.Rules?.Count ?? 0) - validRules.Count;
                if (rulesFilteredCount > 0)
                {
                    var logWriterForRules = _serviceProvider.GetService<ILogWriter>();
                    logWriterForRules?.LogInfo($"AddFieldToFormAsync: filtered {rulesFilteredCount} invalid rules for grid column {grid.GridId}, saving {validRules.Count} rules", "FieldConfigUtils", "AddFieldToFormAsync");
                }

                var newItem = new FieldGridConfig
                {
                    Id = grid.GridId.RemoveSpecialCharacters(),
                    GridTitle = grid.GridTitle ?? string.Empty,
                    Type = fieldTypeUtils.GetNameFromId(int.Parse(grid.GridType)),
                    OptionsName = optionValue == null ? null : optionValue.Name.ToLower(),
                    Width = fieldTypeUtils.GetGridNameFromId(int.Parse(grid.GridWidth)),
                    MultiSelect = isDropdownOrComboBox && grid.MultiSelect == "Yes",
                    Rules = validRules.Select(r => new arc.domain.Configuration.PagesConfig.RuleConfig
                    {
                        Effect = r.Effect,
                        Field = r.Field,
                        Rule = r.Rule,
                        Value = r.Value
                    }).ToList()
                };

                var numberCount = 0;
                var newId = newItem.Id;
                while (gridIdList.Contains(newId.ToLower()))
                {
                    newId = newItem.Id + numberCount++;
                }

                newItem.Id = newId;

                gridIdList.Add(newItem.Id.ToLower());
                gridItems.Add(newItem);
                var logWriterColumn = _serviceProvider.GetService<ILogWriter>();
                logWriterColumn?.LogInfo(
                    $"AddFieldToFormAsync: fieldgrid column saved, fieldId={newFieldName}, columnId={newItem.Id}, gridType={grid.GridType}, gridWidth={newItem.Width}, hasGridTitle={!string.IsNullOrWhiteSpace(newItem.GridTitle)}",
                    nameof(FieldConfigUtils),
                    nameof(AddFieldToFormAsync));
                currentLine++;
            }
            newField.GridFields = gridItems;
            var logWriter = _serviceProvider.GetService<ILogWriter>();
            logWriter?.LogInfo($"AddFieldToFormAsync: fieldgrid field={newFieldName}, gridColumns={gridItems?.Count ?? 0}, RemoveGridAddButton={newField.RemoveGridAddButton}", "FieldConfigUtils", "AddFieldToFormAsync");
        }

        formToUpdate.AddField(newField, pageToAddFieldTo, newFieldInfo.RequiredErrorMessage, orderIndex);

        return newField;
    }

    private static bool IsListFieldWithOtherSupport(int typeId) => typeId is 453 or 454 or 151;

    private static bool IsAllowOtherRequested(EditFieldModel fieldInfo) =>
        string.Equals(fieldInfo.AllowOther, "Yes", StringComparison.OrdinalIgnoreCase);

    private static bool IsAllowOtherEligible(EditFieldModel fieldInfo) =>
        IsListFieldWithOtherSupport(fieldInfo.TypeId)
        && !string.Equals(fieldInfo.MultiSelect, "Yes", StringComparison.OrdinalIgnoreCase)
        && IsAllowOtherRequested(fieldInfo);

    private static string GetOtherDetailsCompanionId(string baseFieldName) =>
        baseFieldName + "OtherDetails";

    private static string ResolveFieldIdOnForm(FullFormConfig form, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(fieldName))
        {
            return null;
        }

        var match = form.GetFieldsForForm()
            .FirstOrDefault(f => f.Id.Equals(fieldName, StringComparison.OrdinalIgnoreCase));
        if (match != null)
        {
            return match.Id;
        }

        var withIdSuffix = fieldName.EndsWith("Id", StringComparison.OrdinalIgnoreCase)
            ? fieldName
            : fieldName + "Id";
        match = form.GetFieldsForForm()
            .FirstOrDefault(f => f.Id.Equals(withIdSuffix, StringComparison.OrdinalIgnoreCase));
        return match?.Id;
    }

    private static FieldConfig CreateOtherDetailsCompanion(string baseFieldName, string parentFieldId, string label) =>
        new FieldConfig
        {
            Id = GetOtherDetailsCompanionId(baseFieldName),
            Label = label,
            Type = "singleline",
            OtherDetailsFor = parentFieldId,
            Configurable = "No",
        };

    private static string ResolveOtherDetailsLabel(EditFieldModel fieldInfo) =>
        string.IsNullOrWhiteSpace(fieldInfo.OtherDetailsLabel)
            ? OtherOptionConstants.DetailsLabelTag
            : fieldInfo.OtherDetailsLabel.Trim();

    private async Task ReconcileOtherDetailsCompanionAsync(
        FieldConfig parentField,
        string baseFieldName,
        EditFieldModel fieldInfo,
        FullFormConfig formToUpdate,
        string pageName,
        IReportTranslator sectionFormatAdapter)
    {
        var logWriter = _serviceProvider.GetService<ILogWriter>();
        var companionId = GetOtherDetailsCompanionId(baseFieldName);
        var existingCompanion = formToUpdate.GetFieldsForForm()
            .FirstOrDefault(f => f.Id.Equals(companionId, StringComparison.OrdinalIgnoreCase)
                || (!string.IsNullOrWhiteSpace(f.OtherDetailsFor)
                    && f.OtherDetailsFor.Equals(parentField.Id, StringComparison.OrdinalIgnoreCase)));

        if (IsAllowOtherEligible(fieldInfo))
        {
            var resolvedLabel = ResolveOtherDetailsLabel(fieldInfo);
            if (existingCompanion == null)
            {
                var companion = CreateOtherDetailsCompanion(baseFieldName, parentField.Id, resolvedLabel);
                RegisterFieldOnForm(formToUpdate, companion, pageName, parentField.Id);
                await PropagateNewFieldAsync(companion, companion.Id, formToUpdate, sectionFormatAdapter);
                logWriter?.LogInfo(
                    $"ReconcileOtherDetailsCompanionAsync: created companion {companion.Id} for parent {parentField.Id}",
                    nameof(FieldConfigUtils),
                    nameof(ReconcileOtherDetailsCompanionAsync));
            }
            else
            {
                if (!string.Equals(existingCompanion.OtherDetailsFor, parentField.Id, StringComparison.OrdinalIgnoreCase))
                {
                    existingCompanion.OtherDetailsFor = parentField.Id;
                }
                existingCompanion.Label = resolvedLabel;
            }

            return;
        }

        if (existingCompanion != null)
        {
            await DeleteFieldFromSystemAsync(existingCompanion.Id, formToUpdate);
            logWriter?.LogInfo(
                $"ReconcileOtherDetailsCompanionAsync: removed companion {existingCompanion.Id} for parent {parentField.Id}",
                nameof(FieldConfigUtils),
                nameof(ReconcileOtherDetailsCompanionAsync));
        }
    }

    private static void RegisterFieldOnForm(
        FullFormConfig formToUpdate,
        FieldConfig fieldToAdd,
        string pageName,
        string insertAfterFieldId)
    {
        formToUpdate.AddField(fieldToAdd, pageName, null, -1);
        MoveFieldAfter(formToUpdate, pageName, insertAfterFieldId, fieldToAdd.Id);
    }

    private static void MoveFieldAfter(
        FullFormConfig formToUpdate,
        string pageName,
        string afterFieldId,
        string fieldIdToMove)
    {
        foreach (var page in formToUpdate.PagesConfig ?? [])
        {
            if (!page.Name.Equals(pageName, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            foreach (var column in page.Columns ?? [])
            {
                foreach (var formGroup in column.FormGroups ?? [])
                {
                    var afterIndex = formGroup.Fields.FindIndex(f =>
                        f.Id.Equals(afterFieldId, StringComparison.OrdinalIgnoreCase));
                    var moveIndex = formGroup.Fields.FindIndex(f =>
                        f.Id.Equals(fieldIdToMove, StringComparison.OrdinalIgnoreCase));

                    if (afterIndex < 0 || moveIndex < 0 || moveIndex == afterIndex + 1)
                    {
                        continue;
                    }

                    var fieldToMove = formGroup.Fields[moveIndex];
                    formGroup.Fields.RemoveAt(moveIndex);
                    var insertAt = formGroup.Fields.FindIndex(f =>
                        f.Id.Equals(afterFieldId, StringComparison.OrdinalIgnoreCase));
                    formGroup.Fields.Insert(insertAt + 1, fieldToMove);
                    return;
                }
            }
        }
    }
}
