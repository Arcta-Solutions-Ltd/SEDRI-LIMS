import React from 'react';
import DataList from './DataList/DataList';
import CardList from './CardList/CardList';

/**
 * ListView component renders a list of items using different display types based on the provided props.
 * 
 * Depending on the type specified in the props, it will either render a card list or a data list with various configurations.
 * 
 * @param {Object} props - The props object containing configuration and data for the list view.
 * @param {string} props.type - The type of list to display ("card" for CardList, otherwise DataList).
 * @param {Array} props.cardFieldNames - An array of field names to be displayed in CardList.
 * @param {Array} props.listData - An array of data objects to be displayed in the list.
 * @param {Array} props.menuItems - An array of menu items for the list.
 * @param {function} props.itemSelected - Handler for item selection event.
 * @param {function} props.itemInvokedHandler - Handler for item invoked event.
 * @param {Array} props.columns - An array of column configurations for DataList.
 * @param {string} props.parentId - The parent ID for hierarchical data structure.
 * @param {function} props.selectionChanged - Handler for selection change event.
 * @param {Object} props.selectedRecord - The currently selected record.
 * @param {Object} props.selectionModel - The selection model configuration.
 * @param {function} props.updateSortedColumn - Handler for updating the sorted column.
 * @param {boolean} props.basic - Flag indicating if the view is in basic mode.
 * @param {boolean} props.isDataLoaded - Flag indicating if the data is loaded.
 * @param {function} props.basicModeButtonHandler - Handler for the basic mode button click event.
 * @param {boolean} props.multiSelect - Flag indicating if multi-select is enabled.
 * @param {boolean} props.displaySummary - Flag indicating if the summary should be displayed.
 * @param {React.Element} props.addButton - The button element for adding new items.
 * @param {string} [props.addId] - DOM id for the add icon, used when a second add action is present.
 * @param {function} [props.onSecondaryAdd] - Handler for a second add action shown beside the add
 *   icon on the grid header. Supplying it switches the header to its two icon form.
 * @param {string} [props.secondaryAddId] - DOM id for the second add icon.
 * @param {string} [props.secondaryAddIcon] - Fluent icon name for the second add icon.
 * @param {string} [props.secondaryAddTooltip] - Translated tooltip for the second add icon.
 * @param {function} props.onClick - Click event handler.
 * @param {string} [props.addButtonTooltip] - Language tag for the add (+) column-header icon tooltip.
 * @param {string} props.language - The language to be used for displaying the list.
 * 
 * @returns {JSX.Element} The rendered ListView component.
 */
const ListView = (props) => {

    let displayType = <DataList></DataList>;

    if (props.type === "card") {
        displayType = <CardList
                        fieldNames={props.cardFieldNames}
                        data={props.listData}
                        menuItems={props.menuItems}
                        itemSelected={props.itemSelected}
                        laboratoryConfig={props.laboratoryConfig}
                        itemInvokedHandler={props.itemInvokedHandler}
                        language={props.language}
                        turnaroundTimeFieldName={props.turnaroundTimeFieldName}
                        onMenuButtonClick={props.onMenuButtonClick}
                        forms={props.forms}
                        viewName={props.viewName}>
                      </CardList>
    } else {
        displayType = <DataList
                        columns={props.columns}
                        parentId={props.parentId}
                        data={props.listData}
                        selectionChanged={props.selectionChanged}
                        itemInvokedHandler={props.itemInvokedHandler}
                        selectedRecord={props.selectedRecord}
                        selectionModel={props.selectionModel}
                        menuItems={props.menuItems}
                        updateSortedColumn={props.updateSortedColumn}
                        basic={props.basic}
                        isDataLoaded={props.isDataLoaded}
                        basicModeButtonHandler={props.basicModeButtonHandler}
                        multiSelect={props.multiSelect}
                        displaySummary={props.displaySummary}
                        addButton={props.addButton}
                        addButtonTooltip={props.addButtonTooltip}
                        addId={props.addId}
                        secondaryAddId={props.secondaryAddId}
                        secondaryAddIcon={props.secondaryAddIcon}
                        secondaryAddTooltip={props.secondaryAddTooltip}
                        onSecondaryAdd={props.onSecondaryAdd}
                        onClick={props.onClick}
                        laboratoryConfig = {props.laboratoryConfig}
                        groupBy={props.groupBy}
                        groupText={props.groupText}
                        groupMenu={props.groupMenu}
                        language={props.language}
                        forms={props.forms}
                        onColumnLayoutChange={props.onColumnLayoutChange}
                        columnLayout={props.columnLayout}
                        viewName={props.viewName}
                        onMenuButtonClick={props.onMenuButtonClick}
                        calloutLazyLoad={props.calloutLazyLoad}
                        calloutParentType={props.calloutParentType}
                        testRowMenuIdPrefix={props.testRowMenuIdPrefix}>
                      </DataList>
    }

    return (
        <React.Fragment>
            {displayType}
        </React.Fragment>
    );
};

export default ListView;
