import React, { useState, useEffect, useRef } from 'react';
import { IconButton, Label, TooltipHost } from '@fluentui/react';
import FieldGridLine from '../../../Forms/FieldGrid/FieldGridLine/FieldGridLine';
import { connect } from 'react-redux';
import Post from '../../../../Data/Post';
import GetIndividualFieldDetails from '../../../../Utils/Forms/GetIndividualFieldDetails';
import TranslateTag from '../../../../Utils/Local/TranslateTag';

/** Sibling combination-rule field id and its default AndOr list item id (list 103: 987 = And). */
const COMBINATION_RULE_FIELD_ID = 'InterfaceCriteriaAndOr';
const DEFAULT_COMBINATION_RULE_ID = '987';

/**
 * Crafted grid for Custom interface type criteria. Mirrors the Alert "Test alert criteria" grid
 * (TestFieldSelector) but omits the Test column: the Field column is populated with the fields of the
 * export profile selected on the same form (props.data.ExportProfileId). Each row is stored/matched by
 * id — Field holds the export profile field id and ListValue holds a list item id.
 *
 * Column order (from page config gridfields): Field(0), Comparison(1), StringValue(2), NumberValue(3), ListValue(4).
 *
 * @param {Object} props
 * @param {Object} props.config - Crafted field config (Id, Label, GridFields, value[]).
 * @param {Object} props.data - Current form values; ExportProfileId is read from here.
 * @param {function} props.changeHandler - Called (undefined, rows[]) when the grid changes.
 */
