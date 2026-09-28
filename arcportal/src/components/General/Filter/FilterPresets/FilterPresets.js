import React, { useState, useRef } from 'react';
import './FilterPresets.css';
import {
    PrimaryButton,
    DefaultButton,
    IconButton,
    TooltipHost,
    Panel,
    PanelType,
    TextField,
    ContextualMenu,
    Dropdown,
} from '@fluentui/react';
import TranslateTag from '../../../../Utils/Local/TranslateTag';

/**
 * Returns true if the filter state has at least one condition selected
 * (filter values, dates, or search text).
 * @param {Object} filterState - Current filter state
 * @returns {boolean}
 */
const hasFilterConditionsSelected = (filterState) => {
    if (!filterState) return false;
    if (filterState.textSearch && String(filterState.textSearch).trim() !== '') return true;
    if (filterState.startDate) return true;
    if (filterState.endDate) return true;
    if (filterState.filters && Array.isArray(filterState.filters)) {
        for (const filter of filterState.filters) {
            if (filter.values && Array.isArray(filter.values) && filter.values.length > 0) return true;
        }
    }
    return false;
};

/**
 * Builds a filter preset from the current filter state.
 * Includes dropdown filter values, keyword search text, and date range.
 * @param {Object} filterState - Current filter state with filters array, textSearch, startDate, endDate
 * @param {string} name - Display name for the preset
 * @returns {Object} Filter preset config with Key, Name, Default, Fields, TextSearch, StartDate, EndDate
 */
const buildPresetFromFilterState = (filterState, name) => {
    const fields = [];
    if (filterState?.filters && Array.isArray(filterState.filters)) {
        for (const filter of filterState.filters) {
            const values = filter.values && Array.isArray(filter.values) ? [...filter.values] : [];
            fields.push({ Key: filter.Key || filter.key, Values: values });
        }
    }
    const key = 'custom_' + Date.now();
    const preset = { Key: key, Name: name || 'Custom', Default: false, Fields: fields };
    if (filterState?.textSearch != null && String(filterState.textSearch).trim() !== '') {
        preset.TextSearch = filterState.textSearch;
    }
    if (filterState?.startDate != null) {
        preset.StartDate = filterState.startDate;
    }
    if (filterState?.endDate != null) {
        preset.EndDate = filterState.endDate;
    }
    return preset;
};

