import {useEffect, useState} from 'react';
import TextDisplay from '../../Forms/TextDisplay/TextDisplay';
import SingleLineField from '../../Forms/SingleLineField/SingleLineField';
import TranslateTag from '../../../Utils/Local/TranslateTag';
import { Icon, CompoundButton } from '@fluentui/react';
import ErrorMessage from '../../General/ErrorMessage/ErrorMessage';
import FormContentLoading from '../../Forms/FormContentLoading/FormContentLoading';
import Post from '../../../Data/Post';
import './PrintPublish.css';
import { WriteAndDownloadReport } from '../../Reports/ReportWriter';
import PostEvent from '../../../Data/PostEvents';
import SuccessMessage from '../../General/SuccessMessage/SuccessMessage';
import CurrentDate from '../../../Utils/Local/CurrentDate';
import { FormatReportData } from '../../Reports/Functions/FormatReportData';
import LaboratoryList from '../../../Classes/Laboratory/LaboratoryList';
import { connect } from 'react-redux';
import StringUtils from '../../../Utils/General/StringUtils';

/**
 * Print / Publish Specimen Report crafted page. Loads enabled report configs, fetches the
 * selected report definition, then loads {@link SpecimenRecordReport} data before publish actions
 * are enabled. Shows a loading spinner until report data is ready; Cancel remains available.
 * @param {Object} props
 * @param {Object} props.config - Page configuration (title, text).
 * @param {number|string} props.id - Specimen id for report data and publish event.
 * @param {Function} props.refresh - Callback to refresh embedded views after publish.
 * @param {string} props.language - Active language code for translations.
 * @param {number|string} [props.laboratoryid] - Laboratory id for approval watermark lookup.
 */
