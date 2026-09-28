using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Configuration for the "Edit Specimen Type Culture Type" form.
/// </summary>
internal class EditSpecimenTypeCultureTypeFormConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Edit Specimen Type Culture Type" form.
    /// </summary>
    /// <remarks>
    /// This configuration specifies the form's name, view title, save event, and associated pages.
    /// It also includes options to suppress the record view and defines the initial query executed
    /// when the form is loaded.
    /// </remarks>
    /// <returns>
    /// A string representation of the form configuration, detailing metadata such as 
    /// the save event, initial query, and associated pages.
    /// </returns>
    public string Get()
    {
        var form = @"{
                        name: 'editspecimentypeculturetypeform',
                        viewTitle: 'Edit specimen type culture type mapping.',
                        saveEvent: 'editspecimentypeculturetype',
                        suppressRecordView: true,
                        initialQuery: 'editdirecttestdefaultquery',
                        pages: [ 'editspecimentypeculturetypepage']
                    }";

        return form;
    }
}
