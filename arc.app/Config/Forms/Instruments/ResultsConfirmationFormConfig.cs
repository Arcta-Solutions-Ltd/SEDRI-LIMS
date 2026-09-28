using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Configuration class for the results confirmation form.
/// </summary>
internal class ResultsConfirmationFormConfig : IDefinition
{
    /// <summary>
    /// Generates the JSON string for the results confirmation form.
    /// </summary>
    /// <returns>A JSON string that represents the form configuration.</returns>
    public string Get()
    {
        var form = @"{
                            name: 'resultsconfirmationform',
                            viewTitle: '@InsAcc@',
                            saveevent: 'acceptinstrumentresults',
                            recordView: 'specimenrecordview',
                            initialquery: 'InstrumentResultByIdQuery',
                            suppressRecordView: true,
                            pages: ['resultsconfirmation']
                        }";

        return form;
    }
}
