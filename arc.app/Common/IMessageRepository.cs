using arc.common.Models;
using arc.common.Models.Specimen;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Common
{
    public interface IMessageRepository
    {
        Task<int> AddAsync(MessageModel record);
        Task UpdateStateAsync(int id, int newStatus, string error, int recordId, SpecimenPatientModel specimenModel, int stateId, string tableName = null);
        Task<MessageModel> GetSingleAsync(int id);

        /// <summary>
        /// Retrieves the full queue hash chain as a dictionary of Id to Message (as stored text).
        /// Used for validation so the actual previous record in the chain can be looked up regardless of list filters.
        /// </summary>
        /// <returns>A dictionary mapping queue Id to Message::text, ordered by Id ascending.</returns>
        Task<Dictionary<int, string>> GetQueueChainAsync();
    }
}
