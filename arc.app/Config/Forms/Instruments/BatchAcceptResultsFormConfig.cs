using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Represents the configuration for the Batch Accept Results form.
/// </summary>
internal class BatchAcceptResultsFormConfig : IDefinition
{
    /// <summary>
    /// Builds and returns a JSON string defining the Batch Accept Results form configuration.
    /// </summary>
    /// <returns>A JSON-formatted string describing the form configuration.</returns>
    public string Get()
    {
        var form = @"{
                            name: 'batchacceptresultsform',
                            viewTitle: '@InsBatD@',
                            saveevent: 'batchacceptresultsevent',
                            recordView: 'specimenrecordview',
                            suppressRecordView: true,
                            pages: ['batchacceptresultspage']
                        }";

        return form;
    }
}

