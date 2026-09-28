using arc.app.Common;

namespace arc.app.Config.Forms.Request;

internal class EditRequestFormConfig : IDefinition
{
    public string Get()
    {
        return @"{
                        name: 'editrequestform',
                        viewTitle: '@NeoReqEdi@',
                        saveEvent: 'editrequest',
                        initialQuery: 'editrequestquery',
                        suppressRecordView: true,
                        pages: [ 'neoshieldrequestheaderpage', 'neoshieldclinicalstatepage' ]
                    }";
    }
}
