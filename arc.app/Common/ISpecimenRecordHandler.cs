using arc.common.Models;
using arc.domain.Reports;
using System.Threading.Tasks;

namespace arc.app.Common
{
    public interface ISpecimenRecordHandler
    {
        Task<ReportData> HandleAsync(string specimenId, TokenInfoModel token);
        Task<ReportData> FilteredHandlerAsync(string specimenId, TokenInfoModel token, bool containsCultureFields, bool containsPatientFields, bool containsApprovalFields, bool containsDirectTests, bool containsCultureTests);
    }
}
