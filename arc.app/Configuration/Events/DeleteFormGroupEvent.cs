using arc.app.Common;
using arc.app.SystemConfig;
using arc.common;
using arc.domain.Configuration.EventsConfig;
using arc.domain.Configuration.PagesConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Configuration.Events
{
    /// <summary>
    /// Removes a form group from a page. Its fields are moved to the previous form group in the same column,
    /// or to the first form group if it is the first.
    /// </summary>
    internal class DeleteFormGroupEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;

        public DeleteFormGroupEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var idList = id.Split("|");
            if (idList.Length < 4)
            {
                return 0;
            }

            var formName = idList[0];
            var pageName = idList[1];
            var columnKey = idList[2];
            var formGroupKey = idList[3];

            var logWriter = _serviceProvider.GetService<ILogWriter>();
            logWriter?.LogInfo($"DeleteFormGroupEvent: formName={formName}, pageName={pageName}, formGroupKey={formGroupKey}", "DeleteFormGroupEvent", "RunAsync");

            var formConfigDefinition = _serviceProvider.GetService<IFormConfigDefinition>();
            var formConfigRepository = _serviceProvider.GetService<IFormConfigRepository>();
            var form = await formConfigDefinition.LoadFormAsync(formName);

            var page = form.PagesConfig?.FirstOrDefault(p => p.Name?.Equals(pageName, StringComparison.OrdinalIgnoreCase) == true);
            if (page?.Columns == null) return 0;

            ColumnConfig targetColumn = null;
            int formGroupIndex = -1;
            foreach (var col in page.Columns)
            {
                if (col.Key?.Equals(columnKey, StringComparison.OrdinalIgnoreCase) != true) continue;
                var idx = col.FormGroups?.FindIndex(fg => fg.Key?.Equals(formGroupKey, StringComparison.OrdinalIgnoreCase) == true) ?? -1;
                if (idx >= 0)
                {
                    targetColumn = col;
                    formGroupIndex = idx;
                    break;
                }
            }

            if (targetColumn == null || formGroupIndex < 0) return 0;

            var formGroupToDelete = targetColumn.FormGroups[formGroupIndex];
            var fieldsToMove = formGroupToDelete.Fields?.ToList() ?? new System.Collections.Generic.List<FieldConfig>();

            FormGroupConfig targetFormGroup = null;
            if (formGroupIndex > 0)
            {
                targetFormGroup = targetColumn.FormGroups[formGroupIndex - 1];
            }
            else if (targetColumn.FormGroups.Count > 1)
            {
                targetFormGroup = targetColumn.FormGroups[1];
            }

            if (targetFormGroup != null)
            {
                foreach (var f in fieldsToMove)
                {
                    targetFormGroup.Fields.Add(f);
                }
            }

            targetColumn.FormGroups.RemoveAt(formGroupIndex);
            await formConfigRepository.UpdateFormAsync(form);

            return 0;
        }
    }
}
