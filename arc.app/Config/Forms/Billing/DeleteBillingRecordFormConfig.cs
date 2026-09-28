using arc.app.Common;

namespace arc.app.Config.Forms.Billing;

internal class DeleteBillingRecordFormConfig : IDefinition
{
    public string Get()
    {
        var form = @"{
                        name: 'deletebillingrecordform',
                        viewTitle: 'Delete billing record.',
                        saveEvent: 'deletebillingrecord',
                        suppressRecordView: true,
                        initialQuery: 'singlebillingrecordforlist',
                        pages: [ 'deletebillingrecordpage' ]
                    }";

        return form;
    }
}
