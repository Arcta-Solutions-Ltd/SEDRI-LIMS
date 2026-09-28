using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Configuration for the "Edit Form Specimen Type Option" form.
/// </summary>
internal class EditFormSpecimenTypeOptionFormConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Edit Form Specimen Type Option" form.
    /// </summary>
    /// <returns>
    /// A string representation of the form configuration, detailing the save event, initial query and
    /// associated page.
    /// </returns>
    public string Get()
    {
        var form = @"{
                        name: 'editformspecimentypeoptionform',
                        viewTitle: '@ConFormSpeEdi@',
                        saveEvent: 'editformspecimentypeoption',
                        suppressRecordView: true,
                        initialQuery: 'editformspecimentypeoptionquery',
                        pages: ['editformspecimentypepage']
                    }";

        return form;
    }
}
