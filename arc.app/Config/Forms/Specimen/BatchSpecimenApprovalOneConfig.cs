using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// This class defines the configuration for the batch specimen approval level one form.
/// </summary>
internal class BatchSpecimenApprovalOneConfig : IDefinition
{
    /// <summary>
    /// Gets the form configuration as a JSON string.
    /// </summary>
    /// <returns>A JSON string representing the form configuration.</returns>
    public string Get()
    {
        var form = @"{
                        name: 'batchspecimenapprovaloneform',
                        title: '@SpeFir@',
                        text: 'Batch level one approval',
                        saveEvent: 'batchspecimenapprovalone',
                        recordView: 'specimenrecordview',
                        suppressRecordView: true,
                        pages: ['batchspecimenapprovalpage']
                    }";

        return form;
    }
}
