using arc.app.Common;

namespace arc.app.Config.Forms.Billing;

internal class DeleteBillingRuleFormConfig : IDefinition
{
    public string Get()
    {
        var form = @"{
                        name: 'deletebillingruleform',
                        viewTitle: 'Delete billing rule.',
                        saveEvent: 'deletebillingrule',
                        suppressRecordView: true,
                        initialQuery: 'singlebillingruleforlist',
                        pages: [ 'deletebillingrulepage' ]
                    }";

        return form;
    }
}
