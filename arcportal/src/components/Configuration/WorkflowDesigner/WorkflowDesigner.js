import React, { useCallback, useEffect, useMemo, useRef, useState } from 'react';
import { connect } from 'react-redux';
import { Panel, PanelType, DefaultButton, PrimaryButton, MessageBar, MessageBarType } from '@fluentui/react';
import Filter from '../../General/Filter/Filter';
import TopbarMenu from '../../General/TopbarMenu/TopBarMenu';
import TranslateTag from '../../../Utils/Local/TranslateTag';
import EventPanel from './EventPanel';
import AddTargetStateForm from './AddTargetStateForm';
import './WorkflowDesigner.css';

/**
 * Splits a workflow entry state list into its individual state ids.
 * @param {string} stateList - Comma separated listitem ids.
 * @returns {Array<string>} The trimmed state ids.
 */
function parseStateListString(stateList) {
    if (!stateList) return [];
    return stateList
        .split(',')
        .map((s) => s.trim())
        .filter((s) => s.length > 0);
}

/**
 * Builds the swimlane data for one focused state: every event reachable from that state, with the
 * states that can trigger it and the states it can lead to.
 *
 * States are matched on their listitem id throughout; the state name is only ever displayed, so a
 * translated label can never change which transition a lane describes.
 * @param {object} workflow - The workflow document being edited.
 * @param {object} supportingConfig - The read-only state, event and list lookups.
 * @param {string} focusStateId - The listitem id of the state currently in focus.
 * @returns {{events: Array<object>, states: Array<object>}} The lanes and the full state list.
 */
function buildWorkflowData(workflow, supportingConfig, focusStateId) {
    if (!focusStateId || !supportingConfig?.StateConfig) {
        return { events: [], states: supportingConfig?.StateConfig || [] };
    }

    const stateConfig = supportingConfig.StateConfig;
    const steps = workflow?.Steps || [];

    // Find events available from the focused state
    const availableEvents = steps.filter(step => {
        const entry = step.entryState || step.entrystate || step.EntryState || '';
        const entryStates = parseStateListString(entry);
        return entryStates.includes(focusStateId);
    });

    // Build event data with their source states and target states
    const events = availableEvents.map((step, index) => {
        const eventName = step.event || step.Event || `event_${index + 1}`;
        const entry = step.entryState || step.entrystate || step.EntryState || '';
        const entryStates = parseStateListString(entry);

        // Get source states (states that can trigger this event)
        const sourceStates = entryStates.map(stateId => {
            const state = stateConfig.find(s => String(s.Key) === stateId);
            return state ? {
                id: stateId,
                name: state.Value || stateId,
                isCurrentState: stateId === focusStateId
            } : null;
        }).filter(Boolean);

        // Collect all possible target states
        const exitState = step.exitstate || step.ExitState;
        const actions = step.actions || step.Actions;
        const targets = new Set();
        if (exitState?.default ?? exitState?.Default) targets.add(String(exitState.default ?? exitState.Default));
        const exitOptions = exitState?.options || exitState?.Options;
        if (Array.isArray(exitOptions)) {
            exitOptions.forEach((opt) => {
                const newState = opt?.newstate ?? opt?.NewState;
                if (newState) targets.add(String(newState));
            });
        }
        const actionOptions = actions?.options || actions?.Options;
        if (Array.isArray(actionOptions)) {
            actionOptions.forEach((opt) => {
                const newState = opt?.newState ?? opt?.NewState ?? opt?.newstate;
                if (newState) targets.add(String(newState));
            });
        }

        // Convert target IDs to state objects
        const targetStates = Array.from(targets).map(stateId => {
            const state = stateConfig.find(s => String(s.Key) === stateId);
            return state ? {
                id: stateId,
                name: state.Value || stateId,
                availableEvents: steps.filter(s => {
                    const stepEntry = s.entryState || s.entrystate || s.EntryState || '';
                    return parseStateListString(stepEntry).includes(stateId);
                }).length
            } : null;
        }).filter(Boolean);

        return {
            id: eventName,
            name: eventName,
            sourceStates,
            targetStates
        };
    });

    return { events, states: stateConfig };
}

