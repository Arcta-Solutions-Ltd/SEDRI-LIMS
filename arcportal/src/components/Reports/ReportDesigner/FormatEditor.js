import React, { useState, useEffect } from 'react';
import { 
    PrimaryButton, 
    DefaultButton, 
    Panel,
    PanelType,
    TextField,
    IconButton,
    Toggle,
    MessageBar,
    MessageBarType
} from '@fluentui/react';
import GridDefinitionEditor from './GridDefinitionEditor';
import ColumnDefinitionEditor from './ColumnDefinitionEditor';
import {
    clampReportLineFontSize,
    REPORT_LINE_FONT_SIZE_MAX,
    REPORT_LINE_FONT_SIZE_MIN
} from '../ReportFontSizeOptions';
import './FormatEditor.css';

const FormatEditor = ({ 
    isOpen, 
    onDismiss, 
    availableFormats,
    currentFormat,
    onSaveFormat,
    onSaveCustomFormat
}) => {
    const [formatName, setFormatName] = useState('');
    const [formatNameError, setFormatNameError] = useState('');
    const [isCustomFormat, setIsCustomFormat] = useState(false);
    // The name field edits the format's Description, which for a built-in format may be a @Tag@
    // language token that the API has already translated for display. Only overwrite the stored
    // description when the user actually retyped it.
    const [descriptionEdited, setDescriptionEdited] = useState(false);
    const [pageWidth] = useState(580); // A4 page width in pixels
    
    // Heading state
    const [heading, setHeading] = useState({ Left: 20, FontSize: 12, Bold: true });
    const [hasHeading, setHasHeading] = useState(false);
    
    // Columns state
    const [columns, setColumns] = useState([]);
    
    // Grids state
    const [grids, setGrids] = useState([]);

    // Initialize format when panel opens
    useEffect(() => {
        if (isOpen && currentFormat) {
            // For existing formats, show Description in the input field
            setFormatName(currentFormat.Description || currentFormat.Name || '');
            setFormatNameError('');
            
            // Initialize heading geometry only; text lives on the section HeadingText field.
            if (currentFormat.Heading && currentFormat.Heading.length > 0) {
                const formatHeading = currentFormat.Heading[0];
                setHeading({
                    Left: formatHeading.Left ?? 20,
                    FontSize: formatHeading.FontSize ?? 12,
                    Bold: formatHeading.Bold ?? true
                });
                setHasHeading(true);
            } else {
                setHeading({ Left: 20, FontSize: 12, Bold: true });
                setHasHeading(false);
            }
            
            // Initialize columns
            setColumns([...(currentFormat.Columns || [])]);
            
            // Initialize grids
            setGrids([...(currentFormat.Grids || [])]);
            
            setIsCustomFormat(false);
            setDescriptionEdited(false);
        } else if (isOpen) {
            // New format
            setFormatName('');
            setFormatNameError('');
            setHeading({ Left: 20, FontSize: 12, Bold: true });
            setHasHeading(false);
            setColumns([]);
            setGrids([]);
            setIsCustomFormat(true);
            setDescriptionEdited(true);
        }
    }, [isOpen, currentFormat]);

    // Field change handlers with error clearing
    const handleFormatNameChange = (e, value) => {
        setFormatName(value);
        setDescriptionEdited(true);
        if (formatNameError) {
            setFormatNameError('');
        }
    };


    // Heading handlers
    const handleHeadingChange = (field, value) => {
        if (field === 'Bold') {
            setHeading({ ...heading, Bold: value });
            return;
        }

        const parsed = parseInt(value, 10) || 0;
        setHeading({ ...heading, [field]: clampReportLineFontSize(parsed) });
    };

    // Column handlers
    /**
     * Appends a new field-layout column to the format geometry.
     */
    const handleAddColumn = () => {
        const lastColumn = columns[columns.length - 1];
        const newLeft = lastColumn ? lastColumn.Left + lastColumn.Width + 20 : 20;
        const newColumn = {
            Left: newLeft,
            Width: 150,
            LabelWidth: 60
        };
        setColumns([...columns, newColumn]);
    };

    /**
     * Removes a field-layout column at the given index, including the last column (grid-only formats).
     * @param {number} index - Zero-based column index to remove.
     */
    const handleRemoveColumn = (index) => {
        const newColumns = columns.filter((_, i) => i !== index);
        setColumns(newColumns);
    };

    const handleColumnChange = (index, field, value) => {
        const newColumns = [...columns];
        newColumns[index] = { ...newColumns[index], [field]: parseInt(value) || 0 };
        setColumns(newColumns);
    };

    // Grid handlers
    /**
     * Appends a new grid position to the format geometry.
     */
    const handleAddGrid = () => {
        const newGrid = {
            Left: 20,
            Width: '150|150|80'
        };
        setGrids([...grids, newGrid]);
    };

    /**
     * Removes a grid position at the given index.
     * @param {number} index - Zero-based grid position index to remove.
     */
    const handleRemoveGrid = (index) => {
        const newGrids = grids.filter((_, i) => i !== index);
        setGrids(newGrids);
    };

    const handleGridChange = (index, field, value) => {
        const newGrids = [...grids];
        newGrids[index] = { ...newGrids[index], [field]: value };
        setGrids(newGrids);
    };

    const handleSave = () => {
        // Clear previous errors
        setFormatNameError('');
        
        let hasErrors = false;
        
        // Validate format name
        if (!formatName.trim()) {
            setFormatNameError('Format name is required');
            hasErrors = true;
        }
        
        if (hasErrors) {
            return;
        }

        // Generate base name by removing spaces and converting to lowercase (for new formats only)
        // For existing formats, preserve the original Name
        const baseName = isCustomFormat 
            ? formatName.replace(/\s+/g, '').toLowerCase()
            : (currentFormat?.Name || formatName.replace(/\s+/g, '').toLowerCase());

        const format = {
            // Carry the configs identity through so the backend updates the record it came from
            // rather than matching it by name.
            ConfigId: isCustomFormat ? null : (currentFormat?.ConfigId ?? null),
            Name: baseName,
            Type: currentFormat?.Type || 'DoubleFieldColumn',
            Description: descriptionEdited ? formatName : (currentFormat?.Description ?? formatName),
            Heading: hasHeading
                ? [{ Line: 1, Left: heading.Left, FontSize: heading.FontSize, Bold: heading.Bold }]
                : [],
            Columns: columns,
            Grids: grids,
            Images: currentFormat?.Images || []
        };

        if (isCustomFormat) {
            onSaveCustomFormat(format);
        } else {
            onSaveFormat(format);
        }
        onDismiss();
    };






    return (
        <Panel
            isOpen={isOpen}
            onDismiss={onDismiss}
            type={PanelType.large}
            headerText={isCustomFormat ? "Create Custom Format" : "Edit Format"}
            closeButtonAriaLabel="Close"
        >
            <div className="format-editor" id="formateditor">
                <div className="format-basic-info">
                    <TextField
                        id="formateditor-name"
                        label="Format Name *"
                        value={formatName}
                        onChange={handleFormatNameChange}
                        placeholder="Enter format name"
                        errorMessage={formatNameError}
                    />
                </div>

                {/* Heading layout — geometry only; heading text is set on the section. */}
                <div className="heading-section">
                    <div className="section-header">
                        <h3>Heading layout</h3>
                        <Toggle
                            id="formateditor-hasheading"
                            label="Include heading slot"
                            checked={hasHeading}
                            onChange={(e, checked) => setHasHeading(checked)}
                            onText="Yes"
                            offText="No"
                        />
                    </div>

                    <MessageBar
                        id="formateditor-heading-hint"
                        messageBarType={MessageBarType.info}
                        isMultiline={true}
                    >
                        Heading text is configured in the section editor. This panel controls whether
                        the format defines heading position and style on the printed report.
                    </MessageBar>
                    
                    {hasHeading && (
                        <div className="heading-config">
                            <TextField
                                id="formateditor-heading-left"
                                label="Left Position"
                                type="number"
                                value={heading.Left.toString()}
                                onChange={(e, value) => handleHeadingChange('Left', value)}
                                suffix="px"
                            />
                            <TextField
                                id="formateditor-heading-fontsize"
                                label="Font Size"
                                type="number"
                                min={REPORT_LINE_FONT_SIZE_MIN}
                                max={REPORT_LINE_FONT_SIZE_MAX}
                                value={heading.FontSize.toString()}
                                onChange={(e, value) => handleHeadingChange('FontSize', value)}
                                suffix="pt"
                            />
                            <Toggle
                                id="formateditor-heading-bold"
                                label="Bold"
                                checked={heading.Bold}
                                onChange={(e, checked) => handleHeadingChange('Bold', checked)}
                                onText="Yes"
                                offText="No"
                            />
                        </div>
                    )}
                </div>

                {/* Columns Section */}
                <ColumnDefinitionEditor
                    columns={columns}
                    onColumnsChange={setColumns}
                    onAddColumn={handleAddColumn}
                    onRemoveColumn={handleRemoveColumn}
                    onColumnChange={handleColumnChange}
                />


                {/* Grids Section */}
                <GridDefinitionEditor
                    grids={grids}
                    onGridsChange={setGrids}
                    onAddGrid={handleAddGrid}
                    onRemoveGrid={handleRemoveGrid}
                    onGridChange={handleGridChange}
                />

                <div className="format-editor-footer">
                    <DefaultButton id="formateditor-cancel" text="Cancel" onClick={onDismiss} />
                    <PrimaryButton 
                        id="formateditor-save"
                        text={isCustomFormat ? "Create Format" : "Update Format"} 
                        onClick={handleSave}
                    />
                </div>
            </div>
        </Panel>
    );
};

export default FormatEditor;
