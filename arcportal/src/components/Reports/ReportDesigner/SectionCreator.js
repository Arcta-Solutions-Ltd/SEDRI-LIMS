import React, { useState, useEffect } from 'react';
import { 
    PrimaryButton, 
    DefaultButton, 
    Panel,
    PanelType,
    Checkbox,
    MessageBar,
    MessageBarType
} from '@fluentui/react';
import './SectionCreator.css';

const SectionCreator = ({ 
    isOpen, 
    onDismiss, 
    availableDataSections, 
    categoryType, 
    onCreateSection,
    onCreateAbsoluteSection,
    onSaveNewSection
}) => {
    const [selectedDataSection, setSelectedDataSection] = useState(null);
    // This component is category-agnostic by design. Parent must pre-filter
    // and pass the correct availableDataSections for the creation context.

    // Reset state when panel opens
    useEffect(() => {
        if (isOpen) {
            setSelectedDataSection(null);
        }
    }, [isOpen, categoryType]);

    const dataSections = Array.isArray(availableDataSections) ? availableDataSections : [];

    /**
     * Updates the selected data section state when the user picks an item from the list.
     * @param {object} dataSection - The data section definition object that was selected,
     *   containing at minimum `Name`, `Title`, `Fields`, and `Grids` properties.
     */
    const handleDataSectionSelect = (dataSection) => {
        setSelectedDataSection(dataSection);
    };

    /**
     * Constructs a new report section from the selected data section and delegates
     * creation to the appropriate parent callback.
     * - Calls `onSaveNewSection` when `categoryType` is `'NewSection'` (command-bar flow).
     * - Calls `onCreateSection` for all other category types (per-category add flow).
     * Dismisses the panel after delegation.
     * New sections default HeadingText to the data section title; users can turn the section heading off
     * in the section editor after creation.
     */
    const handleCreate = () => {
        if (selectedDataSection) {
            const newSection = {
                Type: 'layout',
                Name: selectedDataSection.Name.replace('DataSection', 'Section'),
                Description: selectedDataSection.Title,
                HeadingText: selectedDataSection.Title,
                DataSection: selectedDataSection.Name,
                Fields: selectedDataSection.Fields ? selectedDataSection.Fields.map((field, index) => ({
                    Label: field.Label,
                    Value: field.Value,
                    Column: 1,
                    Order: index + 1
                })) : [],
                Grids: selectedDataSection.Grids ? selectedDataSection.Grids.map(grid => ({
                    Name: grid.name,
                    Head: []
                })) : [],
                Dynamic: false
            };

            // Delegate handling to parent without leaking category concerns here
            if (categoryType === 'NewSection') {
                onSaveNewSection && onSaveNewSection(newSection, categoryType);
            } else {
                onCreateSection && onCreateSection(newSection, categoryType);
            }
            onDismiss();
        }
    };

    /**
     * Renders a single selectable row in the data-sections list.
     * Displays the data section's `Title` as the checkbox label and shows the
     * field and grid counts as the description.
     * @param {object} dataSection - The data section definition to render, containing
     *   `Name` (unique key), `Title` (display label), `Fields` (array), and `Grids` (array).
     * @returns {JSX.Element} A `<div>` containing a Fluent UI `Checkbox` for the data section.
     */
    const renderDataSectionItem = (dataSection) => {
        const isSelected = selectedDataSection?.Name === dataSection.Name;
        
        return (
            <div key={dataSection.Name} className="data-section-item">
                <Checkbox
                    checked={isSelected}
                    onChange={() => handleDataSectionSelect(dataSection)}
                    label={dataSection.Title}
                    description={`${dataSection.Fields?.length || 0} fields, ${dataSection.Grids?.length || 0} grids`}
                />
            </div>
        );
    };

    return (
        <Panel
            isOpen={isOpen}
            onDismiss={onDismiss}
            type={PanelType.medium}
            headerText="Create New Section"
            closeButtonAriaLabel="Close"
        >
            <div className="section-creator" id="sectioncreator">
                <div className="creator-header">
                    <p>Select a data section to create a new report section from:</p>
                </div>

                <div className="creator-content">
                    <div className="section-type-options">
                        <h3>Choose Section Type</h3>
                        <div className="type-buttons">
                            <PrimaryButton
                                id="sectioncreator-create-absolute"
                                text="Create Absolute Section"
                                onClick={() => onCreateAbsoluteSection && onCreateAbsoluteSection()}
                                iconProps={{ iconName: 'Edit' }}
                                className="type-button"
                            />
                        </div>
                    </div>

                    <div className="data-sections-list">
                        {dataSections.length > 0 ? (
                            dataSections.map(renderDataSectionItem)
                        ) : (
                            <div className="no-results">
                                <p>No data sections available.</p>
                            </div>
                        )}
                    </div>

                    {selectedDataSection && (
                        <MessageBar messageBarType={MessageBarType.info}>
                            Selected: <strong>{selectedDataSection.Title}</strong> - {selectedDataSection.Fields?.length || 0} fields available
                        </MessageBar>
                    )}
                </div>

                <div className="creator-footer">
                    <DefaultButton text="Cancel" onClick={onDismiss} />
                    <PrimaryButton 
                        text="Create Section" 
                        onClick={handleCreate}
                        disabled={!selectedDataSection}
                    />
                </div>
            </div>
        </Panel>
    );
};

export default SectionCreator;
