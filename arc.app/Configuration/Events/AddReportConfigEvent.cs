using arc.app.Common;
using arc.app.SystemConfig;
using arc.common;
using arc.common.Models.Config;
using arc.domain.Configuration.EventsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace arc.app.Configuration.Events
{
    internal class AddReportConfigEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;

        public AddReportConfigEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var configRepository = _serviceProvider.GetService<IConfigRepository>();
            var reportConfigDefinition = _serviceProvider.GetService<IReportConfigDefinition>();
            var reportConfigRepository = _serviceProvider.GetService<IReportConfigRepository>();
            var configExtractionUtils = _serviceProvider.GetService<IConfigExtractionUtils>();

            var newReport = JsonConvert.DeserializeObject<UpdateReportConfigModel>(dataToSave);
            var configName = newReport.Title.Replace(" ", "");

            //Get next available name
            var queryFilter = new QueryFilterConfig("Name", configName, "Suffix", "report");
            configName = await configRepository.GetNextAvailableNameAsync(queryFilter);
            configName = configName.Trim();

            //Get the report to clone
            var reportToCopy = await reportConfigDefinition.LoadReportAsync(newReport.ReportToCloneId);
            reportToCopy.Title = newReport.Title;
            reportToCopy.Configurable = "Yes";

            //Rename report so that it is a new report
            reportToCopy = await reportConfigDefinition.ResetNamesForNewReportAsync(reportToCopy, configName);

            //Need to add in the change to the view here.
            if (newReport.Enabled == "Yes")
            {
                var view = await configExtractionUtils.GetViewConfigUsingIdAsync(int.Parse(newReport.Id));
                view.AddReport(configName.ToLower() + "report");
                await configExtractionUtils.SaveViewAsync(view);
            }

            //Save report to the database
            await reportConfigRepository.AddNewReportAsync(reportToCopy);

            return 0;
        }
    }
}



