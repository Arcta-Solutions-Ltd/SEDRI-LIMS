/**
 * Pure helpers shared by the JSON and XML mapping editors so that constraint
 * decisions (which array types are valid, which fields can be used, etc.) are
 * enforced identically in both editors and on the server.
 *
 * The canonical structure tree shape is:
 *   { kind: 'object', name, children: [] }
 *   { kind: 'array',  name, arrayType: 'specimens'|'cultures'|'grid'|'ast', gridFieldKey?, children: [] }
 *   { kind: 'attribute', name, fieldKey?, gridSubFieldId?, uniqueReference?: 'Yes'|'No' }
 *
 * uniqueReference Yes is limited to one attribute per normalized table bucket:
 * patient; specimen+tests; culture+culturetests; ast; custom.
 *
 * Field options note:
 * The fieldOptions list returned by the backend may include flattened
 * grid-sub-field options in addition to the normal one-per-profile-field
 * options. Flattened options are produced server-side from each grid
 * (direct-test or culture-test) profile field - one extra option per grid
 * column - so individual grid columns can be placed as regular attributes
 * outside their grid array. Flattened options carry:
 *   - IsGridField: false (so the existing non-grid filters pick them up),
 *   - ParentGridKey: the Key of the parent grid option,
 *   - GridSubFieldId: the sub-column id inside that grid,
 *   - Text: 'Column (Test)' so the picker label disambiguates them from the
 *     equivalent inside-grid sub-attribute, which uses the bare column label.
 * Inside-grid attribute pickers use the grid's GridSubFields list rather than
 * fieldOptions, so flattened options do not appear there.
 */

const ROOT_PATH = '$';

/**
 * Lookup an option by its canonical Key.
 * @param {Array} fieldOptions
 * @param {string} key
 * @returns {object|undefined}
 */
const findFieldOption = (fieldOptions, key) => {
    if (!Array.isArray(fieldOptions) || !key) return undefined;
    return fieldOptions.find((o) => o.Key === key || o.key === key);
};

/**
 * Field options that are not grid fields (used when adding a normal attribute).
 */
const getNonGridFieldOptions = (fieldOptions) => {
    if (!Array.isArray(fieldOptions)) return [];
    return fieldOptions.filter((o) => !(o.IsGridField || o.isGridField));
};

/**
 * Field options classified as grid fields (used when adding a grid array).
 */
const getGridFieldOptions = (fieldOptions) => {
    if (!Array.isArray(fieldOptions)) return [];
    return fieldOptions.filter((o) => o.IsGridField || o.isGridField);
};

/**
 * Field options that come from the AST (Antibiotic Susceptibility Test) table.
 * One AST record exists per antibiotic per isolate, so when these are present
 * on a profile they must be grouped into their own AST sub-array inside the
 * cultures/isolate array.
 *
 * @param {Array} fieldOptions
 * @returns {Array}
 */
const getAstFieldOptions = (fieldOptions) => {
    if (!Array.isArray(fieldOptions)) return [];
    return fieldOptions.filter((o) => {
        const cat = (o.Category || o.category || '').toLowerCase();
        const tbl = (o.TableName || o.tableName || '').toLowerCase();
        return cat === 'ast' || tbl === 'ast';
    });
};

/**
 * Field options that are valid attribute targets when not inside an AST or
 * grid array. AST table fields are excluded so the editor guides users to
 * place them inside an AST sub-array.
 *
 * @param {Array} fieldOptions
 * @returns {Array}
 */
const getNonAstNonGridFieldOptions = (fieldOptions) => {
    if (!Array.isArray(fieldOptions)) return [];
    return fieldOptions.filter((o) => {
        const isGrid = o.IsGridField || o.isGridField;
        const cat = (o.Category || o.category || '').toLowerCase();
        const tbl = (o.TableName || o.tableName || '').toLowerCase();
        const isAst = cat === 'ast' || tbl === 'ast';
        return !isGrid && !isAst;
    });
};

