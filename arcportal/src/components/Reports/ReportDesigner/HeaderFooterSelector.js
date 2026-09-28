import React, { useState, useEffect } from 'react';
import { 
    PrimaryButton, 
    DefaultButton, 
    Dropdown, 
    Panel, 
    PanelType,
    MessageBar,
    MessageBarType
} from '@fluentui/react';
import AbsoluteSectionDesigner from './AbsoluteSectionDesigner';
import { getSectionDisplayTitle } from './reportSectionDisplay';
import './HeaderFooterSelector.css';

const HeaderFooterSelector = ({ 
    isOpen, 
    onDismiss, 
    type, // 'header' or 'footer'
    allowedSections, // e.g., "DefaultSpecimenReportHeader" or ["Header1", "Header2"]
    sectionDefinitions, // e.g., [{Name: "DefaultSpecimenReportHeader", Description: "..."}]
    reportConfig, // Add reportConfig to access References
    onSelectExisting,
    onCreateNew,
    language = []
}) => {
    const [showAbsoluteDesigner, setShowAbsoluteDesigner] = useState(false);
    const [selectedExisting, setSelectedExisting] = useState(null);

    // Helper function to get fields from the 'Main' reference (for headers/footers)
    const getMainReferenceFields = () => {
        if (!reportConfig || !reportConfig.References) {
            return [];
        }
        const mainReference = reportConfig.References.find(ref => ref.Name === 'Main');
        if (!mainReference || !mainReference.AvailableFields) {
            return [];
        }
        
        const fields = mainReference.AvailableFields.map(field => ({
            name: field.Name,
            label: field.Label
        })).sort((a, b) => a.label.localeCompare(b.label));
        return fields;
    };

    // Simple approach - match allowed sections with definitions
    const getAvailableSections = () => {
        if (!allowedSections || !sectionDefinitions) {
            return [];
        }
        
        // Convert allowedSections to array if it's a string
        const allowedList = Array.isArray(allowedSections) ? allowedSections : [allowedSections];
        
        // Filter definitions to only include allowed ones
        const available = sectionDefinitions.filter(def => allowedList.includes(def.Name));
        
        return available;
    };

    // Get currently selected section - this would need to be passed as a prop
    const getCurrentSelection = () => {
        return null; // For now, no current selection
    };

    const handleSelectExisting = () => {
        if (selectedExisting && onSelectExisting) {
            onSelectExisting(selectedExisting);
            onDismiss();
        }
    };

    const handleCreateNew = () => {
        setShowAbsoluteDesigner(true);
    };

    const handleSaveNewSection = (sectionData) => {
        if (onCreateNew) {
            onCreateNew(sectionData);
        }
        setShowAbsoluteDesigner(false);
        onDismiss();
    };

    const handleCancelNew = () => {
        setShowAbsoluteDesigner(false);
    };

    const availableSections = getAvailableSections();
    const currentSelection = getCurrentSelection();
    
    const dropdownOptions = availableSections.map(section => ({
        key: section.Name,
        text: getSectionDisplayTitle(section, language)
    }));
    

    return (
        <>
            <Panel
                isOpen={isOpen && !showAbsoluteDesigner}
                onDismiss={onDismiss}
                type={PanelType.medium}
                headerText={`Select ${type === 'header' ? 'Header' : 'Footer'} Section`}
                closeButtonAriaLabel="Close"
            >
                <div className="header-footer-selector">
                    {currentSelection && (
                        <MessageBar messageBarType={MessageBarType.info}>
                            Currently selected: {currentSelection}
                        </MessageBar>
                    )}

                    <div className="selection-section">
                        <h3>Select Existing {type === 'header' ? 'Header' : 'Footer'}</h3>
                        <p>Choose from predefined {type === 'header' ? 'header' : 'footer'} sections:</p>
                        
                        <Dropdown
                            label={`Available ${type === 'header' ? 'Headers' : 'Footers'}`}
                            options={dropdownOptions}
                            selectedKey={selectedExisting}
                            onChange={(e, option) => setSelectedExisting(option.key)}
                            placeholder={`Select a ${type === 'header' ? 'header' : 'footer'} section`}
                            className="section-dropdown"
                        />

                        <div className="action-buttons">
                            <PrimaryButton
                                text={`Use Selected ${type === 'header' ? 'Header' : 'Footer'}`}
                                onClick={handleSelectExisting}
                                disabled={!selectedExisting}
                                iconProps={{ iconName: 'CheckMark' }}
                            />
                        </div>
                    </div>

                    <div className="divider">
                        <span>OR</span>
                    </div>

                    <div className="creation-section">
                        <h3>Create New {type === 'header' ? 'Header' : 'Footer'}</h3>
                        <p>Design a custom {type === 'header' ? 'header' : 'footer'} section:</p>
                        
                        <PrimaryButton
                            text={`Create New ${type === 'header' ? 'Header' : 'Footer'}`}
                            onClick={handleCreateNew}
                            iconProps={{ iconName: 'Add' }}
                        />
                    </div>
                </div>
            </Panel>

            <AbsoluteSectionDesigner
                isOpen={showAbsoluteDesigner}
                onDismiss={handleCancelNew}
                sectionDefinition={null}
                categoryType={type === 'header' ? 'HeaderSectionDefinitions' : 'FooterSectionDefinitions'}
                availableFields={getMainReferenceFields()}
                availableOrganismFields={getMainReferenceFields()}
                availableImages={[]}
                onSave={handleSaveNewSection}
            />
        </>
    );
};

export default HeaderFooterSelector;
