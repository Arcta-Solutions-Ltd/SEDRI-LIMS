using arc.app.Common;
using arc.app.Config.Reports;
using arc.domain.Configuration.QueryConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Configuration.Queries
{
    internal class GetReportDefinition : ISingleConfig
    {
        private readonly IServiceProvider _serviceProvider;

        internal GetReportDefinition(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<string> GetAsync(QueryFilterConfig queryFilters, QueryConfig queryData)
        {
            var logWriter = _serviceProvider.GetService<ILogWriter>();

            try
            {
                var reportAdapter = _serviceProvider.GetService<IReportAdapter>();
                var sectionAdapter = _serviceProvider.GetService<ISectionAdapter>();

                var idList = queryFilters.Parameters[0].Value.Split("|");
                var report = await reportAdapter.GetReportAsync(idList[1]);

                var returnObject = new
                {
                    Name = report.Name,
                    Description = report.Title,
                    Header = report.Header,
                    IncludeAlerts = report.IncludeAlerts,
                    MainSections = await GetSectionDescriptions(report.MainSections, sectionAdapter, report.Name),
                    OrganismSections = await GetSectionDescriptions(report.OrganismSections, sectionAdapter, report.Name),
                    FinalSections = await GetSectionDescriptions(report.FinalSections, sectionAdapter, report.Name)
                };

                return JsonConvert.SerializeObject(returnObject);
            }
            catch (Exception e)
            {
                logWriter.LogError(e.Message, "GetReportDefinition", "Get");
            }

            return "";
        }

        private async Task<List<object>> GetSectionDescriptions(List<string> sections, ISectionAdapter adapter, string report) {

            var newSections = new List<object>();
            foreach (var section in sections)
            {
                var fullSection = await adapter.GetSectionAsync(section);
                newSections.Add(new { id = report + "|" + section, section = fullSection.Description });
            }

            return newSections;
        }
    }
}