/**
 * Determine which array types are valid for the given insertion point.
 * - specimens: requires at least one patient field on the profile.
 * - cultures:  requires at least one culture/isolate field on the profile.
 * - grid:      requires at least one grid field on the profile.
 * - ast:       requires at least one AST (ast table) field on the profile;
 *              only valid inside a cultures/isolate array.
 * @param {object} flags  { hasPatient, hasCulture, hasGrid, hasAst }
 * @returns {Array<{ key: string, label: string, enabled: boolean, reason?: string }>}
 */
const getAllowedArrayTypes = ({ hasPatient, hasCulture, hasGrid, hasAst }, language) => {
    const t = (key, fallback) => {
        if (!language) return fallback;
        const tag = language.find((l) => l.Key === key);
        return tag && tag.Value ? tag.Value : fallback;
    };
    return [
        {
            key: 'specimens',
            label: t('@ExpProMapArrSpecimen@', 'Specimens'),
            enabled: !!hasPatient,
            reason: !hasPatient ? t('@ExpProMapNoSpecimen@', 'Specimen arrays require at least one patient field in the export profile.') : undefined
        },
        {
            key: 'cultures',
            label: t('@ExpProMapArrCulture@', 'Cultures / isolates'),
            enabled: !!hasCulture,
            reason: !hasCulture ? t('@ExpProMapNoCulture@', 'Culture arrays require at least one culture or isolate field in the export profile.') : undefined
        },
        {
            key: 'grid',
            label: t('@ExpProMapArrGrid@', 'Grid (test results)'),
            enabled: !!hasGrid,
            reason: !hasGrid ? t('@ExpProMapGridReq@', 'Grid arrays can only be added when a grid field is selected.') : undefined
        },
        {
            key: 'ast',
            label: t('@ExpProMapArrAst@', 'AST results'),
            enabled: !!hasAst,
            reason: !hasAst ? t('@ExpProMapNoAst@', 'AST arrays require at least one AST table field in the export profile.') : undefined
        }
    ];
};

/**
 * Recursively visit the tree and apply mutator to find/insert/replace.
 * Each node must have a stable __id when mutating; we generate them lazily.
 */
let __nextNodeId = 1;
const ensureNodeIds = (node) => {
    if (!node) return node;
    if (!node.__id) node.__id = `n_${__nextNodeId++}`;
    if (Array.isArray(node.children)) {
        for (const c of node.children) ensureNodeIds(c);
    }
    return node;
};

/**
 * Returns a deep clone of the tree, preserving __id.
 */
const cloneTree = (node) => {
    if (!node) return node;
    const next = { ...node };
    if (Array.isArray(node.children)) next.children = node.children.map(cloneTree);
    return next;
};

/**
 * Insert child under a target node id. Returns a new tree.
 * @param {object} root
 * @param {string} targetId
 * @param {object} child
 */
const insertChild = (root, targetId, child) => {
    const next = cloneTree(root);
    const recurse = (node) => {
        if (node.__id === targetId) {
            node.children = Array.isArray(node.children) ? [...node.children, child] : [child];
            return true;
        }
        if (Array.isArray(node.children)) {
            for (const c of node.children) {
                if (recurse(c)) return true;
            }
        }
        return false;
    };
    recurse(next);
    return next;
};

/**
 * Remove a node by id. Refuses to remove the root.
 */
const removeNode = (root, targetId) => {
    if (!targetId || root.__id === targetId) return root;
    const next = cloneTree(root);
    const recurse = (node) => {
        if (Array.isArray(node.children)) {
            const before = node.children.length;
            node.children = node.children.filter((c) => c.__id !== targetId);
            if (node.children.length !== before) return true;
            for (const c of node.children) {
                if (recurse(c)) return true;
            }
        }
        return false;
    };
    recurse(next);
    return next;
};

/**
 * Replace a node by id. Useful for renaming or changing a field reference.
 */
const replaceNode = (root, targetId, replacement) => {
    if (!targetId) return root;
    const next = cloneTree(root);
    const recurse = (node) => {
        if (Array.isArray(node.children)) {
            const idx = node.children.findIndex((c) => c.__id === targetId);
            if (idx >= 0) {
                node.children[idx] = { ...replacement, __id: targetId };
                return true;
            }
            for (const c of node.children) {
                if (recurse(c)) return true;
            }
        }
        return false;
    };
    recurse(next);
    return next;
};

