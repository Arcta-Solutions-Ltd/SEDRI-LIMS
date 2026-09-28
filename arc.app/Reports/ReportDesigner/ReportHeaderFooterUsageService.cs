using arc.app.Common;
using arc.app.Config.Reports;
using arc.common.ExtensionMethods;
using arc.common.Models.Reports.ReportDesigner;
using arc.domain.Configuration.ReportsConfig;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Reports.ReportDesigner;

/// <summary>
/// Computes how many reports reference each header or footer name for save-scope prompting.
/// </summary>
public class ReportHeaderFooterUsageService(
    IReportAdapter reportAdapter,
    IReportFactory reportFactory,
    ILogWriter logWriter) : IReportHeaderFooterUsageService
{
    private static readonly string[] CodeDefinedReportNames = ["defaultspecimenreport", "specimenrecord"];

    /// <summary>
    /// Sets linked-report counts on header and footer section definitions.
    /// </summary>
    /// <param name="model">The designer model to update.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task PopulateLinkedReportCountsAsync(ReportDesignerModel model)
    {
        if (model == null)
        {
            return;
        }

        var headerCounts = await BuildReferenceCountsAsync(report => report?.Header);
        var footerCounts = await BuildReferenceCountsAsync(report => report?.Footer);

        ApplyCounts(model.HeaderSectionDefinitions, headerCounts);
        ApplyCounts(model.FooterSectionDefinitions, footerCounts);

        logWriter.LogInfo(
            $"Linked report counts populated for {model.HeaderSectionDefinitions?.Count ?? 0} header(s) and {model.FooterSectionDefinitions?.Count ?? 0} footer(s) on report '{model.Name}'.",
            nameof(ReportHeaderFooterUsageService),
            nameof(PopulateLinkedReportCountsAsync));
    }

    /// <summary>
    /// Builds a map of normalised section name to the number of reports referencing it.
    /// </summary>
    /// <param name="selector">Selects the header or footer name from a report config.</param>
    /// <returns>Reference counts keyed by normalised section name.</returns>
    private async Task<Dictionary<string, int>> BuildReferenceCountsAsync(
        System.Func<ReportConfig, string> selector)
    {
        var counts = new Dictionary<string, int>(System.StringComparer.Ordinal);

        foreach (var report in await ResolveAllReportsAsync())
        {
            var sectionName = selector(report)?.NormalisedConfigName();
            if (string.IsNullOrEmpty(sectionName))
            {
                continue;
            }

            counts.TryGetValue(sectionName, out var current);
            counts[sectionName] = current + 1;
        }

        return counts;
    }

    /// <summary>
    /// Resolves every report config that should participate in reference counting.
    /// </summary>
    /// <returns>All report configs from the database plus code-defined defaults not already represented.</returns>
    private async Task<List<ReportConfig>> ResolveAllReportsAsync()
    {
        var reports = await reportAdapter.GetAllReportsAsync() ?? [];
        var knownNames = new HashSet<string>(
            reports.Select(report => report?.Name?.NormalisedConfigName()).Where(name => !string.IsNullOrEmpty(name)),
            System.StringComparer.Ordinal);

        foreach (var reportName in CodeDefinedReportNames)
        {
            if (knownNames.Contains(reportName))
            {
                continue;
            }

            try
            {
                var contents = reportFactory.GetReport(reportName).Get();
                var report = JsonConvert.DeserializeObject<ReportConfig>(contents);
                if (report != null)
                {
                    report.Name ??= reportName;
                    reports.Add(report);
                }
            }
            catch (System.Exception ex)
            {
                logWriter.LogWarning(
                    $"Could not load code-defined report '{reportName}' for header/footer usage counting: {ex.Message}",
                    nameof(ReportHeaderFooterUsageService),
                    nameof(ResolveAllReportsAsync));
            }
        }

        return reports;
    }

    /// <summary>
    /// Applies reference counts to a list of header or footer section definitions.
    /// </summary>
    /// <param name="sections">The section definitions to update.</param>
    /// <param name="counts">Reference counts keyed by normalised section name.</param>
    private static void ApplyCounts(List<ReportSectionModel> sections, Dictionary<string, int> counts)
    {
        if (sections == null || counts == null)
        {
            return;
        }

        foreach (var section in sections)
        {
            if (section == null)
            {
                continue;
            }

            var key = section.Name.NormalisedConfigName();
            section.LinkedReportCount = counts.TryGetValue(key, out var count) ? count : 0;
        }
    }
}
