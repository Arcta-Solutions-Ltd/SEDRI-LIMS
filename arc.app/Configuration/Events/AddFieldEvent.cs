using arc.common;
using arc.domain.Configuration.EventsConfig;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using arc.app.Common;
using Newtonsoft.Json;
using arc.common.Models.Config;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.app.SystemConfig;
using arc.common.ExtensionMethods;

namespace arc.app.Configuration.Events
{
    /// <summary>
    /// Executes the "add field" configuration event. This handler loads the target form,
    /// derives a safe field identifier and a repository-assigned unique field name, delegates
    /// to utilities to update the form configuration, and persists the result.
    /// </summary>
    /// <summary>
    /// Adds a field to a form configuration, including mapping fieldgrid UI flags to stored flags.
    /// </summary>
    internal class AddFieldEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Creates a new <see cref="AddFieldEvent"/>.
        /// </summary>
        /// <param name="serviceProvider">Service provider for required configuration services.</param>
        public AddFieldEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Runs the add-field operation. For fieldgrid (TypeId 459), maps IncludeAdd/DeleteButton ("Yes"|"No")
        /// to RemoveGridAdd/DeleteButton (bool) before persistence.
        /// </summary>
        /// <param name="dataToSave">JSON payload describing the field to add (e.g., <see cref="EditFieldModel"/>).</param>
        /// <param name="id">Pipe-delimited context string where the first segment is the form identifier; additional segments may convey scope.</param>
        /// <param name="command">The event command metadata.</param>
        /// <param name="eventData">Optional event configuration data.</param>
        /// <returns>0 on success.</returns>
        /// <remarks>
        /// This method may propagate exceptions from underlying services if the form cannot be loaded or saved.
        /// </remarks>
        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var idList = id.Split("|");

            var formConfigDefinition = _serviceProvider.GetService<IFormConfigDefinition>();
            var formConfigRepository = _serviceProvider.GetService<IFormConfigRepository>();
            var fieldConfigUtils = _serviceProvider.GetService<IFieldConfigUtils>();

            var formToUpdate = await formConfigDefinition.LoadFormAsync(idList[0]);

            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore };
            var newFieldInfo = JsonConvert.DeserializeObject<EditFieldModel>(dataToSave, settings);

            newFieldInfo.FieldId = newFieldInfo.Label.Replace(" ", "").RemoveSpecialCharacters().Trim();
            if (newFieldInfo.FieldId.Length > 36)
            {
                newFieldInfo.FieldId = newFieldInfo.FieldId[..36];
            }

            var queryFilter = new QueryFilterConfig();
            queryFilter.AddString("Name", newFieldInfo.FieldId);
            var newFieldName = await formConfigRepository.GetNextAvailableFieldNameAsync(queryFilter);

            if (newFieldInfo.TypeId == 149)
            {
                var logWriter = _serviceProvider.GetService<ILogWriter>();
                var contentTypeCount = string.IsNullOrWhiteSpace(newFieldInfo.ContentTypeIds) ? 0 : newFieldInfo.ContentTypeIds.Split(',').Length;
                logWriter?.LogInfo($"AddField: adding upload field TypeId=149, ContentTypeIds count={contentTypeCount}, form={idList[0]}, page={idList[1]}", "AddFieldEvent", "RunAsync");
            }

            // Persist using the enriched model; pass through original dataToSave for other consumers
            formToUpdate = await fieldConfigUtils.AddFieldToSystemAsync(newFieldName, newFieldInfo, formToUpdate, dataToSave, idList[1]);

            await formConfigRepository.UpdateFormAsync(formToUpdate);

            return 0;
        }

    }
}
