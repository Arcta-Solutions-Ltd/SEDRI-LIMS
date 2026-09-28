using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Provides the configuration definition for the 'addisolateform',
/// which orchestrates the UI flow for adding a culture isolate.
/// </summary>
internal class AddIsolateFormConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON string representing the form configuration for adding an isolate.
    /// </summary>
    /// <returns>A JSON string containing form metadata, page sequence, rules, and behavior settings.</returns>
    public string Get()
    {
        return """
            {
                "name": "addisolateform",
                "title": "@SpeAddD@",
                "newitem": true,
                "saveevent": "addisolateevent",
                "initialquery": "culturebyidforisolatequery",
                "recordview": "specimenrecordview",
                "startstate": "organismsearch",
                "configurable": "Yes",
                "singleitemname": "culture",
                "suppressrecordview": false,
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
                ],
                "configureactions": [
                    "edit"
                ]
            }
            """;
    }
}

