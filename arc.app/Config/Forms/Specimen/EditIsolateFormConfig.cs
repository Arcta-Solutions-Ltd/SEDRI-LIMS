using arc.app.Common;

namespace arc.app.Config.Forms.Specimen;
/// <summary>
/// Definition for the Edit Isolate form configuration.
/// This form is launched by the UI event 'editisolateuievent' and persists via the specimen event 'editisolateevent'.
/// </summary>
internal class EditIsolateFormConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON configuration for the Edit Isolate form.
    /// - name: 'editisolateform' (referenced by the UI event action)
    /// - saveEvent: 'editisolateevent' (handled by SpecimenEventFactory)
    /// - initialquery: 'editisolatequery' (provides initial payload)
    /// - pages: ordered list of pages composing the form
    /// </summary>
    public string Get()
    {
        return """
            {
                "name": "editisolateform",
                "title": "@SpeEdiA@",
                "newitem": false,
                "saveEvent": "editisolateevent",
                "initialquery": "culturebyid",
                "startstate": "organismsearch",
                "configurable": "Yes",
                "suppressrecordview": true,
                "pages": [
                    "specimengrowthdetailsforisolatepage",
                    "cultureorganismpage",
                    "selectorganismpage",
                    "organismlistpage",
                    "specimenadditionalguidance",
                    "specimenotherinformationpage"
                ],
                "rules": [
                    {
                        "page": "selectorganismpage",
                        "state": "organismsearch",
                        "outcome": "visible"
                    },
                    {
                        "page": "selectorganismpage",
                        "state": "organismselect",
                        "outcome": "visible"
                    },
                    {
                        "page": "organismlistpage",
                        "state": "organismsearch",
                        "outcome": "visible"
                    },
                    {
                        "page": "organismlistpage",
                        "state": "organismselect",
                        "outcome": "visible"
                    },
                    {
                        "page": "cultureorganismpage",
                        "state": "organismselect",
                        "outcome": "visible"
                    }
                ]
            }
            """;
    }
}
