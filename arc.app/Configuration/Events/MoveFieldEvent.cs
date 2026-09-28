using arc.app.Common;
using arc.app.SystemConfig;
using arc.common;
using arc.common.Models.Config;
using arc.domain.Configuration.EventsConfig;
using arc.domain.Configuration.FormStructureConfig;
using arc.domain.Configuration.PagesConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Configuration.Events
{
    /// <summary>
    /// Moves a field from its current form group to a target form group.
    /// Supports moving across pages within the same form.
    /// Id: formName|pageName|fieldId. Payload: TargetFormGroupId = formName|pageName|columnKey|formGroupKey.
    /// </summary>
    internal class MoveFieldEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;

        public MoveFieldEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Removes the field from its current form group (searching all pages) and adds it to the target form group.
        /// </summary>
        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var dataModel = JsonConvert.DeserializeObject<MoveFieldModel>(dataToSave);
            var idSource = !string.IsNullOrEmpty(dataModel?.Id) ? dataModel.Id : id;
            var idList = idSource.Split("|");
            if (idList.Length < 3)
            {
                return 0;
            }

            var formName = idList[0];
            var pageName = idList[1];
            var fieldId = idList[2];

            var targetId = dataModel?.TargetFormGroupId ?? "";
            var targetList = targetId.Split("|");
            if (targetList.Length < 4)
            {
                return 0;
            }

            var targetFormName = targetList[0];
            var targetPageName = targetList[1];
            var columnKey = targetList[2];
            var formGroupKey = targetList[3];

            var logWriter = _serviceProvider.GetService<ILogWriter>();
            logWriter?.LogInfo($"MoveFieldEvent: formName={formName}, fieldId={fieldId}, targetFormGroupId={targetId}", "MoveFieldEvent", "RunAsync");

            var formConfigDefinition = _serviceProvider.GetService<IFormConfigDefinition>();
            var formConfigRepository = _serviceProvider.GetService<IFormConfigRepository>();
            var form = await formConfigDefinition.LoadFormAsync(formName);
            if (form == null || formName != targetFormName)
            {
                return 0;
            }

            var targetPage = form.PagesConfig?.FirstOrDefault(p => p.Name?.Equals(targetPageName, StringComparison.OrdinalIgnoreCase) == true);
            if (targetPage?.Columns == null)
            {
                return 0;
            }

            FormGroupConfig targetFormGroup = null;
            foreach (var col in targetPage.Columns)
            {
                if (col.Key?.Equals(columnKey, StringComparison.OrdinalIgnoreCase) != true) continue;
                targetFormGroup = col.FormGroups?.FirstOrDefault(fg => fg.Key?.Equals(formGroupKey, StringComparison.OrdinalIgnoreCase) == true);
                if (targetFormGroup != null) break;
            }

            if (targetFormGroup == null)
            {
                logWriter?.LogInfo($"MoveFieldEvent: Target form group not found: {targetId}", "MoveFieldEvent", "RunAsync");
                return 0;
            }

            var sourceFormGroupId = FindFormGroupIdForField(form, fieldId);
            if (!string.IsNullOrEmpty(sourceFormGroupId) && sourceFormGroupId.Equals(targetId, StringComparison.OrdinalIgnoreCase))
            {
                logWriter?.LogInfo($"MoveFieldEvent: Field {fieldId} is already in target form group {targetId}; no move performed", "MoveFieldEvent", "RunAsync");
                return 0;
            }

            var field = GetAndRemoveFieldFromForm(form, fieldId);
            if (field == null)
            {
                logWriter?.LogInfo($"MoveFieldEvent: Field {fieldId} not found in form {formName}", "MoveFieldEvent", "RunAsync");
                return 0;
            }

            targetFormGroup.Fields ??= new List<FieldConfig>();
            targetFormGroup.Fields.Add(field);

            if (!pageName.Equals(targetPageName, StringComparison.OrdinalIgnoreCase))
            {
                logWriter?.LogInfo($"MoveFieldEvent: Moved field {fieldId} from page {pageName} to form group {formGroupKey} on page {targetPageName}", "MoveFieldEvent", "RunAsync");
            }

            await formConfigRepository.UpdateFormAsync(form);
            return 0;
        }

        /// <summary>
        /// Searches all pages in the form for the field, removes it from its current form group, and returns it.
        /// </summary>
        /// <param name="form">The form configuration.</param>
        /// <param name="fieldId">The field identifier.</param>
        /// <returns>The removed field, or null if not found.</returns>
        private static FieldConfig GetAndRemoveFieldFromForm(FullFormConfig form, string fieldId)
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
                            return field;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Returns the composite form group id (formName|pageName|columnKey|formGroupKey) containing the field, or null.
        /// </summary>
        private static string FindFormGroupIdForField(FullFormConfig form, string fieldId)
        {
            foreach (var page in form.PagesConfig ?? new List<PageConfig>())
            {
                foreach (var col in page.Columns ?? new List<ColumnConfig>())
                {
                    foreach (var fg in col.FormGroups ?? new List<FormGroupConfig>())
                    {
                        if (fg.Fields?.Any(f => f.Id?.Equals(fieldId, StringComparison.OrdinalIgnoreCase) == true) == true)
                        {
                            return $"{form.Name}|{page.Name}|{col.Key}|{fg.Key}";
                        }
                    }
                }
            }
            return null;
        }
    }
}
