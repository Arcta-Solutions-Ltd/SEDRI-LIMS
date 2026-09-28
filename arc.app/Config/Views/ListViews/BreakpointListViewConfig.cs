using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews
{
    /// <summary>
    /// Represents the configuration for the Breakpoint list view.
    /// </summary>
    internal class BreakpointListViewConfig
    {
        /// <summary>
        /// Gets the view configuration for the Breakpoint list view.
        /// </summary>
        /// <returns>The list view configuration for the Breakpoint list view, including filters for organism, test method, specification, and host.</returns>
        internal static ListViewConfig GetView()
        {
            var view = @"{
                            'name': 'breakpoints',
                            'type': 'ManageList',
                            'title': '@BreMan@',
                            'headerText': '@BreManA@.',
                            'queryName': 'BreakpointList',
                            'singleQuery': 'BreakpointByIdForEdit',
                            multiselect: true,
                            'gridColumns': [
                                { 'key': 'column1', 'name': '@GenOrgA@', 'fieldName': 'OrganismName', 'minWidth': 150, 'maxWidth': 150, 'isResizable': true },
                                { 'key': 'menu', 'name': '', 'fieldName': '', 'minWidth': 120, 'maxWidth': 120, 'isResizable': true, 'isCollapsible': false },
                                { 'key': 'column2', 'name': '@GenOrd@', 'fieldName': 'OrderName', 'minWidth': 110, 'maxWidth': 110, 'isResizable': true },
                                { 'key': 'column3', 'name': '@GenFam@', 'fieldName': 'FamilyName', 'minWidth': 110, 'maxWidth': 110, 'isResizable': true },
                                { 'key': 'column4', 'name': '@GenOrgE@', 'fieldName': 'OrgGroupName', 'minWidth': 110, 'maxWidth': 500, 'isResizable': true },
                                { 'key': 'column6', 'name': '@GenAnt@', 'fieldName': 'AntibioticName', 'minWidth': 160, 'maxWidth': 160, 'isResizable': true, isSorted: true, isSortedDescending: false },
                                { 'key': 'column7', 'name': '@BreTes@', 'fieldName': 'TestMethod', 'minWidth': 80, 'maxWidth': 80, 'isResizable': true, isCollapsible: true },
                                { 'key': 'column8', 'name': '@GenDos@', 'fieldName': 'AntibioticDosage', 'minWidth': 50, 'maxWidth': 50, 'isResizable': true, isCollapsible: true },
                                { 'key': 'column9', 'name': '@BreSpf@', 'fieldName': 'Specification', 'minWidth': 50, 'maxWidth': 50, 'isResizable': true, isCollapsible: true },
                                { 'key': 'column10', 'name': '@BreSpeA@', 'fieldName': 'SpecialConsider', 'minWidth': 100, 'maxWidth': 200, 'isResizable': true, isCollapsible: true },
                                { 'key': 'column11', 'name': '@GenEna@', 'fieldName': 'Enabled', 'minWidth': 60, 'maxWidth': 200, 'isResizable': true }
                            ],
                            'buttons': [
                                { key: 'addBreakpoint', 'text': '@BreAddO@', 'icon': 'Add', uievent: 'addbreakpointuievent', onFinish: 'refresh' },
                                { key: 'view', text: '@GenVieC@', icon: 'RedEye', uievent: 'viewbreakpointuievent', onSelect: true, primaryAction: 1 },
                                { key: 'editBreakpoint', text: '@BreEdiH@', icon: 'Edit', uievent: 'editbreakpointuievent', onFinish: 'refresh', onSelect: true, primaryAction: 2 },
                                { key: 'deleteBreakpoint', text: '@BreDelG@', icon: 'Delete', uievent: 'deletebreakpointuievent', onFinish: 'refresh', onSelect: true, primaryAction: 3 },
                                { key: 'batchbreakpoint', text: '@BreBat@', icon: 'Add', onBatch: true,
                                  buttons: [
                                    { key: 'batchapprovebreakpoint', text: '@BreBatA@', onBatch: true, icon: 'DocumentApproval', uievent: 'batchapprovebreakpointuievent', onFinish: 'refresh' },
                                    { key: 'batchrejectbreakpoint', text: '@BreBatB@', onBatch: true, icon: 'PageRemove', uievent: 'batchrejectbreakpointuievent', onFinish: 'refresh' }
                                  ]
                                },
                                { key: 'testmethods', text: '@BreTesA@', icon: 'TestPlan',
                                    buttons: [       
                                        { key: 'addTestMethod', 'text': '@BreAddP@', 'icon': 'Add', uievent: 'addtestmethoduievent', onFinish: 'refreshfilter' },
                                        { key: 'editTestMethod', text: '@BreEdiI@', icon: 'Edit', uievent: 'edittestmethoduievent', onFinish: 'refreshfilter' },
                                        { key: 'deleteTestMethod', text: '@BreDelH@', icon: 'Remove', uievent: 'deletetestmethoduievent', onFinish: 'refreshfilter' }
                                    ]
                                },
                                { key: 'hosts', text: '@BreHos@', icon: 'FangBody',
                                    buttons: [       
                                        { key: 'addHost', 'text': '@BreAddQ@', 'icon': 'Add', uievent: 'addhostuievent', onFinish: 'refreshfilter' },
                                        { key: 'editHost', text: '@BreEdiJ@', icon: 'Edit', uievent: 'edithostuievent', onFinish: 'refreshfilter' },
                                        { key: 'deleteHost', text: '@BreDelI@', icon: 'Remove', uievent: 'deletehostuievent', onFinish: 'refreshfilter' }
                                    ]
                                },
                                { key: 'testresult', text: '@GenSus@', icon: 'Hospital',
                                    buttons: [       
                                        { key: 'addResult', 'text': '@BreAddR@', 'icon': 'Add', uievent: 'addresultuievent', onFinish: 'refreshfilter' },
                                        { key: 'editResult', text: '@BreEdiK@', icon: 'Edit', uievent: 'editresultuievent', onFinish: 'refreshfilter' },
                                        { key: 'deleteResult', text: '@BreDelJ@', icon: 'Remove', uievent: 'deleteresultuievent', onFinish: 'refreshfilter' }
                                    ]
                                }
                            ],
                            filters: [
                                { key: 'organism', fieldName: 'organismid', placeholder: '@GenOrgA@', multiSelect: true, width: 160, optionsName: 'specimenorganism', dynamic: true, includeFixed: true, dropdownwidth: 400 },
                                { key: 'testmethod', fieldName: 'testmethodid', placeholder: '@BreTes@', multiSelect: true, width: 160, optionsName: 'TestMethod', dynamic: true, includeFixed: true},
                                { key: 'specification', fieldName: 'specificationid', placeholder: '@BreSpf@', multiSelect: true, width: 160, optionsName: 'specification', dynamic: true, includeFixed: true },
                                { key: 'host', fieldName: 'hostid', placeholder: '@GenHos@', multiSelect: true, width: 160, optionsName: 'Host', dynamic: true, includeFixed: true }
                            ],
                            filterSearch: true,
                            searchFields: [ 'antibioticname' ]
                        }";

            var result = JsonConvert.DeserializeObject<ListViewConfig>(view);

            return result;
        }
    }
}


//filterPresets: [
//    { key: 'filter1', name: '@CodCOM@', default: true, fields: [ { key: 'coding', values: [ '1000'] } ] }
//],
