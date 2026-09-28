using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Configuration for the "Delete Test Category" form.
/// </summary>
internal class DeleteTestCategoryFormConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Delete Test Category" form.
    /// </summary>
    /// <remarks>
    /// This configuration defines the form's name, view title, save event, and associated pages.
    /// It also includes an option to suppress the record view when the form is used.
    /// </remarks>
    /// <returns>
    /// A string representation of the form configuration, including metadata and associated pages.
    /// </returns>
    public string Get()
    {
        var form = @"{
                        name: 'deletetestcategoryform',
                        viewTitle: 'Delete test category rule.',
                        saveEvent: 'deletetestcategoryevent',
                        suppressRecordView: true,
                        initialQuery: 'editdirecttestdefaultquery',
                        pages: [ 'deletetestcategorypage']
                    }";

        return form;
    }
}
