import AddListsIntoFilters from '../../../Utils/Forms/AddListsIntoFilters';

/**
 * @param {string|string[]|undefined|null} raw - Persisted `section.filters.graphtype`
 * @returns {string|undefined} First list id as string
 */
export function getFirstGraphtypeId(raw) {
    if (raw == null || raw === '') return undefined;
    if (Array.isArray(raw)) return raw[0] != null ? String(raw[0]) : undefined;
    const s = String(raw).split(',')[0].trim();
    return s || undefined;
}

/**
 * Options for the graphtype dropdown, matching the Analytics graph filter (list-backed).
 * @param {object} graphConfig - Graph definition from config (includes Filters)
 * @param {Array} lists - Config lists
 * @returns {Array<{ key: string, text: string }>}
 */
export function getGraphtypeDropdownOptions(graphConfig, lists) {
    if (!graphConfig?.Filters) return [];
    const enriched = AddListsIntoFilters(graphConfig.Filters, lists);
    const f = enriched.find((x) => x.Key === 'graphtype');
    if (!f?.Options?.length) return [];
    return f.Options.map((o) => ({
        key: String(o.key),
        text: o.text || String(o.key),
    }));
}

/**
 * Drops `graphtype` from saved filters when switching to a graph that does not include that option.
 * @param {object} newGraphConfig
 * @param {Array} lists
 * @param {object} filters - Existing section.filters
 * @returns {object} New filters object (shallow clone)
 */
export function normalizeGraphtypeFiltersOnGraphChange(newGraphConfig, lists, filters) {
    const next = { ...(filters || {}) };
    const opts = getGraphtypeDropdownOptions(newGraphConfig, lists);
    if (opts.length === 0) {
        delete next.graphtype;
        return next;
    }
    const allowed = new Set(opts.map((o) => o.key));
    const id = getFirstGraphtypeId(next.graphtype);
    if (id && !allowed.has(id)) {
        delete next.graphtype;
    }
    return next;
}
