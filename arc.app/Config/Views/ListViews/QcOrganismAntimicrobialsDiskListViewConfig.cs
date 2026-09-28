using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews
{
    internal class QcOrganismAntimicrobialsDiskListViewConfig
    {
        internal ListViewConfig GetView()
        {
            var view = @"{
                                name: 'qcorganismantimicrobialsdisklist',
                                type: 'ManageList',
                                title: '@ManQcAnt@',
                                queryName: 'iqctestprofileqcantibioticsdisk',
                                parentId: 'iqctestprofileqcorganismid',
                                gridColumns:
                                    [
                                        { key: 'column1', name: '@ManQcOrgNam@', fieldName: 'Name', minWidth: 120, maxWidth: 200, isResizable: true  },
                                        { key: 'column2', name: '@GenEna@', fieldName: 'Enabled', minWidth: 80, maxWidth: 160, isResizable: true },
                                        { key: 'column3', name: '@QuaCon@', fieldName: 'Content', minWidth: 80, maxWidth: 160, isResizable: true },
                                        { key: 'column4', name: '@QuaRanLow@', fieldName: 'RangeLower', minWidth: 80, maxWidth: 160, isResizable: true },
                                        { key: 'column5', name: '@QuaRanUpp@', fieldName: 'RangeUpper', minWidth: 80, maxWidth: 160, isResizable: true },
                                        { key: 'column6', name: '@QuaTarLow@', fieldName: 'TargetLower', minWidth: 80, maxWidth: 160, isResizable: true },
                                        { key: 'column7', name: '@QuaTarUpp@', fieldName: 'TargetUpper', minWidth: 80, maxWidth: 160, isResizable: true }
                                    ],
                                buttons: []
                        }";

            return JsonConvert.DeserializeObject<ListViewConfig>(view);
        }
    }
}
