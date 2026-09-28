import React, { useState, useEffect } from 'react';
import { IconButton } from '@fluentui/react';
import PdfViewer from '../../General/PDFViewer/pdfViewer';
import './ReportPreviewOverlay.css';
import Post from '../../../Data/Post';
//import ErrorMessage from '../../General/ErrorMessage/ErrorMessage';
import { connect } from 'react-redux';
import { WriteReport } from '../ReportWriter';
import { FormatReportData } from '../Functions/FormatReportData';
import StringUtils from '../../../Utils/General/StringUtils';


const generateTestDataFromConfig = (config) => {
    // Hardcoded group key - easy to remove later and replace with actual group name
    const GROUP_KEY = "Organisms";
    
    const standardEntries = [];
    const added = new Set();
    const tableEntries = [];
    const tableKeys = new Set();

    // Helper function to generate table rows from width specification
    const generateTableRows = (width) => {
        let columnCount = 1;
        if (Array.isArray(width)) {
            columnCount = width.length > 0 ? width.length : 1;
        } else if (typeof width === 'string' || typeof width === 'number') {
            const widthStr = String(width);
            const parts = widthStr.split('|').map(p => p.trim()).filter(p => p.length > 0);
            columnCount = parts.length > 0 ? parts.length : 1;
        }
        const row = Array(columnCount).fill('X').join('|');
        return [row, row, row];
    };

    // Helper function to extract standard fields from a section
    const extractStandardFieldsFromSection = (section, standardEntries, addedSet) => {
        const addKey = (key) => {
            if (!key) return;
            const normalized = String(key);
            if (addedSet.has(normalized)) return;
            addedSet.add(normalized);
            standardEntries.push({ Key: normalized, Value: 'X' });
        };

        const col1 = section?.Column1?.Fields;
        if (Array.isArray(col1)) {
            col1.forEach(f => addKey(f?.Value));
        }
        const col2 = section?.Column2?.Fields;
        if (Array.isArray(col2)) {
            col2.forEach(f => addKey(f?.Value));
        }
        const col = section?.Column?.Fields;
        if (Array.isArray(col)) {
            col.forEach(f => addKey(f?.Value));
        }
    };

    // Helper function to extract table data from a section
    const extractTableFromSection = (section, tableEntries, tableKeysSet) => {
        const sectionType = section?.Type ? String(section.Type).toLowerCase() : '';
        if (sectionType !== 'table') return;

        const key = section?.Data ? String(section.Data) : '';
        if (!key || tableKeysSet.has(key)) return;

        const rows = generateTableRows(section?.Width);
        tableKeysSet.add(key);
        tableEntries.push({ Key: key, Rows: rows });
    };

    // Helper function to process sections and extract Standard and Tables data
    const processGroupSections = (sections) => {
        const groupStandardEntries = [];
        const groupAdded = new Set();
        const groupTableEntries = [];
        const groupTableKeys = new Set();

        sections.forEach(section => {
            extractStandardFieldsFromSection(section, groupStandardEntries, groupAdded);
            extractTableFromSection(section, groupTableEntries, groupTableKeys);
        });

        return {
            Standard: groupStandardEntries,
            Tables: groupTableEntries
        };
    };

    // Helper function for header/footer line processing (different from section field extraction)
    const addKey = (key) => {
        if (!key) return;
        const normalized = String(key);
        if (added.has(normalized)) return;
        added.add(normalized);
        standardEntries.push({ Key: normalized, Value: 'X' });
    };

    const headerLines = Array.isArray(config?.Header?.Lines) ? config.Header.Lines : [];
    headerLines.forEach(line => addKey(line?.Field));
    const footerLines = Array.isArray(config?.Footer?.Lines) ? config.Footer.Lines : [];
    footerLines.forEach(line => addKey(line?.Field));


    const contents = Array.isArray(config?.Contents) ? config.Contents : [];
    const groupedSections = new Map(); // Map<groupName, sections[]>
    
    contents.forEach(section => {
        // Skip sections with Group attribute during top-level processing
        const groupName = section?.Group;
        if (groupName) {
            // Collect grouped sections for later processing
            const normalizedGroupName = String(groupName);
            if (!groupedSections.has(normalizedGroupName)) {
                groupedSections.set(normalizedGroupName, []);
            }
            groupedSections.get(normalizedGroupName).push(section);
            return; // Skip top-level processing
        }

        // Process ungrouped sections at top level using helper functions
        extractStandardFieldsFromSection(section, standardEntries, added);
        extractTableFromSection(section, tableEntries, tableKeys);
    });

    // Build Groups array
    const groupsArray = [];
    groupedSections.forEach((sections, groupName) => {
        // Process sections for this group
        const groupData = processGroupSections(sections);
        
        // Create 3 Multiple entries for this group
        const multipleEntries = [];
        for (let i = 0; i < 3; i++) {
            multipleEntries.push({
                Standard: groupData.Standard,
                Tables: groupData.Tables,
                Groups: null
            });
        }

        groupsArray.push({
            Key: GROUP_KEY,
            Multiple: multipleEntries
        });
    });

    return {
        Standard: standardEntries,
        Tables: tableEntries,
        Groups: groupsArray
    };
};

const ReportPreviewOverlay = (props) => {
    
    const { visible, onClose, title } = props;

    const [errorStatus, updateErrorStatus] = useState({visible: false, message: ''});
    const [reportData, setReportData] = useState();
    
    // Fetch fresh configuration each time the overlay becomes visible
    useEffect(() => {
        if (!visible) return;
        const name = props.reportName || 'DefaultSpecimenReport';
        const criteria = { ReportName: name };
        Post('config/getreport', criteria, configDataRetrieved, errorWhenRetrievingData);
    }, [visible, props.reportName])

    const configDataRetrieved = (config) => {
        let formattedConfig = JSON.parse(config);
        const generatedTestData = generateTestDataFromConfig(formattedConfig);
        createReport(generatedTestData, formattedConfig);
    }

    const errorWhenRetrievingData = (response) => {
        updateErrorStatus({visible: true, message: response.data});
    }

    const errorCloseHandler = () => {
        updateErrorStatus({visible: false, message: ''});
    }

    const createReport = async (formattedData, reportConfig) => {
        FormatReportData(formattedData);

        let effectiveConfig = reportConfig === undefined ? configuration : reportConfig;
        effectiveConfig = StringUtils.parseJsonIfString(effectiveConfig);

        const report = await WriteReport(formattedData, effectiveConfig, "printpreview", props.language);
        setReportData({ ...report });
    }

    if (!visible) return null;

    return (
        <div className="rpreview-container">
            <div className="rpreview-header">
                <h3 className="rpreview-title">{title || 'Report Preview'}</h3>
                <IconButton
                    iconProps={{ iconName: 'Cancel' }}
                    ariaLabel="Close preview"
                    onClick={onClose}
                />
            </div>
            <div className="rpreview-body">
                {reportData ? (
                    <div className="rpreview-viewer">
                        <PdfViewer data={reportData} />
                    </div>
                ) : (
                    <div className="rpreview-loading">Preparing preview…</div>
                )}
                {/* <ErrorMessage visible={errorStatus.visible} dismissHandler={errorCloseHandler} error={errorStatus.message}></ErrorMessage> */}
            </div>
        </div>
    );
};

const mapStateToProps = state => {
    return {
        language: state.config.language
    };
}

export default connect(mapStateToProps)(ReportPreviewOverlay);


