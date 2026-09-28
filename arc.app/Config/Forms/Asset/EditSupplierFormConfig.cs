using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Represents the configuration for the Edit Supplier form.
/// </summary>
internal class EditSupplierFormConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON string that defines the configuration for the Edit Supplier form.
    /// </summary>
    /// <returns>A JSON string representing the Edit Supplier form configuration.</returns>
    public string Get()
    {
        var form = @"{
                        name: 'editsupplierform',
                        viewTitle: 'Edit an existing supplier.',
                        saveEvent: 'editsupplierevent',
                        initialQuery: 'editsupplierquery',
                        suppressRecordView: true,
                        pages: [ 'editsupplierpage']
                    }";

        return form;
    }
}

