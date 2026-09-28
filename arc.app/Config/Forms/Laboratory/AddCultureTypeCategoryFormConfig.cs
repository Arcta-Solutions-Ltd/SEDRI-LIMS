using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Configuration for the "Add Culture Type Category" form.
/// </summary>
internal class AddCultureTypeCategoryFormConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Add Culture Type Category" form.
    /// </summary>
    /// <remarks>
    /// This configuration specifies the form's name, view title, save event, and associated pages.
    /// It also includes an option to suppress the record view when the form is used.
    /// </remarks>
    /// <returns>
    /// A string representation of the form configuration, including metadata such as 
    /// the save event and associated pages.
    /// </returns>
    public string Get()
    {
        var form = @"{
                        name: 'addculturetypecategoryform',
                        viewTitle: 'Add culture type category form.',
                        saveEvent: 'addculturetypecategoryevent',
                        suppressRecordView: true,
                        pages: [ 'addculturetypecategorypage']
                    }";

        return form;
    }
}
