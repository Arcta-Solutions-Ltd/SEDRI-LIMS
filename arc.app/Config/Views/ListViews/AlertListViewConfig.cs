using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews
{
    /// <summary>
    /// Represents the configuration for the Alert list view.
    /// </summary>
    internal class AlertListViewConfig
    {
        /// <summary>
        /// Gets the view configuration for the Alert list view.
        /// </summary>
        /// <returns>The list view configuration for the Alert view.</returns>
        internal ListViewConfig GetView()
        {
            var view = @"{
                            'name': 'alertsconfig',
                            'type': 'ManageList',
                            'title': '@AleMan@',
                            'headerText': '@AleCre@.',
                            'queryName': 'AlertList',
                            'singleQuery': 'SingleAlertForAlertList',
                            'recordView': 'alerts',
                            'gridColumns': [
                                { 'key': 'column1', 'name': '@AleAle@', 'fieldName': 'AlertName', 'minWidth': 200, 'maxWidth': 200, 'isResizable': true },
                                { 'key': 'menu', 'name': '', 'fieldName': '', 'minWidth': 120, 'maxWidth': 120, 'isResizable': true, 'isCollapsible': false },
                                { 'key': 'column2', 'name': '@GenOrgA@', 'fieldName': 'OrganismName', 'minWidth': 150, 'maxWidth': 150, 'isResizable': true },
                                { 'key': 'column3', 'name': '@GenOrd@', 'fieldName': 'OrderName', 'minWidth': 130, 'maxWidth': 130, 'isResizable': true },
                                { 'key': 'column4', 'name': '@GenFam@', 'fieldName': 'FamilyName', 'minWidth': 130, 'maxWidth': 130, 'isResizable': true },
                                { 'key': 'column5', 'name': '@GenOrgE@', 'fieldName': 'OrgGroupName', 'minWidth': 400, 'maxWidth': 400, 'isResizable': true },
                                { 'key': 'column6', 'name': '@AleTyp@', 'fieldName': 'AlertType', 'minWidth': 80, 'maxWidth': 80, 'isResizable': true },
                                { 'key': 'column7', 'name': '@BreSpf@', 'fieldName': 'Specification', 'minWidth': 80, 'maxWidth': 80, 'isResizable': true },
                                { 'key': 'column8', 'name': '@GenEna@', 'fieldName': 'Enabled', 'minWidth': 80, 'maxWidth': 80, 'isResizable': true }
                            ],
                            'buttons': [
                                { 'key': 'addalert', text: '@AleAddF@', icon: 'Add', uievent: 'addalertuievent', onFinish: 'refresh' },
                                { 'key': 'addorganismalert', text: '@AleAddG@', icon: 'Add', uievent: 'addorganismalertuievent', onFinish: 'refresh' },
                                { 'key': 'view', text: '@GenVieC@', icon: 'RedEye', uievent: 'viewalertuievent', onSelect: true, primaryAction: 1 },
                                { 'key': 'editalert', 'text': '@AleEdiF@', 'icon': 'Edit', 'onSelect': true, uievent: 'editalertuievent', onFinish: 'update', primaryAction: 2 },
                                { 'key': 'deletealert', 'text': '@AleDelG@', 'icon': 'Delete', uievent: 'deletealertuievent', 'onSelect': true, onFinish: 'refresh' }
                            ],
                            filters: [
                                { key: 'alerttype', placeholder: '@AleTyp@', multiSelect: true, width: 140, optionsName: 'AlertType', fieldName: 'alerttypeid' },
                                { key: 'specification', placeholder: '@BreSpf@', multiSelect: true, width: 200, optionsName: 'specification', fieldName: 'specificationid', dynamic: true }
                            ],
                            filterSearch: true,
                            searchFields: [ 'searchText' ]
                        }";

            var result = JsonConvert.DeserializeObject<ListViewConfig>(view);

            return result;
        }
    }
}
