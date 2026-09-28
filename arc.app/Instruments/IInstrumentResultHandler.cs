using arc.common.Models;
using arc.common.Models.Instruments;
using System.Threading.Tasks;

namespace arc.app.Instruments
{
    /// <summary>
    /// Interface for the result handler which handles all results that come through the Instrument Controller and are therefore sent from an external system.
    /// Author: Arcta Solutions Limited
    /// </summary>
    public interface IInstrumentResultHandler
    {
        /// <summary>
        /// Handles all results that come through the Instrument Controller and are therefore sent from an external system.
        /// Author: Arcta Solutions Limited
        /// </summary>
        /// <param name="resultContents">Contents of one record which has been sent from an external interface in JSON string format. The JSON string must in a structure which corresponds to the ResponseModel class.</param>
        /// <param name="token">The token information model.</param>
        Task<bool> HandleAsync(ResponseModel responseModel, TokenInfoModel token);
    }
}
