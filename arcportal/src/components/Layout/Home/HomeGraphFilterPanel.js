import React, { useCallback, useEffect, useMemo, useState } from 'react';
import AddListsIntoFilters from '../../../Utils/Forms/AddListsIntoFilters';
import CombinedFilter from '../../General/Filter/CombinedFilter/CombinedFilter';
import TranslateTag from '../../../Utils/Local/TranslateTag';
import { ChangeFilterCondition, SetDefaultPreset } from '../../Containers/ManageList/Functions/FilterState';

const EXCLUDED_FILTER_KEYS = new Set(['dateinterval', 'graphtype']);

function buildExcludedFilterKeySet(additionalExcludedFilterKeys) {
    const set = new Set(EXCLUDED_FILTER_KEYS);
    if (Array.isArray(additionalExcludedFilterKeys)) {
        for (const k of additionalExcludedFilterKeys) {
            if (k != null && k !== '') {
                set.add(String(k).toLowerCase());
            }
        }
    }
    return set;
}

function filterGraphFiltersByExcludedKeys(graphFilters, excludedKeySet) {
    return (graphFilters || []).filter((f) => {
        const key = String(f.Key ?? '').toLowerCase();
        return !excludedKeySet.has(key);
    });
}

/**
 * Merges visible filter row values into persisted section.filters.
 * Keys not in visibleFilterArray (e.g. graphtype, dateinterval) are preserved.
 */
export function mergeVisibleFiltersIntoSection(visibleFilterArray, previousSectionFilters) {
    const next = { ...(previousSectionFilters || {}) };
    for (const f of visibleFilterArray) {
        if (f.values !== undefined && f.values.length) {
            next[f.Key] = f.values.map(String).join(',');
        } else {
            delete next[f.Key];
        }
    }
    return next;
}

const mergeSectionFilters = (filters, sectionFilters) => {
    if (!sectionFilters) return;
    for (const f of filters) {
        const raw = sectionFilters[f.Key];
        if (raw === undefined || raw === null || raw === '') continue;
        const vals = Array.isArray(raw) ? raw : String(raw).split(',').filter(Boolean);
        if (vals.length) f.values = vals;
    }
};

const buildInitialFilterState = (graphConfig, lists, sectionFilters, additionalExcludedFilterKeys) => {
    const excluded = buildExcludedFilterKeySet(additionalExcludedFilterKeys);
    const raw = filterGraphFiltersByExcludedKeys(graphConfig?.Filters, excluded);
    const filters = AddListsIntoFilters(raw, lists);
    let state = { filters };
    state = SetDefaultPreset({ ...graphConfig, FilterPresets: graphConfig.FilterPresets || [] }, state);
    mergeSectionFilters(state.filters, sectionFilters);
    return state;
};

/**
 * Dashboard graph tile: filter controls from graph definition, excluding dateinterval and graphtype (Edit Section).
 */
const HomeGraphFilterPanel = (props) => {
    const { graphConfig, lists, language, sectionFilters, onPersistVisibleFilters, additionalExcludedFilterKeys } =
        props;
    const [filterState, setFilterState] = useState({});

    const sectionFiltersKey = useMemo(() => JSON.stringify(sectionFilters ?? {}), [sectionFilters]);

    // Full rebuild when graph definition or list config changes only — not when section.filters updates,
    // so SetDefaultPreset does not reset user toggles on each persist (see merge effect below).
    useEffect(() => {
        if (!graphConfig?.Name) {
            setFilterState({});
            return;
        }
        setFilterState(buildInitialFilterState(graphConfig, lists, sectionFilters, additionalExcludedFilterKeys));
        // Intentionally omit sectionFilters from deps: full rebuild only on graph/lists; hydration uses merge effect below.
    }, [graphConfig?.Name, lists, additionalExcludedFilterKeys]);

    // Merge persisted section.filters when they load or change; sectionFiltersKey tracks content without listing sectionFilters in deps.
    useEffect(() => {
        if (!graphConfig?.Name) return;
        setFilterState((prev) => {
            if (!prev.filters?.length) return prev;
            const nextFilters = prev.filters.map((f) => ({
                ...f,
                values: f.values ? [...f.values] : [],
            }));
            mergeSectionFilters(nextFilters, sectionFilters);
            return { ...prev, filters: nextFilters };
        });
    }, [sectionFiltersKey, graphConfig?.Name]);

    const filterDropDownHandler = useCallback(
        (event, option, key) => {
            setFilterState((prev) => {
                const state = ChangeFilterCondition(option, key, prev);
                onPersistVisibleFilters(state.filters);
                return state;
            });
        },
        [onPersistVisibleFilters]
    );

    if (!graphConfig?.Name) {
        return null;
    }

    const visibleFilters = filterState.filters || [];
    if (visibleFilters.length === 0) {
        return (
            <p className="home-dashboard-filter-panel-empty">
                {TranslateTag('@DasHomM@', language) || 'No filters are available for this graph.'}
            </p>
        );
    }

    return (
        <div className="home-dashboard-filter-panel">
            <p>
                {TranslateTag('@DasHomL@', language) ||
                    'Graph type, period, and period length are under Edit section. Other filters match the analytics report for this graph.'}
            </p>
            <CombinedFilter
                viewConfig={graphConfig}
                config={filterState}
                view={graphConfig.Name}
                filterDropDownHandler={filterDropDownHandler}
                language={language}
                refresh={() => {}}
                onClear={() => {}}
                filterSearch={false}
                dateSearch={false}
                showRefresh={false}
                showClear={false}
            />
        </div>
    );
};

export default HomeGraphFilterPanel;
