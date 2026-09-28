import React from 'react';
import EvaluateRules from '../../../Utils/Rules/EvaluateRules';
import FormGroup from '../FormGroup/FormGroup';
import './FormColumn.css';

/**
 * Renders one form column and its visible form groups.
 * @param {Object} props
 * @param {boolean} [props.multiPage] - When true, uses the full-width column layout for workflow pages.
 * @param {boolean} [props.wideColumn] - When true, uses the `formcolumn-wide` layout so fields can
 *   use a wider panel (`wider`, `wide`, or `extraWide` on the page config) while staying stacked
 *   vertically.
 */
const FormColumn = (props) => {

    const fieldNotVisible = (formGroups, fieldName) =>  {
        for (const formGroup of formGroups) {
            formGroup.visible = EvaluateRules("visible",formGroup.Rules, props.data);
            columnVisible = formGroup.visible || columnVisible;
    
            if (formGroup.visible) {
                for (const field of formGroup.Fields) {
                    if (field.Id === fieldName) {
                        return false;
                    }
                }
            }
        }
        return true;
    }
    
    let columnVisible = false;
    let formGroups = [...props.config.FormGroups]
    for (const formGroup of formGroups) {
        formGroup.visible = EvaluateRules("visible",formGroup.Rules, props.data);
        columnVisible = formGroup.visible || columnVisible;

        if (! formGroup.visible) {
            for (const field of formGroup.Fields) {
                if (field.value !== undefined && field.value !== null && field.value !== null && fieldNotVisible(formGroups, field.Id)) {
                    props.changeHandler(field.Id, null);
                }
            }
        }
    }

    const formColumnClass = props.multiPage
        ? "formcolumn-fullscreen"
        : props.wideColumn
            ? "formcolumn-wide"
            : "formcolumn-2col";

    let columnContents = (null);
    if (columnVisible) {
        columnContents = ([
            <div key={props.config.Key} data-cy={props.config.Key} className={formColumnClass}>
                {formGroups.map((formGroup) => {
                    if (formGroup.visible) {
                        return (
                            <FormGroup key={formGroup.Key} config={formGroup} changeHandler={props.changeHandler} fieldFocusOut={props.fieldFocusOut} language={props.language} id={props.id} onKeyDown={props.onKeyDown} lists={props.lists} pages={props.pages} allPages={props.allPages} uievents={props.uievents} forms={props.forms} embeddedPages={props.embeddedPages} data={props.data}></FormGroup>
                        )
                    } else {
                        return (null)
                    }}
                )}
            </div> 
        ])
    }

    return (
        <React.Fragment>
            {columnContents}    
        </React.Fragment>
    )
};
  
export default FormColumn;