using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Configuration class for the results rejection form.
/// </summary>
internal class ResultsRejectionFormConfig : IDefinition
{
    /// <summary>
    /// Generates the JSON string for the results rejection form.
    /// </summary>
    /// <returns>A JSON string that represents the form configuration.</returns>
    public string Get()
    {
        var form = @"{
                            name: 'resultsrejectionform',
                            viewTitle: '@InsRej@',
                            saveevent: 'deleteinstrumentresults',
                            initialquery: 'InstrumentResultByIdQuery',
                            recordView: 'specimenrecordview',
                            suppressRecordView: true,
                            pages: ['resultsrejection']
                        }";

        return form;
    }
}
