import React, { useMemo } from 'react';
import { stripIds } from './useMappingConstraints';

/**
 * JSON-flavoured live preview of the canonical structure tree.
 * Object nodes become JSON objects, array nodes become JSON arrays,
 * attribute nodes become "key": "<placeholder>" entries.
 *
 * @param {object} props
 * @param {object} props.structure - canonical tree.
 * @param {Array}  props.fieldOptions - used to render readable placeholder strings.
 */
const JsonStructureEditor = (props) => {
    const fieldOptions = props.fieldOptions || [];

    const findFieldText = (key) => {
        const opt = fieldOptions.find((o) => (o.Key || o.key) === key);
        return opt ? (opt.Text || opt.text || opt.FieldId || opt.fieldId || key) : key;
    };

    const buildPreview = useMemo(() => (node) => {
        if (!node) return null;
        if (node.kind === 'attribute') {
            if (node.gridSubFieldId) {
                return `<${node.gridSubFieldId}>`;
            }
            if (!node.fieldKey || !String(node.fieldKey).trim()) {
                return '<unmapped>';
            }
            return `<${findFieldText(node.fieldKey)}>`;
        }
        if (node.kind === 'array') {
            const sample = (node.children || []).reduce((acc, c) => {
                if (c.kind === 'attribute') {
                    acc[c.name] = buildPreview(c);
                } else {
                    acc[c.name] = buildPreview(c);
                }
                return acc;
            }, {});
            return [sample];
        }
        const obj = {};
        for (const c of (node.children || [])) {
            obj[c.name] = buildPreview(c);
        }
        return obj;
    }, [props.structure, fieldOptions]);

    const previewObject = buildPreview(props.structure ? stripIds(props.structure) : null);
    const previewText = previewObject == null ? '{}' : JSON.stringify(previewObject, null, 2);

    return (
        <pre className="mappingeditor-preview-json">{previewText}</pre>
    );
};

export default JsonStructureEditor;
