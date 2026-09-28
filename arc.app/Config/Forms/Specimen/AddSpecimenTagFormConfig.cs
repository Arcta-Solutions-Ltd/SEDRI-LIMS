using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Configuration for the "Add Specimen Tag" form.
/// Allows adding a tag to a specimen from the specimen record view.
/// </summary>
internal class AddSpecimenTagFormConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Add Specimen Tag" form.
    /// </summary>
    public string Get()
    {
        var form = @"{
                        name: 'addspecimentagform',
                        viewTitle: '@GenTagB@',
                        saveEvent: 'addspecimentag',
                        recordView: 'specimenrecordview',
                        suppressRecordView: false,
                        InitialQuery: 'AddSpecimenTagFormInitialQuery',
                        pages: [ 'addspecimentagpage' ]
                    }";

        return form;
    }
}
