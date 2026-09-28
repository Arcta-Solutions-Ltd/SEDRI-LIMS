using arc.app.Common;
using arc.app.Configuration;
using arc.app.SystemConfig;
using arc.common;
using arc.common.Models.Config;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Configuration.Events
{
    /// <summary>
    /// Applies a new page order to the form and persists the updated configuration.
    /// Orders that break a page group invariant are rejected with the matching language tag.
    /// </summary>
    internal class EditPagesEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;

        public EditPagesEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Validates the submitted page order, then writes it onto the form.
        /// </summary>
        /// <param name="dataToSave">Serialized <see cref="PageOrderModel"/> from the Page Order form.</param>
        /// <param name="id">Fallback record identifier.</param>
        /// <param name="command">The event command wrapper.</param>
        /// <param name="eventData">The event definition. Not used by this event.</param>
        /// <returns>Zero; this event does not create a record.</returns>
        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var dataModel = JsonConvert.DeserializeObject<PageOrderModel>(dataToSave);
            var formName = PageGroupUtils.ResolveFormName(dataModel?.Id) ?? PageGroupUtils.ResolveFormName(id);

            var formConfigDefinition = _serviceProvider.GetService<IFormConfigDefinition>();
            var formConfigRepository = _serviceProvider.GetService<IFormConfigRepository>();
            var logWriter = _serviceProvider.GetService<ILogWriter>();
            var formToUpdate = await formConfigDefinition.LoadFormAsync(formName);

            var proposedOrder = dataModel?.PageOrder?
                .Select(p => p.Id)
                .Where(v => !string.IsNullOrWhiteSpace(v))
                .ToList() ?? new List<string>();

            var breaches = PageGroupUtils.Validate(formToUpdate, proposedOrder);
            if (breaches.Count > 0)
            {
                foreach (var breach in breaches)
                {
                    logWriter?.LogInfo(
                        $"EditPagesEvent: rejected reorder on form {formName}; reason={breach.Reason}; group={breach.GroupId}; pages=[{string.Join(", ", breach.PageNames)}]",
                        nameof(EditPagesEvent),
                        nameof(RunAsync));
                }

                throw new ArgumentException(breaches[0].MessageTag);
            }

            PageGroupUtils.ApplyOrder(formToUpdate, proposedOrder);

            var fullFieldList = formToUpdate.GetFieldsForForm();
            var orderedFieldList = new List<string>();

            foreach (var field in fullFieldList)
            {
                orderedFieldList.Add(field.Label);
            }

            formToUpdate.SaveEventConfig.ReorderDisplayConfig(orderedFieldList);
            await formConfigRepository.UpdateFormAsync(formToUpdate);

            logWriter?.LogInfo(
                $"EditPagesEvent: saved page order for form {formName}; order=[{string.Join(", ", proposedOrder)}]",
                nameof(EditPagesEvent),
                nameof(RunAsync));

            return 0;
        }
    }
}
