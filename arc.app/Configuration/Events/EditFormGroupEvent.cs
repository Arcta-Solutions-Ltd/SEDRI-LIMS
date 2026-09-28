using arc.app.Common;
using arc.app.Configuration;
using arc.app.SystemConfig;
using arc.common;
using arc.common.Models.Config;
using arc.domain.Configuration.EventsConfig;
using arc.domain.Configuration.FormStructureConfig;
using arc.domain.Configuration.PagesConfig;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Configuration.Events
{
    /// <summary>
    /// Updates an existing form group: Key, Rules, and field order.
    /// Supports moving fields from other form groups and pages within the form.
    /// Incomplete rules (missing Effect, Field, Rule, or Value when Rule is = or !=) are filtered and not saved.
    /// </summary>
    internal class EditFormGroupEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;

        public EditFormGroupEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var logWriter = _serviceProvider.GetService<ILogWriter>();
            var dataModel = FormGroupEventPayloadReader.Read(dataToSave, logWriter);
            var idSource = !string.IsNullOrEmpty(dataModel.Id) ? dataModel.Id : id;
            var idList = idSource.Split("|");
            if (idList.Length < 4)
            {
                return 0;
            }

            var formName = idList[0];
            var pageName = idList[1];
            var columnKey = idList[2];
            var formGroupKey = idList[3];

            logWriter?.LogInfo($"EditFormGroupEvent: formName={formName}, pageName={pageName}, formGroupKey={formGroupKey}", "EditFormGroupEvent", "RunAsync");

            var formConfigDefinition = _serviceProvider.GetService<IFormConfigDefinition>();
            var formConfigRepository = _serviceProvider.GetService<IFormConfigRepository>();
            var form = await formConfigDefinition.LoadFormAsync(formName);

            var page = form.PagesConfig?.FirstOrDefault(p => p.Name?.Equals(pageName, StringComparison.OrdinalIgnoreCase) == true);
            if (page?.Columns == null) return 0;

            FormGroupConfig targetFormGroup = null;
            ColumnConfig targetColumn = null;
            foreach (var col in page.Columns)
            {
                if (col.Key?.Equals(columnKey, StringComparison.OrdinalIgnoreCase) != true) continue;
                targetFormGroup = col.FormGroups?.FirstOrDefault(fg => fg.Key?.Equals(formGroupKey, StringComparison.OrdinalIgnoreCase) == true);
                if (targetFormGroup != null)
                {
                    targetColumn = col;
                    break;
                }
            }

            if (targetFormGroup == null || targetColumn == null) return 0;

            var inputRules = dataModel.Rules ?? new List<FormGroupRuleModel>();
            var validRules = RuleValidationHelper.FilterValidRules(inputRules);
            var filteredCount = inputRules.Count - validRules.Count;
            if (filteredCount > 0)
            {
                logWriter?.LogInfo($"EditFormGroupEvent: filtered {filteredCount} invalid rules, saving {validRules.Count} rules", "EditFormGroupEvent", "RunAsync");
            }

            targetFormGroup.Rules = validRules.Select(r => new RuleConfig
            {
                Effect = r.Effect,
                Field = r.Field,
                Rule = r.Rule,
                Value = r.Value
            }).ToList();

            var fieldIds = (dataModel.FieldList ?? new List<FieldListModel>()).Select(f => f.Value).Where(v => !string.IsNullOrEmpty(v)).ToList();
            var fieldCountBefore = targetFormGroup.Fields?.Count ?? 0;
            if (fieldCountBefore > 0 && fieldIds.Count == 0)
            {
                logWriter?.LogError(
                    $"EditFormGroupEvent: empty FieldList would remove all {fieldCountBefore} field(s). formName={formName}, pageName={pageName}, formGroupKey={formGroupKey}",
                    "EditFormGroupEvent",
                    "RunAsync");
            }

            var fieldsToReorder = new List<FieldConfig>();
            foreach (var fid in fieldIds)
            {
                var (field, sourcePageName) = GetAndRemoveFieldFromForm(form, fid);
                if (field != null)
                {
                    fieldsToReorder.Add(field);
                    if (!string.IsNullOrEmpty(sourcePageName) && !sourcePageName.Equals(pageName, StringComparison.OrdinalIgnoreCase))
                    {
                        logWriter?.LogInfo($"EditFormGroupEvent: Moved field {fid} from page {sourcePageName} to form group {formGroupKey} on page {pageName}", "EditFormGroupEvent", "RunAsync");
                    }
                }
                else
                {
                    logWriter?.LogInfo($"EditFormGroupEvent: Field {fid} not found in form {formName}", "EditFormGroupEvent", "RunAsync");
                }
            }

            targetFormGroup.Fields.Clear();
            foreach (var f in fieldsToReorder)
            {
                targetFormGroup.Fields.Add(f);
            }

            logWriter?.LogInfo(
                $"EditFormGroupEvent: saved {fieldsToReorder.Count} field(s) in order [{string.Join(", ", fieldIds)}]",
                "EditFormGroupEvent",
                "RunAsync");

            await formConfigRepository.UpdateFormAsync(form);
            return 0;
        }

        /// <summary>
        /// Searches all pages in the form for the field, removes it from its current form group, and returns it.
        /// Supports moving fields across pages within the same form.
        /// </summary>
        /// <param name="form">The form configuration.</param>
        /// <param name="fieldId">The field identifier.</param>
        /// <returns>Tuple of (removed field, source page name) or (null, null) if not found.</returns>
        private static (FieldConfig field, string sourcePageName) GetAndRemoveFieldFromForm(FullFormConfig form, string fieldId)
        {
            foreach (var page in form.PagesConfig ?? new List<PageConfig>())
            {
                foreach (var col in page.Columns ?? new List<ColumnConfig>())
                {
                    foreach (var fg in col.FormGroups ?? new List<FormGroupConfig>())
                    {
                        var field = fg.Fields?.FirstOrDefault(f => f.Id?.Equals(fieldId, StringComparison.OrdinalIgnoreCase) == true);
                        if (field != null)
                        {
                            fg.Fields = fg.Fields.Where(f => !f.Id.Equals(fieldId, StringComparison.OrdinalIgnoreCase)).ToList();
                            return (field, page.Name);
                        }
                    }
                }
            }
            return (null, null);
        }
    }
}
