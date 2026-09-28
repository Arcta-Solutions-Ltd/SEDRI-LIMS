using arc.common;
using arc.domain.Configuration.EventsConfig;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using arc.app.Common;
using Newtonsoft.Json;
using arc.common.Models.Common;
using arc.app.SystemConfig;

namespace arc.app.Configuration.Events
{
    internal class EditDirectTestEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;


        public EditDirectTestEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Updates the direct test title and syncs the report section Description to match.
        /// HeadingText is preserved so a deliberately cleared section heading is not reset to the test name.
        /// </summary>
        /// <param name="dataToSave">Serialized title model from the edit form.</param>
        /// <param name="id">Composite event id.</param>
        /// <param name="command">The event model.</param>
        /// <param name="eventData">Optional event configuration.</param>
        /// <returns>Zero on success.</returns>
        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var record = JsonConvert.DeserializeObject<TitleModel>(dataToSave);

            var idList = record.Id.Split("|");

            var formConfigDefinition = _serviceProvider.GetService<IFormConfigDefinition>();
            var formConfigRepository = _serviceProvider.GetService<IFormConfigRepository>();
            var logWriter = _serviceProvider.GetService<ILogWriter>();

            var formToChange = await formConfigDefinition.LoadFormAsync(idList[1]);
            formToChange.Title = record.Title;
            formToChange.SaveEventConfig.Description = record.Title;

            foreach (var section in formToChange.ReportSectionConfigList)
            {
                section.Description = record.Title;
                logWriter?.LogInfo(
                    $"Direct test title updated; Description synced, HeadingText preserved for section {section.Name}",
                    nameof(EditDirectTestEvent), nameof(RunAsync));
            }

            await formConfigRepository.UpdateFormAsync(formToChange);

            return 0;
        }
    }
}
