using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// UI event to open the instrument result record view (read-only full row) from list row actions.
/// </summary>
internal class ViewInstrumentResultRecordUIEventConfig : IDefinition
{
    public string Get()
    {
        return @"{
                        name: 'viewinstrumentresultrecord',
                        description: '@InsViewA@',
                        type: 'view-record',
                        action: 'instrumentresultrecordview'
                    }";
    }
}
