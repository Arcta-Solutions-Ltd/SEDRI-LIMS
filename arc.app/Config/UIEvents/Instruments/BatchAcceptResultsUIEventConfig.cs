using arc.app.Common;

namespace arc.app.Config.UIEvents;
internal class BatchAcceptResultsUIEventConfig : IDefinition
{
    /// <summary>
    /// Generates the JSON string for the batch accept results UI event.
    /// </summary>
    /// <returns>A JSON string that represents the event configuration.</returns>
    public string Get()
    {
        var newEvent = @"{
                        name: 'batchacceptresultsuievent',
                        description: '@InsBatJ@',
                        type: 'form',
                        action: 'batchacceptresultsform'
                    }";

        return newEvent;
    }
}
