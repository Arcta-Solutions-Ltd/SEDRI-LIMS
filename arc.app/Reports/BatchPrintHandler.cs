using arc.app.Utils;
using arc.common.Models;
using arc.common.Models.Batch;
using arc.common.Utils;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Reports;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Reports;

public class BatchPrintHandler : IBatchPrintHandler
{
    private readonly ISpecimenRecordReportHandler _specimenRecordReportHandler;
    private readonly IReportHistoryRepository _reportHistoryRepository;
    private readonly IServiceProvider _serviceProvider;

    public BatchPrintHandler(ISpecimenRecordReportHandler specimenRecordReportHandler, IReportHistoryRepository reportHistoryRepository, IServiceProvider serviceProvider)
    {
        _reportHistoryRepository = reportHistoryRepository;
        _specimenRecordReportHandler = specimenRecordReportHandler;
        _serviceProvider = serviceProvider;
    }

    public async Task<string> HandleAsync(string contents, TokenInfoModel token)
    {
        var model = JsonConvert.DeserializeObject<PrintBatchModel>(contents);

        var reportList = new List<string>();

        foreach (var item in model.ItemsToPrint)
        {
            //var queryfilter = new QueryFilterConfig().AddString("id", item);
            //var specimenRecord = await _specimenRecordReportHandler.HandleAsync(queryfilter, token);

            //var newRecord = new ReportHistory
            //{
            //    SpecimenId = int.Parse(item),
            //    Name = DateTime.Now.ToString(),
            //    ReportId = model.ReportId,
            //    PrintStatusId = model.Type.Equals("print", StringComparison.CurrentCultureIgnoreCase) ? 871 : 869,
            //    ReportConfig = "DefaultSpecimenReport",
            //    Contents = specimenRecord
            //};

            //await _reportHistoryRepository.AddReportHistoryAsync(newRecord);

            string reportContents = model.History ? await GetHistoryReportAsync(item) : await GetSpecimenReportAsync(item, token, model);
            reportList.Add(reportContents);
        }

        return model.Type.Equals("print", StringComparison.CurrentCultureIgnoreCase) ? JsonConvert.SerializeObject(reportList) : "{}";
    }

    private async Task<string> GetSpecimenReportAsync(string id, TokenInfoModel token, PrintBatchModel model)
    {
        var queryfilter = new QueryFilterConfig().AddString("id", id);
        var specimenRecord = await _specimenRecordReportHandler.HandleAsync(queryfilter, token);

        var doReportsNeedApproval = await LaboratoryUtils.DoReportsForThisLaboratoryNeedApprovalAsync(_serviceProvider, token.LaboratoryId);

        var newRecord = new ReportHistory
        {
            SpecimenId = int.Parse(id),
            Name = DateTime.Now.ToString(),
            ReportId = model.ReportId,
            PrintStatusId = model.Type.Equals("print", StringComparison.CurrentCultureIgnoreCase) ? 871 : 869,
            ReportConfig = "DefaultSpecimenReport",
            ReportApprovalId = doReportsNeedApproval ? 122 : 123,
            Contents = specimenRecord
        };

        await _reportHistoryRepository.AddReportHistoryAsync(newRecord);
        return specimenRecord;
    }

    private async Task<string> GetHistoryReportAsync(string id)
    {
        var queryfilter = new QueryFilterConfig().AddString("id", id);
        var report =  await _reportHistoryRepository.GetReportContentsAsync(queryfilter);
        var formattedReportContents = ArcJson.Deserialize<ReportData>(report.Contents);
        return JsonConvert.SerializeObject(formattedReportContents);
    }

    //public async Task<string> HandleHistoryAsync(string contents, TokenInfoModel token)
    //{
    //    var model = JsonConvert.DeserializeObject<PrintBatchModel>(contents);

    //    var reportList = new List<string>();

    //    foreach (var item in model.ItemsToPrint)
    //    {
    //        var queryfilter = new QueryFilterConfig().AddString("id", item);
    //        var report = await _reportHistoryRepository.GetReportContentsAsync(queryfilter);
    //        reportList.Add(report.Contents);
    //    }

    //    return JsonConvert.SerializeObject(reportList);
    //}
}
