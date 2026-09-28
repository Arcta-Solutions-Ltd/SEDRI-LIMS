import {useState, useEffect, useMemo, useRef} from 'react';
import {connect} from 'react-redux';
import * as actionTypes from '../../../store/actions';
import './ManageListEmbedded.css';
import ListView from '../../General/ListView/ListView';
import { applyColumnLayout } from '../../../Utils/General/ApplyColumnLayout';
import { getColumnLayoutFromPreferences } from '../../../Utils/General/GetColumnLayoutFromPreferences';
import { useColumnLayoutChange } from '../../../Utils/General/useColumnLayoutChange';
import ErrorMessage from '../../General/ErrorMessage/ErrorMessage';
import MapButtonsToContextMenu from '../../../Utils/Forms/MapButtonsToContextMenu';
import ArraySorter from '../../../Utils/General/ArraySorter';
import Post from '../../../Data/Post';
import PostEvent from '../../../Data/PostEvents';
import TransformDatesInJson from '../../../Utils/Local/TransformDatesInJson';
import TranslateTag from '../../../Utils/Local/TranslateTag';
import PageHeader from '../../Crafted/Config/FormDefinition/PageHeader/PageHeader';
import FormHandler from '../FormHandler/FormHandler';
import resolveAddButtonTooltip, { translateAddButtonTooltip } from '../../../Utils/Forms/ResolveAddButtonTooltip';

/** Specimen workflow states that permit AST on culture rows (SpecimenDefault updateast). */
const AST_ELIGIBLE_SPECIMEN_STATES = new Set(['529', '532', '533', '535']);

/**
 * Picks the specimen workflow state to use for culture-row menu filtering.
 * Prefers AST-eligible state from either the query row or injected parent context so a stale
 * post-save race (526 injected over 529 from DB, or the reverse) does not hide AST.
 *
 * @param {string|number|undefined} queryStateId - {@code sp.StateId} from the culture list query.
 * @param {string|number|undefined} injectedStateId - Parent specimen state from {@link ManageRecord}.
 * @returns {string|number|undefined}
 */
function resolveRowSpecimenStateForMenus(queryStateId, injectedStateId) {
    const queryStr = queryStateId != null && queryStateId !== '' ? String(queryStateId) : '';
    const injectStr = injectedStateId != null && injectedStateId !== '' ? String(injectedStateId) : '';

    if (queryStr && AST_ELIGIBLE_SPECIMEN_STATES.has(queryStr)) {
        return queryStateId;
    }
    if (injectStr && AST_ELIGIBLE_SPECIMEN_STATES.has(injectStr)) {
        return injectedStateId;
    }
    if (injectStr) {
        return injectedStateId;
    }
    if (queryStr) {
        return queryStateId;
    }
    return undefined;
}

/**
 * Builds the {@code Parameters} array for a {@code query/filteredget} request for an embedded list.
 *
 * The primary parent key/value pair is always included. Additional key/value pairs are
 * appended from {@code recordInfo} (e.g. a test record view passes {@code Source} and
 * {@code TestName} so that instrument queries are scoped to that test). The {@code stateid}
 * field is intentionally excluded from query parameters — specimen state is injected directly
 * into each returned row by {@link dataReceivedHandler} so that client-side workflow
 * filtering ({@link FilterGroupMenuOnState}) has access to the correct specimen state.
 *
 * @param {string} parentId - The key name of the parent identifier (e.g. {@code "SpecimenId"}).
 * @param {string|number} itemId - The value of the parent identifier for the current record.
 * @param {Object|undefined} recordInfo - Optional map of additional key/value pairs from the
 *   parent record view. {@code listdata} and {@code stateid} entries are skipped.
 * @returns {Array<{Key: string, Value: string}>} The array of parameter objects ready to be
 *   sent in the filteredget request body.
 */
function buildEmbeddedListQueryParameters(parentId, itemId, recordInfo) {
    const parameters = [{ Key: parentId, Value: itemId }];
    if (recordInfo && typeof recordInfo === 'object') {
        Object.entries(recordInfo).forEach(([k, v]) => {
            if (v !== undefined && v !== null && String(k).toLowerCase() !== 'listdata' && String(k).toLowerCase() !== 'stateid') {
                parameters.push({ Key: k, Value: String(v) });
            }
        });
    }
    return parameters;
}

/**
 * Ensures list row fields are available under lowercase keys so grid columns match special-query payloads.
 * @param {Object} row - A single list row from the server.
 * @returns {Object} The row with lowercase aliases for any PascalCase properties.
 */
