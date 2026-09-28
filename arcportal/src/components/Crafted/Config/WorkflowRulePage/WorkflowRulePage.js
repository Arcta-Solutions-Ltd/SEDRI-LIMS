import React, { useState, useEffect } from 'react';
import TextDisplay from '../../../Forms/TextDisplay/TextDisplay';
import SingleLineField from '../../../Forms/SingleLineField/SingleLineField';
import TranslateTag from '../../../../Utils/Local/TranslateTag';
import { PostList } from '../../../../Data/Post';
import WorkflowEntrySelector from '../WorkflowEntrySelector/WorkflowEntrySelector';
import { Separator } from '@fluentui/react';

const WorkflowRulePage = (props) => {
    const [eventOptions, setEventOptions] = useState([]);
    const [stateOptions, setStateOptions] = useState([]);

    var eventFieldValue = props.config.Columns[0].FormGroups[0].Fields[0].value;
    eventFieldValue =
        eventFieldValue === undefined
            ? undefined
            : eventFieldValue.toLowerCase();
    var entryStatesFieldValue =
        props.config.Columns[0].FormGroups[0].Fields[1].value;
    var defaultExitStateFieldValue =
        props.config.Columns[0].FormGroups[0].Fields[2].value;
    var workflowValue = props.config.Columns[0].FormGroups[1].Fields[0].value;
    if (workflowValue !== undefined) {
        workflowValue = workflowValue.WorkflowRuleGrid;
    }

    const eventFieldConfig = {
        Id: 'EventField',
        Type: 'combobox',
        Label: TranslateTag('@GenEve@', props.language),
        Required: true,
        Options: eventOptions,
        Placeholder: TranslateTag('@GenSelO@', props.language),
        value: eventFieldValue,
    };
    const entryStatesFieldConfig = {
        Id: 'EntryStates',
        Type: 'combobox',
        Label: TranslateTag('@ConEnt@', props.language),
        Required: true,
        Options: stateOptions,
        Placeholder: TranslateTag('@ConSelE@', props.language),
        value: entryStatesFieldValue,
        MultiSelect: true,
    };
    const defaultExitStateConfig = {
        Id: 'DefaultExitStates',
        Type: 'combobox',
        Label: TranslateTag('@ConDefC@', props.language),
        Options: stateOptions,
        Placeholder: TranslateTag('@ConSelE@', props.language),
        value: defaultExitStateFieldValue,
    };

    const workflowRuleGridConfig = {
        Id: 'WorkflowRuleGrid',
        Type: 'crafted',
        GridFields: props.config.Columns[0].FormGroups[1].Fields[0].GridFields,
        value: workflowValue,
        Label: props.config.Columns[0].FormGroups[1].Fields[0].Label,
    };

    useEffect(() => {
        const eventListRetrieved = (data) => {
            setEventOptions(
                data[0].options.map((option) => {
                    return { key: option.key.toLowerCase(), text: option.text };
                })
            );
        };

        runEventListQuery(eventListRetrieved, errorWhenRetrievingData);
        runStateListQuery(stateListRetrieved, errorWhenRetrievingData);
    }, []);

    const pageCss = props.fullScreen
        ? 'app-crafted-fullscreencontent'
        : 'app-crafted-content';

    const eventChangeHandler = (event, value) => {
        props.changeHandler('EventField', value);
    };

    const entryStatesChangeHandler = (event, value) => {
        props.changeHandler('EntryStates', value);
    };

    const workflowRuleChangeHandler = (event, value) => {
        props.changeHandler('WorkflowRuleGrid', { WorkflowRuleGrid: value });
    };

    const defaultChangeHandler = (event, value) => {
        props.changeHandler('DefaultExitStates', value);
    };

    const errorWhenRetrievingData = () => {};

    const runEventListQuery = (eventListRetrieved, errorWhenRetrievingData) => {
        PostList(
            [{ Name: 'specimenevent', IncludeFixed: true, Translate: true }],
            eventListRetrieved,
            errorWhenRetrievingData
        );
    };

    const stateListRetrieved = (data) => {
        setStateOptions(data[0].options);
    };

    const runStateListQuery = (stateListRetrieved, errorWhenRetrievingData) => {
        PostList(
            [
                {
                    Name: 'specimenworkflowitems',
                    IncludeFixed: true,
                    Translate: true,
                },
            ],
            stateListRetrieved,
            errorWhenRetrievingData
        );
    };

    var gridComponent = null;
    if (eventFieldValue !== undefined) {
        gridComponent = (
            <div>
                <Separator></Separator>
                <WorkflowEntrySelector
                    config={workflowRuleGridConfig}
                    data={props.data}
                    changeHandler={workflowRuleChangeHandler}
                    event={eventFieldValue}
                    language={props.language}
                ></WorkflowEntrySelector>
            </div>
        );
    }

    return (
        <div className={pageCss}>
            <div className="app-crafted-title">{props.config.PageTitle}</div>
            <div className="app-crafted-headertext">
                <TextDisplay text={props.config.Text}></TextDisplay>
            </div>
            <div className="organismsearch-formcolumn">
                <SingleLineField
                    key="event"
                    config={eventFieldConfig}
                    changeHandler={eventChangeHandler}
                ></SingleLineField>
                <SingleLineField
                    key="entrystates"
                    config={entryStatesFieldConfig}
                    changeHandler={entryStatesChangeHandler}
                ></SingleLineField>
                <SingleLineField
                    key="defaultexiststate"
                    config={defaultExitStateConfig}
                    changeHandler={defaultChangeHandler}
                ></SingleLineField>
                {gridComponent}
            </div>
        </div>
    );
};

export default WorkflowRulePage;
