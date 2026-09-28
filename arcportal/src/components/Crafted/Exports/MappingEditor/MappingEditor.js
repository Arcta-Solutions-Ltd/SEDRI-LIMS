import React, { useState, useEffect, useMemo, useRef } from 'react';
import { ChoiceGroup, DefaultButton, MessageBar, MessageBarType } from '@fluentui/react';
import TextDisplay from '../../../Forms/TextDisplay/TextDisplay';
import TranslateTag from '../../../../Utils/Local/TranslateTag';
import MappingTree from './MappingTree';
import JsonStructureEditor from './JsonStructureEditor';
import XmlStructureEditor from './XmlStructureEditor';
import AddAttributeDialog from './AddAttributeDialog';
import AddArrayDialog from './AddArrayDialog';
import ImportStructureDialog from './ImportStructureDialog';
import {
    ensureNodeIds,
    insertChild,
    removeNode,
    replaceNode,
    stripIds,
    findEnclosingGridArray,
    findEnclosingAstArray,
    buildDefaultStructure,
    getNonAstNonGridFieldOptions,
    getAstFieldOptions,
    collectUnmappedNodes
} from './useMappingConstraints';
import './MappingEditor.css';

/**
 * Crafted page entry point for the JSON/XML structural mapping editor.
 *
 * Loads the existing mapping (and the classified field options for the
 * profile) from the form's initial query payload (props.data), then renders a
 * two-pane layout:
 *  - Left pane: editable canonical tree (objects, arrays, attributes).
 *  - Right pane: live preview rendered as JSON or XML based on the format toggle.
 *
 * All state is mirrored back to the form via props.changeHandler so that the
 * standard form Save button posts a SaveExportProfileMappingRequest payload to
 * the saveexportprofilemapping event handler.
 *
 * @param {object} props
 * @param {object} props.data - initial query payload (ExportProfileMappingViewModel).
 * @param {function} props.changeHandler - form field change handler (id, value).
 * @param {Array}    props.language - language tag list.
 * @param {number}   props.id - export profile id (used as a fallback when payload doesn't contain it).
 */
// Locate the ExportProfileMappingViewModel object no matter how the crafted
// load pipeline hands it over. Depending on whether the response passes through
// the standard DataRetrievedHandler (which JSON.parses Crafted[].Contents) or a
// special-query panel path (which may not), props.data can arrive as any of:
//   - the view model object directly ({ ExportProfileId, Structure, FieldOptions, ... })
//   - an array of keyed contents ([{ Key: 'mapping', Value: <vm|string>, Index }])
//   - a single keyed-content object ({ Key: 'mapping', Value: <vm|string> })
//   - a Crafted wrapper ({ Crafted: [{ Contents: <string|array> }] })
//   - a JSON string of any of the above (Contents that was never parsed)
// This walks/unwraps up to a few levels until it finds the view model.
const extractMappingViewModel = (input) => {
    let cur = input;
    for (let depth = 0; depth < 8 && cur !== null && cur !== undefined; depth++) {
        if (typeof cur === 'string') {
            const s = cur.trim();
            if (!s) return null;
            try { cur = JSON.parse(s); } catch (e) { return null; }
            continue;
        }
        if (Array.isArray(cur)) {
            const item = cur.find((d) => {
                const k = d && (d.Key ?? d.key);
                return typeof k === 'string' && k.toLowerCase() === 'mapping';
            })
                || cur.find((d) => d && (d.Value !== undefined || d.value !== undefined))
                || cur[0];
            if (!item) return null;
            cur = (item.Value !== undefined) ? item.Value
                : (item.value !== undefined) ? item.value
                : item;
            continue;
        }
        if (typeof cur === 'object') {
            const looksLikeViewModel = Object.keys(cur).some((k) => {
                const kl = k.toLowerCase();
                return kl === 'fieldoptions' || kl === 'structure' || kl === 'exportprofileid';
            });
            if (looksLikeViewModel) return cur;

            const crafted = cur.Crafted ?? cur.crafted;
            if (Array.isArray(crafted) && crafted.length > 0) {
                cur = crafted[0].Contents ?? crafted[0].contents;
                continue;
            }

            const key = cur.Key ?? cur.key;
            const val = cur.Value ?? cur.value;
            if (typeof key === 'string' && val !== undefined) {
                cur = val;
                continue;
            }

            return cur;
        }
        return null;
    }
    return (cur && typeof cur === 'object' && !Array.isArray(cur)) ? cur : null;
};

