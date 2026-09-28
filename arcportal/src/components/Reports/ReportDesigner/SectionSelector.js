import React, { useState, useEffect } from 'react';
import { 
    PrimaryButton, 
    DefaultButton, 
    Panel,
    PanelType,
    SearchBox,
    Checkbox
} from '@fluentui/react';
import './SectionSelector.css';

const SectionSelector = ({ 
    isOpen, 
    onDismiss, 
    availableSections, 
    selectedSections, 
    onSave, 
    title = "Select Sections" 
}) => {
    const [searchTerm, setSearchTerm] = useState('');
    const [localSelectedSections, setLocalSelectedSections] = useState(selectedSections || []);
    const [filteredSections, setFilteredSections] = useState([]);

    useEffect(() => {
        setLocalSelectedSections(selectedSections || []);
    }, [selectedSections]);

    // Initialize filteredSections when component mounts or availableSections changes
    useEffect(() => {
        setFilteredSections(availableSections || []);
    }, [availableSections]);

    useEffect(() => {
        if (searchTerm) {
            const filtered = availableSections.filter(section =>
                section.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
                section.description?.toLowerCase().includes(searchTerm.toLowerCase())
            );
            setFilteredSections(filtered);
        } else {
            setFilteredSections(availableSections || []);
        }
    }, [searchTerm, availableSections]);

    const handleSectionToggle = (section) => {
        const isSelected = localSelectedSections.some(s => s.id === section.id);
        if (isSelected) {
            setLocalSelectedSections(localSelectedSections.filter(s => s.id !== section.id));
        } else {
            setLocalSelectedSections([...localSelectedSections, section]);
        }
    };

    const handleSelectAll = () => {
        setLocalSelectedSections(filteredSections);
    };

    const handleSelectNone = () => {
        setLocalSelectedSections([]);
    };

    const handleSave = () => {
        onSave(localSelectedSections);
        onDismiss();
    };

    const renderSectionItem = (section) => {
        const isSelected = localSelectedSections.some(s => s.id === section.id);
        
        return (
            <div key={section.id} className="section-item">
                <Checkbox
                    checked={isSelected}
                    onChange={() => handleSectionToggle(section)}
                    label={section.name}
                    description={section.description}
                />
            </div>
        );
    };

    return (
        <Panel
            isOpen={isOpen}
            onDismiss={onDismiss}
            type={PanelType.medium}
            headerText={title}
            closeButtonAriaLabel="Close"
        >
            <div className="section-selector">
                <div className="selector-header">
                    <div className="search-container">
                        <SearchBox
                            placeholder="Search sections..."
                            value={searchTerm}
                            onChange={(e, value) => setSearchTerm(value)}
                        />
                    </div>
                    <div className="bulk-actions">
                        <DefaultButton 
                            text="Select All" 
                            onClick={handleSelectAll}
                            disabled={filteredSections.length === 0}
                        />
                        <DefaultButton 
                            text="Select None" 
                            onClick={handleSelectNone}
                            disabled={localSelectedSections.length === 0}
                        />
                    </div>
                </div>

                <div className="selector-content">
                    <div className="selection-info">
                        <span>
                            {localSelectedSections.length} of {filteredSections.length} sections selected
                        </span>
                    </div>
                    
                    <div className="sections-list">
                        {filteredSections.length > 0 ? (
                            filteredSections.map(renderSectionItem)
                        ) : (
                            <div className="no-results">
                                <p>No sections found matching your search.</p>
                            </div>
                        )}
                    </div>
                </div>

                <div className="selector-footer">
                    <DefaultButton text="Cancel" onClick={onDismiss} />
                    <PrimaryButton 
                        text="Save Selection" 
                        onClick={handleSave}
                        disabled={localSelectedSections.length === 0}
                    />
                </div>
            </div>
        </Panel>
    );
};

export default SectionSelector;
