using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews
{
    /// <summary>
    /// Gets the view configuration for the Expert rules view.
    /// </summary>
    /// <returns>The list view configuration for the Expert rules view.</returns>
    internal class ExpertRuleListViewConfig
    {
        /// <summary>
        /// Gets the view configuration for the Expert rules view.
        /// </summary>
        /// <returns>The list view configuration for the Expert rules view.</returns>
        internal static ListViewConfig GetView()
        {
            var view = @"{
                            'name': 'expertrules',
                            'type': 'ManageList',
                            'title': '@RulMan@',
                            'headerText': '@RulCre@.',
                            'queryName': 'ExpertRuleList',
                            'singleQuery': 'SingleExpertRuleForExpertRuleList',
                            multiselect: true,
                            'gridColumns': [
                                { 'key': 'column1', 'name': '@GenNam@', 'fieldName': 'ExpertRuleName', 'minWidth': 200, 'maxWidth': 200, 'isResizable': true },
                                { 'key': 'menu', 'name': '', 'fieldName': '', 'minWidth': 120, 'maxWidth': 120, 'isResizable': true, 'isCollapsible': false },
                                { 'key': 'column2', 'name': '@GenDes@', 'fieldName': 'RuleText', 'minWidth': 460, 'maxWidth': 550, 'isResizable': true },
                                { 'key': 'column3', 'name': '@GenOrgA@', 'fieldName': 'OrganismName', 'minWidth': 150, 'maxWidth': 150, 'isResizable': true },
                                { 'key': 'column4', 'name': '@GenOrd@', 'fieldName': 'OrderName', 'minWidth': 130, 'maxWidth': 130, 'isResizable': true },
                                { 'key': 'column5', 'name': '@GenFam@', 'fieldName': 'FamilyName', 'minWidth': 130, 'maxWidth': 130, 'isResizable': true },
                                { 'key': 'column6', 'name': '@GenOrgE@', 'fieldName': 'OrgGroupName', 'minWidth': 400, 'maxWidth': 400, 'isResizable': true },
                                { 'key': 'column7', 'name': '@BreSpf@', 'fieldName': 'Specification', 'minWidth': 130, 'maxWidth': 130, 'isResizable': true },
                                { 'key': 'column8', 'name': '@GenEna@', 'fieldName': 'Enabled', 'minWidth': 80, 'maxWidth': 80, 'isResizable': true }
                            ],
                            'buttons': [
                                { 'key': 'addexpertrule', text: '@RulAddA@', icon: 'Add', uievent: 'addexpertruleuievent', onFinish: 'refresh' },
                                { key: 'view', text: '@GenVieC@', icon: 'RedEye', uievent: 'viewexpertruleuievent', onSelect: true, primaryAction: 1 },
                                { 'key': 'editexpertrule', 'text': '@RulEdiA@', 'icon': 'Edit', 'onSelect': true, 'primaryAction': 2, uievent: 'editexpertruleuievent', onFinish: 'update' },
                                { 'key': 'deleteexpertrule', 'text': '@RulDelA@', 'icon': 'Delete', uievent: 'deleteexpertruleuievent', 'onSelect': true, 'primaryAction': 3, onFinish: 'refresh' },
                                { key: 'batchexpertrule', text: '@RulBat@', icon: 'Add', onBatch: true,
                                  buttons: [
                                    { key: 'batchapproveexpertrule', text: '@RulBatA@', onBatch: true, icon: 'DocumentApproval', uievent: 'batchapproveexpertruleuievent', onFinish: 'refresh' },
                                    { key: 'batchrejectexpertrule', text: '@RulBatB@', onBatch: true, icon: 'PageRemove', uievent: 'batchrejectexpertruleuievent', onFinish: 'refresh' }
                                  ]
                                }
                            ],
                            filters: [
                                { key: 'specification', placeholder: '@BreSpf@', multiSelect: true, width: 140, optionsName: 'specification', fieldName: 'specificationid', dynamic: true }
                            ],
                            filterSearch: true,
                            searchFields: [ 'searchText' ]
                        }";

            var result = JsonConvert.DeserializeObject<ListViewConfig>(view);

            return result;
        }
    }
}
