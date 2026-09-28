import React, { useEffect, useState, useRef } from 'react';
import { connect } from 'react-redux';
import PdfViewer from '../../General/PDFViewer/pdfViewer';
import PdfEmbeddedViewer from '../../General/PdFEmbeddedViewer/PdfEmbeddedViewer';
import ErrorMessage from '../../General/ErrorMessage/ErrorMessage';
import FormContentLoading from '../../Forms/FormContentLoading/FormContentLoading';
import Post from '../../../Data/Post';
import { FormatReportData } from '../../Reports/Functions/FormatReportData';
import { WriteReport } from '../../Reports/ReportWriter';
import StringUtils from '../../../Utils/General/StringUtils';
import { useRunOnce } from '../../../Utils/General/UseRunOnce';
import './PrintPreview.css';

const PrintPreview = (props) => {

    const [errorStatus, updateErrorStatus] = useState({ visible: false, message: '' });
    const [reportData, setReportData] = useState();
    const [configuration, setConfiguration] = useState();
    const [isLoading, setIsLoading] = useState(false);
    const requestIdRef = useRef(0);
    const configurationRef = useRef();

    const resolveLayoutConfig = (explicitConfig) => {
        const raw = explicitConfig ?? configurationRef.current ?? configuration;
        if (raw == null) {
            return undefined;
        }
        return StringUtils.parseJsonIfString(raw);
    };

    useRunOnce(() => {
        const criteria = { ReportName: 'DefaultSpecimenReport' };
        Post('config/getreport', criteria, configDataRetrieved, errorWhenRetrievingData);
    }, []);

    useEffect(() => {
        if (!configuration) {
            return;
        }
        getReportData(configurationRef.current ?? configuration);
    }, [props.reportConfiguration, props.id, configuration]);

    /**
     * Stores the default report configuration and triggers the first report fetch.
     * @param {Object|string} data - Report configuration from config/getreport.
     */
    const configDataRetrieved = (data) => {
        configurationRef.current = data;
        setConfiguration(data);
        getReportData(data);
    };

    /**
     * Begins loading report data for preview, clearing stale canvas content and tracking the request id.
     * @param {Object|string} [layoutConfig] - Report layout configuration; required before fetching data.
     */
    const getReportData = (layoutConfig) => {
        const resolvedConfig = resolveLayoutConfig(layoutConfig);
        if (!resolvedConfig) {
            return;
        }

        const requestId = ++requestIdRef.current;
        setIsLoading(true);
        setReportData(undefined);

        if (props.history) {
            const criteria = { Parameters: [{ Key: 'Id', Value: props.id }], Name: 'ReportContents' };
            Post(
                'query/filteredget',
                criteria,
                (response, reportConfig) => historicReportDataRetrieved(response, reportConfig, requestId),
                (response) => errorWhenRetrievingData(response, requestId),
                { config: resolvedConfig, requestId }
            );
        } else {
            const criteria = { ...props.reportConfiguration, Id: props.id };
            Post(
                'report/getreportwithamendments',
                criteria,
                (response, reportConfig) => reportDataRetrieved(response, reportConfig, requestId),
                (response) => errorWhenRetrievingData(response, requestId),
                resolvedConfig
            );
        }
    };

    /**
     * Parses historic report contents and renders the PDF preview.
     * @param {Object} data - Report history row containing Contents JSON.
     * @param {Object} reportConfig - Extra info passed through Post, including config.
     * @param {number} requestId - Id of the in-flight request; stale responses are ignored.
     */
    const historicReportDataRetrieved = (data, reportConfig, requestId) => {
        if (requestId !== requestIdRef.current) {
            return;
        }
        const formattedData = JSON.parse(data.Contents);
        createReport(formattedData, reportConfig.config, requestId);
    };

    /**
     * Parses live report data and renders the PDF preview.
     * @param {string} data - JSON string from getreportwithamendments.
     * @param {Object|string} reportConfig - Report layout configuration.
     * @param {number} requestId - Id of the in-flight request; stale responses are ignored.
     */
    const reportDataRetrieved = (data, reportConfig, requestId) => {
        if (requestId !== requestIdRef.current) {
            return;
        }
        const formattedData = JSON.parse(data);
        createReport(formattedData, reportConfig, requestId);
    };

    /**
     * Formats report data and generates the PDF preview document.
     * @param {Object} formattedData - Parsed report payload.
     * @param {Object|string} reportConfig - Report layout configuration.
     * @param {number} requestId - Id of the in-flight request; stale responses are ignored.
     */
    const createReport = async (formattedData, reportConfig, requestId) => {
        if (requestId !== requestIdRef.current) {
            return;
        }

        FormatReportData(formattedData);

        const effectiveConfig = resolveLayoutConfig(reportConfig);
        if (!effectiveConfig) {
            setIsLoading(false);
            updateErrorStatus({
                visible: true,
                message: 'Report layout is still loading. Please try again.',
            });
            return;
        }

        try {
            const report = await WriteReport(formattedData, effectiveConfig, 'printpreview', props.language);

            if (requestId !== requestIdRef.current) {
                return;
            }

            setReportData({ ...report });
            setIsLoading(false);
        } catch (err) {
            if (requestId !== requestIdRef.current) {
                return;
            }
            setIsLoading(false);
            updateErrorStatus({ visible: true, message: err?.message ?? String(err) });
        }
    };

    /**
     * Surfaces a retrieval error and clears the loading state for the active request.
     * @param {Object} response - Error response from Post.
     * @param {number} [requestId] - Id of the in-flight request; omitted errors always apply.
     */
    const errorWhenRetrievingData = (response, requestId) => {
        if (requestId !== undefined && requestId !== requestIdRef.current) {
            return;
        }
        setIsLoading(false);
        updateErrorStatus({ visible: true, message: response.data });
    };

    const errorCloseHandler = () => {
        updateErrorStatus({ visible: false, message: '' });
    };

    let pdfViewer = null;
    if (reportData !== undefined && !isLoading) {
        if (props.embed) {
            pdfViewer = <PdfEmbeddedViewer data={reportData}></PdfEmbeddedViewer>;
        } else {
            pdfViewer = <PdfViewer data={reportData}></PdfViewer>;
        }
    }

    return (
        <div id="printpreview-root" className="printpreview-root managelist-content">
            {isLoading ? (
                <div id="printpreview-loading" className="printpreview-loading" data-testid="printpreview-loading">
                    <FormContentLoading active={true} delayMs={0} />
                </div>
            ) : null}
            {pdfViewer}
            <ErrorMessage
                visible={errorStatus.visible}
                dismissHandler={errorCloseHandler}
                error={errorStatus.message}
            ></ErrorMessage>
        </div>
    );
};

const mapStateToProps = state => {
    return {
        language: state.config.language
    };
};

export default connect(mapStateToProps)(PrintPreview);
