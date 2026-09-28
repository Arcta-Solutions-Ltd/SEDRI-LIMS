import React from 'react';
import { ShimmeredDetailsList } from '@fluentui/react/lib/ShimmeredDetailsList';
import { getTheme, mergeStyles } from '@fluentui/react/lib/Styling';
import './ArcReportGrid.css';

const ArcReportGrid = (props) => {

  
    // const AddLinksToTableEntries = (items) => {

    //   if (items.length === 0) { return items; }
    //   const returnItems = []
    //   let returnItem;
    //   for (const item of items) {
    //       const keys = Object.keys(item)
    //       returnItem = []
    //       for (const key of keys) {
    //         returnItem[key] = <div class="arcreportgrid-link">{item[key]}</div>    
    //       }
    //   }
    //   returnItems.push(returnItem);
    // }

    const items = props.items;
    const columns = props.columns;
    let draggedItem = undefined;
    let draggedIndex = undefined;

    const theme = getTheme();
    const dragEnterClass = mergeStyles({
        backgroundColor: theme.palette.neutralLight,
      });

    const insertBeforeItem = (item) => {
        const draggedItems = [draggedItem];
        
        const insertIndex = items.indexOf(item);
        const newItems = items.filter(itm => draggedItems.indexOf(itm) === -1);
    
        newItems.splice(insertIndex, 0, ...draggedItems);
    
        props.onChangeRows(newItems);
    }

    const renderItemColumn = (item, index, column) => {
          return (
            <span
              className="arcreportgrid-text"
            >
              {item[column.key]}
            </span>
          );
    }

    const DragDropEvents = {};
    DragDropEvents.canDrop = (dropContext, dragContext) => { return true; };
    DragDropEvents.canDrag = (item) => { return true; };
    DragDropEvents.onDragEnter = (item, event) => { return dragEnterClass; };
    DragDropEvents.onDragLeave = (item, event) => { return; };
    DragDropEvents.onDrop = (item, event) => {
        if (draggedItem) {
            insertBeforeItem(item);
        }
    };
    DragDropEvents.onDragStart = (item, itemIndex, selectedItems, event) => {
        draggedItem = item;
        draggedIndex = itemIndex;
    };
    DragDropEvents.onDragEnd = (item, event) => {
        draggedItem = undefined;
        draggedIndex = -1;
    };

    const handleColumnReorder = (draggedIndex, targetIndex) => {
        const draggedItems = columns[draggedIndex];
        const newColumns = [...columns];
    
        // insert before the dropped item
        newColumns.splice(draggedIndex, 1);
        newColumns.splice(targetIndex, 0, draggedItems);
        props.onChangeColumns(newColumns);
      };

    const columnReorderOptions = {
        frozenColumnCountFromStart: 1,
        frozenColumnCountFromEnd: 0,
        handleColumnReorder: handleColumnReorder
      };

    const onColumnHeaderClick = (data) => {

    };

    return (
        <ShimmeredDetailsList
        items={items}
        columns={columns}
        onColumnHeaderClick={onColumnHeaderClick}
        enableShimmer={false}
        compact={true}
        dragDropEvents={DragDropEvents}
        columnReorderOptions={columnReorderOptions}
        onRenderItemColumn={renderItemColumn}
        multiSelect={true}
    />
    );
}

export default ArcReportGrid