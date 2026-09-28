import { Label } from '@fluentui/react';
import React from 'react';
import ArcSelectorLine from './ArcSelectorLine';
import './ArcSelector.css';

const ArcSelector = (props) => {
    const selectList = Array.isArray(props.data) ? props.data : [];
    const canDrag = props.config.Draggable === true;

    const label =
        props.config.Label === undefined ? null : (
            <Label>{props.config.Label}</Label>
        );

    return (
        <>
            {label}
            {selectList.map((data, index) => (
                <ArcSelectorLine
                    key={(data.id ?? data.value ?? data.Value ?? index) + '-selector-line'}
                    config={props.config}
                    data={data}
                    rowIndex={index}
                    enableClick={props.enableClick}
                    menuClick={props.menuClick}
                    columnClick={props.columnClick}
                    language={props.language}
                    onDragStart={canDrag ? props.onDragStart : undefined}
                    onDragOver={canDrag ? props.onDragOver : undefined}
                    onDrop={canDrag ? props.onDrop : undefined}
                ></ArcSelectorLine>
            ))}
        </>
    );
};

export default ArcSelector;
