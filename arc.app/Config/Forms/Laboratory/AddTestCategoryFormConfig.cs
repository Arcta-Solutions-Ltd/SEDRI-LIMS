using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Configuration for the "Add Test Category" form.
/// </summary>
internal class AddTestCategoryFormConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Add Test Category" form.
    /// </summary>
    /// <remarks>
    /// This configuration defines the name, view title, save event, and other properties
    /// for the form used to add test categories. It includes options to suppress the record view 
    /// and specifies the pages associated with the form.
    /// </remarks>
    /// <returns>
    /// A string representation of the form configuration, including metadata and associated pages.
    /// </returns>
    public string Get()
    {
        var form = @"{
                        name: 'addtestcategoryform',
                        viewTitle: 'Add test category form.',
                        saveEvent: 'addtestcategoryevent',
                        suppressRecordView: true,
                        pages: [ 'addtestcategorypage']
                    }";

        return form;
    }
}