const InterfaceCriteriaSelector = (props) => {
    const [gridLines, setGridLines] = useState([]);
    const [gridData, setGridData] = useState([]);
    const [fieldOptions, setFieldOptions] = useState([]);
    const fieldMetaRef = useRef({});
    const builtForProfileRef = useRef(null);

    const exportProfileId = getExportProfileId(props.data);
    const combinationRuleValue = getCombinationRule(props.data);

    // The framework clears a field's value when its form group becomes invisible. The combination rule
    // starts hidden (no export profile selected) so its configured default is lost by the time the grid
    // is shown. Re-apply the default (And) when this grid is displayed and no rule has been chosen.
    useEffect(() => {
        if (exportProfileId && !combinationRuleValue && props.fieldChangeHandler) {
            props.fieldChangeHandler(COMBINATION_RULE_FIELD_ID, DEFAULT_COMBINATION_RULE_ID);
        }
    }, [exportProfileId, combinationRuleValue]);

    useEffect(() => {
        if (!exportProfileId) {
            fieldMetaRef.current = {};
            setFieldOptions([]);
            setGridLines([]);
            setGridData([]);
            builtForProfileRef.current = null;
            return;
        }

        Post(
            'query/filteredget',
            { Name: 'exportprofilerecordview', Parameters: [{ Key: 'exportprofileid', Value: exportProfileId.toString() }] },
            profileFieldsRetrieved,
            errorWhenRetrievingData,
            { profileId: exportProfileId }
        );
    }, [exportProfileId]);

    const profileFieldsRetrieved = (data, extraInfo) => {
        const fields = Array.isArray(data) ? data : [];
        const meta = {};
        const options = fields.map((field) => {
            const id = (field.Id ?? field.id).toString();
            const formName = field.FormName ?? field.formName;
            const fieldName = field.FieldName ?? field.fieldName;
            const label = field.HeaderName ?? field.headerName ?? field.LabelName ?? field.labelName ?? fieldName;
            meta[id] = { formName, fieldName };
            return { key: id, text: label };
        });

        fieldMetaRef.current = meta;
        setFieldOptions(options);

        // On the first build for this profile use any persisted rows; if the user switched to a different
        // profile, start clean because the previous field ids do not belong to the new profile.
        const isSameProfileAsBuilt = builtForProfileRef.current === extraInfo.profileId.toString();
        const persisted = isFirstBuild() || isSameProfileAsBuilt ? props.config.value : [];
        builtForProfileRef.current = extraInfo.profileId.toString();
        buildGrid(persisted, options);
    };

    const isFirstBuild = () => builtForProfileRef.current === null;

    const buildGrid = (rows, options) => {
        let newLines = [];
        const data = [];

        if (!Array.isArray(rows) || rows.length === 0) {
            newLines.push({ line: newLineConfig(options), number: 1 });
            setGridData([]);
            setGridLines(newLines);
            return;
        }

        for (const row of rows) {
            const number = newLines.length + 1;
            const lineConfig = newLineConfig(options);
            const rowData = { ...row, lineNumber: number };
            applyRowState(lineConfig, rowData);
            newLines.push({ line: lineConfig, number });
            data.push(rowData);
        }

        setGridData(data);
        setGridLines(newLines);
    };

    const newLineConfig = (options) => {
        return props.config.GridFields.map((field) => ({
            ...field,
            Visible: field.Id === 'Field',
            Options: field.Id === 'Field' ? options : undefined,
        }));
    };

    const applyRowState = (lineConfig, rowData) => {
        for (const element of lineConfig) {
            if (rowData[element.Id] !== undefined && rowData[element.Id] !== null) {
                element.value = rowData[element.Id].toString();
            }
        }
        if (rowData.Field) {
            applyComparison(lineConfig, rowData.Field);
        }
    };

    const resolveFieldDetails = (fieldId) => {
        const meta = fieldMetaRef.current[fieldId];
        if (!meta || !meta.formName || !meta.fieldName) {
            return undefined;
        }
        return GetIndividualFieldDetails(meta.formName, props.forms, props.pages, meta.fieldName);
    };

    const applyComparison = (lineConfig, fieldId) => {
        const comparison = lineConfig[1];
        const stringValue = lineConfig[2];
        const numberValue = lineConfig[3];
        const listValue = lineConfig[4];

        stringValue.Visible = false;
        numberValue.Visible = false;
        listValue.Visible = false;

        const details = resolveFieldDetails(fieldId);
        const type = details && details.Type ? details.Type.toLowerCase() : 'singleline';

        switch (type) {
            case 'number':
                comparison.Options = [
                    { key: '>', text: '>' },
                    { key: '>=', text: '>=' },
                    { key: '=', text: '=' },
                    { key: '<=', text: '<=' },
                    { key: '<', text: '<' },
                ];
                numberValue.Visible = true;
                numberValue.Max = details.Max;
                numberValue.Min = details.Min;
                numberValue.Step = details.Step;
                break;
            case 'combobox':
            case 'dropdown':
                comparison.Options = [{ key: '=', text: '=' }];
                listValue.Visible = true;
                const list = (props.lists || []).filter(
                    (l) => l.Name.toLowerCase() === (details.OptionsName || '').toLowerCase()
                );
                listValue.Options = list.length > 0
                    ? list[0].Options.map((option) => ({ key: option.Key, text: option.Text, ParentKey: option.ParentKey }))
                    : [];
                break;
            default:
                comparison.Options = [{ key: '=', text: '=' }];
                stringValue.Visible = true;
        }
        comparison.Visible = true;
    };

    const addLine = () => {
        const newLines = [...gridLines];
        newLines.push({ line: newLineConfig(fieldOptions), number: newLines.length + 1 });
        setGridLines(newLines);
    };

    const removeLine = (number) => {
        const newLines = gridLines
            .filter((f) => f.number !== number)
            .map((line, i) => ({ ...line, number: i + 1 }));
        setGridLines(newLines);

        const newData = gridData
            .filter((f) => f.lineNumber !== number)
            .map((line, i) => ({ ...line, lineNumber: i + 1 }));
        setGridData(newData);
        sendToChangeHandler(newData);
    };

    const gridChangeHandler = (lineNumber, field, value) => {
        let data = [...gridData];
        let rowIndex = data.findIndex((line) => line.lineNumber === lineNumber);
        if (rowIndex === -1) {
            data.push({ lineNumber: lineNumber });
            rowIndex = data.length - 1;
        }
        data[rowIndex] = { ...data[rowIndex], [field]: value };

        if (field === 'Field') {
            data[rowIndex] = { lineNumber: lineNumber, Field: value };
        }

        const newGridLines = [...gridLines];
        for (const line of newGridLines) {
            if (line.number === lineNumber) {
                for (const element of line.line) {
                    if (element.Id === field) {
                        element.value = value;
                    }
                }
                if (field === 'Field') {
                    for (let i = 1; i < line.line.length; i++) {
                        line.line[i].value = null;
                    }
                    applyComparison(line.line, value);
                }
            }
        }

        setGridLines(newGridLines);
        setGridData(data);
        sendToChangeHandler(data);
    };

    const sendToChangeHandler = (data) => {
        const changedData = data.map((line) => {
            const changedLine = { ...line };
            delete changedLine.lineNumber;
            return changedLine;
        });
        props.changeHandler(undefined, changedData);
    };

    const errorWhenRetrievingData = (response) => {
        if (props.errorHandler !== undefined) {
            props.errorHandler(response);
        }
    };

    const showAddIcon = props.config.RemoveGridAddButton === undefined || !props.config.RemoveGridAddButton;
    const addIcon = showAddIcon ? (
        <TooltipHost content={TranslateTag('@GenAddI@', props.language)} calloutProps={{ gapSpace: 10 }}>
            <IconButton
                iconProps={{ iconName: 'Add' }}
                onClick={addLine}
                data-test="interfacecriteria-add"
                ariaLabel={TranslateTag('@GenAddI@', props.language)}
            />
        </TooltipHost>
    ) : null;

    return (
        <React.Fragment>
            <div data-test="interfacecriteria-grid">
                <div className="fieldgrid-head">
                    <Label>{props.config.Label}</Label>
                    {addIcon}
                </div>
                {gridLines.map((config) => (
                    <div className="fieldgrid-row" data-test={`interfacecriteria-row-${config.number}`} key={config.number}>
                        <FieldGridLine
                            key={config.number}
                            lineNumber={config.number}
                            config={config.line}
                            changeHandler={gridChangeHandler}
                            data={gridData}
                        ></FieldGridLine>
                        <IconButton
                            iconProps={{ iconName: 'Cancel' }}
                            data-test={`interfacecriteria-remove-${config.number}`}
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

/**
 * Reads the selected export profile id from the current form values, tolerant of key casing.
 * @param {Object} data - Current form values.
 * @returns {string|undefined}
 */
const getExportProfileId = (data) => {
    if (!data) return undefined;
    const key = Object.keys(data).find((k) => k.toLowerCase() === 'exportprofileid');
    const value = key ? data[key] : undefined;
    return value === undefined || value === null || value.toString() === '' ? undefined : value;
};

/**
 * Reads the current interface combination rule value from the form values, tolerant of key casing.
 * @param {Object} data - Current form values.
 * @returns {string|undefined}
 */
const getCombinationRule = (data) => {
    if (!data) return undefined;
    const key = Object.keys(data).find((k) => k.toLowerCase() === COMBINATION_RULE_FIELD_ID.toLowerCase());
    const value = key ? data[key] : undefined;
    return value === undefined || value === null || value.toString() === '' ? undefined : value;
};

const mapStateToProps = (state) => {
    return {
        forms: state.config.forms,
        pages: state.config.pages,
        lists: state.config.lists,
        language: state.config.language,
    };
};

export default connect(mapStateToProps)(InterfaceCriteriaSelector);
