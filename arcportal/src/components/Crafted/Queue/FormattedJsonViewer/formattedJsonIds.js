/**
 * Builds the DOM ids used by the formatted JSON viewer.
 *
 * Ids are derived from `JsonItemModel.Key`, the untranslated field id from the stored payload, rather than
 * from `Label`, which the backend replaces with the user's language. That keeps the ids stable across
 * languages so tests and automation can address a field without depending on English text.
 */

/**
 * Normalises a payload field id into an id fragment: lowercase, with anything that is not a letter,
 * digit or hyphen collapsed to a hyphen.
 *
 * @param {string|number} value - Field id, label, or index to normalise.
 * @returns {string} Id fragment safe to embed in a DOM id.
 */
export function toIdFragment(value) {
    return String(value ?? '')
        .toLowerCase()
        .replace(/[^a-z0-9-]+/g, '-')
        .replace(/^-+|-+$/g, '');
}

/**
 * Returns the id fragment for one rendered item, preferring the untranslated key and falling back to the
 * label and then the array index so an id is always produced.
 *
 * @param {Object} element - Rendered item from the backend.
 * @param {number} index - Position of the item within its parent.
 * @returns {string} Id fragment for the item.
 */
export function fragmentForElement(element, index) {
    return toIdFragment(element?.Key ?? element?.Label ?? index);
}

/**
 * Joins a parent path and a child fragment into a single path.
 *
 * @param {string} path - Parent path, may be empty at the top level.
 * @param {string} fragment - Fragment to append.
 * @returns {string} Combined path.
 */
export function joinPath(path, fragment) {
    return path ? `${path}-${fragment}` : fragment;
}
