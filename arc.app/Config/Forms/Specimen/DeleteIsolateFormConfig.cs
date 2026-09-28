using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Form configuration definition for deleting an isolate.
/// Opens the 'deleteisolateform' flow and posts to the 'deleteisolate' event.
/// </summary>
internal class DeleteIsolateFormConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON configuration for the Delete Isolate form.
    /// - name: 'deleteisolateform'
    /// - saveEvent: 'deleteisolate'
    /// - initialquery: 'deleteisolatequery'
    /// - pages: [ 'deletedirecttestpage' ] (example page)
    /// </summary>
    public string Get()
    {
        return """
            {
                "name": "deleteisolateform",
                "viewTitle": "Delete isolate",
                "saveEvent": "deleteisolateevent",
                "initialquery": "cultureviewdetailsquery",
                "suppressRecordView": true,
                "pages": [
                    "deleteisolatepage"
                ]
            }
            """;
    }
}
