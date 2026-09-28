using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews;

internal class BillingRuleViewConfig
{
    internal static ListViewConfig GetView()
    {
        var view = @"{
                            'name': 'billingrule',
                            'type': 'ManageList',
                            'title': '@BilBilA@',
                            'headerText': '@BilRulH@',
                            'queryName': 'BillingRuleList',
                            'singleQuery': 'SingleBillingRuleForList',
                            'gridColumns': [
                                { 'key': 'c1', 'name': '@GenNam@', 'fieldName': 'name', 'minWidth': 200, 'maxWidth': 500, 'isResizable': true },
                                { 'key': 'menu', 'name': '', 'fieldName': '', 'minWidth': 120, 'maxWidth': 120, 'isResizable': true, 'isCollapsible': false }
                            ],
                            'buttons': [
                                { 'key': 'addBillingRule', 'text': '@BilAdd@', 'icon': 'Add', 'onSelect': false, 'uievent': 'addbillingruleuievent', 'onFinish': 'refresh' },
                                { 'key': 'editBillingRule', 'text': '@GenEdi@', 'icon': 'Edit', primaryAction: 1, 'onSelect': true, 'uievent': 'editbillingruleuievent', 'onFinish': 'update' },
                                { 'key': 'deleteBillingRule', 'text': '@GenDel@', 'icon': 'Delete', primaryAction: 2, 'onSelect': true, 'uievent': 'deletebillingruleuievent', 'onFinish': 'refresh' }
                            ],
                            'filterSearch': true,
                            'searchFields': [ 'name' ]
                        }";

        return JsonConvert.DeserializeObject<ListViewConfig>(view);
    }
}
