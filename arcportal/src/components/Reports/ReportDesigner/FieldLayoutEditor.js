import React, { useState, useEffect } from 'react';
import { 
    PrimaryButton, 
    DefaultButton, 
    IconButton,
    Checkbox,
    MessageBar,
    MessageBarType,
    Panel,
    PanelType
} from '@fluentui/react';
import './FieldLayoutEditor.css';
import { trimSectionFieldsToFormat } from './sectionValidation';

const FieldLayoutEditor = ({ 
    isOpen, 
    onDismiss, 
    fields, 
    format, 
    availableFields,
    onSaveFields,
    onAddField
}) => {
    const [localFields, setLocalFields] = useState([]);
    const [draggedField, setDraggedField] = useState(null);
    const [showAddFieldPanel, setShowAddFieldPanel] = useState(false);
    const [dragOverColumn, setDragOverColumn] = useState(null);
    const [pageWidth] = useState(580); // A4 page width in pixels

    useEffect(() => {
        if (isOpen) {
            setLocalFields(trimSectionFieldsToFormat(fields, format));
        }
    }, [isOpen, fields, format]);

    // Get available fields that aren't already added
    const getAvailableFields = () => {
        return availableFields.filter(af => 
            !localFields.some(f => f.Value === af.Value)
        );
    };

    // Calculate field position within its column (relative positioning)
    const getFieldPosition = (field, columnIndex) => {
        // Get all fields in the same column
        const columnFields = localFields.filter(f => f.Column === columnIndex + 1);
        
        // Sort by order within the column
        const sortedColumnFields = columnFields.sort((a, b) => (a.Order || 1) - (b.Order || 1));
        
        // Find the position of this field within the column
        const fieldIndex = sortedColumnFields.findIndex(f => f.Value === field.Value);
        
        // Start first field at 10px to account for delete button (which extends 8px above field)
        // Subsequent fields are spaced 60px apart
        const top = fieldIndex === 0 ? 10 : (fieldIndex * 72) + 10;
        
        return { top };
    };

    // Handle field drag start
    const handleFieldDragStart = (e, field) => {
        setDraggedField(field);
        e.dataTransfer.effectAllowed = 'move';
        e.dataTransfer.setData('text/plain', field.Value);
    };

    // Handle drag over
    const handleDragOver = (e) => {
        e.preventDefault();
        e.dataTransfer.dropEffect = 'move';
    };

    // Handle column drag over
    const handleColumnDragOver = (e, columnIndex) => {
        e.preventDefault();
        e.dataTransfer.dropEffect = 'move';
        setDragOverColumn(columnIndex);
    };

    // Handle column drag leave
    const handleColumnDragLeave = (e) => {
        e.preventDefault();
        setDragOverColumn(null);
    };

    // Handle field drop on another field
    const handleFieldDrop = (e, targetField) => {
        e.preventDefault();
        e.stopPropagation();
        
        if (!draggedField || draggedField.Value === targetField.Value) {
            setDraggedField(null);
            return;
        }

        setLocalFields(prev => {
            const newFields = [...prev];
            
            // Find indices
            const draggedIndex = newFields.findIndex(f => f.Value === draggedField.Value);
            const targetIndex = newFields.findIndex(f => f.Value === targetField.Value);
            
            if (draggedIndex === -1 || targetIndex === -1) return prev;

            // Remove the dragged field
            const [draggedItem] = newFields.splice(draggedIndex, 1);

            // Update the dragged field's position
            draggedItem.Column = targetField.Column;
            draggedItem.Order = targetField.Order;

            // Shift other fields in the target column
            newFields.forEach(field => {
                if (field.Column === targetField.Column && field.Order >= targetField.Order) {
                    field.Order = field.Order + 1;
                }
            });

            // Add the dragged field back
            newFields.push(draggedItem);

            // Sort and reorder
            const sortedFields = newFields.sort((a, b) => {
                if (a.Column !== b.Column) {
                    return a.Column - b.Column;
                }
                return a.Order - b.Order;
            });

            // Renumber orders within each column
            const finalFields = [];
            const columns = [...new Set(sortedFields.map(f => f.Column))].sort();
            
            columns.forEach(column => {
                const columnFields = sortedFields.filter(f => f.Column === column);
                columnFields.forEach((field, index) => {
                    finalFields.push({
                        ...field,
                        Order: index + 1
                    });
                });
            });

            return finalFields;
        });

        setDraggedField(null);
    };

    // Handle drop on column header (add to end of column)
    const handleColumnDrop = (e, columnIndex) => {
        e.preventDefault();
        e.stopPropagation();
        
        if (!draggedField) return;

        setLocalFields(prev => {
            const newFields = [...prev];
            const draggedIndex = newFields.findIndex(f => f.Value === draggedField.Value);
            
            if (draggedIndex === -1) return prev;

            // Remove the dragged field
            const [draggedItem] = newFields.splice(draggedIndex, 1);

            // Find the highest order in the target column
            const targetColumn = columnIndex + 1;
            const maxOrder = Math.max(0, ...newFields.filter(f => f.Column === targetColumn).map(f => f.Order));

            // Update the dragged field
            draggedItem.Column = targetColumn;
            draggedItem.Order = maxOrder + 1;

            // Add back and sort
            newFields.push(draggedItem);
            
            const sortedFields = newFields.sort((a, b) => {
                if (a.Column !== b.Column) {
                    return a.Column - b.Column;
                }
                return a.Order - b.Order;
            });

            // Renumber orders within each column
            const finalFields = [];
            const columns = [...new Set(sortedFields.map(f => f.Column))].sort();
            
            columns.forEach(column => {
                const columnFields = sortedFields.filter(f => f.Column === column);
                columnFields.forEach((field, index) => {
                    finalFields.push({
                        ...field,
                        Order: index + 1
                    });
                });
            });

            return finalFields;
        });

        setDraggedField(null);
    };

    // Handle no-box toggle for a field
    const handleNoBoxChange = (field, checked) => {
        setLocalFields(prev =>
            prev.map(f =>
                f.Value === field.Value ? { ...f, NoBox: checked } : f
            )
        );
    };

    // Handle field removal
    const handleRemoveField = (field) => {
        setLocalFields(prev => {
            const newFields = prev.filter(f => f.Value !== field.Value);
            // Reorder remaining fields
            return newFields.map((f, index) => ({
                ...f,
                Order: index + 1
            }));
        });
    };

    // Handle add field
    const handleAddField = (field) => {
        const maxOrder = Math.max(0, ...localFields.map(f => f.Order));
        const newField = {
            Label: field.Label,
            Value: field.Value,
            Column: 1,
            Order: maxOrder + 1,
            NoBox: false
        };
        setLocalFields(prev => [...prev, newField]);
        setShowAddFieldPanel(false);
    };

    // Handle save
    const handleSave = () => {
        onSaveFields(localFields);
        onDismiss();
    };

    // Render field item
    const renderFieldItem = (field, columnIndex) => {
        const position = getFieldPosition(field, columnIndex);
        const isDragging = draggedField && draggedField.Value === field.Value;
        
        return (
            <div
                key={field.Value}
                className={`field-item ${field.NoBox ? 'field-item-nobox' : ''} ${isDragging ? 'dragging' : ''}`}
                id={`fieldlayouteditor-column-${columnIndex}-field-${field.Value}`}
                style={{
                    top: `${position.top}px`
                }}
                draggable
                onDragStart={(e) => handleFieldDragStart(e, field)}
                onDragOver={(e) => {
                    e.preventDefault();
                    e.stopPropagation();
                }}
                onDrop={(e) => handleFieldDrop(e, field)}
                onDragEnd={() => {
                    setDraggedField(null);
                }}
            >
                <div className="field-content">
                    <div className="field-label">{field.Label}</div>
                    <div className="field-value">{field.Value}</div>
                    <Checkbox
                        id={`fieldlayouteditor-column-${columnIndex}-field-${field.Value}-nobox`}
                        label="No box"
                        checked={!!field.NoBox}
                        onChange={(e, checked) => handleNoBoxChange(field, !!checked)}
                        onClick={(e) => e.stopPropagation()}
                        styles={{ root: { marginTop: 2 }, text: { fontSize: 10 } }}
                    />
                </div>
                <IconButton
                    iconProps={{ iconName: 'Delete' }}
                    onClick={() => handleRemoveField(field)}
                    className="field-remove"
                />
            </div>
        );
    };

    // Render column
    const renderColumn = (column, index) => {
        const columnFields = localFields.filter(f => f.Column === index + 1);
        const isDragOver = dragOverColumn === index;
        
        // Calculate column position as percentage of page width
        const leftPercent = (column.Left / pageWidth) * 100;
        const widthPercent = (column.Width / pageWidth) * 100;
        
        return (
            <div
                key={index}
                className={`format-column ${isDragOver ? 'drag-over' : ''}`}
                id={`fieldlayouteditor-column-${index}`}
                style={{
                    left: `${leftPercent}%`,
                    width: `${widthPercent}%`
                }}
                onDragOver={(e) => handleColumnDragOver(e, index)}
                onDragLeave={handleColumnDragLeave}
                onDrop={(e) => {
                    handleColumnDrop(e, index);
                    setDragOverColumn(null);
                }}
            >
                <div className="column-header">
                    <h4>Column {index + 1}</h4>
                    <div className="column-info">
                        {column.Width}px × {column.LabelWidth}px label
                    </div>
                </div>
                <div 
                    className={`column-drop-zone ${isDragOver ? 'drag-over' : ''}`}
                    onDragOver={(e) => handleColumnDragOver(e, index)}
                >
                    {columnFields.map(field => renderFieldItem(field, index))}
                </div>
            </div>
        );
    };

    return (
        <Panel
            isOpen={isOpen}
            onDismiss={onDismiss}
            type={PanelType.large}
            headerText="Edit Field Layout"
            closeButtonAriaLabel="Close"
        >
            <div className="field-layout-editor" id="fieldlayouteditor">
                <div className="layout-header">
                    <h3>Field Layout: {format?.Name}</h3>
                    <div className="layout-actions">
                        <PrimaryButton 
                            id="fieldlayouteditor-addfield"
                            text="Add Field" 
                            onClick={() => setShowAddFieldPanel(true)}
                            iconProps={{ iconName: 'Add' }}
                            disabled={!format?.Columns || format.Columns.length === 0}
                        />
                    </div>
                </div>

                <div className="layout-preview">
                    <div className="format-preview" style={{ width: `${pageWidth}px` }}>
                        {(!format?.Columns || format.Columns.length === 0) && (
                            <p id="fieldlayouteditor-columns-empty">
                                This format has no field columns. Add columns in the format editor or use a grid-only format.
                            </p>
                        )}
                        {format?.Columns?.map(renderColumn)}
                    </div>
                </div>

                <div className="layout-footer">
                    <DefaultButton id="fieldlayouteditor-cancel" text="Cancel" onClick={onDismiss} />
                    <PrimaryButton id="fieldlayouteditor-save" text="Save Layout" onClick={handleSave} />
                </div>

                {/* Add Field Panel */}
                <Panel
                    isOpen={showAddFieldPanel}
                    onDismiss={() => setShowAddFieldPanel(false)}
                    type={PanelType.small}
                    headerText="Add Field"
                >
                    <div className="add-field-panel">
                        <div className="available-fields">
                            {getAvailableFields().map(field => (
                                <div 
                                    key={field.Value}
                                    className="available-field-item"
                                    onClick={() => handleAddField(field)}
                                >
                                    <div className="field-label">{field.Label}</div>
                                    <div className="field-value">{field.Value}</div>
                                </div>
                            ))}
                        </div>
                        {getAvailableFields().length === 0 && (
                            <MessageBar messageBarType={MessageBarType.info}>
                                All available fields have been added to this section.
                            </MessageBar>
                        )}
                    </div>
                </Panel>
            </div>
        </Panel>
    );
};

export default FieldLayoutEditor;
