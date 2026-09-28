using arc.app.Common;
using arc.common;
using arc.common.Models;
using arc.domain.Configuration.EventsConfig;
using System.Threading.Tasks;

namespace arc.app.Instruments;

/// <summary>
/// Persists a manual instrument test request: validates context, ensures prerequisite rows, inserts pending <c>instrumentresults</c>.
/// </summary>
public class RequestInstrumentTestEvent : IRun
{
    private readonly IInstrumentManualRequestService _manualRequestService;
    private readonly TokenInfoModel _token;

    /// <summary>Creates the handler with explicit dependencies (no service locator).</summary>
    public RequestInstrumentTestEvent(IInstrumentManualRequestService manualRequestService, TokenInfoModel token)
    {
        _manualRequestService = manualRequestService;
        _token = token;
    }

    /// <inheritdoc />
    public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
    {
        var username = _token?.Username ?? string.Empty;
        return await _manualRequestService.RequestAsync(dataToSave, username);
    }
}