const FilterPresets = (props) => {
    const canCustomise = props.onSavePreset != null && props.onRemovePreset != null && props.viewName != null;
    const [showSavePanel, setShowSavePanel] = useState(false);
    const [showRemovePanel, setShowRemovePanel] = useState(false);
    const [showPresetMenu, setShowPresetMenu] = useState(false);
    const [presetName, setPresetName] = useState('Custom');
    const [presetToDeleteKey, setPresetToDeleteKey] = useState(null);
    const menuButtonRef = useRef(null);

    const handleOpenSavePanel = () => {
        if (!hasFilterConditionsSelected(props.filterState)) return;
        setPresetName('Custom');
        setShowSavePanel(true);
        setShowPresetMenu(false);
    };

    const handleSaveFromPanel = () => {
        if (!canCustomise || !props.onSavePreset || !props.filterState) return;
        if (!hasFilterConditionsSelected(props.filterState)) return;
        const name = (presetName || 'Custom').trim();
        if (name !== '') {
            const newPreset = buildPresetFromFilterState(props.filterState, name);
            props.onSavePreset(newPreset);
        }
        setShowSavePanel(false);
    };

    const handleOpenRemovePanel = () => {
        const presets = props.filterPresets || [];
        setPresetToDeleteKey(presets.length > 0 ? presets[0].Key : null);
        setShowRemovePanel(true);
        setShowPresetMenu(false);
    };

    const handleDeleteFromPanel = () => {
        if (!canCustomise || !props.onRemovePreset || !presetToDeleteKey) return;
        const preset = (props.filterPresets || []).find((p) => p.Key === presetToDeleteKey);
        if (preset) {
            props.onRemovePreset(preset);
        }
        setShowRemovePanel(false);
    };

    const onShowPresetMenu = (ev) => {
        ev.preventDefault();
        ev.stopPropagation();
        setShowPresetMenu(true);
    };

    const onHidePresetMenu = () => setShowPresetMenu(false);

    const hasFilterConditions = hasFilterConditionsSelected(props.filterState);
    const hasPresets = (props.filterPresets || []).length > 0;

    const menuItems = canCustomise
        ? [
              {
                  key: 'savePreset',
                  text: TranslateTag('@GenFilD@', props.language),
                  iconProps: { iconName: 'Save' },
                  onClick: handleOpenSavePanel,
                  disabled: !hasFilterConditions,
              },
              {
                  key: 'removePreset',
                  text: TranslateTag('@GenFilE@', props.language),
                  iconProps: { iconName: 'Delete' },
                  onClick: handleOpenRemovePanel,
                  disabled: !hasPresets,
              },
          ]
        : [];

    return (
        <div className="filterpreset-content" data-testid="filterpreset-content">
            <div className="filterpresets-buttons">
                <div className="filterpresets-label">
                    {TranslateTag('@GenFilA@', props.language)}:
                </div>
                {props.filterPresets.map((preset) => {
                    return (
                        <div className="filterpresets-button" key={preset.Key}>
                            <PrimaryButton
                                text={preset.Name}
                                data-testid={`filterpreset-${preset.Key}`}
                                onClick={() => props.click(preset)}
                                checked={
                                    props.selectedPreset != null &&
                                    preset.Key === props.selectedPreset.Key
                                        ? true
                                        : false
                                }
                                styles={{
                                    root: { padding: '2px' },
                                    rootChecked: { backgroundColor: 'navy' },
                                    label: { fontSize: '12px' },
                                }}
                            />
                        </div>
                    );
                })}
                {canCustomise && (
                    <div className="filterpresets-menu" ref={menuButtonRef} data-testid="filterpresets-menu">
                        <TooltipHost content={TranslateTag('@GenFilF@', props.language)}>
                            <IconButton
                                iconProps={{ iconName: 'MoreVertical' }}
                                onClick={onShowPresetMenu}
                                styles={{ root: { marginLeft: '8px' } }}
                                ariaLabel={TranslateTag('@GenFilF@', props.language)}
                            />
                        </TooltipHost>
                        <ContextualMenu
                            items={menuItems}
                            hidden={!showPresetMenu}
                            target={menuButtonRef}
                            onItemClick={onHidePresetMenu}
                            onDismiss={onHidePresetMenu}
                        />
                    </div>
                )}
            </div>

            <Panel
                isOpen={showSavePanel}
                onDismiss={() => setShowSavePanel(false)}
                type={PanelType.small}
                headerText={TranslateTag('@GenFilD@', props.language)}
                closeButtonAriaLabel="Close"
            >
                <div className="filterpresets-save-panel">
                    <TextField
                        label={TranslateTag('@GenNam@', props.language)}
                        value={presetName}
                        onChange={(e, value) => setPresetName(value || '')}
                        placeholder="Custom"
                        data-testid="filterpreset-save-name"
                    />
                    <div className="filterpresets-panel-footer">
                        <DefaultButton text={TranslateTag('@GenCan@', props.language)} onClick={() => setShowSavePanel(false)} />
                        <PrimaryButton
                            text={TranslateTag('@GenFilD@', props.language)}
                            onClick={handleSaveFromPanel}
                            disabled={!(presetName || '').trim() || !hasFilterConditionsSelected(props.filterState)}
                            data-testid="filterpreset-save-confirm"
                        />
                    </div>
                </div>
            </Panel>

            <Panel
                isOpen={showRemovePanel}
                onDismiss={() => setShowRemovePanel(false)}
                type={PanelType.small}
                headerText={TranslateTag('@GenFilE@', props.language)}
                closeButtonAriaLabel="Close"
            >
                <div className="filterpresets-remove-panel">
                    <Dropdown
                        label={TranslateTag('@GenFilA@', props.language)}
                        options={(props.filterPresets || []).map((p) => ({ key: p.Key, text: p.Name }))}
                        selectedKey={presetToDeleteKey}
                        onChange={(e, option) => setPresetToDeleteKey(option ? option.key : null)}
                        placeholder={TranslateTag('@GenFilG@', props.language)}
                    />
                    <div className="filterpresets-panel-footer">
                        <DefaultButton text={TranslateTag('@GenCan@', props.language)} onClick={() => setShowRemovePanel(false)} />
                        <PrimaryButton
                            text={TranslateTag('@GenFilE@', props.language)}
                            onClick={handleDeleteFromPanel}
                            disabled={!presetToDeleteKey}
                            iconProps={{ iconName: 'Delete' }}
                        />
                    </div>
                </div>
            </Panel>
        </div>
    );
};

export default FilterPresets;