const PrintPublish = (props) => {

    const printIcon = <Icon iconName="Print" />;
    const publishIcon = <Icon iconName="PublisherLogo" />;

    const nameConfig = { Id: "PublishName", Type: 'singleline', Label: TranslateTag("@GenNam@", props.language) };

    const [errorStatus, updateErrorStatus] = useState({visible: false, message: ''});
    const [successStatus, setSuccessStatus] = useState({visible: false, message: ''});
    const [reportConfig, setReportConfig] = useState();
    const [reportData, setReportData] = useState([]);
    const [originalReportData, setOriginalReportData] = useState([]);
    const [reportName, setReportName] = useState();
    const [printSelector, setPrintSelector] = useState({});
    const [selectedReport, setSelectedReport] = useState(null);
    const [reportOptions, setReportOptions] = useState([]);
    const [reportLoading, setReportLoading] = useState(true);
    const [reportDataReady, setReportDataReady] = useState(false);

    useEffect(() => {
        const criteria = { Name: "reportconfiglistquery", Parameters: [{ Key: 'id', Value: 'reportlistconfig' }] };
        const reportListErrorHandler = (response) => {
            setReportLoading(false);
            errorWhenRetrievingData(response);
        };
        Post('query/filteredget', criteria, reportsListRetrieved, reportListErrorHandler);
    }, []);

    /**
     * Parses the enabled report list, auto-selects when only one report is available,
     * or stops loading when the user must pick from multiple options.
     * @param {Array|string} data - Response from reportconfiglistquery.
     */
    const reportsListRetrieved = (data) => {
        let parsedData = data;
        if (typeof data === 'string') {
            try {
                parsedData = JSON.parse(data);
            } catch (error) {
                parsedData = [];
            }
        }

        if (!Array.isArray(parsedData)) {
            parsedData = [];
        }

        const enabledReports = parsedData
            .filter(item => (item.Enabled || '').toLowerCase() === 'yes')
            .map(item => {
                const rawId = item.Id || '';
                const [, configName] = rawId.split('|');
                const key = configName || rawId;
                return {
                    key,
                    text: item.Name || key
                };
            })
            .filter(option => option.key);

        enabledReports.sort((a, b) => a.text.localeCompare(b.text));
        setReportOptions(enabledReports);

        if (enabledReports.length === 0) {
            setReportLoading(false);
            updateErrorStatus({ visible: true, message: TranslateTag('@GenErr@', props.language) || 'No enabled reports available.' });
        } else if (enabledReports.length === 1) {
            setSelectedReport(enabledReports[0].key);
        } else {
            setReportLoading(false);
        }
    };

    useEffect(() => {
        if (!selectedReport) {
            return;
        }
        setReportLoading(true);
        setReportDataReady(false);
        setReportConfig(undefined);
        setReportData([]);
        setOriginalReportData([]);
        const criteria = { ReportName: selectedReport };
        Post('config/getreport', criteria, configDataRetrieved, errorWhenRetrievingData);
    }, [selectedReport]);

    const nameChangeHandler = (key, value) => {
        setReportName(value);
    };

    const selectorChangeHandler = (key, value) => {
        setPrintSelector(value);
    };

    const reportChangeHandler = (key, value) => {
        setSelectedReport(value);
    };

    /**
     * Stores the selected report config and requests specimen report data.
     * @param {Object|string} data - Report configuration from config/getreport.
     */
    const configDataRetrieved = (data) => {
        let parsedConfig = data;
        if (typeof data === 'string') {
            try {
                parsedConfig = JSON.parse(data);
            } catch (e) {
                parsedConfig = data;
            }
        }
        setReportConfig(parsedConfig);
        const parameters = [{ Key: 'id', Value: props.id }];
        const criteria = { Name: "SpecimenRecordReport", Parameters: parameters};
        Post('query/filteredget', criteria, reportDataRetrieved, errorWhenRetrievingData);
    };

    /**
     * Populates report name and formatted data; marks the form ready for publish actions.
     * @param {string} data - JSON string from SpecimenRecordReport query.
     */
    const reportDataRetrieved = (data) => {
        setOriginalReportData(data);
        let formattedData = JSON.parse(data);
        FormatReportData(formattedData);
        var accessionNumber = formattedData.Standard.filter((f) => f.Key === "accessionnumber");
        setReportName(accessionNumber[0].Value + " - " + CurrentDate());
        setReportData(formattedData);
        setReportDataReady(true);
        setReportLoading(false);
    };

    /**
     * Surfaces a retrieval error and clears the loading state.
     * @param {Object} response - Error response from Post/PostEvent.
     */
    const errorWhenRetrievingData = (response) => {
        setReportLoading(false);
        setReportDataReady(false);
        updateErrorStatus({visible: true, message: response.data});
    };

    /**
     * Publishes or prints the specimen report when report data has finished loading.
     * @param {boolean} print - When true, also downloads the report PDF after publish.
     */
    const publishClickHandler = (print) => {
        if (reportLoading || !reportDataReady) {
            return;
        }
        if (reportConfig !== undefined && reportData !== undefined) {
            const reportToUse = selectedReport || (reportOptions.length === 1 ? reportOptions[0].key : 'DefaultSpecimenReport');
            const event = { Event: 'specimenreport', Id: props.id, Name: reportName, Contents: originalReportData, Config: reportToUse, printed: print};
            PostEvent(event, () => reportPublishedSuccessfully(print), errorWhenRetrievingData );
        }
    };

    /**
     * After publish succeeds, optionally downloads the report PDF. When the laboratory requires
     * report approval ({@link Laboratory.ApproveReports}), applies the not-approved watermark.
     * @param {boolean} print - When true, generates and downloads the PDF.
     */
    const reportPublishedSuccessfully = async (print) => {
        if (print) {
            const labConfig = new LaboratoryList(props.laboratory);
            const currentLaboratory = labConfig.getLaboratory(props.laboratoryid);
            const watermarkText =
                currentLaboratory?.ApproveReports === 'Yes' ? TranslateTag('@GenNotA@', props.language) : '';
            const effectiveConfig = StringUtils.parseJsonIfString(reportConfig);
            await WriteAndDownloadReport(reportData, effectiveConfig, reportName, props.language, watermarkText);
            setSuccessStatus({visible: true, message: TranslateTag('@RepRepB@', props.language)});
        } else {
            setSuccessStatus({visible: true, message: TranslateTag('@RepRepA@', props.language)});
        }
        props.refresh("embeddedrefresh");
    };

    const errorCloseHandler = () => {
        updateErrorStatus({visible: false, message: ''});
    };

    nameConfig.value = reportName;

    const publishActionsEnabled = reportDataReady && !reportLoading;

    const reportSelectionConfig = {
        Id: "ReportSelection",
        Type: "dropdown",
        Label: TranslateTag("@GenRep@", props.language) || "Report",
        Required: true,
        Options: reportOptions,
        value: selectedReport,
        Disabled: reportLoading
    };

    return (
        <div id="printpublish-root" className="printpublish-root app-crafted-content">
            <div className='app-crafted-title'>{props.config.PageTitle}</div>
            <div className='app-crafted-headertext'>
                <TextDisplay text={props.config.Text}></TextDisplay>
            </div>

            {reportLoading ? (
                <div id="printpublish-loading" className="printpublish-loading">
                    <FormContentLoading active={true} />
                </div>
            ) : (
                <>
                    <div className="app-formcolumn">
                        {reportOptions.length > 0 && (
                            <SingleLineField key="ReportSelection" config={reportSelectionConfig} changeHandler={reportChangeHandler}></SingleLineField>
                        )}
                        <SingleLineField key="PublishName" config={nameConfig} changeHandler={nameChangeHandler}></SingleLineField>
                    </div>

                    <br></br>
                    <div className="printpublish-buttons">
                        <CompoundButton
                            id="printpublish-publish-btn"
                            primary
                            disabled={!publishActionsEnabled}
                            onClick={() => publishClickHandler(false)}
                        >
                            <div className="printpublish-button">
                                {publishIcon}
                                <br></br>
                                {TranslateTag("@RepPub@", props.language)}
                            </div>
                        </CompoundButton>
                        <div className="printpublish-separator"></div>
                        <CompoundButton
                            id="printpublish-print-publish-btn"
                            primary
                            disabled={!publishActionsEnabled}
                            onClick={() => publishClickHandler(true)}
                        >
                            <div className="printpublish-button">
                                {publishIcon}  {printIcon}
                                <br></br>
                                {TranslateTag("@RepPriA@", props.language)}
                            </div>
                        </CompoundButton>
                    </div>
                </>
            )}

            <div id="printpublish-error">
                <ErrorMessage visible={errorStatus.visible} dismissHandler={errorCloseHandler} error={errorStatus.message}></ErrorMessage>
            </div>
            <SuccessMessage visible={successStatus.visible} message={successStatus.message}></SuccessMessage>

        </div>
    );
};

const mapStateToProps = state => {
    return {
        laboratory: state.config.laboratory
    };
};

export default connect(mapStateToProps)(PrintPublish);
