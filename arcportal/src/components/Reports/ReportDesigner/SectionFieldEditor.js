import React, { useState, useEffect, useRef, useMemo } from 'react';
import { 
    PrimaryButton, 
    DefaultButton, 
    Panel,
    PanelType,
    TextField,
    Dropdown,
    IconButton,
    Toggle
} from '@fluentui/react';
import FieldLayoutEditor from './FieldLayoutEditor';
import FormatEditor from './FormatEditor';
import GridLayoutEditor from './GridLayoutEditor';
import LayoutSectionPreviewCanvas from './LayoutSectionPreviewCanvas';
import SectionAreaLayoutEditor from './SectionAreaLayoutEditor';
import { resolveEditorLayoutRows } from './sectionAreaLayoutUtils';
import { translateLayoutSectionToContents } from '../Functions/layoutSectionTranslator';
import { findFormatByName } from './reportChangeSet';
import { validateSection, trimSectionGridsToFormat, trimSectionFieldsToFormat, trimSectionGridHeadsToFormat, reconcileSectionFieldsAfterFormatChange, hasSectionHeadingText } from './sectionValidation';
import { ReportPageDimensions } from '../ReportPageDimensions';
import './SectionFieldEditor.css';
import './AbsoluteSectionDesigner.css';

const { PAGE_WIDTH, MARGIN_LEFT, MARGIN_RIGHT } = ReportPageDimensions;

