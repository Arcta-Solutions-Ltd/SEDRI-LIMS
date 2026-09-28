using arc.app.Common;
using arc.app.SystemConfig;
using arc.common;
using arc.common.Models.Config;
using arc.domain.Configuration.EventsConfig;
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
    /// Reorders form groups on a page. Flattens form groups from all columns, applies the new order,
    /// and places all form groups in the first column.
    /// </summary>
    internal class ReorderFormGroupsEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;

        public ReorderFormGroupsEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var dataModel = JsonConvert.DeserializeObject<EditPageModel>(dataToSave);
            var idSource = !string.IsNullOrEmpty(dataModel.Id) ? dataModel.Id : id;
            var idList = idSource.Split("|");
            if (idList.Length < 2)
            {
                return 0;
            }

            var formName = idList[0];
            var pageName = idList[1];

            var logWriter = _serviceProvider.GetService<ILogWriter>();
            var formConfigDefinition = _serviceProvider.GetService<IFormConfigDefinition>();
            var formConfigRepository = _serviceProvider.GetService<IFormConfigRepository>();
            var form = await formConfigDefinition.LoadFormAsync(formName);

            var page = form?.PagesConfig?.FirstOrDefault(p => p.Name?.Equals(pageName, StringComparison.OrdinalIgnoreCase) == true);
            if (page?.Columns == null || page.Columns.Count == 0)
            {
                return 0;
            }

            var formGroupCountBefore = page.Columns.Sum(c => c.FormGroups?.Count ?? 0);
            logWriter?.LogInfo($"ReorderFormGroupsEvent: formName={formName}, pageName={pageName}, formGroupCount={formGroupCountBefore}", "ReorderFormGroupsEvent", "RunAsync");

            var formGroupMap = new Dictionary<string, FormGroupConfig>(StringComparer.OrdinalIgnoreCase);
            foreach (var col in page.Columns)
            {
                foreach (var fg in col.FormGroups ?? new List<FormGroupConfig>())
                {
                    var key = $"{col.Key}|{fg.Key}";
                    formGroupMap[key] = fg;
                }
            }

            var orderedValues = (dataModel.FieldList ?? new List<FieldListModel>())
                .Select(f => f.Value)
                .Where(v => !string.IsNullOrEmpty(v))
                .ToList();

            if (formGroupCountBefore > 0 && orderedValues.Count == 0)
            {
                logWriter?.LogError(
                    $"ReorderFormGroupsEvent: empty FieldList would remove all {formGroupCountBefore} form group(s). formName={formName}, pageName={pageName}",
                    "ReorderFormGroupsEvent",
                    "RunAsync");
            }

            var orderedFormGroups = new List<FormGroupConfig>();
            foreach (var value in orderedValues)
            {
                if (formGroupMap.TryGetValue(value, out var fg))
                {
                    orderedFormGroups.Add(fg);
                }
            }

            foreach (var col in page.Columns)
            {
                col.FormGroups = new List<FormGroupConfig>();
            }

            foreach (var fg in orderedFormGroups)
            {
                page.Columns[0].FormGroups.Add(fg);
            }

            await formConfigRepository.UpdateFormAsync(form);

            var formGroupCountAfter = page.Columns[0].FormGroups.Count;
            var fieldCountAfter = page.Columns[0].FormGroups.Sum(fg => fg.Fields?.Count ?? 0);
            logWriter?.LogInfo($"ReorderFormGroupsEvent: formName={formName}, pageName={pageName}, formGroupCountAfter={formGroupCountAfter}, fieldCountAfter={fieldCountAfter}", "ReorderFormGroupsEvent", "RunAsync");

            return 0;
        }
    }
}
