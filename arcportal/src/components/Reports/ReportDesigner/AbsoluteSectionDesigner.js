import React, { useState, useEffect, useCallback, useRef } from 'react';
import { 
    PrimaryButton, 
    DefaultButton, 
    TextField, 
    Dropdown, 
    Checkbox, 
    Panel, 
    PanelType,
    IconButton,
    MessageBar,
    MessageBarType,
    Spinner,
    SpinnerSize
} from '@fluentui/react';
import axios from 'axios';
import { connect } from 'react-redux';
import { ReportPageDimensions } from '../ReportPageDimensions';
import { REPORT_LINE_FONT_SIZE_DROPDOWN_OPTIONS } from '../ReportFontSizeOptions';
import { getEffectiveFontSize, lineTextOverflowsPage } from '../ReportPdfTextMetrics';
import {
    ABSOLUTE_DEFAULT_FONT_SIZE,
    calculateAbsoluteSectionHeight
} from '../Functions/AbsoluteSectionLineLayout';
import {
    getDistinctLineCount,
    reorderLineGroups,
    remapCollapsedLineNumbers
} from './AbsoluteSectionLineUtils';
import {
    getElementsOnLineSorted,
    swapElementLeftValues,
    moveElementToLine,
    remapCollapsedElementIndices
} from './AbsoluteSectionElementUtils';
import AbsoluteSectionPreviewCanvas from './AbsoluteSectionPreviewCanvas';
import './AbsoluteSectionDesigner.css';

const {
    PAGE_WIDTH,
    MARGIN_LEFT,
    MARGIN_RIGHT,
    MARGIN_TOP,
    CONTENT_WIDTH
} = ReportPageDimensions;

/** Extra padding below the horizontal rule so the last preview line is fully visible. */
const PREVIEW_BOTTOM_PADDING = 20;
const MARGIN_RIGHT_OFFSET = PAGE_WIDTH - MARGIN_RIGHT;