/**
 * Strip transient identifiers before serialisation.
 */
const stripIds = (node) => {
    if (!node) return node;
    const { __id, ...rest } = node;
    if (Array.isArray(rest.children)) rest.children = rest.children.map(stripIds);
    return rest;
};

/**
 * Lookup a node by id.
 */
const findNode = (root, targetId) => {
    if (!root || !targetId) return null;
    if (root.__id === targetId) return root;
    if (Array.isArray(root.children)) {
        for (const c of root.children) {
            const r = findNode(c, targetId);
            if (r) return r;
        }
    }
    return null;
};

/**
 * Determine whether the given node sits inside a 'grid' array. When inside a
 * grid array, attribute pickers are restricted to the grid's prepopulated
 * sub-fields rather than the full profile field list.
 */
const isInsideGridArray = (root, targetId) => {
    if (!root || !targetId) return false;
    const path = [];
    const walk = (node, stack) => {
        if (node.__id === targetId) {
            path.push(...stack);
            return true;
        }
        if (Array.isArray(node.children)) {
            for (const c of node.children) {
                if (walk(c, [...stack, node])) return true;
            }
        }
        return false;
    };
    walk(root, []);
    return path.some((n) => n.kind === 'array' && n.arrayType === 'grid');
};

/**
 * Find the nearest enclosing grid array for a given node id. The target node
 * itself is considered part of the stack: if the user clicks "Add attribute"
 * on a grid array, that grid is returned so its sub-fields can be offered.
 */
const findEnclosingGridArray = (root, targetId) => {
    let result = null;
    const walk = (node, gridStack) => {
        const stackHere = node.kind === 'array' && node.arrayType === 'grid'
            ? [...gridStack, node]
            : gridStack;
        if (node.__id === targetId) {
            result = stackHere[stackHere.length - 1] || null;
            return true;
        }
        if (Array.isArray(node.children)) {
            for (const c of node.children) if (walk(c, stackHere)) return true;
        }
        return false;
    };
    walk(root, []);
    return result;
};

/**
 * Returns the array-type keys ('specimens'|'cultures'|'grid'|'ast') that are
 * valid as direct children of <code>parentNode</code>. Nested arrays are only
 * allowed inside isolate/culture arrays, and only as AST arrays (one record
 * per antibiotic per isolate, sourced from the ast table). When the profile
 * has no AST table fields, no nested array is permitted.
 *
 * @param {object} parentNode
 * @param {object} [opts]
 * @param {boolean} [opts.hasAst] - true when the profile exposes at least one AST table field.
 * @returns {Array<'specimens'|'cultures'|'grid'|'ast'>}
 */
const getAllowedArrayTypeKeysForParent = (parentNode, opts) => {
    if (!parentNode) return ['specimens', 'cultures', 'grid'];
    if (parentNode.kind === 'object') return ['specimens', 'cultures', 'grid'];
    if (parentNode.kind === 'array' && parentNode.arrayType === 'cultures') {
        return opts && opts.hasAst ? ['ast'] : [];
    }
    return [];
};

/**
 * Find the nearest enclosing AST array for a given node id. The target node
 * itself is included in the stack so that clicking "Add attribute" on the
 * AST array itself returns it; the attribute picker is then restricted to
 * AST table fields.
 */
const findEnclosingAstArray = (root, targetId) => {
    let result = null;
    const walk = (node, astStack) => {
        const stackHere = node.kind === 'array' && node.arrayType === 'ast'
            ? [...astStack, node]
            : astStack;
        if (node.__id === targetId) {
            result = stackHere[stackHere.length - 1] || null;
            return true;
        }
        if (Array.isArray(node.children)) {
            for (const c of node.children) if (walk(c, stackHere)) return true;
        }
        return false;
    };
    walk(root, []);
    return result;
};

/**
 * Build a default grid array node from a grid field option. Pre-populates one
 * attribute per grid sub-field with the sub-field id as the attribute name.
 */
