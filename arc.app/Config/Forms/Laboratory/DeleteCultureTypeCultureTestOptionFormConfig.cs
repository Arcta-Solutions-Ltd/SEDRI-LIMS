using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Configuration for the "Delete Culture Type Culture Test Option" form.
/// </summary>
internal class DeleteCultureTypeCultureTestOptionFormConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Delete Culture Type Culture Test Option" form.
    /// </summary>
    /// <remarks>
    /// This configuration defines the form's name, view title, save event, and associated pages.
    /// It also includes options to suppress the record view and defines the initial query 
    /// executed when the form is loaded.
    /// </remarks>
    /// <returns>
    /// A string representation of the form configuration, detailing metadata such as 
    /// the save event, initial query, and associated pages.
    /// </returns>
    public string Get()
    {
        var form = @"{
                        name: 'deleteculturetypeculturetestoptionform',
                        viewTitle: 'Delete culture type culture test option.',
                        saveEvent: 'deleteculturetypeculturetestoptionevent',
                        suppressRecordView: true,
                        initialQuery: 'editdirecttestdefaultquery',
                        pages: [ 'deleteculturetypeculturetestpage']
                    }";

        return form;
    }
}
