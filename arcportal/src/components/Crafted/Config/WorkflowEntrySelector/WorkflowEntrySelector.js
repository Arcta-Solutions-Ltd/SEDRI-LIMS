import React, {useState, useEffect} from 'react';
import FieldGridLine from '../../../Forms/FieldGrid/FieldGridLine/FieldGridLine';
import { IconButton, Label, TooltipHost } from '@fluentui/react';
import { setGridLinesUtil, setGridDataUtil, resetLineNumberUtil } from './SelectorGridUtils';
import {connect} from 'react-redux';
import Post, { PostList } from '../../../../Data/Post';
import { GetIndividualFieldAcrossAllForms } from '../../../../Utils/Forms/GetIndividualFieldDetails';
import TranslateTag from '../../../../Utils/Local/TranslateTag';
import { getStandardTooltipProps } from '../../../../Utils/General/StandardTooltipProps';
import { buildGridHeaderCells } from '../../../Forms/FieldGrid/FieldGridLayoutUtils';

const WorkflowEntrySelector = (props) => {

    const [gridLines, setGridLines] = useState([]);
    const [gridData, setGridData] = useState();
    const [listData, setListData] = useState();
    const [fieldListData, setFieldListData] = useState();
    const [gridTitles, setGridTitles] = useState([]);

    useEffect(() => {
        let newLines = [];
        const data = [];
        const gridFields = [...props.config.GridFields]
        if (props.config.value === undefined || ! Array.isArray(props.config.value) || props.config.value.length === 0) {
            newLines.push({ 'line': copyGridFields(props.config.GridFields), 'number': newLines.length+1});
            setGridData([]);
        } else 
        {
            for (const value of props.config.value) { 
                const newGridField = [];
                for (const field of gridFields) {
                    const newField = {...field};
                    if (value[field.Id] !== undefined && value[field.Id] !== null) {
                        newField.value = value[field.Id].toString();
                    }
                    newGridField.push(newField);
                }
                newLines.push({ 'line': copyGridFields(newGridField, listData, fieldListData), 'number': newLines.length+1 });

                const newLineData = {...value, lineNumber: newLines.length};

                data.push(newLineData);

                newLines = updateFieldOptions(newLineData.lineNumber,"ExitState", value.Test, newLines);
                newLines = setValueFieldVisibility(newLineData.lineNumber, newLineData.Field, newLines);
            }

            setGridData(data);
        }
        setGridLines(newLines);

        const referenceRowData = data.length > 0 ? data[0] : {};
        setGridTitles(buildGridHeaderCells(props.config.GridFields, referenceRowData));

        LoadConditionList(newLines);

    }, [props.config])

    const gridChangeHandler = (lineNumber, field, value) => {
        const data = [...gridData];
        let found = false;

        for (const line of data) {
            if (line.lineNumber === lineNumber) {
                found = true;
                line[field] = value;
            }
        }
        if (! found) {
            const newLine = { lineNumber: lineNumber};
            newLine[field] = value;
            data.push(newLine);
        }

        let newGridLines = updateFieldOptions(lineNumber,field, value,[...gridLines]);

        if (field == "Field") {
            newGridLines = setValueFieldVisibility(lineNumber,value, newGridLines);
        }

        for (const line of newGridLines) {
            if (line.number === lineNumber) {
                for (const element of line.line) {
                    if (element.Id === field) {
                        element.value = value;
                    }
                }
            }
        }
        
        setGridLines(newGridLines);

        setGridData(data);

        sendToChangeHandler(data);
    };

    const removeLine = (number) => {
        setGridLines(setGridLinesUtil(number, gridLines));

        var newData = setGridDataUtil(number,gridData);
        setGridData(newData);
        sendToChangeHandler(newData);
    }

    const sendToChangeHandler = (data) => {
        const changedData = resetLineNumberUtil(data);
        props.changeHandler(undefined,changedData);
    }

    const addLine = () => {
        const newLines = [...gridLines];
        newLines.push({ 'line': copyGridFields(props.config.GridFields, listData, fieldListData), 'number': newLines.length+1});
        setGridLines(newLines);
    }

    const copyGridFields = (gridFields, options, fieldOptions) => {
        const newGrid = [];
        for (const field of gridFields) {
            let newField = {...field, Visible: field.Id === "ExitState" || field.Id === "Field"};
            if (options !== undefined && field.Id === "ExitState") { newField.Options = options;}
            if (fieldOptions !== undefined && field.Id === "Field") { newField.Options = fieldOptions;}
            newField.GridTitle = field.GridTitle;
            newGrid.push(newField);
        }
        return newGrid;
    }

    const updateFieldOptions = (lineNumber, field, value, lines) => {

        if (field === "ExitState" || field === "Field") {
            for (const line of lines) {
                if (line.number === lineNumber) {
                    line.line[1].Options = fieldListData;
                    line.line[1].Visible = true;
                }
            }
        }

        return lines;
    }

    const setValueFieldVisibility = (lineNumber, value, lines) => {
        for (const line of lines) {
            if (line.number === lineNumber) {
                line.line[2].Visible = false;
                line.line[3].Visible = false;
                line.line[4].Visible = false;
                if (value !== undefined && value !== null) {
                    const field = GetIndividualFieldAcrossAllForms(props.forms, props.pages, value)
                    if(field !== undefined){
                    switch (field.Type.toLowerCase()) {
                        case "number":
                            line.line[3].Visible = true;
                            line.line[3].Max = field.Max;
                            line.line[3].Min = field.Min;
                            line.line[3].Step = field.Step;
                            break;
                        case "radio":
                        case "combobox":
                        case "dropdown":
                            line.line[4].Visible = true;
                            const list = props.lists.filter(l => l.Name.toLowerCase() === field.OptionsName.toLowerCase());
                            line.line[4].Options =  list[0].Options.map((option) => { return {key: option.Key, text: option.Text, ParentKey: option.ParentKey}});
                            break;
                        default:
                            line.line[2].Visible = true;
                    }
                 }
                }
            }
            //setGridLines(lines);
        }
        return lines;
    }
    
    const LoadConditionList = (newLines, event) => {
        PostList([{ Name: "specimenworkflowitems", IncludeFixed: true, Translate: true}], dynamicListsRetrieved, errorWhenRetrievingData, { lines: newLines});

        const criteria = { Name: "fieldsforeventquery", Parameters: [{ key: "eventName", value: props.event}] };
        Post('query/filteredget', criteria, fieldListsRetrieved, errorWhenRetrievingData, { lines: newLines, number: 1});
    }

    const dynamicListsRetrieved = (data, extraInfo) => {
        for (const line of extraInfo.lines) {
            line.line[0].Options = data[0].options;
        }
        setListData(data[0].options);
    }

    const fieldListsRetrieved = (data, extraInfo) => {
        const optionData = data.map((el) => ( {key: el.Key.toLowerCase(), text: el.Text}));
        for (const line of extraInfo.lines) {
            line.line[1].Options = optionData;
        }
        setFieldListData(optionData);
    }

    const errorWhenRetrievingData = (response) => {
        if (props.errorHandler !== undefined) {
            props.errorHandler(response);
        };
    }

    const addLineTooltip = TranslateTag("@GenAddI@", props.language);
    const deleteLineTooltip = TranslateTag("@GenDel@", props.language);
    const tooltipProps = getStandardTooltipProps();
    const calloutProps = { gapSpace: 10 };

    const showAddIcon = props.config.RemoveGridAddButton === undefined || !props.config.RemoveGridAddButton;
    const addIcon = showAddIcon ? (
        <TooltipHost content={addLineTooltip} tooltipProps={tooltipProps} calloutProps={calloutProps}>
            <IconButton id="workflowentry-add-btn" iconProps={{ iconName: 'Add' }} onClick={addLine} ariaLabel={addLineTooltip} />
        </TooltipHost>
    ) : null;
    
    return (
        <React.Fragment>
            <div>
                <div className="fieldgrid-head">
                    <Label>{props.config.Label}</Label>
                    {addIcon}
                </div>
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
                {gridLines.map((config) => (
                    <div className = "fieldgrid-row">
                        <FieldGridLine key={config.number} lineNumber={config.number} config={config.line} changeHandler={gridChangeHandler} data={gridData}></FieldGridLine>
                        <TooltipHost content={deleteLineTooltip} tooltipProps={tooltipProps} calloutProps={calloutProps}>
                            <IconButton
                                id={`workflowentry-delete-${config.number}`}
                                iconProps={{iconName: 'Cancel'}}
                                ariaLabel={deleteLineTooltip}
                                onClick={() => {removeLine(config.number)}}
                            />
                        </TooltipHost>
                    </div>
                ))}
            </div>
        </React.Fragment>
    )
}

const mapStateToProps = state => {
    return {
        forms: state.config.forms,
        pages: state.config.pages,
        lists: state.config.lists,
        language: state.config.language
    };
}

export default connect(mapStateToProps)(WorkflowEntrySelector);