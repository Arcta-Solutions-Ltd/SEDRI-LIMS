import React, {useState} from 'react';
import {connect} from 'react-redux';
import TranslateTag from '../../../../Utils/Local/TranslateTag';
import MapButtonsToContextMenu from '../../../../Utils/Forms/MapButtonsToContextMenu';
import ListView from '../../../General/ListView/ListView';
import FormHandler from '../../../Containers/FormHandler/FormHandler';
import { IconButton, TooltipHost } from '@fluentui/react';
import { getStandardTooltipProps } from '../../../../Utils/General/StandardTooltipProps';

const ReportDefinition = (props) => {

    const [formStartConfig, setFormStartConfig] = useState({});
    const [listVisibility, setListVisibility] = useState(true);

    const CreateSectionList = (sections) => {
        let sectionList = [];
        for (let x = 0; x < sections.length; x++) {
            const lineData = sections[x];
           sectionList.push({Id: lineData.id, Section: lineData.section })
        }
        return sectionList;
    }

    const displayList = [];
    if (props.data !== undefined && props.data.length > 0) {
        displayList.push({ sectionType: "top", pageTitle: TranslateTag("@ConTop@", props.language), sections: CreateSectionList(props.data[0].MainSections)});
        displayList.push({ sectionType: "organism", pageTitle: TranslateTag("@ConOrg@", props.language), sections: CreateSectionList(props.data[0].OrganismSections)});
        displayList.push({ sectionType: "bottom", pageTitle: TranslateTag("@ConBot@", props.language), sections: CreateSectionList(props.data[0].FinalSections)});
    } 

    const refreshAfterReturningFromForm = () => {
        props.onRefresh("formdefinitionrefresh" );
    }

    const setFormConfig = (button, recordId, record) => {
        setFormStartConfig({
            button: button, 
            id: recordId, 
            refresh: refreshAfterReturningFromForm,
            containerVisibility: setListVisibility
        })
    }

    const buttonClickHandler = (button, item, index) => {
        const id = item.id === undefined ? item.Id : item.id;
        setFormConfig(button, id )
    }

    const itemSelected = (item, selected) => {
        if (selected) {
            var arrayOfSelectedItems = [];
            arrayOfSelectedItems[0] = item;
        }
    }

    const selectionChangeHandler = () => { }

    const embeddedModeButtonHandler = (item, button) => {
        itemSelected(item, true);
        buttonClickHandler(button, item);
    }

    const editClickHandler = (section) => {
        setFormStartConfig({
            button: {UIEvent: "editreportsectionuievent", onFinish: 'refresh'}, 
            id: props.data[0].Name + "|" + section.sectionType, 
            refresh: refreshAfterReturningFromForm,
            containerVisibility: setListVisibility
        })
    }

    const menuButtons = [
        {Icon: "Edit", Key: "editsection", PrimaryAction: 4, Text: TranslateTag("@GenEdiB@",props.language), UIEvent: "editsectionuievent"}
    ];

    // if (Array.isArray(props.data) && props.data.length > 0 && props.data[0].Name !== "DefaultSpecimenReport") {
        menuButtons.push({Icon: "Delete", Key: "deletesection", PrimaryAction: 5, Text: TranslateTag("@GenDelC@",props.language), UIEvent: "deletesectionuievent", OnFinish: "refresh" })
    // };
    let addButton = "addsectionuievent";
    
    let menuItems = MapButtonsToContextMenu(menuButtons, buttonClickHandler, 'reportdefinition');

    // if (Array.isArray(props.data) && props.data.length > 0 && props.data[0].Name === "DefaultSpecimenReport") {
    //     addButton = "";
    // };

    const columns = [
        { Key: 'column1', Name: TranslateTag("@GenSec@",props.language), FieldName: 'Section', MinWidth: 180, MaxWidth: 180, IsResizable: true, IsCollapsible: false },
        { Key: 'menu', Name: '', FieldName: '', MinWidth: 120, MaxWidth: 120, IsResizable: true, IsCollapsible: false }
    ];

    // Display list controls.
    const listContentCss = listVisibility ? "" : " app-invisible";

    const reportName = Array.isArray(props.data) && props.data[0] !== undefined ? props.data[0].Name : "";

    const forms = props.forms.filter(f => f.Name === "editreportsectionform");
    const displayEditButton = forms.length > 0;
    const editSectionTooltip = TranslateTag('@GenEdiB@', props.language);

    return (
        <div className='directtestsgrid-content'>
            {displayList.map((page) => {
                return (
                    <div className={listContentCss}>
                        <div className='managerecord-sectiontitle'>
                            <span>
                                {page.pageTitle}
                                <span class="formdefinition-menuposition">
                                    {displayEditButton && (
                                        <TooltipHost content={editSectionTooltip} tooltipProps={getStandardTooltipProps()} calloutProps={{ gapSpace: 10 }}>
                                            <IconButton
                                                id={`reportdefinition-edit-${page.sectionType}`}
                                                iconProps={{ iconName: 'Edit' }}
                                                ariaLabel={editSectionTooltip}
                                                onClick={(event) => editClickHandler(page)}
                                                styles={{
                                                    root: { height: '18px', verticalAlign: 'middle'}
                                                }}
                                            />
                                        </TooltipHost>
                                    )}
                                </span>
                            </span>
                        </div>
                        <div className='managerecord-section-table'>
                            <ListView 
                                type={"grid"}
                                parentId={reportName + "|" + page.pageTitle}
                                refresh={props.refresh}
                                columns={columns}
                                listData={page.sections}
                                menuItems={menuItems}
                                itemSelected={itemSelected}
                                selectionChanged={selectionChangeHandler}
                                basicModeButtonHandler={embeddedModeButtonHandler}
                                onClick={buttonClickHandler}
                                isDataLoaded={true}
                                addButton={addButton}
                                addButtonTooltip="@ConAddN@"
                                viewName="reportdefinition"
                                basic={true}
                                displaySummary={true}
                                language={props.language}>
                            </ListView>
                        </div>
                    </div>
            )})}
            <FormHandler startConfig={formStartConfig} showNextButton={false}></FormHandler>
        </div>
    )
}

const mapStateToProps = state => {
    return {
        forms: state.config.forms,
        language: state.config.language,
    };
}

export default connect(mapStateToProps)(ReportDefinition);