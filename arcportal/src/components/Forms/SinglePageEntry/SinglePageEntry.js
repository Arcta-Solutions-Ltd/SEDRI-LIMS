import React, { useEffect } from 'react';
import './SinglePageEntry.css';
import FormColumn from '../FormColumn/FormColumn';
import TextDisplay from '../TextDisplay/TextDisplay';

// 'text' = read-only label; space/separator are layout. Date/combo: Fluent uses suffixed ids (see getFocusCandidatesForField).
const TYPES_WITHOUT_INPUT_FOCUS = new Set(['text', 'space', 'separator']);
/** ComboBox Autofill uses `${id}-input`; root uses `${id}wrapper` (no hyphen before "wrapper"). */
const FLUENT_COMBO_FIELD_TYPES = new Set(['combobox', 'filteredcombo']);

const shouldSkipField = (field) => {
    if (!field || TYPES_WITHOUT_INPUT_FOCUS.has(field.Type)) return true;
    if (field.Disabled === true) return true;
    return false;
};

/** @returns {boolean} whether focus moved to this element or a focusable descendant (e.g. Fluent composites). */
const tryFocusElement = (el) => {
    if (el == null || typeof el.focus !== 'function') return false;
    if ('disabled' in el && el.disabled) return false;
    el.focus({ preventScroll: true });
    const active = document.activeElement;
    return el === active || (active != null && typeof el.contains === 'function' && el.contains(active));
};

/**
 * Map config field id to DOM node(s) to try for programmatic focus. Fluent often does not put `field.Id` on the focusable node.
 */
const getFocusCandidatesForField = (field) => {
    const id = field.Id;
    if (id == null) return [];
    if (field.Type === 'date') {
        const textField = document.getElementById(`${id}-label`);
        const root = document.getElementById(id);
        const list = [];
        if (textField) list.push(textField);
        if (root && root !== textField) list.push(root);
        return list;
    }
    if (FLUENT_COMBO_FIELD_TYPES.has(field.Type)) {
        const input = document.getElementById(`${id}-input`);
        const wrapper = document.getElementById(`${id}wrapper`);
        const root = document.getElementById(id);
        const list = [];
        if (input) list.push(input);
        if (wrapper && wrapper !== input) list.push(wrapper);
        if (root && root !== input && root !== wrapper) list.push(root);
        return list;
    }
    const el = document.getElementById(id);
    return el ? [el] : [];
};

const SinglePageEntry = (props) => {

const pageCss = props.fullScreen ? "singlepageentry-fullscreencontent" : "singlepageentry-content";

useEffect(() => {
    const frameId = requestAnimationFrame(() => {
        const columns = props.config.Columns || [];
        for (const column of columns) {
            const formGroups = column.FormGroups || [];
            for (const formGroup of formGroups) {
                const fields = formGroup.Fields || [];
                for (const field of fields) {
                    if (shouldSkipField(field)) continue;
                    const candidates = getFocusCandidatesForField(field);
                    for (const el of candidates) {
                        if (tryFocusElement(el)) return;
                    }
                }
            }
        }
    });
    return () => cancelAnimationFrame(frameId);
}, [props.config.Name]);


    const pageUsesWideColumn = props.config.Wider || props.config.wider
        || props.config.ExtraWide || props.config.extraWide
        || props.config.Wide || props.config.wide;

    return (
        <div className={pageCss}>
            <div id='singlepageentry-title' className='singlepageentry-title'>{props.config.PageTitle}</div>
            <div id='singlepageentry-headertext' className='singlepageentry-headertext'>
                <TextDisplay text={props.config.Text}></TextDisplay>
            </div>
            <div>
                {props.config.Columns.map((column) => {
                    return (
                        <FormColumn 
                            key={column.Key} 
                            onKeyDown={props.onKeyDown}
                            config={column} 
                            multiPage={props.config.MultiPageView}
                            wideColumn={pageUsesWideColumn}
                        changeHandler={props.changeHandler} 
                        fieldFocusOut={props.fieldFocusOut} 
                        data={props.data} 
                        language={props.language} 
                        id={props.id}
                        lists={props.lists}
                        pages={props.pages}
                        allPages={props.allPages}
                        uievents={props.uievents}
                        forms={props.forms}
                        embeddedPages={props.embeddedPages}
                        >                        
                        </FormColumn>
                )
            })}
        </div>
    </div>
)

};

export default SinglePageEntry;
