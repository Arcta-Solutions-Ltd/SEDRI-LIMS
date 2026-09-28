//using arc.domain.Configuration.ViewConfig.ListViewConfig;
//using Newtonsoft.Json;

//namespace arc.app.Config.Views.ListViews
//{
//    internal class SpecimenAlertListViewConfig
//    {
//        internal ListViewConfig GetView()
//        {
//            var view = @"{
//                            'name': 'alerts',
//                            'type': 'ManageList',
//                            'title': '@AleAleD@',
//                            'headerText': '@AleSpe@.',
//                            'queryName': 'SpecimenAlertList',
//                            'gridColumns': [
//                                { 'key': 'column1', 'name': '@SpeAcc@', 'fieldName': 'AccessionNumber', 'minWidth': 200, 'maxWidth': 400, 'isResizable': true },
//                                { 'key': 'column2', 'name': '@AleAle@', 'fieldName': 'AlertName', 'minWidth': 200, 'maxWidth': 400, 'isResizable': true },
//                                { 'key': 'column3', 'name': '@PatFir@', 'fieldName': 'FirstName', 'minWidth': 200, 'maxWidth': 400, 'isResizable': true },
//                                { 'key': 'column3', 'name': '@PatSurA@', 'fieldName': 'Surname', 'minWidth': 200, 'maxWidth': 400, 'isResizable': true },
//                                { 'key': 'column3', 'name': '@SpeRecD@', 'fieldName': 'ReceivedDate', 'minWidth': 200, 'maxWidth': 400, 'isResizable': true },
//                                { 'key': 'column3', 'name': '@GenSta@', 'fieldName': 'State', 'minWidth': 200, 'maxWidth': 400, 'isResizable': true }
//                            ],
//                            'buttons': [
//                            ],
//                            filterSearch: true,
//                            searchFields: [ 'searchText' ]
//                        }";

//            var result = JsonConvert.DeserializeObject<ListViewConfig>(view);

//            return result;
//        }
//    }
//}
