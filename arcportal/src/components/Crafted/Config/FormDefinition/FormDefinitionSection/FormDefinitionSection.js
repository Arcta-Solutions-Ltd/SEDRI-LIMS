import React from 'react';
import ListView from '../../../../General/ListView/ListView';

/**
 * Renders the fields grid for a form group in the Form Definition.
 * The add field and add existing field actions are both icons on the grid column header, so the two
 * are presented identically. Both are hidden on pages whose configureActions include "nofields",
 * because those pages hold query criteria rather than stored fields, and neither appears unless the
 * page configureActions include "add".
 * @param {Array} props.language - Language array for TranslateTag (tooltips, column headers).
 * @param {string} [props.addButton] - UI event name for the add new field button.
 * @param {string} [props.addId] - DOM id for the add field icon.
 * @param {string} [props.addExistingButton] - UI event name for the add existing field action.
 *   Left undefined by the caller on pages that cannot take referenced fields, so the icon is
 *   absent rather than opening an empty picker.
 * @param {string} [props.addExistingText] - Translated tooltip for the add existing field icon.
 * @param {string} [props.addExistingId] - DOM id for the add existing field icon.
 * @param {Function} [props.onAddExisting] - Invoked when the add existing field icon is clicked.
 */
const FormDefinitionSection = (props) => {

    let addButton;
    let canAdd = false;

    if (props.page.configureActions !== undefined && props.page.configureActions !== null) {
        if (props.page.configureActions.includes("add")) {
            addButton = props.addButton;
            canAdd = true;
        }       
    }

    let hasFields = true;
    if (props.page.configureActions !== undefined && props.page.configureActions !== null) {
        if (props.page.configureActions.includes("nofields")) {
            hasFields = false;
        }       
    }

    const showAddExisting = hasFields && canAdd && props.addExistingButton !== undefined;

    const listView = hasFields ? <ListView 
                        type={props.type}
                        parentId={props.parentId}
                        refresh={props.refresh}
                        columns={props.columns}
                        listData={props.page.fields}
                        menuItems={props.menuItems}
                        itemSelected={props.itemSelected}
                        selectedRecord={props.selectedRecord}
                        selectionChanged={props.selectionChanged}
                        basicModeButtonHandler={props.basicModeButtonHandler}
                        onClick={props.onClick}
                        isDataLoaded={true}
                        addButton={addButton}
                        addId={props.addId}
                        secondaryAddId={showAddExisting ? props.addExistingId : undefined}
                        secondaryAddIcon={showAddExisting ? "AddLink" : undefined}
                        secondaryAddTooltip={showAddExisting ? props.addExistingText : undefined}
                        onSecondaryAdd={showAddExisting ? props.onAddExisting : undefined}
                        basic={true}
                        displaySummary={true}
                        viewName="formdefinition"
                        addButtonTooltip="@ConAddJ@"
                        language={props.language}>
                    </ListView> : (null);

    return (
        <div className='managerecord-section-table'>
            {listView}
        </div>
    )
}

export default FormDefinitionSection;
