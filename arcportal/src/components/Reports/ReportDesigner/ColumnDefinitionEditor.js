import React from 'react';
import { PrimaryButton, IconButton } from '@fluentui/react';
import './ColumnDefinitionEditor.css';

const ColumnDefinitionEditor = ({ 
    columns, 
    onColumnsChange, 
    onAddColumn, 
    onRemoveColumn, 
    onColumnChange 
}) => {
    const handleColumnWidthChange = (columnIndex, field, value) => {
        const newColumns = [...columns];
        const newValue = parseInt(value) || 0;
        
        // Apply basic constraints only
        if (field === 'LabelWidth') {
            // Label width can't exceed column width
            newColumns[columnIndex].LabelWidth = Math.min(newValue, newColumns[columnIndex].Width);
        } else {
            // For Left and Width, just set the value directly
            newColumns[columnIndex][field] = newValue;
        }
        
        onColumnsChange(newColumns);
    };

    const renderColumnEditor = (column, index) => {
        return (
            <div key={index} className="column-editor" id={`formateditor-column-${index}`}>
                <div className="column-header">
                    <h4>Column {index + 1}</h4>
                    <IconButton
                        id={`formateditor-removecolumn-${index}`}
                        iconProps={{ iconName: 'Delete' }}
                        onClick={() => onRemoveColumn(index)}
                        title="Remove column"
                    />
                </div>
                
                {/* Interactive Controls */}
                <div className="column-controls">
                    <div className="control-group">
                        <div className="control-label">Left Position</div>
                        <div className="slider-container">
                            <input
                                id={`formateditor-column-${index}-left`}
                                type="range"
                                min="0"
                                max="400"
                                value={column.Left}
                                onChange={(e) => handleColumnWidthChange(index, 'Left', e.target.value)}
                                className="column-slider"
                            />
                            <div className="slider-value">{column.Left}px</div>
                        </div>
                    </div>
                    
                    <div className="control-group">
                        <div className="control-label">Total Width</div>
                        <div className="slider-container">
                            <input
                                id={`formateditor-column-${index}-width`}
                                type="range"
                                min="100"
                                max="500"
                                value={column.Width}
                                onChange={(e) => handleColumnWidthChange(index, 'Width', e.target.value)}
                                className="column-slider"
                            />
                            <div className="slider-value">{column.Width}px</div>
                        </div>
                    </div>
                    
                    <div className="control-group">
                        <div className="control-label">Label Width</div>
                        <div className="slider-container">
                            <input
                                id={`formateditor-column-${index}-labelwidth`}
                                type="range"
                                min="20"
                                max={column.Width - 20}
                                value={column.LabelWidth}
                                onChange={(e) => handleColumnWidthChange(index, 'LabelWidth', e.target.value)}
                                className="column-slider label-slider"
                            />
                            <div className="slider-value">{column.LabelWidth}px</div>
                        </div>
                    </div>
                </div>
            </div>
        );
    };

    return (
        <div className="columns-section">
            <div className="section-header">
                <h3>Columns (Total: {columns.reduce((sum, col) => sum + col.Left + col.Width, 0)}px / 580px)</h3>
                <PrimaryButton 
                    id="formateditor-addcolumn"
                    text="Add Column" 
                    onClick={onAddColumn}
                    iconProps={{ iconName: 'Add' }}
                />
            </div>
            
            {/* Single Page Layout Visualization */}
            <div className="page-layout-visualization">
                <div className="page-layout-container">
                    {columns.map((column, colIndex) => {
                        const valueWidth = column.Width - column.LabelWidth;
                        return (
                            <div 
                                key={colIndex} 
                                className="page-column-item"
                                style={{ 
                                    left: `${(column.Left / 580) * 100}%`,
                                    width: `${(column.Width / 580) * 100}%`,
                                    backgroundColor: `hsl(${colIndex * 60}, 70%, 85%)`
                                }}
                            >
                                <div className="field-container">
                                    <div className="field-label-section" style={{ width: `${column.LabelWidth}px` }}>
                                        <div className="field-label-text">Label</div>
                                        <div className="field-label-width">{column.LabelWidth}px</div>
                                    </div>
                                    <div className="field-value-section" style={{ width: `${valueWidth}px` }}>
                                        <div className="field-value-text">Value</div>
                                        <div className="field-value-width">{valueWidth}px</div>
                                    </div>
                                </div>
                                <div className="column-total-width">
                                    Col {colIndex + 1}: {column.Width}px
                                </div>
                            </div>
                        );
                    })}
                    {/* Page width indicator */}
                    <div className="page-width-indicator" style={{ 
                        left: `${(columns.reduce((sum, col) => sum + col.Left + col.Width, 0) / 580) * 100}%`,
                        width: `${((580 - columns.reduce((sum, col) => sum + col.Left + col.Width, 0)) / 580) * 100}%`
                    }}>
                        <div className="page-width-label">Available: {580 - columns.reduce((sum, col) => sum + col.Left + col.Width, 0)}px</div>
                    </div>
                </div>
            </div>
            
            <div className="columns-list">
                {columns.length === 0 && (
                    <p id="formateditor-columns-empty" className="columns-empty-message">
                        No field columns defined. Add a column for scalar fields, or use grids only.
                    </p>
                )}
                {columns.map(renderColumnEditor)}
            </div>
        </div>
    );
};

export default ColumnDefinitionEditor;
