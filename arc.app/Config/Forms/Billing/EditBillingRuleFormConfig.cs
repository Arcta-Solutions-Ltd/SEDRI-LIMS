using arc.app.Common;

namespace arc.app.Config.Forms.Billing;

internal class EditBillingRuleFormConfig : IDefinition
{
    public string Get()
    {
        var form = @"{
                        name: 'editbillingruleform',
                        viewTitle: 'Edit billing rule.',
                        saveEvent: 'editbillingrule',
                        initialQuery: 'singlebillingruleforlist',
                        suppressRecordView: true,
                        pages: [ 'editbillingrulepage' ]
                    }";

        return form;
    }
}