const MappingEditor = (props) => {
    const initialPayload = useMemo(() => {
        const vm = extractMappingViewModel(props.data);
        if (!vm) return {};
        const norm = {};
        Object.keys(vm).forEach((k) => { norm[k.toLowerCase()] = vm[k]; });
        return norm;
    }, [props.data]);

    const initialFormat = useMemo(() => {
        const f = (initialPayload.format || 'json').toString().toLowerCase();
        return f === 'xml' ? 'xml' : 'json';
    }, [initialPayload]);

    const initialStructure = useMemo(() => {
        let raw = initialPayload.structure;
        if (typeof raw === 'string' && raw.trim().length > 0) {
            try {
                return ensureNodeIds(JSON.parse(raw));
            } catch (e) {
                // Fall through to default structure when the persisted structure can't be parsed.
            }
        }
        if (raw && typeof raw === 'object') {
            return ensureNodeIds(raw);
        }
        return ensureNodeIds(buildDefaultStructure(props.language));
    }, [initialPayload, props.language]);

    const fieldOptions = useMemo(() => Array.isArray(initialPayload.fieldoptions) ? initialPayload.fieldoptions : [], [initialPayload]);
    const hasPatient = !!initialPayload.haspatientfields;
    const hasCulture = !!initialPayload.hasculturefields;
    const hasAst = useMemo(() => getAstFieldOptions(fieldOptions).length > 0, [fieldOptions]);
    const exportProfileId = useMemo(() => {
        const explicit = initialPayload.exportprofileid;
        if (explicit !== undefined && explicit !== null && explicit !== '') return explicit;
        return props.id;
    }, [initialPayload, props.id]);

    const [format, setFormat] = useState(initialFormat);
    const [structure, setStructure] = useState(initialStructure);

    const [addAttrTarget, setAddAttrTarget] = useState(null);
    const [addArrayTarget, setAddArrayTarget] = useState(null);
    const [editAttrTarget, setEditAttrTarget] = useState(null);
    const [editArrayTarget, setEditArrayTarget] = useState(null);
    const [importVisible, setImportVisible] = useState(false);

    const initialPushed = useRef(false);

    const pushPayloadUpstream = (nextFormat, nextStructure) => {
        if (!props.changeHandler) return;
        const payload = {
            ExportProfileId: exportProfileId !== undefined && exportProfileId !== null && exportProfileId !== ''
                ? Number(exportProfileId) || exportProfileId
                : 0,
            Format: nextFormat,
            Structure: JSON.stringify(stripIds(nextStructure)),
            // Echo the read-only view-model fields so that when this payload is
            // round-tripped back through the crafted page Contents (e.g. after a
            // fullscreen remount) the editor can still resolve its field options.
            // These extra fields are ignored by the backend save deserializer.
            FieldOptions: fieldOptions,
            HasPatientFields: hasPatient,
            HasCultureFields: hasCulture
        };
        const change = { Key: 'mapping', value: payload };
        props.changeHandler('multiplechanges', [{ key: 'mapping', value: change }]);
    };

    useEffect(() => {
        if (!props.changeHandler) return;
        if (initialPushed.current) return;
        initialPushed.current = true;
        pushPayloadUpstream(format, structure);
    }, []);

    const pushStructureUpstream = (next) => {
        pushPayloadUpstream(format, next);
    };

    const onFormatChange = (_, opt) => {
        const next = opt?.key === 'xml' ? 'xml' : 'json';
        setFormat(next);
        pushPayloadUpstream(next, structure);
    };

    const onAddAttribute = (parentNode) => {
        setAddAttrTarget(parentNode);
    };

    const onAddArray = (parentNode) => {
        setAddArrayTarget(parentNode);
    };

    const onRemove = (node) => {
        const next = removeNode(structure, node.__id);
        setStructure(next);
        pushStructureUpstream(next);
    };

    const handleAddAttributeConfirm = (name, fieldKey, gridSubFieldId, uniqueReference) => {
        if (!addAttrTarget) return;
        const child = ensureNodeIds({
            kind: 'attribute',
            name,
            ...(fieldKey ? { fieldKey } : {}),
            ...(gridSubFieldId ? { gridSubFieldId } : {}),
            ...(uniqueReference === 'Yes' ? { uniqueReference: 'Yes' } : {})
        });
        const next = insertChild(structure, addAttrTarget.__id, child);
        setStructure(next);
        pushStructureUpstream(next);
        setAddAttrTarget(null);
    };

    const handleAddArrayConfirm = (node) => {
        if (!addArrayTarget) return;
        const child = ensureNodeIds(node);
        const next = insertChild(structure, addArrayTarget.__id, child);
        setStructure(next);
        pushStructureUpstream(next);
        setAddArrayTarget(null);
    };

    const onEditAttribute = (node) => {
        setEditAttrTarget(node);
    };

    const onEditArray = (node) => {
        setEditArrayTarget(node);
    };

    const handleEditAttributeConfirm = (name, fieldKey, gridSubFieldId, uniqueReference) => {
        if (!editAttrTarget) return;
        const replacement = ensureNodeIds({
            kind: 'attribute',
            name,
            ...(fieldKey ? { fieldKey } : {}),
            ...(gridSubFieldId ? { gridSubFieldId } : {}),
            ...(uniqueReference === 'Yes' ? { uniqueReference: 'Yes' } : {})
        });
        const next = replaceNode(structure, editAttrTarget.__id, replacement);
        setStructure(next);
        pushStructureUpstream(next);
        setEditAttrTarget(null);
    };

    const handleEditArrayConfirm = (node) => {
        if (!editArrayTarget) return;
        const replacement = ensureNodeIds(node);
        const next = replaceNode(structure, editArrayTarget.__id, replacement);
        setStructure(next);
        pushStructureUpstream(next);
        setEditArrayTarget(null);
    };

    const handleImportConfirm = (tree) => {
        const next = ensureNodeIds(tree);
        setStructure(next);
        pushStructureUpstream(next);
        setImportVisible(false);
    };

    // Find the immediate parent of a node id (used to constrain array classification).
    const findParentNode = (root, childId) => {
        if (!root || !childId || !Array.isArray(root.children)) return null;
        if (root.children.some((c) => c.__id === childId)) return root;
        for (const c of root.children) {
            const p = findParentNode(c, childId);
            if (p) return p;
        }
        return null;
    };

    // Compute the context-aware attribute field options / grid sub-fields for a
    // target node. Works for both an insertion parent (add) and an existing
    // attribute being edited, because findEnclosing* include the node itself.
    const attributeContextFor = (node) => {
        if (!node) return { options: getNonAstNonGridFieldOptions(fieldOptions), gridSubFields: [] };
        const grid = findEnclosingGridArray(structure, node.__id);
        if (grid) {
            const opt = fieldOptions.find((o) => (o.Key || o.key) === grid.gridFieldKey);
            const subFields = opt ? (opt.GridSubFields || opt.gridSubFields || []) : [];
            return { options: [], gridSubFields: subFields };
        }
        const ast = findEnclosingAstArray(structure, node.__id);
        if (ast) return { options: getAstFieldOptions(fieldOptions), gridSubFields: [] };
        return { options: getNonAstNonGridFieldOptions(fieldOptions), gridSubFields: [] };
    };

    const addAttrContext = useMemo(() => attributeContextFor(addAttrTarget), [addAttrTarget, structure, fieldOptions]);
    const editAttrContext = useMemo(() => attributeContextFor(editAttrTarget), [editAttrTarget, structure, fieldOptions]);
    const editArrayParent = useMemo(
        () => (editArrayTarget ? findParentNode(structure, editArrayTarget.__id) : null),
        [editArrayTarget, structure]
    );
    const addAttrGridFieldKey = useMemo(
        () => (addAttrTarget ? findEnclosingGridArray(structure, addAttrTarget.__id)?.gridFieldKey : ''),
        [addAttrTarget, structure]
    );
    const editAttrGridFieldKey = useMemo(
        () => (editAttrTarget ? findEnclosingGridArray(structure, editAttrTarget.__id)?.gridFieldKey : ''),
        [editAttrTarget, structure]
    );

    const unmapped = useMemo(() => collectUnmappedNodes(structure), [structure]);
    const unmappedCount = unmapped.unboundAttributes.length + unmapped.untypedArrays.length;

    const hasExistingContent = Array.isArray(structure?.children) && structure.children.length > 0;

    const empty = !Array.isArray(fieldOptions) || fieldOptions.length === 0;

    const formatOptions = [
        { key: 'json', text: TranslateTag('@ExpProMapJson@', props.language) || 'JSON' },
        { key: 'xml', text: TranslateTag('@ExpProMapXml@', props.language) || 'XML' }
    ];

    return (
        <div className="app-crafted-content">
            <div className="app-crafted-title">{props.config?.PageTitle}</div>
            <div className="app-crafted-headertext">
                <TextDisplay text={props.config?.Text}></TextDisplay>
            </div>
            <div className="mappingeditor-root">
                <div className="mappingeditor-toolbar">
                    <ChoiceGroup
                        label={TranslateTag('@ExpProMapFmt@', props.language) || 'Output format'}
                        options={formatOptions}
                        selectedKey={format}
                        onChange={onFormatChange}
                        styles={{ flexContainer: { display: 'flex', gap: 10 } }}
                    />
                    <DefaultButton
                        id="mapping-import-open"
                        className="mappingeditor-import-button"
                        iconProps={{ iconName: 'Upload' }}
                        text={TranslateTag('@ExpProMapImport@', props.language) || 'Import structure'}
                        onClick={() => setImportVisible(true)}
                    />
                </div>
                {unmappedCount > 0 && (
                    <MessageBar
                        className="mappingeditor-unmapped-banner"
                        messageBarType={MessageBarType.info}
                        isMultiline={true}
                    >
                        {(TranslateTag('@ExpProMapUnmappedWarn@', props.language)
                            || '{attrs} attribute(s) and {arrays} array(s) have no field/type and will be ignored when this mapping is used. You can bind them with the edit action on each highlighted node, or save the mapping as-is.')
                            .replace('{attrs}', unmapped.unboundAttributes.length)
                            .replace('{arrays}', unmapped.untypedArrays.length)}
                    </MessageBar>
                )}
                {(empty && !hasExistingContent) ? (
                    <div className="mappingeditor-empty">
                        {TranslateTag('@ExpProMapNoFields@', props.language)
                            || 'There are no fields in the export profile yet. Add fields to the profile before building a mapping.'}
                    </div>
                ) : (
                    <>
                        {empty && (
                            <MessageBar messageBarType={MessageBarType.info} isMultiline={true} styles={{ root: { marginBottom: 12 } }}>
                                {TranslateTag('@ExpProMapNoFieldsBind@', props.language)
                                    || 'This export profile has no fields yet, so the imported attributes cannot be bound to anything. Add fields to the profile, then use the edit (pencil) action on each node to map them.'}
                            </MessageBar>
                        )}
                        <div className="mappingeditor-body">
                            <div className="mappingeditor-tree-pane">
                                <MappingTree
                                    node={structure}
                                    isRoot={true}
                                    fieldOptions={fieldOptions}
                                    language={props.language}
                                    onAddAttribute={onAddAttribute}
                                    onAddArray={onAddArray}
                                    onEditAttribute={onEditAttribute}
                                    onEditArray={onEditArray}
                                    onRemove={onRemove}
                                />
                            </div>
                            <div className="mappingeditor-preview-pane">
                                <h3>{TranslateTag('@ExpProMapPreview@', props.language) || 'Preview'}</h3>
                                {format === 'xml'
                                    ? <XmlStructureEditor structure={structure} fieldOptions={fieldOptions} />
                                    : <JsonStructureEditor structure={structure} fieldOptions={fieldOptions} />}
                            </div>
                        </div>
                    </>
                )}
            </div>
            <AddAttributeDialog
                visible={!!addAttrTarget}
                structure={structure}
                allFieldOptions={fieldOptions}
                gridFieldKey={addAttrGridFieldKey}
                fieldOptions={addAttrContext.options}
                gridSubFields={addAttrContext.gridSubFields}
                language={props.language}
                onConfirm={handleAddAttributeConfirm}
                onDismiss={() => setAddAttrTarget(null)}
            />
            <AddAttributeDialog
                visible={!!editAttrTarget}
                isEdit={true}
                structure={structure}
                excludeNodeId={editAttrTarget?.__id}
                allFieldOptions={fieldOptions}
                gridFieldKey={editAttrGridFieldKey}
                initialName={editAttrTarget?.name}
                initialFieldKey={editAttrTarget?.fieldKey}
                initialGridSubFieldId={editAttrTarget?.gridSubFieldId}
                initialUniqueReference={editAttrTarget?.uniqueReference}
                fieldOptions={editAttrContext.options}
                gridSubFields={editAttrContext.gridSubFields}
                language={props.language}
                onConfirm={handleEditAttributeConfirm}
                onDismiss={() => setEditAttrTarget(null)}
            />
            <AddArrayDialog
                visible={!!addArrayTarget}
                fieldOptions={fieldOptions}
                hasPatient={hasPatient}
                hasCulture={hasCulture}
                hasAst={hasAst}
                parentNode={addArrayTarget}
                language={props.language}
                onConfirm={handleAddArrayConfirm}
                onDismiss={() => setAddArrayTarget(null)}
            />
            <AddArrayDialog
                visible={!!editArrayTarget}
                isEdit={true}
                initialName={editArrayTarget?.name}
                initialArrayType={editArrayTarget?.arrayType}
                initialGridFieldKey={editArrayTarget?.gridFieldKey}
                existingChildren={editArrayTarget?.children}
                fieldOptions={fieldOptions}
                hasPatient={hasPatient}
                hasCulture={hasCulture}
                hasAst={hasAst}
                parentNode={editArrayParent}
                language={props.language}
                onConfirm={handleEditArrayConfirm}
                onDismiss={() => setEditArrayTarget(null)}
            />
            <ImportStructureDialog
                visible={importVisible}
                hasExistingContent={hasExistingContent}
                language={props.language}
                onConfirm={handleImportConfirm}
                onDismiss={() => setImportVisible(false)}
            />
        </div>
    );
};

export default MappingEditor;
