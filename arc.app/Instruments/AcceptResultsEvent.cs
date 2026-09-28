using arc.app.Common;
using arc.common.Models;
using arc.common;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using arc.data.model.Instruments;

namespace arc.app.Instruments;
/// <summary>
/// Represents the event that handles the acceptance of results for a specific instrument.
/// </summary>
internal class AcceptResultsEvent : IRun
{
    private readonly IServiceProvider _serviceProvider;
    private readonly TokenInfoModel _tokenInfo;

    /// <summary>
    /// Initializes a new instance of the <see cref="AcceptResultsEvent"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider for resolving dependencies.</param>
    /// <param name="tokenInfo">The token information for authentication.</param>
    public AcceptResultsEvent(IServiceProvider serviceProvider, TokenInfoModel tokenInfo)
    {
        _serviceProvider = serviceProvider;
        _tokenInfo = tokenInfo;
    }

    /// <summary>
    /// Executes the process to handle the acceptance of results and updates related records.
    /// </summary>
    /// <param name="dataToSave">The serialized data to be processed.</param>
    /// <param name="id">The identifier of the record being updated.</param>
    /// <param name="command">The event model containing event details.</param>
    /// <param name="eventData">Optional additional event configuration data.</param>
    /// <returns>A task that returns an integer status code upon completion.</returns>
    public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
    {
        var runEventHandler = _serviceProvider.GetService<IRunEventHandler>();
        var generalRepository = _serviceProvider.GetService<IGeneralRepository>();

        var resultMessage = await runEventHandler.RunAsync(dataToSave, _tokenInfo);

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

