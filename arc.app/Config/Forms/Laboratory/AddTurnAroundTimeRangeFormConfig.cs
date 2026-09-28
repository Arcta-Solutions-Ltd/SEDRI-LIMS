using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Form configuration for adding a Turn Around Time range.
/// </summary>
internal class AddTurnAroundTimeRangeFormConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON form configuration.
    /// </summary>
    public string Get()
    {
        var form = @"{
                        name: 'addturnaroundtimerangeform',
                        viewTitle: '@GenTATB@',
                        saveEvent: 'addturnaroundtimerange',
                        saveOperation: 'updategrid',
                        suppressRecordView: true,
                        pages: ['addturnaroundtimerangepage']
                    }";

        return form;
    }
}
