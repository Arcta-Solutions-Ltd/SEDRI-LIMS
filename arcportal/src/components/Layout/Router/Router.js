import React, {useEffect, useMemo, useState} from 'react';
import {connect} from 'react-redux';
import * as actionTypes from '../../../store/actions';
import DirectTestsList from '../../Crafted/Specimen/DirectTestsList/DirectTestsList';
import ManageList from '../../Containers/ManageList/ManageList';
import ManageRecord from '../../Containers/ManageRecord/ManageRecord';
// import MultiList from '../../Containers/MultiList/MultiList';
import Login from '../../Security/Login/Login';
import Graph from '../../Containers/Graph/Graph';
import Home from '../Home/Home';
import ReportingGrid from '../../Containers/ReportingGrid/ReportingGrid';
import { viewNameMatchesCurrent } from '../../../Utils/Configuration/resolveRecordViewDefinition';

const Router = (props) => {

    const [data, setData] = useState();
    const [childRecordView, setChildRecordView] = useState(null);

    /**
     * Resolve which view config drives the shell. Synchronous resolution (useMemo) replaces useState + useEffect so
     * we do not lag one frame behind `currentView`. When Home Recently Used sets `homeNavigation` before or without
     * a matching `currentView` in the same paint, prefer `recordviews` lookup by `homeNavigation.viewName` first so we
     * do not briefly treat the route as `home` and skip `ManageRecord` + record id.
     */
    const resolvedViewConfig = useMemo(() => {
        const hn = props.homeNavigation;
        const findIn = (list, target) =>
            list?.find((view) => viewNameMatchesCurrent(view.Name, target));

        if (hn?.mode === 'record' && hn.viewName) {
            const fromNav = findIn(props.recordviews, hn.viewName);
            if (fromNav !== undefined) {
                return fromNav;
            }
        }

        const cv = props.currentView;
        if (cv == null || cv === '') {
            return undefined;
        }
        let newView = findIn(props.views, cv);
        if (newView === undefined) {
            newView = findIn(props.recordviews, cv);
        }
        if (newView === undefined) {
            newView = findIn(props.reportinggrids, cv);
        }
        return newView;
    }, [props.currentView, props.views, props.recordviews, props.reportinggrids, props.homeNavigation]);

    const viewConfig = resolvedViewConfig ?? { Type: "login" };

    useEffect(() => {
        if (resolvedViewConfig === undefined) {
            return;
        }
        const hn = props.homeNavigation;
        if (hn && hn.viewName === resolvedViewConfig.Name && hn.mode === 'tests' && hn.listData) {
            setData(hn.listData);
        } else if (resolvedViewConfig.Name !== "specimens") {
            setData(undefined);
        }
    }, [props.currentView, props.views, props.recordviews, props.reportinggrids, props.homeNavigation, resolvedViewConfig]);

    useEffect(() => {
        setChildRecordView(null);
    }, [props.currentView]);

    const toggleFullScreenHandler = () => {
        props.onToggleFullScreen();
    };

    const homeClickHandler = (data) => {
        props.homeClick();
        setData(data);
    }
  
    const hn = props.homeNavigation;
    const testsListData =
        hn && viewNameMatchesCurrent(hn.viewName, viewConfig.Name) && hn.mode === 'tests' && hn.listData != null
            ? hn.listData
            : data;
    const homeReturnFromTests =
        typeof props.onReturnToHome === 'function' &&
        hn &&
        viewNameMatchesCurrent(hn.viewName, viewConfig.Name) &&
        hn.mode === 'tests'
            ? props.onReturnToHome
            : undefined;

    const onNavigateToRecordView = (type, id, recordInfo) => {
        setChildRecordView({ type, id, recordInfo });
    };

    let viewToDisplay;
    if (childRecordView) {
        viewToDisplay = (
            <ManageRecord
                itemId={childRecordView.id}
                type={childRecordView.type}
                recordInfo={childRecordView.recordInfo}
                cancel={() => setChildRecordView(null)}
                passive={false}
                toggleFullScreen={toggleFullScreenHandler}
            />
        );
    } else if (viewConfig.Name === "graphs") {
        viewToDisplay = <Graph 
                            config={viewConfig}
                            toggleFullScreen={toggleFullScreenHandler}
                        ></Graph>
    } else {
        switch (viewConfig.Type.toLowerCase()) {
            case "directtestslist":
                viewToDisplay = <DirectTestsList
                                    config={viewConfig}
                                    data={testsListData}
                                    homeReturnHandler={homeReturnFromTests}
                                    toggleFullScreen={toggleFullScreenHandler}
                                    onNavigateToRecordView={onNavigateToRecordView}>
                                </DirectTestsList>
                break;
            case "managelist":
            case "managehierarchylist":
                viewToDisplay = <ManageList
                                    config={viewConfig}
                                    data={data}
                                    toggleFullScreen={toggleFullScreenHandler}>
                                </ManageList>
                break;
            // case "multilist":
            //     viewToDisplay = <MultiList
            //                         config={viewConfig}
            //                         toggleFullScreen={toggleFullScreenHandler}>
            //                     </MultiList>
            //     break;
            case "recordview": {
                /** Deep link when `homeNavigation` matches this resolved record view (`currentView` can lag one frame). */
                const homeRecord =
                    hn &&
                    hn.mode === 'record' &&
                    hn.itemId != null &&
                    hn.itemId !== '' &&
                    resolvedViewConfig &&
                    viewNameMatchesCurrent(hn.viewName, resolvedViewConfig.Name)
                        ? hn
                        : null;
                const recordItemId = homeRecord ? homeRecord.itemId : undefined;
                const recordCancel =
                    homeRecord && typeof props.onReturnToHome === 'function'
                        ? () => props.onReturnToHome()
                        : undefined;
                viewToDisplay = (
                    <ManageRecord
                        config={viewConfig}
                        type={viewConfig.Name}
                        itemId={recordItemId}
                        recordInfo={
                            homeRecord
                                ? { fromHome: true, ...(homeRecord.recordInfo || {}) }
                                : undefined
                        }
                        cancel={recordCancel}
                        passive={false}
                        toggleFullScreen={toggleFullScreenHandler}
                    />
                );
                break;
            }
            case "reportgrid":
                viewToDisplay = <ReportingGrid
                                    config={viewConfig}
                                    language={props.language}
                                    toggleFullScreen={toggleFullScreenHandler}>
                                </ReportingGrid>
                break;
            case "home":
                viewToDisplay = (
                    <Home
                        onClick={homeClickHandler}
                        language={props.language}
                        laboratorySelected={props.laboratorySelected}
                        onRecentlyUsedNavigate={props.onRecentlyUsedNavigate}
                    />
                );
                break;
            default:
                viewToDisplay = <Login successfulLogin={props.successfulLogin} language={props.language}></Login>
        }
    }
    
    return (
        <React.Fragment>
            {viewToDisplay}
        </React.Fragment>
    )
};

const mapStateToProps = state => {
    return {
        views: state.config.views,
        recordviews: state.config.recordviews,
        reportinggrids: state.config.reportinggrids,
        language: state.config.language
    };
}

const mapDispatchToProps = dispatch => {
    return {
        onToggleFullScreen: () => dispatch({type: actionTypes.TOGGLEFULLSCREEN})
    }
};

export default connect(mapStateToProps, mapDispatchToProps)(Router);