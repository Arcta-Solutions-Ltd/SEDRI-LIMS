import React from 'react';
import ArcSplitButton from '../ArcSplitButton/ArcSplitButton';
import FieldGridField from '../FieldGrid/FieldGridField/FieldGridField';
import TranslateTag from '../../../Utils/Local/TranslateTag';


const ArcSelectorLine = (props) => {

    const recordId = props.data.id ?? props.data.value ?? props.data.Value ?? '';
    const rowId = recordId ? `fieldlist-row-${recordId}` : undefined;
    const canDrag = props.config.Draggable === true;

    const onMenuClick= (event, menu) => {
        props.menuClick(props.data, menu);
    }
    
    const optionMenu = {
        items: [
          { key: 'moveTop', text: TranslateTag("@MovTop@", props.language), iconProps: { iconName: 'UpLoad' }, onClick: onMenuClick },
          { key: 'moveUp', text: TranslateTag("@MovUp@", props.language), iconProps: { iconName: 'Up' }, onClick: onMenuClick },
          { key: 'moveDown', text: TranslateTag("@MovDow@", props.language), iconProps: { iconName: 'Down' }, onClick: onMenuClick },
          { key: 'moveBottom', text: TranslateTag("@MovBot@", props.language), iconProps: { iconName: 'DownLoad' }, onClick: onMenuClick },
        ],
      };

    const rowClassName = canDrag ? 'fieldgrid-row fieldgrid-row-draggable' : 'fieldgrid-row';

    return (
        <React.Fragment>
            <div
                id={rowId}
                className={rowClassName}
                draggable={canDrag}
                onDragStart={canDrag ? () => props.onDragStart(props.rowIndex) : undefined}
                onDragOver={canDrag ? props.onDragOver : undefined}
                onDrop={canDrag ? (event) => {
                    event.preventDefault();
                    props.onDrop(props.rowIndex);
                } : undefined}
            >
                <FieldGridField key={props.data.value + "label"} config={{Id: props.data.label, Type: 'text', Width: 'medium', value: props.data.label}}></FieldGridField>
                {props.config.CanEnable && <FieldGridField key={props.data.value} config={{Id: props.data.label, Type: 'toggle', Width: 'small', value: props.data.enabled}} changeHandler={props.enableClick}></FieldGridField>}
                {props.config.CanMoveEntries && (
                    <ArcSplitButton
                        key={props.data.value + "menu"}
                        id={recordId ? `fieldlist-move-${recordId}` : undefined}
                        icon="Move"
                        options={optionMenu}
                        onClick={onMenuClick}
                        tooltip={TranslateTag("@ConMovFG@", props.language)}
                        ariaLabel={TranslateTag("@ConMovFG@", props.language)}
                    />
                )}
                {props.config.IncludeOptions && <FieldGridField key={props.data.value + "combo"} config={{Id: props.data.label, Type: 'dropdown', Width: 'medium', Options: props.config.Options, value: props.data.combo}} changeHandler={props.columnClick} ></FieldGridField>}
            </div>
        </React.Fragment>
    )
}

export default ArcSelectorLine;
