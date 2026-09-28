using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews;

/// <summary>
/// Configuration for the "Form Specimen Type Option" view.
/// </summary>
internal class FormSpecimenTypeOptionViewConfig
{
    /// <summary>
    /// Retrieves the configuration for the "Form Specimen Type Option" view.
    /// </summary>
    /// <remarks>
    /// The view lists, per laboratory, which specimen types a given request form offers. A form with no row
    /// here is unrestricted, so the list is empty until a laboratory chooses to narrow a form down.
    /// </remarks>
    /// <returns>
    /// A <see cref="ListViewConfig"/> object representing the list view configuration, including grid columns,
    /// buttons and query details.
    /// </returns>
    internal ListViewConfig GetView()
    {
        var view = @"{
                            'name': 'formspecimentypeoptionview',
                            'type': 'ManageList',
                            'title': '@ConFormSpe@',
                            'headerText': '@ConFormSpeA@.',
                            'queryName': 'formspecimentypeoptionlistquery',
                            'displaySummary': false,
                            'addbutton': 'addformspecimentypeoptionuievent',
                            'addButtonTooltip': '@ConFormSpeAdd@',
                            'DisplayIfEmpty':true,
                            'gridColumns': [
                                { 'key': 'column1', 'name': '@ConFormNam@', 'fieldName': 'GroupDescription', 'minWidth': 180, 'maxWidth': 180, 'isResizable': true, isCollapsible: false },
                                { 'key': 'menu', name: '', fieldName: '', minWidth: 120, maxWidth: 120, isResizable: true, isCollapsible: false },
                                { 'key': 'column2', 'name': '@SpeSpeB@', 'fieldName': 'AssociatedList', 'minWidth': 200, 'maxWidth': 200, 'isResizable': true, isCollapsible: false }
                            ],
                            'buttons': [
                                { 'key': 'edit', 'text': '@GenEdiB@', 'icon': 'Edit', 'onSelect': true, primaryAction: 1, uievent: 'editformspecimentypeoptionuievent', onFinish: 'embeddedrefresh' },
                                { 'key': 'delete', 'text': '@GenDelC@', 'icon': 'Delete', 'onSelect': true, primaryAction: 1, uievent: 'deleteformspecimentypeoptionuievent', onFinish: 'embeddedrefresh' }
                            ]
                        }";

        var result = JsonConvert.DeserializeObject<ListViewConfig>(view);

        return result;
    }
}
