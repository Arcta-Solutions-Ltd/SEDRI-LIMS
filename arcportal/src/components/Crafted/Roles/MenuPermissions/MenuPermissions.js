import React, { useState, useEffect } from 'react';
import './MenuPermissions.css';
import TextDisplay from '../../../Forms/TextDisplay/TextDisplay';
import ArcToggle from '../../../Forms/ArcToggle/ArcToggle';
import SingleLineField from '../../../Forms/SingleLineField/SingleLineField';
import { isMandatorySidebarKey } from './menuPermissionKeys';

const MenuPermissions = (props) => {

    const [selectedKey, setSelectedKey] = useState();

    useEffect(() => {

        var id = props.data.length !== 0 ? props.data[0].Key : "";
        if (id !== undefined && id !== null) {
            let doc = document.getElementById(id);
            if (doc !== undefined && doc !== null) {
                doc.focus();
            }
        }

        // const categoryList = getCategoryList();

        // if (props.categorySelection === undefined) {
        //     if (categoryList.length > 0) 
        //         setSelectedKey(optionList[0].key.toString());
        // } else {
        //     setSelectedKey(props.categorySelection);
        // }
    
    }, []);

    useEffect(() => {
        const categoryList = getCategoryList();
        if (props.categorySelection === undefined || props.categorySelection === "" || props.categorySelection === null) {
            if (categoryList.length > 0) 
                setSelectedKey(optionList[0].key.toString());
        } else {
            setSelectedKey(props.categorySelection);
        }
    }, [props.categorySelection, props.categoryName]);

    const getCategoryList = () => {
        let testListToDisplay = props.data.filter(item => item.Key !== "XCategX" && item.Display === "Yes" );

        let returnList = [];
        const categoryItem = props.data.find(i => i.Key === "XCategX");
        if (Array.isArray(categoryItem?.Categories) && categoryItem.Categories.length > 0) {
           returnList = categoryItem.Categories
                .map((item, index) => ({ key: item.CategoryId, text: item.Category }))
                .filter(option => filterTestListByCategory(testListToDisplay, option.key, "Yes").length > 0);
         }

         return returnList;
    }

    const filterTestListByCategory = (testlist, key, displayCondition) => {
        if (key === undefined) {
            return testlist
        } else {
            const categoryItem = props.data.find(i => i.Key === "XCategX");
            if (!categoryItem?.Categories) return [];

            const keyList = key.split(",").map(k => k.trim()); 

            return testlist.filter(item => 
                keyList.some(k => categoryItem.Categories.find((c) => c.CategoryId === k)?.Values.includes(item.Key)) &&
                item.Display === displayCondition
            );
        }
    };

    const data = props.data;

    let testListToDisplay = props.data.filter(item => item.Key !== "XCategX" && (item.Display === "Yes" || item.Display === undefined));
    const optionList = getCategoryList();
    const hasDropdown = optionList.length > 1;
    if (optionList.length > 0) {
        testListToDisplay = filterTestListByCategory(testListToDisplay, selectedKey, "Yes");
    }
    
    const categoryChangeHandler = (event, value) => { 
        setSelectedKey(value);
        let testList = props.data.filter(item => item.Key !== "XCategX" && item.Display === "Yes");
        testListToDisplay = filterTestListByCategory(testList, value, "Yes");
    }

    const valueChangeHandler = (id, value) => {
        if (isMandatorySidebarKey(id)) {
            return;
        }

        const itemToChange = data.filter((item) => item.Key === id)[0];
        itemToChange.Allowed = value;

        const returnValue = {
            Allowed: value,
            Key: id,
            Name: itemToChange.Name,
        };
        props.changeHandler(id, returnValue);
    };

    const categoryConfig = { Id: "categoryList", Type: 'combobox', MultiSelect: true, Options: optionList, value: selectedKey};

    return (
        <div className="app-crafted-content">
            <div className="app-crafted-title">{props.config.PageTitle}</div>
            <div className="app-crafted-headertext">
                <TextDisplay text={props.config.Text}></TextDisplay>
            </div>
            <div className="menupermissions-content" data-testid="menupermissions-list">
                {hasDropdown && <div className="menupermissions-data">
                    <SingleLineField key="catlist" config={categoryConfig} changeHandler={categoryChangeHandler} onKeyDown={props.onKeyDown}></SingleLineField>
                </div>}
                {testListToDisplay.map((item, index) => {
                    const isMandatory = isMandatorySidebarKey(item.Key);
                    const config = {
                        Label: item.Name,
                        value: isMandatory ? 'Yes' : item.Allowed,
                        id: item.Key,
                        Disabled: isMandatory,
                    };
                    return (
                        <div key={index} className="menupermissions-item" data-testid={`test-option-${item.Key}`}>
                            <ArcToggle
                                config={config}
                                valueChangeHandler={valueChangeHandler}
                                showText={false}
                                onKeyDown={props.onKeyDown}
                            ></ArcToggle>
                        </div>
                    );
                })}
            </div>
        </div>
    );
};

export default MenuPermissions;
