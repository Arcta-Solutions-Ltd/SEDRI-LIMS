using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews;

internal class BillingRecordViewConfig
{
    internal static ListViewConfig GetView()
    {
        var view = @"{
                            'name': 'billingrecord',
                            'type': 'ManageList',
                            'title': '@BilBilB@',
                            'headerText': '@BilRecH@',
                            'queryName': 'BillingRecordList',
                            'singleQuery': 'SingleBillingRecordForList',
                            'gridColumns': [
                                { 'key': 'c1', 'name': '@BilPat@', 'fieldName': 'patientid', 'minWidth': 90, 'maxWidth': 120, 'isResizable': true },
                                { 'key': 'menu', 'name': '', 'fieldName': '', 'minWidth': 120, 'maxWidth': 120, 'isResizable': true, 'isCollapsible': false },
                                { 'key': 'c2', 'name': '@BilSpe@', 'fieldName': 'specimenid', 'minWidth': 90, 'maxWidth': 120, 'isResizable': true },
                                { 'key': 'c3', 'name': '@BilCul@', 'fieldName': 'cultureid', 'minWidth': 90, 'maxWidth': 120, 'isResizable': true },
                                { 'key': 'c4', 'name': '@BilDir@', 'fieldName': 'directtestname', 'minWidth': 160, 'maxWidth': 320, 'isResizable': true },
                                { 'key': 'c5', 'name': '@BilTes@', 'fieldName': 'testpatternid', 'minWidth': 90, 'maxWidth': 120, 'isResizable': true },
                                { 'key': 'c6', 'name': '@GenDes@', 'fieldName': 'description', 'minWidth': 200, 'maxWidth': 400, 'isResizable': true },
                                { 'key': 'c7', 'name': '@BilAmt@', 'fieldName': 'totalamount', 'minWidth': 100, 'maxWidth': 140, 'isResizable': true }
                            ],
                            'buttons': [
                                { 'key': 'editBillingRecord', 'text': '@GenEdi@', 'icon': 'Edit', primaryAction: 1, 'onSelect': true, 'uievent': 'editbillingrecorduievent', 'onFinish': 'update' },
                                { 'key': 'deleteBillingRecord', 'text': '@GenDel@', 'icon': 'Delete', primaryAction: 2, 'onSelect': true, 'uievent': 'deletebillingrecorduievent', 'onFinish': 'refresh' }
                            ],
                            'filterSearch': true,
                            'searchFields': [ 'description', 'directtestname' ]
                        }";

        return JsonConvert.DeserializeObject<ListViewConfig>(view);
    }
}
