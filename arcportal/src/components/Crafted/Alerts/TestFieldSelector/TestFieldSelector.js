import React, {useState, useEffect} from 'react';
import { IconButton, Label, TooltipHost } from '@fluentui/react';
import FieldGridLine from '../../../Forms/FieldGrid/FieldGridLine/FieldGridLine';
import { connect } from 'react-redux';
import { PostList } from '../../../../Data/Post';
import GetFieldsForForm from '../../../../Utils/Forms/GetFieldsForForm';
import GetIndividualFieldDetails from '../../../../Utils/Forms/GetIndividualFieldDetails';
import TranslateTag from '../../../../Utils/Local/TranslateTag';

const TestFieldSelector = (props) => {
    const [gridLines, setGridLines] = useState([]);
    const [gridData, setGridData] = useState();
    const [testOptions, setTestOptions] = useState();

    useEffect(() => {
        let newLines = [];
        let data = [];
        const gridFields = [...props.config.GridFields];
        if (
            props.config.value === undefined ||
            !Array.isArray(props.config.value) ||
            props.config.value.length === 0
        ) {
            newLines.push({
                line: setOptionsAndVisibility(props.config.GridFields),
                number: 1,
            });
            setGridData([]);
        } else {
            for (const value of props.config.value) {
                const newGridField = [];
                for (const field of gridFields) {
                    const newField = { ...field };
                    if (value[field.Id]) {
                        newField.value = value[field.Id].toString();
                    }
                    newGridField.push(newField);
                }
                newLines.push({
                    line: setOptionsAndVisibility(newGridField, testOptions),
                    number: newLines.length + 1,
                });

                const newLineData = { ...value, lineNumber: newLines.length };

                data.push(newLineData);

                newLines = updateFieldOptions(
                    newLineData.lineNumber,
                    'Test',
                    value.Test,
                    newLines
                );
                if (value.Field) {
                    newLines = updateComparison(
                        data,
                        newLineData.lineNumber,
                        'Field',
                        value.Field,
                        newLines
                    );
                }
            }

            setGridData(data);
        }
        setGridLines(newLines);

        GetTestConfigList(newLines, props.config.GridFields[0].OptionsName);
    }, [props.config]);

    const addLine = () => {
        const newLines = [...gridLines];
        newLines.push({
            line: setOptionsAndVisibility(props.config.GridFields, testOptions),
            number: newLines.length + 1,
        });
        setGridLines(newLines);
    };

    const setOptionsAndVisibility = (gridFields, options) => {
        return gridFields.map(field => ({
          ...field, 
          Visible: field.Id === "Test",
          Options: field.Id === "Test" ? options : undefined 
        }));
      };

    const removeLine = (number) => {
        const newLinesToSave = gridLines
            .filter((f) => f.number !== number)
            .map((line, i) => ({ ...line, number: i + 1 }));

        setGridLines(newLinesToSave);

        const newData = gridData
            .filter((f) => f.lineNumber !== number)
            .map((line, i) => ({ ...line, lineNumber: i + 1 }));

        setGridData(newData);
        sendToChangeHandler(newData);
    };

    const gridChangeHandler = (lineNumber, field, value) => {
        let data = [...gridData];
        let found = false;

        for (const line of data) {
            if (line.lineNumber === lineNumber) {
                found = true;
                line[field] = value;

            }
        }
        if (!found) {
            const newLine = { lineNumber: lineNumber };
            newLine[field] = value;
            data.push(newLine);
        }

        let newGridLines = updateFieldOptions(lineNumber, field, value, [
            ...gridLines,
        ]);
        if (field === 'Test')
        {
            data = clearGridData(lineNumber, 'Test', data);
        }
        newGridLines = updateComparison(
            data,
            lineNumber,
            field,
            value,
            newGridLines
        );

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

    const sendToChangeHandler = (data) => {
        const changedData = [];
        for (const line of data) {
            const changedLine = { ...line };
            delete changedLine.lineNumber;
            changedData.push(changedLine);
        }
        props.changeHandler(undefined, changedData);
    };

    const updateFieldOptions = (lineNumber, field, value, lines) => {
        if (field === 'Test') {
            const fieldList = GetFieldsForForm(
                value,
                props.forms,
                props.pages,
                false
            );
            for (const line of lines) {
                if (line.number === lineNumber) {
                    // update field options
                    line.line[1].Options = fieldList.map((el) => ({
                        key: el.id,
                        text: el.field,
                    }));
                    line.line[1].Visible = true;
                    for (let i = 2; i < 6; i++) {
                        line.line[i].Visible = false;
                        line.line[i].Value = null;
                    }
                }
            }
        }

        return lines;
    };

    const updateComparison = (data, lineNumber, field, value, lines) => {
        if (field === 'Field') {
            const formName = data.filter(
                (el) => el.lineNumber === lineNumber
            )[0].Test;

            for (const line of lines) {
                if (line.number === lineNumber) {
                    line.line[3].Visible = false;
                    line.line[4].Visible = false;
                    line.line[5].Visible = false;
                    const field = GetIndividualFieldDetails(
                        formName,
                        props.forms,
                        props.pages,
                        value
                    );
                    switch (field.Type.toLowerCase()) {
                        case 'number':
                            line.line[2].Options = [
                                { key: '>', text: '>' },
                                { key: '>=', text: '>=' },
                                { key: '=', text: '=' },
                                { key: '<=', text: '<=' },
                                { key: '<', text: '<' },
                            ];
                            line.line[4].Visible = true;
                            line.line[4].Max = field.Max;
                            line.line[4].Min = field.Min;
                            line.line[4].Step = field.Step;
                            break;
                        case 'combobox':
                        case 'dropdown':
                            line.line[2].Options = [{ key: '=', text: '=' }];
                            line.line[5].Visible = true;
                            const list = props.lists.filter(
                                (l) =>
                                    l.Name.toLowerCase() ===
                                    field.OptionsName.toLowerCase()
                            );
                            line.line[5].Options = list[0].Options.map(
                                (option) => {
                                    return {
                                        key: option.Key,
                                        text: option.Text,
                                        ParentKey: option.ParentKey,
                                    };
                                }
                            );
                            break;
                        default:
                            line.line[2].Options = [{ key: '=', text: '=' }];
                            line.line[3].Visible = true;
                    }
                    line.line[2].Visible = true;
                }
            }
        }
        return lines;
    };

    const GetTestConfigList = (newLines, optionsName) => {
        PostList(
            [{ Name: optionsName, IncludeFixed: true, Translate: true }],
            dynamicListsRetrieved,
            errorWhenRetrievingData,
            { lines: newLines } // "extraInfo"
        );
    };

    const dynamicListsRetrieved = (data, extraInfo) => {
        for (const line of extraInfo.lines) {
            line.line[0].Options = data[0].options;
        }

        const returnList = new Array();
        for (const test of data[0].options) {
            let foundItem = props.forms.filter(
                (f) => f.Name.toLowerCase() === test.key.toLowerCase()
            );
            if (foundItem.length > 0) {
                returnList.push(test);
            }
        }
        setTestOptions(returnList);
    };

    const errorWhenRetrievingData = (response) => {
        if (props.errorHandler !== undefined) {
            props.errorHandler(response);
        }
    };


    const clearGridData = (lineNumber, key, gridData) => {
        if (isArrayNullOrEmpty(gridData)) return;
        const updatedGridData = [...gridData];
        const dataKeys = [
            'Test',
            'Field',
            'Comparison',
            'ListValue',
            'NumberValue',
            'StringValue',
            'Matched',
        ];
        const startIndex = dataKeys.indexOf(key);
        if (startIndex === -1) return;
        const item = updatedGridData.find(
            (item) => item.lineNumber === lineNumber
        );
        for (let i = startIndex + 1; i < dataKeys.length; i++) {
            const key = dataKeys[i];
            delete item[key];
        }

        return gridData;
    };

    const isArrayNullOrEmpty = (arr) =>
        arr === null || arr === undefined || arr.length === 0;

    const showAddIcon = props.config.RemoveGridAddButton === undefined || !props.config.RemoveGridAddButton;
    const addIcon = showAddIcon ? (
        <TooltipHost content={TranslateTag('@GenAddI@', props.language)} calloutProps={{ gapSpace: 10 }}>
            <IconButton iconProps={{ iconName: 'Add' }} onClick={addLine} ariaLabel={TranslateTag('@GenAddI@', props.language)} />
        </TooltipHost>
    ) : null;

    return (
        <React.Fragment>
            <div>
                <div className="fieldgrid-head">
                    <Label>{props.config.Label}</Label>
                    {addIcon}
                </div>
                {gridLines.map((config) => (
                    <div className="fieldgrid-row">
                        <FieldGridLine
                            key={config.number}
                            lineNumber={config.number}
                            config={config.line}
                            changeHandler={gridChangeHandler}
                            data={gridData}
                        ></FieldGridLine>
                        <IconButton
                            iconProps={{ iconName: 'Cancel' }}
                            onClick={() => {
                                removeLine(config.number);
                            }}
                        />
                    </div>
                ))}
            </div>
        </React.Fragment>
    );
};

const mapStateToProps = (state) => {
    return {
        forms: state.config.forms,
        pages: state.config.pages,
        lists: state.config.lists,
        language: state.config.language,
    };
};

export default connect(mapStateToProps)(TestFieldSelector);
