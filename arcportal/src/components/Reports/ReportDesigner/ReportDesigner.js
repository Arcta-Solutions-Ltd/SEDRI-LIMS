import React, { useState, useEffect, useMemo, useRef } from 'react';
import { 
    PrimaryButton, 
    TextField, 
    Dropdown,
    Toggle,
    CommandBar,
    IconButton,
    TooltipHost,
    MessageBar,
    MessageBarType,
    Panel,
    PanelType,
    DefaultButton,
    Spinner,
    SpinnerSize
} from '@fluentui/react';
import SectionCreator from './SectionCreator';
import SectionFieldEditor from './SectionFieldEditor';
import AbsoluteSectionDesigner from './AbsoluteSectionDesigner';
import FormatEditor from './FormatEditor';
import HeaderFooterSelector from './HeaderFooterSelector';
import './ReportDesigner.css';
import ReportPreviewOverlay from './ReportPreviewOverlay';
import { buildChangeSet, findFormatByName, normaliseName } from './reportChangeSet';
import {
    applyHeaderFooterSaveScope,
    getSharedHeaderFooterSectionsNeedingPrompt,
    HEADER_FOOTER_SAVE_SCOPE,
    needsHeaderFooterSaveScopePrompt,
} from './headerFooterSaveScope';
import { getSectionDisplayTitle, getSectionMetaLabel } from './reportSectionDisplay';
import { validateSection } from './sectionValidation';
import TranslateTag from '../../../Utils/Local/TranslateTag';

/**
 * Translates a language tag and substitutes numbered placeholders.
 * @param {Array<Object>|undefined} language - The active language pack.
 * @param {string} tag - The language tag key.
 * @param {...string} args - Values for {0}, {1}, etc.
 * @returns {string} The translated and formatted text.
 */
const formatTag = (language, tag, ...args) => {
    let text = TranslateTag(tag, language) || tag;
    args.forEach((arg, index) => {
        text = text.replace(`{${index}}`, arg);
    });
    return text;
};

