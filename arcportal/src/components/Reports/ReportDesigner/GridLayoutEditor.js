import React, { useState, useEffect } from 'react';
import { PrimaryButton, DefaultButton, IconButton, TextField, Panel, PanelType, Dropdown, Checkbox } from '@fluentui/react';
import { trimSectionGridsToFormat, trimSectionGridHeadsToFormat } from './sectionValidation';
import { getGridDisplayText } from './gridLayoutDisplay';
import './GridLayoutEditor.css';

const GridLayoutEditor = ({ 
    isOpen, 
    onDismiss, 
    grids, 
    format, 
    dataSection,
    dataSectionGrids,
    language = [],
    onSave 
}) => {
    const [localGrids, setLocalGrids] = useState([]);
    const [pageWidth] = useState(580); // A4 page width in pixels

    /**
     * Picks the format grid position supplying the geometry for a slot, reusing the last position
     * where there are more slots than the format defines.
     * @param {Array} formatGrids - The grid positions the format defines.
     * @param {number} index - The zero based slot index.
     * @returns {object|null} The format grid to read geometry from, or null when the format has none.
     */
    const formatGridForIndex = (formatGrids, index) => {
        if (!formatGrids || formatGrids.length === 0) {
            return null;
        }
        return index < formatGrids.length ? formatGrids[index] : formatGrids[formatGrids.length - 1];
    };

    /**
     * Builds one local slot per format grid position when the panel opens.
     */
    useEffect(() => {
        if (isOpen && format && format.Grids && format.Grids.length > 0) {
            const formatGrids = format.Grids || [];
            const trimmedGrids = trimSectionGridHeadsToFormat(
                trimSectionGridsToFormat(grids, format),
                format
            );
            const slotCount = formatGrids.length;
            const initializedGrids = Array.from({ length: slotCount }, (_, index) => {
                const formatGrid = formatGridForIndex(formatGrids, index);
                const existingGrid = trimmedGrids[index] || {};
                let name = existingGrid.Name || '';
                if (!name && dataSectionGrids?.length === 1) {
                    name = dataSectionGrids[0].name || dataSectionGrids[0].Name || '';
                }

                return {
                    Name: name,  // Data section grid name
                    Head: initializeHeadersForFormatGrid(formatGrid, existingGrid.Head || []),
                    NoBox: !!existingGrid.NoBox
                };
            });
            setLocalGrids(initializedGrids);
        } else {
            // Clear localGrids when format is invalid or panel is closed
            setLocalGrids([]);
        }
    }, [isOpen, format, grids, dataSectionGrids]);

    /**
     * Persists grid bindings for each format position and closes the panel.
     */
    const handleSave = () => {
        const formatCapacity = format?.Grids?.length ?? 0;
        const gridsToSave = localGrids.slice(0, formatCapacity).map((grid, index) => {
            let name = grid.Name || '';
            if (!name && dataSectionGrids?.length === 1) {
                name = dataSectionGrids[0].name || dataSectionGrids[0].Name || '';
            }

            return {
                Name: name,
                Head: grid.Head || [],
                NoBox: !!grid.NoBox
            };
        });
        onSave(gridsToSave);
        onDismiss();
    };

    const handleCancel = () => {
        setLocalGrids([]);
        onDismiss();
    };

    const handleHeaderChange = (gridIndex, headerIndex, value) => {
        const newGrids = [...localGrids];
        const newHeaders = [...newGrids[gridIndex].Head];
        newHeaders[headerIndex] = value;
        newGrids[gridIndex] = { ...newGrids[gridIndex], Head: newHeaders };
        setLocalGrids(newGrids);
    };

    const handleGridSelectionChange = (gridIndex, gridName) => {
        const newGrids = [...localGrids];
        newGrids[gridIndex] = { ...newGrids[gridIndex], Name: gridName };
        setLocalGrids(newGrids);
    };

    const handleNoBoxChange = (gridIndex, checked) => {
        const newGrids = [...localGrids];
        newGrids[gridIndex] = { ...newGrids[gridIndex], NoBox: !!checked };
        setLocalGrids(newGrids);
    };

    // Initialize headers to match format grid column count
    const initializeHeadersForFormatGrid = (formatGrid, currentHeaders) => {
        const columnCount = getColumnCountForFormatGrid(formatGrid);
        
        // Ensure we have the right number of headers
        if (currentHeaders.length < columnCount) {
            // Add empty headers for missing columns
            const newHeaders = [...currentHeaders];
            while (newHeaders.length < columnCount) {
                newHeaders.push('');
            }
            return newHeaders;
        } else if (currentHeaders.length > columnCount) {
            // Remove excess headers
            return currentHeaders.slice(0, columnCount);
        }
        
        return currentHeaders;
    };

    const getColumnCountForFormatGrid = (formatGrid) => {
        if (formatGrid && formatGrid.Width) {
            return formatGrid.Width.split('|').length;
        }
        return 2; // Default to 2 columns
    };

    const getColumnWidthsForFormatGrid = (formatGrid) => {
        if (formatGrid && formatGrid.Width) {
            return formatGrid.Width.split('|').map(width => parseInt(width) || 150);
        }
        return [150, 150]; // Default widths
    };

    const getTableLeftForFormatGrid = (formatGrid) => {
        return formatGrid ? (formatGrid.Left || 10) : 10;
    };

    const renderGridItem = (grid, gridIndex) => {
        const formatGrid = formatGridForIndex(format?.Grids, gridIndex);

        // Guard: Don't render if the format defines no grid positions at all
        if (!formatGrid) {
            return null;
        }
        const columnWidths = getColumnWidthsForFormatGrid(formatGrid);
        const tableLeft = getTableLeftForFormatGrid(formatGrid);

        return (
            <div key={gridIndex} className="grid-item" id={`gridlayouteditor-grid-${gridIndex}`}>
                <div className="grid-header">
                    <h4>Grid {gridIndex + 1}</h4>
                    <div className="grid-info">
                        <span className="column-count">{getColumnCountForFormatGrid(formatGrid)} columns</span>
                    </div>
                </div>
                
                <div className="data-section-assignment">
                    <Checkbox
                        id={`gridlayouteditor-grid-${gridIndex}-nobox`}
                        label="No box"
                        checked={!!grid.NoBox}
                        onChange={(e, checked) => handleNoBoxChange(gridIndex, !!checked)}
                        styles={{ root: { marginBottom: 8 } }}
                    />
                    <Dropdown
                        id={`gridlayouteditor-grid-${gridIndex}-datasection`}
                        label="Data Section Grid"
                        options={[
                            { key: '', text: 'Select Grid' },
                            ...(dataSectionGrids || []).map(dsGrid => ({
                                key: dsGrid.name || dsGrid.Name,
                                text: getGridDisplayText(dsGrid, language)
                            }))
                        ]}
                        selectedKey={grid.Name || ''}
                        onChange={(e, option) => handleGridSelectionChange(gridIndex, option.key)}
                        placeholder="Choose grid from data section"
                    />
                </div>

                <div className="grid-preview">
                    <div className="format-preview" style={{ width: `${pageWidth}px` }}>
                        <div className="table-preview">
                            <div className="table-header-row">
                                {(grid.Head || []).map((header, headerIndex) => {
                                    const width = columnWidths[headerIndex] || 150;
                                    const widthPercent = (width / pageWidth) * 100;
                                    return (
                                        <div
                                            key={headerIndex}
                                            className="table-header-cell"
                                            style={{ 
                                                width: `${widthPercent}%`,
                                                left: `${(tableLeft / pageWidth) * 100}%`
                                            }}
                                        >
                                            <TextField
                                                id={`gridlayouteditor-grid-${gridIndex}-header-${headerIndex}`}
                                                value={header}
                                                onChange={(e, value) => handleHeaderChange(gridIndex, headerIndex, value)}
                                                placeholder={`Header ${headerIndex + 1}`}
                                                className="header-input"
                                            />
                                        </div>
                                    );
                                })}
                            </div>
                            <div className="table-data-row">
                                {(grid.Head || []).map((_, headerIndex) => {
                                    const width = columnWidths[headerIndex] || 150;
                                    const widthPercent = (width / pageWidth) * 100;
                                    return (
                                        <div
                                            key={headerIndex}
                                            className="table-data-cell"
                                            style={{ 
                                                width: `${widthPercent}%`,
                                                left: `${(tableLeft / pageWidth) * 100}%`
                                            }}
                                        >
                                            <div className="data-placeholder" aria-hidden="true" />
                                        </div>
                                    );
                                })}
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        );
    };

    return (
        <Panel
            isOpen={isOpen}
            onDismiss={handleCancel}
            type={PanelType.medium}
            closeButtonAriaLabel="Close"
            headerText="Grid Layout Editor"
        >
            <div className="grid-layout-editor" id="gridlayouteditor">
                <div className="editor-content">
                    <div className="format-info">
                        <h3>Format: {format?.Name || 'Unknown'}</h3>
                        <p>Configure grid headers and select which data section grid to use. Each grid from the format corresponds to a grid position in the section.</p>
                        <div className="format-details">
                            <span id="gridlayouteditor-format-grid-count"><strong>Grids in Format:</strong> {format?.Grids?.length || 0}</span>
                        </div>
                    </div>

                    <div className="grids-section">
                        <h3>Grid Configuration</h3>
                        {!format || !format.Grids || localGrids.length === 0 ? (
                            <div className="no-grids">
                                <p>{!format ? 'No format selected.' : 'No grids found in the selected format.'}</p>
                            </div>
                        ) : (
                            <div className="grids-list" id="gridlayouteditor-grids-list">
                                {localGrids.map((grid, index) => renderGridItem(grid, index))}
                            </div>
                        )}
                    </div>
                </div>

                <div className="editor-footer">
                    <DefaultButton id="gridlayouteditor-cancel" text="Cancel" onClick={handleCancel} />
                    <PrimaryButton id="gridlayouteditor-save" text="Save" onClick={handleSave} />
                </div>
            </div>
        </Panel>
    );
};

export default GridLayoutEditor;
