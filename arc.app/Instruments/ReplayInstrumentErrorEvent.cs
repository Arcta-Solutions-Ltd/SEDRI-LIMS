using arc.app.Common;
using arc.common;
using arc.common.Models;
using arc.common.Models.Instruments;
using arc.common.Utils;
using arc.data.model.Instruments;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace arc.app.Instruments;

/// <summary>
/// Represents an event to replay instrument errors.
/// </summary>
internal class ReplayInstrumentErrorEvent : IRun
{
    private readonly IServiceProvider _serviceProvider;
    private readonly TokenInfoModel _token;

    /// <summary>
    /// Initializes a new instance of the <see cref="ReplayInstrumentErrorEvent"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider.</param>
    /// <param name="token">The token information model.</param>
    public ReplayInstrumentErrorEvent(IServiceProvider serviceProvider, TokenInfoModel token)
    {
        _serviceProvider = serviceProvider;
        _token = token;
    }

    /// <summary>
    /// Asynchronously runs the replay instrument error event.
    /// </summary>
    /// <param name="dataToSave">The data to save.</param>
    /// <param name="id">The identifier.</param>
    /// <param name="command">The event model command.</param>
    /// <param name="eventData">The optional event configuration data.</param>
    /// <returns>A task representing the asynchronous operation, with an integer result.</returns>
    public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
    {
        var runEventHandler = _serviceProvider.GetService<IRunEventHandler>();
        var generalRepository = _serviceProvider.GetService<IGeneralRepository>();

        var errorData = ArcJson.Deserialize<InstrumentErrorModel>(dataToSave);

        var resultMessage = await runEventHandler.RunAsync(errorData.Message, _token);

        var errorRecord = new InstrumentErrorsDataModel
        {
            Id = int.Parse(id),
            ErrorText = "@InsUpdA@",
            ErrorStatusId = string.IsNullOrEmpty(resultMessage) ? 12 : 11
        };

        await generalRepository.UpdateAsync(errorRecord, "instrumenterrors", "id");
        return 0;
    }
}

