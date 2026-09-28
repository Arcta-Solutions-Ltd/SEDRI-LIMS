import React, { useCallback, useEffect, useLayoutEffect, useMemo, useRef, useState } from 'react';
import { connect } from 'react-redux';
import GridLayout from 'react-grid-layout/legacy';
import 'react-grid-layout/css/styles.css';
import 'react-resizable/css/styles.css';
import { IconButton, Panel, PanelType, Dropdown, TextField, Checkbox, SwatchColorPicker } from '@fluentui/react';
import './Home.css';
import PostEvent from '../../../Data/PostEvents';
import TranslateTag from '../../../Utils/Local/TranslateTag';
import * as actionTypes from '../../../store/actions';
import ErrorMessage from '../../General/ErrorMessage/ErrorMessage';
import HomeButtonTile from './HomeButtonTile';
import HomeGraphTile from './HomeGraphTile';
import HomeKpiTile from './HomeKpiTile';
import HomeRecentlyUsedTile from './HomeRecentlyUsedTile';
import HomeTatComplianceTile from './HomeTatComplianceTile';
import HomeGraphFilterPanel, { mergeVisibleFiltersIntoSection } from './HomeGraphFilterPanel';
import {
    parseHomeDashboardPreference,
    normalizeHomeSection,
    DASHBOARD_TIMERANGE_UNIT_LIST_NAME,
    DEFAULT_TIMERANGE_AMOUNT,
    DEFAULT_TIMERANGE_UNIT_LIST_ITEM_ID,
    RECENTLY_USED_KINDS,
    DEFAULT_RECENTLY_USED_LIMIT,
    HOME_SECTION_VISUALIZATIONS,
    TAT_COMPLIANCE_EXCLUDED_FILTER_KEYS,
    omitTatComplianceExcludedFilterKeys,
} from './homeDashboardUtils';
import {
    getGraphtypeDropdownOptions,
    getFirstGraphtypeId,
    normalizeGraphtypeFiltersOnGraphChange,
} from './homeGraphFilterUi';
import {
    HOME_KPI_COLOR_CELLS,
    DEFAULT_KPI_COLOR_SWATCH_ID,
    DEFAULT_KPI_FONT_SIZE_PX,
    KPI_FONT_SIZE_MIN,
    KPI_FONT_SIZE_MAX,
} from './homeKpiPalette';

