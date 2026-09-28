import React, {useState, useEffect} from 'react';
import './Wizard.css';
import { Pivot, PivotItem } from '@fluentui/react';
import FormColumn from '../FormColumn/FormColumn';
import ExtractFieldListFromPageStructure from '../../../Utils/PageStructure/ExtractFieldListFromPageStructure';
import CreateJsonFromFieldList from '../../../Utils/PageStructure/CreateJsonFromFieldList';
import EvaluateRules from '../../../Utils/Rules/EvaluateRules';
import AddValuesIntoPageStructure from '../../../Utils/PageStructure/AddValuesIntoPageStructure';
import AddButtonsIntoPageStructure from '../../../Utils/PageStructure/AddButtonsIntoPageStructure';
import AddEmptyButtonArrays from '../../../Utils/PageStructure/AddEmptyButtonArrays';
import Post from '../../../Data/Post';
import ErrorMessage from '../../General/ErrorMessage/ErrorMessage';

const Wizard = (props) => {

    const [pageStructure, updatePageStructure] = useState([]);
    const [pageSelectedKey, updatePageSelectedKey] = useState("");
    const [errorStatus, updateErrorStatus] = useState({visible: false, message: ''});
 
    useEffect(() => {
        const listOfFields = ExtractFieldListFromPageStructure(props.config.pages);
        const pageStructure = AddValuesIntoPageStructure(props.config.pages,listOfFields);
        updatePageSelectedKey(props.config.pages[0].pageTitle);
        updatePageStructure(AddEmptyButtonArrays(pageStructure));
    },[props.config])

    const changeHandler = (id, value) => {
        var newValue = [{key: id, value: value}];
        const newPageStructure = AddValuesIntoPageStructure(pageStructure,newValue);
        updatePageStructure(AddButtonsIntoPageStructure(newPageStructure,navButtonClickHandler, finishButtonClickHandler));
    }

    const navButtonClickHandler = (nextPage) => {
        updatePageSelectedKey(nextPage.Name);
    }

    const finishButtonClickHandler = () => {
        var fieldList = ExtractFieldListFromPageStructure(props.config.pages);
        var jsonToSave = CreateJsonFromFieldList(fieldList);
        Post(props.config.endpoint,jsonToSave,dataSavedSuccessfully, errorWhenSavingData )
    }

    const errorCloseHandler = () => {
        updateErrorStatus({visible: false, message: ''});
    }

    const dataSavedSuccessfully = () => {
        props.closeWindowHandler();
    }

    const errorWhenSavingData = (response) => {
        updateErrorStatus({visible: true, message: response.data});
    }

    // Set display classes

    let contentClass = "wizard-content";
    let pivotItemClass = "";
    if (props.config.columns === 2) {
        contentClass = "wizard-2col-content";
        pivotItemClass = "wizard-2col-pivotitems";
    }

    return (
        <div className={contentClass}>
            <Pivot selectedKey={pageSelectedKey}>
                {pageStructure.map((page) => {
                    const isVisible = EvaluateRules("visible", page.Rules, pageStructure);
                    if (isVisible) {
                        return (
                            <PivotItem key={page.pageTitle} headerText={page.pageTitle} itemKey={page.pageTitle}>
                                <div className="wizard-pivot">
                                    <div className={pivotItemClass}>
                                        {page.Columns.map((column) => {
                                            return (
                                                <FormColumn config={column} changeHandler={changeHandler} pageStructure={pageStructure}></FormColumn>
                                            )
                                        })}
                                    </div>
                                    <div>
                                        <ErrorMessage visible={errorStatus.visible} dismissHandler={errorCloseHandler} error={errorStatus.message}></ErrorMessage>
                                        <div className="wizard-nextbutton">
                                            {page.buttons.map((button) => {
                                                return ( button )
                                            })}
                                        </div>
                                    </div>
                                </div>
                            </PivotItem>
                        )
                    } else {
                        return (null)
                    }
                })}
            </Pivot>
        </div>
    )
};
  
export default Wizard;