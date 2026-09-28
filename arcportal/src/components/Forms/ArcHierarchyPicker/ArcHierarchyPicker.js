import React, { useState, useRef, useCallback, useEffect, useMemo } from 'react';
import { Label, Callout, IconButton, SearchBox, DefaultButton } from '@fluentui/react';
import TreePickerBody from './TreePickerBody';
import FormHandler from '../../Containers/FormHandler/FormHandler';
import TranslateTag from '../../../Utils/Local/TranslateTag';
import { PostList } from '../../../Data/Post';
import {
    getHierarchyPickerAddConfig,
    mapListOptions,
    buildAddChildPrefillValues,
} from '../../../Utils/Forms/getHierarchyPickerAddConfig';
import './ArcHierarchyPicker.css';

/**
 * Hierarchy picker that displays options in a tree (expand/collapse) like HierarchyView.
 * Multi-select: TagPicker-style chips with add-via-tree; single-select: text trigger with clear button.
 *
 * @param {Object} props
 * @param {Object} props.config - Field configuration object.
 * @param {string} props.config.Id - Unique field identifier.
 * @param {string} [props.config.Label] - Label text shown above the picker.
 * @param {boolean} [props.config.Required] - When true, appends " *" to the label.
 * @param {string} [props.config.Placeholder] - Placeholder text shown in the trigger when nothing is selected.
 * @param {string} [props.config.SearchPlaceholder] - Placeholder inside the callout search box.
 * @param {Array}  props.config.Options - Flat list of option objects with key/Key, text/Text, and optional ParentKey.
 * @param {string} [props.config.value] - Current value as a comma-separated string of keys.
 * @param {boolean} [props.config.MultiSelect] - When true, enables multi-select chip mode.
 * @param {number|string} [props.config.Width] - Minimum width of the trigger element.
 * @param {number|string} [props.config.DropDownWidth] - Minimum width of the callout dropdown.
 * @param {boolean} [props.config.LeavesOnly] - When true, only leaf nodes are selectable in the tree.
 * @param {Function} props.valueChangeHandler - Callback invoked on selection change: (id, newValue) where
 *   newValue is a comma-separated key string, or undefined when the selection is cleared.
 * @param {Function} [props.onKeyDown] - Optional keydown handler attached to the outer wrapper div.
 * @param {Array} [props.uievents] - Allowed UI events for permission-gated add actions.
 * @param {Array} [props.forms] - Form definitions for embedded add forms.
 * @param {string} [props.language] - Language code for translated button labels.
 * @param {number|string} [props.recordId] - Parent record id passed to nested FormHandler.
 * @param {string} [props.view] - View name passed to nested FormHandler.
 * @param {Function} [props.onOptionsRefreshed] - Optional callback after list reload: (mappedOptions) => void.
 */