function normalizeEmbeddedListRow(row) {
    if (!row || typeof row !== 'object') {
        return row;
    }

    const normalized = { ...row };
    Object.entries(row).forEach(([key, value]) => {
        const lowerKey = key.toLowerCase();
        if (normalized[lowerKey] === undefined) {
            normalized[lowerKey] = value;
        }
    });
    return normalized;
}

/**
 * Resolves the parent specimen workflow state for embedded list row injection.
 * Prefers {@code props.stateid} from {@link ManageRecord} (derived from fresh list row data)
 * over {@code props.selectedRecord}, which can lag after Add Culture save.
 *
 * @param {Object} props - ManageListEmbedded props.
 * @returns {string|number|undefined} Specimen state id for workflow menu filtering.
 */
function resolveSpecimenContextStateId(props) {
    const refState = props.specimenContextStateIdRef?.current;
    if (refState !== undefined && refState !== null && refState !== '') {
        return refState;
    }

    if (props.stateid !== undefined && props.stateid !== null && props.stateid !== '') {
        return props.stateid;
    }

    const sr = props.selectedRecord;
    if (sr && typeof sr === 'object') {
        const fromSelected = sr.stateid ?? sr.StateId;
        if (fromSelected !== undefined && fromSelected !== null && fromSelected !== '') {
            return fromSelected;
        }
    }

    return undefined;
}

/**
 * Injects parent specimen context (type, laboratory, workflow state) onto embedded list rows
 * so {@link FilterMenusOnState} and {@link FilterGroupMenuOnState} evaluate the correct state.
 *
 * @param {Array<Object>} rows - Normalized list rows from the embedded query.
 * @param {Object} props - ManageListEmbedded props.
 * @returns {Array<Object>} Rows enriched with specimen context fields.
 */
function enrichEmbeddedRowsWithSpecimenContext(rows, props) {
    if (!Array.isArray(rows) || rows.length === 0) {
        return rows;
    }

    const sr = props.selectedRecord;
    const specimenTypeId = sr?.specimentypeid ?? sr?.SpecimenTypeId;
    const laboratoryId = sr?.laboratoryid ?? sr?.LaboratoryId;
    const stateId = resolveSpecimenContextStateId(props);

    if (specimenTypeId === undefined && laboratoryId === undefined && stateId === undefined) {
        return rows;
    }

    return rows.map((item) => {
        const next = { ...item };
        if (specimenTypeId !== undefined) {
            next.specimentypeid = specimenTypeId;
        }
        if (laboratoryId !== undefined) {
            next.laboratoryid = laboratoryId;
        }
        const queryStateId = item.stateid ?? item.StateId;
        const resolvedStateId = resolveRowSpecimenStateForMenus(queryStateId, stateId);
        if (resolvedStateId !== undefined) {
            const queryStateStr = queryStateId != null && queryStateId !== '' ? String(queryStateId) : '';
            const resolvedStateStr = String(resolvedStateId);
            if (queryStateStr !== '' && queryStateStr !== resolvedStateStr) {
                // #region agent log
                console.log('DEBUG', {
                    location: 'ManageListEmbedded.js:enrichEmbeddedRowsWithSpecimenContext',
                    message: 'Resolved culture row stateid for workflow menu filtering',
                    data: {
                        viewKey: props.config?.Name ?? props.config?.QueryName,
                        cultureId: item.id ?? item.Id,
                        queryStateId,
                        injectedStateId: stateId,
                        resolvedStateId,
                    },
                    timestamp: Date.now(),
                });
                // #endregion
            }
            next.stateid = resolvedStateId;
        }
        return next;
    });
}

