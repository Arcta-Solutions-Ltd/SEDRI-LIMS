import React, { useRef } from 'react';
import ArcSelector from '../ArcSelector/ArcSelector';
import { moveBottom, moveDown, moveTop, moveUp, reorderList } from '../ArcSelector/ArcSelectorReorderUtils';

const ArcFieldSelector = (props) => {

    const draggedItemIndexRef = useRef(null);
    const selectorConfig = {
        ...props.config,
        Draggable: props.config.Draggable === true
            || (props.config.CanMoveEntries === true && props.config.IncludeOptions !== true),
    };

    const applyReorder = (newData) => {
        props.changeHandler(props.config.Id, newData);
    };

    const moveTopHandler = (record) => {
        applyReorder(moveTop(props.data, record));
    };

    const moveUpHandler = (record) => {
        applyReorder(moveUp(props.data, record));
    };

    const moveDownHandler = (record) => {
        applyReorder(moveDown(props.data, record));
    };

    const moveBottomHandler = (record) => {
        applyReorder(moveBottom(props.data, record));
    };

    const menuClick = (record, menu) => {
        if (menu !== undefined) {
            switch(menu.key) {
                case "moveTop":
                    moveTopHandler(record);
                    break;
                case "moveUp":
                    moveUpHandler(record);
                     break;
                case "moveDown":
                    moveDownHandler(record);
                    break;
                case "moveBottom":
                    moveBottomHandler(record);
                    break;
                default:
              }
        }
    }

    const enableClick = (id, value) => {
        const newData = props.data.map((record) => enabledChangeMapping(id, value, record));
        applyReorder(newData);
    }

    const columnClick = (id, value) => {
        const newData = props.data.map((record) => columnChangeMapping(id, value, record));
        applyReorder(newData);
    }

    const enabledChangeMapping = (id, value, record) => {
        if (record.label === id)   {
            record.enabled = value;
        }
        return {label: record.label, value: record.value, enabled: record.enabled, combo: record.combo} 
    }

    const columnChangeMapping = (id, value, record) => {
        if (record.label === id)   {
            record.combo = value;
        }
        return {label: record.label, value: record.value, enabled: record.enabled, combo: record.combo} 
    }

    const handleDragStart = (index) => {
        draggedItemIndexRef.current = index;
    };

    const handleDragOver = (event) => {
        event.preventDefault();
    };

    const handleDrop = (index) => {
        const fromIndex = draggedItemIndexRef.current;
        if (fromIndex === null || fromIndex === index) {
            draggedItemIndexRef.current = null;
            return;
        }
        applyReorder(reorderList(props.data, fromIndex, index));
        draggedItemIndexRef.current = null;
    };

    return (
        <div>
            <ArcSelector
                config={selectorConfig}
                data={props.data}
                menuClick={menuClick}
                enableClick={enableClick}
                columnClick={columnClick}
                language={props.language}
                onDragStart={handleDragStart}
                onDragOver={handleDragOver}
                onDrop={handleDrop}
            ></ArcSelector>
        </div>
    )
}

export default ArcFieldSelector;
