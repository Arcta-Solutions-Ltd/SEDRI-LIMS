using arc.app.Common;

namespace arc.app.Config.UIEvents.Request;

internal class ViewRequestRecordUIEventConfig : IDefinition
{
    public string Get()
    {
        return @"{
                        name: 'viewrequestrecorduievent',
                        description: 'View request record',
                        type: 'view-record',
                        action: 'requestrecordview'
                    }";
    }
}
