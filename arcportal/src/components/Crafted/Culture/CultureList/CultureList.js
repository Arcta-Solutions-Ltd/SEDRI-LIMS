// import {useState} from 'react';
// import { connect } from 'react-redux';
// import Post from '../../../../Data/Post';
// import TransformDatesInJson from '../../../../Utils/Local/TransformDatesInJson';
// import EmbeddedListDisplay from '../../../Containers/ManageListEmbedded/EmbeddedListDisplay';
// import FormHandler from '../../../Containers/FormHandler/FormHandler';
// import ErrorMessage from '../../../General/ErrorMessage/ErrorMessage';
// import { useRunOnce } from '../../../../Utils/General/UseRunOnce';

// const CultureList = (props) => {

//     const [cultureListData, setCultureListData] = useState([]);
//     const [errorStatus, updateErrorStatus] = useState({visible: false, message: ''});
//     const [formStartConfig, setFormStartConfig] = useState({});

//     useRunOnce(() => {
//         const viewConfig = props.views.find((view) => { return view.Name === props.config.ListViewName; });
//         const parameters = [ { key: "specimenid", value: props.id } ];
//         const criteria = { Name: viewConfig.QueryName, Parameters: parameters};
//         Post('query/filteredget', criteria, dataReceivedHandler, errorWhenRetrievingData);
//     });

//     const errorCloseHandler = () => {
//         updateErrorStatus({visible: false, message: ''});
//     }

//     const dataReceivedHandler = (data) => {
//         TransformDatesInJson(data);
          
//         const newListData = data.map(item => ({ ...item}));
//         setCultureListData(newListData);
//     }

//     const errorWhenRetrievingData = (response) => {
//         updateErrorStatus({visible: true, message: response.data});
//     }

//     const getListView = (listViewName) => {
//         return props.views.filter((view) => {
//             return view.Name === listViewName;
//         })[0];
//     }

//     return (
//         <div className='directtestgrid-content'>
//             {cultureListData.map((region, index) => {
//                     const displayData = [];
//                     displayData[0] = region;
//                     region.ListView = getListView(props.config.ListViewName);
//                     return (
//                         <div key={region.Id}>
//                             <div>
//                                 <EmbeddedListDisplay
//                                     config={region.ListView}
//                                     parentId={region.ListView?.ParentId}
//                                     itemId={props.id}
//                                     stateid={props.stateid} 
//                                     toggleFullScreen={false}
//                                     region={index}
//                                     refresh={props.refresh}
//                                     passive={props.passive}
//                                     containerVisibility={props.containerVisibility}
//                                     onButtonClick={props.onButtonClick}
//                                     onRefreshDone={props.onRefreshDone}
//                                     title={region.Type}
//                                     selectedRecord={props.selectedRecord}
//                                     listData={displayData}
//                                 >
//                                 </EmbeddedListDisplay>
//                             </div>
//                         </div>
//                     )
//                 })
//             }

//             <FormHandler startConfig={formStartConfig} showNextButton={false}></FormHandler>
//             <ErrorMessage visible={errorStatus.visible} dismissHandler={errorCloseHandler} error={errorStatus.message}></ErrorMessage>
//         </div>
//     )
// };

// const mapStateToProps = state => {
//     return {
//         views: state.config.views,
//         uievents: state.config.uievents,
//         forms: state.config.forms,
//         pages: state.config.pages,
//         lists: state.config.lists,
//         language: state.config.language,
//         laboratory: state.config.laboratory,
//         showFullScreen: state.display.showFullScreen
//     };
// }

// const mapDispatchToProps = dispatch => {
//     return {
//         onFilterSelect: (value) => dispatch({type: actionTypes.SETFILTERPRESET, value: value})
//     }
// };

// export default connect(mapStateToProps, mapDispatchToProps)(CultureList);