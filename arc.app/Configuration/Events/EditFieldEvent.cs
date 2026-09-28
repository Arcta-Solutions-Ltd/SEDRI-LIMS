using arc.app.Common;
using arc.common;
using arc.common.Models.Config;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using arc.app.SystemConfig;
using System.Linq;

namespace arc.app.Configuration.Events
{
    /// <summary>
    /// Handles the event for editing an existing field in a form configuration.
    /// This event is responsible for updating the field's information, maintaining its position in the form structure,
    /// and updating related configurations such as report sections and record views.
    /// </summary>
    internal class EditFieldEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditFieldEvent"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider for dependency resolution.</param>
        public EditFieldEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Executes the logic to edit an existing field in the system.
        /// </summary>
        /// <param name="dataToSave">JSON string containing the updated field's data (<see cref="EditFieldModel"/>) and the field identifier.</param>
        /// <param name="id">A pipe-separated string containing the form ID and the page name where the field is located.</param>
        /// <param name="command">The event command model.</param>
        /// <param name="eventData">Optional event configuration data.</param>
        /// <returns>A task representing the asynchronous operation, returning 0 on success.</returns>
        /// <remarks>
        /// This method performs the following steps:
        /// 1. Deserializes the updated field information from <paramref name="dataToSave"/>.
        /// 2. Retrieves the current field's position index in the page.
        /// 3. Deletes the existing field from the form configuration using <see cref="IFieldConfigUtils"/>.
        /// 4. Adds the updated field back to the form configuration at the same position.
        /// 5. Persists the updated form configuration.
        /// This delete-and-add approach ensures that all field properties are properly updated while maintaining the field's position.
        /// </remarks>
        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var formConfigDefinition = _serviceProvider.GetService<IFormConfigDefinition>();
            var formConfigRepository = _serviceProvider.GetService<IFormConfigRepository>();
            var fieldConfigUtils = _serviceProvider.GetService<IFieldConfigUtils>();

            var idModel = JsonConvert.DeserializeObject<IdModel>(dataToSave);
            var idList = idModel.Id.Split("|");
            var formToUpdate = await formConfigDefinition.LoadFormAsync(idList[0]);

            var pageId = idList[1];
            var fieldId = idList[2];
            var currentFieldIndex = formToUpdate.PagesConfig.First(x => x.Name == pageId).GetFieldList().FindIndex(x => x.Id == fieldId);

            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore };
            var newFieldInfo = JsonConvert.DeserializeObject<EditFieldModel>(dataToSave, settings);

            formToUpdate = await fieldConfigUtils.DeleteFieldFromSystemAsync(newFieldInfo.FieldId, formToUpdate);
            formToUpdate = await fieldConfigUtils.AddFieldToSystemAsync(newFieldInfo.FieldId, newFieldInfo, formToUpdate, dataToSave, idList[1], currentFieldIndex);

            if (newFieldInfo.TypeId == 453 || newFieldInfo.TypeId == 454)
            {
                var logWriter = _serviceProvider.GetService<ILogWriter>();
                logWriter?.LogInfo(
                    $"EditField: saved list field FieldId={newFieldInfo.FieldId}, FormId={idList[0]}, ParentList='{newFieldInfo.ParentList ?? ""}'",
                    nameof(EditFieldEvent),
                    nameof(RunAsync));
            }

            await formConfigRepository.UpdateFormAsync(formToUpdate);

            // TypeId 459 = fieldgrid (see FieldConfigUtils / field type list). Helps trace data-section/report grid sync at customer sites without debuggers.
            if (newFieldInfo.TypeId == 459)
            {
                var logWriter = _serviceProvider.GetService<ILogWriter>();
                var dataSectionGridCount = formToUpdate.DataSectionConfig?.Grids?.Count ?? 0;
                var reportGridSummary = formToUpdate.ReportSectionConfigList == null
                    ? string.Empty
                    : string.Join(",", formToUpdate.ReportSectionConfigList.Select(s => s.Grids?.Count ?? 0));
                logWriter?.LogInfo(
                    $"EditField: saved fieldgrid FieldId={newFieldInfo.FieldId}, FormId={idList[0]}, DataSectionGrids={dataSectionGridCount}, ReportSectionGridCountsPerSection=[{reportGridSummary}]",
                    nameof(EditFieldEvent),
                    nameof(RunAsync));
            }

            return 0;
        }
    }
}
