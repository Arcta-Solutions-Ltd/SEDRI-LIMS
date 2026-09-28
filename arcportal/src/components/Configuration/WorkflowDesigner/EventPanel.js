import React from 'react';
import { IconButton, DefaultButton } from '@fluentui/react';

/**
 * One swimlane: the states that can raise an event, the event itself, and the states it can lead to.
 *
 * Every DOM id is built from the event's normalised name and the state's listitem id, so the ids stay
 * stable when a state is renamed or translated.
 * @param {object} props - Component props.
 * @param {object} props.event - The lane's event, with its source and target states.
 * @param {string} props.eventDomId - The event name normalised for use inside a DOM id.
 * @param {string} props.focusStateId - The listitem id of the state currently in focus.
 * @param {Function} props.onStateClick - Called with a state id to move the focus to that state.
 * @param {Function} props.onRemoveEvent - Called with the event name and the focused state id.
 * @param {Function} props.onRemoveTargetState - Called with the event name and a target state id.
 * @param {Function} props.onRemoveSourceState - Called with the event name and a source state id.
 * @param {Function} props.onAddTargetState - Called with the event name to open the add target form.
 * @returns {JSX.Element} The swimlane.
 */
const EventPanel = ({ 
    event, 
    eventDomId,
    focusStateId, 
    onStateClick, 
    onRemoveEvent, 
    onRemoveTargetState,
    onRemoveSourceState,
    onAddTargetState
}) => {
    const laneId = `workflowdesigner-event-${eventDomId ?? ''}`;

    return (
        <div className="swimlane" id={laneId}>
            <div className="source-states-column">
                <div className="column-header">Source States</div>
                {event.sourceStates.map((state, stateIndex) => (
                    <div 
                        key={state.id} 
                        id={`${laneId}-source-${state.id}`}
                        className={`source-state-node ${state.isCurrentState ? 'current-state' : ''}`}
                        onClick={() => onStateClick(state.id)}
                        title={`Click to focus on state ${state.id}`}
                    >
                        <div className="state-content">
                            <div className="state-label">{state.name}</div>
                            {state.isCurrentState && <div className="current-badge">Current</div>}
                        </div>
                        {!state.isCurrentState && (
                            <IconButton
                                id={`${laneId}-source-${state.id}-remove`}
                                iconProps={{ iconName: 'Delete' }}
                                onClick={(e) => {
                                    e.stopPropagation();
                                    onRemoveSourceState(event.name, state.id);
                                }}
                                title={`Remove ${state.name} as a source state for ${event.name}`}
                                styles={{ 
                                    root: { 
                                        color: '#d13438',
                                        padding: '4px',
                                        minWidth: 'auto'
                                    },
                                    rootHovered: {
                                        backgroundColor: '#f8f8f8'
                                    }
                                }}
                            />
                        )}
                    </div>
                ))}
            </div>
            
            <div className="event-column">
                <div className="event-node">
                    <div className="event-content">
                        <div className="event-label">{event.name}</div>
                        <IconButton
                            id={`${laneId}-remove`}
                            iconProps={{ iconName: 'Delete' }}
                            onClick={(e) => {
                                e.stopPropagation();
                                onRemoveEvent(event.name, focusStateId);
                            }}
                            title={`Remove ${event.name} from state ${focusStateId}`}
                            styles={{ 
                                root: { 
                                    color: '#d13438',
                                    padding: '4px',
                                    minWidth: 'auto'
                                },
                                rootHovered: {
                                    backgroundColor: '#f8f8f8'
                                }
                            }}
                        />
                    </div>
                </div>
            </div>
            
            <div className="target-states-column">
                <div className="column-header">Target States</div>
                {event.targetStates.map((state, stateIndex) => (
                    <div 
                        key={state.id} 
                        id={`${laneId}-target-${state.id}`}
                        className="target-state-node"
                        onClick={() => onStateClick(state.id)}
                        title={`Click to focus on state ${state.id}`}
                    >
                        <div className="state-content">
                            <div className="state-label">{state.name}</div>
                            <div className="state-badge">{state.availableEvents} events</div>
                        </div>
                        <IconButton
                            id={`${laneId}-target-${state.id}-remove`}
                            iconProps={{ iconName: 'Delete' }}
                            onClick={(e) => {
                                e.stopPropagation();
                                onRemoveTargetState(event.name, state.id);
                            }}
                            title={`Remove ${state.name} as target state for ${event.name}`}
                            styles={{ 
                                root: { 
                                    color: '#d13438',
                                    padding: '4px',
                                    minWidth: 'auto'
                                },
                                rootHovered: {
                                    backgroundColor: '#f8f8f8'
                                }
                            }}
                        />
                    </div>
                ))}
                <DefaultButton
                    id={`${laneId}-addtarget`}
                    text="Add Target State"
                    iconProps={{ iconName: 'Add' }}
                    onClick={() => onAddTargetState(event.name)}
                    styles={{ 
                        root: { 
                            marginTop: '8px',
                            width: '100%',
                            backgroundColor: '#f3f2f1',
                            borderColor: '#8a8886'
                        },
                        rootHovered: {
                            backgroundColor: '#edebe9'
                        }
                    }}
                />
            </div>
        </div>
    );
};

export default EventPanel;
