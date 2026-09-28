import React, { useState, useEffect, useRef } from 'react';
import { Panel, PanelType, Checkbox, PrimaryButton, DefaultButton } from '@fluentui/react';
import TranslateTag from '../../../Utils/Local/TranslateTag';
import './ColumnPicker.css';

/**
 * Column picker panel for selecting which columns to display in a list view.
 * @param {Object} props - Component props
 * @param {boolean} props.isOpen - Whether the panel is open
 * @param {function} props.onDismiss - Called when the panel is closed
 * @param {Array} props.columns - Base column configs (Key, Name, FieldName, ...)
 * @param {Object} props.columnLayout - Current layout { VisibleKeys, Order, Widths }
 * @param {function} props.onColumnLayoutChange - Called with updated layout
 * @param {function} [props.onReset] - Called when user clicks Reset to default (clears saved layout)
 * @param {Object} props.language - Language config for TranslateTag
 */
const ColumnPicker = (props) => {
    const { isOpen, onDismiss, columns, columnLayout, onColumnLayoutChange, language } = props;

    const visibleKeys = columnLayout?.VisibleKeys || columnLayout?.visibleKeys;
    const hasCustomVisibility = Array.isArray(visibleKeys) && visibleKeys.length > 0;

    const [localChecked, setLocalChecked] = useState({});
    const wasOpenRef = useRef(false);

    // Sync localChecked when panel opens or when columnLayout/columns change.
    // Use visibilityKey (stable string) instead of visibleSet to avoid effect running every render
    // (visibleSet is a new Set each render, which would overwrite user's in-progress toggles).
    const visibilityKey = hasCustomVisibility ? visibleKeys.map((k) => String(k).toLowerCase()).sort().join(',') : 'default';
    useEffect(() => {
        if (!isOpen || !columns) {
            wasOpenRef.current = false;
            return;
        }
        const opening = !wasOpenRef.current;
        wasOpenRef.current = true;
        if (!opening) return;
        const keySet = visibilityKey === 'default' ? null : new Set(visibilityKey.split(','));
        const checked = {};
        columns.forEach((col) => {
            const key = (col.Key || col.key || '').toLowerCase();
            checked[key] = keySet ? keySet.has(key) : !(col.defaultHidden || col.DefaultHidden);
        });
        setLocalChecked(checked);
    }, [isOpen, columns, visibilityKey]);

    const handleToggle = (key, checked) => {
        setLocalChecked((prev) => ({ ...prev, [key]: checked }));
    };

    const handleApply = () => {
        const newVisibleKeys = columns
            .filter((col) => localChecked[(col.Key || col.key || '').toLowerCase()])
            .map((col) => col.Key || col.key);
        onColumnLayoutChange({
            VisibleKeys: newVisibleKeys,
            Order: columnLayout?.Order || columnLayout?.order,
            Widths: columnLayout?.Widths || columnLayout?.widths,
        });
        onDismiss();
    };

    const handleReset = () => {
        props.onReset?.();
        setLocalChecked({});
        onDismiss();
    };

    if (!columns || columns.length === 0) return null;

    const headerText = TranslateTag('@GenCol@', language);

    return (
        <Panel
            isOpen={isOpen}
            onDismiss={onDismiss}
            type={PanelType.small}
            headerText={headerText}
            headerTextProps={{ 'data-testid': 'column-picker-header' }}
            closeButtonAriaLabel={TranslateTag('@GenClo@', language)}
        >
            <div className="column-picker-content" data-testid="column-picker-panel">
                <p className="column-picker-description" data-testid="column-picker-description">
                    {TranslateTag('@GenSelCol@', language)}
                </p>
                <div className="column-picker-list">
                    {columns.map((col) => {
                        const key = col.Key || col.key;
                        const keyLower = (key || '').toLowerCase();
                        const name = col.Name || col.name || key || '';
                        const displayName = typeof name === 'string' && name.startsWith('@') ? TranslateTag(name, language) : name;
                        const checked = localChecked[keyLower] !== false;
                        return (
                            <div key={keyLower} className="column-picker-option" data-testid={`column-picker-${keyLower}`} data-column-name={displayName || key}>
                            <Checkbox
                                key={keyLower}
                                label={displayName || key}
                                checked={checked}
                                onChange={(e, isChecked) => handleToggle(keyLower, isChecked)}
                                styles={{ root: { marginBottom: 8 } }}
                            />
                            </div>
                        );
                    })}
                </div>
                <div className="column-picker-actions">
                    <PrimaryButton data-testid="column-picker-apply" text={TranslateTag('@GenColB@', language)} onClick={handleApply} />
                    <DefaultButton data-testid="column-picker-reset" text={TranslateTag('@GenColC@', language)} onClick={handleReset} />
                </div>
            </div>
        </Panel>
    );
};

export default ColumnPicker;
