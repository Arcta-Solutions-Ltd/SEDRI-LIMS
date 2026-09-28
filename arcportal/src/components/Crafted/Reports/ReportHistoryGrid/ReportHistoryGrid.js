import React, {useState, useEffect, useMemo} from 'react';
import { connect } from 'react-redux';
import * as actionTypes from '../../../../store/actions';
import './ReportHistoryGrid.css';
import MapButtonsToContextMenu from '../../../../Utils/Forms/MapButtonsToContextMenu';
import ListView from '../../../General/ListView/ListView';
import { applyColumnLayout } from '../../../../Utils/General/ApplyColumnLayout';
import { getColumnLayoutFromPreferences } from '../../../../Utils/General/GetColumnLayoutFromPreferences';
import { useColumnLayoutChange } from '../../../../Utils/General/useColumnLayoutChange';
import ErrorMessage from '../../../General/ErrorMessage/ErrorMessage';
import Post from '../../../../Data/Post';
import { WriteAndDownloadReport } from '../../../Reports/ReportWriter';
import TransformDatesInJson from '../../../../Utils/Local/TransformDatesInJson';
import TranslateTag from '../../../../Utils/Local/TranslateTag';
import { FormatReportData } from '../../../Reports/Functions/FormatReportData';
import StringUtils from '../../../../Utils/General/StringUtils';