const SectionFieldEditor = ({ 
    isOpen, 
    onDismiss, 
    sectionDefinition, 
    dataSection, 
    availableFormats, 
    onSave, 
    onDelete,
    onUpdateFormats,
    language = [],
}) => {
    const [sectionName, setSectionName] = useState(sectionDefinition?.Name || '');
    const [description, setDescription] = useState(sectionDefinition?.Description || '');
    const [headingText, setHeadingText] = useState(sectionDefinition?.HeadingText || '');
    const [hasSectionHeading, setHasSectionHeading] = useState(
        hasSectionHeadingText(sectionDefinition?.HeadingText)
    );
    const [format, setFormat] = useState(sectionDefinition?.Format || 'SingleColumn');
    const [fields, setFields] = useState(sectionDefinition?.Fields || []);
    const [grids, setGrids] = useState(sectionDefinition?.Grids || []);
    const [layoutRows, setLayoutRows] = useState(sectionDefinition?.LayoutRows || []);
    const [dynamic, setDynamic] = useState(sectionDefinition?.Dynamic || false);
    const [showFieldLayoutEditor, setShowFieldLayoutEditor] = useState(false);
    const [showFormatEditor, setShowFormatEditor] = useState(false);
    const [editingFormat, setEditingFormat] = useState(null);
    const [showGridEditor, setShowGridEditor] = useState(false);
    const [previewFormatSnapshot, setPreviewFormatSnapshot] = useState(null);

    const availableFormatsRef = useRef(availableFormats);
    availableFormatsRef.current = availableFormats;

    const resolveFormat = (name) => findFormatByName(availableFormatsRef.current, name);

    useEffect(() => {
        if (sectionDefinition) {
            setSectionName(sectionDefinition.Name || '');
            setDescription(sectionDefinition.Description || '');
            setHeadingText(sectionDefinition.HeadingText || '');
            setHasSectionHeading(hasSectionHeadingText(sectionDefinition.HeadingText));
            const sectionFormat = sectionDefinition.Format || 'SingleColumn';
            const resolved = resolveFormat(sectionFormat);
            setFormat(resolved ? resolved.Name : '');
            setFields(sectionDefinition.Fields || []);
            setGrids(sectionDefinition.Grids || []);
            setLayoutRows(sectionDefinition.LayoutRows || []);
            setDynamic(sectionDefinition.Dynamic || false);
        } else {
            const resolved = resolveFormat('SingleColumn');
            setFormat(resolved ? resolved.Name : '');
        }
    }, [sectionDefinition, isOpen]);

    useEffect(() => {
        if (!isOpen) {
            setPreviewFormatSnapshot(null);
        }
    }, [isOpen]);

    useEffect(() => {
        if (!previewFormatSnapshot) {
            return;
        }

        const resolved = findFormatByName(availableFormats, previewFormatSnapshot.Name);
        if (!resolved) {
            return;
        }

        const snapshotHeading = JSON.stringify(previewFormatSnapshot.Heading ?? []);
        const resolvedHeading = JSON.stringify(resolved.Heading ?? []);
        if (snapshotHeading === resolvedHeading) {
            setPreviewFormatSnapshot(null);
        }
    }, [availableFormats, previewFormatSnapshot]);

    const handleSaveFields = (updatedFields) => {
        setFields(updatedFields);
    };

    /**
     * Applies grid layout edits from the grid layout editor to section state.
     * @param {Array} updatedGrids - Bindings with Name and Head per format position.
     */
    const handleSaveGrids = (updatedGrids) => {
        setGrids(updatedGrids);
    };

    const getCurrentFormat = () =>
        previewFormatSnapshot || findFormatByName(availableFormats, format);

    const handleEditFormat = (formatToEdit) => {
        setEditingFormat(formatToEdit);
        setShowFormatEditor(true);
    };

    const handleCreateCustomFormat = () => {
        setEditingFormat(null);
        setShowFormatEditor(true);
    };

    /**
     * Applies format geometry changes to section field and grid bindings.
     * @param {object} updatedFormat - The format document from the format editor.
     * @param {object} [previousFormat] - The format before the edit, when known.
     */
    const applyFormatGeometryToSection = (updatedFormat, previousFormat = null) => {
        setGrids(currentGrids => trimSectionGridHeadsToFormat(
            trimSectionGridsToFormat(currentGrids, updatedFormat),
            updatedFormat
        ));
        setFields(currentFields => reconcileSectionFieldsAfterFormatChange(
            currentFields,
            previousFormat,
            updatedFormat
        ));
    };

    /**
     * Saves an edited format and trims section bindings to the new format capacity.
     * @param {object} updatedFormat - The format document from the format editor.
     */
    const handleSaveFormat = (updatedFormat) => {
        const previousFormat = editingFormat;
        setShowFormatEditor(false);
        setEditingFormat(null);
        setPreviewFormatSnapshot(updatedFormat);

        if (onUpdateFormats) {
            onUpdateFormats(updatedFormat, false);
        }

        applyFormatGeometryToSection(updatedFormat, previousFormat);
    };

    const handleSaveCustomFormat = (newFormat) => {
        setShowFormatEditor(false);
        setEditingFormat(null);
        setPreviewFormatSnapshot(newFormat);

        if (onUpdateFormats) {
            onUpdateFormats(newFormat, true);
        }

        const selectedFormat = findFormatByName(availableFormatsRef.current, format);
        if (selectedFormat && selectedFormat.Name === newFormat.Name) {
            applyFormatGeometryToSection(newFormat, selectedFormat);
        }
    };

    /**
     * Persists section edits. HeadingText is empty when the include-heading toggle is off;
     * otherwise it is the trimmed text field value.
     */
    const handleSave = () => {
        if (!format || format.trim() === '') {
            alert('Please select a format for this section before saving.');
            return;
        }

        const selectedFormat = findFormatByName(availableFormats, format);
        if (!selectedFormat) {
            alert('The selected format is no longer available. Please select a different format.');
            return;
        }

        const persistedHeadingText = hasSectionHeading ? (headingText || '').trim() : '';

        const updatedSection = {
            ...(sectionDefinition || {}),
            Name: sectionName,
            Description: description,
            HeadingText: persistedHeadingText,
            Format: selectedFormat.Name,
            DataSection: dataSection?.Name,
            Fields: fields,
            Grids: grids,
            LayoutRows: layoutRows,
            Dynamic: dynamic
        };
        onSave(updatedSection);
        onDismiss();
    };

    const currentFormat = getCurrentFormat();
    const formatSupportsGrids = currentFormat?.Grids && currentFormat.Grids.length > 0;

    const sectionIssues = validateSection({
        section: { ...(sectionDefinition || {}), Fields: fields, Grids: grids, Format: format },
        format: currentFormat,
        dataSection
    });

    const previewSection = useMemo(() => ({
        ...(sectionDefinition || {}),
        Name: sectionName,
        Description: description,
        HeadingText: hasSectionHeading ? (headingText || '').trim() : '',
        Format: format,
        DataSection: dataSection?.Name,
        Fields: fields,
        Grids: grids,
        LayoutRows: layoutRows,
        Dynamic: dynamic,
    }), [
        sectionDefinition,
        sectionName,
        description,
        hasSectionHeading,
        headingText,
        format,
        dataSection,
        fields,
        grids,
        layoutRows,
        dynamic,
    ]);

    // Translated once here and handed to the area layout control so its schematic shows the same computed
    // geometry the preview canvas and the printed report use.
    const previewContents = useMemo(() => {
        if (!currentFormat?.Type) {
            return [];
        }

        return translateLayoutSectionToContents({
            section: previewSection,
            format: currentFormat,
            dataSection,
        }).contents;
    }, [previewSection, currentFormat, dataSection]);

    const resolvedLayoutRows = useMemo(
        () => resolveEditorLayoutRows(layoutRows, grids, currentFormat, fields),
        [layoutRows, grids, currentFormat, fields]
    );

    const previewFieldCount = fields.length;
    const previewGridCount = grids.filter((grid) => grid?.Name).length;

    return (
        <Panel
            isOpen={isOpen}
            onDismiss={onDismiss}
            type={PanelType.extraLarge}
            headerText={`Edit Section: ${sectionName || 'New Section'}`}
            closeButtonAriaLabel="Close"
        >
            <div className="section-field-editor" id="sectionfieldeditor">
                <div className="designer-split-container">
                    <div className="designer-left-panel">
                        <div className="preview-section-sticky">
                            <h3>Preview</h3>
                            <div className="preview-info">
                                <span id="sectionfieldeditor-preview-info">
                                    {previewFieldCount} field{previewFieldCount === 1 ? '' : 's'}
                                    {previewGridCount > 0
                                        ? `, ${previewGridCount} grid${previewGridCount === 1 ? '' : 's'}`
                                        : ''}
                                </span>
                            </div>
                            <div
                                id="sectionfieldeditor-preview-container"
                                className="preview-container"
                                style={{
                                    width: `${PAGE_WIDTH}px`,
                                    maxHeight: '800px',
                                    overflow: 'auto',
                                }}
                            >
                                <div
                                    id="sectionfieldeditor-margin-left"
                                    className="preview-margin-guide preview-margin-guide-left"
                                    style={{ left: `${MARGIN_LEFT}px` }}
                                />
                                <div
                                    id="sectionfieldeditor-margin-right"
                                    className="preview-margin-guide preview-margin-guide-right"
                                    style={{ left: `${PAGE_WIDTH - MARGIN_RIGHT}px` }}
                                />
                                <LayoutSectionPreviewCanvas
                                    section={previewSection}
                                    format={currentFormat}
                                    dataSection={dataSection}
                                    language={language}
                                />
                            </div>

                            <SectionAreaLayoutEditor
                                rows={resolvedLayoutRows}
                                contents={previewContents}
                                format={currentFormat}
                                grids={grids}
                                dataSectionGrids={dataSection?.Grids || []}
                                language={language}
                                onChange={setLayoutRows}
                            />
                        </div>
                    </div>

                    <div className="designer-right-panel">
                        <div className="editor-header">
                            <div className="form-row">
                                <TextField
                                    id="sectionfieldeditor-name"
                                    label="Section Name"
                                    value={sectionName}
                                    onChange={(e, value) => setSectionName(value)}
                                    placeholder="Enter section name"
                                    className="form-field"
                                    disabled={!!sectionDefinition}
                                />
                            </div>
                            <div className="form-row">
                                <TextField
                                    id="sectionfieldeditor-description"
                                    label="Description"
                                    value={description}
                                    onChange={(e, value) => setDescription(value)}
                                    placeholder="Enter section description"
                                    className="form-field"
                                />
                            </div>
                            <div className="form-row">
                                <Toggle
                                    id="sectionfieldeditor-hasheading"
                                    label="Include section heading"
                                    checked={hasSectionHeading}
                                    onChange={(e, checked) => {
                                        setHasSectionHeading(checked);
                                        if (!checked) {
                                            setHeadingText('');
                                        }
                                    }}
                                    onText="Yes"
                                    offText="No"
                                    className="form-field"
                                />
                                <TextField
                                    id="sectionfieldeditor-headingtext"
                                    label="Heading Text"
                                    value={headingText}
                                    onChange={(e, value) => {
                                        setHeadingText(value);
                                    }}
                                    placeholder="Enter heading text"
                                    className="form-field"
                                    disabled={!hasSectionHeading}
                                />
                                <div className="format-section">
                                    <Dropdown
                                        id="sectionfieldeditor-format"
                                        label="Format"
                                        options={availableFormats
                                            .map(f => ({ key: f.Name, text: f.Description || f.Name }))
                                            .sort((a, b) => a.text.localeCompare(b.text))}
                                        selectedKey={format}
                                        onChange={(e, option) => {
                                            const selectedKey = option ? option.key : '';
                                            setFormat(selectedKey);
                                            const resolvedFormat = findFormatByName(availableFormats, selectedKey);
                                            if (resolvedFormat) {
                                                setGrids(currentGrids => trimSectionGridHeadsToFormat(
                                                    trimSectionGridsToFormat(currentGrids, resolvedFormat),
                                                    resolvedFormat
                                                ));
                                                setFields(currentFields => trimSectionFieldsToFormat(currentFields, resolvedFormat));
                                            }
                                        }}
                                        className="form-field"
                                        required
                                    />
                                    <div className="format-actions">
                                        <IconButton
                                            id="sectionfieldeditor-format-edit"
                                            iconProps={{ iconName: 'Edit' }}
                                            onClick={() => {
                                                const resolvedFormat = findFormatByName(availableFormats, format);
                                                if (resolvedFormat) {
                                                    handleEditFormat(resolvedFormat);
                                                }
                                            }}
                                            title="Edit format"
                                        />
                                        <IconButton
                                            id="sectionfieldeditor-format-add"
                                            iconProps={{ iconName: 'Add' }}
                                            onClick={handleCreateCustomFormat}
                                            title="Create custom format"
                                        />
                                    </div>
                                </div>
                            </div>
                            <div className="form-row">
                                <Toggle
                                    id="sectionfieldeditor-dynamic"
                                    label="Dynamic Section"
                                    checked={dynamic}
                                    onChange={(e, checked) => setDynamic(checked)}
                                    onText="Yes"
                                    offText="No"
                                    className="form-field"
                                />
                            </div>
                        </div>

                        <div className="editor-content">
                            <div className="data-section-info">
                                <h3>Data Section Information</h3>
                                <p id="sectionfieldeditor-datasection-name"><strong>Data Section:</strong> {dataSection?.Name || 'None'}</p>
                                <p><strong>Fields Available:</strong> {dataSection?.Fields?.length || 0}</p>
                                <p><strong>Grid Available:</strong> {dataSection?.Grids?.length || 0}</p>

                                {sectionIssues.length > 0 && (
                                    <div className="section-issues" id="sectionfieldeditor-issues">
                                        {sectionIssues.map((issue) => (
                                            <span
                                                key={issue.type}
                                                id={`sectionfieldeditor-issue-${issue.type}`}
                                                className="section-issue-text"
                                            >
                                                ⚠ {issue.message}
                                            </span>
                                        ))}
                                    </div>
                                )}

                                <div className="action-buttons">
                                    {dataSection?.Fields && dataSection.Fields.length > 0 && (
                                        <PrimaryButton 
                                            id="sectionfieldeditor-fieldlayout"
                                            text="Edit Field Layout" 
                                            onClick={() => setShowFieldLayoutEditor(true)}
                                            iconProps={{ iconName: 'Edit' }}
                                            className="edit-layout-button"
                                        />
                                    )}

                                    {dataSection?.Grids && dataSection.Grids.length > 0 && formatSupportsGrids && (
                                        <PrimaryButton 
                                            id="sectionfieldeditor-gridlayout"
                                            text="Edit Grid Layout" 
                                            onClick={() => setShowGridEditor(true)}
                                            iconProps={{ iconName: 'Table' }}
                                            className="edit-layout-button"
                                        />
                                    )}
                                </div>
                            </div>
                        </div>

                        <div className="editor-footer">
                            <div className="footer-left">
                                {sectionDefinition && (
                                    <DefaultButton 
                                        id="sectionfieldeditor-delete"
                                        text="Delete Section" 
                                        onClick={() => {
                                            onDelete(sectionDefinition);
                                            onDismiss();
                                        }}
                                        iconProps={{ iconName: 'Delete' }}
                                        styles={{ root: { color: '#d13438' } }}
                                    />
                                )}
                            </div>
                            <div className="footer-right">
                                <DefaultButton id="sectionfieldeditor-cancel" text="Cancel" onClick={onDismiss} />
                                <PrimaryButton 
                                    id="sectionfieldeditor-save"
                                    text="Save Section" 
                                    onClick={handleSave}
                                    disabled={!sectionName || !format || format.trim() === ''}
                                />
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <FieldLayoutEditor
                isOpen={showFieldLayoutEditor}
                onDismiss={() => setShowFieldLayoutEditor(false)}
                fields={fields}
                format={currentFormat}
                availableFields={dataSection?.Fields || []}
                onSaveFields={handleSaveFields}
            />

            <FormatEditor
                isOpen={showFormatEditor}
                onDismiss={() => {
                    setShowFormatEditor(false);
                    setEditingFormat(null);
                }}
                availableFormats={availableFormats}
                currentFormat={editingFormat}
                onSaveFormat={handleSaveFormat}
                onSaveCustomFormat={handleSaveCustomFormat}
            />

            <GridLayoutEditor
                isOpen={showGridEditor}
                onDismiss={() => setShowGridEditor(false)}
                grids={grids}
                format={currentFormat}
                dataSection={dataSection}
                dataSectionGrids={dataSection?.Grids || []}
                language={language}
                onSave={handleSaveGrids}
            />
        </Panel>
    );
};

export default SectionFieldEditor;
