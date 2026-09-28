import React, {useState, useEffect, useMemo} from 'react';
import './FieldGrid.css';
import './FieldGridField/FieldGridField.css';
import FieldGridLine from './FieldGridLine/FieldGridLine';
import { IconButton, Label, TooltipHost } from '@fluentui/react';
import FormHandler from '../../Containers/FormHandler/FormHandler';
import TranslateTag from '../../../Utils/Local/TranslateTag';
import { getStandardTooltipProps } from '../../../Utils/General/StandardTooltipProps';
import GetTextForListItemsInGrid from '../../../Utils/Forms/GetTextForListItemsInGrid';
import ResolveEmbeddedPagesForFieldGrid from '../../../Utils/Forms/ResolveEmbeddedPagesForFieldGrid';
import { buildGridHeaderCells } from './FieldGridLayoutUtils';

const FieldGrid = (props) => {

    const [gridLines, setGridLines] = useState([]);
    const [gridData, setGridData] = useState([]);
    const [gridTitles, setGridTitles] = useState([]);
    const [formStartConfig, setFormStartConfig] = useState({});
    const [draggedItemIndex, setDraggedItemIndex] = useState(null);

    /**
     * Pages from optional parent `embeddedPages` plus pages for subforms referenced by FormUIEvent / AddFormUIEvent,
     * so list keys (FieldFormat) resolve on the parent workflow without opening the subform.
     */
    const effectiveEmbeddedPages = useMemo(() => {
        const resolved = ResolveEmbeddedPagesForFieldGrid(
            props.config,
            props.uievents,
            props.forms,
            props.allPages
        );
        const fromProps = Array.isArray(props.embeddedPages) ? props.embeddedPages : [];
        return [...fromProps, ...resolved];
    }, [props.config, props.embeddedPages, props.allPages, props.uievents, props.forms]);

    const calloutProps = { gapSpace: 10 };
    const tooltipProps = getStandardTooltipProps();
    let tooltipId = 1;

    const gridRootId = props.config.Id ?? props.config.id ?? 'fieldgrid';

    /**
     * Builds per-row field configs with unique DOM ids while preserving FieldKey for save payload keys.
     * @param {number} lineNumber - 1-based row index in the grid.
     * @returns {Array<Object>} Field configs for one grid row.
     */
    const buildLineFields = (lineNumber) =>
        props.config.GridFields.map((field) => ({
            ...field,
            FieldKey: field.FieldKey ?? field.Id,
            Id: `${gridRootId}-line-${lineNumber}-${field.Id}`,
            value: undefined,
        }));

    const createEmptyLine = (lineNumber) => buildLineFields(lineNumber);

    const buildButtonConfig = (uiEvent) => (props.config.onFinish !== undefined && props.config.onFinish !== "")
        ? { UIEvent: uiEvent, OnFinish: props.config.onFinish }
        : { UIEvent: uiEvent };

    const getRecordIdForLine = (lineNumber) => {
        const zeroBasedIndex = lineNumber - 1;
        if (Array.isArray(props.config.value) && props.config.value[zeroBasedIndex]?.Id !== undefined) {
            return props.config.value[zeroBasedIndex].Id;
        }
        const currentLine = gridData.find(line => line.lineNumber === lineNumber);
        return currentLine?.Id;
    };

    /**
     * Composite row id for grids with multiple logical rows sharing a parent record id (e.g. AST parent + special consideration).
     * @param {number} lineNumber - 1-based row index.
     * @returns {string|undefined}
     */
    const getCompositeRowId = (lineNumber) => {
        const zeroBasedIndex = lineNumber - 1;
        const fromProps = Array.isArray(props.config.value) ? props.config.value[zeroBasedIndex] : undefined;
        const fromData = gridData.find((line) => line.lineNumber === lineNumber);
        const line = fromData ?? fromProps;
        if (line?.Id !== undefined && line?.Id !== null && line?.Id !== '') {
            const specialId = line.SpecialConsiderationId ?? line.specialConsiderationId ?? 0;
            return `${line.Id}-${specialId}`;
        }
        const recordId = getRecordIdForLine(lineNumber);
        return recordId !== undefined && recordId !== null ? String(recordId) : undefined;
    };

    const cloneLineDefinition = (line) => {
        if (!line) {
            return line;
        }

        return {
            ...line,
            line: Array.isArray(line.line) ? line.line.map(field => ({...field})) : []
        };
    };

    const normaliseSavedArray = (savedArray) => (Array.isArray(savedArray) ? savedArray : []);

    const DURATION_FIELD_IDS = ['RangeFromDays', 'RangeFromHours', 'RangeFromMinutes', 'RangeToDays', 'RangeToHours', 'RangeToMinutes'];

    const getLineValueCaseInsensitive = (lineValues, element) => {
        if (lineValues[element] !== undefined && lineValues[element] !== null) {
            return lineValues[element];
        }
        const key = Object.keys(lineValues).find((k) => k.toLowerCase() === element.toLowerCase());
        return key !== undefined ? lineValues[key] : undefined;
    };

    /**
     * Gets the display value for a grid field, resolving FieldFormat tokens (e.g. @AntibioticId@) from displayData or lineValues.
     * @param {Object} field - Grid field config with Id, FieldFormat.
     * @param {Object} lineValues - Raw row values.
     * @param {Array<{key: string, value: string}>} displayData - Resolved {key, value} data from GetTextForListItemsInGrid.
     * @returns {string|undefined} Display string or undefined.
     */
    const getDisplayValueForField = (field, lineValues, displayData) => {
        if (field?.FieldFormat) {
            let updatedValue = field.FieldFormat;
            const fieldContents = GetFieldFormat(field.FieldFormat);
            fieldContents.forEach((element) => {
                const rawValue = displayData.find(data => data.key.toLowerCase() === element.toLowerCase())?.value
                    ?? getLineValueCaseInsensitive(lineValues, element);
                const isEmpty = rawValue === undefined || rawValue === null || rawValue === '';
                const displayVal = (isEmpty && DURATION_FIELD_IDS.includes(element))
                    ? '0'
                    : (rawValue !== undefined && rawValue !== null ? rawValue.toString() : '');
                updatedValue = updatedValue.replace("@" + element + "@", displayVal);
            });
            return updatedValue;
        }

        if (lineValues[field.Id] !== undefined && lineValues[field.Id] !== null) {
            return lineValues[field.Id].toString();
        }

        return undefined;
    };

    const applyDisplayValuesToLine = (lineDefinition, displayValues) => {
        if (!lineDefinition) {
            return;
        }

        for (const [fieldId, value] of Object.entries(displayValues)) {
            const targetLine = lineDefinition.line?.find(
                (line) => (line.FieldKey ?? line.Id) === fieldId || line.Id === fieldId
            );
            if (targetLine) {
                targetLine.value = value;
            }
        }
    };

    /**
     * Resolves grid row values to display strings using GetTextForListItemsInGrid and FieldFormat.
     * @param {Object} lineValues - Raw row values.
     * @param {Array<{key: string, value: string|number}>} savedArray - Row data as {key, value} pairs.
     * @param {Array} [pagesOverride] - Page definitions for list resolution (e.g. subform pages on save); if empty or omitted, uses effectiveEmbeddedPages.
     * @returns {Object} Map of field Id to display string.
     */
    const deriveDisplayValues = (lineValues, savedArray, pagesOverride) => {
        const pagesForGrid =
            pagesOverride != null && Array.isArray(pagesOverride) && pagesOverride.length > 0
                ? pagesOverride
                : effectiveEmbeddedPages;
        const displayData = GetTextForListItemsInGrid(savedArray, props.lists, pagesForGrid);
        const displayValues = {};

        props.config.GridFields.forEach((field) => {
            const displayValue = getDisplayValueForField(field, lineValues, displayData);
            if (displayValue !== undefined) {
                displayValues[field.Id] = displayValue;
            }
        });

        return displayValues;
    };

    /**
     * Keys the grid renders, matched on both FieldKey (save payload key) and Id (column id).
     * @returns {Set<string>}
     */
    const getGridColumnKeys = () => {
        const keys = new Set();
        (props.config.GridFields ?? []).forEach((field) => {
            if (field?.Id !== undefined) {
                keys.add(field.Id);
            }
            if (field?.FieldKey !== undefined) {
                keys.add(field.FieldKey);
            }
        });
        return keys;
    };

    /**
     * A subform only returns its own fields, so saved values are merged onto the row rather than replacing it.
     * Keys the grid does not render and the row does not already hold belong to the subform payload alone.
     * @param {Object} newLine - Row seeded with the existing values; mutated in place.
     * @param {Array<{key: string, value: *}>} normalisedArray - Saved subform values.
     * @param {Object|undefined} existingLine - Row being edited, or undefined when adding a row.
     */
    const mergeSavedValuesIntoLine = (newLine, normalisedArray, existingLine) => {
        const columnKeys = existingLine !== undefined && existingLine !== null ? getGridColumnKeys() : undefined;

        normalisedArray.forEach((item) => {
            if (item?.key === undefined) {
                return;
            }
            if (columnKeys === undefined || columnKeys.has(item.key) || Object.prototype.hasOwnProperty.call(newLine, item.key)) {
                newLine[item.key] = item.value;
            }
        });
    };

    const buildLineFromSavedArray = (lineNumber, savedArray, pages, lineDefinition, existingLine) => {
        const normalisedArray = normaliseSavedArray(savedArray);
        const existingValues = {...(existingLine ?? {})};
        delete existingValues.lineNumber;
        delete existingValues.__displayValues;

        const newLine = { ...existingValues, lineNumber, __displayValues: {} };
        mergeSavedValuesIntoLine(newLine, normalisedArray, existingLine);

        const mergedArray = Object.entries(newLine)
            .filter(([key]) => key !== 'lineNumber' && key !== '__displayValues')
            .map(([key, value]) => ({ key, value: value !== undefined && value !== null ? value.toString() : '' }));

        const displayValues = deriveDisplayValues(newLine, mergedArray, pages);
        newLine.__displayValues = displayValues;
        applyDisplayValuesToLine(lineDefinition, displayValues);

        return newLine;
    };

    useEffect(() => {
        const newLines = [];
        const data = [];

        if (props.config.value === undefined || ! Array.isArray(props.config.value) || props.config.value.length === 0) {
            if ((props.config.FormUIEvent === undefined || props.config.FormUIEvent === "") && props.config.IncludeFirstLine) {
                newLines.push({ line: createEmptyLine(1), number: 1 });
            }
            setGridData([]);
        } 
        else {
            for (const value of props.config.value) { 
                const lineNumber = newLines.length + 1;
                const newGridField = createEmptyLine(lineNumber);
                const savedArray = Object.entries(value).map(([key, fieldValue]) => ({ key, value: fieldValue !== undefined && fieldValue !== null ? fieldValue.toString() : '' }));
                const newLineData = {...value, lineNumber: lineNumber, __displayValues: {}};
                const displayValues = deriveDisplayValues(newLineData, savedArray, effectiveEmbeddedPages);
                newLineData.__displayValues = displayValues;

                const lineDefinition = { line: newGridField };
                applyDisplayValuesToLine(lineDefinition, displayValues);
                newLines.push({ 'line': newGridField, 'number': lineNumber});
                data.push(newLineData);
            }
            setGridData(data);
        }
        setGridLines(newLines);

        const referenceRowData = data.length > 0 ? data[0] : {};
        setGridTitles(buildGridHeaderCells(props.config.GridFields, referenceRowData));
    }, [props.config, props.lists, effectiveEmbeddedPages])

    /**
     * Adds a new empty row to the grid. Used when AddFormUIEvent is not configured.
     * @returns {Array} Updated grid lines array.
     */
    const addLine = () => {
        const newLines = [...gridLines];
        const lineNumber = newLines.length + 1;
        const oneLine = createEmptyLine(lineNumber);

        if (gridData.length === newLines.length) {
            newLines.push({ line: oneLine, number: lineNumber });
            setGridLines(newLines);
        }

        return newLines;
    }

    const getLocalFormValuesForLine = (targetLineNumber) => {
        const line = gridData.find(item => item.lineNumber === targetLineNumber);
        if (!line) {
            return {};
        }

        const { lineNumber, __displayValues, ...rest } = line;
        return { ...rest };
    };

    /**
     * Builds field options for RulesEditor from grid columns. Excludes upload-type and fieldgrid-type columns.
     * Includes type and optionsName so dropdown/combobox fields can show value options.
     * @returns {Array<{id: string, label: string, type?: string, optionsName?: string}>}
     */
    const buildFieldOptionsFromGridFields = () => {
        if (!props.config.GridFields || !Array.isArray(props.config.GridFields)) return [];
        const typeLower = (t) => (t || '').toLowerCase();
        return props.config.GridFields
            .filter((g) => typeLower(g.Type) !== 'upload' && typeLower(g.Type) !== 'fieldgrid')
            .map((g) => ({
                id: g.Id,
                label: g.GridTitle || g.Id,
                type: g.Type,
                optionsName: g.OptionsName,
            }));
    };

    const DEFAULT_EFFECT_OPTIONS = [{ id: 'visible', label: '@GenVis@' }];

    const openForm = (line) => {
        setFormStartConfig({
            button: buildButtonConfig(props.config.FormUIEvent), 
            id: getRecordIdForLine(line), 
            view: "specimen",
            showFullScreenButton: false,
            onSave: props.config.onSave,
            refresh: (savedData, pages) => updateGridLineAfterReturningFromForm(line, savedData, pages),
            deferSave: true,
            localFormData: { values: getLocalFormValuesForLine(line), fieldOptions: buildFieldOptionsFromGridFields(), effectOptions: DEFAULT_EFFECT_OPTIONS }
        })        
    }

    /**
     * Opens the add sub-form (AddFormUIEvent). On save, the new row is appended via updateGridAfterReturningFromForm.
     */
    const openAddForm = () => {
        setFormStartConfig({
            button: buildButtonConfig(props.config.AddFormUIEvent),
            id: props.recordid, 
            view: "specimen",
            showFullScreenButton: false,
            onSave: props.config.onSave,
            refresh: updateGridAfterReturningFromForm,
            deferSave: true,
            localFormData: { values: {}, fieldOptions: buildFieldOptionsFromGridFields(), effectOptions: DEFAULT_EFFECT_OPTIONS }
        })        
    }

    const removeLine = (number) => {
        const newData = gridData.filter(f => f.lineNumber !== number);
        let numberCount = 1;
        for (const line of newData) {
            line.lineNumber = numberCount;
            numberCount++;
        }

        const newLinesWithValues = [];
        for (const value of newData) { 
            const newGridField = createEmptyLine(value.lineNumber);
            const displayValues = {};
            props.config.GridFields.forEach((field) => {
                if (value.__displayValues?.[field.Id] !== undefined) {
                    displayValues[field.Id] = value.__displayValues[field.Id];
                } else if (value[field.Id] !== undefined && value[field.Id] !== null) {
                    displayValues[field.Id] = value[field.Id].toString();
                }
            });
            applyDisplayValuesToLine({ line: newGridField }, displayValues);
            newLinesWithValues.push({ 'line': newGridField, 'number': value.lineNumber});
        }

        setGridLines(newLinesWithValues);
        setGridData(newData);
        sendToChangeHandler(newData);
    }

    const gridChangeHandler = (lineNumber, field, value) => {
        const data = [...gridData];
        let found = false;
        const displayValue = value !== undefined && value !== null ? value.toString() : '';
        for (const line of data) {
            if (line.lineNumber === lineNumber) {
                found = true;
                line[field] = value;
                if (line.__displayValues === undefined) {
                    line.__displayValues = {};
                }
                line.__displayValues[field] = displayValue;
            }
        }
        if (! found) {
            const newLine = { lineNumber: lineNumber, __displayValues: { [field]: displayValue }};
            newLine[field] = value;
            data.push(newLine);
        }
        setGridData(data);

        sendToChangeHandler(data);

        for (const line of gridLines) {
            if (line.number === lineNumber) {
                for (const fieldInfo of line.line) {
                    const fieldKey = fieldInfo.FieldKey ?? fieldInfo.Id;
                    if (fieldKey === field || fieldInfo.Id === field) {
                        fieldInfo.value = displayValue;
                    }
                }
            }
        }

    }

    const handleDragStart = (index) => {
        setDraggedItemIndex(index);
    };
    
      const handleDragOver = (event) => {
        event.preventDefault();
      };
    
      const handleDrop = (index) => {
        const updatedItems = [...gridLines];
        const [draggedItem] = updatedItems.splice(draggedItemIndex-1, 1);
        updatedItems.splice(index-1, 0, draggedItem);
    
        updatedItems.forEach((item, index) => {
            item.number = index+1; 
        });

        const dataItems = [...gridData];
        const [dataItem] = dataItems.splice(draggedItemIndex-1, 1);
        dataItems.splice(index-1, 0, dataItem);

        sendToChangeHandler(dataItems);
        setGridLines(updatedItems);
        setGridData(dataItems);
        setDraggedItemIndex(null);
    };

    const sendToChangeHandler = (data) => {
        const changedData = [];
        for (const line of data) {
            const changedLine = {...line};
            delete changedLine.lineNumber;
            delete changedLine.__displayValues;
            changedData.push(changedLine);
        }        
        props.changeHandler(undefined,changedData);
    }

    const updateGridAfterReturningFromForm = (savedData, pages) => {
        const lines = addLine();
        const data = [...gridData];
        const lineNumber = lines.length;
        const updatedLines = lines.map((line, index) => index === lineNumber - 1 ? cloneLineDefinition(line) : line);
        const targetLine = updatedLines[lineNumber - 1];
        const newLine = buildLineFromSavedArray(lineNumber, savedData, pages, targetLine);

        data.push(newLine);
        setGridData(data);
        setGridLines(updatedLines);
        sendToChangeHandler(data);
    }

    const updateGridLineAfterReturningFromForm = (lineNumber, savedData, pages) => {
        const lines = gridLines.map((line) => line.number === lineNumber ? cloneLineDefinition(line) : line);
        const targetLine = lines.find(line => line.number === lineNumber);
        const data = gridData.map((line) => line.lineNumber === lineNumber ? buildLineFromSavedArray(lineNumber, savedData, pages, targetLine, line) : line);

        setGridData(data);
        setGridLines(lines);
        sendToChangeHandler(data);
    }

    const GetFieldFormat = (format) => {

        const input = format;
        const matches = input.match(/@([^@]+)@/g); // Find all strings enclosed in '@'
        const result = matches.map(match => match.slice(1, -1));

        return result;
    }

    const addLineTooltip = TranslateTag("@GenAddI@", props.language);
    const deleteLineTooltip = TranslateTag("@GenDel@", props.language);
    const showAddIcon = props.config.RemoveGridAddButton === undefined || !props.config.RemoveGridAddButton;
    const addIconHandler = (!props.config.AddFormUIEvent || props.config.AddFormUIEvent === "") ? addLine : openAddForm;
    const addIcon = showAddIcon ? (
        <TooltipHost content={addLineTooltip} id={`fieldgrid-add-${tooltipId++}`} tooltipProps={tooltipProps} calloutProps={calloutProps}>
            <IconButton id="fieldgrid-add-btn" iconProps={{ iconName: 'Add' }} onClick={addIconHandler} ariaLabel={addLineTooltip} />
        </TooltipHost>
    ) : null;

    const deleteIcon = props.config.RemoveGridDeleteButton === undefined || ! props.config.RemoveGridDeleteButton;
    const formIcon = props.config.IncludeGridFormButton;

    const gridLineClass = props.config.Draggable ? "fieldgrid-row-draggable" : "fieldgrid-row";

    return (
        <React.Fragment>
            <div id={gridRootId || undefined}>
                {(props.config.Label !== "" && props.config.Label !== undefined) || addIcon ? (
                    <div className="fieldgrid-head">
                        {props.config.Label !== "" && props.config.Label !== undefined ? <Label>{props.config.Label}</Label> : null}
                        {addIcon}
                    </div>
                ) : null}
                {gridTitles.length > 0 ? (
                    <div id="fieldgrid-header-row" className="fieldgrid-row">
                        {gridTitles.map((column) => (
                            <span
                                key={column.id}
                                id={`fieldgrid-header-${column.id}`}
                                className={column.className}
                            >
                                {column.title}
                            </span>
                        ))}
                    </div>
                ) : (null)}
                {gridLines.length > 0 ? gridLines.map((config) => {
                    const compositeRowId = getCompositeRowId(config.number);
                    return (
                    <div
                        id={compositeRowId !== undefined ? `fieldgrid-row-${compositeRowId}` : undefined}
                        className={gridLineClass}
                        data-row-id={compositeRowId}
                        onDragStart={() => handleDragStart(config.number)}
                        onDragOver={handleDragOver}
                        onDrop={() => handleDrop(config.number)}
                        draggable={props.config.Draggable}>
                        <FieldGridLine  onKeyDown={props.onKeyDown} key={config.number} lineNumber={config.number} config={config.line} changeHandler={gridChangeHandler} data={gridData} uievents={props.uievents} forms={props.forms} lists={props.lists} language={props.language} recordId={props.recordid}></FieldGridLine>
                        {deleteIcon && (
                            <TooltipHost content={deleteLineTooltip} tooltipProps={tooltipProps} calloutProps={calloutProps}>
                                <IconButton
                                    id={`fieldgrid-delete-${compositeRowId}`}
                                    iconProps={{iconName: 'Cancel'}}
                                    ariaLabel={deleteLineTooltip}
                                    onClick={() => {removeLine(config.number)}}
                                />
                            </TooltipHost>
                        )}
                        {formIcon && (() => {
                            const openFormTooltip = TranslateTag(props.config.IconText !== undefined ? props.config.IconText : "@GenOpe@", props.language);
                            return (
                        <TooltipHost
                                content={openFormTooltip}
                                id={tooltipId++}
                                tooltipProps={tooltipProps}
                                calloutProps={calloutProps}>
                                <IconButton
                                    id={`fieldgrid-open-${compositeRowId}`}
                                    iconProps={{iconName: props.config.Icon !== undefined ? props.config.Icon : 'KnowledgeArticle'}}
                                    ariaLabel={openFormTooltip}
                                    onClick={() => {openForm(config.number)}}
                                />
                        </TooltipHost>
                            );
                        })()}
                    </div>
                    );
                }) : (
                    <div>
                        ----- {TranslateTag("@GenNon@", props.language)} -----
                        <br /><br />

                    </div>
                )}
            </div>
            {formIcon && <FormHandler startConfig={formStartConfig}></FormHandler>}
        </React.Fragment>
    )
}

export default FieldGrid;

