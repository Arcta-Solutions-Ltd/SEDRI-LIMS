import React, { useCallback, useEffect, useState } from 'react';
import WorkflowDesigner from './WorkflowDesigner';
import Post from '../../../Data/Post';
import './WorkflowDesigner.css';

/**
 * Splits the composite record identifier the list view supplies into its id and name parts.
 *
 * The list rows use the same `id|name` convention the report designer uses, but a workflow row may
 * arrive with only a numeric id, so both shapes are handled.
 * @param {object} config - The record the list view passed in.
 * @returns {{id: string, name: string}} The configs identity and configuration name of the workflow.
 */
const readWorkflowReference = (config) => {
    const rawId = config?.id ?? config?.Id;

    if (rawId === undefined || rawId === null) {
        return { id: '', name: config?.Name ?? config?.name ?? '' };
    }

    const [id, nameFromId] = String(rawId).split('|');

    return { id: id ?? '', name: nameFromId ?? config?.Name ?? config?.name ?? '' };
};

/**
 * Loads one workflow from the configs table, hands it to the designer, and writes the designer's
 * edits back.
 *
 * The container owns everything that touches the API so the designer only ever deals with the
 * workflow document it was given. The document is round-tripped in the shape it was stored in, so a
 * save cannot quietly rewrite parts of the workflow the designer does not edit.
 * @param {object} props - Component props.
 * @param {object} props.config - The workflow record selected in the list view.
 * @returns {JSX.Element} The workflow designer, wrapped in its loading and error states.
 */
const WorkflowContainer = (props) => {
    const [workflow, setWorkflow] = useState(null);
    const [supportingConfig, setSupportingConfig] = useState(null);
    const [configId, setConfigId] = useState(0);
    const [configName, setConfigName] = useState('');
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(null);

    const reference = readWorkflowReference(props.config);

    const fetchWorkflowConfig = useCallback(() => {
        const { id, name } = readWorkflowReference(props.config);

        if (!id) {
            setError('No workflow was selected, so there is nothing to edit.');
            return;
        }

        setIsLoading(true);
        setError(null);

        const successFunction = (data) => {
            const parsed = typeof data === 'string' ? JSON.parse(data) : data;

            setWorkflow(parsed?.Workflow ?? null);
            setSupportingConfig(parsed?.SupportingConfig ?? null);
            setConfigId(parsed?.ConfigId ?? 0);
            setConfigName(parsed?.ConfigName ?? name);
            setIsLoading(false);
        };

        const errorFunction = (fetchError) => {
            setError(fetchError.message || 'Failed to fetch the workflow configuration');
            setIsLoading(false);
        };

        Post('config/getworkflowdesignerconfig', { id, name }, successFunction, errorFunction);
    }, [props.config]);

    useEffect(() => {
        fetchWorkflowConfig();
    }, [fetchWorkflowConfig]);

    /**
     * Writes the edited workflow back and reloads it so the designer shows what was actually stored.
     * @param {object} editedWorkflow - The workflow document as edited by the designer.
     * @returns {Promise<object|null>} The parsed save result, or null when it could not be read.
     */
    const handleSaveConfiguration = useCallback((editedWorkflow) => {
        return new Promise((resolve, reject) => {
            if (!configId) {
                const message = 'The workflow was not loaded with a configuration id, so it cannot be saved.';
                setError(message);
                reject(new Error(message));
                return;
            }

            const successFunction = (data) => {
                let saveResult = null;
                try {
                    saveResult = typeof data === 'string' ? JSON.parse(data) : data;
                } catch (parseError) {
                    saveResult = null;
                }

                fetchWorkflowConfig();
                resolve(saveResult);
            };

            const errorFunction = (saveError) => {
                setError(saveError.message || 'Failed to save the workflow configuration');
                reject(saveError);
            };

            Post(
                'config/saveworkflowdesignerconfig',
                { ConfigId: configId, ConfigName: configName, Workflow: editedWorkflow },
                successFunction,
                errorFunction
            );
        });
    }, [configId, configName, fetchWorkflowConfig]);

    return (
        <div className="workflowdesigner-container" id="workflowdesigner-container">
            {isLoading && <div id="workflowdesigner-loading">Loading workflow configuration...</div>}
            {error && <div id="workflowdesigner-error" style={{ color: 'red' }}>Error: {error}</div>}
            <WorkflowDesigner
                workflow={workflow}
                supportingConfig={supportingConfig}
                workflowName={configName || reference.name}
                onSaveConfiguration={handleSaveConfiguration}
            />
        </div>
    );
};

export default WorkflowContainer;
