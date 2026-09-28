import React, { useState, useEffect } from 'react';
import { 
    PrimaryButton, 
    DefaultButton, 
    Panel,
    PanelType,
    DetailsList,
    DetailsListLayoutMode,
    SelectionMode,
    Selection,
    CommandBar,
    IconButton,
    TooltipHost
} from '@fluentui/react';
import SectionSelector from './SectionSelector';
import SectionFieldEditor from './SectionFieldEditor';
import SectionCreator from './SectionCreator';
import './SectionCategoryEditor.css';

const SectionCategoryEditor = ({ 
    isOpen, 
    onDismiss, 
    categoryData, 
    availableSections, 
    availableDataSections,
    onSave, 
    categoryType 
}) => {
    const [sections, setSections] = useState(categoryData?.sections || []);
    const [selectedSection, setSelectedSection] = useState(null);
    const [showSectionSelector, setShowSectionSelector] = useState(false);
    const [showSectionCreator, setShowSectionCreator] = useState(false);
    const [showSectionFieldEditor, setShowSectionFieldEditor] = useState(false);
    const [editingSection, setEditingSection] = useState(null);

    useEffect(() => {
        setSections(categoryData?.sections || []);
    }, [categoryData]);

    const selection = new Selection({
        onSelectionChanged: () => {
            const selectedItems = selection.getSelection();
            setSelectedSection(selectedItems.length > 0 ? selectedItems[0] : null);
        }
    });

    const handleAddSection = () => {
        setShowSectionSelector(true);
    };

    const handleCreateNewSection = () => {
        setShowSectionCreator(true);
    };

    const handleEditSection = (section) => {
        setEditingSection(section);
        setShowSectionFieldEditor(true);
    };

    const handleDeleteSection = (section) => {
        setSections(sections.filter(s => s.Name !== section.Name));
    };

    const handleCreateSection = (newSection) => {
        setSections([...sections, newSection]);
        setShowSectionCreator(false);
    };

    const handleSaveSection = (updatedSection) => {
        setSections(sections.map(s => 
            s.Name === updatedSection.Name ? updatedSection : s
        ));
        setShowSectionFieldEditor(false);
        setEditingSection(null);
    };

    const handleRemoveSection = () => {
        if (selectedSection) {
            setSections(sections.filter(s => s.id !== selectedSection.id));
            setSelectedSection(null);
        }
    };

    const handleMoveUp = () => {
        if (selectedSection) {
            const index = sections.findIndex(s => s.id === selectedSection.id);
            if (index > 0) {
                const newSections = [...sections];
                [newSections[index - 1], newSections[index]] = [newSections[index], newSections[index - 1]];
                setSections(newSections);
                
                // Maintain selection after reordering
                setTimeout(() => {
                    selection.setIndexSelected(index - 1, true, false);
                }, 0);
            }
        }
    };

    const handleMoveDown = () => {
        if (selectedSection) {
            const index = sections.findIndex(s => s.id === selectedSection.id);
            if (index < sections.length - 1) {
                const newSections = [...sections];
                [newSections[index], newSections[index + 1]] = [newSections[index + 1], newSections[index]];
                setSections(newSections);
                
                // Maintain selection after reordering
                setTimeout(() => {
                    selection.setIndexSelected(index + 1, true, false);
                }, 0);
            }
        }
    };

    const handleSectionSelectorSave = (selectedSections) => {
        setSections(selectedSections);
        setShowSectionSelector(false);
    };

    const handleSave = () => {
        onSave({
            sections: sections,
            type: categoryType
        });
        onDismiss();
    };

    const columns = [
        {
            key: 'name',
            name: 'Section Name',
            fieldName: 'Name',
            minWidth: 200,
            maxWidth: 300,
            isResizable: true
        },
        {
            key: 'description',
            name: 'Description',
            fieldName: 'Description',
            minWidth: 150,
            maxWidth: 200,
            isResizable: true
        },
        {
            key: 'format',
            name: 'Format',
            fieldName: 'Format',
            minWidth: 100,
            maxWidth: 150,
            isResizable: true
        },
        {
            key: 'fields',
            name: 'Fields',
            fieldName: 'Fields',
            minWidth: 80,
            maxWidth: 100,
            isResizable: true,
            onRender: (item) => item.Fields?.length || 0
        },
        {
            key: 'actions',
            name: 'Actions',
            minWidth: 100,
            maxWidth: 120,
            isResizable: true,
            onRender: (item) => (
                <div className="section-actions">
                    <TooltipHost content="Edit Section">
                        <IconButton
                            iconProps={{ iconName: 'Edit' }}
                            onClick={() => handleEditSection(item)}
                        />
                    </TooltipHost>
                    <TooltipHost content="Delete Section">
                        <IconButton
                            iconProps={{ iconName: 'Delete' }}
                            onClick={() => handleDeleteSection(item)}
                        />
                    </TooltipHost>
                </div>
            )
        }
    ];

    const commandBarItems = [
        {
            key: 'add',
            text: 'Select Existing Sections',
            iconProps: { iconName: 'Add' },
            onClick: handleAddSection
        },
        {
            key: 'create',
            text: 'Create New Section',
            iconProps: { iconName: 'AddTo' },
            onClick: handleCreateNewSection
        },
        {
            key: 'remove',
            text: 'Remove Section',
            iconProps: { iconName: 'Delete' },
            onClick: handleRemoveSection,
            disabled: !selectedSection
        },
        {
            key: 'moveUp',
            text: 'Move Up',
            iconProps: { iconName: 'Up' },
            onClick: handleMoveUp,
            disabled: !selectedSection || sections.findIndex(s => s.Name === selectedSection?.Name) === 0
        },
        {
            key: 'moveDown',
            text: 'Move Down',
            iconProps: { iconName: 'Down' },
            onClick: handleMoveDown,
            disabled: !selectedSection || sections.findIndex(s => s.Name === selectedSection?.Name) === sections.length - 1
        }
    ];

    return (
        <Panel
            isOpen={isOpen}
            onDismiss={onDismiss}
            type={PanelType.medium}
            headerText={`Edit ${categoryType}`}
            closeButtonAriaLabel="Close"
        >
            <div className="section-category-editor">

                <div className="editor-content">
                    <div className="sections-list-header">
                        <h3>Sections in this Category</h3>
                    </div>

                    <CommandBar items={commandBarItems} />

                    <DetailsList
                        items={sections}
                        columns={columns}
                        selection={selection}
                        layoutMode={DetailsListLayoutMode.fixedColumns}
                        selectionMode={SelectionMode.single}
                        isHeaderVisible={true}
                        className="sections-list"
                    />

                    {sections.length === 0 && (
                        <div className="empty-state">
                            <p>No sections added to this category yet.</p>
                            <PrimaryButton 
                                text="Add First Section" 
                                onClick={handleAddSection}
                                iconProps={{ iconName: 'Add' }}
                            />
                        </div>
                    )}
                </div>

                <div className="editor-footer">
                    <DefaultButton text="Cancel" onClick={onDismiss} />
                    <PrimaryButton text="Save" onClick={handleSave} />
                </div>
            </div>

            <SectionSelector
                isOpen={showSectionSelector}
                onDismiss={() => setShowSectionSelector(false)}
                availableSections={availableSections}
                selectedSections={sections}
                onSave={handleSectionSelectorSave}
                title={`Select ${categoryType} Sections`}
            />

            <SectionCreator
                isOpen={showSectionCreator}
                onDismiss={() => setShowSectionCreator(false)}
                availableDataSections={availableDataSections}
                onCreateSection={handleCreateSection}
            />

            <SectionFieldEditor
                isOpen={showSectionFieldEditor}
                onDismiss={() => {
                    setShowSectionFieldEditor(false);
                    setEditingSection(null);
                }}
                sectionDefinition={editingSection}
                dataSection={editingSection ? availableDataSections.find(ds => ds.Name === editingSection.DataSection) : null}
                onSave={handleSaveSection}
                onDelete={handleDeleteSection}
            />
        </Panel>
    );
};

export default SectionCategoryEditor;
