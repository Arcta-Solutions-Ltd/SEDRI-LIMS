import React, { useMemo } from 'react';
import { stripIds } from './useMappingConstraints';

/**
 * XML-flavoured live preview of the canonical structure tree.
 *
 * Object nodes render as element wrappers, array nodes render as a repeating
 * element (with an inferred singular item element name), attribute nodes render
 * as a text-content placeholder showing the bound field. The output is plain
 * text rendered inside a <pre> so the angle brackets are visible as literals.
 *
 * @param {object} props
 * @param {object} props.structure - canonical tree.
 * @param {Array}  props.fieldOptions - used to render readable placeholder strings.
 */
const XmlStructureEditor = (props) => {
    const fieldOptions = props.fieldOptions || [];
    const findFieldText = (key) => {
        const opt = fieldOptions.find((o) => (o.Key || o.key) === key);
        return opt ? (opt.Text || opt.text || opt.FieldId || opt.fieldId || key) : key;
    };

    const sanitiseTag = (name) => {
        if (!name) return 'item';
        const cleaned = String(name).replace(/[^A-Za-z0-9_\-]/g, '_');
        if (/^[0-9]/.test(cleaned)) return `_${cleaned}`;
        return cleaned || 'item';
    };

    const inferItemName = (node) => {
        const base = sanitiseTag(node.name || 'item');
        if (base.toLowerCase().endsWith('s') && base.length > 1) return base.slice(0, -1);
        return `${base}Item`;
    };

    const buildXml = useMemo(() => {
        const recurse = (node, indent) => {
            if (!node) return '';
            const pad = '  '.repeat(indent);
            if (node.kind === 'attribute') {
                const tag = sanitiseTag(node.name);
                let inner;
                if (node.gridSubFieldId) {
                    inner = `{${node.gridSubFieldId}}`;
                } else if (!node.fieldKey || !String(node.fieldKey).trim()) {
                    inner = '{unmapped}';
                } else {
                    inner = `{${findFieldText(node.fieldKey)}}`;
                }
                return `${pad}<${tag}>${inner}</${tag}>`;
            }
            if (node.kind === 'array') {
                const tag = sanitiseTag(node.name);
                const itemTag = sanitiseTag(inferItemName(node));
                const childXml = (node.children || []).map((c) => recurse(c, indent + 2)).join('\n');
                if (!childXml) {
                    return `${pad}<${tag}>\n${pad}  <${itemTag}/>\n${pad}</${tag}>`;
                }
                return `${pad}<${tag}>\n${pad}  <${itemTag}>\n${childXml}\n${pad}  </${itemTag}>\n${pad}</${tag}>`;
            }
            const tag = sanitiseTag(node.name || 'root');
            const childXml = (node.children || []).map((c) => recurse(c, indent + 1)).join('\n');
            if (!childXml) return `${pad}<${tag}/>`;
            return `${pad}<${tag}>\n${childXml}\n${pad}</${tag}>`;
        };
        return recurse;
    }, [fieldOptions]);

    const xml = props.structure ? buildXml(stripIds(props.structure), 0) : '<root/>';

    return (
        <pre className="mappingeditor-preview-xml">{xml}</pre>
    );
};

export default XmlStructureEditor;
