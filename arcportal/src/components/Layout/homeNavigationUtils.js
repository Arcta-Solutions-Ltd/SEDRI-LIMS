/**
 * Resolves record/list view names for Home → Recently Used deep links.
 * Names come from server config (views / recordviews); these helpers prefer stable conventions.
 */

/**
 * @param {Array<{Name?: string, Type?: string}>|undefined} recordviews
 * @returns {string|null}
 */
export function resolveCultureRecordViewName(recordviews) {
    const list = recordviews || [];
    const recordOnly = list.filter((x) => (x.Type || '').toLowerCase() === 'recordview');
    const exact = ['cultures'];
    for (const name of exact) {
        const rv = recordOnly.find((x) => (x.Name || '').toLowerCase() === name);
        if (rv?.Name) {
            return rv.Name;
        }
    }
    const fallback = recordOnly.find((x) => /culture/i.test(x.Name || ''));
    return fallback?.Name || null;
}

export function resolvePatientRecordViewName(recordviews) {
    const list = recordviews || [];
    const recordOnly = list.filter((x) => (x.Type || '').toLowerCase() === 'recordview');
    const exact = ['patientrecordview', 'patients'];
    for (const name of exact) {
        const rv = recordOnly.find((x) => (x.Name || '').toLowerCase() === name);
        if (rv?.Name) {
            return rv.Name;
        }
    }
    const fallback = recordOnly.find((x) => /patient/i.test(x.Name || ''));
    return fallback?.Name || null;
}

/**
 * Record view name for opening a specimen by id (e.g. Home → Recently Used specimen rows).
 * Prefers `specimenrecordview` per server config; falls back to another recordview whose name suggests specimen (not patient).
 * @param {Array<{Name?: string, Type?: string}>|undefined} recordviews
 * @returns {string|null}
 */
export function resolveSpecimenRecordViewName(recordviews) {
    const list = recordviews || [];
    const recordOnly = list.filter((x) => (x.Type || '').toLowerCase() === 'recordview');
    const exact = ['specimenrecordview'];
    for (const name of exact) {
        const rv = recordOnly.find((x) => (x.Name || '').toLowerCase() === name);
        if (rv?.Name) {
            return rv.Name;
        }
    }
    const fallback = recordOnly.find((x) => {
        const n = x.Name || '';
        return /specimen/i.test(n) && !/patient/i.test(n);
    });
    return fallback?.Name || null;
}

/**
 * @param {Array<{Name?: string, Type?: string}>|undefined} views
 * @returns {string|null}
 */
export function resolveDirectTestsListViewName(views) {
    const list = views || [];
    const rv = list.find((x) => (x.Type || '').toLowerCase() === 'directtestslist');
    return rv?.Name || null;
}

/**
 * Filter Key used by SetDefaultFiltersPassedIn / ChangeFilterCondition (must match view Filters[].Key).
 * @param {object|undefined} viewConfig
 * @returns {string}
 */


/**
 * Record view name for opening a test by id (e.g. Home → Recently Used test rows).
 * @param {Array<{Name?: string, Type?: string}>|undefined} recordviews
 * @returns {string|null}
 */
export function resolveTestRecordViewName(recordviews) {
    const list = recordviews || [];
    const recordOnly = list.filter((x) => (x.Type || '').toLowerCase() === 'recordview');
    const exact = ['testrecordview'];
    for (const name of exact) {
        const rv = recordOnly.find((x) => (x.Name || '').toLowerCase() === name);
        if (rv?.Name) {
            return rv.Name;
        }
    }
    const fallback = recordOnly.find((x) => /testrecord/i.test(x.Name || ''));
    return fallback?.Name || null;
}

export function resolveTestListFilterKey(viewConfig) {
    const filters = viewConfig?.Filters;
    if (!Array.isArray(filters) || filters.length === 0) {
        return 'id';
    }
    const byId = filters.find((f) => {
        const k = (f.Key || f.key || '').toLowerCase();
        return k === 'id' || k === 'testid' || k.endsWith('id');
    });
    return byId?.Key || byId?.key || filters[0].Key || filters[0].key || 'id';
}
