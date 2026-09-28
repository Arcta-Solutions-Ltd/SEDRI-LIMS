using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews;

internal class InstrumentConfigListViewConfig
{
    /// <summary>
    /// Retrieves the ListView configuration.
    /// </summary>
    /// <returns>A ListViewConfig object representing the view configuration.</returns>
    internal ListViewConfig GetView()
    {
        var view = @"{
                        name: 'instrumentconfig',
                        type: 'ManageList',
                        title: '@InsMan@',
                        headerText: '@InsManA@.',
                        queryName: 'InstrumentProfileListQuery',
                        singleQuery: 'singleinstrumentprofilelistquery',
                        itemType: 'specimen',
                        gridColumns:
                            [
                                { key: 'column1', name: '@InsProNam@', fieldName: 'InstrumentName', minWidth: 130, maxWidth: 130, isResizable: true, isCollapsible: false, isSorted: true, isSortedDescending: true },
                                { key: 'menu', name: '', fieldName: '', minWidth: 130, maxWidth: 130, isResizable: true, isCollapsible: false },
                                { key: 'columnInstrumentMachine', name: '@GenIns@', fieldName: 'InstrumentMachine', minWidth: 130, maxWidth: 180, isResizable: true, isCollapsible: false },
                                { key: 'column2', name: '@SpeSpeB@', fieldName: 'SpecimenType', minWidth: 130, maxWidth: 130, isResizable: true, isCollapsible: false },
                                { key: 'column3', name: '@CulTyp@', fieldName: 'CultureType', minWidth: 130, maxWidth: 130, isResizable: true, isCollapsible: false},
                                { key: 'column4', name: '@ConDirA@', fieldName: 'DirectTest', minWidth: 130, maxWidth: 130, isResizable: true, isCollapsible: false },
                                { key: 'column5', name: '@SpeCulC@', fieldName: 'CultureTest', minWidth: 130, maxWidth: 130, isResizable: true, isCollapsible: false },
                                { key: 'column6', name: '@GenCodB@', fieldName: 'OrganismGroup', minWidth: 130, maxWidth: 130, isResizable: true, isCollapsible: false }
                            ],
                        buttons:
                            [
                                { key: 'addinstrumentprofile', text: '@InsAdd@', icon: 'Add', onSelect: false, uievent: 'addinstrumentprofileuievent', onFinish: 'refresh' },
                                { key: 'editinstrumentprofile', text: '@InsEdi@', icon: 'Edit', onSelect: true, primaryAction: 1, uievent: 'editinstrumentprofileuievent', onFinish: 'update' },
                                { key: 'deleteinstrumentprofile', text: '@InsDel@', icon: 'Delete', onSelect: true, primaryAction: 2, uievent: 'deleteinstrumentprofileuievent', onFinish: 'refresh' }
                            ],
                        filterSearch: true,
                        searchFields: [ 'searchText' ]
                }";

        var result = JsonConvert.DeserializeObject<ListViewConfig>(view);

        return result;
    }
}
