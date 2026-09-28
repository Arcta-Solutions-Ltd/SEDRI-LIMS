using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Configuration for the "Edit Specimen Type Direct Test Option" form.
/// </summary>
internal class EditSpecimenTypeDirectTestOptionFormConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Edit Specimen Type Direct Test Option" form.
    /// </summary>
    /// <remarks>
    /// This configuration specifies the form's name, title, save event, and associated pages.
    /// It also includes options to suppress the record view and indicates whether the form is configurable.
    /// </remarks>
    /// <returns>
    /// A string representation of the form configuration, detailing metadata such as 
    /// the save event, configurable property, and associated pages.
    /// </returns>
    public string Get()
    {
        var form = @"{
                        name: 'editspecimentypedirecttestoptionform',
                        title: 'Edit specimen type direct test option form.',
                        saveEvent: 'editspecimentypedirecttestoptionevent',
                        suppressRecordView: true,
                        configurable: 'Yes',
                        initialQuery: 'editdirecttestdefaultquery',
                        pages: ['editspecimentypedirecttestpage']
                    }";

        return form;
    }
}
