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
    /// Moves a form group (subsection) from one page to another within the same form.
    /// The form group is appended to the end of the first column on the target page.
    /// Id: formName|pageName|columnKey|formGroupKey. Payload: TargetPageId = target page name.
    /// </summary>
    internal class MoveFormGroupEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;

        public MoveFormGroupEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Removes the source form group and appends it to the target page's first column.
        /// </summary>
        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var dataModel = JsonConvert.DeserializeObject<MoveFormGroupModel>(dataToSave);
            var idSource = !string.IsNullOrEmpty(dataModel?.Id) ? dataModel.Id : id;
            var idList = idSource.Split("|");
            if (idList.Length < 4)
            {
                return 0;
            }

            var formName = idList[0];
            var sourcePageName = idList[1];
            var columnKey = idList[2];
            var formGroupKey = idList[3];
            var targetPageName = dataModel?.TargetPageId ?? "";

            var logWriter = _serviceProvider.GetService<ILogWriter>();
            logWriter?.LogInfo(
                $"MoveFormGroupEvent: formName={formName}, sourcePage={sourcePageName}, formGroupKey={formGroupKey}, targetPage={targetPageName}",
                "MoveFormGroupEvent",
                "RunAsync");

            if (string.IsNullOrWhiteSpace(targetPageName)
                || sourcePageName.Equals(targetPageName, StringComparison.OrdinalIgnoreCase))
            {
                logWriter?.LogInfo("MoveFormGroupEvent: target page missing or same as source; no move performed", "MoveFormGroupEvent", "RunAsync");
                return 0;
            }

            var formConfigDefinition = _serviceProvider.GetService<IFormConfigDefinition>();
            var formConfigRepository = _serviceProvider.GetService<IFormConfigRepository>();
            var form = await formConfigDefinition.LoadFormAsync(formName);
            if (form == null)
            {
                return 0;
            }

            var sourcePage = form.PagesConfig?.FirstOrDefault(p => p.Name?.Equals(sourcePageName, StringComparison.OrdinalIgnoreCase) == true);
            var targetPage = form.PagesConfig?.FirstOrDefault(p => p.Name?.Equals(targetPageName, StringComparison.OrdinalIgnoreCase) == true);
            if (sourcePage?.Columns == null || targetPage?.Columns == null)
            {
                logWriter?.LogInfo("MoveFormGroupEvent: source or target page not found", "MoveFormGroupEvent", "RunAsync");
                return 0;
            }

            FormGroupConfig formGroupToMove = null;
            ColumnConfig sourceColumn = null;
            foreach (var col in sourcePage.Columns)
            {
                if (col.Key?.Equals(columnKey, StringComparison.OrdinalIgnoreCase) != true) continue;
                var idx = col.FormGroups?.FindIndex(fg => fg.Key?.Equals(formGroupKey, StringComparison.OrdinalIgnoreCase) == true) ?? -1;
                if (idx >= 0)
                {
                    sourceColumn = col;
                    formGroupToMove = col.FormGroups[idx];
                    col.FormGroups.RemoveAt(idx);
                    break;
                }
            }

            if (formGroupToMove == null)
            {
                logWriter?.LogInfo($"MoveFormGroupEvent: form group {formGroupKey} not found on page {sourcePageName}", "MoveFormGroupEvent", "RunAsync");
                return 0;
            }

            var remainingOnSource = sourcePage.Columns.Sum(c => c.FormGroups?.Count ?? 0);
            if (remainingOnSource < 1)
            {
                sourceColumn.FormGroups.Add(formGroupToMove);
                logWriter?.LogInfo(
                    $"MoveFormGroupEvent: cannot move last subsection from page {sourcePageName}",
                    "MoveFormGroupEvent",
                    "RunAsync");
                return 0;
            }

            var movedFieldIds = (formGroupToMove.Fields ?? new List<FieldConfig>())
                .Select(f => f.Id)
                .Where(id => !string.IsNullOrWhiteSpace(id))
                .ToList();

            LogOrphanedPageRuleReferences(sourcePage, movedFieldIds, logWriter);

            if (targetPage.Columns.Count == 0)
            {
                targetPage.Columns.Add(new ColumnConfig { Key = "col1", FormGroups = new List<FormGroupConfig>() });
            }

            var targetColumn = targetPage.Columns[0];
            targetColumn.FormGroups ??= new List<FormGroupConfig>();
            targetColumn.FormGroups.Add(formGroupToMove);

            await formConfigRepository.UpdateFormAsync(form);

            logWriter?.LogInfo(
                $"MoveFormGroupEvent: moved form group {formGroupKey} from {sourcePageName} to {targetPageName}, fieldCount={movedFieldIds.Count}, targetFormGroupCount={targetColumn.FormGroups.Count}",
                "MoveFormGroupEvent",
                "RunAsync");

            return 0;
        }

        /// <summary>
        /// Logs a warning when source page state rules reference field ids that are leaving the page.
        /// </summary>
        private static void LogOrphanedPageRuleReferences(PageConfig sourcePage, List<string> movedFieldIds, ILogWriter logWriter)
        {
            if (movedFieldIds.Count == 0 || logWriter == null || sourcePage == null)
            {
                return;
            }

            var stateRules = sourcePage.NextButton?.OnClickState?.Rules ?? new List<RuleConfig>();
            var orphaned = stateRules
                .Where(r => !string.IsNullOrWhiteSpace(r.Field) && movedFieldIds.Any(f => f.Equals(r.Field, StringComparison.OrdinalIgnoreCase)))
                .Select(r => r.Field)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (orphaned.Count > 0)
            {
                logWriter.LogInfo(
                    $"MoveFormGroupEvent: page {sourcePage.Name} state rules reference field(s) leaving the page: {string.Join(", ", orphaned)}",
                    "MoveFormGroupEvent",
                    nameof(LogOrphanedPageRuleReferences));
            }
        }
    }
}