const ReportHistoryGrid = (props) => {

    const [listData, setListDataState] = useState([]);
    const [selectedRecord, setSelectedRecord] = useState({});
    const [selectedIndex, setSelectedIndex] = useState({});
    const [errorStatus, setErrorStatus] = useState({visible: false, message: ''});

    let displayType = (null);

    useEffect(() => {
        const parameters = [{ Key: 'id', Value: props.id }];
        const criteria = { Name: props.config.QueryName, Parameters: parameters };
        Post('query/filteredget', criteria, dataReceivedHandler, errorWhenRetrievingData);
    }, [props.id, props.config.QueryName, props.refresh]);

    const buttonClickHandler = (button, item) => {
        const criteria = { ReportName: item.reportconfig };
        Post('config/getreport', criteria, configDataRetrieved, errorWhenRetrievingData, {id: item.id});
    }

    const configDataRetrieved = (data, extraInfo) => {
        // Parse config if it's a string (like ReportPreviewOverlay and PrintPublish do)
        let parsedConfig = data;
        if (typeof data === 'string') {
            try {
                parsedConfig = JSON.parse(data);
            } catch (e) {
                // If parsing fails, use original data
                parsedConfig = data;
            }
        }
        const parameters = [{ Key: 'id', Value: extraInfo.id }];
        const criteria = { Name: "ReportContents", Parameters: parameters};
        Post('query/filteredget', criteria, reportDataRetrieved, errorWhenRetrievingData, {config: parsedConfig});
    }

    const reportDataRetrieved = async (data, extraInfo) => {
        const reportData = JSON.parse(data.Contents)
        FormatReportData(reportData);
        const watermarkText = data.ReportApprovalId === 123 ? "" : TranslateTag("@GenNotA@", props.language);
        // Parse config if it's a string (like ReportPreviewOverlay does)
        const effectiveConfig = StringUtils.parseJsonIfString(extraInfo.config);
        await WriteAndDownloadReport(reportData, effectiveConfig, data.Name, props.language, watermarkText);
    }

    const dataReceivedHandler = (data) => {
        TransformDatesInJson(data);
        if (data.length > 0 && data[0].name === undefined && data[0].Name !== undefined) {
            for (const item of data) {
                item.name = item.Name;
                item.accessionnumber = item.AccessionNumber;
                item.lastmodifieddate = item.LastModifiedDate;
                item.reportconfig = item.ReportConfig;
                item.id = item.Id;
            }
        }
        setListDataState(data);
    }

    const errorWhenRetrievingData = (response) => {
        setErrorStatus({ visible: true, message: response });
    };

    const errorCloseHandler = () => {
        setErrorStatus({ visible: false, message: '' });
    };

    const menuButtons = [
       // { Key: 'view', Text: TranslateTag("@GenVieF@",props.language), Icon: 'RedEye', PrimaryAction: 1 },
        {Icon: "Print", Key: "printreport", PrimaryAction: 2, Text: TranslateTag("@GenPriD@",props.language)}
        ];
    const menuItems = MapButtonsToContextMenu(menuButtons, buttonClickHandler);

    const baseColumns = [
        { Key: 'column0', Name: '', FieldName: '', MinWidth: 20, MaxWidth: 20, IsResizable: true, IsCollapsible: false },
        { Key: 'column1', Name: TranslateTag("@RepNam@",props.language), FieldName: 'name', MinWidth: 180, MaxWidth: 180, IsResizable: true, IsCollapsible: false },
        { Key: 'menu', Name: '', FieldName: '', MinWidth: 120, MaxWidth: 120, IsResizable: true, IsCollapsible: false },
        { Key: 'column2', Name: TranslateTag("@SinSpeA@",props.language), FieldName: 'accessionnumber', MinWidth: 200, MaxWidth: 200, IsResizable: true, IsCollapsible: false },
        { Key: 'column3', Name: TranslateTag("@GenDatB@",props.language), FieldName: 'lastmodifieddate', MinWidth: 180, MaxWidth: 180, IsResizable: true, IsCollapsible: false },
        { Key: 'column4', Name: TranslateTag("@GenSta@",props.language), FieldName: 'reportapproval', MinWidth: 180, MaxWidth: 180, IsResizable: true, IsCollapsible: false }
    ];

    let baseColumnsFiltered = [...baseColumns];
    if (props.passive) {
        baseColumnsFiltered = baseColumnsFiltered.filter((c) => c.Key !== 'menu');
    }

    const viewKey = props.config.Name || props.config.QueryName;
    const columnLayout = useMemo(() => getColumnLayoutFromPreferences(props.preferences, viewKey), [props.preferences, viewKey]);
    const onColumnLayoutChange = useColumnLayoutChange({
        viewKey,
        columnLayout,
        onUpdate: props.onUpdatePreferencesColumnLayout,
        onSaveError: errorWhenRetrievingData,
    });

    const columns = applyColumnLayout(baseColumnsFiltered, columnLayout);
  
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

    const embeddedModeButtonHandler = (item, button) => {
        itemSelected(item, true);
        buttonClickHandler(button, item);
    }

    if (listData.length > 0) {
        displayType =
            <ListView 
                type={"grid"}
                columns={columns}
                listData={listData}
                menuItems={menuItems}
                itemSelected={itemSelected}
                selectedRecord={selectedIndex}
                selectionChanged={selectionChangeHandler}
                isDataLoaded={true}
                basic={true}
                basicModeButtonHandler={embeddedModeButtonHandler}
                viewName={viewKey}
                columnLayout={columnLayout}
                onColumnLayoutChange={onColumnLayoutChange}>
            </ListView>
    } else {
        displayType = (
            <div className='reporthistorygrid-no-list'>
                ----- {TranslateTag("@GenNon@", props.language)} -----
            </div>
        );
    }

    return (
        <div className='reporthistorygrid-content' data-testid={props.config?.Id ? `record-section-${props.config.Id}` : undefined}>
            {displayType}
            <ErrorMessage visible={errorStatus.visible} dismissHandler={errorCloseHandler} error={errorStatus.message}></ErrorMessage>
        </div>
    )
}

const mapStateToProps = state => ({
    preferences: state.config.preferences,
});

const mapDispatchToProps = dispatch => ({
    onUpdatePreferencesColumnLayout: (viewKey, columnLayout) =>
        dispatch({ type: actionTypes.UPDATE_PREFERENCES_COLUMN_LAYOUT, viewKey, columnLayout }),
});

export default connect(mapStateToProps, mapDispatchToProps)(ReportHistoryGrid);