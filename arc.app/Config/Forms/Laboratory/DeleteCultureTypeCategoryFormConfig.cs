using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Configuration for the "Delete Culture Type Category" form.
/// </summary>
internal class DeleteCultureTypeCategoryFormConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Delete Culture Type Category" form.
    /// </summary>
    /// <remarks>
    /// This configuration defines the form's name, view title, save event, and associated pages.
    /// It also includes options to suppress the record view and specifies the initial query 
    /// used when the form is loaded.
    /// </remarks>
    /// <returns>
    /// A string representation of the form configuration, including metadata and associated pages.
    /// </returns>
    public string Get()
    {
        var form = @"{
                        name: 'deleteculturetypecategoryform',
                        viewTitle: 'Delete culture type category rule.',
                        saveEvent: 'deleteculturetypecategoryevent',
                        suppressRecordView: true,
                        initialQuery: 'editdirecttestdefaultquery',
                        pages: [ 'deleteculturetypecategorypage']
                    }";

        return form;
    }
}