const ManageListEmbedded = (props) => {

    const latestPropsRef = useRef(props);
    latestPropsRef.current = props;

    const [listData, setListData] = useState({test: 'test'});
    const [errorStatus, updateErrorStatus] = useState({visible: false, message: ''});
    const [selectedRecord, setSelectedRecord] = useState({});
    const [selectedIndex, setSelectedIndex] = useState({});
    const [formStartConfig, setFormStartConfig] = useState({});
    const [listVisibility, setListVisibility] = useState(true);

    let displayType = (null);

    useEffect(() => {
        const criteria = {
            Name: props.config.QueryName,
            Parameters: buildEmbeddedListQueryParameters(props.parentId, props.itemId, props.recordInfo),
        };
        Post('query/filteredget', criteria, dataReceivedHandler, errorWhenRetrievingData);
    }, [props.config.QueryName, props.itemId, props.parentId, props.refresh, props.config.GridColumns, props.recordInfo]);

    useEffect(() => {
        setListData((prev) => {
            if (!Array.isArray(prev) || prev.length === 0) {
                return prev;
            }
            return enrichEmbeddedRowsWithSpecimenContext(prev, props);
        });
    }, [
        props.stateid,
        props.selectedRecord?.stateid,
        props.selectedRecord?.StateId,
        props.selectedRecord?.specimentypeid,
        props.selectedRecord?.SpecimenTypeId,
        props.selectedRecord?.laboratoryid,
        props.selectedRecord?.LaboratoryId,
    ]);

    const errorCloseHandler = () => {
        updateErrorStatus({visible: false, message: ''});
    }

    /**
     * Handles the data returned from the embedded list query.
     *
     * After transforming dates and localising Yes/No values, the handler enriches every
     * row with context fields from the parent specimen record. {@code stateid} prefers
     * {@code props.stateid} from {@link ManageRecord} (fresh list row) over
     * {@code props.selectedRecord}, which can lag after Add Culture save.
     * Three fields are injected when present:
     *
     * - {@code specimentypeid} — required by {@code GetEntryStatesForRecord} to select the
     *   correct laboratory entry for workflow resolution.
     * - {@code laboratoryid} — required alongside {@code specimentypeid} to look up the
     *   active workflow ID for the current lab.
     * - {@code stateid} — the **specimen's** workflow state. Culture and isolate rows have
     *   their own record-level state IDs, but workflow-entry-state checks (both for per-row
     *   menu items via {@link FilterMenusOnState} and for group-header menu items via
     *   {@link FilterGroupMenuOnState}) must evaluate against the parent specimen state.
     *   Injecting the specimen {@code stateid} here makes that state available to
     *   {@code DataList} without requiring additional props threading.
     *
     * @param {Array<Object>} data - The raw list rows returned from the server query.
     * @returns {void}
     */
    const dataReceivedHandler = (data) => {
        const activeProps = latestPropsRef.current;
        TransformDatesInJson(data);

        for (var i = 0; i < data.length; i++) {
            for (let key in data[i]) {
                data[i][key] = data[i][key] === 'Yes' ? TranslateTag("@GenYesA@", activeProps.language) : data[i][key] === 'No' ? TranslateTag("@GenNo@", activeProps.language) : data[i][key];
              }
        }
          
        const newListData = enrichEmbeddedRowsWithSpecimenContext(
            data.map(normalizeEmbeddedListRow),
            activeProps
        );

        setListData(newListData);
        if (props.onRefreshDone !== undefined) {
            props.onRefreshDone(false);
        }
    }

    const embeddedModeButtonHandler = (item, button) => {
        itemSelected(item, true);
        buttonClickHandler(button, item);
    }

    const buttonClickHandler = (button, item) => {
        const id = item.id === undefined ? item.Id : item.id;
        props.onButtonClick({ button: button, id: id, stateid: item.stateid, listdata: listData });
    }

    const errorWhenRetrievingData = (response) => {
        updateErrorStatus({visible: true, message: response.data});
    }

    const selectionChangeHandler = (selectionState) => {
        setSelectedRecord(selectionState.getSelection());
        setSelectedIndex(selectionState.getSelectedIndices());
    }

    const itemSelected = (item, selected) => {
        if (selected) {
            var arrayOfSelectedItems = [];
            arrayOfSelectedItems[0] = item;
            setSelectedRecord(arrayOfSelectedItems);
        }
    }

    let displayEditButton = false; 
    if (props.config.EditButton !== undefined && props.config.EditButton !== "") {
        const founduievent = props.uievents.filter(e => e.Name === props.config.EditButton );
        displayEditButton = founduievent.length > 0;
    }

    let displayDeleteButton = false; 
    if (props.config.DeleteButton !== undefined && props.config.DeleteButton !== "") {
        const founduievent = props.uievents.filter(e => e.Name === props.config.DeleteButton );
        displayDeleteButton = founduievent.length > 0;
    }
    const page = { pageTitle: props.title };

    const refreshAfterReturningFromForm = () => {
        props.onRefreshDone("embeddedrefresh" );
    }

    const editClickHandler = () => {
        setFormStartConfig({
            button: {UIEvent: props.config.EditButton, onFinish: 'refresh'}, 
            refresh: refreshAfterReturningFromForm,
            containerVisibility: setListVisibility
        })
    }

    const deleteClickHandler = () => {
        setFormStartConfig({
            button: {UIEvent: props.config.DeleteButton, onFinish: 'refresh'}, 
            refresh: refreshAfterReturningFromForm,
            containerVisibility: setListVisibility
        })
    }

    const menuButtons =  props.config.Buttons.filter((button) => { return button.OnSelect; });
    let unsortedMenuItems = MapButtonsToContextMenu(menuButtons, buttonClickHandler, props.config.Name);
    const menuItems = unsortedMenuItems.sort(ArraySorter("primaryAction"));

    const baseColumns = (props.config.GridColumns || []).map((column) => ({
        Key: column.Key,
        Name: column.Name,
        FieldName: column.FieldName,
        MinWidth: column.MinWidth,
        MaxWidth: column.MaxWidth,
        IsResizable: column.IsResizable !== false,
        IsCollapsible: column.IsCollapsible,
        IsSorted: column.FieldName === false,
        IsSortedDescending: column.FieldName === false,
        Highlight: column.Highlight,
    }));

    const viewKey = props.config.Name || props.config.QueryName;
    const columnLayout = useMemo(() => getColumnLayoutFromPreferences(props.preferences, viewKey), [props.preferences, viewKey]);
    const onColumnLayoutChange = useColumnLayoutChange({
        viewKey,
        columnLayout,
        onUpdate: props.onUpdatePreferencesColumnLayout,
        onSaveError: errorWhenRetrievingData,
    });

    let columns = applyColumnLayout(baseColumns, columnLayout);

    // User column preferences may omit the "menu" column; without it the header "+" never binds (DataList).
    if (props.config.AddButton && !columns.some((c) => String(c.Key || c.key || '').toLowerCase() === 'menu')) {
        const menuCol = baseColumns.find((c) => String(c.Key || c.key || '').toLowerCase() === 'menu');
        if (menuCol) {
            columns = [menuCol, ...columns];
        }
    }

    let groupMenu = props.config.GroupMenu
    if (props.passive) {
        columns = columns.filter(obj => obj.Key !== 'menu');
        groupMenu = [];
    }
    let displaySection = false;

    // Don't display at all if there's no data.
    if (listData.length > 0 || props.config.DisplayIfEmpty === true) {
        if (!props.language) {
            // #region agent log
            console.log('DEBUG', {
                location: 'ManageListEmbedded.js',
                message: 'language prop missing when rendering embedded ListView; client-side tooltips will be blank',
                data: { viewKey },
                timestamp: Date.now(),
            });
            // #endregion
        }
        const addTooltipTag = resolveAddButtonTooltip(props.config);
        const addTooltipText = translateAddButtonTooltip(addTooltipTag, props.language);
        displayType =
            <ListView
                key={viewKey}
                type={"grid"}
                parentId={props.itemId}
                columns={columns}
                listData={listData}
                menuItems={menuItems}
                itemSelected={itemSelected}
                selectedRecord={selectedIndex}
                isDataLoaded={true}
                selectionChanged={selectionChangeHandler}
                basic={true}
                refresh={props.refresh}
                displaySummary={props.config.DisplaySummary}
                addButton={props.config.AddButton}
                addButtonTooltip={addTooltipText}
                basicModeButtonHandler={embeddedModeButtonHandler}
                laboratoryConfig={props.laboratory}
                groupBy={props.config.GroupBy}
                groupText={props.config.GroupText}
                groupMenu={groupMenu}
                onClick={buttonClickHandler}
                viewName={viewKey}
                language={props.language}
                columnLayout={columnLayout}
                onColumnLayoutChange={onColumnLayoutChange}>
            </ListView>
        displaySection = true;
    } else {
        if (props.config.DisplayIfEmpty) {
            displayType = (
                <div className='managelistembedded-no-list'>
                    ----- {TranslateTag("@GenNon@", props.language)} -----
                </div>
            );
            displaySection = true;
        }
    }

    return (
        <div>
            {props.title !== undefined && props.title !== '' && displaySection ? (
                <PageHeader page={page} canEdit={displayEditButton} canDelete={displayDeleteButton} edit={editClickHandler} delete={deleteClickHandler} language={props.language}></PageHeader>
            ) : (null)}
            <div className='managerecord-section-table'>

                {displayType}
                <FormHandler startConfig={formStartConfig} showNextButton={false}></FormHandler>
                <ErrorMessage visible={errorStatus.visible} dismissHandler={errorCloseHandler} error={errorStatus.message}></ErrorMessage>
            </div>
        </div>
    )
};

const mapStateToProps = state => {
    return {
        uievents: state.config.uievents,
        forms: state.config.forms,
        pages: state.config.pages,
        lists: state.config.lists,
        language: state.config.language,
        laboratory: state.config.laboratory,
        showFullScreen: state.display.showFullScreen,
        preferences: state.config.preferences,
    };
}

const mapDispatchToProps = dispatch => {
    return {
        onFilterSelect: (value) => dispatch({type: actionTypes.SETFILTERPRESET, value: value}),
        onUpdatePreferencesColumnLayout: (viewKey, columnLayout) =>
            dispatch({ type: actionTypes.UPDATE_PREFERENCES_COLUMN_LAYOUT, viewKey, columnLayout }),
    }
};

export default connect(mapStateToProps, mapDispatchToProps)(ManageListEmbedded);
