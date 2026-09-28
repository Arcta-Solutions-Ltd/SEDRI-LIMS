using arc.app.Common;
using arc.app.Configuration;
using arc.app.SystemConfig;
using arc.common;
using arc.common.ExtensionMethods;
using arc.common.Models.Config;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Configuration.Events
{
    internal class EditPageEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditPageEvent"/> class.
        /// </summary>
        /// <param name="serviceProvider">Application service provider.</param>
        public EditPageEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Updates page title, description and TableName on the form configuration.
        /// </summary>
        /// <param name="dataToSave">Serialised edit page payload.</param>
        /// <param name="id">Context id (unused).</param>
        /// <param name="command">Event command metadata.</param>
        /// <param name="eventData">Optional event configuration.</param>
        /// <returns>Zero on success.</returns>
        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var dataModel = JsonConvert.DeserializeObject<EditPageModel>(dataToSave);
            var idList = dataModel.Id.Split("|");
            var pageName = idList[1];

            var formConfigDefinition = _serviceProvider.GetService<IFormConfigDefinition>();
            var formConfigRepository = _serviceProvider.GetService<IFormConfigRepository>();
            var formToUpdate = await formConfigDefinition.LoadFormAsync(idList[0]);

            var pageToUpdate = formToUpdate.PagesConfig?.FirstOrDefault(p => p.Name?.Equals(pageName, StringComparison.OrdinalIgnoreCase) == true);
            if (pageToUpdate == null) return 0;

            pageToUpdate.PageTitle = dataModel.Name;
            pageToUpdate.Text = dataModel.Description;
            if (FormPageTargetTableExtensions.IsSpecimenRecordForm(formToUpdate.SingleItemName))
            {
                pageToUpdate.TableName = NormalizeTableName(dataModel.TableName);
            }

            var previousGroup = pageToUpdate.PageGroup;
            PageGroupUtils.AssignGroupFromTableName(pageToUpdate, formToUpdate);
            if (!string.Equals(previousGroup, pageToUpdate.PageGroup, StringComparison.OrdinalIgnoreCase))
            {
                var normalised = PageGroupUtils.Normalise(formToUpdate, formToUpdate.Pages);
                PageGroupUtils.ApplyOrder(formToUpdate, normalised);
            }

            var logWriter = _serviceProvider.GetService<ILogWriter>();
            logWriter?.LogInfo(
                $"EditPageEvent: form={formToUpdate.Name}, page={pageName}, table={pageToUpdate.TableName}, pageGroup={pageToUpdate.PageGroup ?? "(none)"}",
                nameof(EditPageEvent),
                nameof(RunAsync));

            await formConfigRepository.UpdateFormAsync(formToUpdate);

            return 0;
        }

        private static string NormalizeTableName(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                return "specimen";
            }

            var normalized = FormPageTargetTableExtensions.Normalize(tableName);
            return FormPageTargetTableExtensions.IsSpecimenFormTarget(normalized) ? normalized : "specimen";
        }
    }
}
