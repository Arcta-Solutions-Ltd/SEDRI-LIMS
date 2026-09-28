using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews;

/// <summary>
/// Configuration for the "Organism Scope Culture Test Option" list view.
/// Displays organism scope and associated isolate tests in the laboratory config record view.
/// </summary>
internal class OrganismScopeCultureTestOptionViewConfig
{
    /// <summary>
    /// Retrieves the configuration for the "Organism Scope Culture Test Option" view.
    /// </summary>
    /// <returns>
    /// A <see cref="ListViewConfig"/> object representing the list view configuration.
    /// </returns>
    internal static ListViewConfig GetView()
    {
        var view = @"{
                            'name': 'organismscopeculturetestoptionview',
                            'type': 'ManageList',
                            'title': '@LabOrgA@',
                            'headerText': '@LabManD@.',
                            'queryName': 'organismscopeculturetestoptionlistquery',
                            'displaySummary': false,
                            'addbutton': 'addorganismscopeculturetestoptionuievent',
                            'addButtonTooltip': '@LabOrgC@',
                            'DisplayIfEmpty':true,
                            'gridColumns': [
                                { 'key': 'column1', 'name': '@GenSelI@', 'fieldName': 'GroupDescription', 'minWidth': 180, 'maxWidth': 180, 'isResizable': true, isCollapsible: false },
                                { 'key': 'menu', name: '', fieldName: '', minWidth: 120, maxWidth: 120, 'isResizable': true, 'isCollapsible': false },
                                { 'key': 'column2', 'name': '@ConCulB@', 'fieldName': 'AssociatedList', 'minWidth': 200, 'maxWidth': 200, 'isResizable': true, isCollapsible: false }
                            ],
                            'buttons': [
                                { 'key': 'edit', 'text': '@GenEdiB@', 'icon': 'Edit', 'onSelect': true, primaryAction: 1, uievent: 'editorganismscopeculturetestoptionuievent', onFinish: 'embeddedrefresh' },
                                { 'key': 'delete', 'text': '@GenDelC@', 'icon': 'Delete', 'onSelect': true, primaryAction: 1, uievent: 'deleteorganismscopeculturetestoptionuievent', onFinish: 'embeddedrefresh' }
                            ]
                        }";

        var result = JsonConvert.DeserializeObject<ListViewConfig>(view);

        return result;
    }
}
