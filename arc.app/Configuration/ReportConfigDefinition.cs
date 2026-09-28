using arc.app.Config.Reports;
using arc.app.SystemConfig;
using arc.common.Utils;
using arc.domain.Configuration.FormStructureConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Configuration.ReportsConfig;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Configuration
{
    public class ReportConfigDefinition : IReportConfigDefinition
    {
        private IReportAdapter _reportAdapter;
        private ISectionAdapter _sectionAdapter;
        private IConfigRepository _configRepository;
        private readonly ICopyProperties _copyProperties;

        public ReportConfigDefinition(IReportAdapter reportAdapter, IConfigRepository configRepository, ICopyProperties copyProperties, ISectionAdapter sectionAdapter)
        {
            _reportAdapter = reportAdapter;
            _configRepository = configRepository;
            _copyProperties = copyProperties;
            _sectionAdapter = sectionAdapter;
        }

        public async Task<FullReportConfig> LoadReportAsync(string reportName)
        {
            var report = await _reportAdapter.GetReportAsync(reportName);
            var reportDef = new FullReportConfig();
            _copyProperties.CopyAll(report, reportDef);

            reportDef.MainSectionsConfig = await LoadSectionDetailsAsync(report.MainSections);
            reportDef.OrganismSectionsConfig = await LoadSectionDetailsAsync(report.OrganismSections);
            reportDef.FinalSectionsConfig = await LoadSectionDetailsAsync(report.FinalSections);

            return reportDef;
        }

        public async Task<FullReportConfig> ResetNamesForNewReportAsync(FullReportConfig reportToCopy, string reportName)
        {
            reportToCopy.Name = reportName.ToLower() + "report";
            reportToCopy.MainSectionsConfig = await RenameSectionsAsync(reportToCopy.MainSectionsConfig.ToList());
            reportToCopy.OrganismSectionsConfig = await RenameSectionsAsync(reportToCopy.OrganismSectionsConfig.ToList());
            reportToCopy.FinalSectionsConfig = await RenameSectionsAsync(reportToCopy.FinalSectionsConfig.ToList());

            reportToCopy.MainSections = CreateSectionList(reportToCopy.MainSectionsConfig.ToList());
            reportToCopy.OrganismSections = CreateSectionList(reportToCopy.OrganismSectionsConfig.ToList());
            reportToCopy.FinalSections = CreateSectionList(reportToCopy.FinalSectionsConfig.ToList());
            return reportToCopy;
        }

        private async Task<List<ReportSectionConfig>> LoadSectionDetailsAsync(List<string> sections)
        {
            var sectionList = new List<ReportSectionConfig>();
            foreach (var section in sections)
            {
                var fullSection = await _sectionAdapter.GetSectionAsync(section);
                sectionList.Add(fullSection);
            }
            return sectionList;
        }

        private List<string> CreateSectionList(List<ReportSectionConfig> sections) 
        {
            var sectionList = new List<string>();
            foreach (var section in sections)
            {
                sectionList.Add(section.Name);
            }
            return sectionList;
        }

        private async Task<List<ReportSectionConfig>> RenameSectionsAsync(List<ReportSectionConfig> sections)
        {
            var newSections = new List<ReportSectionConfig>();
            foreach (var section in sections)
            {
                var queryFilter = new QueryFilterConfig("Name", section.Name, "Suffix", "");
                section.Name = await _configRepository.GetNextAvailableNameAsync(queryFilter);
                section.Name = section.Name.Trim();
                newSections.Add(section);
            }
            return newSections;
        }
    }
}
