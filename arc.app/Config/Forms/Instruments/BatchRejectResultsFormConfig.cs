using arc.app.Common;

namespace arc.app.Config.Forms;
/// <summary>
/// Represents the configuration for the Batch Reject Results form.
/// </summary>
internal class BatchRejectResultsFormConfig : IDefinition
{
    /// <summary>
    /// Builds and returns a JSON string defining the Batch Reject Results form configuration.
    /// </summary>
    /// <returns>A JSON-formatted string describing the form configuration.</returns>
    public string Get()
    {
        var form = @"{
                            name: 'batchrejectresultsform',
                            viewTitle: '@InsBatE@',
                            saveevent: 'batchrejectresultsevent',
                            recordView: 'specimenrecordview',
                            suppressRecordView: true,
                            pages: ['batchrejectresultspage']
                        }";

        return form;
    }
}

