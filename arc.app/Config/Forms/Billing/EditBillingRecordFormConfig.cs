using arc.app.Common;

namespace arc.app.Config.Forms.Billing;

internal class EditBillingRecordFormConfig : IDefinition
{
    public string Get()
    {
        var form = @"{
                        name: 'editbillingrecordform',
                        viewTitle: 'Edit billing record.',
                        saveEvent: 'editbillingrecord',
                        initialQuery: 'singlebillingrecordforlist',
                        suppressRecordView: true,
                        pages: [ 'editbillingrecordpage' ]
                    }";

        return form;
    }
}
