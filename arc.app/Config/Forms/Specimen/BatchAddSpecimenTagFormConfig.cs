using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Configuration for the batch add specimen tag form.
/// Allows adding tags to multiple specimens in batch from the specimen list view.
/// </summary>
internal class BatchAddSpecimenTagFormConfig : IDefinition
{
    /// <summary>
    /// Retrieves the form configuration.
    /// </summary>
    public string Get()
    {
        var form = @"{
                        name: 'batchaddspecimentagform',
                        viewTitle: '@GenTagB@',
                        saveEvent: 'batchaddspecimentag',
                        recordView: 'specimenrecordview',
                        suppressRecordView: true,
                        pages: ['batchaddspecimentagpage']
                    }";

        return form;
    }
}
