import React, { useState, useCallback } from 'react';
import { 
    Panel, 
    PanelType, 
    PrimaryButton, 
    DefaultButton, 
    Dropdown,
    TextField,
    Stack,
    Label,
    Checkbox
} from '@fluentui/react';

/**
 * Panel for adding a target state, and optionally the conditions that lead to it, to one event.
 *
 * Fields are offered by their stored id and only displayed by their label, so a condition is always
 * written against the payload field id rather than against translated text.
 * @param {object} props - Component props.
 * @param {boolean} props.isOpen - Whether the panel is showing.
 * @param {Function} props.onDismiss - Closes the panel without applying anything.
 * @param {Function} props.onSubmit - Receives the new target state entry.
 * @param {Array<object>} props.availableStates - Every state of the workflow, keyed by listitem id.
 * @param {string} props.eventName - The event the target state is being added to.
 * @param {Array<object>} props.existingTargetStates - Target states the event already has.
 * @param {Array<object>} props.eventConfig - Event lookups supplied by the backend.
 * @param {Array<object>} props.listConfig - Option lists referenced by event fields.
 * @param {object} props.language - The active language.
 * @returns {JSX.Element} The panel.
 */
const AddTargetStateForm = ({ 
    isOpen, 
    onDismiss, 
    onSubmit, 
    availableStates, 
    eventName,
    existingTargetStates,
    eventConfig,
    listConfig,
    language 
}) => {
    const [selectedStateId, setSelectedStateId] = useState('');
    const [conditionType, setConditionType] = useState('and');
    const [conditions, setConditions] = useState([{ field: '', value: '' }]);
    const [isDefault, setIsDefault] = useState(false);

    /**
     * Builds the target state entry from the form and hands it to the designer.
     * Conditions with no field or no value are dropped rather than stored half filled.
     */
    const handleSubmit = useCallback(() => {
        if (!selectedStateId) return;

        const newTargetState = {
            newstate: selectedStateId,
            conditiontype: conditionType,
            conditions: conditions.filter(c => c.field && c.value),
            isDefault: isDefault
        };

        onSubmit(newTargetState);
        
        // Reset form
        setSelectedStateId('');
        setConditionType('and');
        setConditions([{ field: '', value: '' }]);
        setIsDefault(false);
        onDismiss();
    }, [selectedStateId, conditionType, conditions, isDefault, onSubmit, onDismiss]);

    const handleCancel = useCallback(() => {
        setSelectedStateId('');
        setConditionType('and');
        setConditions([{ field: '', value: '' }]);
        setIsDefault(false);
        onDismiss();
    }, [onDismiss]);

    const addCondition = useCallback(() => {
        setConditions(prev => [...prev, { field: '', value: '' }]);
    }, []);

    const removeCondition = useCallback((index) => {
        setConditions(prev => prev.filter((_, i) => i !== index));
    }, []);

    const updateCondition = useCallback((index, field, value) => {
        setConditions(prev => prev.map((condition, i) => 
            i === index ? { ...condition, [field]: value } : condition
        ));
    }, []);

    // Filter out states that are already target states for this event
    const existingTargetStateIds = (existingTargetStates || []).map(targetState => String(targetState.id));
    const filteredStates = (availableStates || []).filter(state => 
        !existingTargetStateIds.includes(String(state.Key))
    );
    
    const stateOptions = filteredStates.map(state => ({
        key: String(state.Key),
        text: state.Value
    }));

    // Get the current event's field configuration. Events are matched by name, case-insensitively,
    // because a workflow step and its event configuration do not always agree on casing.
    const currentEventConfig = eventConfig?.find(event =>
        String(event.EventName || '').toLowerCase() === String(eventName || '').toLowerCase());
    const eventFields = currentEventConfig?.Fields || [];
    
    const fieldOptions = [
        // Add special fields that are commonly used in conditions
        { key: 'currentstate', text: 'Current State' },
        // Add event fields
        ...eventFields.map(field => ({
            key: field.Id,
            text: field.Label || field.Id
        }))
    ];

    const conditionTypeOptions = [
        { key: 'and', text: 'AND' },
        { key: 'or', text: 'OR' }
    ];

    /**
     * Finds the option list a condition value should be picked from, when the field has one.
     * @param {string} fieldId - The stored field id the condition is written against.
     * @returns {Array<object>|null} Dropdown options, or null when the value is free text.
     */
    const getFieldOptions = useCallback((fieldId) => {
        // Special case for currentstate - show available states
        if (fieldId === 'currentstate') {
            return (availableStates || []).map(state => ({
                key: String(state.Key),
                text: state.Value
            }));
        }
        
        const field = eventFields.find(f => f.Id === fieldId);
        if (!field?.OptionsName) return null;
        
        const listConfigItem = listConfig?.find(list => list.OptionsName === field.OptionsName);
        return listConfigItem?.Contents?.map(item => ({
            key: String(item.Key),
            text: item.Value
        })) || null;
    }, [eventFields, listConfig, availableStates]);

    return (
        <Panel
            isOpen={isOpen}
            onDismiss={handleCancel}
            type={PanelType.medium}
            headerText={`Add Target State for ${eventName}`}
            closeButtonAriaLabel="Close"
        >
            <div style={{ padding: '20px' }} id="addtargetstate">
                <Stack tokens={{ childrenGap: 16 }}>
                    <Dropdown
                        id="addtargetstate-state"
                        label="Target State"
                        placeholder={stateOptions.length === 0 ? "No available states to add" : "Select a state..."}
                        options={stateOptions}
                        selectedKey={selectedStateId}
                        onChange={(event, option) => setSelectedStateId(option?.key || '')}
                        required
                        disabled={stateOptions.length === 0}
                    />
                    {stateOptions.length === 0 && (
                        <div style={{ color: '#666', fontSize: '12px', marginTop: '4px' }}>
                            All available states are already target states for this event.
                        </div>
                    )}

                    <Dropdown
                        id="addtargetstate-conditiontype"
                        label="Condition Type"
                        options={conditionTypeOptions}
                        selectedKey={conditionType}
                        onChange={(event, option) => setConditionType(option?.key || 'and')}
                    />

                    <Checkbox
                        id="addtargetstate-isdefault"
                        label="Set as default target state"
                        checked={isDefault}
                        onChange={(event, checked) => setIsDefault(checked || false)}
                    />
                    {isDefault && (
                        <div style={{ color: '#666', fontSize: '12px', marginTop: '4px' }}>
                            This state will be used when no other conditions are met.
                        </div>
                    )}

                    <div>
                        <Label>Conditions</Label>
                        {conditions.map((condition, index) => {
                            const fieldOptionsForValue = getFieldOptions(condition.field);
                            return (
                                <Stack key={index} horizontal tokens={{ childrenGap: 8 }} style={{ marginBottom: 8 }}>
                                    <Dropdown
                                        id={`addtargetstate-condition-${index}-field`}
                                        placeholder="Select field..."
                                        options={fieldOptions}
                                        selectedKey={condition.field}
                                        onChange={(event, option) => updateCondition(index, 'field', option?.key || '')}
                                        styles={{ root: { flex: 1 } }}
                                    />
                                    {fieldOptionsForValue ? (
                                        <Dropdown
                                            id={`addtargetstate-condition-${index}-value`}
                                            placeholder="Select value..."
                                            options={fieldOptionsForValue}
                                            selectedKey={condition.value}
                                            onChange={(event, option) => updateCondition(index, 'value', option?.key || '')}
                                            styles={{ root: { flex: 1 } }}
                                        />
                                    ) : (
                                        <TextField
                                            id={`addtargetstate-condition-${index}-value`}
                                            placeholder="Enter value"
                                            value={condition.value}
                                            onChange={(event, value) => updateCondition(index, 'value', value || '')}
                                            styles={{ root: { flex: 1 } }}
                                        />
                                    )}
                                    {conditions.length > 1 && (
                                        <DefaultButton
                                            id={`addtargetstate-condition-${index}-remove`}
                                            iconProps={{ iconName: 'Delete' }}
                                            onClick={() => removeCondition(index)}
                                            title="Remove condition"
                                            styles={{ root: { minWidth: 'auto' } }}
                                        />
                                    )}
                                </Stack>
                            );
                        })}
                        <DefaultButton
                            id="addtargetstate-addcondition"
                            text="Add Condition"
                            iconProps={{ iconName: 'Add' }}
                            onClick={addCondition}
                            styles={{ root: { marginTop: 8 } }}
                        />
                    </div>

                    <Stack horizontal tokens={{ childrenGap: 8 }} style={{ marginTop: 20 }}>
                        <PrimaryButton
                            id="addtargetstate-submit"
                            text="Add Target State"
                            onClick={handleSubmit}
                            disabled={!selectedStateId}
                        />
                        <DefaultButton
                            id="addtargetstate-cancel"
                            text="Cancel"
                            onClick={handleCancel}
                        />
                    </Stack>
                </Stack>
            </div>
        </Panel>
    );
};

export default AddTargetStateForm;
