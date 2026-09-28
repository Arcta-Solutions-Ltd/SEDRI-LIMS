/**
 * Pure helpers that turn a raw JSON or XML document into the canonical mapping
 * tree used by the editor. Imported files describe the *shape* the user wants
 * (objects, arrays, leaf attributes) but do not carry any profile-field
 * bindings, so:
 *   - attribute nodes are produced WITHOUT a fieldKey/gridSubFieldId (unbound),
 *   - array nodes are produced WITHOUT an arrayType (untyped).
 * The user then binds fields and classifies arrays in the editor before saving.
 *
 * The canonical node shapes match useMappingConstraints.js:
 *   { kind: 'object', name, children: [] }
 *   { kind: 'array',  name, arrayType: '', children: [] }
 *   { kind: 'attribute', name }
 */

const ROOT_NAME = 'root';

const isPlainObject = (v) => v !== null && typeof v === 'object' && !Array.isArray(v);

/**
 * Convert a single JSON value into a canonical node.
 * @param {string} name - the key/name this value sits under.
 * @param {*} value - the JSON value.
 * @returns {object} canonical node.
 */
const convertJsonValue = (name, value) => {
    if (Array.isArray(value)) {
        return {
            kind: 'array',
            name,
            arrayType: '',
            children: convertJsonArrayItemChildren(value)
        };
    }
    if (isPlainObject(value)) {
        return {
            kind: 'object',
            name,
            children: Object.keys(value).map((k) => convertJsonValue(k, value[k]))
        };
    }
    // Scalar (string/number/boolean/null) -> unbound leaf attribute.
    return { kind: 'attribute', name };
};

/**
 * Derive the child nodes that represent a single item of a JSON array. Array
 * nodes in the canonical tree hold the *fields of one representative item*, not
 * a wrapper item node (see JsonStructureEditor/XmlStructureEditor previews).
 * @param {Array} arr
 * @returns {Array} child nodes.
 */
const convertJsonArrayItemChildren = (arr) => {
    if (!Array.isArray(arr) || arr.length === 0) return [];
    const sample = arr[0];
    if (isPlainObject(sample)) {
        return Object.keys(sample).map((k) => convertJsonValue(k, sample[k]));
    }
    if (Array.isArray(sample)) {
        return [convertJsonValue('item', sample)];
    }
    // Array of scalars -> a single unbound value attribute.
    return [{ kind: 'attribute', name: 'value' }];
};

/**
 * Parse a JSON document into the canonical tree. The root is always an object
 * node (the editor/validator require an object root).
 * @param {string} text
 * @returns {object} root object node.
 */
const parseJsonToStructure = (text) => {
    let parsed;
    try {
        parsed = JSON.parse(text);
    } catch (e) {
        throw new Error(`The file is not valid JSON: ${e.message}`);
    }

    if (Array.isArray(parsed)) {
        return {
            kind: 'object',
            name: ROOT_NAME,
            children: [convertJsonValue('items', parsed)]
        };
    }
    if (isPlainObject(parsed)) {
        return {
            kind: 'object',
            name: ROOT_NAME,
            children: Object.keys(parsed).map((k) => convertJsonValue(k, parsed[k]))
        };
    }
    // A bare scalar document.
    return {
        kind: 'object',
        name: ROOT_NAME,
        children: [{ kind: 'attribute', name: 'value' }]
    };
};

/**
 * Group an element's direct child elements by tag name, preserving first-seen
 * order.
 * @param {Element} el
 * @returns {Array<{ tag: string, elements: Element[] }>}
 */
const groupChildElements = (el) => {
    const order = [];
    const byTag = new Map();
    const children = el.children ? Array.from(el.children) : [];
    for (const child of children) {
        const tag = child.tagName;
        if (!byTag.has(tag)) {
            byTag.set(tag, []);
            order.push(tag);
        }
        byTag.get(tag).push(child);
    }
    return order.map((tag) => ({ tag, elements: byTag.get(tag) }));
};

/**
 * Convert a single XML element into a canonical node.
 * Elements with no child elements and no attributes become leaf attributes;
 * anything richer becomes an object node (with xml attributes surfaced as
 * unbound leaf attributes). Repeated sibling elements collapse into an array.
 * @param {Element} el
 * @returns {object} canonical node.
 */
const convertXmlElement = (el) => {
    const childGroups = groupChildElements(el);
    const attrs = el.attributes ? Array.from(el.attributes) : [];

    if (childGroups.length === 0 && attrs.length === 0) {
        return { kind: 'attribute', name: el.tagName };
    }

    const children = [];
    for (const attr of attrs) {
        children.push({ kind: 'attribute', name: attr.name });
    }
    for (const group of childGroups) {
        if (group.elements.length > 1) {
            children.push({
                kind: 'array',
                name: group.tag,
                arrayType: '',
                children: convertXmlArrayItemChildren(group.elements[0])
            });
        } else {
            children.push(convertXmlElement(group.elements[0]));
        }
    }
    return { kind: 'object', name: el.tagName, children };
};

/**
 * Derive the child nodes for one item of an XML array from the representative
 * (first) repeated element. Mirrors convertJsonArrayItemChildren: the array
 * holds the item's fields, not a wrapper node.
 * @param {Element} representative
 * @returns {Array} child nodes.
 */
const convertXmlArrayItemChildren = (representative) => {
    const node = convertXmlElement(representative);
    if (node.kind === 'object') return node.children;
    // A repeated leaf element -> a single unbound attribute for the item.
    return [node];
};

/**
 * Parse an XML document into the canonical tree. The root is always an object
 * node.
 * @param {string} text
 * @returns {object} root object node.
 */
const parseXmlToStructure = (text) => {
    if (typeof DOMParser === 'undefined') {
        throw new Error('XML import is not supported in this environment.');
    }
    const doc = new DOMParser().parseFromString(text, 'application/xml');
    const parseError = doc.getElementsByTagName('parsererror');
    if (parseError && parseError.length > 0) {
        const detail = (parseError[0].textContent || '').trim().split('\n')[0];
        throw new Error(`The file is not valid XML${detail ? `: ${detail}` : '.'}`);
    }
    const rootEl = doc.documentElement;
    if (!rootEl) {
        throw new Error('The XML file has no root element.');
    }
    const rootNode = convertXmlElement(rootEl);
    if (rootNode.kind === 'object') return rootNode;
    // Root element was a bare leaf; wrap it so the tree root stays an object.
    return { kind: 'object', name: ROOT_NAME, children: [rootNode] };
};

/**
 * Auto-detect the document format from its content.
 * @param {string} text - already-trimmed content.
 * @returns {'json'|'xml'}
 */
const detectFormat = (text) => (text.startsWith('<') ? 'xml' : 'json');

/**
 * Parse raw imported text into the canonical tree.
 * @param {string} text - the file contents or pasted text.
 * @param {'json'|'xml'|'auto'} [format='auto'] - explicit format, or auto-detect.
 * @returns {object} root object node.
 * @throws {Error} with a user-friendly message when the content can't be parsed.
 */
const parseStructure = (text, format = 'auto') => {
    const trimmed = (text || '').trim();
    if (trimmed.length === 0) {
        throw new Error('There is nothing to import. Paste some content or choose a file first.');
    }
    const chosen = (!format || format === 'auto') ? detectFormat(trimmed) : format;
    return chosen === 'xml' ? parseXmlToStructure(trimmed) : parseJsonToStructure(trimmed);
};

export {
    parseStructure,
    parseJsonToStructure,
    parseXmlToStructure,
    detectFormat
};
