using arc.common.Models.Specimen;
using arc.domain.Configuration.EventsConfig;
using System.Threading.Tasks;

namespace arc.app.Common
{
    public interface IMessageQueue
    {
        Task<int> AddAsync(string message, string userName, EventConfig eventData);
        Task UpdateStatusAsync(int id, int newStatus, int recordid, string table = "", int stateId = 0, string error = "", string eventName = "", SpecimenPatientModel data = null);
        Task<SpecimenPatientModel> GetSpecimenPatientDetailsAsync(string table, string eventName, int recordId);
        Task<SpecimenPatientModel> GetSpecimenPatientDetailsForWorkflowAsync(EventConfig eventDetails, int recordId);
    }
}
