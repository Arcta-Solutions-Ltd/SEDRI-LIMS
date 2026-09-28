import { useState, useEffect } from 'react';
import Post from '../../../Data/Post';
import AddListsIntoFilters from '../../../Utils/Forms/AddListsIntoFilters';
import { SetDefaultPreset } from '../../Containers/ManageList/Functions/FilterState';
import { GenerateThreeDimensions, CreateTwoDimensionsFromThreeDimensions } from './homeGraphDataPipeline';
import { sectionTimerangeToParameters } from './homeDashboardUtils';
import { sectionFiltersForDataFetch } from '../../Containers/Graph/graphChartLifecycle';

const setDefaultFilters = (filters) => {
    for (const filter of filters) {
        if (filter.Key === 'graphtype') {
            filter.values = ['478'];
        }
        if (filter.Key === 'dateinterval') {
            filter.values = ['480'];
        }
    }
};

const mergeSectionFilters = (filters, sectionFilters) => {
    if (!sectionFilters) return;
    for (const f of filters) {
        const raw = sectionFilters[f.Key];
        if (raw === undefined || raw === null || raw === '') continue;
        const vals = Array.isArray(raw) ? raw : String(raw).split(',').filter(Boolean);
        if (vals.length) f.values = vals;
    }
};

/**
 * Builds the filter state used by the home dashboard graph/KPI pipeline (same as full-page graph defaults + section overrides).
 * @param {object|null|undefined} graphConfig Graph definition from config (must have `Name` and `Filters`).
 * @param {object} section Home section with `filters`, timerange fields.
 * @param {Array|undefined} lists Redux list config for {@link AddListsIntoFilters}.
 * @returns {object|null} State object passed to {@link GenerateThreeDimensions}, or null if the graph cannot be loaded.
 */
export function buildHomeGraphDashboardState(graphConfig, section, lists) {
    if (!graphConfig?.Name || !graphConfig.Filters) {
        return null;
    }
    let filters = AddListsIntoFilters(graphConfig.Filters, lists);
    setDefaultFilters(filters);
    let state = { filters };
    state = SetDefaultPreset({ ...graphConfig, FilterPresets: graphConfig.FilterPresets || [] }, state);
    mergeSectionFilters(state.filters, section.filters);
    return state;
}

/**
 * Builds `graph/getdata` POST body and merged filter state for a home dashboard graph or KPI tile.
 * @param {object|null|undefined} graphConfig
 * @param {object} section
 * @param {Array|undefined} lists
 * @returns {{ state: object, criteria: { Name: string, Parameters: Array<{Key: string, Value: string}> } }|null}
 */
export function buildHomeGraphDashboardRequest(graphConfig, section, lists) {
    const state = buildHomeGraphDashboardState(graphConfig, section, lists);
    if (!state) {
        return null;
    }
    const param = [];
    for (const filter of state.filters) {
        if (filter.Key === 'graphtype') {
            continue;
        }
        if (filter.values !== undefined && filter.values.length) {
            param.push({ Key: filter.FieldName, Value: filter.values.map(String).join(',') });
        }
    }
    for (const p of sectionTimerangeToParameters(section)) {
        param.push(p);
    }
    if (state.startDate !== undefined) {
        param.push({ Key: 'StartDate', Value: state.startDate });
    }
    if (state.endDate !== undefined) {
        param.push({ Key: 'EndDate', Value: state.endDate });
    }
    return {
        state,
        criteria: { Name: graphConfig.Name, Parameters: param },
    };
}

/**
 * Loads graph data for a home dashboard chart tile and exposes raw rows + filter state for KPI totals.
 * @param {object|null|undefined} graphConfig
 * @param {object} section
 * @param {Array|undefined} lists
 * @param {string} [homeAuthScopeKey=''] Stable scope marker from toolbar after lab/org switch (e.g. `L1`/`O2`). Included in fetch dependencies so scoped `graph/getdata` re-runs when the JWT laboratory or organisation changes while Home stays mounted.
 */
export function useHomeGraphData(graphConfig, section, lists, homeAuthScopeKey = '') {
    const [threeDimensions, setThreeDimensions] = useState();
    const [twoDimensions, setTwoDimensions] = useState();
    const [sourceData, setSourceData] = useState();
    const [filterState, setFilterState] = useState();

    const sectionKey = JSON.stringify({
        f: sectionFiltersForDataFetch(section?.filters),
        a: section?.timerangeAmount,
        u: section?.timerangeUnitListItemId,
        scope: homeAuthScopeKey ?? '',
    });

    useEffect(() => {
        const built = buildHomeGraphDashboardRequest(graphConfig, section, lists);
        if (!built) {
            setThreeDimensions(undefined);
            setTwoDimensions(undefined);
            setSourceData(undefined);
            setFilterState(undefined);
            return;
        }
        const { state, criteria } = built;
        Post(
            'graph/getdata',
            criteria,
            (data) => {
                const three = GenerateThreeDimensions(data, state);
                const two = CreateTwoDimensionsFromThreeDimensions(three);
                setThreeDimensions(three);
                setTwoDimensions(two);
                setSourceData(data);
                setFilterState(state);
            },
            () => {
                setThreeDimensions(undefined);
                setTwoDimensions(undefined);
                setSourceData(undefined);
                setFilterState(undefined);
            }
        );
    }, [graphConfig, sectionKey, lists, homeAuthScopeKey]);

    return { threeDimensions, twoDimensions, sourceData, filterState };
}
