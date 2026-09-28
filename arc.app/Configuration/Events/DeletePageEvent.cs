using arc.app.Common;
using arc.common;
using arc.common.Models.Config;
using arc.domain.Configuration.EventsConfig;
using Newtonsoft.Json;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using arc.app.SystemConfig;

namespace arc.app.Configuration.Events
{
    internal class DeletePageEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;

        public DeletePageEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Deletes a page and its fields from the form. Anchor pages that start a page group
        /// cannot be deleted.
        /// </summary>
        /// <param name="dataToSave">Serialized payload containing the form and page ids.</param>
        /// <param name="id">Fallback record identifier.</param>
        /// <param name="command">The event command wrapper.</param>
        /// <param name="eventData">The event definition. Not used by this event.</param>
        /// <returns>Zero; this event does not create a record.</returns>
        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var dataModel = JsonConvert.DeserializeObject<AddPageModel>(dataToSave);
            var idList = dataModel.Id.Split("|");
            var formName = idList[0];
            var pageName = idList[1];

            var formConfigDefinition = _serviceProvider.GetService<IFormConfigDefinition>();
            var formConfigRepository = _serviceProvider.GetService<IFormConfigRepository>();
            var fieldConfigUtils = _serviceProvider.GetService<IFieldConfigUtils>();
            var configRepository = _serviceProvider.GetService<IConfigRepository>();

            var formToDeletePageFrom = await formConfigDefinition.LoadFormAsync(formName);
            var logWriter = _serviceProvider.GetService<ILogWriter>();

            var pageConfig = formToDeletePageFrom.GetPage(pageName);
            if (pageConfig == null)
            {
                logWriter?.LogInfo(
                    $"DeletePageEvent: page {pageName} not found on form {formName}",
                    nameof(DeletePageEvent),
                    nameof(RunAsync));
                return 0;
            }

            if (pageConfig.GroupAnchor > 0)
            {
                logWriter?.LogInfo(
                    $"DeletePageEvent: refused delete of anchor page {pageName} on form {formName}; group={pageConfig.PageGroup} anchor={pageConfig.GroupAnchor}",
                    nameof(DeletePageEvent),
                    nameof(RunAsync));
                throw new ArgumentException("@ConPagP@");
            }

            foreach (var field in pageConfig.GetFieldList())
            {
                formToDeletePageFrom = await fieldConfigUtils.DeleteFieldFromSystemAsync(field.Id, formToDeletePageFrom);
            }

            formToDeletePageFrom.RemovePage(pageName);

            //Remove page from the form definition
            await formConfigRepository.UpdateFormAsync(formToDeletePageFrom);

            //Remove page from the database
            await configRepository.DeleteCustomEntryAsync(pageName);

            return 0;
        }
    }
}