const ReportDesigner = (props) => {
    const [reportConfig, setReportConfig] = useState(props.reportConfig || {});
    const [showSectionCreator, setShowSectionCreator] = useState(false);
    const [showSectionFieldEditor, setShowSectionFieldEditor] = useState(false);
    const [showAbsoluteSectionDesigner, setShowAbsoluteSectionDesigner] = useState(false);
    const [showHeaderFooterSelector, setShowHeaderFooterSelector] = useState(false);
    const [headerFooterType, setHeaderFooterType] = useState(null);
    const [showDeleteConfirmation, setShowDeleteConfirmation] = useState(false);
    const [deleteConfirmationData, setDeleteConfirmationData] = useState(null);
    const [showClearConfirmation, setShowClearConfirmation] = useState(false);
    const [showHeaderFooterSaveScope, setShowHeaderFooterSaveScope] = useState(false);
    const [pendingSaveData, setPendingSaveData] = useState(null);
    const [pendingSaveMode, setPendingSaveMode] = useState(null);
    const [editingSection, setEditingSection] = useState(null);
    const [currentCategoryType, setCurrentCategoryType] = useState(null);
    const [showPreview, setShowPreview] = useState(false);
    const [hasUnsavedChanges, setHasUnsavedChanges] = useState(false);
    // Why a save failed, or null. A failed save leaves the unsaved changes banner up, which on its own
    // tells the user nothing about what went wrong or whether retrying is worth it.
    const [saveError, setSaveError] = useState(null);
    const [isPreviewOpen, setIsPreviewOpen] = useState(false);
    const [previewData, setPreviewData] = useState(null);
    const [isSavingForPreview, setIsSavingForPreview] = useState(false);
    const [isSaving, setIsSaving] = useState(false);
    const [draggedSection, setDraggedSection] = useState(null);
    const [draggedCategory, setDraggedCategory] = useState(null);
    const [draggedCategoryOrder, setDraggedCategoryOrder] = useState(null);
    const draggedCategoryRef = useRef(null);
    const [customFormats, setCustomFormats] = useState([]);
    
    // New: stable inputs for SectionCreator to avoid timing issues
    const [creatorCategoryType, setCreatorCategoryType] = useState(null);
    const [creatorAvailableDataSections, setCreatorAvailableDataSections] = useState([]);
    
    // Change tracking state. These sets only drive the "changed" highlighting in the UI; what actually
    // gets saved is decided by diffing against `baseline` in buildSaveStructure.
    const [changedSections, setChangedSections] = useState(new Set());
    const [changedFormats, setChangedFormats] = useState(new Set());
    const [changedCategories, setChangedCategories] = useState(new Set());

    // The configuration exactly as it was last loaded from the backend. Every save is a diff against this.
    const [baseline, setBaseline] = useState(null);

    // Read inside the adoption effect without making it a dependency, so the effect fires only when a
    // fetch actually delivers a new configuration and never when the unsaved flag flips.
    const hasUnsavedChangesRef = useRef(false);
    const loadedReportName = useRef(null);

    useEffect(() => {
        hasUnsavedChangesRef.current = hasUnsavedChanges;
    }, [hasUnsavedChanges]);

    /**
     * Adopts a configuration delivered by the backend as the new baseline.
     *
     * An incoming configuration is only adopted when there are no unsaved changes to lose, or when the
     * user has opened a different report. Previously this ran on every prop identity change, so a
     * refetch arriving mid-edit silently discarded a newly created format while leaving the section
     * that referenced it in place.
     */
    useEffect(() => {
        const incoming = props.reportConfig;
        if (!incoming) {
            return;
        }

        const incomingName = normaliseName(incoming.Name);
        const isDifferentReport = loadedReportName.current !== null && loadedReportName.current !== incomingName;

        if (hasUnsavedChangesRef.current && !isDifferentReport) {
            return;
        }

        loadedReportName.current = incomingName;
        setReportConfig(incoming);
        setCustomFormats(incoming.CustomFormats || []);
        setBaseline(JSON.parse(JSON.stringify(incoming)));
        setChangedSections(new Set());
        setChangedFormats(new Set());
        setChangedCategories(new Set());
        setHasUnsavedChanges(false);
        setSaveError(null);
    }, [props.reportConfig]);

    // Helper function to notify parent of changes
    const notifyReportChange = () => {
        if (props.onReportChange) {
            const currentReportData = {
                ...reportConfig,
                CustomFormats: customFormats
            };
            props.onReportChange(currentReportData, true); // true indicates unsaved changes
        }
    };

    // Helper function to find header/footer by name (case-insensitive)
    const findHeaderFooterByName = (definitions, name) => {
        if (!name || !definitions) return null;
        return definitions.find(def => 
            def.Name && def.Name.toLowerCase() === name.toLowerCase()
        );
    };

    // Helper functions for change tracking
    const isCategoryChanged = (categoryType) => {
        return changedCategories.has(categoryType);
    };

    const isSectionChanged = (sectionName) => {
        if (!sectionName) return false;
        const name = typeof sectionName === 'string' ? sectionName : sectionName.Name;
        return changedSections.has(name);
    };

    // Helper to convert categoryType to unique identifier
    // Uses category Name for regular categories (e.g., "Main Sections", "Final Sections")
    // Uses Type for headers/footers (e.g., "HeaderSectionDefinitions", "FooterSectionDefinitions")
    const getCategoryUniqueIdentifier = (categoryType) => {
        if (!categoryType) return null;
        // Headers and footers use their Type as identifier
        if (categoryType === 'HeaderSectionDefinitions' || categoryType === 'FooterSectionDefinitions') {
            return categoryType;
        }
        // For regular categories, check if categoryType is already a category Name
        if (reportConfig.Categories) {
            const categoryByName = reportConfig.Categories.find(cat => cat.Name === categoryType);
            if (categoryByName) {
                // Already a Name, return as-is
                return categoryType;
            }
            // Try to find by Type and return its Name
            const categoryByType = reportConfig.Categories.find(cat => cat.Type === categoryType);
            if (categoryByType) {
                return categoryByType.Name;
            }
        }
        // Fallback to categoryType if not found
        return categoryType;
    };

    const markCategoryAsChanged = (categoryType) => {
        if (categoryType) {
            const uniqueId = getCategoryUniqueIdentifier(categoryType);
            setChangedCategories(prev => new Set(prev).add(uniqueId));
        }
    };

    const getCategoryType = (categoryType, categoryData) => {
        // Return the category identifier (Type for regular categories, categoryType for header/footer)
        if (categoryType === 'HeaderSectionDefinitions' || categoryType === 'FooterSectionDefinitions') {
            return categoryType;
        }
        return categoryData?.Type || categoryType;
    };

    const findCategoryForSection = (sectionName) => {
        // Find which category contains this section and return its unique identifier (Name for regular categories, Type for headers/footers)
        if (!sectionName || !reportConfig) return null;
        
        const name = typeof sectionName === 'string' ? sectionName : sectionName.Name;
        if (!name) return null;
        
        // Check header/footer sections
        if (reportConfig.HeaderSectionDefinitions) {
            const headerSection = reportConfig.HeaderSectionDefinitions.find(s => 
                (typeof s === 'string' ? s : s.Name)?.toLowerCase() === name.toLowerCase()
            );
            if (headerSection) return 'HeaderSectionDefinitions';
        }
        
        if (reportConfig.FooterSectionDefinitions) {
            const footerSection = reportConfig.FooterSectionDefinitions.find(s => 
                (typeof s === 'string' ? s : s.Name)?.toLowerCase() === name.toLowerCase()
            );
            if (footerSection) return 'FooterSectionDefinitions';
        }
        
        // Check regular categories - return category Name (unique identifier) instead of Type
        if (reportConfig.Categories) {
            for (const category of reportConfig.Categories) {
                if (category.Sections && category.Sections.some(s => {
                    const sectionNameStr = typeof s === 'string' ? s : s.Name;
                    return sectionNameStr?.toLowerCase() === name.toLowerCase();
                })) {
                    return category.Name; // Use Name as unique identifier
                }
            }
        }
        
        return null;
    };

    const {
        availableImages = []
    } = props;

    // Helper: resolve a section definition by name (case-insensitive) across all definition collections
    const findSectionDefinitionByName = (sectionName) => {
        if (!sectionName) return null;
        const target = String(sectionName).toLowerCase();
        const findIn = (arr) => (arr || []).find(def => String(def?.Name || '').toLowerCase() === target) || null;
        return (
            findIn(reportConfig.SectionDefinitions) ||
            findIn(reportConfig.HeaderSectionDefinitions) ||
            findIn(reportConfig.FooterSectionDefinitions)
        );
    };

    // Helper: resolve a data section definition by name (case-insensitive)
    const findDataSectionByName = (dataSectionName) => {
        if (!dataSectionName) return null;
        const target = String(dataSectionName).toLowerCase();
        const defs = reportConfig.DataSectionsDefinition || [];
        return defs.find(ds => String(ds?.Name || '').toLowerCase() === target) || null;
    };

    // Helper: resolve a data section for a given section definition using only DataSection field
    const resolveDataSectionForSection = (sectionDef) => {
        if (!sectionDef || !sectionDef.DataSection) return null;
        
        const dataSectionName = sectionDef.DataSection;
        const found = findDataSectionByName(dataSectionName);
        return found;
    };

    // Helper to resolve the correct reference block for a given category type
    /**
     * Resolves the `References` entry from `reportConfig` that corresponds to a given
     * category type.  Header and footer categories are mapped to the `Main` reference;
     * other categories are matched by name with a fallback lookup table.
     * @param {string} categoryType - The category identifier (e.g. `'Main'`, `'Organism'`,
     *   `'Final'`, `'HeaderSectionDefinitions'`, `'FooterSectionDefinitions'`).
     * @returns {object|null} The matching reference object (with `Name`, `AvailableDataSections`,
     *   `AvailableFields`), or `null` if no match is found.
     */
    const resolveReferenceForCategoryType = (categoryType) => {
        if (!reportConfig || !reportConfig.References || !categoryType) {
            return null;
        }
        
        // For header/footer definitions, use Main reference
        if (categoryType === 'HeaderSectionDefinitions' || categoryType === 'FooterSectionDefinitions') {
            return reportConfig.References.find(r => r.Name === 'Main') || null;
        }
        
        // For regular categories, try exact match first
        let ref = reportConfig.References.find(ref => ref.Name === categoryType);
        if (ref) {
            return ref;
        }
        
        // If no exact match, try the mapping
        const map = {
            'Main': 'Main',
            'Organism': 'Organism', 
            'Final': 'Main' // Final sections use Main reference
        };
        
        const mappedName = map[categoryType];
        if (mappedName) {
            return reportConfig.References.find(r => r.Name === mappedName);
        }
        
        return null;
    };

    /**
     * Returns the list of available fields for a given category type, resolved through
     * the category's reference entry.  Fields are sorted alphabetically by label.
     * @param {string} categoryType - The category identifier used to look up the reference
     *   via `resolveReferenceForCategoryType`.
     * @returns {{ name: string, label: string }[]} Sorted array of field descriptor objects,
     *   or an empty array if the category or its reference cannot be resolved.
     */
    const getAvailableFieldsForCategory = (categoryType) => {
        const ref = resolveReferenceForCategoryType(categoryType);
        if (!ref || !ref.AvailableFields) {
            return [];
        }
        const out = ref.AvailableFields.map(field => ({ name: field.Name, label: field.Label }))
            .sort((a, b) => a.label.localeCompare(b.label));
        return out;
    };

    /**
     * Returns the subset of `DataSectionsDefinition` entries that are permitted for a
     * specific category type, as declared in that category's reference `AvailableDataSections`
     * list.  Each name in the reference list is resolved to its full definition object.
     * @param {string} categoryType - The category identifier used to look up the reference
     *   via `resolveReferenceForCategoryType`.
     * @returns {object[]} Array of resolved data section definition objects (each with `Name`,
     *   `Title`, `Fields`, `Grids`), or an empty array if the category or its reference cannot
     *   be resolved.
     */
    const getAvailableDataSectionsForCategory = (categoryType) => {
        const ref = resolveReferenceForCategoryType(categoryType);
        if (!ref || !ref.AvailableDataSections) {
            return [];
        }
        if (!reportConfig.DataSectionsDefinition) {
            return [];
        }
        const resolved = ref.AvailableDataSections.map(sectionName => 
            reportConfig.DataSectionsDefinition.find(def => def.Name === sectionName)
        ).filter(Boolean);
        return resolved;
    };

    /**
     * Returns the full `DataSectionsDefinition` array from the current report configuration.
     * Used when no category filter is needed (e.g. the command-bar "Create New Section" flow).
     * @returns {object[]} Array of all data section definition objects (each with `Name`,
     *   `Title`, `Fields`, `Grids`), or an empty array if the configuration is not loaded.
     */
    const getAllAvailableDataSections = () => {
        if (!reportConfig.DataSectionsDefinition) return [];
        return reportConfig.DataSectionsDefinition;
    };

    /**
     * Returns the available fields from the `Main` reference entry in `reportConfig`.
     * These fields are used to populate the header and footer section editors.
     * Fields are sorted alphabetically by label.
     * @returns {{ name: string, label: string }[]} Sorted array of field descriptor objects,
     *   or an empty array if the `Main` reference is absent or has no fields.
     */
    const getMainReferenceFields = () => {
        if (!reportConfig.References) {
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

    const handleReportTitleChange = (value) => {
        setReportConfig(prev => ({ ...prev, Title: value }));
        setHasUnsavedChanges(true);
        notifyReportChange();
    };


    const handleEnabledChange = (checked) => {
        setReportConfig(prev => ({ ...prev, Enabled: checked }));
        setHasUnsavedChanges(true);
        notifyReportChange();
    };

    const handleIncludeAlertsChange = (checked) => {
        setReportConfig(prev => ({ ...prev, IncludeAlerts: checked }));
        setHasUnsavedChanges(true);
        notifyReportChange();
    };

    // Helper function to check if category uses header/footer selector
    const usesHeaderFooterSelector = (categoryType) => {
        return categoryType === 'HeaderSectionDefinitions' || categoryType === 'FooterSectionDefinitions';
    };

    /**
     * Handles the "Add section" action for a category panel button or the command-bar
     * "Create New Section" entry.
     * - For header/footer categories, opens the `HeaderFooterSelector` panel.
     * - For all other categories, pre-computes the permitted data sections and opens
     *   the `SectionCreator` panel.
     * @param {string} categoryType - The category identifier that the new section will
     *   belong to (e.g. `'Main'`, `'Organism'`, `'Final'`, `'HeaderSectionDefinitions'`,
     *   `'FooterSectionDefinitions'`, or `'NewSection'` for the command-bar flow).
     */
    const handleCreateNewSection = (categoryType) => {
        if (usesHeaderFooterSelector(categoryType)) {
            setHeaderFooterType(categoryType === 'HeaderSectionDefinitions' ? 'header' : 'footer');
            setShowHeaderFooterSelector(true);
        } else {
            // Precompute inputs for SectionCreator to ensure immediate availability
            const precomputedCategoryType = categoryType;
            const precomputedDataSections = categoryType === 'NewSection'
                ? getAllAvailableDataSections()
                : getAvailableDataSectionsForCategory(categoryType);

            setCreatorCategoryType(precomputedCategoryType);
            setCreatorAvailableDataSections(precomputedDataSections);

        setCurrentCategoryType(categoryType);
        setShowSectionCreator(true);
        }
    };

    const handleEditSection = (section, categoryType) => {
        // Normalize: if we received a section name (string), resolve to a full definition
        let resolvedSection = section;
        if (typeof section === 'string') {
            resolvedSection = findSectionDefinitionByName(section) || null;
        }

        setEditingSection(resolvedSection || section);
        setCurrentCategoryType(categoryType);
        
        // Decide designer strictly by Type and ensure only one panel is open
        const sectionType = (resolvedSection || section)?.Type;
        setShowSectionFieldEditor(false);
        setShowAbsoluteSectionDesigner(false);
        if (sectionType === 'Absolute') {
            setShowAbsoluteSectionDesigner(true);
        } else {
            setShowSectionFieldEditor(true);
        }
    };

    const handleDeleteSection = (section, categoryType) => {
        if (categoryType === 'HeaderSectionDefinitions' || categoryType === 'FooterSectionDefinitions') {
            // For header/footer definitions, show confirmation panel
            const isHeader = categoryType === 'HeaderSectionDefinitions';
            const sectionName = typeof section === 'string' ? section : section.Name;
            
            setDeleteConfirmationData({
                section,
                categoryType,
                sectionName,
                isHeader
            });
            setShowDeleteConfirmation(true);
        } else {
            // For other categories, use the normal delete logic
        setReportConfig(prev => {
            const newConfig = { ...prev };
                if (prev.Categories) {
                // For Categories, remove the section from the category's Sections array
                newConfig.Categories = prev.Categories.map(category => {
                    if (category.Type === categoryType) {
                        const filteredSections = category.Sections.filter(s => {
                            // Compare section names, not objects (case-insensitive)
                            const sectionName = typeof s === 'string' ? s : s.Name;
                            const deleteSectionName = typeof section === 'string' ? section : section.Name;
                            return sectionName.toLowerCase() !== deleteSectionName.toLowerCase();
                        });
                        return {
                            ...category,
                            Sections: filteredSections
                        };
                    }
                    return category;
                });
            }
                return newConfig;
            });
            markCategoryAsChanged(categoryType);
            setHasUnsavedChanges(true);
            notifyReportChange();
        }
    };

    const handleDeleteFromReport = () => {
        if (deleteConfirmationData) {
            const { isHeader, categoryType } = deleteConfirmationData;
            
            setReportConfig(prev => ({
                ...prev,
                [isHeader ? 'Header' : 'Footer']: null
            }));
            markCategoryAsChanged(categoryType);
            setHasUnsavedChanges(true);
            notifyReportChange();
        }
        
        setShowDeleteConfirmation(false);
        setDeleteConfirmationData(null);
    };

    const handleDeletePermanently = () => {
        if (deleteConfirmationData) {
            const { section, categoryType } = deleteConfirmationData;
            
            setReportConfig(prev => {
                const newConfig = { ...prev };
                newConfig[categoryType] = (prev[categoryType] || []).filter(s => 
                    typeof s === 'string' ? s !== section : s.Name !== section.Name
                );
            return newConfig;
        });
        markCategoryAsChanged(categoryType);
        setHasUnsavedChanges(true);
        notifyReportChange();
        }
        
        setShowDeleteConfirmation(false);
        setDeleteConfirmationData(null);
    };

    const handleCancelDelete = () => {
        setShowDeleteConfirmation(false);
        setDeleteConfirmationData(null);
    };

    /**
     * Clears every section reference from the report.
     *
     * Section, header and footer *definitions* are deliberately left in place: they are shared
     * configuration records that other reports may use, and removing them here would delete their
     * configs rows on the next save. Clearing the report means clearing what it references.
     */
    const handleClearReportDefinition = () => {
        setReportConfig(prev => {
            const newConfig = { ...prev };
            
            // Deselect the header and footer, keeping their definitions available for reuse
            newConfig.Header = null;
            newConfig.Footer = null;
            
            // Clear sections from all categories (preserve category structure)
            if (newConfig.Categories) {
                newConfig.Categories = newConfig.Categories.map(category => ({
                    ...category,
                    Sections: []
                }));
            }
            
            // Preserve DataSectionsDefinition - do not modify
            
            return newConfig;
        });
        
        setShowClearConfirmation(false);
        setHasUnsavedChanges(true);
        notifyReportChange();
    };

    const handleSelectExistingHeaderFooter = (sectionName) => {
        setReportConfig(prev => ({
            ...prev,
            [headerFooterType === 'header' ? 'Header' : 'Footer']: sectionName
        }));
        setHasUnsavedChanges(true);
        notifyReportChange();
        setShowHeaderFooterSelector(false);
        setHeaderFooterType(null);
    };

    const handleCreateNewHeaderFooter = (sectionData) => {
        setReportConfig(prev => {
            const newConfig = { ...prev };
            const categoryKey = headerFooterType === 'header' ? 'HeaderSectionDefinitions' : 'FooterSectionDefinitions';
            const configKey = headerFooterType === 'header' ? 'Header' : 'Footer';
            
            // Add the new section to the definitions
            newConfig[categoryKey] = [...(prev[categoryKey] || []), sectionData];
            
            // Set it as the current selection
            newConfig[configKey] = sectionData.Name;
            
            return newConfig;
        });
        setHasUnsavedChanges(true);
        if (sectionData?.Name) {
            setChangedSections(prev => new Set(prev).add(sectionData.Name));
        }
        notifyReportChange();
        
        // Close the HeaderFooterSelector and open the AbsoluteSectionDesigner for editing
        const categoryType = headerFooterType === 'header' ? 'HeaderSectionDefinitions' : 'FooterSectionDefinitions';
        setShowHeaderFooterSelector(false);
        setEditingSection(sectionData);
        setCurrentCategoryType(categoryType);
        setShowAbsoluteSectionDesigner(true);
        setHeaderFooterType(null);
    };

    const handleCreateSection = (newSection, categoryType) => {
        // Use the passed categoryType parameter instead of state
        const targetCategoryType = categoryType || currentCategoryType;
        
        setReportConfig(prev => {
            const newConfig = { ...prev };
            
            if (targetCategoryType === 'HeaderSectionDefinitions') {
                newConfig.HeaderSectionDefinitions = [...(prev.HeaderSectionDefinitions || []), newSection];
            } else if (targetCategoryType === 'FooterSectionDefinitions') {
                newConfig.FooterSectionDefinitions = [...(prev.FooterSectionDefinitions || []), newSection];
            } else if (prev.Categories) {
                // For Categories, add the section to the appropriate category
                newConfig.Categories = prev.Categories.map(category => {
                    if (category.Type === targetCategoryType) {
                        return {
                            ...category,
                            Sections: [...category.Sections, newSection.Name]
                        };
                    }
                    return category;
                });
                
                // Also add the section definition to SectionDefinitions if it doesn't exist
                if (!newConfig.SectionDefinitions) {
                    newConfig.SectionDefinitions = [];
                }
                if (!newConfig.SectionDefinitions.find(def => def.Name === newSection.Name)) {
                    newConfig.SectionDefinitions.push(newSection);
                }
            }
            
            return newConfig;
        });
        setShowSectionCreator(false);
        setCreatorCategoryType(null);
        setCreatorAvailableDataSections([]);
        setCurrentCategoryType(null);
        setHasUnsavedChanges(true);
        if (newSection?.Name) {
            setChangedSections(prev => new Set(prev).add(newSection.Name));
        }
        markCategoryAsChanged(targetCategoryType);
        notifyReportChange();
        
        // Open the SectionFieldEditor to configure the newly created section
        setEditingSection(newSection);
        setShowSectionFieldEditor(true);
    };

    /**
     * Opens the Absolute Section Designer in create mode with a blank form.
     * Clears any previously edited section so the designer is not prefilled.
     */
    const handleCreateAbsoluteSection = () => {
        setEditingSection(null);
        setShowSectionCreator(false);
        setCreatorCategoryType(null);
        setCreatorAvailableDataSections([]);
        setShowAbsoluteSectionDesigner(true);
    };

    const handleSaveAbsoluteSection = (sectionData) => {
        if (!currentCategoryType) return;

        if (editingSection) {
            // Update existing section
            setReportConfig(prev => ({
                ...prev,
                [currentCategoryType]: prev[currentCategoryType].map(s => 
                    s.Name === editingSection.Name ? sectionData : s
                )
            }));
        } else {
            // Create new section
            const newSection = {
                ...sectionData,
                Type: 'Absolute'
            };

            setReportConfig(prev => ({
                ...prev,
                [currentCategoryType]: [...(prev[currentCategoryType] || []), newSection]
            }));
        }

        // Track change
        setChangedSections(prev => new Set(prev).add(sectionData.Name));
        markCategoryAsChanged(currentCategoryType);
        setHasUnsavedChanges(true);

        setShowAbsoluteSectionDesigner(false);
        setCurrentCategoryType(null);
        setEditingSection(null);
        notifyReportChange();
    };

    const handleSaveNewSection = (sectionData, category) => {
        const newSection = {
            ...sectionData,
            Type: category === 'OrganismSections' ? 'Organism' : 'Specimen'
        };

        setReportConfig(prev => {
            const newConfig = { ...prev };
            
            if (category === 'HeaderSectionDefinitions') {
                newConfig.HeaderSectionDefinitions = [...(prev.HeaderSectionDefinitions || []), newSection];
            } else if (category === 'FooterSectionDefinitions') {
                newConfig.FooterSectionDefinitions = [...(prev.FooterSectionDefinitions || []), newSection];
            } else if (prev.Categories) {
                // For Categories, add the section to the appropriate category
                newConfig.Categories = prev.Categories.map(cat => {
                    if (cat.Type === category) {
                        return {
                            ...cat,
                            Sections: [...cat.Sections, newSection.Name]
                        };
                    }
                    return cat;
                });
                
                // Also add the section definition to SectionDefinitions if it doesn't exist
                if (!newConfig.SectionDefinitions) {
                    newConfig.SectionDefinitions = [];
                }
                if (!newConfig.SectionDefinitions.find(def => def.Name === newSection.Name)) {
                    newConfig.SectionDefinitions.push(newSection);
                }
            }
            
            return newConfig;
        });

        // Track change
        setChangedSections(prev => new Set(prev).add(sectionData.Name));
        markCategoryAsChanged(category);
        setHasUnsavedChanges(true);

        setShowSectionCreator(false);
        setCreatorCategoryType(null);
        setCreatorAvailableDataSections([]);
        setCurrentCategoryType(null);
        notifyReportChange();
    };

    const handleSaveSection = (updatedSection) => {
        setReportConfig(prev => {
            const newConfig = { ...prev };
            
            if (currentCategoryType === 'HeaderSectionDefinitions') {
                newConfig.HeaderSectionDefinitions = (prev.HeaderSectionDefinitions || []).map(s => 
                    typeof s === 'string' ? s : (s.Name === updatedSection.Name ? updatedSection : s)
                );
            } else if (currentCategoryType === 'FooterSectionDefinitions') {
                newConfig.FooterSectionDefinitions = (prev.FooterSectionDefinitions || []).map(s => 
                    typeof s === 'string' ? s : (s.Name === updatedSection.Name ? updatedSection : s)
                );
            } else if (prev.SectionDefinitions) {
                // Update the section definition in SectionDefinitions
                newConfig.SectionDefinitions = prev.SectionDefinitions.map(s => 
                    s.Name === updatedSection.Name ? updatedSection : s
                );
            }
            
            return newConfig;
        });
        setShowSectionFieldEditor(false);
        setEditingSection(null);
        // Track section change
        if (updatedSection?.Name) {
            setChangedSections(prev => new Set(prev).add(updatedSection.Name));
            // Find the category that contains this section and mark it as changed using its unique identifier
            const categoryIdentifier = findCategoryForSection(updatedSection.Name);
            if (categoryIdentifier) {
                markCategoryAsChanged(categoryIdentifier);
            } else if (currentCategoryType) {
                // Fallback: try to find category by currentCategoryType
                // For regular categories, we need to find the category Name, not Type
                if (reportConfig.Categories) {
                    const category = reportConfig.Categories.find(cat => cat.Type === currentCategoryType);
                    if (category) {
                        markCategoryAsChanged(category.Name);
                    } else {
                        // For headers/footers, use currentCategoryType directly
                        markCategoryAsChanged(currentCategoryType);
                    }
                } else {
                    markCategoryAsChanged(currentCategoryType);
                }
            }
        }
        setCurrentCategoryType(null);
        setHasUnsavedChanges(true);
        notifyReportChange();
    };

    // Drag and drop handlers for sections
    const handleSectionDragStart = (e, section, categoryType) => {
        setDraggedSection(section);
        setDraggedCategory(categoryType);
        e.dataTransfer.effectAllowed = 'move';
        e.dataTransfer.setData('text/html', e.target.outerHTML);
    };

    const handleSectionDragOver = (e) => {
        e.preventDefault();
        e.dataTransfer.dropEffect = 'move';
    };

    const handleSectionDrop = (e, targetSection, targetCategoryType) => {
        e.preventDefault();
        
        if (!draggedSection || !draggedCategory) return;
        
        // Don't allow dropping on the same section
        if (draggedSection === targetSection && draggedCategory === targetCategoryType) {
            setDraggedSection(null);
            setDraggedCategory(null);
            return;
        }

        setReportConfig(prev => {
            const newConfig = { ...prev };
            
            // Helper function to get sections for a category type
            const getSectionsForCategory = (categoryType) => {
                if (categoryType === 'HeaderSectionDefinitions') {
                    return newConfig.HeaderSectionDefinitions || [];
                } else if (categoryType === 'FooterSectionDefinitions') {
                    return newConfig.FooterSectionDefinitions || [];
                } else if (newConfig.Categories) {
                    const category = newConfig.Categories.find(cat => cat.Type === categoryType);
                    return category ? category.Sections : [];
                }
                return [];
            };
            
            // Helper function to set sections for a category type
            const setSectionsForCategory = (categoryType, sections) => {
                if (categoryType === 'HeaderSectionDefinitions') {
                    newConfig.HeaderSectionDefinitions = sections;
                } else if (categoryType === 'FooterSectionDefinitions') {
                    newConfig.FooterSectionDefinitions = sections;
                } else if (newConfig.Categories) {
                    newConfig.Categories = newConfig.Categories.map(cat => {
                        if (cat.Type === categoryType) {
                            return { ...cat, Sections: sections };
                        }
                        return cat;
                    });
                }
            };
            
            // Handle same category vs different category moves
            if (draggedCategory === targetCategoryType) {
                // Moving within the same category - reorder
                const sections = [...getSectionsForCategory(draggedCategory)];
                
                // Find the dragged section index
                const draggedIndex = sections.findIndex(s => 
                    (typeof s === 'string' ? s : s.Name).toLowerCase() === (typeof draggedSection === 'string' ? draggedSection : draggedSection.Name).toLowerCase()
                );
                
                // Find the target section index
                const targetIndex = sections.findIndex(s => 
                    (typeof s === 'string' ? s : s.Name).toLowerCase() === (typeof targetSection === 'string' ? targetSection : targetSection.Name).toLowerCase()
                );
                
                if (draggedIndex === -1 || targetIndex === -1) return prev;
                
                // Remove the dragged section
                const [removedSection] = sections.splice(draggedIndex, 1);
                
                // Adjust target index if we're moving down (since we already removed the item)
                const adjustedIndex = draggedIndex < targetIndex ? targetIndex - 1 : targetIndex;
                
                // Insert at the new position
                sections.splice(adjustedIndex, 0, removedSection);
                
                // Update the category
                setSectionsForCategory(draggedCategory, sections);
                // Mark category as changed for reordering
                markCategoryAsChanged(draggedCategory);
            } else {
                // Moving between different categories
                const sourceSections = [...getSectionsForCategory(draggedCategory)];
                const targetSections = [...getSectionsForCategory(targetCategoryType)];
                
                // Find the dragged section index in source
                const draggedIndex = sourceSections.findIndex(s => 
                    (typeof s === 'string' ? s : s.Name).toLowerCase() === (typeof draggedSection === 'string' ? draggedSection : draggedSection.Name).toLowerCase()
                );
                
                if (draggedIndex === -1) return prev;
                
                // Remove the dragged section from source
                const [removedSection] = sourceSections.splice(draggedIndex, 1);
                
                // Find the target section index
                const targetIndex = targetSections.findIndex(s => 
                    (typeof s === 'string' ? s : s.Name).toLowerCase() === (typeof targetSection === 'string' ? targetSection : targetSection.Name).toLowerCase()
                );
                
                // Insert the dragged section at the target position
                if (targetIndex === -1) {
                    targetSections.push(removedSection);
                } else {
                    targetSections.splice(targetIndex, 0, removedSection);
                }
                
                // Update both categories
                setSectionsForCategory(draggedCategory, sourceSections);
                setSectionsForCategory(targetCategoryType, targetSections);
            }
            
            return newConfig;
        });
        
        // Mark both source and target categories as changed
        markCategoryAsChanged(draggedCategory);
        if (draggedCategory !== targetCategoryType) {
            markCategoryAsChanged(targetCategoryType);
        }
        
        setDraggedSection(null);
        setDraggedCategory(null);
        setHasUnsavedChanges(true);
        notifyReportChange();
    };

    const handleSectionDragEnd = () => {
        setDraggedSection(null);
        setDraggedCategory(null);
    };

    // Drag and drop handlers for category reordering
    const handleCategoryDragStart = (e, categoryKey) => {
        // Stop propagation to prevent section drag handlers from interfering
        e.stopPropagation();
        
        // Don't allow dragging Headers or Footers
        if (categoryKey === 'HeaderSectionDefinitions' || categoryKey === 'FooterSectionDefinitions') {
            e.preventDefault();
            return false;
        }
        
        setDraggedCategoryOrder(categoryKey);
        draggedCategoryRef.current = categoryKey; // Store in ref as backup
        e.dataTransfer.effectAllowed = 'move';
        e.dataTransfer.setData('text/plain', categoryKey);
        
        // Add visual feedback
        e.target.style.opacity = '0.5';
        
        return true;
    };

    const handleCategoryDragOver = (e) => {
        e.preventDefault();
        e.stopPropagation();
        e.dataTransfer.dropEffect = 'move';
        return false;
    };

    const handleCategoryDragEnter = (e) => {
        e.preventDefault();
        e.stopPropagation();
        e.target.classList.add('drag-over');
    };

    const handleCategoryDragLeave = (e) => {
        e.preventDefault();
        e.stopPropagation();
        e.target.classList.remove('drag-over');
    };

    const handleCategoryDrop = (e, targetCategoryKey) => {
        e.preventDefault();
        e.stopPropagation();
        
        // Reset visual feedback
        e.target.classList.remove('drag-over');
        
        // Don't allow dropping on Headers or Footers
        if (targetCategoryKey === 'HeaderSectionDefinitions' || targetCategoryKey === 'FooterSectionDefinitions') {
            return false;
        }
        
        // Use ref as fallback if state is null
        const draggedCategory = draggedCategoryOrder || draggedCategoryRef.current;
        
        if (!draggedCategory) {
            return false;
        }
        
        if (draggedCategory === targetCategoryKey) {
            return false;
        }

        // Get current category order
        const currentOrder = getOrderedCategories().map(cat => cat.key);
        const sourceIndex = currentOrder.indexOf(draggedCategory);
        const targetIndex = currentOrder.indexOf(targetCategoryKey);
        
        if (sourceIndex === -1 || targetIndex === -1) {
            return false;
        }
        
        // Create new order
        const newOrder = [...currentOrder];
        const [movedCategory] = newOrder.splice(sourceIndex, 1);
        newOrder.splice(targetIndex, 0, movedCategory);
        
        // Store the new order in a state variable for rendering
        setReportConfig(prev => {
            const newConfig = { ...prev };
            // Store the category order in the config
            newConfig._categoryOrder = newOrder;
            return newConfig;
        });
        
        setDraggedCategoryOrder(null);
        draggedCategoryRef.current = null; // Clear ref too
        setHasUnsavedChanges(true);
        notifyReportChange();
        
        return true;
    };

    const handleCategoryDragEnd = (e) => {
        e.stopPropagation();
        // Reset visual feedback
        e.target.style.opacity = '';
        // Remove drag-over class from all elements
        document.querySelectorAll('.drag-over').forEach(el => el.classList.remove('drag-over'));
        
        // Only reset if we haven't already processed a drop
        // The drop handler will reset the state if successful
        setTimeout(() => {
            if (draggedCategoryOrder || draggedCategoryRef.current) {
                setDraggedCategoryOrder(null);
                draggedCategoryRef.current = null;
            }
        }, 100);
    };


    /**
     * Returns the formats offered in the section format dropdown.
     *
     * `customFormats` is the single source of truth for formats: it is seeded from the loaded
     * configuration and is the only collection `handleUpdateFormats` writes to. Splitting formats
     * across `customFormats` and `reportConfig.CustomFormats` is what previously let an edited format
     * become invisible to the save.
     *
     * Memoised because `SectionFieldEditor` uses the identity of this array to decide when to reset
     * its own editing state; a fresh array every render wiped in-progress edits.
     * @returns {object[]} Formats sorted by name, excluding the special 'Absolute' section type.
     */
    const availableFormats = useMemo(() => {
        const uniqueFormats = (customFormats || []).reduce((acc, current) => {
            if (!current || !current.Name) return acc;
            const existingIndex = acc.findIndex(format => normaliseName(format.Name) === normaliseName(current.Name));
            if (existingIndex >= 0) {
                acc[existingIndex] = current;
            } else {
                acc.push(current);
            }
            return acc;
        }, []);

        return uniqueFormats
            .filter(format => normaliseName(format.Name) !== 'absolute')
            .sort((a, b) => a.Name.localeCompare(b.Name));
    }, [customFormats]);

    const getAllFormats = () => availableFormats;

    // Helper to get format by name
    const getFormatByName = (formatName) => findFormatByName(getAllFormats(), formatName);

    /**
     * Checks a section against its format and the data section it reads, so a test that captures
     * more grids than the section's format can place is reported against the section rather than
     * discovered when the report is printed.
     * @param {object} section - The section definition to check.
     * @returns {Array<{type: string, message: string}>} The issues found, empty when consistent.
     */
    const getSectionIssues = (section) => validateSection({
        section,
        format: getFormatByName(section?.Format),
        dataSection: resolveDataSectionForSection(section)
    });

    /**
     * Applies a format created or edited in the FormatEditor.
     *
     * Everything lands in `customFormats`, which is the collection the save diffs and the dropdown
     * reads, so a format can never be written somewhere the save does not look.
     * @param {object} format - The format as edited.
     * @param {boolean} isNew - True when the format was just created.
     */
    const handleUpdateFormats = (format, isNew = false) => {
        if (!format || !format.Name) {
            return;
        }

        setCustomFormats(prev => {
            const existingIndex = (prev || []).findIndex(cf => normaliseName(cf.Name) === normaliseName(format.Name));
            if (!isNew && existingIndex >= 0) {
                const next = [...prev];
                // Keep the configs identity of the record being edited so the backend updates it by id.
                next[existingIndex] = { ...format, ConfigId: format.ConfigId ?? prev[existingIndex].ConfigId ?? null };
                return next;
            }
            return [...(prev || []), { ...format, ConfigId: isNew ? null : (format.ConfigId ?? null) }];
        });

        setChangedFormats(prev => new Set(prev).add(format.Name));
        setHasUnsavedChanges(true);
        notifyReportChange();

        // DO NOT update section Format property - it should only change via dropdown selection
    };

    /**
     * Builds the payload for a save by diffing the current designer state against the baseline
     * captured when the report was loaded.
     *
     * The change set is derived, not accumulated, so no React state reset can cause a change to be
     * dropped from the payload. `Enabled` and `View` round-trip, category membership is sent as the
     * section sources, and anything removed since the load is sent as an explicit deletion.
     * @returns {object} The SaveReportDesignerConfigModel payload.
     */
    const buildSaveStructure = () => {
        const changeSet = buildChangeSet(baseline, reportConfig, customFormats);

        return {
            Name: reportConfig.Name,
            Title: reportConfig.Title,
            View: reportConfig.View,
            Header: reportConfig.Header,
            Footer: reportConfig.Footer,
            IncludeAlerts: reportConfig.IncludeAlerts,
            Enabled: reportConfig.Enabled,
            Categories: reportConfig.Categories?.map((cat) => ({
                Name: cat.Name,
                Type: cat.Type,
                SourceName: cat.SourceName,
                Source: cat.Source,
                Sections: cat.Sections || [],
            })) || [],
            // Send back the source the report was loaded with. Deriving it from the category type would
            // rewrite FinalSections from Main to Final on every save.
            SectionSources: reportConfig.Categories?.filter(cat => cat.SourceName).map((cat) => ({
                Name: cat.SourceName,
                Source: cat.Source || cat.Type,
            })) || [],
            ChangedSectionDefinitions: changeSet.changedSections,
            ChangedCustomFormats: changeSet.changedFormats,
            DeletedSections: changeSet.deletedSections,
            DeletedFormats: changeSet.deletedFormats,
        };
    };

    /**
     * Reconciles local state with what the backend actually stored.
     *
     * A new format or section whose name collides with an existing configuration record is renamed by
     * the backend. Applying the returned names here, before the container refetches, stops a section
     * being left pointing at a format name that does not exist.
     * @param {object} saveResult - The ReportDesignerSaveResultModel returned by the save endpoint.
     */
    const applySaveResult = (saveResult) => {
        if (!saveResult) {
            return;
        }

        const formatRenames = new Map();
        const formatIds = new Map();

        (saveResult.Formats || []).forEach((entry) => {
            if (!entry?.SavedName || entry.State === 'Deleted') return;
            const requested = normaliseName(entry.RequestedName);
            const saved = normaliseName(entry.SavedName);
            formatIds.set(saved, entry.ConfigId);
            if (requested && requested !== saved) {
                formatRenames.set(requested, entry.SavedName);
            }
        });

        const sectionIds = new Map();
        const sectionTypes = new Map();
        const sectionRenames = new Map();

        (saveResult.Sections || []).forEach((entry) => {
            if (!entry?.SavedName || entry.State === 'Deleted') return;
            const requested = normaliseName(entry.RequestedName);
            const saved = normaliseName(entry.SavedName);
            sectionIds.set(saved, entry.ConfigId);
            sectionTypes.set(saved, entry.ConfigTypeId);
            if (requested && requested !== saved) {
                sectionRenames.set(requested, entry.SavedName);
            }
        });

        const resolveName = (renames, name) => renames.get(normaliseName(name)) || name;

        setCustomFormats(prev => (prev || []).map((format) => {
            const savedName = resolveName(formatRenames, format.Name);
            return { ...format, Name: savedName, ConfigId: formatIds.get(normaliseName(savedName)) ?? format.ConfigId ?? null };
        }));

        setReportConfig(prev => {
            const remapSections = (sections) => (sections || []).map((section) => {
                if (typeof section === 'string') {
                    return resolveName(sectionRenames, section);
                }
                const savedName = resolveName(sectionRenames, section.Name);
                return {
                    ...section,
                    Name: savedName,
                    Format: resolveName(formatRenames, section.Format),
                    ConfigId: sectionIds.get(normaliseName(savedName)) ?? section.ConfigId ?? null,
                    // A section created by this save has no type until the backend reports the one it
                    // was written to. Without it, saving again in the same session would fall back to
                    // inferring the type and could write the second save somewhere else.
                    ConfigTypeId: sectionTypes.get(normaliseName(savedName)) ?? section.ConfigTypeId ?? null,
                };
            });

            return {
                ...prev,
                SectionDefinitions: remapSections(prev.SectionDefinitions),
                HeaderSectionDefinitions: remapSections(prev.HeaderSectionDefinitions),
                FooterSectionDefinitions: remapSections(prev.FooterSectionDefinitions),
                Header: resolveName(sectionRenames, prev.Header),
                Footer: resolveName(sectionRenames, prev.Footer),
                Categories: (prev.Categories || []).map(category => ({
                    ...category,
                    Sections: remapSections(category.Sections),
                })),
            };
        });
    };

    /**
     * Turns whatever a failed save threw into a sentence worth showing.
     *
     * The save can fail before the request is built, while it is in flight, or with a response the
     * backend rejected, and each of those arrives here in a different shape.
     * @param {*} error - The value the save rejected with or threw.
     * @returns {string} Text for the error banner.
     */
    const describeSaveError = (error) => {
        if (!error) {
            return 'The report could not be saved.';
        }
        if (typeof error === 'string') {
            return error;
        }
        if (error.data) {
            return typeof error.data === 'string' ? error.data : `The report could not be saved (${error.status}).`;
        }
        return error.message || 'The report could not be saved.';
    };

    /**
     * Posts a save payload and reconciles the designer with the backend result.
     * @param {object} saveData - The SaveReportDesignerConfigModel payload.
     * @returns {Promise<void>}
     */
    const executeSave = async (saveData) => {
        if (props.onSave) {
            applySaveResult(await props.onSave(saveData));
        }
        setHasUnsavedChanges(false);
        setChangedSections(new Set());
        setChangedFormats(new Set());
        setChangedCategories(new Set());
    };

    /**
     * Opens the header/footer save-scope panel when shared definitions changed, otherwise saves immediately.
     * @param {object} saveData - The SaveReportDesignerConfigModel payload.
     * @param {'report'|'preview'} mode - Which save action initiated the request.
     * @returns {Promise<boolean>} True when the save completed; false when waiting for scope choice.
     */
    const saveOrPromptForHeaderFooterScope = async (saveData, mode) => {
        if (needsHeaderFooterSaveScopePrompt(saveData.ChangedSectionDefinitions, reportConfig)) {
            setPendingSaveData(saveData);
            setPendingSaveMode(mode);
            setShowHeaderFooterSaveScope(true);
            return false;
        }

        await executeSave(saveData);
        return true;
    };

    /**
     * Completes a deferred save after the user chooses a header/footer save scope.
     * @param {string} saveScope - One of the HEADER_FOOTER_SAVE_SCOPE values.
     * @returns {Promise<void>}
     */
    const handleConfirmHeaderFooterSaveScope = async (saveScope) => {
        if (!pendingSaveData) {
            setShowHeaderFooterSaveScope(false);
            return;
        }

        const mode = pendingSaveMode;
        const saveData = applyHeaderFooterSaveScope(pendingSaveData, saveScope, reportConfig);

        setShowHeaderFooterSaveScope(false);
        setPendingSaveData(null);
        setPendingSaveMode(null);

        try {
            if (mode === 'preview') {
                setIsSavingForPreview(true);
            } else {
                setIsSaving(true);
            }
            setSaveError(null);

            await executeSave(saveData);

            if (mode === 'preview') {
                setPreviewData(null);
                setIsPreviewOpen(true);
            }
        } catch (error) {
            setSaveError(describeSaveError(error));
            if (mode === 'preview') {
                alert('Save failed. Preview cancelled.');
            } else {
                console.error('Failed to save report:', error);
            }
        } finally {
            setIsSaving(false);
            setIsSavingForPreview(false);
        }
    };

    const handleCancelHeaderFooterSaveScope = () => {
        setShowHeaderFooterSaveScope(false);
        setPendingSaveData(null);
        setPendingSaveMode(null);
    };

    const handleSaveReport = async () => {
        if (!hasUnsavedChanges) {
            alert('No changes to save');
            return;
        }

        try {
            setIsSaving(true);
            setSaveError(null);
            const saveData = buildSaveStructure();
            await saveOrPromptForHeaderFooterScope(saveData, 'report');
        } catch (error) {
            setSaveError(describeSaveError(error));
            console.error('Failed to save report:', error);
        } finally {
            setIsSaving(false);
        }
    };

    const handleSaveAndPreview = async () => {
        if (!reportConfig?.Name) {
            alert('Please provide a report name before preview.');
            return;
        }
        try {
            setIsSavingForPreview(true);
            setSaveError(null);
            const saveData = buildSaveStructure();
            const completed = await saveOrPromptForHeaderFooterScope(saveData, 'preview');
            if (completed) {
                setPreviewData(null);
                setIsPreviewOpen(true);
            }
        } catch (e) {
            setSaveError(describeSaveError(e));
            alert('Save failed. Preview cancelled.');
        } finally {
            setIsSavingForPreview(false);
        }
    };

    const getOrderedCategories = () => {
        // If we have a stored order, use it
        if (reportConfig._categoryOrder) {
            const categories = [];
            reportConfig._categoryOrder.forEach(key => {
                if (key === 'HeaderSectionDefinitions' && reportConfig.HeaderSectionDefinitions) {
                    categories.push({ key: 'HeaderSectionDefinitions', name: 'Headers' });
                } else if (key === 'FooterSectionDefinitions' && reportConfig.FooterSectionDefinitions) {
                    categories.push({ key: 'FooterSectionDefinitions', name: 'Footers' });
                } else if (reportConfig.Categories) {
                    const category = reportConfig.Categories.find(cat => cat.Type === key);
                    if (category) {
                        categories.push({ key: category.Type, name: category.Name, category: category });
                    }
                }
            });
            return categories;
        }
        
        // Default order if no custom order is stored
        const categories = [];
        
        // Always start with Headers
        if (reportConfig.HeaderSectionDefinitions) {
            categories.push({ key: 'HeaderSectionDefinitions', name: 'Headers' });
        }
        
        // Add Categories from the new structure
        if (reportConfig.Categories) {
            reportConfig.Categories.forEach(category => {
                // Use category.Name as the unique identifier for change tracking
                // This ensures each category (e.g., "Main Sections", "Final Sections") is tracked separately
                categories.push({ key: category.Type, name: category.Name, category: category, uniqueKey: category.Name });
            });
        }
        
        // Always end with Footers
        if (reportConfig.FooterSectionDefinitions) {
            categories.push({ key: 'FooterSectionDefinitions', name: 'Footers' });
        }
        
        return categories;
    };

    const renderCategorySection = (categoryType, categoryName, categoryData = null, uniqueKey = null) => {
        let sections = [];
        
        // Use uniqueKey (category Name) for change tracking if available, otherwise fall back to categoryType
        const categoryIdentifier = uniqueKey || categoryType;
        
        // Handle different category types
        if (categoryType === 'HeaderSectionDefinitions') {
            // For headers, show only the currently selected header (if any)
            // Use case-insensitive comparison since database stores config names in lowercase
            if (reportConfig.Header) {
                const selectedHeader = findHeaderFooterByName(reportConfig.HeaderSectionDefinitions, reportConfig.Header);
                sections = selectedHeader ? [selectedHeader] : [];
            } else {
                sections = [];
            }
        } else if (categoryType === 'FooterSectionDefinitions') {
            // For footers, show only the currently selected footer (if any)
            // Use case-insensitive comparison since database stores config names in lowercase
            if (reportConfig.Footer) {
                const selectedFooter = findHeaderFooterByName(reportConfig.FooterSectionDefinitions, reportConfig.Footer);
                sections = selectedFooter ? [selectedFooter] : [];
            } else {
                sections = [];
            }
        } else if (categoryData && categoryData.Sections) {
            // For Categories, get the sections from the category data
            sections = categoryData.Sections.map(sectionName => {
                // Find the section definition (case-insensitive across all definitions)
                const sectionDef = findSectionDefinitionByName(sectionName);
                return sectionDef || sectionName; // Return definition if found, otherwise just the name
            });
        }
        
        const isDraggable = categoryType !== 'HeaderSectionDefinitions' && categoryType !== 'FooterSectionDefinitions';
        const categoryChanged = isCategoryChanged(categoryIdentifier);
        
        return (
            <div 
                key={`${categoryType}-${categoryName}`} 
                className={`category-section ${isDraggable ? 'draggable-category' : 'fixed-category'} ${categoryChanged ? 'changed' : ''}`}
                draggable={isDraggable}
                onDragStart={(e) => isDraggable && handleCategoryDragStart(e, categoryType)}
                onDragEnd={isDraggable ? handleCategoryDragEnd : undefined}
                onDragOver={isDraggable ? handleCategoryDragOver : undefined}
                onDragEnter={isDraggable ? handleCategoryDragEnter : undefined}
                onDragLeave={isDraggable ? handleCategoryDragLeave : undefined}
                onDrop={(e) => isDraggable && handleCategoryDrop(e, categoryType)}
            >
                <div 
                    className={`category-header ${categoryChanged ? 'changed' : ''}`}
                >
                    <div className="category-title">
                        {isDraggable && <div className="drag-handle">⋮⋮</div>}
                        <h3>{categoryName}</h3>
                    </div>
                    {((categoryType !== 'HeaderSectionDefinitions' || !reportConfig.Header) && 
                      (categoryType !== 'FooterSectionDefinitions' || !reportConfig.Footer)) && (
                    <PrimaryButton 
                        id={`reportdesigner-category-${categoryType.toLowerCase()}-add`}
                        text={`Add ${categoryName.slice(0, -1)}`}
                        onClick={() => handleCreateNewSection(categoryType)}
                        iconProps={{ iconName: 'Add' }}
                    />
                    )}
                </div>
                
                <div className="sections-list">
                    {sections.length > 0 ? (
                        sections.map((section, index) => {
                            // Handle both old format (strings) and new format (objects)
                            if (typeof section === 'string') {
                                const sectionChanged = isSectionChanged(section);
                                return (
                                    <div 
                                        key={section} 
                                        id={`reportdesigner-section-${normaliseName(section)}`}
                                        className={`section-item draggable ${sectionChanged ? 'changed' : ''}`}
                                        draggable
                                        onDragStart={(e) => handleSectionDragStart(e, section, categoryType)}
                                        onDragOver={handleSectionDragOver}
                                        onDrop={(e) => handleSectionDrop(e, section, categoryType)}
                                        onDragEnd={handleSectionDragEnd}
                                    >
                                        <div className="section-info">
                                            <div className="drag-handle">⋮⋮</div>
                                            <h4>{section}</h4>
                                            <span className="section-description">
                                                Section reference
                                            </span>
                                        </div>
                                        <div className="section-actions">
                                            <IconButton
                                                id={`reportdesigner-section-${normaliseName(section)}-edit`}
                                                iconProps={{ iconName: 'Edit' }}
                                                onClick={() => handleEditSection(section, categoryType)}
                                            />
                                            <IconButton
                                                id={`reportdesigner-section-${normaliseName(section)}-delete`}
                                                iconProps={{ iconName: 'Delete' }}
                                                onClick={() => handleDeleteSection(section, categoryType)}
                                            />
                                        </div>
                                    </div>
                                );
                            } else {
                                // New format - section object
                                const sectionChanged = isSectionChanged(section);
                                const formatIssues = getSectionIssues(section);
                                return (
                                    <div 
                                        key={section.Name || index} 
                                        id={`reportdesigner-section-${normaliseName(section.Name)}`}
                                        className={`section-item draggable ${sectionChanged ? 'changed' : ''}`}
                                        draggable
                                        onDragStart={(e) => handleSectionDragStart(e, section, categoryType)}
                                        onDragOver={handleSectionDragOver}
                                        onDrop={(e) => handleSectionDrop(e, section, categoryType)}
                                        onDragEnd={handleSectionDragEnd}
                                    >
                                        <div className="section-info">
                                            <div className="drag-handle">⋮⋮</div>
                                            <h4 id={`reportdesigner-section-${normaliseName(section.Name)}-title`}>
                                                {getSectionDisplayTitle(section, props.language)}
                                            </h4>
                                            <span
                                                className="section-description"
                                                id={`reportdesigner-section-${normaliseName(section.Name)}-meta`}
                                            >
                                                {getSectionMetaLabel(section)}
                                            </span>
                                            {/* Add format inconsistency warnings */}
                                            {formatIssues.length > 0 && (
                                                <div
                                                    className="format-issues"
                                                    id={`reportdesigner-section-${normaliseName(section.Name)}-issues`}
                                                >
                                                    {formatIssues.map((issue) => (
                                                        <span
                                                            key={issue.type}
                                                            id={`reportdesigner-section-${normaliseName(section.Name)}-issue-${issue.type}`}
                                                            className="format-issue-text"
                                                        >
                                                            ⚠ {issue.message}
                                                        </span>
                                                    ))}
                                                </div>
                                            )}
                                        </div>
                                        <div className="section-actions">
                                            <IconButton
                                                id={`reportdesigner-section-${normaliseName(section.Name)}-edit`}
                                                iconProps={{ iconName: 'Edit' }}
                                                onClick={() => handleEditSection(section, categoryType)}
                                            />
                                            <IconButton
                                                id={`reportdesigner-section-${normaliseName(section.Name)}-delete`}
                                                iconProps={{ iconName: 'Delete' }}
                                                onClick={() => handleDeleteSection(section, categoryType)}
                                            />
                                        </div>
                                    </div>
                                );
                            }
                        })
                    ) : (
                        <div className="empty-category">
                            <p>No {categoryName.toLowerCase()} added yet.</p>
                        </div>
                    )}
                </div>
            </div>
        );
    };

    const commandBarItems = [
        {
            key: 'createSection',
            id: 'reportdesigner-createsection',
            text: 'Create New Section',
            iconProps: { iconName: 'Add' },
            onClick: () => {
                setCurrentCategoryType('NewSection');
                // Precompute union of all data sections for the New Section wizard
                setCreatorCategoryType('NewSection');
                setCreatorAvailableDataSections(getAllAvailableDataSections());
                setShowSectionCreator(true);
            }
        },
        {
            key: 'clearReportDefinition',
            id: 'reportdesigner-clear',
            text: TranslateTag('@RepClrA@', props.language) || 'Clear Report Definition',
            iconProps: { iconName: 'Delete' },
            onClick: () => {
                setShowClearConfirmation(true);
            }
        },
        {
            key: 'save',
            id: 'reportdesigner-save',
            text: 'Save Report',
            iconProps: { iconName: 'Save' },
            onClick: handleSaveReport,
            disabled: !hasUnsavedChanges || isSaving
        },
        {
            key: 'preview',
            id: 'reportdesigner-savepreview',
            text: 'Save and Preview Report',
            iconProps: { iconName: 'View' },
            onClick: handleSaveAndPreview,
            disabled: isSavingForPreview || isSaving
        }
    ];

    return (
        <div className="report-designer" id="reportdesigner">
            <div className="designer-header">
                <CommandBar items={commandBarItems} />
            </div>

            {hasUnsavedChanges && (
                <MessageBar id="reportdesigner-unsaved-banner" messageBarType={MessageBarType.warning}>
                    You have unsaved changes. Don't forget to save your report.
                </MessageBar>
            )}

            {saveError && (
                <MessageBar
                    id="reportdesigner-save-error"
                    messageBarType={MessageBarType.error}
                    onDismiss={() => setSaveError(null)}
                >
                    {saveError}
                </MessageBar>
            )}

            <div className="designer-content" style={{ pointerEvents: isSaving ? 'none' : 'auto' }}>
                <div id="reportdesigner-basicinfo" className="report-basic-info">
                    <h3>Basic Information</h3>
                    <div className="form-row">
                        <TextField
                            id="reportdesigner-title"
                            label="Report Title"
                            value={reportConfig.Title || ''}
                            onChange={(e, value) => handleReportTitleChange(value)}
                            placeholder="Enter report title"
                            className="form-field"
                        />
                    </div>
                    <div className="form-row">
                        <Toggle
                            id="reportdesigner-enabled"
                            label="Enabled"
                            checked={reportConfig.Enabled || false}
                            onChange={(e, checked) => handleEnabledChange(checked)}
                        />
                        <Toggle
                            id="reportdesigner-includealerts"
                            label="Include Alerts"
                            checked={reportConfig.IncludeAlerts || false}
                            onChange={(e, checked) => handleIncludeAlertsChange(checked)}
                        />
                    </div>
                </div>

                <div className="report-sections">
                    <h3>Report Sections</h3>
                    {getOrderedCategories().map(category => 
                        renderCategorySection(category.key, category.name, category.category, category.uniqueKey)
                    )}
                </div>
            </div>

            <SectionCreator
                isOpen={showSectionCreator}
                onDismiss={() => {
                    setShowSectionCreator(false);
                    setCreatorCategoryType(null);
                    setCreatorAvailableDataSections([]);
                    setCurrentCategoryType(null);
                }}
                availableDataSections={creatorAvailableDataSections}
                categoryType={creatorCategoryType}
                onCreateSection={handleCreateSection}
                onCreateAbsoluteSection={handleCreateAbsoluteSection}
                onSaveNewSection={handleSaveNewSection}
            />

            <SectionFieldEditor
                isOpen={showSectionFieldEditor}
                onDismiss={() => {
                    setShowSectionFieldEditor(false);
                    setEditingSection(null);
                    setCurrentCategoryType(null);
                }}
                sectionDefinition={editingSection}
                dataSection={editingSection ? resolveDataSectionForSection(editingSection) : null}
                availableFormats={getAllFormats()}
                onSave={handleSaveSection}
                onDelete={(section) => handleDeleteSection(section, currentCategoryType)}
                onUpdateFormats={handleUpdateFormats}
                language={props.language}
            />

            <AbsoluteSectionDesigner
                isOpen={showAbsoluteSectionDesigner}
                onDismiss={() => {
                    setShowAbsoluteSectionDesigner(false);
                    setEditingSection(null);
                    setCurrentCategoryType(null);
                }}
                sectionDefinition={editingSection}
                categoryType={currentCategoryType}
                availableFields={(() => {
                    const isHeaderFooter = currentCategoryType === 'HeaderSectionDefinitions' || currentCategoryType === 'FooterSectionDefinitions';
                    const fields = isHeaderFooter 
                        ? getMainReferenceFields() 
                        : getAvailableFieldsForCategory(currentCategoryType);
                    return fields;
                })()}
                availableOrganismFields={currentCategoryType === 'HeaderSectionDefinitions' || currentCategoryType === 'FooterSectionDefinitions' 
                    ? getMainReferenceFields() 
                    : getAvailableFieldsForCategory('Organism')}
                availableImages={availableImages}
                onSave={handleSaveAbsoluteSection}
            />

            {/* Header/Footer Save Scope Panel */}
            <Panel
                isOpen={showHeaderFooterSaveScope}
                onDismiss={handleCancelHeaderFooterSaveScope}
                type={PanelType.medium}
                headerText={TranslateTag('@RepSavA@', props.language)}
                closeButtonAriaLabel={TranslateTag('@GenClo@', props.language)}
            >
                <div id="reportdesigner-headerfooter-save-scope" style={{ padding: '20px' }}>
                    <p>
                        {TranslateTag('@RepSavB@', props.language)}
                    </p>
                    <ul style={{ marginTop: '10px', paddingLeft: '20px' }}>
                        {(pendingSaveData
                            ? getSharedHeaderFooterSectionsNeedingPrompt(
                                pendingSaveData.ChangedSectionDefinitions,
                                reportConfig
                            )
                            : []
                        ).map((section) => (
                            <li key={`${section.Scope}-${section.Name}`}>
                                {`${TranslateTag(section.Scope === 'Footer' ? '@RepSavD@' : '@RepSavC@', props.language)}: `}
                                <strong>{section.Name}</strong>
                            </li>
                        ))}
                    </ul>

                    <div style={{ marginTop: '20px' }}>
                        <div style={{ marginBottom: '15px', padding: '15px', border: '1px solid #e1dfdd', borderRadius: '4px' }}>
                            <h4 style={{ margin: '0 0 10px 0' }}>{TranslateTag('@RepSavE@', props.language)}</h4>
                            <p style={{ margin: '0 0 10px 0', fontSize: '14px' }}>
                                {TranslateTag('@RepSavF@', props.language)}
                            </p>
                            <DefaultButton
                                id="reportdesigner-headerfooter-save-current-report-only"
                                text={TranslateTag('@RepSavG@', props.language)}
                                onClick={() => handleConfirmHeaderFooterSaveScope(HEADER_FOOTER_SAVE_SCOPE.CURRENT_REPORT_ONLY)}
                            />
                        </div>

                        <div style={{ marginBottom: '15px', padding: '15px', border: '1px solid #e1dfdd', borderRadius: '4px' }}>
                            <h4 style={{ margin: '0 0 10px 0' }}>{TranslateTag('@RepSavH@', props.language)}</h4>
                            <p style={{ margin: '0 0 10px 0', fontSize: '14px' }}>
                                {TranslateTag('@RepSavI@', props.language)}
                            </p>
                            <PrimaryButton
                                id="reportdesigner-headerfooter-save-all-reports"
                                text={TranslateTag('@RepSavJ@', props.language)}
                                onClick={() => handleConfirmHeaderFooterSaveScope(HEADER_FOOTER_SAVE_SCOPE.SHARED)}
                            />
                        </div>
                    </div>

                    <div style={{ marginTop: '20px', display: 'flex', justifyContent: 'flex-end' }}>
                        <DefaultButton
                            id="reportdesigner-headerfooter-save-scope-cancel"
                            text={TranslateTag('@GenCan@', props.language)}
                            onClick={handleCancelHeaderFooterSaveScope}
                        />
                    </div>
                </div>
            </Panel>

            {/* Delete Confirmation Panel */}
            <Panel
                isOpen={showDeleteConfirmation}
                onDismiss={handleCancelDelete}
                type={PanelType.medium}
                headerText={TranslateTag('@RepHfDelA@', props.language)}
                closeButtonAriaLabel={TranslateTag('@GenClo@', props.language)}
            >
                {deleteConfirmationData && (
                    <div style={{ padding: '20px' }}>
                        <p>
                            {formatTag(
                                props.language,
                                '@RepHfDelB@',
                                TranslateTag(deleteConfirmationData.isHeader ? '@RepSavC@' : '@RepSavD@', props.language),
                                deleteConfirmationData.sectionName
                            )}
                        </p>
                        
                        <div style={{ marginTop: '20px' }}>
                            <div style={{ marginBottom: '15px', padding: '15px', border: '1px solid #e1dfdd', borderRadius: '4px' }}>
                                <h4 style={{ margin: '0 0 10px 0' }}>{TranslateTag('@RepHfDelC@', props.language)}</h4>
                                <p style={{ margin: '0 0 10px 0', fontSize: '14px' }}>
                                    {formatTag(
                                        props.language,
                                        '@RepHfDelD@',
                                        TranslateTag(deleteConfirmationData.isHeader ? '@RepSavC@' : '@RepSavD@', props.language)
                                    )}
                                </p>
                                <DefaultButton
                                    text={TranslateTag('@RepHfDelE@', props.language)}
                                    onClick={handleDeleteFromReport}
                                />
                            </div>
                            
                            <div style={{ padding: '15px', border: '1px solid #d13438', borderRadius: '4px' }}>
                                <h4 style={{ margin: '0 0 10px 0', color: '#d13438' }}>{TranslateTag('@RepHfDelF@', props.language)}</h4>
                                <p style={{ margin: '0 0 10px 0', fontSize: '14px', color: '#d13438' }}>
                                    {formatTag(
                                        props.language,
                                        '@RepHfDelG@',
                                        TranslateTag(deleteConfirmationData.isHeader ? '@RepSavC@' : '@RepSavD@', props.language)
                                    )}
                                </p>
                                <PrimaryButton
                                    text={TranslateTag('@RepHfDelH@', props.language)}
                                    onClick={handleDeletePermanently}
                                    styles={{ root: { backgroundColor: '#d13438', borderColor: '#d13438' } }}
                                />
                            </div>
                        </div>
                        
                        <div style={{ marginTop: '20px', display: 'flex', justifyContent: 'flex-end' }}>
                            <DefaultButton
                                text={TranslateTag('@GenCan@', props.language)}
                                onClick={handleCancelDelete}
                            />
                        </div>
                    </div>
                )}
            </Panel>

            {/* Clear Report Definition Confirmation Panel */}
            <Panel
                isOpen={showClearConfirmation}
                onDismiss={() => setShowClearConfirmation(false)}
                type={PanelType.medium}
                headerText={TranslateTag('@RepClrA@', props.language)}
                closeButtonAriaLabel={TranslateTag('@GenClo@', props.language)}
            >
                <div style={{ padding: '20px' }}>
                    <MessageBar messageBarType={MessageBarType.warning} styles={{ root: { marginBottom: '20px' } }}>
                        {TranslateTag('@RepClrB@', props.language)}
                    </MessageBar>
                    
                    <p style={{ marginBottom: '15px' }}>
                        <strong>{TranslateTag('@RepClrC@', props.language)}</strong>
                    </p>
                    <ul style={{ marginBottom: '20px', paddingLeft: '20px' }}>
                        <li>{TranslateTag('@RepClrD@', props.language)}</li>
                        <li>{TranslateTag('@RepClrE@', props.language)}</li>
                        <li>{TranslateTag('@RepClrF@', props.language)}</li>
                    </ul>
                    
                    <p style={{ marginBottom: '15px' }}>
                        <strong>{TranslateTag('@RepClrG@', props.language)}</strong>
                    </p>
                    <ul style={{ marginBottom: '20px', paddingLeft: '20px' }}>
                        <li>{TranslateTag('@RepClrH@', props.language)}</li>
                        <li>{TranslateTag('@RepClrI@', props.language)}</li>
                        <li>{TranslateTag('@RepClrJ@', props.language)}</li>
                        <li>{TranslateTag('@RepClrK@', props.language)}</li>
                    </ul>
                    
                    <div style={{ marginTop: '20px', display: 'flex', justifyContent: 'flex-end', gap: '10px' }}>
                        <DefaultButton
                            text={TranslateTag('@GenCan@', props.language)}
                            onClick={() => setShowClearConfirmation(false)}
                        />
                        <PrimaryButton
                            text={TranslateTag('@RepClrL@', props.language)}
                            onClick={handleClearReportDefinition}
                            styles={{ root: { backgroundColor: '#d13438', borderColor: '#d13438' } }}
                        />
                    </div>
                </div>
            </Panel>

            <HeaderFooterSelector
                isOpen={showHeaderFooterSelector}
                onDismiss={() => {
                    setShowHeaderFooterSelector(false);
                    setHeaderFooterType(null);
                }}
                type={headerFooterType}
                allowedSections={Array.isArray(headerFooterType === 'header' ? reportConfig.AllowedHeaders : reportConfig.AllowedFooters) 
                    ? (headerFooterType === 'header' ? reportConfig.AllowedHeaders : reportConfig.AllowedFooters)
                    : [headerFooterType === 'header' ? reportConfig.AllowedHeaders : reportConfig.AllowedFooters]}
                sectionDefinitions={headerFooterType === 'header' ? reportConfig.HeaderSectionDefinitions : reportConfig.FooterSectionDefinitions}
                reportConfig={reportConfig}
                onSelectExisting={handleSelectExistingHeaderFooter}
                onCreateNew={handleCreateNewHeaderFooter}
                language={props.language}
            />

            <ReportPreviewOverlay
                visible={isPreviewOpen}
                onClose={() => setIsPreviewOpen(false)}
                reportData={previewData}
                title={reportConfig?.Title || reportConfig?.Name || 'Report Preview'}
                reportName={reportConfig?.Name}
            />

            {isSavingForPreview && (
                <div className="rd-saving-overlay" role="status" aria-live="polite">
                    <Spinner size={SpinnerSize.large} />
                    <div className="rd-saving-text">Saving report…</div>
                </div>
            )}

            {isSaving && (
                <div className="rd-saving-overlay" role="status" aria-live="polite">
                    <Spinner size={SpinnerSize.large} />
                    <div className="rd-saving-text">Saving report…</div>
                </div>
            )}
        </div>
    );
};

export default ReportDesigner;
