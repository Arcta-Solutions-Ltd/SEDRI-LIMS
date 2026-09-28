using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Form configuration for editing a Turn Around Time range.
/// </summary>
internal class EditTurnAroundTimeRangeFormConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON form configuration.
    /// </summary>
    public string Get()
    {
        var form = @"{
                        name: 'editturnaroundtimerangeform',
                        viewTitle: '@GenTATD@',
                        saveEvent: 'editturnaroundtimerange',
                        saveOperation: 'updategrid',
                        suppressRecordView: true,
                        pages: ['addturnaroundtimerangepage']
                    }";

        return form;
    }
}
