import React from 'react';
import './SingleLineField.css';
import FieldGrid from '../FieldGrid/FieldGrid';
import ArcToggle from '../ArcToggle/ArcToggle';
import ArcTextField from '../ArcTextField/ArcTextField';
import ArcChoiceGroup from '../ArcChoiceGroup/ArcChoiceGroup';
import ArcDropdown from '../ArcDropdown/ArcDropdown';
import ArcDate from '../ArcDate/ArcDate';
import ArcFilteredCombo from '../ArcFilteredCombo/ArcFilteredCombo';
import ArcCombo from '../ArcCombo/ArcCombo';
import ArcNumber2 from '../ArcNumber/ArcNumber2';
import ArcTime from '../Arctime/ArcTime';
import ArcHierarchy from '../ArcHierarchy/ArcHierarchy';
import ArcUpload from '../ArcUpload/ArcUpload';
import { SearchBox, Separator, TextField } from '@fluentui/react';
import ArcColourPicker from '../ArcColourPicker/ArcColourPicker';
import CraftedComponentFactory from '../../Crafted/CraftedComponentFactory';
import ArcPicker from '../ArcPicker/ArcPicker';
import ArcHierarchyPicker from '../ArcHierarchyPicker/ArcHierarchyPicker';
import ReportFilter from '../../Crafted/Reports/ReportFilter/ReportFilter';
import ArcFieldSelector from '../ArcFieldSelector/ArcFieldSelector';
import InstrumentProfileSelector from '../InstrumentProfileSelector/InstrumentProfileSelector';
import ArcPlainText from '../ArcPlaintText/ArcPlainText';
import ArcOrganismList from '../ArcOrganismList/ArcOrganismList';
import MicDosageNumber from '../ArcNumber/MicDosageNumber';
import ArcJson from '../ArcJson/ArcJson';
import RulesEditor from '../../Crafted/Config/RulesEditor/RulesEditor';
import PageRulesEditor from '../../Crafted/Config/PageRulesEditor/PageRulesEditor';
import PageOrderEditor from '../../Crafted/Config/PageOrderEditor/PageOrderEditor';
import ParentLinkEditor from '../../Crafted/Config/ParentLinkEditor/ParentLinkEditor';
import ExistingFieldSelector from '../../Crafted/Config/ExistingFieldSelector/ExistingFieldSelector';
import ArcAge from '../ArcAge/ArcAge';
import ArcDuration from '../ArcDuration/ArcDuration';
import resolveQueryDropdownOptions from '../../../Utils/General/ResolveQueryDropdownOptions';
import TranslateTag from '../../../Utils/Local/TranslateTag';
import { translateEmbeddedLanguageTags } from '../../../Utils/General/FormatAgeDisplay';

const resolveTaggedText = (text, language) => {
    if (typeof text === 'string' && text.startsWith('@')) {
        return TranslateTag(text, language);
    }
    return text;
};

const resolveDisplayValue = (value, language) => {
    if (typeof value !== 'string' || !value.includes('@')) {
        return value;
    }
    return translateEmbeddedLanguageTags(value, language);
};

document.addEventListener("contextmenu", (event) => {
    event.preventDefault();
});