/** @param {{ laboratorySelected?: string }} props Layout passes toolbar scope after auth/switch (e.g. `L1`, `O2`). */
const Home = (props) => {
    const [errorStatus, updateErrorStatus] = useState({ visible: false, message: '' });
    const [rightPanel, setRightPanel] = useState(null);
    const [layoutEditingEnabled, setLayoutEditingEnabled] = useState(false);
    const containerRef = useRef(null);
    const [width, setWidth] = useState(1200);

    const homeModel = useMemo(() => {
        return parseHomeDashboardPreference(props.preferences?.HomeDashboard, props.language);
    }, [props.preferences?.HomeDashboard]);

    const [model, setModel] = useState(homeModel);

    useEffect(() => {
        setModel(homeModel);
    }, [homeModel]);

    useLayoutEffect(() => {
        const el = containerRef.current;
        if (!el) return undefined;
        const ro = new ResizeObserver(() => setWidth(el.offsetWidth || 1200));
        ro.observe(el);
        setWidth(el.offsetWidth || 1200);
        return () => ro.disconnect();
    }, []);

    const graphCatalog = props.homeDashboardGraphs?.length ? props.homeDashboardGraphs : props.graphs || [];

    const graphOptions = useMemo(() => {
        return graphCatalog.map((g) => ({
            key: g.Name,
            text: TranslateTag(g.Title, props.language) || g.Title || g.Name,
        }));
    }, [graphCatalog, props.language]);

    const saveToServer = useCallback(
        (nextModel) => {
            const payload = { Event: 'savehomedashboardevent', Id: '0', HomeDashboard: nextModel };
            PostEvent(
                payload,
                () => {
                    props.onHomeDashboardSaved(JSON.stringify(nextModel));
                },
                (err) => updateErrorStatus({ visible: true, message: err?.data || 'Save failed' })
            );
        },
        [props.onHomeDashboardSaved]
    );

    const onLayoutChange = useCallback((layout) => {
        setModel((prev) => ({ ...prev, layout }));
    }, []);

    const onDragOrResizeStop = useCallback(
        (layout) => {
            setModel((prev) => {
                const next = { ...prev, layout };
                saveToServer(next);
                return next;
            });
        },
        [saveToServer]
    );

    const duplicateSection = useCallback(
        (sourceSectionId) => {
            setModel((prev) => {
                const sourceLayout = prev.layout.find((l) => l.i === sourceSectionId);
                const sourceSection = prev.sections[sourceSectionId];
                if (!sourceLayout || !sourceSection) {
                    return prev;
                }
                const newId = `sec_${Date.now()}`;
                const clonedSection = JSON.parse(JSON.stringify(sourceSection));
                const newLayoutItem = {
                    ...sourceLayout,
                    i: newId,
                    y: Infinity,
                };
                const nextLayout = [...prev.layout, newLayoutItem];
                const nextSections = { ...prev.sections, [newId]: clonedSection };
                const next = { ...prev, layout: nextLayout, sections: nextSections };
                saveToServer(next);
                return next;
            });
        },
        [saveToServer]
    );

    const deleteSection = useCallback(
        (sectionId) => {
            setModel((prev) => {
                if (prev.layout.length <= 1) {
                    const msg =
                        TranslateTag('@DasHomJ@', props.language) ||
                        'Cannot delete the last section on the dashboard.';
                    setTimeout(() => updateErrorStatus({ visible: true, message: msg }), 0);
                    return prev;
                }
                const nextLayout = prev.layout.filter((l) => l.i !== sectionId);
                const nextSections = { ...prev.sections };
                delete nextSections[sectionId];
                const next = { ...prev, layout: nextLayout, sections: nextSections };
                saveToServer(next);
                return next;
            });
            setRightPanel((cur) => (cur?.sectionId === sectionId ? null : cur));
        },
        [saveToServer, props.language]
    );

    const updateSection = useCallback(
        (sectionId, patch) => {
            setModel((prev) => {
                const nextSections = {
                    ...prev.sections,
                    [sectionId]: { ...prev.sections[sectionId], ...patch },
                };
                const next = { ...prev, sections: nextSections };
                saveToServer(next);
                return next;
            });
        },
        [saveToServer]
    );

    const homeClickHandler = props.onClick;

    const persistDashboardFilters = useCallback(
        (sectionId, visibleFilterArray) => {
            setModel((prev) => {
                const prevFilters = prev.sections[sectionId]?.filters || {};
                const merged = mergeVisibleFiltersIntoSection(visibleFilterArray, prevFilters);
                const filters =
                    prev.sections[sectionId]?.visualization === 'tatCompliance'
                        ? omitTatComplianceExcludedFilterKeys(merged)
                        : merged;
                const nextSections = {
                    ...prev.sections,
                    [sectionId]: { ...prev.sections[sectionId], filters },
                };
                const next = { ...prev, sections: nextSections };
                saveToServer(next);
                return next;
            });
        },
        [saveToServer]
    );

    const editingSectionId = rightPanel?.mode === 'edit' ? rightPanel.sectionId : null;
    const filterSectionId = rightPanel?.mode === 'filter' ? rightPanel.sectionId : null;
    const panelOpen = rightPanel !== null;

    const [draftSectionTitle, setDraftSectionTitle] = useState('');
    const draftSectionTitleRef = useRef('');
    const modelRef = useRef(model);
    modelRef.current = model;

    const commitSectionTitle = useCallback(
        (sectionId, title) => {
            if (!sectionId) return;
            const nextTitle = title ?? '';
            const cur = modelRef.current.sections[sectionId]?.sectionTitle ?? '';
            if (cur === nextTitle) return;
            updateSection(sectionId, { sectionTitle: nextTitle });
        },
        [updateSection]
    );

    const openEditSection = useCallback(
        (sectionId) => {
            if (rightPanel?.mode === 'edit' && editingSectionId && editingSectionId !== sectionId) {
                commitSectionTitle(editingSectionId, draftSectionTitleRef.current);
            }
            setRightPanel({ mode: 'edit', sectionId });
        },
        [rightPanel?.mode, editingSectionId, commitSectionTitle]
    );

    const openFilterSection = useCallback(
        (sectionId) => {
            if (rightPanel?.mode === 'edit' && editingSectionId) {
                commitSectionTitle(editingSectionId, draftSectionTitleRef.current);
            }
            setRightPanel({ mode: 'filter', sectionId });
        },
        [rightPanel?.mode, editingSectionId, commitSectionTitle]
    );

    useEffect(() => {
        if (!editingSectionId) return;
        const t = modelRef.current.sections[editingSectionId]?.sectionTitle ?? '';
        setDraftSectionTitle(t);
        draftSectionTitleRef.current = t;
    }, [editingSectionId]);

    const editingSection = editingSectionId ? model.sections[editingSectionId] : null;

    const editingGraphConfig = useMemo(
        () =>
            editingSection &&
            (editingSection.visualization === 'graph' || editingSection.visualization === 'kpi')
                ? graphCatalog.find((g) => g.Name === editingSection.graphName)
                : null,
        [graphCatalog, editingSection]
    );

    const filterSection = filterSectionId ? model.sections[filterSectionId] : null;
    const filterGraphConfig = useMemo(() => {
        if (!filterSection) {
            return null;
        }
        if (filterSection.visualization === 'graph' || filterSection.visualization === 'kpi') {
            return graphCatalog.find((g) => g.Name === filterSection.graphName) ?? null;
        }
        if (filterSection.visualization === 'tatCompliance') {
            return graphCatalog.find((g) => g.Name === 'specimentypesummarygraph') || graphCatalog[0] || null;
        }
        return null;
    }, [graphCatalog, filterSection]);

    const graphtypeOptionsForPanel = useMemo(
        () => getGraphtypeDropdownOptions(editingGraphConfig, props.lists),
        [editingGraphConfig, props.lists]
    );

    const periodUnitOptions = useMemo(() => {
        const list = props.lists?.find(
            (l) => (l.Name || l.name || '').toLowerCase() === DASHBOARD_TIMERANGE_UNIT_LIST_NAME
        );
        const opts = list?.Options || list?.options || [];
        return opts.map((o) => ({
            key: String(o.key ?? o.Key),
            text: String(o.text ?? o.Text ?? ''),
        }));
    }, [props.lists]);

    const directTestConfigOptions = useMemo(() => {
        const list = props.lists?.find(
            (l) => (l.Name || l.name || '').toLowerCase() === 'directtestconfiglist'
        );
        const opts = list?.Options || list?.options || [];
        return opts.map((o) => ({
            key: String(o.key ?? o.Key),
            text: String(o.text ?? o.Text ?? ''),
        }));
    }, [props.lists]);

    const cultureTypeListOptions = useMemo(() => {
        const list = props.lists?.find((l) => (l.Name || l.name || '').toLowerCase() === 'culturetype');
        const opts = list?.Options || list?.options || [];
        return opts.map((o) => ({
            key: String(o.key ?? o.Key),
            text: String(o.text ?? o.Text ?? ''),
        }));
    }, [props.lists]);

    /** Labels and option text must have non-empty fallbacks when language entries are missing (Fluent UI Dropdown breaks on empty text/unknown selectedKey). */
    const visualizationEditOptions = useMemo(
        () => [
            { key: 'buttons', text: TranslateTag('@DasHomS@', props.language) || 'Buttons' },
            { key: 'graph', text: TranslateTag('@DasHomT@', props.language) || 'Graph' },
            { key: 'kpi', text: TranslateTag('@DasHomKpi@', props.language) || 'KPI' },
            { key: 'recentlyUsed', text: TranslateTag('@DasHomN@', props.language) || 'Recently Used' },
            { key: 'tatCompliance', text: TranslateTag('@DasHomTat@', props.language) || 'TAT Compliance' },
        ],
        [props.language]
    );

    const recentlyUsedKindLabel = useCallback(
        (kind) => {
            switch (kind) {
                case 'specimen':
                    return TranslateTag('@DasHomU@', props.language) || 'Specimen';
                case 'patient':
                    return TranslateTag('@DasHomV@', props.language) || 'Patient';
                case 'test':
                    return TranslateTag('@DasHomW@', props.language) || 'Test';
                case 'culture':
                    return TranslateTag('@SpeCulA@', props.language) || 'Isolate';
                default:
                    return kind;
            }
        },
        [props.language]
    );

    const dashboardMenuProps = useMemo(
        () => ({
            items: [
                {
                    key: 'editLayout',
                    text: TranslateTag('@DasHomB@', props.language) || 'Edit dashboard layout',
                    canCheck: true,
                    checked: layoutEditingEnabled,
                    onClick: () => {
                        if (layoutEditingEnabled && rightPanel?.mode === 'edit' && editingSectionId) {
                            commitSectionTitle(editingSectionId, draftSectionTitleRef.current);
                        }
                        setLayoutEditingEnabled((prev) => {
                            if (prev) {
                                setRightPanel(null);
                            }
                            return !prev;
                        });
                    },
                },
            ],
        }),
        [
            layoutEditingEnabled,
            props.language,
            rightPanel?.mode,
            editingSectionId,
            commitSectionTitle,
        ]
    );

    const sectionCount = model.layout.length;
    const canDeleteSection = sectionCount > 1;

    const renderTile = (item) => {
        const section = model.sections[item.i];
        if (!section) return <div />;

        const graphConfig =
            section.visualization === 'graph' || section.visualization === 'kpi'
                ? graphCatalog.find((g) => g.Name === section.graphName)
                : null;
        const isGraphTile = section.visualization === 'graph' && graphConfig;
        const isKpiTile = section.visualization === 'kpi' && graphConfig;
        const isRecentlyUsedTile = section.visualization === 'recentlyUsed';
        const isTatTile = section.visualization === 'tatCompliance';

        const layoutEditMenuItems = [];
        if (layoutEditingEnabled) {
            if (isGraphTile || isKpiTile || isTatTile) {
                layoutEditMenuItems.push({
                    key: 'filter',
                    text: TranslateTag('@DasHomK@', props.language) || 'Filter Section',
                    onClick: () => openFilterSection(item.i),
                });
            }
            layoutEditMenuItems.push(
                {
                    key: 'edit',
                    text: TranslateTag('@DasHomE@', props.language) || 'Edit Section',
                    onClick: () => openEditSection(item.i),
                },
                {
                    key: 'duplicate',
                    text: TranslateTag('@DasHomI@', props.language) || 'Duplicate Section',
                    onClick: () => duplicateSection(item.i),
                },
                {
                    key: 'del',
                    text: TranslateTag('@DasHomF@', props.language) || 'Delete Section',
                    disabled: !canDeleteSection,
                    title: !canDeleteSection
                        ? TranslateTag('@DasHomJ@', props.language) ||
                          'Cannot delete the last section on the dashboard.'
                        : undefined,
                    onClick: () => deleteSection(item.i),
                }
            );
        }

        const headingText = (
            layoutEditingEnabled &&
            rightPanel?.mode === 'edit' &&
            rightPanel.sectionId === item.i
                ? draftSectionTitle
                : section.sectionTitle ?? ''
        ).trim();

        return (
            <div className="home-dashboard-tile" key={item.i}>
                {layoutEditingEnabled && (
                    <div className="home-dashboard-tile-header">
                        <span className="home-dashboard-drag-hint" title="Drag to move" />
                        <IconButton
                            key={`tile-more-${item.i}`}
                            className="home-dashboard-tile-menu"
                            iconProps={{ iconName: 'More' }}
                            menuProps={{ items: layoutEditMenuItems }}
                            ariaLabel="Section menu"
                        />
                    </div>
                )}
                <div className="home-dashboard-tile-content">
                    {headingText ? (
                        <div className="home-dashboard-section-heading">{headingText}</div>
                    ) : null}
                    {section.visualization === 'buttons' ? (
                        <HomeButtonTile
                            section={section}
                            onCardClick={homeClickHandler}
                            homeAuthScopeKey={props.laboratorySelected}
                        />
                    ) : isRecentlyUsedTile ? (
                        <HomeRecentlyUsedTile
                            section={section}
                            lists={props.lists}
                            onItemClick={props.onRecentlyUsedNavigate}
                            homeAuthScopeKey={props.laboratorySelected}
                        />
                    ) : isGraphTile ? (
                        <HomeGraphTile
                            graphConfig={graphConfig}
                            section={section}
                            lists={props.lists}
                            language={props.language}
                            homeAuthScopeKey={props.laboratorySelected}
                        />
                    ) : isKpiTile ? (
                        <HomeKpiTile
                            graphConfig={graphConfig}
                            section={section}
                            lists={props.lists}
                            language={props.language}
                            homeAuthScopeKey={props.laboratorySelected}
                        />
                    ) : isTatTile ? (
                        <HomeTatComplianceTile
                            section={section}
                            language={props.language}
                            homeAuthScopeKey={props.laboratorySelected}
                        />
                    ) : (
                        <div className="home-dashboard-tile-inner" />
                    )}
                </div>
            </div>
        );
    };

    const errorCloseHandler = () => {
        updateErrorStatus({ visible: false, message: '' });
    };

    return (
        <div className="home-page">
            <div className="home-dashboard-toolbar">
                <div className="home-dashboard-toolbar-right">
                    <IconButton
                        iconProps={{ iconName: 'Settings' }}
                        title={TranslateTag('@DasHomD@', props.language) || 'Home dashboard menu'}
                        ariaLabel={TranslateTag('@DasHomD@', props.language) || 'Home dashboard menu'}
                        menuProps={dashboardMenuProps}
                    />
                </div>
            </div>

            <div ref={containerRef} className="home-content home-dashboard-grid-host">
                <GridLayout
                    className="home-dashboard-grid"
                    layout={model.layout}
                    cols={12}
                    rowHeight={24}
                    width={width}
                    onLayoutChange={onLayoutChange}
                    onDragStop={onDragOrResizeStop}
                    onResizeStop={onDragOrResizeStop}
                    draggableHandle=".home-dashboard-drag-hint"
                    isDraggable={layoutEditingEnabled}
                    isResizable={layoutEditingEnabled}
                    compactType="vertical"
                >
                    {model.layout.map((item) => (
                        <div key={item.i} data-grid={item}>
                            {renderTile(item)}
                        </div>
                    ))}
                </GridLayout>
            </div>

            <Panel
                isOpen={panelOpen}
                onDismiss={() => {
                    if (rightPanel?.mode === 'edit' && editingSectionId) {
                        commitSectionTitle(editingSectionId, draftSectionTitleRef.current);
                    }
                    setRightPanel(null);
                }}
                type={PanelType.smallFluidRight}
                headerText={
                    rightPanel?.mode === 'filter'
                        ? TranslateTag('@DasHomK@', props.language) || 'Filter Section'
                        : TranslateTag('@DasHom@', props.language) || 'Home dashboard'
                }
                closeButtonAriaLabel="Close"
            >
                <div className="home-dashboard-panel">
                    {rightPanel?.mode === 'filter' && filterGraphConfig && filterSection && (
                        <HomeGraphFilterPanel
                            key={filterSectionId}
                            graphConfig={filterGraphConfig}
                            lists={props.lists}
                            language={props.language}
                            sectionFilters={filterSection.filters}
                            additionalExcludedFilterKeys={
                                filterSection.visualization === 'tatCompliance'
                                    ? TAT_COMPLIANCE_EXCLUDED_FILTER_KEYS
                                    : undefined
                            }
                            onPersistVisibleFilters={(filters) =>
                                persistDashboardFilters(filterSectionId, filters)
                            }
                        />
                    )}
                    {rightPanel?.mode === 'filter' && (!filterSection || !filterGraphConfig) && (
                        <p>
                            Unable to load filters for this section. Close the panel and try again.
                        </p>
                    )}
                    {rightPanel?.mode === 'edit' && !editingSection && (
                        <p>
                            Nothing to edit. Close the panel and use Edit section on a tile to open settings.
                        </p>
                    )}
                    {rightPanel?.mode === 'edit' && editingSection && (
                        <>
                            <p>
                                {TranslateTag('@DasHomA@', props.language) ||
                                    'These settings apply to the tile you opened from. Use Duplicate section on a tile (when layout editing is on) to add another section. Change visualization, graph type, and time range. Filters use the same lists as analytics (IDs only).'}
                            </p>
                            <TextField
                                label={TranslateTag('@DasHomSectionTitle@', props.language) || 'Section name'}
                                value={draftSectionTitle}
                                onChange={(_, val) => {
                                    const v = val ?? '';
                                    draftSectionTitleRef.current = v;
                                    setDraftSectionTitle(v);
                                }}
                                onBlur={() =>
                                    commitSectionTitle(editingSectionId, draftSectionTitleRef.current)
                                }
                            />
                            <Dropdown
                                label={TranslateTag('@DasHomR@', props.language) || 'Visualization'}
                                selectedKey={
                                    HOME_SECTION_VISUALIZATIONS.includes(editingSection.visualization)
                                        ? editingSection.visualization
                                        : 'buttons'
                                }
                                options={visualizationEditOptions}
                                onChange={(_, o) => {
                                    if (!o || o.key == null) {
                                        return;
                                    }
                                    if (o.key === 'recentlyUsed') {
                                        const next = normalizeHomeSection({
                                            ...editingSection,
                                            visualization: 'recentlyUsed',
                                            recentlyUsedKinds:
                                                editingSection.recentlyUsedKinds || [...RECENTLY_USED_KINDS],
                                            recentlyUsedDirectTestIds: editingSection.recentlyUsedDirectTestIds || [],
                                            recentlyUsedCultureTypeIds: editingSection.recentlyUsedCultureTypeIds || [],
                                            recentlyUsedLimit:
                                                editingSection.recentlyUsedLimit != null
                                                    ? editingSection.recentlyUsedLimit
                                                    : DEFAULT_RECENTLY_USED_LIMIT,
                                        });
                                        updateSection(editingSectionId, next);
                                    } else if (o.key === 'kpi') {
                                        const next = normalizeHomeSection({
                                            ...editingSection,
                                            visualization: 'kpi',
                                            graphName:
                                                editingSection.graphName ||
                                                (graphOptions.length > 0 ? graphOptions[0].key : ''),
                                            filters: editingSection.filters || {},
                                        });
                                        updateSection(editingSectionId, next);
                                    } else if (o.key === 'tatCompliance') {
                                        const next = normalizeHomeSection({
                                            ...editingSection,
                                            visualization: 'tatCompliance',
                                            filters: editingSection.filters || {},
                                            timerangeAmount: 24,
                                            timerangeUnitListItemId: '1542',
                                        });
                                        updateSection(editingSectionId, next);
                                    } else {
                                        updateSection(editingSectionId, { visualization: o.key });
                                    }
                                }}
                            />
                            {(editingSection.visualization === 'graph' || editingSection.visualization === 'kpi') &&
                                graphOptions.length > 0 && (
                                <Dropdown
                                    label="Graph"
                                    selectedKey={
                                        graphOptions.some((opt) => opt.key === editingSection.graphName)
                                            ? editingSection.graphName
                                            : graphOptions[0].key
                                    }
                                    options={graphOptions}
                                    onChange={(_, o) => {
                                        if (!o || o.key == null) {
                                            return;
                                        }
                                        const newConfig = graphCatalog.find((g) => g.Name === o.key);
                                        const nextFilters = normalizeGraphtypeFiltersOnGraphChange(
                                            newConfig,
                                            props.lists,
                                            editingSection.filters
                                        );
                                        updateSection(editingSectionId, {
                                            graphName: o.key,
                                            filters: nextFilters,
                                        });
                                    }}
                                />
                            )}
                            {editingSection.visualization === 'graph' && graphtypeOptionsForPanel.length > 0 && (
                                <Dropdown
                                    label={TranslateTag('@GraTyp@', props.language) || 'Chart type'}
                                    selectedKey={
                                        getFirstGraphtypeId(editingSection.filters?.graphtype) ??
                                        graphtypeOptionsForPanel[0]?.key
                                    }
                                    options={graphtypeOptionsForPanel}
                                    onChange={(_, o) => {
                                        if (!o || o.key == null) {
                                            return;
                                        }
                                        updateSection(editingSectionId, {
                                            filters: { ...editingSection.filters, graphtype: o.key },
                                        });
                                    }}
                                />
                            )}
                            {editingSection.visualization === 'kpi' && (
                                <>
                                    <div className="home-dashboard-kpi-swatch-label">
                                        {TranslateTag('@DasHomKpiColor@', props.language) || 'KPI colour'}
                                    </div>
                                    <SwatchColorPicker
                                        columnCount={5}
                                        cellWidth={32}
                                        cellHeight={32}
                                        colorCells={HOME_KPI_COLOR_CELLS}
                                        selectedId={
                                            editingSection.kpiColorSwatchId || DEFAULT_KPI_COLOR_SWATCH_ID
                                        }
                                        onChange={(_, id) => {
                                            if (id) {
                                                updateSection(editingSectionId, { kpiColorSwatchId: id });
                                            }
                                        }}
                                    />
                                    <TextField
                                        label={TranslateTag('@DasHomKpiFont@', props.language) || 'KPI font size (px)'}
                                        type="number"
                                        min={KPI_FONT_SIZE_MIN}
                                        max={KPI_FONT_SIZE_MAX}
                                        value={String(
                                            editingSection.kpiFontSizePx != null
                                                ? editingSection.kpiFontSizePx
                                                : DEFAULT_KPI_FONT_SIZE_PX
                                        )}
                                        onChange={(_, v) => {
                                            const n = parseInt(String(v || ''), 10);
                                            updateSection(editingSectionId, {
                                                kpiFontSizePx:
                                                    Number.isFinite(n) && n >= KPI_FONT_SIZE_MIN && n <= KPI_FONT_SIZE_MAX
                                                        ? n
                                                        : DEFAULT_KPI_FONT_SIZE_PX,
                                            });
                                        }}
                                    />
                                    <Checkbox
                                        label={TranslateTag('@DasHomKpiBold@', props.language) || 'Bold'}
                                        checked={editingSection.kpiBold !== false}
                                        onChange={(_, checked) =>
                                            updateSection(editingSectionId, { kpiBold: !!checked })
                                        }
                                    />
                                </>
                            )}
                            {editingSection.visualization === 'tatCompliance' && (
                                <>
                                    <p className="home-dashboard-hint">
                                        {TranslateTag('@DasHomA@', props.language) ||
                                            'Filters use the same lists as analytics (IDs only). Period length applies to the primary compliance window.'}
                                    </p>
                                    <div className="home-dashboard-kpi-swatch-label">
                                        {TranslateTag('@DasHomTatTitle@', props.language) || 'TAT Compliance (%)'}
                                    </div>
                                    <TextField
                                        label={TranslateTag('@DasHomKpiFont@', props.language) || 'KPI font size (px)'}
                                        type="number"
                                        min={KPI_FONT_SIZE_MIN}
                                        max={KPI_FONT_SIZE_MAX}
                                        value={String(
                                            editingSection.tatFontSizePx != null
                                                ? editingSection.tatFontSizePx
                                                : DEFAULT_KPI_FONT_SIZE_PX
                                        )}
                                        onChange={(_, v) => {
                                            const n = parseInt(String(v || ''), 10);
                                            updateSection(editingSectionId, {
                                                tatFontSizePx:
                                                    Number.isFinite(n) && n >= KPI_FONT_SIZE_MIN && n <= KPI_FONT_SIZE_MAX
                                                        ? n
                                                        : DEFAULT_KPI_FONT_SIZE_PX,
                                            });
                                        }}
                                    />
                                    <Checkbox
                                        label={TranslateTag('@DasHomKpiBold@', props.language) || 'Bold'}
                                        checked={editingSection.tatBold !== false}
                                        onChange={(_, checked) =>
                                            updateSection(editingSectionId, { tatBold: !!checked })
                                        }
                                    />
                                    <TextField
                                        label={TranslateTag('@DasHomTatTarget@', props.language) || 'Target TAT (hours)'}
                                        type="number"
                                        min={0.1}
                                        step={0.5}
                                        value={String(
                                            editingSection.tatTargetHours != null
                                                ? editingSection.tatTargetHours
                                                : 48
                                        )}
                                        onChange={(_, v) => {
                                            const n = parseFloat(String(v || ''));
                                            updateSection(
                                                editingSectionId,
                                                normalizeHomeSection({
                                                    ...editingSection,
                                                    visualization: 'tatCompliance',
                                                    tatTargetHours:
                                                        Number.isFinite(n) && n > 0 ? n : 48,
                                                })
                                            );
                                        }}
                                    />
                                    <TextField
                                        label={TranslateTag('@DasHomTatLate@', props.language) || 'Late reporting threshold (hours)'}
                                        type="number"
                                        min={0.1}
                                        step={0.5}
                                        value={String(
                                            editingSection.tatLateThresholdHours != null
                                                ? editingSection.tatLateThresholdHours
                                                : 48
                                        )}
                                        onChange={(_, v) => {
                                            const n = parseFloat(String(v || ''));
                                            updateSection(
                                                editingSectionId,
                                                normalizeHomeSection({
                                                    ...editingSection,
                                                    visualization: 'tatCompliance',
                                                    tatLateThresholdHours:
                                                        Number.isFinite(n) && n > 0 ? n : 48,
                                                })
                                            );
                                        }}
                                    />
                                    <TextField
                                        label={TranslateTag('@DasHomTatAvg@', props.language) || 'Rolling average (days, 0 = off)'}
                                        type="number"
                                        min={0}
                                        value={String(
                                            editingSection.tatRollingAverageDays != null
                                                ? editingSection.tatRollingAverageDays
                                                : 0
                                        )}
                                        onChange={(_, v) => {
                                            const n = parseInt(String(v || ''), 10);
                                            updateSection(
                                                editingSectionId,
                                                normalizeHomeSection({
                                                    ...editingSection,
                                                    visualization: 'tatCompliance',
                                                    tatRollingAverageDays:
                                                        Number.isFinite(n) && n >= 0 ? n : 0,
                                                })
                                            );
                                        }}
                                    />
                                    <div className="home-tat-rag-row">
                                        <span
                                            className="home-tat-rag-swatch"
                                            style={{ background: '#107c10' }}
                                            aria-hidden
                                        />
                                        <TextField
                                            label={TranslateTag('@DasHomTatRagG@', props.language) || 'Green — minimum %'}
                                            type="number"
                                            min={0}
                                            max={100}
                                            value={String(
                                                editingSection.ragGreenMin != null
                                                    ? editingSection.ragGreenMin
                                                    : 90
                                            )}
                                            onChange={(_, v) => {
                                                const n = parseFloat(String(v || ''));
                                                updateSection(
                                                    editingSectionId,
                                                    normalizeHomeSection({
                                                        ...editingSection,
                                                        visualization: 'tatCompliance',
                                                        ragGreenMin: Number.isFinite(n)
                                                            ? Math.min(100, Math.max(0, n))
                                                            : 90,
                                                    })
                                                );
                                            }}
                                        />
                                    </div>
                                    <div className="home-tat-rag-row">
                                        <span
                                            className="home-tat-rag-swatch"
                                            style={{ background: '#ca5010' }}
                                            aria-hidden
                                        />
                                        <TextField
                                            label={TranslateTag('@DasHomTatRagA@', props.language) || 'Amber — minimum %'}
                                            type="number"
                                            min={0}
                                            max={100}
                                            value={String(
                                                editingSection.ragAmberMin != null
                                                    ? editingSection.ragAmberMin
                                                    : 75
                                            )}
                                            onChange={(_, v) => {
                                                const n = parseFloat(String(v || ''));
                                                updateSection(
                                                    editingSectionId,
                                                    normalizeHomeSection({
                                                        ...editingSection,
                                                        visualization: 'tatCompliance',
                                                        ragAmberMin: Number.isFinite(n)
                                                            ? Math.min(100, Math.max(0, n))
                                                            : 75,
                                                    })
                                                );
                                            }}
                                        />
                                    </div>
                                    <div className="home-tat-rag-row">
                                        <span
                                            className="home-tat-rag-swatch"
                                            style={{ background: '#a4262c' }}
                                            aria-hidden
                                        />
                                        <span className="ms-Label" style={{ alignSelf: 'center', color: '#605e5c' }}>
                                            {TranslateTag('@DasHomTatRagR@', props.language) || 'Red — below amber minimum'}
                                        </span>
                                    </div>
                                </>
                            )}
                            {editingSection.visualization === 'buttons' && (
                                <Dropdown
                                    label={TranslateTag('@DasHomAA@', props.language) || 'Button data'}
                                    selectedKey={editingSection.buttonKind}
                                    options={[
                                        { key: 'state', text: TranslateTag('@DasHomAB@', props.language) || 'States' },
                                        {
                                            key: 'specimentype',
                                            text: TranslateTag('@DasHomAC@', props.language) || 'Specimen types',
                                        },
                                        { key: 'tag', text: TranslateTag('@DasHomAD@', props.language) || 'Tags' },
                                    ]}
                                    onChange={(_, o) => {
                                        if (!o || o.key == null) {
                                            return;
                                        }
                                        updateSection(editingSectionId, { buttonKind: o.key });
                                    }}
                                />
                            )}
                            {editingSection.visualization === 'recentlyUsed' && (
                                <>
                                    <p className="home-dashboard-hint">
                                        {TranslateTag('@DasHomZ@', props.language) ||
                                            'These settings use list and config IDs only (no translated labels). When Test is selected, optionally restrict to specific direct tests.'}
                                    </p>
                                    <div className="home-recently-used-kinds">
                                        <div className="home-recently-used-kinds-label">
                                            {TranslateTag('@DasHomO@', props.language) || 'Entity types to include'}
                                        </div>
                                        {RECENTLY_USED_KINDS.map((k) => (
                                            <Checkbox
                                                key={k}
                                                label={recentlyUsedKindLabel(k)}
                                                checked={(editingSection.recentlyUsedKinds || []).includes(k)}
                                                onChange={(_, checked) => {
                                                    const cur = new Set(editingSection.recentlyUsedKinds || []);
                                                    if (checked) {
                                                        cur.add(k);
                                                    } else {
                                                        cur.delete(k);
                                                    }
                                                    updateSection(editingSectionId, {
                                                        recentlyUsedKinds: Array.from(cur),
                                                    });
                                                }}
                                            />
                                        ))}
                                    </div>
                                    {(editingSection.recentlyUsedKinds || []).includes('test') && (
                                        <Dropdown
                                            label={TranslateTag('@DasHomP@', props.language) || 'Direct tests'}
                                            multiSelect
                                            selectedKeys={editingSection.recentlyUsedDirectTestIds || []}
                                            options={directTestConfigOptions}
                                            onChange={(_, option) => {
                                                if (!option) {
                                                    return;
                                                }
                                                const id = String(option.key);
                                                const next = new Set(editingSection.recentlyUsedDirectTestIds || []);
                                                if (option.selected) {
                                                    next.add(id);
                                                } else {
                                                    next.delete(id);
                                                }
                                                updateSection(editingSectionId, {
                                                    recentlyUsedDirectTestIds: Array.from(next),
                                                });
                                            }}
                                        />
                                    )}
                                    {(editingSection.recentlyUsedKinds || []).includes('culture') && (
                                        <Dropdown
                                            label={TranslateTag('@DasHomCT@', props.language) || 'Culture types'}
                                            multiSelect
                                            selectedKeys={editingSection.recentlyUsedCultureTypeIds || []}
                                            options={cultureTypeListOptions}
                                            onChange={(_, option) => {
                                                if (!option) {
                                                    return;
                                                }
                                                const id = String(option.key);
                                                const next = new Set(editingSection.recentlyUsedCultureTypeIds || []);
                                                if (option.selected) {
                                                    next.add(id);
                                                } else {
                                                    next.delete(id);
                                                }
                                                updateSection(editingSectionId, {
                                                    recentlyUsedCultureTypeIds: Array.from(next),
                                                });
                                            }}
                                        />
                                    )}
                                    <TextField
                                        label={TranslateTag('@DasHomQ@', props.language) || 'Maximum recent items'}
                                        type="number"
                                        min={1}
                                        value={String(
                                            editingSection.recentlyUsedLimit != null
                                                ? editingSection.recentlyUsedLimit
                                                : DEFAULT_RECENTLY_USED_LIMIT
                                        )}
                                        onChange={(_, v) => {
                                            const n = parseInt(String(v || ''), 10);
                                            updateSection(editingSectionId, {
                                                recentlyUsedLimit:
                                                    Number.isFinite(n) && n >= 1 ? n : DEFAULT_RECENTLY_USED_LIMIT,
                                            });
                                        }}
                                    />
                                </>
                            )}
                            <Dropdown
                                label={TranslateTag('@DasHomG@', props.language) || 'Period'}
                                selectedKey={String(
                                    editingSection.timerangeUnitListItemId ?? DEFAULT_TIMERANGE_UNIT_LIST_ITEM_ID
                                )}
                                options={
                                    periodUnitOptions.length > 0
                                        ? periodUnitOptions
                                        : [
                                              {
                                                  key: DEFAULT_TIMERANGE_UNIT_LIST_ITEM_ID,
                                                  text: 'Years',
                                              },
                                          ]
                                }
                                onChange={(_, o) => {
                                    if (!o || o.key == null) {
                                        return;
                                    }
                                    updateSection(editingSectionId, {
                                        timerangeUnitListItemId: String(o.key),
                                    });
                                }}
                            />
                            <TextField
                                label={TranslateTag('@DasHomH@', props.language) || 'Period length'}
                                type="number"
                                min={1}
                                value={String(
                                    editingSection.timerangeAmount != null
                                        ? editingSection.timerangeAmount
                                        : DEFAULT_TIMERANGE_AMOUNT
                                )}
                                onChange={(_, v) => {
                                    const n = parseInt(String(v || ''), 10);
                                    updateSection(editingSectionId, {
                                        timerangeAmount: Number.isFinite(n) && n >= 1 ? n : 1,
                                    });
                                }}
                            />
                        </>
                    )}
                </div>
            </Panel>

            <ErrorMessage visible={errorStatus.visible} dismissHandler={errorCloseHandler} error={errorStatus.message} />
        </div>
    );
};

const mapStateToProps = (state) => ({
    preferences: state.config.preferences,
    lists: state.config.lists,
    graphs: state.config.graphs,
    homeDashboardGraphs: state.config.homeDashboardGraphs,
    language: state.config.language,
});

const mapDispatchToProps = (dispatch) => ({
    onHomeDashboardSaved: (homeDashboard) =>
        dispatch({ type: actionTypes.UPDATE_PREFERENCES_HOME_DASHBOARD, homeDashboard }),
});

export default connect(mapStateToProps, mapDispatchToProps)(Home);
