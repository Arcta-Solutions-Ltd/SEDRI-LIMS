import React, {useState} from 'react';
import {connect} from 'react-redux';
import TranslateTag from '../../../../Utils/Local/TranslateTag';
import MapButtonsToContextMenu from '../../../../Utils/Forms/MapButtonsToContextMenu';
import FormDefinitionSection from './FormDefinitionSection/FormDefinitionSection';
import FormGroupHeader from './FormGroupHeader/FormGroupHeader';
import FormHandler from '../../../Containers/FormHandler/FormHandler';
import './FormDefinition.css';
import PageHeader from './PageHeader/PageHeader';

/**
 * Form Definition component that displays the Define Page Contents screen.
 * Renders a three-level hierarchy: Page → Form Groups → Fields.
 *
 * displayList structure: Array of { pageTitle, id, formGroups, configureActions, canReuseFields }
 * Each formGroups item: { formGroupKey, columnKey, fields, id, addExistingFieldId, addFieldDomId,
 * addExistingDomId }
 * Each fields item: { Id, DisplayId, Label, Type, stateid }
 *
 * addExistingFieldId carries the view config id as a fifth segment, because the add existing field
 * query needs the view to work out which other forms can contribute fields. The two DomId values are
 * distinct from it: they address the header icons in the DOM and are unique per form group.
 *
 * The add field and add existing field actions render as icons on each form group's grid header, and
 * add form group renders as an icon in the page header, so every configuration action on this screen
 * is presented the same way.
 */
