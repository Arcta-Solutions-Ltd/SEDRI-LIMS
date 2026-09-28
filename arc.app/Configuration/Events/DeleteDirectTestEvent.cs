using arc.app.Common;
using arc.common;
using arc.domain.Configuration.EventsConfig;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using arc.app.SystemConfig;

namespace arc.app.Configuration.Events
{
    internal class DeleteDirectTestEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;


        public DeleteDirectTestEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var record = JsonConvert.DeserializeObject<IdModel>(dataToSave);

            var idList = record.Id.Split("|");

            //Load form definition
            var formConfigDefinition = _serviceProvider.GetService<IFormConfigDefinition>();
            var formToDelete = await formConfigDefinition.LoadFormAsync(idList[1]);

            //For each report remove the section.
            if (formToDelete.ReportSectionConfigList.Count > 0)
            {
                foreach(var section in formToDelete.ReportSectionConfigList)
                {
                    for (var x = 0; x < formToDelete.ReportConfigList.Count; x++)
                    {
                        formToDelete.ReportConfigList[x].DeleteSection(section.Name.ToLower());
                    }
                }
            }

            //Delete the form from the database
            var formConfigRepository = _serviceProvider.GetService<IFormConfigRepository>();
            await formConfigRepository.DeleteFormAsync(formToDelete);

            //Update the view
            var configExtractionUtils = _serviceProvider.GetService<IConfigExtractionUtils>();
            var view = await configExtractionUtils.GetViewConfigUsingIdAsync(int.Parse(idList[0]));

            view.RemoveTest(formToDelete.UIEvent);
            await configExtractionUtils.SaveViewAsync(view);

            return 0;
        }
    }
}
