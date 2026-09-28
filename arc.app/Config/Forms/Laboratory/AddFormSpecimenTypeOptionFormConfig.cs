using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Configuration for the "Add Form Specimen Type Option" form.
/// </summary>
internal class AddFormSpecimenTypeOptionFormConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Add Form Specimen Type Option" form.
    /// </summary>
    /// <returns>
    /// A string representation of the form configuration, detailing the save event and associated page.
    /// </returns>
    public string Get()
    {
        var form = @"{
                        name: 'addformspecimentypeoptionform',
                        title: '@ConFormSpeAdd@',
                        saveEvent: 'addformspecimentypeoption',
                        suppressRecordView: true,
                        configurable: 'Yes',
                        pages: ['formspecimentypepage']
                    }";

        return form;
    }
}