const buildGridArrayFromField = (option, defaultName) => {
    const subFields = option?.GridSubFields || option?.gridSubFields || [];
    const children = subFields.map((sf) => ({
        kind: 'attribute',
        name: sf.Id || sf.id || sf.Label || sf.label || 'attribute',
        gridSubFieldId: sf.Id || sf.id || ''
    }));
    return {
        kind: 'array',
        name: defaultName || option?.FieldId || option?.fieldId || 'GridArray',
        arrayType: 'grid',
        gridFieldKey: option?.Key || option?.key,
        children
    };
};

/**
 * Walk the tree and collect nodes that are left unmapped: attribute nodes with
 * no field binding (no fieldKey and no gridSubFieldId) and array nodes with no
 * arrayType. These arise when a structure is imported from a raw JSON/XML file,
 * where shape is known but field bindings and array classifications are not.
 *
 * Unmapped nodes do NOT block saving: a mapping describes the shape of the
 * inbound/outbound data, which legitimately contains fields that are not on the
 * profile. Such nodes are saved as-is and are ignored by the load/unload
 * process. The result is used only to show an informational note and to
 * highlight the affected nodes so the user can optionally bind them.
 *
 * @param {object} root
 * @returns {{ unboundAttributes: object[], untypedArrays: object[] }}
 */
const collectUnmappedNodes = (root) => {
    const unboundAttributes = [];
    const untypedArrays = [];
    const walk = (node) => {
        if (!node) return;
        if (node.kind === 'attribute') {
            const hasBinding = !!(node.fieldKey && String(node.fieldKey).trim())
                || !!(node.gridSubFieldId && String(node.gridSubFieldId).trim());
            if (!hasBinding) unboundAttributes.push(node);
        } else if (node.kind === 'array') {
            if (!node.arrayType || !String(node.arrayType).trim()) untypedArrays.push(node);
        }
        if (Array.isArray(node.children)) {
            for (const c of node.children) walk(c);
        }
    };
    walk(root);
    return { unboundAttributes, untypedArrays };
};

/**
 * Maps an export profile TableName to the canonical unique-reference bucket.
 * @param {string} tableName
 * @returns {string}
 */
const normalizeUniqueReferenceTableBucket = (tableName) => {
    const tbl = String(tableName || '').trim().toLowerCase();
    if (!tbl) return '';
    if (tbl === 'patient') return 'patient';
    if (tbl === 'specimen' || tbl === 'tests') return 'specimen';
    if (tbl === 'culture' || tbl === 'culturetests') return 'culture';
    if (tbl === 'ast') return 'ast';
    if (tbl === 'custom') return 'custom';
    return tbl;
};

/**
 * Parses the table segment from a canonical field key.
 * @param {string} fieldKey
 * @returns {string}
 */
const parseTableNameFromFieldKey = (fieldKey) => {
    if (!fieldKey) return '';
    const parts = String(fieldKey).split('|');
    return parts.length >= 3 ? parts[2] : '';
};

/**
 * Resolves the raw TableName for an attribute binding.
 * @param {string|undefined} fieldKey
 * @param {string|undefined} gridSubFieldId
 * @param {string|undefined} parentGridFieldKey
 * @param {Array} fieldOptions
 * @returns {string}
 */
const resolveAttributeTableName = (fieldKey, gridSubFieldId, parentGridFieldKey, fieldOptions) => {
    const options = Array.isArray(fieldOptions) ? fieldOptions : [];
    if (fieldKey) {
        const opt = findFieldOption(options, fieldKey);
        if (opt?.TableName || opt?.tableName) return opt.TableName || opt.tableName;
        return parseTableNameFromFieldKey(fieldKey);
    }
    if (gridSubFieldId && parentGridFieldKey) {
        const gridOpt = findFieldOption(options, parentGridFieldKey);
        if (gridOpt?.TableName || gridOpt?.tableName) return gridOpt.TableName || gridOpt.tableName;
    }
    return '';
};

/**
 * Resolves the unique-reference bucket for an attribute node or binding inputs.
 * @param {object|string|undefined} attributeOrFieldKey - attribute node or fieldKey string
 * @param {object|null} [structure]
 * @param {Array} [fieldOptions]
 * @param {string|undefined} [gridFieldKey]
 * @param {string|undefined} [gridSubFieldId]
 * @returns {string}
 */
