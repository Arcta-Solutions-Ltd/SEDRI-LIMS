using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Configuration for the "Add Specimen Type Culture Type" form.
/// </summary>
internal class AddSpecimenTypeCultureTypeFormConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Add Specimen Type Culture Type" form.
    /// </summary>
    /// <remarks>
    /// This configuration specifies the form's name, view title, save event, and associated pages.
    /// It also includes options to suppress the record view, defining the form's behavior and layout 
    /// for adding a new specimen type culture type mapping within the system.
    /// </remarks>
    /// <returns>
    /// A string representation of the form configuration, detailing metadata such as 
    /// the save event and associated pages.
    /// </returns>
    public string Get()
    {
        var form = @"{
                        name: 'addspecimentypeculturetypeform',
                        viewTitle: 'Add specimen type culture type mapping.',
                        saveEvent: 'addspecimentypeculturetype',
                        suppressRecordView: true,
                        pages: [ 'specimentypeculturetypepage']
                    }";

        return form;
    }
}