const AbsoluteSectionDesigner = ({ 
    isOpen, 
    onDismiss, 
    sectionDefinition, 
    categoryType,
    availableFields,
    availableOrganismFields,
    availableImages,
    onSave,
    language = []
}) => {
    const [sectionName, setSectionName] = useState('');
    const [description, setDescription] = useState('');
    const [lines, setLines] = useState([]);
    const [images, setImages] = useState([]);
    const [collapsedLines, setCollapsedLines] = useState(new Set());
    const [collapsedElements, setCollapsedElements] = useState(new Set());
    const [collapsedImages, setCollapsedImages] = useState(new Set(['images'])); // Default images section collapsed
    const [draggedLineNumber, setDraggedLineNumber] = useState(null);
    const [dragOverLineNumber, setDragOverLineNumber] = useState(null);
    const [draggedElementIndex, setDraggedElementIndex] = useState(null);
    const [dragOverElementIndex, setDragOverElementIndex] = useState(null);
    const draggedLineNumberRef = useRef(null);
    const draggedElementIndexRef = useRef(null);
    const isFooterSection = categoryType === 'FooterSectionDefinitions';

    /**
     * Calculates preview container height using shared absolute section layout logic.
     * @returns {number} Preview height in points.
     */
    const calculatePreviewHeight = () =>
        calculateAbsoluteSectionHeight(lines, images, {
            startingY: isFooterSection ? 0 : MARGIN_TOP,
            bottomPadding: PREVIEW_BOTTOM_PADDING,
        });

    /**
     * Returns whether any preview line text extends past the right page margin.
     * @returns {boolean} True when at least one line overflows.
     */
    const hasPreviewTextOverflow = () =>
        lines.some((line) => lineTextOverflowsPage(line, PAGE_WIDTH, MARGIN_RIGHT, language));
    
    // Image cache: fileAttachmentId -> { objectUrl, width, height, contentType, loading, error }
    const [imageCache, setImageCache] = useState({});
    const imageCacheRef = useRef(new Map());

    // Fetch image from file service and cache it
    const fetchImage = useCallback(async (fileAttachmentId) => {
        if (!fileAttachmentId || imageCacheRef.current.has(fileAttachmentId)) {
            return imageCacheRef.current.get(fileAttachmentId);
        }

        // Mark as loading
        const loadingEntry = { loading: true, error: null };
        imageCacheRef.current.set(fileAttachmentId, loadingEntry);
        setImageCache(prev => ({ ...prev, [fileAttachmentId]: loadingEntry }));

        try {
            const token = localStorage.getItem('arctoken');
            const response = await axios.get(`file/${fileAttachmentId}/download`, {
                responseType: 'blob',
                headers: token ? { Authorization: `Bearer ${token}` } : {}
            });

            // Validate content type
            const contentType = response.headers['content-type'] || '';
            if (!contentType.startsWith('image/')) {
                throw new Error('File is not an image');
            }

            const objectUrl = URL.createObjectURL(response.data);
            
            // Get image dimensions
            const img = new Image();
            const dimensions = await new Promise((resolve, reject) => {
                img.onload = () => resolve({ width: img.naturalWidth, height: img.naturalHeight });
                img.onerror = reject;
                img.src = objectUrl;
            });

            const cacheEntry = {
                objectUrl,
                width: dimensions.width,
                height: dimensions.height,
                contentType,
                loading: false,
                error: null
            };

            imageCacheRef.current.set(fileAttachmentId, cacheEntry);
            setImageCache(prev => ({ ...prev, [fileAttachmentId]: cacheEntry }));
            return cacheEntry;
        } catch (error) {
            const errorEntry = {
                loading: false,
                error: error.message || 'Failed to load image',
                objectUrl: null,
                width: 0,
                height: 0,
                contentType: ''
            };
            imageCacheRef.current.set(fileAttachmentId, errorEntry);
            setImageCache(prev => ({ ...prev, [fileAttachmentId]: errorEntry }));
            return errorEntry;
        }
    }, []);

    // Cleanup object URLs on unmount
    useEffect(() => {
        return () => {
            imageCacheRef.current.forEach(entry => {
                if (entry.objectUrl) {
                    URL.revokeObjectURL(entry.objectUrl);
                }
            });
        };
    }, []);

    // Initialize section when panel opens
    useEffect(() => {
        if (isOpen) {
            if (sectionDefinition) {
                setSectionName(sectionDefinition.Name || '');
                setDescription(sectionDefinition.Description || '');
                const sectionLines = [...(sectionDefinition.Lines || [])];
                setLines(sectionLines);
                
                // Map image Names to FileAttachmentIds from availableImages
                const imagesWithIds = (sectionDefinition.Images || []).map(img => {
                    // Find the matching image in availableImages by name (case-insensitive)
                    const matchingAvailableImage = availableImages?.find(availImg => 
                        availImg.name && img.Name && 
                        availImg.name.toLowerCase() === img.Name.toLowerCase()
                    );
                    
                    return {
                        ...img,
                        FileAttachmentId: matchingAvailableImage?.FileAttachmentId || img.FileAttachmentId
                    };
                });
                setImages(imagesWithIds);
                
                // Default all existing lines and elements to collapsed
                const lineNumbers = [...new Set(sectionLines.map(line => line.Line))];
                setCollapsedLines(new Set(lineNumbers));
                setCollapsedElements(new Set(sectionLines.map((_, index) => index)));
                setCollapsedImages(new Set(['images']));
            } else {
                setSectionName('');
                setDescription('');
                setLines([]);
                setImages([]);
                // Reset collapse states for new sections
                setCollapsedLines(new Set());
                setCollapsedElements(new Set());
                setCollapsedImages(new Set(['images']));
            }
        }
    }, [isOpen, sectionDefinition, availableImages]);

    // Pre-fetch images when images array changes
    useEffect(() => {
        if (images && images.length > 0) {
            images.forEach(image => {
                if (image.FileAttachmentId && !imageCacheRef.current.has(image.FileAttachmentId)) {
                    fetchImage(image.FileAttachmentId);
                }
            });
        }
    }, [images, fetchImage]);

    const handleSave = () => {
        if (!sectionName.trim()) {
            alert('Please enter a section name');
            return;
        }

        const updatedSection = {
            ...sectionDefinition, // Preserve existing fields
            Name: sectionName,
            Description: description,
            Lines: lines,
            Images: images,
            Type: 'Absolute',
            Dynamic: false
        };

        onSave(updatedSection);
        onDismiss();
    };

    const handleCancel = () => {
        onDismiss();
    };

    const getAvailableFields = () => {
        if (categoryType === 'Organism') {
            return [...availableFields, ...availableOrganismFields];
        } else {
            return availableFields;
        }
    };

    const handleAddLine = () => {
        const newLineNumber = lines.length > 0 ? Math.max(...lines.map(l => l.Line)) + 1 : 1;
        const newLine = {
            Line: newLineNumber,
            Left: 20,
            Text: '',
            Field: '',
            FontSize: ABSOLUTE_DEFAULT_FONT_SIZE,
            Bold: false
        };
        setLines([...lines, newLine]);
        // Default new line to collapsed
        setCollapsedLines(prev => new Set([...prev, newLineNumber]));
    };

    const handleRemoveLine = (lineIndex) => {
        const newLines = lines.filter((_, index) => index !== lineIndex);
        setLines(newLines);
    };

    const handleLineChange = (lineIndex, field, value) => {
        const newLines = [...lines];
        const updatedElement = { ...newLines[lineIndex], [field]: value };
        
        // Ensure mutual exclusivity between Field and Text
        if (field === 'Field' && value) {
            // When a field is selected, clear any text
            updatedElement.Text = '';
        } else if (field === 'Text' && value) {
            // When text is entered, clear any field selection
            updatedElement.Field = '';
        }
        
        newLines[lineIndex] = updatedElement;
        setLines(newLines);
    };

    const handleAddElement = (lineNumber) => {
        const newElement = {
            Line: lineNumber,
            Left: 20, // Default left position
            Text: '',
            Field: '',
            FontSize: ABSOLUTE_DEFAULT_FONT_SIZE,
            Bold: false
        };
        const newLines = [...lines, newElement];
        setLines(newLines);
        // Default new element to collapsed (use the new index)
        const newElementIndex = newLines.length - 1;
        setCollapsedElements(prev => new Set([...prev, newElementIndex]));
    };

    // Image management functions
    const handleAddImage = () => {
        if (!availableImages || availableImages.length === 0) {
            alert('No images available. Please add images to the system first.');
            return;
        }
        
        const newImage = {
            X: 20,
            Y: 0,
            FileAttachmentId: availableImages[0].FileAttachmentId,
            Name: availableImages[0].name,
            Description: availableImages[0].Description,
            Label: '',
            Order: images.length + 1,
            Width: 100,
            Height: 100
        };
        const newImages = [...images, newImage];
        setImages(newImages);
        // Default new image to collapsed (use the new index)
        const newImageIndex = newImages.length - 1;
        setCollapsedImages(prev => new Set([...prev, newImageIndex]));
        
        // Pre-fetch the image
        fetchImage(newImage.FileAttachmentId);
    };

    const handleImageChange = (imageIndex, field, value) => {
        const newImages = [...images];
        newImages[imageIndex] = { ...newImages[imageIndex], [field]: value };
        
        // If changing FileAttachmentId, update related fields and pre-fetch image
        if (field === 'FileAttachmentId') {
            const selectedImage = availableImages.find(img => img.FileAttachmentId === value);
            if (selectedImage) {
                newImages[imageIndex].Name = selectedImage.name;
                newImages[imageIndex].Description = selectedImage.Description;
                // Pre-fetch the new image
                fetchImage(value);
            }
        }
        
        setImages(newImages);
    };

    const handleRemoveImage = (imageIndex) => {
        const newImages = images.filter((_, index) => index !== imageIndex);
        setImages(newImages);
    };

    const toggleLineCollapse = (lineNumber) => {
        const newCollapsed = new Set(collapsedLines);
        if (newCollapsed.has(lineNumber)) {
            newCollapsed.delete(lineNumber);
        } else {
            newCollapsed.add(lineNumber);
        }
        setCollapsedLines(newCollapsed);
    };

    const toggleElementCollapse = (elementIndex) => {
        const newCollapsed = new Set(collapsedElements);
        if (newCollapsed.has(elementIndex)) {
            newCollapsed.delete(elementIndex);
        } else {
            newCollapsed.add(elementIndex);
        }
        setCollapsedElements(newCollapsed);
    };

    const toggleImageCollapse = (imageIndex) => {
        const newCollapsed = new Set(collapsedImages);
        if (newCollapsed.has(imageIndex)) {
            newCollapsed.delete(imageIndex);
        } else {
            newCollapsed.add(imageIndex);
        }
        setCollapsedImages(newCollapsed);
    };

    /**
     * Starts dragging a line group from the gripper handle.
     * @param {DragEvent} e - Native drag event.
     * @param {number} lineNumber - Line number being dragged.
     */
    const handleLineDragStart = (e, lineNumber) => {
        draggedLineNumberRef.current = lineNumber;
        setDraggedLineNumber(lineNumber);
        e.dataTransfer.effectAllowed = 'move';
        e.dataTransfer.setData('text/plain', String(lineNumber));
        e.dataTransfer.setData('application/x-absolutesection-line', String(lineNumber));
    };

    /**
     * Allows dropping on a target line group and highlights the drop target.
     * @param {DragEvent} e - Native drag event.
     * @param {number} lineNumber - Line number of the drop target.
     */
    const handleLineDragOver = (e, lineNumber) => {
        e.preventDefault();
        e.dataTransfer.dropEffect = 'move';
        const isElementDrag = draggedElementIndexRef.current !== null || draggedElementIndex !== null;
        if (isElementDrag) {
            setDragOverLineNumber(lineNumber);
            return;
        }
        if (draggedLineNumber !== null && draggedLineNumber !== lineNumber) {
            setDragOverLineNumber(lineNumber);
        }
    };

    /**
     * Clears drag-over highlight when the pointer leaves a line group.
     * @param {DragEvent} e - Native drag event.
     * @param {number} lineNumber - Line number being left.
     */
    const handleLineDragLeave = (e, lineNumber) => {
        if (dragOverLineNumber === lineNumber) {
            setDragOverLineNumber(null);
        }
    };

    /**
     * Reorders line groups when a dragged line is dropped onto a target line.
     * @param {DragEvent} e - Native drag event.
     * @param {number} targetLineNumber - Line number to drop onto.
     */
    const handleLineDrop = (e, targetLineNumber) => {
        if (handleElementDropOnLine(e, targetLineNumber)) {
            return;
        }

        e.preventDefault();
        setDragOverLineNumber(null);

        const fromLineData = e.dataTransfer.getData('application/x-absolutesection-line')
            || e.dataTransfer.getData('text/plain');
        const parsedFromLine = fromLineData ? parseInt(fromLineData, 10) : NaN;
        const fromLineNumber = !Number.isNaN(parsedFromLine)
            ? parsedFromLine
            : draggedLineNumberRef.current ?? draggedLineNumber;

        if (fromLineNumber === null || Number.isNaN(fromLineNumber) || fromLineNumber === targetLineNumber) {
            draggedLineNumberRef.current = null;
            setDraggedLineNumber(null);
            return;
        }

        setLines((currentLines) => {
            const { lines: reorderedLines, lineNumberMap } = reorderLineGroups(
                currentLines,
                fromLineNumber,
                targetLineNumber
            );
            setCollapsedLines((prev) => remapCollapsedLineNumbers(prev, lineNumberMap));
            setCollapsedElements((prev) => remapCollapsedElementIndices(prev, currentLines, reorderedLines));
            return reorderedLines;
        });
        draggedLineNumberRef.current = null;
        setDraggedLineNumber(null);
    };

    /**
     * Resets drag state when a drag operation ends without a drop.
     */
    const handleLineDragEnd = () => {
        draggedLineNumberRef.current = null;
        setDraggedLineNumber(null);
        setDragOverLineNumber(null);
    };

    /**
     * Starts dragging an element from its gripper handle.
     * @param {DragEvent} e - Native drag event.
     * @param {number} originalIndex - Flat-array index of the element being dragged.
     */
    const handleElementDragStart = (e, originalIndex) => {
        e.stopPropagation();
        draggedElementIndexRef.current = originalIndex;
        setDraggedElementIndex(originalIndex);
        e.dataTransfer.effectAllowed = 'move';
        e.dataTransfer.setData('text/plain', String(originalIndex));
        e.dataTransfer.setData('application/x-absolutesection-element', String(originalIndex));
    };

    /**
     * Allows dropping an element onto another element and highlights the drop target.
     * @param {DragEvent} e - Native drag event.
     * @param {number} targetOriginalIndex - Flat-array index of the drop target element.
     */
    const handleElementDragOver = (e, targetOriginalIndex) => {
        e.preventDefault();
        e.stopPropagation();
        e.dataTransfer.dropEffect = 'move';
        const fromIndex = draggedElementIndexRef.current ?? draggedElementIndex;
        if (fromIndex !== null && fromIndex !== targetOriginalIndex) {
            setDragOverElementIndex(targetOriginalIndex);
        }
    };

    /**
     * Clears element drag-over highlight when the pointer leaves an element.
     * @param {DragEvent} e - Native drag event.
     * @param {number} targetOriginalIndex - Flat-array index being left.
     */
    const handleElementDragLeave = (e, targetOriginalIndex) => {
        if (dragOverElementIndex === targetOriginalIndex) {
            setDragOverElementIndex(null);
        }
    };

    /**
     * Reorders or moves an element when dropped onto another element.
     * Same-line drops swap Left values; cross-line drops update the Line property.
     * @param {DragEvent} e - Native drag event.
     * @param {number} targetOriginalIndex - Flat-array index of the drop target element.
     */
    const handleElementDrop = (e, targetOriginalIndex) => {
        e.preventDefault();
        e.stopPropagation();
        setDragOverElementIndex(null);

        const fromData = e.dataTransfer.getData('application/x-absolutesection-element')
            || e.dataTransfer.getData('text/plain');
        const parsedFromIndex = fromData ? parseInt(fromData, 10) : NaN;
        const fromIndex = !Number.isNaN(parsedFromIndex)
            ? parsedFromIndex
            : draggedElementIndexRef.current ?? draggedElementIndex;

        if (fromIndex === null || Number.isNaN(fromIndex) || fromIndex === targetOriginalIndex) {
            draggedElementIndexRef.current = null;
            setDraggedElementIndex(null);
            return;
        }

        setLines((currentLines) => {
            const fromElement = currentLines[fromIndex];
            const targetElement = currentLines[targetOriginalIndex];
            if (!fromElement || !targetElement) {
                return currentLines;
            }

            let updatedLines;
            if (fromElement.Line === targetElement.Line) {
                updatedLines = swapElementLeftValues(currentLines, fromIndex, targetOriginalIndex);
            } else {
                updatedLines = moveElementToLine(currentLines, fromIndex, targetElement.Line);
            }

            setCollapsedElements((prev) => remapCollapsedElementIndices(prev, currentLines, updatedLines));
            return updatedLines;
        });

        draggedElementIndexRef.current = null;
        setDraggedElementIndex(null);
    };

    /**
     * Moves an element to a line when dropped on the line container (not on a specific element).
     * @param {DragEvent} e - Native drag event.
     * @param {number} targetLineNumber - Destination line number.
     */
    const handleElementDropOnLine = (e, targetLineNumber) => {
        const fromData = e.dataTransfer.getData('application/x-absolutesection-element');
        if (!fromData) {
            return false;
        }

        e.preventDefault();
        e.stopPropagation();
        setDragOverLineNumber(null);
        setDragOverElementIndex(null);

        const fromIndex = parseInt(fromData, 10);
        if (Number.isNaN(fromIndex)) {
            return true;
        }

        setLines((currentLines) => {
            const updatedLines = moveElementToLine(currentLines, fromIndex, targetLineNumber);
            setCollapsedElements((prev) => remapCollapsedElementIndices(prev, currentLines, updatedLines));
            return updatedLines;
        });

        draggedElementIndexRef.current = null;
        setDraggedElementIndex(null);
        return true;
    };

    /**
     * Resets element drag state when a drag operation ends without a drop.
     */
    const handleElementDragEnd = () => {
        draggedElementIndexRef.current = null;
        setDraggedElementIndex(null);
        setDragOverElementIndex(null);
    };

    /**
     * Groups elements by line number, sorted by Left ascending within each line.
     * @returns {Object<number, Array>} Map of line number to sorted elements with originalIndex.
     */
    const getElementsByLine = () => {
        const grouped = {};
        const lineNumbers = [...new Set(lines.map((element) => element.Line))];
        lineNumbers.forEach((lineNumber) => {
            grouped[lineNumber] = getElementsOnLineSorted(lines, lineNumber);
        });
        return grouped;
    };

    const renderLine = (lineNumber, elements) => {
        const fieldOptions = getAvailableFields().map(field => ({
            key: field.name,
            text: field.label
        }));
        const isLineCollapsed = collapsedLines.has(lineNumber);
        const isDragging = draggedLineNumber === lineNumber;
        const isDragOver = dragOverLineNumber === lineNumber;

        return (
            <div
                key={lineNumber}
                id={`absolutesectiondesigner-line-group-${lineNumber}`}
                className={`line-container${isDragging ? ' dragging' : ''}${isDragOver ? ' drag-over' : ''}`}
                data-line-number={lineNumber}
                onDragOver={(e) => handleLineDragOver(e, lineNumber)}
                onDragLeave={(e) => handleLineDragLeave(e, lineNumber)}
                onDrop={(e) => handleLineDrop(e, lineNumber)}
            >
                <div className="line-header">
                    <div className="line-title-section">
                        <span
                            id={`absolutesectiondesigner-line-group-${lineNumber}-drag-handle`}
                            className="line-drag-handle"
                            title="Drag to reorder line"
                            role="button"
                            aria-label={`Drag to reorder line ${lineNumber}`}
                            draggable
                            onDragStart={(e) => handleLineDragStart(e, lineNumber)}
                            onDragEnd={handleLineDragEnd}
                        >
                            <IconButton
                                iconProps={{ iconName: 'GripperBarVertical' }}
                                styles={{ root: { padding: '4px', minWidth: 'auto', cursor: 'grab', pointerEvents: 'none' } }}
                                tabIndex={-1}
                            />
                        </span>
                        <IconButton
                            iconProps={{ iconName: isLineCollapsed ? 'ChevronRight' : 'ChevronDown' }}
                            onClick={() => toggleLineCollapse(lineNumber)}
                            title={isLineCollapsed ? "Expand line" : "Collapse line"}
                            styles={{ root: { padding: '4px', minWidth: 'auto' } }}
                        />
                        <h4>Line {lineNumber}</h4>
                    </div>
                    <div className="line-actions">
                        <IconButton
                            iconProps={{ iconName: 'Add' }}
                            onClick={() => handleAddElement(lineNumber)}
                            title="Add element to this line"
                            text="Add Element"
                        />
                        <IconButton
                            iconProps={{ iconName: 'Delete' }}
                            onClick={() => {
                                // Remove all elements on this line
                                const newLines = lines.filter(l => l.Line !== lineNumber);
                                setLines(newLines);
                            }}
                            title="Remove this line"
                            styles={{ root: { color: '#d13438' } }}
                            text="Remove Line"
                        />
                    </div>
                </div>
                
                {!isLineCollapsed && (
                    <div className="line-elements">
                        {elements.map((element, position) => {
                            const isElementCollapsed = collapsedElements.has(element.originalIndex);
                            const isElementDragging = draggedElementIndex === element.originalIndex;
                            const isElementDragOver = dragOverElementIndex === element.originalIndex;
                            return (
                                <div
                                    key={element.originalIndex}
                                    id={`absolutesectiondesigner-line-${lineNumber}-element-${position}`}
                                    className={`element-container${isElementDragging ? ' dragging' : ''}${isElementDragOver ? ' drag-over' : ''}`}
                                    onDragOver={(e) => handleElementDragOver(e, element.originalIndex)}
                                    onDragLeave={(e) => handleElementDragLeave(e, element.originalIndex)}
                                    onDrop={(e) => handleElementDrop(e, element.originalIndex)}
                                >
                                    <div className="element-header">
                                        <div className="element-title-section">
                                            <span
                                                id={`absolutesectiondesigner-line-${lineNumber}-element-${position}-drag-handle`}
                                                className="element-drag-handle"
                                                title="Drag to reorder or move element"
                                                role="button"
                                                aria-label={`Drag element on line ${lineNumber}`}
                                                draggable
                                                onDragStart={(e) => handleElementDragStart(e, element.originalIndex)}
                                                onDragEnd={handleElementDragEnd}
                                            >
                                                <IconButton
                                                    iconProps={{ iconName: 'GripperBarVertical' }}
                                                    styles={{ root: { padding: '4px', minWidth: 'auto', cursor: 'grab', pointerEvents: 'none' } }}
                                                    tabIndex={-1}
                                                />
                                            </span>
                                            <IconButton
                                                iconProps={{ iconName: isElementCollapsed ? 'ChevronRight' : 'ChevronDown' }}
                                                onClick={() => toggleElementCollapse(element.originalIndex)}
                                                title={isElementCollapsed ? "Expand element" : "Collapse element"}
                                                styles={{ root: { padding: '4px', minWidth: 'auto' } }}
                                            />
                                            <span className="element-title">
                                                {element.Field ? `Field: ${fieldOptions.find(f => f.key === element.Field)?.text || element.Field}` : element.Text ? `Text: ${element.Text.substring(0, 30)}${element.Text.length > 30 ? '...' : ''}` : 'Empty Element'}
                                            </span>
                                        </div>
                                        <IconButton
                                            iconProps={{ iconName: 'Delete' }}
                                            onClick={() => handleRemoveLine(element.originalIndex)}
                                            title="Remove this element"
                                            styles={{ root: { color: '#d13438' } }}
                                        />
                                    </div>
                                    
                                    {!isElementCollapsed && (
                                        <div className="element-controls">
                                <TextField
                                    id={`absolutesectiondesigner-line-${element.originalIndex}-left`}
                                    label="Left Position"
                                    type="number"
                                    value={element.Left.toString()}
                                    onChange={(e, value) => handleLineChange(element.originalIndex, 'Left', parseInt(value) || 0)}
                                    suffix="px"
                                    className="position-field"
                                />
                                
                                <Dropdown
                                    id={`absolutesectiondesigner-line-${element.originalIndex}-field`}
                                    label="Field"
                                    options={[{ key: '', text: 'Select Field' }, ...fieldOptions]}
                                    selectedKey={element.Field || ''}
                                    onChange={(e, option) => handleLineChange(element.originalIndex, 'Field', option.key)}
                                    className="field-dropdown"
                                    disabled={!!element.Text}
                                />
                                
                                <TextField
                                    id={`absolutesectiondesigner-line-${element.originalIndex}-text`}
                                    label="Text"
                                    value={element.Text || ''}
                                    onChange={(e, value) => handleLineChange(element.originalIndex, 'Text', value)}
                                    placeholder={element.Field ? "Clear field selection to enter text" : "Static text"}
                                    className="text-field"
                                    disabled={!!element.Field}
                                />
                                
                                <Dropdown
                                    id={`absolutesectiondesigner-line-${element.originalIndex}-font-size`}
                                    label="Font Size"
                                    options={REPORT_LINE_FONT_SIZE_DROPDOWN_OPTIONS}
                                    selectedKey={getEffectiveFontSize(element.FontSize)}
                                    onChange={(e, option) => handleLineChange(element.originalIndex, 'FontSize', option.key)}
                                    className="font-size-dropdown"
                                />
                                
                                <Checkbox
                                    label="Bold"
                                    checked={element.Bold || false}
                                    onChange={(e, checked) => handleLineChange(element.originalIndex, 'Bold', checked)}
                                    className="bold-checkbox"
                                />
                                
                                        </div>
                                    )}
                                </div>
                            );
                        })}
                    </div>
                )}
            </div>
        );
    };

    return (
        <Panel
            isOpen={isOpen}
            onDismiss={handleCancel}
            type={PanelType.extraLarge}
            closeButtonAriaLabel="Close"
            headerText="Absolute Section Designer"
        >
            <div className="absolute-section-designer" id="absolutesectiondesigner">
                <div className="designer-split-container">
                    {/* Left Panel - Section Info & Preview (Always Visible) */}
                    <div className="designer-left-panel">
                        <div className="section-info-sticky">
                            <div className="form-row">
                                <TextField
                                    id="absolutesectiondesigner-section-name"
                                    label="Section Name"
                                    value={sectionName}
                                    onChange={(e, value) => setSectionName(value)}
                                    className="form-field"
                                    disabled={!!sectionDefinition}
                                />
                            </div>
                            <div className="form-row">
                                <TextField
                                    label="Description"
                                    value={description}
                                    onChange={(e, value) => setDescription(value)}
                                    multiline
                                    rows={2}
                                    className="form-field"
                                />
                            </div>
                        </div>

                        <div className="preview-section-sticky">
                            <h3>Preview</h3>
                            <div className="preview-info">
                                <span>
                                    Page: {PAGE_WIDTH}pt | Margins: {MARGIN_LEFT}pt left, {MARGIN_RIGHT}pt right | Content: {CONTENT_WIDTH}pt
                                </span>
                                <span id="absolutesectiondesigner-preview-info-lines">
                                    Lines: {getDistinctLineCount(lines)} | Images: {images.length}
                                </span>
                            </div>
                            {hasPreviewTextOverflow() && (
                                <MessageBar
                                    id="absolutesectiondesigner-preview-overflow-warning"
                                    messageBarType={MessageBarType.warning}
                                    isMultiline
                                >
                                    One or more lines extend past the right margin and will overflow on the printed report.
                                </MessageBar>
                            )}
                            <div 
                                id="absolutesectiondesigner-preview-container"
                                className="preview-container" 
                                style={{ 
                                    width: `${PAGE_WIDTH}px`, 
                                    height: `${calculatePreviewHeight()}px`,
                                    maxHeight: '800px',
                                    overflow: 'auto'
                                }}
                            >
                                <div
                                    id="absolutesectiondesigner-margin-left"
                                    className="preview-margin-guide preview-margin-guide-left"
                                    style={{ left: `${MARGIN_LEFT}px` }}
                                    title={`Left margin (${MARGIN_LEFT}pt)`}
                                />
                                <div
                                    id="absolutesectiondesigner-margin-right"
                                    className="preview-margin-guide preview-margin-guide-right"
                                    style={{ left: `${MARGIN_RIGHT_OFFSET}px` }}
                                    title={`Right margin (${MARGIN_RIGHT}pt)`}
                                />
                                <AbsoluteSectionPreviewCanvas
                                    lines={lines}
                                    images={images}
                                    imageCache={imageCache}
                                    language={language}
                                    isFooter={isFooterSection}
                                    lineSpacing={sectionDefinition?.LineSpacing ?? 4}
                                />
                            </div>
                        </div>
                    </div>

                    {/* Right Panel - Controls (Scrollable) */}
                    <div className="designer-right-panel">
                        <div className="designer-actions">
                            <PrimaryButton
                                id="absolutesectiondesigner-add-line"
                                text="Add Line"
                                onClick={handleAddLine}
                                iconProps={{ iconName: 'Add' }}
                            />
                            <PrimaryButton
                                text="Add Image"
                                onClick={handleAddImage}
                                iconProps={{ iconName: 'Image' }}
                            />
                        </div>

                        <div className="lines-section">
                        <h3>Section Lines</h3>
                        {lines.length === 0 ? (
                            <MessageBar messageBarType={MessageBarType.info}>
                                No lines defined. Click "Add Line" to start designing your section.
                            </MessageBar>
                        ) : (
                            <div className="lines-list">
                                {Object.entries(getElementsByLine())
                                    .sort(([a], [b]) => parseInt(a) - parseInt(b))
                                    .map(([lineNumber, elements]) => renderLine(parseInt(lineNumber), elements))}
                            </div>
                        )}
                        </div>

                        <div className="images-section">
                            <div className="images-header">
                                <div className="images-title-section">
                                    <IconButton
                                        iconProps={{ iconName: collapsedImages.has('images') ? 'ChevronRight' : 'ChevronDown' }}
                                        onClick={() => toggleImageCollapse('images')}
                                        title={collapsedImages.has('images') ? "Expand images" : "Collapse images"}
                                        styles={{ root: { padding: '4px', minWidth: 'auto' } }}
                                    />
                                    <h3>Section Images</h3>
                                </div>
                            </div>
                        
                        {!collapsedImages.has('images') && (
                            <>
                                {images.length === 0 ? (
                                    <MessageBar messageBarType={MessageBarType.info}>
                                        No images added yet. Click "Add Image" to add an image to this section.
                                    </MessageBar>
                                ) : (
                                    <div className="images-list">
                                        {images.map((image, index) => {
                                            const isImageCollapsed = collapsedImages.has(index);
                                            return (
                                                <div key={index} className="image-container">
                                                    <div className="image-header">
                                                        <div className="image-title-section">
                                                            <IconButton
                                                                iconProps={{ iconName: isImageCollapsed ? 'ChevronRight' : 'ChevronDown' }}
                                                                onClick={() => toggleImageCollapse(index)}
                                                                title={isImageCollapsed ? "Expand image" : "Collapse image"}
                                                                styles={{ root: { padding: '4px', minWidth: 'auto' } }}
                                                            />
                                                            <span className="image-title">
                                                                {image.Name || 'Unnamed Image'} - {image.Label || 'No Label'}
                                                            </span>
                                                        </div>
                                                        <IconButton
                                                            iconProps={{ iconName: 'Delete' }}
                                                            onClick={() => handleRemoveImage(index)}
                                                            title="Remove image"
                                                            styles={{ root: { color: '#d13438' } }}
                                                        />
                                                    </div>
                                                    
                                                    {!isImageCollapsed && (
                                                        <div className="image-content">
                                                            <div className="image-controls">
                                            <TextField
                                                label="X Position"
                                                type="number"
                                                value={image.X.toString()}
                                                onChange={(e, value) => handleImageChange(index, 'X', parseInt(value) || 0)}
                                                suffix="px"
                                                className="position-field"
                                            />
                                            
                                            <TextField
                                                label="Y Position"
                                                type="number"
                                                value={image.Y.toString()}
                                                onChange={(e, value) => handleImageChange(index, 'Y', parseInt(value) || 0)}
                                                suffix="px"
                                                className="position-field"
                                            />
                                            
                                            <Dropdown
                                                label="Image"
                                                options={availableImages ? availableImages.map(img => ({
                                                    key: img.FileAttachmentId,
                                                    text: `${img.name} - ${img.Description}`
                                                })) : []}
                                                selectedKey={image.FileAttachmentId || ''}
                                                onChange={(e, option) => handleImageChange(index, 'FileAttachmentId', option.key)}
                                                className="image-dropdown"
                                            />
                                            
                                            <TextField
                                                label="Label"
                                                value={image.Label || ''}
                                                onChange={(e, value) => handleImageChange(index, 'Label', value)}
                                                placeholder="Image label"
                                                className="label-field"
                                            />
                                            
                                            <TextField
                                                label="Width"
                                                type="number"
                                                value={image.Width.toString()}
                                                onChange={(e, value) => handleImageChange(index, 'Width', parseInt(value) || 100)}
                                                suffix="px"
                                                className="size-field"
                                            />
                                            
                                            <TextField
                                                label="Height"
                                                type="number"
                                                value={image.Height.toString()}
                                                onChange={(e, value) => handleImageChange(index, 'Height', parseInt(value) || 100)}
                                                suffix="px"
                                                className="size-field"
                                            />
                                            
                                                            </div>
                                                            
                                                            <div className="image-preview">
                                            {(() => {
                                                if (!image.FileAttachmentId) {
                                                    return (
                                                        <div style={{ 
                                                            display: 'flex', 
                                                            alignItems: 'center', 
                                                            justifyContent: 'center',
                                                            color: '#666',
                                                            fontSize: '12px',
                                                            height: '100px',
                                                            border: '1px dashed #ccc'
                                                        }}>
                                                            No image selected
                                                        </div>
                                                    );
                                                }

                                                const cacheEntry = imageCache[image.FileAttachmentId];
                                                
                                                if (!cacheEntry) {
                                                    // Trigger fetch if not in cache
                                                    fetchImage(image.FileAttachmentId);
                                                    return (
                                                        <div style={{ 
                                                            display: 'flex', 
                                                            alignItems: 'center', 
                                                            justifyContent: 'center',
                                                            height: '100px',
                                                            border: '1px dashed #ccc'
                                                        }}>
                                                            <Spinner size={SpinnerSize.small} label="Loading..." />
                                                        </div>
                                                    );
                                                }

                                                if (cacheEntry.loading) {
                                                    return (
                                                        <div style={{ 
                                                            display: 'flex', 
                                                            alignItems: 'center', 
                                                            justifyContent: 'center',
                                                            height: '100px',
                                                            border: '1px dashed #ccc'
                                                        }}>
                                                            <Spinner size={SpinnerSize.small} label="Loading..." />
                                                        </div>
                                                    );
                                                }

                                                if (cacheEntry.error) {
                                                    return (
                                                        <div style={{ 
                                                            display: 'flex', 
                                                            flexDirection: 'column',
                                                            alignItems: 'center', 
                                                            justifyContent: 'center',
                                                            color: '#d13438',
                                                            fontSize: '12px',
                                                            height: '100px',
                                                            border: '1px dashed #d13438',
                                                            padding: '8px'
                                                        }}>
                                                            <div>Failed to load image</div>
                                                            <div style={{ fontSize: '10px', marginTop: '4px' }}>{cacheEntry.error}</div>
                                                            <DefaultButton 
                                                                text="Retry" 
                                                                size="small"
                                                                onClick={() => fetchImage(image.FileAttachmentId)}
                                                                style={{ marginTop: '4px' }}
                                                            />
                                                        </div>
                                                    );
                                                }

                                                if (cacheEntry.objectUrl) {
                                                    return (
                                                        <img
                                                            src={cacheEntry.objectUrl}
                                                            alt={image.Label || image.Name || 'Image'}
                                                            style={{
                                                                width: Math.min(image.Width, 150),
                                                                height: Math.min(image.Height, 150),
                                                                objectFit: 'contain',
                                                                border: '1px solid #ccc'
                                                            }}
                                                            onLoad={() => {}}
                                                            onError={() => {}}
                                                        />
                                                    );
                                                }

                                                return (
                                                    <div style={{ 
                                                        display: 'flex', 
                                                        alignItems: 'center', 
                                                        justifyContent: 'center',
                                                        color: '#666',
                                                        fontSize: '12px',
                                                        height: '100px',
                                                        border: '1px dashed #ccc'
                                                    }}>
                                                        Unknown error
                                                    </div>
                                                );
                                            })()}
                                                            </div>
                                                        </div>
                                                    )}
                                                </div>
                                            );
                                        })}
                                    </div>
                                )}
                            </>
                        )}
                        </div>
                    </div>
                </div>

                <div className="designer-footer">
                    <DefaultButton id="absolutesectiondesigner-cancel" text="Cancel" onClick={handleCancel} />
                    <PrimaryButton id="absolutesectiondesigner-save" text="Save Section" onClick={handleSave} />
                </div>
            </div>
        </Panel>
    );
};

const mapStateToProps = (state) => ({
    language: state.config.language,
});

export default connect(mapStateToProps)(AbsoluteSectionDesigner);
