using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews;

internal class OrganisationListViewConfig
{
    internal ListViewConfig GetView()
    {
        var view = @"{
                        'name': 'organisations',
                        'type': 'ManageList',
                        'parentId': 'parentorganisationid',
                        'title': '@OrgMan@',
                        'headerText': '@OrgCre@.',
                        'queryName': 'OrganisationList',
                        'singleQuery': 'SingleOrganisationForOrganisationList',
                        'gridColumns': [
                            { 'key': 'column1', 'name': '@OrgOrg@', 'fieldName': 'organisationname', 'minWidth': 80, 'maxWidth': 400, 'isResizable': true },
                            { 'key': 'menu', 'name': '', 'fieldName': '', 'minWidth': 80, 'maxWidth': 120, 'isResizable': true },
                            { 'key': 'column2', 'name': '@GenCodA@', 'fieldName': 'code', 'minWidth': 80, 'maxWidth': 120, 'isResizable': true },
                            { 'key': 'column4', 'name': '@GenEna@', 'fieldName': 'enabled', 'minWidth': 80, 'maxWidth': 120, 'isResizable': true }
                        ],
                        'buttons': [
                            { 'key': 'addorganisation', 'text': '@OrgAddB@', 'icon': 'Add', 'onSelect': false, uievent: 'addorganisationuievent', onFinish: 'refresh' },
                            { 'key': 'addchildorganisation', 'text': '@OrgAddChild@', 'icon': 'Add', 'onSelect': true, 'primaryAction': 1, uievent: 'addorganisationuievent', onFinish: 'refresh', addChildContext: true, prefillFormFields: { 'ParentOrganisationId': 'id', 'ParentOrganisation': 'organisationname' } },
                            { 'key': 'editorganisation', 'text': '@OrgEdiC@', 'icon': 'Edit', 'onSelect': true, 'primaryAction': 1, uievent: 'editorganisationuievent', onFinish: 'refresh' },
                            { 'key': 'deleteorganisation', 'text': '@OrgDelC@', 'icon': 'Delete', 'onSelect': true, 'primaryAction': 2, uievent: 'deleteorganisationuievent', onFinish: 'refresh' }
                        ],
                        filterSearch: true,
                        searchFields: [ 'organisationname', 'code', 'fullyqualifiedname' ]
                    }";

        var result = JsonConvert.DeserializeObject<ListViewConfig>(view);

        return result;
    }
}