const FormDefinition = (props) => {

    const [selectedIndex, setSelectedIndex] = useState({});
    const [formStartConfig, setFormStartConfig] = useState({});
    const [listVisibility, setListVisibility] = useState(true);

    const displayList = [];
    let form = "";
    let showTableName = false;
    let viewId = "";

    if (props.data !== undefined && props.data.length > 0) {
        var pages = props.data[0].pages;
        var keys = Object.keys(pages);
        form = props.data[0].form;
        viewId = props.data[0].viewId ?? props.data[0].ViewId ?? "";
        const singleItemName = props.data[0].singleItemName ?? props.data[0].SingleItemName;
        showTableName = typeof singleItemName === 'string' && singleItemName.toLowerCase() === 'specimen';

        for (let x = 0; x < keys.length; x++) {
            const page = pages[x];

            const formGroups = [];
            if (page.Columns !== undefined) {
                for (const column of page.Columns) {
                    for (const formGroup of column.FormGroups) {
                        const fieldList = [];
                        for (const field of formGroup.Fields) {
                            const otherDetailsFor = field.OtherDetailsFor ?? field.otherDetailsFor;
                            if (otherDetailsFor) {
                                continue;
                            }
                            let stateId = field.Configurable === "No" ? "none" : "canconfig";
                            if (stateId == "canconfig") {
                                const editForm = props.forms.filter(f => f.Name === "editfieldform");
                                stateId = editForm.length > 0 ? stateId : "candelete";
                                const deleteForm = props.forms.filter(f => f.Name === "deletefieldform");
                                stateId = deleteForm.length > 0 ? stateId : stateId === "canconfig" ? "canedit" : "none";
                            }
                            const allowOther = field.AllowOther === true || field.allowOther === true;
                            const typeLabel = allowOther ? `${field.Type} (Other option)` : field.Type;
                            fieldList.push({Id: form + "|" + page.Name + "|" + field.Id, DisplayId: field.Id, Label: field.Label, Type: typeLabel, stateid: stateId});
                        }
                        const formGroupId = form + "|" + page.Name + "|" + column.Key + "|" + formGroup.Key;
                        formGroups.push({
                            formGroupKey: formGroup.Key,
                            columnKey: column.Key,
                            fields: fieldList,
                            id: formGroupId,
                            addExistingFieldId: formGroupId + "|" + viewId,
                            addFieldDomId: `formdefinition-addfield-${page.Name}-${column.Key}-${formGroup.Key}`,
                            addExistingDomId: `formdefinition-addexistingfield-${page.Name}-${column.Key}-${formGroup.Key}`
                        });
                    }
                }
            }

            const newDisplay = {
                pageTitle: page.PageTitle,
                id: page.Name,
                formGroups,
                configureActions: page.ConfigureActions,
                canReuseFields: page.canReuseFields === true,
                showTableName,
                tableName: showTableName ? (page.TableName ?? page.tableName) : undefined
            };
            displayList.push(newDisplay);
        }
    } 

    const refreshAfterReturningFromForm = () => {
        props.onRefresh("formdefinitionrefresh" );
    }

    const setFormConfig = (button, recordId) => {
        setFormStartConfig({
            button: button, 
            id: recordId, 
            refresh: refreshAfterReturningFromForm,
            containerVisibility: setListVisibility
        })
    }

    const buttonClickHandler = (button, item) => {
        const id = item.id === undefined ? item.Id : item.id;
        setFormConfig(button, id )
    }

    const selectionChangeHandler = (selectionState) => {
        setSelectedIndex(selectionState.getSelectedIndices());
    }

    const itemSelected = (item, selected) => {
        if (selected) {
            var arrayOfSelectedItems = [];
            arrayOfSelectedItems[0] = item;
        }
    }

    const embeddedModeButtonHandler = (item, button) => {
        itemSelected(item, true);
        buttonClickHandler(button, item);
    }

    const moveFieldForm = props.forms.filter(f => f.Name === "movefieldform");
    const displayMoveFieldButton = moveFieldForm.length > 0;
    const menuButtons = [
        {Icon: "Edit", Key: "editfield", PrimaryAction: true, Text: TranslateTag("@ConEdiK@",props.language), UIEvent: "editfielduievent", EntryStates: "canconfig, canedit", Workflow: true, OnFinish: "refresh"},
        ...(displayMoveFieldButton ? [{Icon: "Move", Key: "movefield", PrimaryAction: true, Text: TranslateTag("@ConMovFG@",props.language), UIEvent: "movefielduievent", OnFinish: "refresh", EntryStates: "canconfig, canedit, candelete", Workflow: true}] : []),
        {Icon: "Delete", Key: "deletefield", PrimaryAction: true, Text: TranslateTag("@ConDelK@",props.language), UIEvent: "deletefielduievent", OnFinish: "refresh", EntryStates: "canconfig, candelete", Workflow: true }
    ];

    let addButton = undefined;
    const addForm = props.forms.filter(f => f.Name === "addfieldform");
    if (addForm.length > 0) {
        addButton = "addfielduievent";
    }

    let addExistingFieldButton = undefined;
    const addExistingFieldForm = props.forms.filter(f => f.Name === "addexistingfieldform");
    if (addExistingFieldForm.length > 0) {
        addExistingFieldButton = "addexistingfielduievent";
    }

    let addFormGroupButton = undefined;
    const addFormGroupForm = props.forms.filter(f => f.Name === "addformgroupform");
    if (addFormGroupForm.length > 0) {
        addFormGroupButton = "addformgroupuievent";
    }

    const menuItems = MapButtonsToContextMenu(menuButtons, buttonClickHandler, 'formdefinition');

    const columns = [
        { Key: 'column2', Name: TranslateTag("@GenLabB@",props.language), FieldName: 'Label', MinWidth: 180, MaxWidth: 180, IsResizable: true, IsCollapsible: true },
        { Key: 'menu', Name: '', FieldName: '', MinWidth: 120, MaxWidth: 120, IsResizable: true, IsCollapsible: false },
        { Key: 'column3', Name: TranslateTag("@GenTyp@",props.language), FieldName: 'Type', MinWidth: 180, MaxWidth: 180, IsResizable: true, IsCollapsible: true },
    ];

    const editClickHandler = (page) => {
        setFormStartConfig({
            button: {UIEvent: "editpageuievent", onFinish: 'refresh'}, 
            id: props.data[0].form + "|" + page.id, 
            refresh: refreshAfterReturningFromForm,
            containerVisibility: setListVisibility
        })
    }

    const deleteClickHandler = (page) => {
        setFormStartConfig({
            button: {UIEvent: "deletepageuievent", onFinish: 'refresh'}, 
            id: props.data[0].form + "|" + page.id, 
            refresh: refreshAfterReturningFromForm,
            containerVisibility: setListVisibility
        })
    }

    const editFormGroupClickHandler = (formGroup) => {
        setFormStartConfig({
            button: {UIEvent: "editformgroupuievent", onFinish: 'refresh'},
            id: formGroup.id,
            refresh: refreshAfterReturningFromForm,
            containerVisibility: setListVisibility
        })
    }

    const deleteFormGroupClickHandler = (formGroup) => {
        setFormStartConfig({
            button: {UIEvent: "deleteformgroupuievent", onFinish: 'refresh'},
            id: formGroup.id,
            refresh: refreshAfterReturningFromForm,
            containerVisibility: setListVisibility
        })
    }

    const moveFormGroupClickHandler = (formGroup) => {
        setFormStartConfig({
            button: {UIEvent: "moveformgroupuievent", onFinish: 'refresh'},
            id: formGroup.id,
            refresh: refreshAfterReturningFromForm,
            containerVisibility: setListVisibility
        })
    }

    const addExistingFieldClickHandler = (formGroup) => {
        setFormStartConfig({
            button: {UIEvent: "addexistingfielduievent", onFinish: 'refresh'},
            id: formGroup.addExistingFieldId,
            refresh: refreshAfterReturningFromForm,
            containerVisibility: setListVisibility
        })
    }

    const addFormGroupClickHandler = (page) => {
        setFormStartConfig({
            button: {UIEvent: "addformgroupuievent", onFinish: 'refresh'},
            id: props.data[0].form + "|" + page.id,
            refresh: refreshAfterReturningFromForm,
            containerVisibility: setListVisibility
        })
    }

    const reorderFormGroupClickHandler = (page) => {
        setFormStartConfig({
            button: {UIEvent: "reorderformgroupsuievent", onFinish: 'refresh'},
            id: props.data[0].form + "|" + page.id,
            refresh: refreshAfterReturningFromForm,
            containerVisibility: setListVisibility
        })
    }

    const editPageRulesClickHandler = (page) => {
        setFormStartConfig({
            button: {UIEvent: "editpagerulesuievent", onFinish: 'refresh'},
            id: props.data[0].form + "|" + page.id,
            refresh: refreshAfterReturningFromForm,
            containerVisibility: setListVisibility
        })
    }

    // Display list controls.
    const listContentCss = listVisibility ? "" : " app-invisible";

    let forms = props.forms.filter(f => f.Name === "editpageform");
    const displayEditButton = forms.length > 0;
    forms = props.forms.filter(f => f.Name === "deletepageform");
    const displayDeleteButton = forms.length > 0;
    forms = props.forms.filter(f => f.Name === "editformgroupform");
    const displayEditFormGroupButton = forms.length > 0;
    forms = props.forms.filter(f => f.Name === "deleteformgroupform");
    const displayDeleteFormGroupButton = forms.length > 0;
    forms = props.forms.filter(f => f.Name === "reorderformgroupsform");
    const displayReorderFormGroupButton = forms.length > 0;
    forms = props.forms.filter(f => f.Name === "moveformgroupform");
    const displayMoveFormGroupButton = forms.length > 0;
    forms = props.forms.filter(f => f.Name === "editpagerulesform");
    const displayPageRulesButton = forms.length > 0;

    return (
        <div className='directtestsgrid-content'>
            {displayList.map((page) => {
                return (
                    <div key={page.id} id={`formdefinition-page-${page.id}`} className={`formdefinition-page-block ${listContentCss}`}>
                        <PageHeader
                            page={page}
                            canEdit={displayEditButton}
                            canDelete={displayDeleteButton}
                            canAddFormGroup={addFormGroupButton !== undefined}
                            canReorder={displayReorderFormGroupButton && page.formGroups && page.formGroups.length > 0}
                            canEditRules={displayPageRulesButton}
                            edit={editClickHandler}
                            delete={deleteClickHandler}
                            addFormGroup={addFormGroupClickHandler}
                            reorder={reorderFormGroupClickHandler}
                            editRules={editPageRulesClickHandler}
                            language={props.language}
                            useFormDefinitionStyle={true}
                        />
                        {page.formGroups && page.formGroups.map((formGroup, index) => (
                            <div
                                key={formGroup.id}
                                id={`formdefinition-formgroup-${page.id}-${formGroup.columnKey}-${formGroup.formGroupKey}`}
                                className="formdefinition-formgroup-block"
                            >
                                <FormGroupHeader
                                    formGroup={formGroup}
                                    page={page}
                                    formGroupIndex={index}
                                    pageName={page.id}
                                    canEdit={displayEditFormGroupButton}
                                    canDelete={displayDeleteFormGroupButton}
                                    canMove={displayMoveFormGroupButton}
                                    edit={editFormGroupClickHandler}
                                    delete={deleteFormGroupClickHandler}
                                    move={moveFormGroupClickHandler}
                                    language={props.language}
                                />
                                <FormDefinitionSection
                                    type={"grid"}
                                    parentId={form + "|" + page.id}
                                    refresh={props.refresh}
                                    columns={columns}
                                    listData={formGroup.fields}
                                    menuItems={menuItems}
                                    itemSelected={itemSelected}
                                    selectedRecord={selectedIndex}
                                    selectionChanged={selectionChangeHandler}
                                    basicModeButtonHandler={embeddedModeButtonHandler}
                                    onClick={buttonClickHandler}
                                    page={{ ...page, fields: formGroup.fields }}
                                    addButton={addButton}
                                    addId={formGroup.addFieldDomId}
                                    addExistingButton={page.canReuseFields ? addExistingFieldButton : undefined}
                                    addExistingText={TranslateTag("@ConAddEF@", props.language)}
                                    addExistingId={formGroup.addExistingDomId}
                                    onAddExisting={() => addExistingFieldClickHandler(formGroup)}
                                    language={props.language}
                                />
                            </div>
                        ))}
                    </div>
            )})}
            <FormHandler startConfig={formStartConfig} showNextButton={false}></FormHandler>
        </div>
    )
}

const mapStateToProps = state => {
    return {
        forms: state.config.forms,
        language: state.config.language
    };
}

export default connect(mapStateToProps)(FormDefinition);