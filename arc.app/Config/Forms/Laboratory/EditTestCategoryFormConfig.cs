using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Configuration for the "Edit Test Category" form.
/// </summary>
internal class EditTestCategoryFormConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Edit Test Category" form.
    /// </summary>
    /// <remarks>
    /// This configuration specifies the form's name, view title, save event, and associated pages.
    /// It also includes an option to suppress the record view when the form is used.
    /// </remarks>
    /// <returns>
    /// A string representation of the form configuration, detailing the metadata 
    /// and pages associated with the form.
    /// </returns>
    public string Get()
    {
        var form = @"{
                        name: 'edittestcategoryform',
                        viewTitle: 'Edit test category rule.',
                        saveEvent: 'edittestcategoryevent',
                        suppressRecordView: true,
                        initialQuery: 'editdirecttestdefaultquery',
                        pages: [ 'edittestcategorypage']
                    }";

        return form;
    }
}
