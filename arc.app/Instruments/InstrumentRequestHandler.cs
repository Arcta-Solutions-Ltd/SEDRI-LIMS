using arc.common.Models.Instruments;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Instruments
{
    /// <summary>
    /// Handles all the instrument communication.
    /// </summary>
    /// <param name="instrumentRepository">Repository which makes changes/gets data to/from the database related to instrument information</param>
    /// <param name="instrumentErrorRepository">Repository which makes changes/gets data to/from the database related to instrument errors</param>
    public class InstrumentRequestHandler : IInstrumentRequestHandler
    {
        private readonly IInstrumentRepository _instrumentRepository;
        private readonly IInstrumentErrorRepository _instrumentErrorRepository;

        /// <summary>
        /// Initializes a new instance of the InstrumentRequestHandler class.
        /// </summary>
        /// <param name="instrumentRepository">The instrument repository.</param>
        /// <param name="instrumentErrorRepository">The instrument error repository.</param>
        public InstrumentRequestHandler(IInstrumentRepository instrumentRepository, IInstrumentErrorRepository instrumentErrorRepository)
        {
            _instrumentRepository = instrumentRepository;
            _instrumentErrorRepository = instrumentErrorRepository;
        }

        /// <summary>
        /// Retrieves the next instrument requests based on the provided profiles.
        /// </summary>
        /// <param name="profiles">The instrument profile model containing the profiles.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains a list of InstrumentRequestModel objects.</returns>
        public async Task<List<InstrumentRequestModel>> GetNextRequest(InstrumentProfileModel profiles)
        {
            var returnList = new List<InstrumentRequestModel>();

            foreach (var profile in profiles.Profiles)
            {
                var queryFilter = new QueryFilterConfig();
                queryFilter.AddString("Value", profile);
                var requestList = await _instrumentRepository.GetNextRequestAsync(queryFilter);
                returnList.AddRange(requestList);
            }

            return returnList;
        }

        /// <summary>
        /// Confirms outbound processing for a pending instrument result: updates status to Requested (883) and optionally links
        /// <see cref="RequestConfirmModel.SourceFileAttachmentIds"/> (from <c>POST api/file/upload</c>) to that <c>instrumentresults</c> row.
        /// </summary>
        /// <param name="requestModel">The request confirm model (id, instrument name, optional file attachment ids).</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task ConfirmRequests(RequestConfirmModel requestModel)
        {
            await _instrumentRepository.ConfirmRequestsAsync(requestModel);
        }

        /// <summary>
        /// Saves an instrument error asynchronously.
        /// </summary>
        /// <param name="error">The instrument error model.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task SaveErrorAsync(InstrumentErrorModel error)
        {
            await _instrumentErrorRepository.AddAsync(error);
        }
    }

    /// <summary>
    /// Model for instrument profiles.
    /// </summary>
    public class InstrumentProfileModel
    {
        /// <summary>
        /// Gets or sets the list of profiles.
        /// </summary>
        public List<string> Profiles { get; set; }
    }
}
