using arc.app.Common;
using arc.common.Models.Instruments;
using arc.data.model.Instruments;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Instruments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Instruments
{
    /// <summary>
    /// Repository interface containing all the methods which query or update instrument information.
    /// Author: Arcta Solutions Limited
    /// </summary>
    public interface IInstrumentRepository : IGeneralRepository
    {
        Task<List<InstrumentResultsListModel>> GetInstrumentResultsListAsync(QueryFilterConfig queryFilter);
        Task<List<InstrumentRequestModel>> GetNextRequestAsync(QueryFilterConfig queryFilter);
        Task<int> ConfirmRequestsAsync(RequestConfirmModel requestModel);
        Task<List<InstrumentResultsListModel>> GetCultureInstrumentResultsAsync(QueryFilterConfig queryFilter);
        Task<List<InstrumentResultsListModel>> GetSpecimenInstrumentResultsAsync(QueryFilterConfig queryFilter);
        /// <summary>
        /// Gets instrument result rows for the test record view: scoped to one direct test or one culture/isolate test via <c>instrumentresults.moredata</c>.
        /// </summary>
        /// <param name="queryFilter">Parameters: <c>id</c> (test row id), <c>source</c> (<c>direct</c> or <c>culture</c>).</param>
        Task<List<InstrumentResultsListModel>> GetTestInstrumentResultsAsync(QueryFilterConfig queryFilter);
        Task<int> AddAsync(InstrumentResult dataToSave);
        Task<int> UpdateAsync(InstrumentResult dataToSave);
        Task<int> EditAsync(string dataToSave);
        Task<SingleInstrumentConfig> GetInstrumentProfileByCultureIdAsync(int id);
        Task<SingleInstrumentConfig> GetInstrumentProfileBySpecimenIdAsync(int id);
        Task<SingleInstrumentConfig> GetInstrumentProfileByDirectTestNameAsync(int id, string name);
        Task<SingleInstrumentConfig> GetInstrumentProfileByCultureTestNameAsync(int id, string name);
        /// <summary>
        /// Resolves the <c>instrumentresults</c> row for an inbound payload: by id when set, otherwise composite match on accession, machine id, profile, and <c>moredata</c>.
        /// </summary>
        Task<InboundInstrumentMatchResult> ResolveInboundInstrumentResultAsync(
            int instrumentResultId,
            string profileName,
            string accessionNumber,
            string cultureNumber,
            int cultureIdFromPayload,
            int? instrumentMachineId,
            bool isDirectTestProfile,
            string resolvedDirectTestName);
        /// <summary>
        /// Denormalized accession from <c>specimen</c> for pending inserts.
        /// </summary>
        Task<string> GetAccessionNumberForSpecimenIdAsync(int specimenId);
        /// <summary>
        /// Denormalized culture number text from <c>culture</c> for pending inserts.
        /// </summary>
        Task<string> GetCultureNumberTextForCultureIdAsync(int cultureId);
        /// <summary>
        /// Loads one instrument result row for <c>instrumentresultrecordview</c> (parameter: <c>id</c> = instrument result id).
        /// </summary>
        Task<InstrumentResultRecordViewModel> GetInstrumentResultRecordViewByIdAsync(QueryFilterConfig queryFilter);
        Task<int> InstrumentCultureUpdateAsync(ResponseModel responseModel);
        /// <summary>
        /// Returns true if <paramref name="testName"/> matches <c>configs.configname</c> for at least one id or name in the comma-separated profile list.
        /// </summary>
        Task<bool> ProfileConfigIdsMatchTestNameAsync(string commaSeparatedProfileIds, string testName);
        /// <summary>
        /// Returns <c>Culture.SpecimenId</c> for the given culture.
        /// </summary>
        Task<int> GetSpecimenIdForCultureAsync(int cultureId);

        /// <summary>
        /// Returns <c>configs.configname</c> for the first id in a comma-separated config id list (profile DirectTestId / CultureTestId).
        /// </summary>
        Task<string> GetFirstConfigNameFromConfigIdListAsync(string commaSeparatedConfigIds);

        /// <summary>
        /// Appends link rows from <paramref name="fileAttachmentIds"/> to <c>instrumentresultfileattachments</c> for the given instrument result. Duplicate pairs are ignored.
        /// </summary>
        /// <param name="instrumentResultId">Primary key of <c>instrumentresults</c>.</param>
        /// <param name="fileAttachmentIds">File attachment ids from the standard upload API (positive integers only).</param>
        /// <returns>Number of new link rows inserted.</returns>
        Task<int> AddInstrumentResultFileAttachmentsAsync(int instrumentResultId, IEnumerable<int> fileAttachmentIds);

    }
}
