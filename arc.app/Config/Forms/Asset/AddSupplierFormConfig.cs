using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Represents the configuration for the Add Supplier form.
/// </summary>
internal class AddSupplierFormConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON string that defines the configuration for the Add Supplier form.
    /// </summary>
    /// <returns>A JSON string representing the Add Supplier form configuration.</returns>
    public string Get()
    {
        var form = @"{
                        name: 'addsupplierform',
                        viewTitle: 'Add a new supplier.',
                        saveEvent: 'addsupplierevent',
                        suppressRecordView: true,
                        pages: [ 'addsupplierpage']
                    }";

        return form;
    }
}

