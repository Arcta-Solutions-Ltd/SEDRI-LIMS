using arc.common.Models.Reports;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Reports;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Reports;

public interface IReportHistoryRepository
{
    Task<int> AddReportHistoryAsync(ReportHistory dataToSave);
    Task<ReportHistory> GetReportContentsAsync(QueryFilterConfig queryFilters);
    Task<List<PatientReportListModel>> GetPatientReportListAsync(QueryFilterConfig queryFilters);
    Task<List<PatientReportListModel>> GetAdmissionReportListAsync(QueryFilterConfig queryFilters);
    Task<List<PatientReportListModel>> GetRequestReportListAsync(QueryFilterConfig queryFilters);
}
