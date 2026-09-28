import React, { useState, useEffect, useRef, useCallback } from 'react';
import { connect } from 'react-redux';
import ManageListEmbedded from '../ManageListEmbedded/ManageListEmbedded';
import Post from '../../../Data/Post';
import GeneralViewer from '../../General/GeneralViewer/GeneralViewer';
import CraftedRegionFactory from '../../Crafted/CraftedRegionFactory';
import ManageRecordDates from '../ManageRecord/ManageRecordDates';
import { PutButtonsIntoPassiveMode } from '../../../Utils/Forms/GetVisibleButtons';
import TopbarMenu from '../../General/TopbarMenu/TopBarMenu';
import PrintPreview from '../PrintPreview/PrintPreview';
import { Separator } from '@fluentui/react';
import './ViewRecord.css';
import ErrorMessage from '../../General/ErrorMessage/ErrorMessage';
import { findRecordViewDefinition } from '../../../Utils/Configuration/resolveRecordViewDefinition';


const ViewRecord = (props) => {

    const [rxdItemId, setRxdItemId] = useState(props.itemId);
    const [itemId, setItemId] = useState(props.itemId);
    const [regionData, _setRegionData] = useState([]); const regionDataRef = useRef(regionData);
    const [displayMode, setDisplayMode] = useState("");

    const errorWhenRetrievingData = (response) => {
    };

    const configErrorDismiss = () => {};

    const setRegionData = data => {
        regionDataRef.current = data;
        _setRegionData(data);
    };

    const viewDataRetrievedSuccessfully = useCallback((data, index) => {

        ManageRecordDates(data);
        var newRegionData = [...regionDataRef.current];
        newRegionData[index] = {...data};
        setRegionData(newRegionData);

        // Issue query for next standard (non list-view) region, if there is one.
        var currentView = findRecordViewDefinition(props.type, props.recordviews);

        var resultantItemId = itemId;
        if (props.itemId !== rxdItemId) {
            setItemId(props.itemId);
            setRxdItemId(props.itemId);
            resultantItemId = props.itemId;
        }

        const regions = currentView?.Regions;
        if (
            regions &&
            regions.length > index + 1 &&
            regions[index + 1]?.QueryName
        ) {
            const criteria = { Name: regions[index + 1].QueryName, Parameters: [{ Key: 'id', Value: resultantItemId }] };
            Post('query/filteredget', criteria, viewDataRetrievedSuccessfully, errorWhenRetrievingData, index + 1);
        }
    }, [rxdItemId, itemId, props.itemId, props.recordviews, props.views, props.type]);

    useEffect(() => {
        var newView = findRecordViewDefinition(props.type, props.recordviews);

        if (newView?.Regions?.length > 0 && newView.Regions[0].QueryName !== '') {
            const criteria = { Name: newView.Regions[0].QueryName, Parameters: [{ Key: 'id', Value: itemId }]};
            Post('query/filteredget', criteria, viewDataRetrievedSuccessfully, errorWhenRetrievingData, 0);
        }

        if (props.reportConfiguration?.displayId !== undefined) {
            setDisplayMode("printpreview");
        }
    }, [props.recordviews, props.views, props.type, viewDataRetrievedSuccessfully, itemId, props.reportConfiguration]);

    const getListView = (listViewName) => {
        return props.views.filter((view) => {
            return view.Name === listViewName;
        })[0];
    }

    const clickButtonHandler = (button, id, recordInfo) => {
        const action = props.uievents.filter(a => a.Name === button.UIEvent);
        if (action[0].Type === 'printpreview')
        {
            const newDisplayMode = displayMode == "printpreview" ? "normal" : "printpreview";
            setDisplayMode(newDisplayMode)
        }
    }

    const buttonClickHandler = (button) => {
        clickButtonHandler(button, itemId);
    }

    var viewConfig = findRecordViewDefinition(props.type, props.recordviews);

    if (!viewConfig) {
        return (
            <div className='viewrecord-page'>
                <ErrorMessage
                    visible={true}
                    dismissHandler={configErrorDismiss}
                    error="Record view configuration is not available for this account."
                />
            </div>
        );
    }

    for (var region of viewConfig.Regions) {
        if (region.Type === 'listview' && region.ListViewName !== '') {
            region.ListView = getListView(region.ListViewName);
        }
    }

    let visibleButtons = PutButtonsIntoPassiveMode(viewConfig.Buttons);
    const passive = visibleButtons.length == 0;

    let mainContent;
    if (displayMode == "printpreview") {
        const configDisplayId = props.reportConfiguration?.displayId;
        const history = configDisplayId !== undefined;
        const displayId  = history ? configDisplayId : itemId;
        mainContent = <PrintPreview id={displayId} reportConfiguration={props.reportConfiguration} history={history}></PrintPreview>
    } else {
        mainContent = (
            <div className='viewrecord-content'>
                {viewConfig.Regions.map((region, index) => {

                    switch (region.Type) {
                        case "standard":
                            if (regionData[index] !== undefined) {
                                return (<GeneralViewer key={region.Id} data={regionData[index]} language={props.language}></GeneralViewer>)
                            } else return (null) 
                        case "listview":
                            return (
                                <div key={region.Id}>
                                    {region.Title !== undefined && region.Title !== '' ? (
                                        <div className='viewrecord-sectiontitle'>
                                            {region.Title}
                                        </div>
                                    ) : (null)}
                                    <div>
                                        <ManageListEmbedded
                                            config={region.ListView}
                                            parentId={region.ListView.ParentId}
                                            itemId={itemId}
                                            toggleFullScreen={false}
                                            region={index}
                                            refresh={false}
                                            passive={true}>
                                        </ManageListEmbedded>
                                    </div>
                                </div>
                            )
                        case "crafted":
                            return(
                                <div key={region.Id}>
                                    {region.Title !== undefined && region.Title !== '' ? (
                                    <div className='viewrecord-sectiontitle'>
                                        {region.Title}
                                    </div>
                                    ) : (null)}
                                    <div className='viewrecord-section-table'>
                                        <CraftedRegionFactory
                                            config={region}
                                            region={index}
                                            refresh={false}
                                            passive={true}
                                            suppressRowMenus={region.Name?.toLowerCase() !== 'directtestsgrid'}
                                            language={props.language}
                                            data={regionData}
                                            lists={props.lists}
                                            forms={props.forms}
                                            pages={props.pages}
                                            id={itemId}>
                                        </CraftedRegionFactory>
                                    </div>
                                </div>
                                
                            )
                        default: return (null)
                    }
                })}
            </div>
        );
    }

    const topBarMenu = <TopbarMenu
        suppressFarItems={true}
        embeddedInForm={true}
        showFilterIcon={false}
        displayGridView={false}
        buttons={visibleButtons}
        language={props.language}
        clickButton={buttonClickHandler}>
    </TopbarMenu>

    return (
        <div className='viewrecord-page' data-testid='viewrecord-page'>
            <div className='viewrecord-titlebar'>
                <div className='viewrecord-title'>
                    {viewConfig.Title}
                </div>
                {passive !== true ? (
                    <div className='viewrecord-topbar-menu' data-testid="viewrecord-topbar-menu">
                        {topBarMenu}
                        <div className='viewrecord-topbar-separator'>
                            <Separator></Separator>
                        </div>
                    </div>
                ) : (null)}
            </div>
            {mainContent}
        </div>
    );
};

const mapStateToProps = state => {
    return {
        currentView: state.display.currentView,
        views: state.config.views,
        recordviews: state.config.recordviews,
        uievents: state.config.uievents,
        forms: state.config.forms,
        pages: state.config.pages,
        lists: state.config.lists,
        language: state.config.language
    };
}

export default connect(mapStateToProps)(ViewRecord);
