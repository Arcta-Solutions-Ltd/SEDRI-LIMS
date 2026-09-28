import React from 'react';
import { IconButton, PrimaryButton } from '@fluentui/react';
import { TextField } from '@fluentui/react';
import './GridDefinitionEditor.css';

const GridDefinitionEditor = ({ 
    grids, 
    onGridsChange, 
    onAddGrid, 
    onRemoveGrid, 
    onGridChange 
}) => {
    const handleGridColumnWidthChange = (gridIndex, columnIndex, value) => {
        const newGrids = [...grids];
        const currentWidths = newGrids[gridIndex].Width.split('|').map(w => parseInt(w) || 100);
        const newValue = parseInt(value) || 100;
        
        // Just update the target column during drag
        currentWidths[columnIndex] = newValue;
        
        newGrids[gridIndex] = { ...newGrids[gridIndex], Width: currentWidths.join('|') };
        onGridsChange(newGrids);
    };

    const handleGridColumnWidthChangeEnd = (gridIndex, columnIndex, value) => {
        const newGrids = [...grids];
        const currentWidths = newGrids[gridIndex].Width.split('|').map(w => parseInt(w) || 100);
        const pageWidth = 580; // Match the preview container width
        
        // Calculate the new total if this column changes
        const newValue = parseInt(value) || 100;
        const otherColumnsTotal = currentWidths.reduce((sum, width, index) => 
            index === columnIndex ? sum : sum + width, 0);
        const newTotal = otherColumnsTotal + newValue;
        
        // If new total exceeds page width, scale down OTHER columns proportionally
        if (newTotal > pageWidth) {
            const availableSpace = pageWidth - newValue; // Space available for other columns
            const scaleFactor = availableSpace / otherColumnsTotal;
            
            // Update the target column to its new value
            currentWidths[columnIndex] = newValue;
            
            // Scale down OTHER columns proportionally
            currentWidths.forEach((width, index) => {
                if (index !== columnIndex) {
                    currentWidths[index] = Math.round(width * scaleFactor);
                }
            });
        } else {
            // Just update the target column
            currentWidths[columnIndex] = newValue;
        }
        
        newGrids[gridIndex] = { ...newGrids[gridIndex], Width: currentWidths.join('|') };
        onGridsChange(newGrids);
    };

    const addGridColumn = (gridIndex) => {
        const newGrids = [...grids];
        const currentWidths = newGrids[gridIndex].Width.split('|').map(w => parseInt(w) || 100);
        const pageWidth = 580; // Match the preview container width
        const currentTotal = currentWidths.reduce((sum, width) => sum + width, 0);
        
        // Calculate available space for new column
        const availableSpace = pageWidth - currentTotal;
        const newColumnWidth = Math.max(50, Math.min(100, availableSpace)); // At least 50px, max 100px or available space
        
        currentWidths.push(newColumnWidth);
        
        // If we're still over the page width, scale everything down proportionally
        const newTotal = currentWidths.reduce((sum, width) => sum + width, 0);
        if (newTotal > pageWidth) {
            const scaleFactor = pageWidth / newTotal;
            currentWidths.forEach((width, index) => {
                currentWidths[index] = Math.round(width * scaleFactor);
            });
        }
        
        newGrids[gridIndex] = { ...newGrids[gridIndex], Width: currentWidths.join('|') };
        onGridsChange(newGrids);
    };

    const removeGridColumn = (gridIndex, columnIndex) => {
        const newGrids = [...grids];
        const currentWidths = newGrids[gridIndex].Width.split('|');
        if (currentWidths.length > 1) {
            currentWidths.splice(columnIndex, 1);
            newGrids[gridIndex] = { ...newGrids[gridIndex], Width: currentWidths.join('|') };
            onGridsChange(newGrids);
        }
    };

    const renderGridEditor = (grid, index) => {
        const columnWidths = grid.Width.split('|').map(w => parseInt(w) || 100);
        const totalWidth = columnWidths.reduce((sum, width) => sum + width, 0);
        
        return (
            <div key={index} className="grid-editor" id={`formateditor-grid-${index}`}>
                <div className="grid-header">
                    <h4>Grid {index + 1}</h4>
                    <IconButton
                        id={`formateditor-removegrid-${index}`}
                        iconProps={{ iconName: 'Delete' }}
                        onClick={() => onRemoveGrid(index)}
                        title="Remove grid"
                    />
                </div>
                <div className="grid-fields">
                    <TextField
                        id={`formateditor-grid-${index}-left`}
                        label="Left Position"
                        type="number"
                        value={grid.Left.toString()}
                        onChange={(e, value) => onGridChange(index, 'Left', value)}
                        suffix="px"
                    />
                    <div className="column-widths-section">
                        <div className="column-widths-header">
                            <h5>Column Widths (Total: {totalWidth}px / 580px)</h5>
                            <IconButton
                                id={`formateditor-grid-${index}-addcolumn`}
                                iconProps={{ iconName: 'Add' }}
                                onClick={() => addGridColumn(index)}
                                title="Add column"
                                disabled={totalWidth >= 580}
                            />
                        </div>
                        
                        {/* Visual Column Width Editor */}
                        <div className="visual-column-editor">
                            <div className="column-visualization">
                                {columnWidths.map((width, colIndex) => {
                                    const leftPosition = columnWidths.slice(0, colIndex).reduce((sum, w) => sum + w, 0);
                                    const leftPercentage = (leftPosition / 580) * 100;
                                    const widthPercentage = (width / 580) * 100;
                                    return (
                                        <div 
                                            key={colIndex} 
                                            className="column-visual-item"
                                            style={{ 
                                                left: `${leftPercentage}%`,
                                                width: `${widthPercentage}%`,
                                                backgroundColor: `hsl(${colIndex * 60}, 70%, 80%)`
                                            }}
                                        >
                                            <div className="column-label">
                                                Col {colIndex + 1}
                                            </div>
                                            <div className="column-width-display">
                                                {width}px
                                            </div>
                                        </div>
                                    );
                                })}
                                {/* Page width indicator */}
                                <div className="page-width-indicator" style={{ 
                                    left: `${(totalWidth / 580) * 100}%`,
                                    width: `${((580 - totalWidth) / 580) * 100}%` 
                                }}>
                                    <div className="page-width-label">Available: {580 - totalWidth}px</div>
                                </div>
                            </div>
                            
                            {/* Slider Controls */}
                            <div className="column-sliders">
                                {columnWidths.map((width, colIndex) => (
                                    <div key={colIndex} className="column-slider-item">
                                        <div className="slider-label">
                                            Column {colIndex + 1}
                                        </div>
                                        <div className="slider-container">
                                            <input
                                                id={`formateditor-grid-${index}-width-${colIndex}`}
                                                type="range"
                                                min="50"
                                                max="500"
                                                value={width}
                                                onChange={(e) => handleGridColumnWidthChange(index, colIndex, e.target.value)}
                                                onMouseUp={(e) => handleGridColumnWidthChangeEnd(index, colIndex, e.target.value)}
                                                onTouchEnd={(e) => handleGridColumnWidthChangeEnd(index, colIndex, e.target.value)}
                                                className="column-slider"
                                            />
                                            <div className="slider-value">{width}px</div>
                                        </div>
                                        {columnWidths.length > 1 && (
                                            <IconButton
                                                id={`formateditor-grid-${index}-removecolumn-${colIndex}`}
                                                iconProps={{ iconName: 'Delete' }}
                                                onClick={() => removeGridColumn(index, colIndex)}
                                                title="Remove column"
                                                className="remove-column-btn"
                                            />
                                        )}
                                    </div>
                                ))}
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        );
    };

    return (
        <div className="grids-section">
            <div className="section-header">
                <h3>Grids</h3>
                <PrimaryButton 
                    id="formateditor-addgrid"
                    text="Add Grid" 
                    onClick={onAddGrid}
                    iconProps={{ iconName: 'Add' }}
                />
            </div>
            <div className="grids-list">
                {grids.map(renderGridEditor)}
            </div>
        </div>
    );
};

export default GridDefinitionEditor;