const resolveAttributeTableBucket = (attributeOrFieldKey, structure, fieldOptions, gridFieldKey, gridSubFieldId) => {
    let fieldKey = '';
    let subFieldId = gridSubFieldId || '';
    let parentGridFieldKey = gridFieldKey || '';

    if (typeof attributeOrFieldKey === 'string') {
        fieldKey = attributeOrFieldKey;
    } else if (attributeOrFieldKey && attributeOrFieldKey.kind === 'attribute') {
        fieldKey = attributeOrFieldKey.fieldKey || '';
        subFieldId = attributeOrFieldKey.gridSubFieldId || '';
        if (!parentGridFieldKey && structure && attributeOrFieldKey.__id) {
            const grid = findEnclosingGridArray(structure, attributeOrFieldKey.__id);
            parentGridFieldKey = grid?.gridFieldKey || '';
        }
    }

    const tableName = resolveAttributeTableName(fieldKey, subFieldId, parentGridFieldKey, fieldOptions);
    return tableName ? normalizeUniqueReferenceTableBucket(tableName) : '';
};

/**
 * Returns true when another attribute in the same bucket already has uniqueReference Yes.
 * @param {object|null} structure
 * @param {string} bucket
 * @param {string|undefined} excludeId
 * @param {Array} fieldOptions
 * @returns {boolean}
 */
const isUniqueReferenceBlocked = (structure, bucket, excludeId, fieldOptions) => {
    if (!structure || !bucket) return false;

    const walk = (node, parentGridFieldKey) => {
        if (!node) return false;
        if (node.kind === 'attribute'
            && String(node.uniqueReference || '').toLowerCase() === 'yes'
            && node.__id !== excludeId) {
            const nodeBucket = resolveAttributeTableBucket(node, structure, fieldOptions, parentGridFieldKey);
            if (nodeBucket === bucket) return true;
        }
        const childGridFieldKey = node.kind === 'array' && node.arrayType === 'grid'
            ? (node.gridFieldKey || '')
            : parentGridFieldKey;
        const children = Array.isArray(node.children) ? node.children : [];
        for (const child of children) {
            if (walk(child, childGridFieldKey)) return true;
        }
        return false;
    };

    return walk(structure, '');
};

/**
 * User-facing bucket label for validation messages.
 * @param {string} bucket
 * @returns {string}
 */
const describeUniqueReferenceBucket = (bucket) => {
    switch ((bucket || '').toLowerCase()) {
        case 'patient': return 'patient';
        case 'specimen': return 'specimen';
        case 'culture': return 'culture';
        case 'ast': return 'AST';
        case 'custom': return 'custom';
        default: return bucket || 'table';
    }
};

/**
 * Build the default empty mapping (root object node) when a profile has no
 * mapping yet.
 */
const buildDefaultStructure = (language) => {
    const t = (key, fallback) => {
        if (!language) return fallback;
        const tag = language.find((l) => l.Key === key);
        return tag && tag.Value ? tag.Value : fallback;
    };
    return {
        kind: 'object',
        name: t('@ExpProMapRoot@', 'root'),
        children: []
    };
};

export {
    ROOT_PATH,
    findFieldOption,
    getNonGridFieldOptions,
    getNonAstNonGridFieldOptions,
    getGridFieldOptions,
    getAstFieldOptions,
    getAllowedArrayTypes,
    ensureNodeIds,
    cloneTree,
    insertChild,
    removeNode,
    replaceNode,
    stripIds,
    findNode,
    isInsideGridArray,
    findEnclosingGridArray,
    findEnclosingAstArray,
    getAllowedArrayTypeKeysForParent,
    buildGridArrayFromField,
    buildDefaultStructure,
    collectUnmappedNodes,
    normalizeUniqueReferenceTableBucket,
    resolveAttributeTableName,
    resolveAttributeTableBucket,
    isUniqueReferenceBlocked,
    describeUniqueReferenceBucket
};
