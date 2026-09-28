using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Configuration for the "Add Specimen Type Culture Type Option" form.
/// </summary>
internal class AddSpecimenTypeCultureTypeOptionFormConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Add Specimen Type Culture Type Option" form.
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
                        name: 'addspecimentypeculturetypeoptionform',
                        title: 'Add new specimen type culture type form.',
                        saveEvent: 'addspecimentypeculturetypeoptionevent',
                        suppressRecordView: true,
                        configurable: 'Yes',
                        pages: ['specimentypeculturetypepage']
                    }";

        return form;
    }
}

