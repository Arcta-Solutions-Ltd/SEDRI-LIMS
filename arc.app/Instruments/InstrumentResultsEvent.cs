using arc.app.Common;
using arc.common;
using arc.common.Models;
using arc.common.Models.Instruments;
using arc.common.Utils;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace arc.app.Instruments;

/// <summary>
/// Special event for inbound <c>InstrumentResults</c>: selects AST vs direct-test processor from the instrument profile <c>InterfaceTypeId</c>.
/// </summary>
public class InstrumentResultsEvent : IRun
{
    private readonly IServiceProvider _serviceProvider;
    private readonly TokenInfoModel _token;

    /// <summary>
    /// Creates an instance that can forward <paramref name="token"/> to processors that save via <see cref="IRunEventHandler"/> (direct test path).
    /// </summary>
    public InstrumentResultsEvent(IServiceProvider serviceProvider, TokenInfoModel token)
    {
        _serviceProvider = serviceProvider;
        _token = token;
    }

    /// <inheritdoc />
    public async Task<int> RunAsync(string dataToSave, string Id, EventModel command, EventConfig eventData = null)
    {
        var instrumentFactory = _serviceProvider.GetRequiredService<IInstrumentFactory>();
        var singleInstrumentProfile = _serviceProvider.GetRequiredService<ISingleInstrumentProfile>();
        var responseModel = ArcJson.Deserialize<ResponseModel>(dataToSave);

        var profile = await singleInstrumentProfile.GetAsync(responseModel.ProfileName);
        var factoryKey = InstrumentProcessorTypeResolver.ResolveFactoryKey(profile?.InterfaceTypeId);
        var processor = instrumentFactory.Get(factoryKey) ?? instrumentFactory.Get(InstrumentProcessorTypeResolver.FactoryKeyAst);
        if (processor == null)
            throw new InvalidOperationException($"No instrument processor registered for key '{factoryKey}' or '{InstrumentProcessorTypeResolver.FactoryKeyAst}'.");

        await processor.Run(responseModel, _token);
        return 0;
    }
}
