using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Represents the configuration for the Delete Supplier form.
/// </summary>
internal class DeleteSupplierFormConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON string that defines the configuration for the Delete Supplier form.
    /// </summary>
    /// <returns>A JSON string representing the Delete Supplier form configuration.</returns>
    public string Get()
    {
        var form = @"{
                        name: 'deletesupplierform',
                        viewTitle: 'Delete an existing supplier.',
                        saveEvent: 'deletesupplierevent',
                        initialQuery: 'editsupplierquery',
                        suppressRecordView: true,
                        pages: [ 'deletesupplierpage']
                    }";

        return form;
    }
}

