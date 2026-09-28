using arc.domain.Configuration.ViewConfig.RecordViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.RecordViews
{
    /// <summary>
    /// Defines the Export Profile record view, including the embedded Field Order, Run Export and Manage Mapping buttons,
    /// and the regions that show the profile, its fields, and its schedules.
    /// </summary>
    internal class ExportProfileRecordViewConfig
    {
        /// <summary>
        /// Returns the deserialised <see cref="RecordViewConfig"/> for the Export Profile record view.
        /// </summary>
        internal RecordViewConfig GetView()
        {

            var view = @"{
                            'title': '@ExpProRec@',
                            'name': 'exportprofile',
                            'type': 'recordview',
                            'singleItemName': '@ExpProRec@',
                             buttons:
                                [
                                 { key: 'editfields', text: '@ExpProOrderFie@', icon: 'ChevronUnfold10', uievent: 'editexportprofilefieldsuievent', onFinish: 'refresh'},
                                 { key: 'runexport', text: '@RunExpT@', icon: 'Play', uievent: 'runexportuievent', onFinish: 'refresh'},
                                 { key: 'manageexportprofilemapping', text: '@ExpProMap@', icon: 'Mapping', uievent: 'manageexportprofilemappinguievent', onFinish: 'refresh'}
                                ],
                            'regions': [
                                { 'id': 'exportprofile',
                                  'type': 'standard',
                                  'queryName': 'exportprofileview',
                                },
                                {
                                  'id': 'exportprofilefield',
                                  'type': 'listview',
                                  'title': '@ExpProField@',
                                  'listViewName': 'exportprofilefield'
                                },
                                {
                                  'id': 'exportschedules',
                                  'type': 'listview',
                                  'title': '@ExpSch@',
                                  'listViewName': 'exportschedules'
                                }

                            ]
                         }";


            var result = JsonConvert.DeserializeObject<RecordViewConfig>(view);

            return result;
        }
    }
}
