using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Configuration for the "Delete Form Specimen Type Option" form.
/// </summary>
internal class DeleteFormSpecimenTypeOptionFormConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Delete Form Specimen Type Option" form.
    /// </summary>
    /// <returns>
    /// A string representation of the form configuration, detailing the save event, initial query and
    /// associated page.
    /// </returns>
    public string Get()
    {
        var form = @"{
                        name: 'deleteformspecimentypeoptionform',
                        viewTitle: '@ConFormSpeDel@',
                        saveEvent: 'deleteformspecimentypeoption',
                        suppressRecordView: true,
                        initialQuery: 'editformspecimentypeoptionquery',
                        pages: ['deleteformspecimentypepage']
                    }";

        return form;
    }
}