const SingleLineField = (props) => {

    if (!props.config) {
        return null;
      }
      
    const valueChangeHandler = (event, value) => {
        props.changeHandler(props.config.Id, value)
    }

    const textChangeHandler = (event, value) => {
        const maxValue = isNaN(parseInt(props.config.Max)) ? 100000 : Number(props.config.Max);
        value = value.length > maxValue ? value.substring(0,maxValue) : value;
        value = value.replace(/[|'"]/g, '');

        props.changeHandler(props.config.Id, value)
    }

    const comboBoxChangeHandler = (event, value, overrideId) => {
        const id = overrideId === undefined || typeof overrideId !== 'string' ? props.config.Id : overrideId;
        if (value !== undefined) {
            props.changeHandler(id, value.key);
        } else {
            props.changeHandler(id, undefined);
        }
    }

    const radioChangeHandler = (event, value) => {
        props.changeHandler(props.config.Id, value)
    }

    const label = resolveTaggedText(props.config.Label, props.language);
    const placeholder = resolveTaggedText(props.config.Placeholder, props.language);

    let fieldToDisplay;
    switch (props.config.Type) {
        case "singleline":
            fieldToDisplay = <ArcTextField 
                id={props.config.Id} 
                config={props.config} 
                language={props.language}
                onKeyDown={props.onKeyDown}
                changeHandler={valueChangeHandler}
                focusOut={props.focusOut} 
                first={props.first}
                >
            </ArcTextField>
            break;
        case "multiline":
            fieldToDisplay = <TextField
                                label={label}
                                id={props.config.Id}
                                multiline autoAdjustHeight
                                required={props.config.Required}
                                placeholder={placeholder}
                                onChange={textChangeHandler}
                                onKeyDown={props.onKeyDown}
                                focusOut={props.focusOut}
                                value={props.config.value ?? ''}
                                disabled={props.config.ReadOnly === true}
                            />;
            break;
        case "password":
            fieldToDisplay = <TextField
                                id={props.config.Id}
                                label={label}
                                onKeyDown={props.onKeyDown}
                                autoComplete="new-password"
                                type="password"
                                placeholder={placeholder}
                                canRevealPassword={true}
                                required={props.config.Required}
                                onChange={textChangeHandler}
                             />;
            break;
        case "toggle":
            fieldToDisplay = <ArcToggle 
                config={props.config} 
                valueChangeHandler={valueChangeHandler} 
                showText={props.config.ShowText !== false}
                first={props.first}
                onKeyDown={props.onKeyDown}
            >
            </ArcToggle>
            break;
        case "dropdown":
            const dropdownOptionsName = props.config.OptionsName || props.config.optionsName;
            const queryDropdownOptions = resolveQueryDropdownOptions(props.data, dropdownOptionsName, props.config);
            const dropdownConfig = queryDropdownOptions != null
                ? { ...props.config, Options: queryDropdownOptions }
                : props.config;
            fieldToDisplay = <ArcDropdown
                                config={dropdownConfig}
                                otherOptionParentIds={props.otherOptionParentIds}
                                valueChangeHandler={props.changeHandler}
                                first = {props.first}
                                onKeyDown={props.onKeyDown}
                             ></ArcDropdown>
            break;
        case "combobox":
            const comboboxOptionsName = props.config.OptionsName || props.config.optionsName;
            const queryComboboxOptions = resolveQueryDropdownOptions(props.data, comboboxOptionsName, props.config);
            const comboboxConfig = queryComboboxOptions != null
                ? { ...props.config, Options: queryComboboxOptions }
                : props.config;
            fieldToDisplay = <ArcCombo
                                config={comboboxConfig}
                                otherOptionParentIds={props.otherOptionParentIds}
                                changeHandler={props.changeHandler}
                                first = {props.first}
                                onKeyDown={props.onKeyDown}
                                >
                            </ArcCombo>
            break;
        case "hierarchicalpicker":
            fieldToDisplay = <ArcHierarchyPicker
                                config={props.config}
                                valueChangeHandler={props.changeHandler}
                                onKeyDown={props.onKeyDown}
                                uievents={props.uievents}
                                forms={props.forms}
                                language={props.language}
                                recordId={props.id}
                            ></ArcHierarchyPicker>
            break;
        case "picker":
            fieldToDisplay = <ArcPicker
                                config={props.config}
                                valueChangeHandler={props.changeHandler}
                                onKeyDown={props.onKeyDown}
                            ></ArcPicker>
            break;
        case "filteredcombo":
            fieldToDisplay = <ArcFilteredCombo config={props.config} changeHandler={comboBoxChangeHandler} onKeyDown={props.onKeyDown}></ArcFilteredCombo>
            break;
        case "instrumentprofileselector":
            fieldToDisplay = (
                <InstrumentProfileSelector
                    config={props.config}
                    data={props.data}
                    changeHandler={comboBoxChangeHandler}
                    onKeyDown={props.onKeyDown}
                />
            );
            break;
        case "date":
            fieldToDisplay = <ArcDate config={props.config} changeHandler={valueChangeHandler} focusOut={props.focusOut} onKeyDown={props.onKeyDown}></ArcDate>
            break;
        case "search":
            fieldToDisplay = <SearchBox
                                id={props.config.Id}
                                onKeyDown={props.onKeyDown}
                                placeholder={props.config.Placeholder}
                                label={label}/>;
            break;
        case "radio":
            fieldToDisplay = <ArcChoiceGroup
                                config={props.config}
                                otherOptionParentIds={props.otherOptionParentIds}
                                onKeyDown={props.onKeyDown}
                                radioChangeHandler={radioChangeHandler}
                            />;
            break;
        case "age":
            fieldToDisplay = <ArcAge
                config={props.config}
                changeHandler={props.changeHandler}
                focusOut={props.focusOut}
                onKeyDown={props.onKeyDown}
                data={props.data}
                language={props.language}
            />;
            break;
        case "duration":
            fieldToDisplay = <ArcDuration
                config={props.config}
                changeHandler={props.changeHandler}
                focusOut={props.focusOut}
                onKeyDown={props.onKeyDown}
                data={props.data}
                language={props.language}
            />;
            break;
        case "number":
            fieldToDisplay = <ArcNumber2
                                key={props.key} 
                                config={props.config} 
                                changeHandler={valueChangeHandler}
                                first={props.first}
                                focusOut={props.focusOut}
                                onKeyDown={props.onKeyDown}
                            />;
            break;
        case "micdosage":
            fieldToDisplay = <MicDosageNumber
                                key={props.key} 
                                config={props.config} 
                                changeHandler={valueChangeHandler}
                                first={props.first}
                            />;
            break;
        case "time":
            fieldToDisplay = <ArcTime 
                                key={props.key} 
                                config={props.config} 
                                onKeyDown={props.onKeyDown}
                                changeHandler={valueChangeHandler}
                                >
                            </ArcTime>
            break;
        case "fieldgrid":
            fieldToDisplay = <FieldGrid recordid={props.id} config={props.config} changeHandler={valueChangeHandler} onKeyDown={props.onKeyDown} language={props.language} lists={props.lists} pages={props.pages} allPages={props.allPages} uievents={props.uievents} forms={props.forms} embeddedPages={props.embeddedPages}></FieldGrid>;
            break;
        case "space":
            fieldToDisplay = <br />
            break;
        case "separator":
            fieldToDisplay = <Separator>{label}</Separator>
            break;
        case "crafted":
            fieldToDisplay = <CraftedComponentFactory config={props.config} data={props.data} changeHandler={valueChangeHandler} fieldChangeHandler={props.changeHandler} language={props.language}></CraftedComponentFactory>
            break
        case "colourpicker":
            fieldToDisplay = <ArcColourPicker config={props.config} changeHandler={valueChangeHandler} language={props.language}></ArcColourPicker>;
            break;
        case "reportfilter":
            fieldToDisplay = <ReportFilter specimenid={props.id} language={props.language} config={props.config} changeHandler={valueChangeHandler}></ReportFilter>
            break;
        case "fieldselector":
            const fieldSelectorData = (props.config.value || []).map(f => ({
                id: f.Value ?? f.value ?? f.id,
                label: f.Label ?? f.label,
                value: f.Value ?? f.value ?? f.id
            }));
            const fieldSelectorConfig = props.data?.fieldOptions && props.config.OptionsName === 'fieldOptions'
                ? { ...props.config, Options: props.data.fieldOptions }
                : props.config;
            const fieldSelectorChangeHandler = (fieldId, newData) => {
                const forBackend = (newData || []).map(r => ({ Label: r.label ?? r.Label, Value: r.value ?? r.Value ?? r.id }));
                props.changeHandler(props.config.Id, forBackend);
            };
            fieldToDisplay = <ArcFieldSelector config={fieldSelectorConfig} data={fieldSelectorData} changeHandler={fieldSelectorChangeHandler} language={props.language}></ArcFieldSelector>
            break;
        case "ruleseditor":
            fieldToDisplay = <RulesEditor
                rules={props.config.value || []}
                onChange={(rules) => valueChangeHandler(null, rules)}
                fieldOptions={props.data?.fieldOptions || []}
                effectOptions={props.data?.effectOptions || []}
                context={props.config.Context || 'formgroup'}
                language={props.language}
                lists={props.lists || []}
            />
            break;
        case "pagerules":
            fieldToDisplay = <PageRulesEditor
                value={props.config.value || {}}
                onChange={(pageRules) => valueChangeHandler(null, pageRules)}
                stateOptions={props.data?.stateOptions || []}
                fieldOptions={props.data?.fieldOptions || []}
                stateLocked={props.data?.stateLocked === true}
                language={props.language}
                lists={props.lists || []}
            />
            break;
        case "pageorder":
            fieldToDisplay = <PageOrderEditor
                value={props.config.value || []}
                onChange={(pageOrder) => valueChangeHandler(null, pageOrder)}
                language={props.language}
            />
            break;
        case "parentlinkeditor":
            fieldToDisplay = <ParentLinkEditor
                value={props.config.value || ''}
                onChange={(parentList) => valueChangeHandler(null, parentList)}
                childLists={props.data?.ChildLists || props.data?.childLists || []}
                pageListFields={props.data?.PageListFields || props.data?.pageListFields || []}
                selectedListId={props.data?.List ?? props.data?.list}
                language={props.language}
            />
            break;
        case "existingfieldselector":
            fieldToDisplay = <ExistingFieldSelector
                value={props.config.value || ''}
                onChange={(existingFieldIds) => valueChangeHandler(null, existingFieldIds)}
                options={props.data?.ExistingFieldOptions || props.data?.existingFieldOptions || []}
                targetTable={props.data?.TargetTable || props.data?.targetTable}
                language={props.language}
            />
            break;
        case "json":
            fieldToDisplay = <ArcJson label={props.config.Label} value={props.config.value} readOnly = {true}/>;
            break;        
        case "text":
            fieldToDisplay = <TextField id={props.config.Id} label={props.config.Label} disabled required={props.config.Required} onChange={valueChangeHandler} focusOut={props.focusOut} value={resolveDisplayValue(props.config.value, props.language)} readOnly = {true}/>;
            break;
        case "multitext":
            fieldToDisplay = <TextField id={props.config.Id} label={props.config.Label} multiline autoAdjustHeight required={props.config.Required} onChange={valueChangeHandler} focusOut={props.focusOut} value={resolveDisplayValue(props.config.value, props.language)} readOnly = {true}/>;
            break;
        case "plaintext":
            fieldToDisplay = <ArcPlainText config={props.config} value={props.config.value}></ArcPlainText>  
            break; 
        case "organismlist":
            fieldToDisplay = <ArcOrganismList config={props.config} value={props.config.value} language={props.language} changeHandler={valueChangeHandler} onKeyDown={props.onKeyDown}></ArcOrganismList>
            break;
        case "hierarchy":
            fieldToDisplay = <ArcHierarchy config={props.config} changeHandler={props.changeHandler}></ArcHierarchy>
            break;
        case "upload":
            fieldToDisplay = <ArcUpload config={props.config} changeHandler={props.changeHandler}></ArcUpload>
            break;
        default:
            break;
    }

    let fieldClass = "singlelinefield-data";
    if (props.config.Width !== undefined) {
        if (props.config.Width === "narrow") {
            fieldClass = "singlelinefield-narrow"
        }
        if (props.config.Width === "medium") {
            fieldClass = "singlelinefield-medium"
        }
    }

    return (
        <div className="singlelinefield-item">
            <div className={fieldClass}>
                {fieldToDisplay}
            </div>
        </div>
    )
};

export default SingleLineField;