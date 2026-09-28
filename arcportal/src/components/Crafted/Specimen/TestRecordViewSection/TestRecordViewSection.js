import React, { useState, useEffect } from 'react';
import { InvokeFormHandlerUsingForm, DataRetrievedHandler } from '../../../../Utils/General/FormStateHandler';
import TestRecordViewFieldRenderer from '../TestRecordViewPanel/TestRecordViewFieldRenderer';
import TranslateTag from '../../../../Utils/Local/TranslateTag';
import './TestRecordViewSection.css';

/**
 * Crafted region that displays test record data (normal fields, grid fields, uploads) in read-only mode.
 * Receives region data from testrecordviewbyid query (Id, TestName, TestResults, Status).
 * Fetches full form data via the form's InitialQuery and renders using TestRecordViewFieldRenderer.
 *
 * @param {Object} props - Component props
 * @param {number} props.region - Region index
 * @param {Array} props.data - Region data array (data[region] = { Id, TestName, TestResults, Status })
 * @param {string|number} props.id - Record id
 * @param {string} props.language - Language for translations
 * @param {Array} props.lists - Option lists for resolving dropdown/combobox values
 * @param {Array} props.forms - Form definitions
 * @param {Array} props.pages - Page definitions
 */
const TestRecordViewSection = (props) => {
    const { region, data, id, language, lists, forms, pages } = props;
    const [formDef, setFormDef] = useState(null);
    const [error, setError] = useState(null);

    const regionData = Array.isArray(data) && data[region] !== undefined ? data[region] : null;
    const first = Array.isArray(regionData) ? regionData[0] : regionData?.[0];
    const recordData = first ?? regionData;
    const testName = recordData?.TestName;
    const recordId = recordData?.Id ?? id;

    useEffect(() => {
        if (!testName || !forms || forms.length === 0) {
            setFormDef(null);
            setError(!testName ? null : 'No test data');
            return;
        }

        const form = forms.filter((f) => (f.Name || f.name) === testName);
        if (form.length === 0 || !form[0].InitialQuery) {
            setFormDef(null);
            setError('View not available for this test.');
            return;
        }

        const viewDataRetrievedSuccessfully = (apiData, extraInfo) => {
            setError(null);
            const state = DataRetrievedHandler(
                apiData,
                extraInfo,
                forms,
                pages,
                lists
            );
            setFormDef(state.formDef);
            if (props.onRefreshDone) {
                props.onRefreshDone(false);
            }
        };

        const errorWhenRetrievingData = (err) => {
            setError(err?.data ?? err ?? 'Failed to load test data');
            setFormDef(null);
        };

        InvokeFormHandlerUsingForm(
            form,
            String(recordId),
            viewDataRetrievedSuccessfully,
            errorWhenRetrievingData
        );
    }, [testName, recordId, forms, pages, lists, props.refresh]);

    if (error) {
        return (
            <div className="testrecordview-section testrecordview-error">
                {error}
            </div>
        );
    }

    if (!formDef) {
        return (
            <div className="testrecordview-section testrecordview-loading">
                ...
            </div>
        );
    }

    const pagesToRender = formDef.Pages || formDef.pages || [];
    const formTitle = formDef.Title || formDef.title || '';

    const allFields = [];
    pagesToRender.forEach((page, pageIndex) => {
        (page.Columns || page.columns || []).forEach((column) =>
            (column.FormGroups || column.formGroups || []).forEach((group) =>
                (group.Fields || group.fields || []).forEach((field) => {
                    allFields.push({ field, pageIndex });
                })
            )
        );
    });

    const getFieldType = (f) => (f.Type || f.type || '').toLowerCase();
    const regularFields = allFields.filter(({ field }) => {
        const t = getFieldType(field);
        return t !== 'fieldgrid' && t !== 'upload' && t !== 'space' && t !== 'separator';
    });
    const gridFields = allFields.filter(({ field }) => getFieldType(field) === 'fieldgrid');
    const uploadFields = allFields.filter(({ field }) => getFieldType(field) === 'upload');

    return (
        <div className="testrecordview-section">
            <div className="testrecordview-section-block">
                <div className="testrecordview-fieldcontent">
                    {formTitle && (
                        <TestRecordViewFieldRenderer
                            key="testrecordview-testname"
                            field={{
                                Id: 'testname',
                                Label: TranslateTag('@GenTesC@', language),
                                type: 'text',
                                value: formTitle
                            }}
                            lists={lists}
                            pages={pagesToRender}
                            language={language}
                        />
                    )}
                    {regularFields.map(({ field }) => (
                        <TestRecordViewFieldRenderer
                            key={field.Id || field.id || field.Key || field.key}
                            field={field}
                            lists={lists}
                            pages={pagesToRender}
                            language={language}
                        />
                    ))}
                </div>
            </div>
            {gridFields.length > 0 && (
                <div className="testrecordview-section-block testrecordview-grids-block">
                    {gridFields.map(({ field }) => (
                        <TestRecordViewFieldRenderer
                            key={field.Id || field.id || field.Key || field.key}
                            field={field}
                            lists={lists}
                            pages={pagesToRender}
                            language={language}
                        />
                    ))}
                </div>
            )}
            {uploadFields.length > 0 && (
                <div className="testrecordview-section-block testrecordview-uploads-block">
                    <div className="testrecordview-fieldcontent">
                        {uploadFields.map(({ field }) => (
                            <TestRecordViewFieldRenderer
                                key={field.Id || field.id || field.Key || field.key}
                                field={field}
                                lists={lists}
                                pages={pagesToRender}
                                language={language}
                            />
                        ))}
                    </div>
                </div>
            )}
        </div>
    );
};

export default TestRecordViewSection;
