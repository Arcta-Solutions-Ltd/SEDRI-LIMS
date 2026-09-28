using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Configuration for the "Edit Culture Type Category" form.
/// </summary>
internal class EditCultureTypeCategoryFormConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Edit Culture Type Category" form.
    /// </summary>
    /// <remarks>
    /// This configuration specifies the form's name, view title, save event, and associated pages.
    /// It also includes options to suppress the record view and defines the initial query 
    /// that is executed when the form is loaded.
    /// </remarks>
    /// <returns>
    /// A string representation of the form configuration, including metadata such as the 
    /// save event, initial query, and associated pages.
    /// </returns>
    public string Get()
    {
        var form = @"{
                        name: 'editculturetypecategoryform',
                        viewTitle: 'Edit culture type category rule.',
                        saveEvent: 'editculturetypecategoryevent',
                        suppressRecordView: true,
                        initialQuery: 'editdirecttestdefaultquery',
                        pages: [ 'editculturetypecategorypage']
                    }";

        return form;
    }
}
