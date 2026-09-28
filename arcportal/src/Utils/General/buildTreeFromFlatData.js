/**
 * Converts a flat list of items into a nested tree structure based on parent-child relationships.
 *
 * @param {Array} items - Flat array of items, each with an id and parent id field.
 * @param {string} idField - Name of the field containing the item's unique identifier (e.g. 'id', 'Id').
 * @param {string} parentIdField - Name of the field containing the parent's identifier (e.g. 'parentorganisationid', 'ParentOrganisationId').
 * @returns {Array} Array of root nodes. Each node has shape { item, children: [] }.
 *   Items with null, undefined, or 0 parentId are treated as roots.
 */
const buildTreeFromFlatData = (items, idField, parentIdField) => {
    if (!items || !Array.isArray(items)) {
        return [];
    }

    const getValue = (obj, field) => {
        if (!obj) return undefined;
        const key = Object.keys(obj).find(k => k.toLowerCase() === field.toLowerCase());
        return key !== undefined ? obj[key] : undefined;
    };
    const normalizeId = (id) => (id == null ? null : String(id));

    const itemMap = new Map();
    items.forEach(item => {
        const id = normalizeId(getValue(item, idField));
        if (id !== null) {
            itemMap.set(id, { item: { ...item }, children: [] });
        }
    });

    const roots = [];
    items.forEach(item => {
        const id = normalizeId(getValue(item, idField));
        const parentId = getValue(item, parentIdField);
        const node = itemMap.get(id);

        if (!node) return;

        const isRoot = parentId === null || parentId === undefined || parentId === 0 || parentId === '';
        if (isRoot) {
            roots.push(node);
        } else {
            const parent = itemMap.get(normalizeId(parentId));
            if (parent) {
                parent.children.push(node);
            } else {
                roots.push(node);
            }
        }
    });

    return roots;
};

export default buildTreeFromFlatData;
