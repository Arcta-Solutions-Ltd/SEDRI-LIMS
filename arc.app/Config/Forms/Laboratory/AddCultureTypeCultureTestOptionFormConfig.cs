using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Configuration for the "Add Culture Type Culture Test Option" form.
/// </summary>
internal class AddCultureTypeCultureTestOptionFormConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Add Culture Type Culture Test Option" form.
    /// </summary>
    /// <remarks>
    /// This configuration defines the form's name, title, save event, and associated pages.
    /// It also includes options to suppress the record view and indicates whether the form is configurable.
    /// </remarks>
    /// <returns>
    /// A string representation of the form configuration, detailing metadata such as 
    /// the save event, configurable property, and associated pages.
    /// </returns>
    public string Get()
    {
        var form = @"{
                        name: 'addculturetypeculturetestoptionform',
                        title: 'Add new culture type culture est form.',
                        saveEvent: 'addculturetypeculturetestoptionevent',
                        suppressRecordView: true,
                        configurable: 'Yes',
                        pages: ['culturetypeculturetestpage']
                    }";

        return form;
    }
}
