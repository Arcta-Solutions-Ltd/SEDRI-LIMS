using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Defines the configuration for the culture deletion form.
/// </summary>
/// <remarks>
/// This form is named <c>deletecultureform</c> and is designed to handle culture deletion workflows.
/// It initializes with the <c>cultureviewdetailsquery</c>, suppresses the record view,
/// and triggers the <c>deleteculture</c> event upon save. The form includes a single page: <c>deleteculturepage</c>.
/// </remarks>
internal class DeleteCultureFormConfig : IDefinition
{
    /// <summary>
    /// Returns the form configuration as a JSON-formatted string.
    /// </summary>
    /// <returns>
    /// A JSON string containing metadata for the <c>deletecultureform</c>,
    /// including title, event bindings, query initialization, and page layout.
    /// </returns>
    public string Get()
    {
        var form = @"{
                        name: 'deletecultureform',
                        viewTitle: 'Delete culture.',
                        saveEvent: 'deleteculture',
                        initialQuery: 'cultureviewdetailsquery',
                        suppressRecordView: true,
                        pages: [ 'deleteculturepage']
                    }";

        return form;
    }
}
