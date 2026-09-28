using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Configuration for the "Add Specimen Type Direct Test Option" form.
/// </summary>
internal class AddSpecimenTypeDirectTestOptionFormConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Add Specimen Type Direct Test Option" form.
    /// </summary>
    /// <remarks>
    /// This configuration specifies the form's name, title, save event, and associated pages.
    /// It also includes options to suppress the record view and indicates whether the form 
    /// is configurable.
    /// </remarks>
    /// <returns>
    /// A string representation of the form configuration, including metadata such as 
    /// the save event, configurable property, and associated pages.
    /// </returns>
    public string Get()
    {
        var form = @"{
                        name: 'addspecimentypedirecttestoptionform',
                        title: 'Add specimen type direct test option form.',
                        saveEvent: 'addspecimentypedirecttestoptionevent',
                        suppressRecordView: true,
                        configurable: 'Yes',
                        pages: ['specimentypedirecttestpage']
                    }";

        return form;
    }
}
