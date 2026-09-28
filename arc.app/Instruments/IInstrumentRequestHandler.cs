using arc.common.Models.Instruments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Instruments
{
    public interface IInstrumentRequestHandler
    {
        Task<List<InstrumentRequestModel>> GetNextRequest(InstrumentProfileModel profiles);
        Task ConfirmRequests(RequestConfirmModel requestModel);
        Task SaveErrorAsync(InstrumentErrorModel error);
    }
}
