import React from 'react';
import './SimpleCardItem.css';
import { IconButton } from '@fluentui/react';

const SimpleCardItem = (props) => {

    const deleteIcon = { iconName: 'Cancel' };
    const lockIcon = { iconName: 'Lock' };

    const textArray = [];
    let description = null;
    for (var key in props.data){
        if (key.toLowerCase() !== "id" && key.toLowerCase() !== "colour" && key.toLowerCase() !== "centre" && key.toLowerCase() !== "uievent" && key.toLowerCase() !== "link") {
            if (key.toLowerCase() === "description") {
                description = props.data[key];
            } else if (key.toLowerCase() !== "key") {
                textArray.push(props.data[key]);
            }
        }
    }

    const cardClicked = () => {
        if (props.onClick !== undefined) {
            props.onClick(props.data);
        }
    }

    const cardDoubleClicked = () => {
        if (props.onDoubleClick !== undefined) {
            props.onDoubleClick(props.data);
        }
    }

    const cardDeleteClick = (event) => {
        event.stopPropagation();
        if (props.deleteDisabled) {
            return;
        }
        props.onDelete(props.data);
    }

    let classname = "simplecarditem-content";

    if (props.data.colour !== undefined) {
        switch (props.data.colour.toLowerCase()) {
            case "red":
                classname += " simplecarditem-red";
                break;
            case "green":
                classname += " simplecarditem-green";
                break;
            case "blue":
                classname += " simplecarditem-blue";
                break;
            default:
                classname = "simplecarditem-content";
        }
    }

    if (props.data.centre !== undefined && props.data.centre) {
        classname += " simplecarditem-aligncentre";
    }

    const cardId = props.data.patientref ? `patientsearchresult-card-${props.data.patientref}` : undefined;

    const testCardId = props.data.patientref === undefined && props.data.id !== undefined && props.data.id !== null
        ? props.data.id
        : undefined;
    const cardTestId = testCardId === -1
        ? 'test-card-add'
        : (testCardId !== undefined ? `test-card-${testCardId}` : undefined);
    const deleteTestId = testCardId !== undefined && testCardId !== -1
        ? `test-card-delete-${testCardId}`
        : undefined;

    let iconContent = (null);
    if (props.data.delete !== undefined && props.data.delete) {
        iconContent =  <div className="simplecarditem-delete">
                            <IconButton tabIndex={-1} iconProps={ deleteIcon } title="Delete" onClick={cardDeleteClick} data-testid={deleteTestId} disabled={props.deleteDisabled} aria-disabled={props.deleteDisabled}/>
                        </div>
    }

    if (props.data.lock !== undefined && props.data.lock) {
        iconContent =  <div className="simplecarditem-delete">
                            <IconButton tabIndex={-1} iconProps= { lockIcon } title="Lock" />
                        </div>
    }

    const handleKeySelect = (event) => {
        if (event.key === "Enter") {
            props.onKeySelect(props.data);
        }
    }

    return (
        <div id={cardId} data-testid={cardTestId} className={classname} onClick={cardClicked} onDoubleClick={cardDoubleClicked} onKeyPress={handleKeySelect} tabIndex={0}>
            {iconContent}
            {description !== null ? <div className="simplecarditem-description">{description}</div> : (null)}
            {textArray !== undefined && textArray.map((item, index) => (item === "" || item === null) ? "" : (
                <div key={index}>{item}</div>
            ))}
        </div>
    );
};
  
export default SimpleCardItem;