/**
 * Turns an event or state name into a value that is safe to embed in a DOM id.
 * @param {string} value - The name to normalise.
 * @returns {string} The lower cased name with anything that is not a letter or digit removed.
 */
const normaliseId = (value) => String(value ?? '').toLowerCase().replace(/[^a-z0-9]/g, '');

/**
 * The workflow designer canvas.
 *
 * Edits are made against a local copy of the loaded workflow, so Cancel is a genuine revert to the
 * last persisted document rather than to another in-memory copy. Save writes the whole document
 * through the container and the designer then re-renders from whatever the backend stored.
 * @param {object} props - Component props.
 * @param {object} props.workflow - The stored workflow document, as loaded.
 * @param {object} props.supportingConfig - Read-only state, event and list lookups.
 * @param {string} props.workflowName - The configuration name of the workflow, shown in the header.
 * @param {Function} props.onSaveConfiguration - Persists the edited document; returns a Promise.
 * @param {object} props.language - The active language, used for button labels.
 * @returns {JSX.Element} The designer.
 */
const WorkflowDesigner = ({ workflow, supportingConfig, workflowName, onSaveConfiguration, language }) => {
    const [focusStateId, setFocusStateId] = useState(null);
    const [showDeleteConfirmation, setShowDeleteConfirmation] = useState(false);
    const [deleteConfirmationData, setDeleteConfirmationData] = useState(null);
    const [hasUnsavedChanges, setHasUnsavedChanges] = useState(false);
    const [isSaving, setIsSaving] = useState(false);
    const [localWorkflow, setLocalWorkflow] = useState(workflow);
    const [showAddTargetStateForm, setShowAddTargetStateForm] = useState(false);
    const [addTargetStateEventName, setAddTargetStateEventName] = useState('');

    const hasUnsavedChangesRef = useRef(hasUnsavedChanges);
    hasUnsavedChangesRef.current = hasUnsavedChanges;

    // Adopt the loaded workflow. Unsaved edits are kept when the same document is re-supplied,
    // because a parent re-render must not silently discard work the user has not saved yet.
    useEffect(() => {
        if (hasUnsavedChangesRef.current) {
            return;
        }
        setLocalWorkflow(workflow);
    }, [workflow]);

    // Focus the workflow's start state. This reads the loaded document rather than the local copy,
    // because the local copy is adopted in an effect and so is still empty on the render where the
    // supporting config first arrives; reading it here would fall through to the first state in the
    // list instead.
    useEffect(() => {
        if (focusStateId) return;
        const start = String(workflow?.StartState ?? workflow?.startState ?? '') || null;
        const first = supportingConfig?.StateConfig && supportingConfig.StateConfig.length > 0
            ? String(supportingConfig.StateConfig[0].Key)
            : null;
        setFocusStateId(start || first);
    }, [focusStateId, workflow, supportingConfig?.StateConfig]);

    // Build workflow data for the focused state
    const { events, states } = useMemo(() => {
        return buildWorkflowData(localWorkflow, supportingConfig, focusStateId);
    }, [localWorkflow, supportingConfig, focusStateId]);

    // Build filter configuration for state selection
    const stateFilter = useMemo(() => {
        const stateOptions = states.map(state => ({
            key: String(state.Key),
            text: state.Value
        }));

        return {
            Key: 'stateSelection',
            FieldName: 'stateSelection',
            PlaceHolder: 'Select State...',
            MultiSelect: false,
            Options: stateOptions,
            Width: 300,
            DropDownWidth: 300,
            values: focusStateId ? [focusStateId] : []
        };
    }, [states, focusStateId]);


    const handleStateClick = useCallback((stateId) => {
        setFocusStateId(stateId);
    }, []);

    const handleFilterClick = useCallback((event, option, filterKey) => {
        if (filterKey === 'stateSelection' && option) {
            setFocusStateId(option.key);
        }
    }, []);

    const handleFilterRefresh = useCallback(() => {
        // Refresh logic if needed
    }, []);

    const handleFilterClear = useCallback(() => {
        setFocusStateId(null);
    }, []);

    const handleRemoveEvent = useCallback((eventName, sourceStateId) => {
        // Show confirmation panel
        setDeleteConfirmationData({
            eventName,
            sourceStateId,
            stateName: supportingConfig?.StateConfig?.find(s => String(s.Key) === sourceStateId)?.Value || sourceStateId
        });
        setShowDeleteConfirmation(true);
    }, [supportingConfig]);

    const handleConfirmDelete = useCallback(() => {
        if (!localWorkflow || !deleteConfirmationData) return;
        
        const { eventName, sourceStateId, targetStateId, type } = deleteConfirmationData;
        
        // Create a copy of the local workflow
        const updatedWorkflow = { ...localWorkflow };
        
        if (type === 'targetState') {
            // Handle target state removal
            const updatedSteps = localWorkflow.Steps.map(step => {
                if (step.event === eventName) {
                    const updatedStep = { ...step };
                    
                    // Remove from exitstate.default
                    if (updatedStep.exitstate?.default === targetStateId) {
                        updatedStep.exitstate = { ...updatedStep.exitstate };
                        delete updatedStep.exitstate.default;
                    }
                    
                    // Remove from exitstate.options
                    if (updatedStep.exitstate?.options) {
                        updatedStep.exitstate = { ...updatedStep.exitstate };
                        updatedStep.exitstate.options = updatedStep.exitstate.options.filter(option => 
                            String(option.newstate) !== targetStateId
                        );
                    }
                    
                    // Remove from actions.options
                    if (updatedStep.actions?.options) {
                        updatedStep.actions = { ...updatedStep.actions };
                        updatedStep.actions.options = updatedStep.actions.options.filter(option => 
                            String(option.newState) !== targetStateId
                        );
                    }
                    
                    return updatedStep;
                }
                return step;
            });
            
            updatedWorkflow.Steps = updatedSteps;
        } else if (type === 'sourceState') {
            // Handle source state removal (specific source)
            const updatedSteps = localWorkflow.Steps.map(step => {
                const entry = step.entryState || step.entrystate || '';
                const entryStates = parseStateListString(entry);
                
                // If this step has the event and the source state, remove the source state
                if (step.event === eventName && entryStates.includes(sourceStateId)) {
                    const newEntryStates = entryStates.filter(stateId => stateId !== sourceStateId);
                    
                    // If no states left, remove the entire step
                    if (newEntryStates.length === 0) {
                        return null;
                    }
                    
                    // Update the step with remaining states
                    return {
                        ...step,
                        entryState: newEntryStates.join(', '),
                        entrystate: newEntryStates.join(', ')
                    };
                }
                
                return step;
            }).filter(Boolean); // Remove null entries
            
            updatedWorkflow.Steps = updatedSteps;
        } else {
            // Fallback: remove event from current focused state
            const updatedSteps = localWorkflow.Steps.map(step => {
                const entry = step.entryState || step.entrystate || '';
                const entryStates = parseStateListString(entry);

                if (step.event === eventName && entryStates.includes(String(focusStateId))) {
                    const newEntryStates = entryStates.filter(stateId => stateId !== String(focusStateId));

                    if (newEntryStates.length === 0) {
                        return null;
                    }

                    return {
                        ...step,
                        entryState: newEntryStates.join(', '),
                        entrystate: newEntryStates.join(', ')
                    };
                }

                return step;
            }).filter(Boolean);

            updatedWorkflow.Steps = updatedSteps;
        }
        
        // Update local workflow and mark as having unsaved changes
        setLocalWorkflow(updatedWorkflow);
        setHasUnsavedChanges(true);
        
        // Close the panel
        setShowDeleteConfirmation(false);
        setDeleteConfirmationData(null);
    }, [localWorkflow, deleteConfirmationData, focusStateId]);

    const handleCancelDelete = useCallback(() => {
        setShowDeleteConfirmation(false);
        setDeleteConfirmationData(null);
    }, []);

    const handleRemoveSourceState = useCallback((eventName, sourceStateId) => {
        // Prevent removing the currently focused source state via this path
        if (String(sourceStateId) === String(focusStateId)) return;

        setDeleteConfirmationData({
            eventName,
            sourceStateId,
            stateName: supportingConfig?.StateConfig?.find(s => String(s.Key) === String(sourceStateId))?.Value || sourceStateId,
            type: 'sourceState'
        });
        setShowDeleteConfirmation(true);
    }, [supportingConfig, focusStateId]);

    const handleAddTargetState = useCallback((eventName) => {
        setAddTargetStateEventName(eventName);
        setShowAddTargetStateForm(true);
    }, []);

    const handleAddTargetStateSubmit = useCallback((newTargetState) => {
        if (!localWorkflow || !addTargetStateEventName) return;

        const updatedWorkflow = { ...localWorkflow };
        
        // Find the step for this event and add the new target state
        const updatedSteps = localWorkflow.Steps.map(step => {
            if (step.event === addTargetStateEventName) {
                const updatedStep = { ...step };
                
                // Ensure exitstate exists
                if (!updatedStep.exitstate) {
                    updatedStep.exitstate = {};
                } else {
                    updatedStep.exitstate = { ...updatedStep.exitstate };
                }
                
                // Handle default state
                if (newTargetState.isDefault) {
                    updatedStep.exitstate.default = newTargetState.newstate;
                } else {
                    // Ensure options array exists
                    if (!updatedStep.exitstate.options) {
                        updatedStep.exitstate.options = [];
                    } else {
                        updatedStep.exitstate.options = [...updatedStep.exitstate.options];
                    }
                    
                    // Add the new target state option (remove isDefault from the object)
                    const { isDefault, ...targetStateOption } = newTargetState;
                    updatedStep.exitstate.options.push(targetStateOption);
                }
                
                return updatedStep;
            }
            return step;
        });
        
        updatedWorkflow.Steps = updatedSteps;
        
        // Update local workflow and mark as having unsaved changes
        setLocalWorkflow(updatedWorkflow);
        setHasUnsavedChanges(true);
        
        // Close the form
        setShowAddTargetStateForm(false);
        setAddTargetStateEventName('');
    }, [localWorkflow, addTargetStateEventName]);

    const handleAddTargetStateCancel = useCallback(() => {
        setShowAddTargetStateForm(false);
        setAddTargetStateEventName('');
    }, []);

    const handleRemoveTargetState = useCallback((eventName, targetStateId) => {
        // Show confirmation panel
        setDeleteConfirmationData({
            eventName,
            targetStateId,
            stateName: supportingConfig?.StateConfig?.find(s => String(s.Key) === targetStateId)?.Value || targetStateId,
            type: 'targetState'
        });
        setShowDeleteConfirmation(true);
    }, [supportingConfig]);

    /**
     * Persists the edited workflow. The unsaved marker is only cleared once the backend has confirmed
     * the write, so a failed save leaves the edits in place rather than pretending they landed.
     */
    const handleSave = useCallback(() => {
        if (!onSaveConfiguration || !localWorkflow || isSaving) {
            return;
        }

        setIsSaving(true);

        Promise.resolve(onSaveConfiguration(localWorkflow))
            .then(() => {
                setHasUnsavedChanges(false);
            })
            .catch(() => {
                // The container surfaces the error; the edits stay so the user can retry.
            })
            .finally(() => {
                setIsSaving(false);
            });
    }, [localWorkflow, onSaveConfiguration, isSaving]);

    const handleCancel = useCallback(() => {
        setLocalWorkflow(workflow);
        setHasUnsavedChanges(false);
    }, [workflow]);

    // Top bar menu buttons. Keys double as DOM ids, which is what the end to end tests target.
    const menuButtons = useMemo(() => {
        const buttons = [
            {
                key: 'workflowdesigner-addstate',
                text: TranslateTag('@GenAdd@', language),
                icon: 'Add',
                primaryAction: true
            },
            {
                key: 'workflowdesigner-editstate',
                text: TranslateTag('@GenEdi@', language),
                icon: 'Edit',
                primaryAction: true
            },
            {
                key: 'workflowdesigner-deletestate',
                text: TranslateTag('@GenDel@', language),
                icon: 'Delete',
                primaryAction: true
            },
            {
                key: 'workflowdesigner-save',
                text: TranslateTag('@GenSav@', language),
                icon: 'Save',
                primaryAction: true
            },
            {
                key: 'workflowdesigner-cancel',
                text: TranslateTag('@GenCan@', language),
                icon: 'Cancel',
                primaryAction: false
            }
        ];

        return buttons;
    }, [language]);

    const handleButtonClick = useCallback((button) => {
        switch (button.key) {
            case 'workflowdesigner-addstate':
                break;
            case 'workflowdesigner-editstate':
                break;
            case 'workflowdesigner-deletestate':
                break;
            case 'workflowdesigner-save':
                handleSave();
                break;
            case 'workflowdesigner-cancel':
                handleCancel();
                break;
            default:
                break;
        }
    }, [handleSave, handleCancel]);


    return (
        <div className="workflow-designer" id="workflowdesigner">
            <div className="workflow-header">
                <h2>Workflow Designer{workflowName ? ` - ${workflowName}` : ''}</h2>
            </div>
            {hasUnsavedChanges && (
                <MessageBar id="workflowdesigner-unsaved-banner" messageBarType={MessageBarType.warning}>
                    You have unsaved changes. Don't forget to save your workflow.
                </MessageBar>
            )}
            <div className="workflow-topbar">
                <TopbarMenu
                    buttons={menuButtons}
                    clickButton={handleButtonClick}
                    language={language}
                    showFilterIcon={false}
                    showCardIcon={false}
                    suppressFarItems={true}
                />
            </div>
            <div className="workflow-filter">
                <Filter
                    filters={[stateFilter]}
                    click={handleFilterClick}
                    refresh={handleFilterRefresh}
                    clear={handleFilterClear}
                    language={language}
                    filterSearch={false}
                    dateSearch={false}
                />
            </div>


            <div className="workflow-canvas">
                <div className="swimlanes">
                    {events.map((event, eventIndex) => (
                        <div key={event.id} className={eventIndex % 2 === 0 ? 'even' : 'odd'}>
                            <EventPanel
                                event={event}
                                eventDomId={normaliseId(event.name)}
                                focusStateId={focusStateId}
                                onStateClick={handleStateClick}
                                onRemoveEvent={handleRemoveEvent}
                                onRemoveTargetState={handleRemoveTargetState}
                                onRemoveSourceState={handleRemoveSourceState}
                                onAddTargetState={handleAddTargetState}
                            />
                        </div>
                    ))}
                </div>
            </div>

            {/* Delete Confirmation Panel */}
            <Panel
                isOpen={showDeleteConfirmation}
                onDismiss={handleCancelDelete}
                type={PanelType.medium}
                headerText={deleteConfirmationData?.type === 'targetState' ? "Remove Target State" : "Remove Event from State"}
                closeButtonAriaLabel="Close"
            >
                {deleteConfirmationData && (
                    <div style={{ padding: '20px' }}>
                        {deleteConfirmationData.type === 'targetState' ? (
                            <p>
                                Are you sure you want to remove <strong>"{deleteConfirmationData.stateName}"</strong> as a target state for event <strong>"{deleteConfirmationData.eventName}"</strong>?
                            </p>
                        ) : (
                            <p>
                                Are you sure you want to remove the event <strong>"{deleteConfirmationData.eventName}"</strong> from state <strong>"{deleteConfirmationData.stateName}"</strong>?
                            </p>
                        )}
                        
                        <div style={{ marginTop: '20px', display: 'flex', gap: '10px' }}>
                            <PrimaryButton
                                id="workflowdesigner-delete-confirm"
                                text={deleteConfirmationData.type === 'targetState' ? "Remove Target State" : "Remove Event"}
                                onClick={handleConfirmDelete}
                                styles={{ root: { backgroundColor: '#d13438' } }}
                            />
                            <DefaultButton
                                id="workflowdesigner-delete-cancel"
                                text="Cancel"
                                onClick={handleCancelDelete}
                            />
                        </div>
                    </div>
                )}
            </Panel>

            {/* Add Target State Form */}
            <AddTargetStateForm
                isOpen={showAddTargetStateForm}
                onDismiss={handleAddTargetStateCancel}
                onSubmit={handleAddTargetStateSubmit}
                availableStates={states}
                eventName={addTargetStateEventName}
                existingTargetStates={events.find(e => e.name === addTargetStateEventName)?.targetStates || []}
                eventConfig={supportingConfig?.EventConfig}
                listConfig={supportingConfig?.ListConfig}
                language={language}
            />

        </div>
    );
};

const mapStateToProps = state => {
    return {
        language: state.config.language
    };
};

export default connect(mapStateToProps)(WorkflowDesigner);
