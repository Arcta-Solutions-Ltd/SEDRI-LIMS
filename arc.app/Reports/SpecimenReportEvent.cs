using arc.app.Common;
using arc.app.Security;
using arc.app.Specimen;
using arc.app.Utils;
using arc.common;
using arc.common.Models;
using arc.common.Utils;
using arc.domain.Configuration.EventsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Reports;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Threading.Tasks;

namespace arc.app.Reports;

/// <summary>
/// Handles the execution of a specimen report event, parsing input data and saving report history.
/// </summary>
internal class SpecimenReportEvent : IRun
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="SpecimenReportEvent"/> class.
    /// </summary>
    /// <param name="serviceProvider">Service provider used for dependency resolution.</param>
    public SpecimenReportEvent(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Parses the provided JSON data and saves a new report history entry.
    /// </summary>
    /// <param name="dataToSave">Raw JSON string containing report data.</param>
    /// <param name="id">Identifier for the event (not used in this implementation).</param>
    /// <param name="command">Event model containing metadata (not used in this implementation).</param>
    /// <param name="eventData">Optional event configuration (not used in this implementation).</param>
    /// <returns>Returns the ID of the newly created report history record.</returns>
    public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
    {
        JToken json;
        using (var sr = new StringReader(dataToSave))
        using (var jr = new JsonTextReader(sr) { DateParseHandling = DateParseHandling.None })
        {
            json = JToken.ReadFrom(jr);
        }

        var fieldsCollector = new JsonFieldsCollector("", "Contents");
        var fieldList = fieldsCollector.GetAllFields(json);

        var specimenId = int.Parse(fieldList[1].Value);
        var specimenRepository = _serviceProvider.GetService<ISpecimenRepository>();
        var queryFilter = new QueryFilterConfig().AddInteger("id", specimenId);
        var specimenRecord = await specimenRepository.SpecimenByIdAsync(queryFilter);

        var doReportsNeedApproval = await LaboratoryUtils.DoReportsForThisLaboratoryNeedApprovalAsync(_serviceProvider, specimenRecord.LaboratoryId);

        //var laboratoryRepository = _serviceProvider.GetService<ILaboratoryRepository>();
        //var labQueryFilter = new QueryFilterConfig().AddInteger("id", specimenRecord.LaboratoryId);
        //var laboratoryRecord = await laboratoryRepository.LaboratoryByIdAsync(labQueryFilter);

        var reportHistory = new ReportHistory
        {
            SpecimenId = specimenId,
            ReportId = 868,
            Name = fieldList[2].Value,
            Contents = fieldList[3].Value,
            ReportConfig = fieldList[4].Value,
            ReportApprovalId = doReportsNeedApproval ? 122 : 123
        };

        var reportHistoryRepository = _serviceProvider.GetService<IReportHistoryRepository>();
        return await reportHistoryRepository.AddReportHistoryAsync(reportHistory);
    }
}
