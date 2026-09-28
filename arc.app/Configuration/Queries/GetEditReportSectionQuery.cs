using arc.app.Config.Reports;
using arc.domain.Configuration.QueryConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Configuration.Queries
{
    internal class GetEditReportSectionQuery : ISingleConfig
    {
        private readonly IServiceProvider _serviceProvider;

        internal GetEditReportSectionQuery(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<string> GetAsync(QueryFilterConfig queryFilters, QueryConfig queryData)
        {
            var reportAdapter = _serviceProvider.GetService<IReportAdapter>();
            var sectionAdapter = _serviceProvider.GetService<ISectionAdapter>();

            var id = queryFilters.Parameters.Where(p => p.Key.ToLower() == "id").First().Value;
            var idList = id.Split("|");
            var reportName = idList[0];
            var reportSection = idList[1];
            var report = await reportAdapter.GetReportAsync(reportName);

            var sections = reportSection == "top" ? report.MainSections : reportSection == "bottom" ? report.FinalSections : report.OrganismSections;
            var sectionList = await GetSectionDescriptions(sections, sectionAdapter, reportName);
            var selectorValues = sectionList.Select(s => new { id = s.Section, label = s.Section, value = s.Id });

            var result = new { Id = reportName, SectionList = selectorValues };

            return JsonConvert.SerializeObject(result);
        }

        private async Task<List<ReportSectionModel>> GetSectionDescriptions(List<string> sections, ISectionAdapter adapter, string report)
        {

            var newSections = new List<ReportSectionModel>();
            foreach (var section in sections)
            {
                var fullSection = await adapter.GetSectionAsync(section);
                newSections.Add(new ReportSectionModel { Id = report + "|" + section, Section = fullSection.Description });
            }

            return newSections;
        }
    }

    internal class ReportSectionModel
    {
        internal string Id { get; set; }
        internal string Section { get; set; }
    }
}