const ArcHierarchyPicker = (props) => {
    const [isCalloutOpen, setIsCalloutOpen] = useState(false);
    const [expandedIds, setExpandedIds] = useState(new Set());
    const [searchText, setSearchText] = useState('');
    const [localOptions, setLocalOptions] = useState([]);
    const [contextSelectedNode, setContextSelectedNode] = useState(null);
    const [formStartConfig, setFormStartConfig] = useState({});
    const triggerRef = useRef(null);
    const addFormSavedRef = useRef(false);

    const config = props.config || {};
    const options = config.Options || [];
    const multiSelect = config.MultiSelect === true || config.multiSelect === true;
    const addConfig = useMemo(
        () => getHierarchyPickerAddConfig(config, props.uievents),
        [config, props.uievents]
    );
    const showAddActions = addConfig != null && Array.isArray(props.forms) && props.forms.length > 0;

    useEffect(() => {
        setLocalOptions(options);
    }, [options]);

    const normalizedOptions = React.useMemo(() => {
        return localOptions.map(opt => ({
            key: opt.key ?? opt.Key,
            text: opt.text ?? opt.Text ?? '',
            ParentKey: opt.ParentKey ?? opt.parentkey ?? opt.parentKey,
            id: opt.id ?? opt.Id ?? opt.key ?? opt.Key,
            organisationname: opt.organisationname ?? opt.OrganisationName ?? opt.text ?? opt.Text,
        }));
    }, [localOptions]);

    const selectedKeys = React.useMemo(() => {
        const val = config.value;
        if (val === undefined || val === null || val === '') return [];
        const str = String(val).trim();
        if (!str) return [];
        return str.split(',').map(s => s.trim()).filter(Boolean);
    }, [config.value]);

    const selectedSet = React.useMemo(() => new Set(selectedKeys), [selectedKeys]);

    const selectedItems = React.useMemo(() => {
        return selectedKeys.map(k => {
            const opt = normalizedOptions.find(o => String(o.key) === String(k));
            return { key: k, text: opt ? opt.text : k };
        });
    }, [selectedKeys, normalizedOptions]);

    const getDisplayText = useCallback(() => {
        if (selectedKeys.length === 0) return '';
        return selectedItems.map(s => s.text).join(', ');
    }, [selectedItems, selectedKeys.length]);

    const toggleExpand = useCallback((id) => {
        setExpandedIds(prev => {
            const next = new Set(prev);
            const sid = id == null ? null : String(id);
            if (next.has(sid)) next.delete(sid);
            else next.add(sid);
            return next;
        });
    }, []);

    const handleSelect = useCallback((item) => {
        const key = item?.key ?? item?.Key;
        if (key == null) return;

        if (multiSelect) {
            if (selectedSet.has(String(key))) return;
            const next = [...selectedKeys, String(key)];
            props.valueChangeHandler(config.Id, next.join(','));
            setIsCalloutOpen(false);
        } else {
            props.valueChangeHandler(config.Id, String(key));
            setIsCalloutOpen(false);
        }
        setSearchText('');
        setContextSelectedNode(null);
    }, [multiSelect, selectedKeys, selectedSet, config.Id, props.valueChangeHandler]);

    const handleContextSelect = useCallback((item) => {
        setContextSelectedNode(item);
    }, []);

    const handleRemove = useCallback((keyToRemove) => {
        const next = selectedKeys.filter(k => String(k) !== String(keyToRemove));
        props.valueChangeHandler(config.Id, next.length ? next.join(',') : undefined);
    }, [selectedKeys, config.Id, props.valueChangeHandler]);

    const handleClear = useCallback(() => {
        props.valueChangeHandler(config.Id, undefined);
    }, [config.Id, props.valueChangeHandler]);

    const handleDismiss = useCallback(() => {
        if (formStartConfig?.button) {
            return;
        }
        setIsCalloutOpen(false);
        setSearchText('');
        setContextSelectedNode(null);
    }, [formStartConfig]);

    const refreshOptionsAfterAdd = useCallback((savedId, savedData) => {
        if (!addConfig?.listName) {
            return;
        }

        const extractNewId = () => {
            if (savedData && !Array.isArray(savedData)) {
                return savedData.Id ?? savedData.id ?? savedData.Key ?? savedData.key;
            }
            if (Array.isArray(savedData)) {
                const idEntry = savedData.find((entry) => ['id', 'Id'].includes(entry.key));
                return idEntry?.value;
            }
            return savedId;
        };

        PostList(
            [{ Name: addConfig.listName, IncludeFixed: true, Translate: true }],
            (data) => {
                const mapped = mapListOptions(data, addConfig.listName);
                setLocalOptions(mapped);
                props.onOptionsRefreshed?.(mapped);

                let newId = extractNewId();
                if ((newId == null || newId === '' || newId === 0 || newId === '0') && savedData && !Array.isArray(savedData)) {
                    const savedName = savedData.Name ?? savedData.name;
                    if (savedName) {
                        const match = mapped.find((option) => String(option.text).includes(String(savedName)));
                        newId = match?.key;
                    }
                }
                if (newId != null && newId !== '' && newId !== 0 && newId !== '0') {
                    if (multiSelect) {
                        const next = selectedSet.has(String(newId))
                            ? selectedKeys
                            : [...selectedKeys, String(newId)];
                        props.valueChangeHandler(config.Id, next.join(','));
                    } else {
                        props.valueChangeHandler(config.Id, String(newId));
                    }
                }
                setIsCalloutOpen(false);
                setSearchText('');
                setContextSelectedNode(null);
            },
            () => {},
            {},
            props.recordId ?? 0
        );
    }, [
        addConfig,
        config.Id,
        multiSelect,
        props.onOptionsRefreshed,
        props.recordId,
        props.valueChangeHandler,
        selectedKeys,
        selectedSet,
    ]);

    const handleAddFormClose = useCallback(() => {
        setFormStartConfig({});
        if (!addFormSavedRef.current) {
            setIsCalloutOpen(true);
        }
        addFormSavedRef.current = false;
    }, []);

    const openAddForm = useCallback(() => {
        if (!addConfig) {
            return;
        }

        addFormSavedRef.current = false;

        let localFormData;
        const values = buildAddChildPrefillValues(addConfig, contextSelectedNode);
        if (values) {
            localFormData = { values };
        }

        setFormStartConfig({
            button: {
                UIEvent: addConfig.uiEvent,
                OnFinish: 'refresh',
            },
            id: 0,
            view: props.view,
            showFullScreenButton: false,
            localFormData,
            refresh: (action, id, savedData) => {
                addFormSavedRef.current = true;
                refreshOptionsAfterAdd(id, savedData);
            },
            onClose: handleAddFormClose,
        });
        setIsCalloutOpen(false);
    }, [addConfig, contextSelectedNode, props.view, refreshOptionsAfterAdd, handleAddFormClose]);

    const filteredOptions = React.useMemo(() => {
        const q = (searchText || '').trim().toLowerCase();
        if (!q) return normalizedOptions;
        return normalizedOptions.filter(opt =>
            (opt.text ?? '').toLowerCase().includes(q)
        );
    }, [normalizedOptions, searchText]);

    const leavesOnly = config.LeavesOnly === true || config.leavesOnly === true
        || ((config.Id === 'growthid' || config.id === 'growthid') && (config.OptionsName === 'SpecimenGrowth' || config.optionsName === 'SpecimenGrowth'));
    const parentIdsFromFullTree = React.useMemo(() => {
        if (!leavesOnly) return null;
        const ids = new Set();
        for (const opt of normalizedOptions) {
            const parentKey = opt.ParentKey ?? opt.parentkey ?? opt.parentKey;
            if (parentKey != null && parentKey !== '' && parentKey !== 0) {
                ids.add(String(parentKey));
            }
        }
        return ids;
    }, [normalizedOptions, leavesOnly]);

    const contextSelectedKey = contextSelectedNode?.key ?? contextSelectedNode?.Key ?? contextSelectedNode?.id ?? contextSelectedNode?.Id;

    const handleTriggerKeyDown = (e) => {
        if (e.key === "Enter" || e.key === " ") {
            e.preventDefault();
            setIsCalloutOpen(true);
        }
        if (e.key === "ArrowDown") {
            e.preventDefault();
            setIsCalloutOpen(true);
        }
        if (e.key === "Escape") {
            setIsCalloutOpen(false);
        }
    };

    const canRenderShell = normalizedOptions.length > 0 || showAddActions;
    if (!canRenderShell) {
        return null;
    }

    const addPlaceholder = config.Placeholder || config.placeholder || '+ Add...';
    const searchPlaceholder = config.SearchPlaceholder ?? config.searchPlaceholder ?? 'Filter...';
    const triggerMinWidth = config.Width ?? config.width;
    const calloutMinWidth = config.DropDownWidth ?? config.dropdownwidth;
    const language = props.language;

    const labelText = (config.Label ?? '') + (config.Required ? ' *' : '');

    const addButtonText = addConfig ? TranslateTag(addConfig.addLabelTag, language) : '';
    const contextNodeText = contextSelectedNode?.text ?? contextSelectedNode?.Text ?? '';
    const addButtonLabel = contextNodeText
        ? `${addButtonText} (${contextNodeText})`
        : addButtonText;

    return (
        <div id={config.Id} onKeyDown={props.onKeyDown}>
            {labelText ? <Label>{labelText}</Label> : null}
            <div
                ref={triggerRef}
                className={`hierarchy-picker-trigger${multiSelect ? ' hierarchy-picker-trigger-multiselect' : ''}`}
                style={triggerMinWidth ? { minWidth: triggerMinWidth } : undefined}
            >
                {multiSelect ? (
                    <div className="hierarchy-picker-chips-container" onClick={() => setIsCalloutOpen(true)}>
                        {selectedItems.map(item => (
                            <span key={item.key} className="hierarchy-picker-chip">
                                <span className="hierarchy-picker-chip-text">{item.text}</span>
                                <IconButton
                                    iconProps={{ iconName: 'Cancel' }}
                                    styles={{ root: { width: 20, height: 20 }, icon: { fontSize: 10 } }}
                                    onClick={(e) => { e.stopPropagation(); handleRemove(item.key); }}
                                    ariaLabel="Remove"
                                />
                            </span>
                        ))}
                        <span id={`${config.Id}-add-trigger`} className="hierarchy-picker-add-trigger">{addPlaceholder}</span>
                    </div>
                ) : (
                    <div
                        className="hierarchy-picker-single-trigger"
                        onClick={() => setIsCalloutOpen(true)}
                        role="combobox"
                        aria-haspopup="listbox"
                        aria-expanded={isCalloutOpen}
                        tabIndex={0}
                        onKeyDown={handleTriggerKeyDown}
                    >
                        <span className="hierarchy-picker-single-text">
                            {getDisplayText() || <span className="hierarchy-picker-placeholder">{addPlaceholder}</span>}
                        </span>
                        {selectedKeys.length > 0 && (
                            <IconButton
                                iconProps={{ iconName: 'Cancel' }}
                                styles={{ root: { width: 24, height: 24, flexShrink: 0 }, icon: { fontSize: 10 } }}
                                onClick={(e) => { e.stopPropagation(); handleClear(); }}
                                ariaLabel="Clear selection"
                            />
                        )}
                    </div>
                )}
            </div>
            {isCalloutOpen && triggerRef.current && (
                <Callout
                    target={triggerRef.current}
                    onDismiss={handleDismiss}
                    setInitialFocus
                    calloutMaxHeight={400}
                >
                    <div
                        className="hierarchy-picker-callout"
                        style={calloutMinWidth ? { minWidth: calloutMinWidth } : undefined}
                    >
                        <div className="hierarchy-picker-search">
                            <SearchBox
                                id={`${config.Id}-search`}
                                placeholder={searchPlaceholder}
                                value={searchText}
                                onChange={(e, value) => setSearchText(value ?? '')}
                            />
                        </div>
                        <div className="hierarchy-picker-scroll" style={{ maxHeight: 280, overflowY: 'auto' }}>
                            <TreePickerBody
                                fieldId={config.Id}
                                options={filteredOptions}
                                idField="key"
                                parentIdField="ParentKey"
                                textField="text"
                                expandedIds={expandedIds}
                                onToggleExpand={toggleExpand}
                                onSelect={handleSelect}
                                selectedKeys={multiSelect ? undefined : selectedSet}
                                multiSelect={multiSelect}
                                leavesOnly={leavesOnly}
                                parentIdsFromFullTree={parentIdsFromFullTree}
                                useContextSelection={showAddActions}
                                contextSelectedKey={contextSelectedKey}
                                onContextSelect={handleContextSelect}
                            />
                        </div>
                        {showAddActions && (
                            <div className="hierarchy-picker-actions">
                                <DefaultButton
                                    id={`${config.Id}-add-root`}
                                    text={addButtonLabel}
                                    iconProps={{ iconName: 'Add' }}
                                    onClick={openAddForm}
                                    title={contextNodeText
                                        ? TranslateTag(addConfig.addChildLabelTag, language)
                                        : addButtonText}
                                />
                            </div>
                        )}
                    </div>
                </Callout>
            )}
            {showAddActions && <FormHandler startConfig={formStartConfig} />}
        </div>
    );
};

export default ArcHierarchyPicker;
