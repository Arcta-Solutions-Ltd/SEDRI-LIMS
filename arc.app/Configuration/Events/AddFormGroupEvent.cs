using arc.app.Common;
using arc.app.Configuration;
using arc.app.SystemConfig;
using arc.common;
using arc.common.Models.Config;
using arc.domain.Configuration.EventsConfig;
using arc.domain.Configuration.PagesConfig;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Configuration.Events
{
    /// <summary>
    /// Adds a new empty form group to a page. Places it in the first column.
    /// Fields are assigned later via Edit Form Group. Incomplete rules (missing Effect, Field, Rule,
    /// or Value when Rule is = or !=) are filtered and not saved.
    /// </summary>
    internal class AddFormGroupEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;

        public AddFormGroupEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var logWriter = _serviceProvider.GetService<ILogWriter>();
            var dataModel = FormGroupEventPayloadReader.ReadForAdd(dataToSave, logWriter);
            var idSource = !string.IsNullOrEmpty(dataModel.Id) ? dataModel.Id : id;
            var idList = idSource.Split("|");
            var formName = idList.Length > 0 ? idList[0] : "";
            var pageName = idList.Length > 1 ? idList[1] : "";

            var formConfigDefinition = _serviceProvider.GetService<IFormConfigDefinition>();
            var formConfigRepository = _serviceProvider.GetService<IFormConfigRepository>();
            var form = await formConfigDefinition.LoadFormAsync(formName);

            var page = form.PagesConfig?.FirstOrDefault(p => p.Name?.Equals(pageName, StringComparison.OrdinalIgnoreCase) == true);
            if (page == null || page.Columns == null || page.Columns.Count == 0)
            {
                return 0;
            }

            var firstColumn = page.Columns[0];
            var nextIndex = (firstColumn.FormGroups?.Count ?? 0) + 1;
            var newKey = $"fg{nextIndex}";

            logWriter?.LogInfo($"AddFormGroupEvent: formName={formName}, pageName={pageName}, newKey={newKey}", "AddFormGroupEvent", "RunAsync");

            var inputRules = dataModel.Rules ?? new List<FormGroupRuleModel>();
            var validRules = RuleValidationHelper.FilterValidRules(inputRules);
            var filteredCount = inputRules.Count - validRules.Count;
            if (filteredCount > 0)
            {
                logWriter?.LogInfo($"AddFormGroupEvent: filtered {filteredCount} invalid rules, saving {validRules.Count} rules", "AddFormGroupEvent", "RunAsync");
            }

            var rules = validRules.Select(r => new RuleConfig
            {
                Effect = r.Effect,
                Field = r.Field,
                Rule = r.Rule,
                Value = r.Value
            }).ToList();

            var newFormGroup = new FormGroupConfig { Key = newKey, Rules = rules };

            firstColumn.FormGroups.Add(newFormGroup);
            await formConfigRepository.UpdateFormAsync(form);

            return 0;
        }
    }
}
