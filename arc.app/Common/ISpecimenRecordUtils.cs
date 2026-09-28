using arc.common.Models;
using arc.domain.Reports;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Common
{
    public interface ISpecimenRecordUtils
    {
        Task<List<KeyValueModel>> GetPatientDetailsAsync(int patientId, TokenInfoModel token);
        Task<List<KeyValueModel>> GetSpecimenDetailsAsync(string specimenId);
        Task<List<KeyValueModel>> GetApproverDetailsAsync(string specimenId);
        Task<List<TableRow>> GetAlertDetailsAsync(string specimenId);
        Task<ReportData> GetDirectTestsAsync(string specimenId, ReportData returnValue, TokenInfoModel token);
        Task<GroupRow> GetOrganismDataAsync(string specimenId, TokenInfoModel token);
        List<string> GetListsUsedInReport();
    }
}
