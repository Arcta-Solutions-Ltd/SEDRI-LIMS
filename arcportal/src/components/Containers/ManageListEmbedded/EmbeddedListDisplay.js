// import {useState} from 'react';
// import {connect} from 'react-redux';
// import * as actionTypes from '../../../store/actions';
// import './ManageListEmbedded.css';
// import ListView from '../../General/ListView/ListView';
// import ErrorMessage from '../../General/ErrorMessage/ErrorMessage';
// import MapButtonsToContextMenu from '../../../Utils/Forms/MapButtonsToContextMenu';
// import ArraySorter from '../../../Utils/General/ArraySorter';
// import TranslateTag from '../../../Utils/Local/TranslateTag';
// import PageHeader from '../../Crafted/Config/FormDefinition/PageHeader/PageHeader';
// import FormHandler from '../FormHandler/FormHandler';

// const EmbeddedListDisplay = (props) => {

//     const [errorStatus, updateErrorStatus] = useState({visible: false, message: ''});
//     const [selectedRecord, setSelectedRecord] = useState({});
//     const [selectedIndex, setSelectedIndex] = useState({});
//     const [formStartConfig, setFormStartConfig] = useState({});
//     const [listVisibility, setListVisibility] = useState(true);

//     let displayType = (null);

//     const errorCloseHandler = () => {
//         updateErrorStatus({visible: false, message: ''});
//     }

//     const embeddedModeButtonHandler = (item, button) => {
//         itemSelected(item, true);
//         buttonClickHandler(button, item);
//     }

//     const buttonClickHandler = (button, item) => {
//         const id = item.id === undefined ? item.Id : item.id;
//         props.onButtonClick({ button: button, id: id, stateid: item.stateid, listdata: props.listData });
//     }

//     const selectionChangeHandler = (selectionState) => {
//         setSelectedRecord(selectionState.getSelection());
//         setSelectedIndex(selectionState.getSelectedIndices());
//     }

//     const itemSelected = (item, selected) => {
//         if (selected) {
//             var arrayOfSelectedItems = [];
//             arrayOfSelectedItems[0] = item;
//             setSelectedRecord(arrayOfSelectedItems);
//         }
//     }

//     let displayEditButton = false; 
//     if (props.config.EditButton !== undefined && props.config.EditButton !== "") {
//         const founduievent = props.uievents.filter(e => e.Name === props.config.EditButton );
//         displayEditButton = founduievent.length > 0;
//     }

//     let displayDeleteButton = false; 
//     if (props.config.DeleteButton !== undefined && props.config.DeleteButton !== "") {
//         const founduievent = props.uievents.filter(e => e.Name === props.config.DeleteButton );
//         displayDeleteButton = founduievent.length > 0;
//     }
//     const page = { pageTitle: props.title };

//     const refreshAfterReturningFromForm = () => {
//         props.onRefreshDone("embeddedrefresh" );
//     }

//     const editClickHandler = () => {
//         setFormStartConfig({
//             button: {UIEvent: props.config.EditButton, onFinish: 'refresh'}, 
//             refresh: refreshAfterReturningFromForm,
//             containerVisibility: setListVisibility
//         })
//     }

//     const deleteClickHandler = () => {
//         setFormStartConfig({
//             button: {UIEvent: props.config.DeleteButton, onFinish: 'refresh'}, 
//             refresh: refreshAfterReturningFromForm,
//             containerVisibility: setListVisibility
//         })
//     }

//     const menuButtons =  props.config.Buttons.filter((button) => { return button.OnSelect; });
//     let unsortedMenuItems = MapButtonsToContextMenu(menuButtons, buttonClickHandler);
//     const menuItems = unsortedMenuItems.sort(ArraySorter("primaryAction"));

//     let columns = props.config.GridColumns.map((column) => {
//         return { Key: column.Key,
//                  Name: column.Name,
//                  FieldName: column.FieldName,
//                  MinWidth: column.MinWidth,
//                  MaxWidth: column.MaxWidth,
//                  IsResizable: column.IsResizable,
//                  IsCollapsible: column.IsCollapsible,
//                  IsSorted: column.FieldName === false,
//                  IsSortedDescending: column.FieldName === false,
//                  Highlight: column.Highlight
//                }
//     })

//     if (props.passive) {
//         columns = columns.filter(obj => obj.Key !== 'menu');
//     }

//     // Don't display at all if there's no data.
//     if (props.listData.length > 0 || props.config.DisplayIfEmpty === true) {
//         displayType =
//             <ListView
//                 type={"grid"}
//                 parentId={props.itemId}
//                 columns={columns}
//                 listData={props.listData}
//                 menuItems={menuItems}
//                 itemSelected={itemSelected}
//                 selectedRecord={selectedIndex}
//                 isDataLoaded={true}                
//                 selectionChanged={selectionChangeHandler}
//                 basic={true}
//                 refresh={props.refresh}
//                 displaySummary={props.config.DisplaySummary}
//                 addButton={props.config.AddButton}
//                 basicModeButtonHandler={embeddedModeButtonHandler}
//                 laboratoryConfig={props.laboratory}
//                 onClick={buttonClickHandler}>
//             </ListView>
//     } else {
//         displayType = (
//             <div className='managelistembedded-no-list'>
//                 ----- {TranslateTag("@GenNon@", props.language)} -----
//             </div>
//         );
//     }

//     return (
//         <div>
//             {props.title !== undefined && props.title !== '' ? (
//                 <PageHeader page={page} canEdit={displayEditButton} canDelete={displayDeleteButton} edit={editClickHandler} delete={deleteClickHandler}></PageHeader>
//             ) : (null)}
//             <div>

//                 {displayType}
//                 <FormHandler startConfig={formStartConfig} showNextButton={false}></FormHandler>
//                 <ErrorMessage visible={errorStatus.visible} dismissHandler={errorCloseHandler} error={errorStatus.message}></ErrorMessage>
//             </div>
//         </div>
//     )
// };

// const mapStateToProps = state => {
//     return {
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

// export default connect(mapStateToProps, mapDispatchToProps)(EmbeddedListDisplay);
