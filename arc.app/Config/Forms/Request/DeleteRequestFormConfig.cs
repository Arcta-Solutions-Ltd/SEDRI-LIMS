using arc.app.Common;

namespace arc.app.Config.Forms.Request;

internal class DeleteRequestFormConfig : IDefinition
{
    public string Get()
    {
        return @"{
                        name: 'deleterequestform',
                        viewTitle: '@NeoReqDel@',
                        saveEvent: 'deleterequest',
                        initialQuery: 'editrequestquery',
                        suppressRecordView: true,
                        pages: [ 'deleterequestpage' ]
                    }";
    }
}
