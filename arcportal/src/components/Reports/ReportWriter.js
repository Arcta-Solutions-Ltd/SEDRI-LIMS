import ReportPdf from './ReportPdf';
import { SortLines } from './Functions/LineSorter';
import { WriteLinesAndSetPosition } from './Functions/LineWriter';
import { NewSectionHeight } from './Functions/FieldHeights';
import {
    MultipleReportTables,
    SingleReportTable,
} from './Functions/MultipleReportTables';
import './Phetsarath OT-normal';
import {
    PlaceImages,
    PlaceSectionColumnFieldImages,
} from './Functions/ReportImagePlacer';
import {
    DoesDoubleFieldContainData,
    DoubleFieldColumn,
    DoubleFieldColumnWithRowHeading,
    SingleFieldColumn,
    SingleFieldColumnWithRowHeading,
    SingleFieldColumnWithDynamicHeader,
    SingleFieldColumnWithSeparateHeading,
} from './Functions/FieldColumn';
import { SectionHeading } from './Functions/Headings';
import { WriteAbsoluteSection } from './Functions/AbsoluteSectionWriter';
import {
    groupContentsIntoLayoutRows,
    renderLayoutRow,
} from './Functions/layoutRowRenderer';

const writeReport = async (data, config, reportName, language, watermarkText) => {
    const reportPdf = new ReportPdf(
        { left: 20, right: 20, top: 20, bottom: 12 },
        'a4',
        watermarkText
    );

    // Write header and footer to first page
    // if (hasLinesOrImages(config.Header)) {
    //     await writeHeader(config.Header, reportPdf, data, language);
    // }

    // if (hasLinesOrImages(config.Footer)) {
    //     await writeFooter(config.Footer, reportPdf, data, language);
    // }
    

    // await writeContent(config, reportPdf, data, language);

    // // Write header and footer on the remaining pages after all contents have been written
    // // Store page count after content is written to ensure we capture all pages including dynamically added ones
    // const totalPages = reportPdf.getNumberOfPages();
    // for (let i = 2; i <= totalPages; i++) {
    //     reportPdf.setPage(i);
    //     if (hasLinesOrImages(config.Header)) {
    //         await writeHeader(config.Header, reportPdf, data, language);
    //     }

    //     if (hasLinesOrImages(config.Footer)) {
    //         await writeFooter(config.Footer, reportPdf, data, language);
    //     }
        
    // }

    // // Re-insert watermarks after all content has been written so they don't get obscured by tables.
    // // This uses ReportPdf's internal page-content manipulation to prepend watermark ops.
    // if (watermarkText) {
    //     reportPdf.redrawWatermarksOnAllPages();
    // }

    // if (typeof reportPdf.putTotalPages === 'function') {
    //     reportPdf.putTotalPages('{total_pages_count_string}');
    // }
    // return reportPdf;

    await writeIndividualReport(reportPdf, data, config, language, watermarkText)

    return reportPdf;
};

const writeIndividualReport = async (reportPdf, data, config, language, watermarkText, skipPutTotalPages = false, reportStartPage = 1) => {
    if (!config) {
        throw new Error('Report configuration is not available.');
    }

    if (hasLinesOrImages(config?.Header)) {
        await writeHeader(config.Header, reportPdf, data, language);
    }

    if (hasLinesOrImages(config?.Footer)) {
        await writeFooter(config.Footer, reportPdf, data, language);
    }
    

    await writeContent(config, reportPdf, data, language);

    // Write header and footer on the remaining pages after all contents have been written
    // Store page count after content is written to ensure we capture all pages including dynamically added ones
    // In batch mode, only iterate over pages belonging to this report (reportStartPage+1 to totalPages)
    // to avoid overwriting headers/footers on pages belonging to previous reports
    const totalPages = reportPdf.getNumberOfPages();
    const firstRemainingPage = reportStartPage + 1;
    for (let i = firstRemainingPage; i <= totalPages; i++) {
        reportPdf.setPage(i);
        if (hasLinesOrImages(config?.Header)) {
            await writeHeader(config.Header, reportPdf, data, language);
        }

        if (hasLinesOrImages(config?.Footer)) {
            await writeFooter(config.Footer, reportPdf, data, language);
        }
        
    }

    // Re-insert watermarks after all content has been written so they don't get obscured by tables.
    // This uses ReportPdf's internal page-content manipulation to prepend watermark ops.
    if (watermarkText) {
        reportPdf.redrawWatermarksOnAllPages();
    }

    if (!skipPutTotalPages && typeof reportPdf.putTotalPages === 'function') {
        reportPdf.putTotalPages('{total_pages_count_string}');
    }
}

const writeAndDownloadReport = async (data, config, reportName, language, watermarkText) => {
    const reportPdf = await writeReport(data, config, reportName, language, watermarkText);
    reportPdf.save(reportName);
};

const writeMultipleReports = async (data, config, language) => {
    let reportPdf = new ReportPdf(
        { left: 20, right: 20, top: 20, bottom: 12 },
        'a4'
    );

    for (let i = 0; i < data.length; i++) {
        if (i > 0) {
            reportPdf.addPage();
        }
        const startPage = reportPdf.getNumberOfPages();
        await writeIndividualReport(reportPdf, data[i], config, language, "", true, startPage);
        const endPage = reportPdf.getNumberOfPages();
        const reportPageCount = endPage - startPage + 1;
        reportPdf.putTotalPagesForPageRange(startPage, endPage, reportPageCount);
    }

    reportPdf.save('report.pdf'); // TODO: make this configurable
};

const writeHeader = async (headerConfig, reportPdf, data, language) => {
    if (!headerConfig) {
        return;
    }
    // Place images first (now async due to orientation correction)
    await PlaceImages(headerConfig.Images ?? [], reportPdf);
    
    const lines = SortLines(headerConfig.Lines);
    reportPdf.setCurrentLine(getFirstLineNumber(lines));
    reportPdf.setCurrentLinePosition(
        reportPdf.getMargins(reportPdf.getCurrentPage).top
    );
    WriteLinesAndSetPosition(
        lines,
        reportPdf,
        data,
        language,
        false,
        headerConfig.LineSpacing ?? 4
    );
    
    // Calculate max bottom position from images
    let maxBottom = reportPdf.getCurrentLinePosition();
    if (headerConfig.Images && headerConfig.Images.length > 0) {
        const maxImageBottom = Math.max(
            ...headerConfig.Images.map(img => (img.Y || 0) + (img.Height || 0))
        );
        maxBottom = Math.max(maxBottom, maxImageBottom);
    }
    
    // Draw horizontal rule at max bottom + spacing
    reportPdf.setCurrentLinePosition(maxBottom + reportPdf.spacing.md);
    reportPdf.horizontalRule();
    reportPdf.setHeaderHeight(reportPdf.getCurrentLinePosition());
};

const writeFooter = async (footerConfig, reportPdf, data, language) => {
    if (!footerConfig) {
        return;
    }
    await PlaceImages(footerConfig.Images, reportPdf);
    const lines = SortLines(footerConfig.Lines, true);
    reportPdf.setCurrentLine(getLastLineNumber(lines));
    reportPdf.setCurrentLinePosition(
        reportPdf.getPageHeight() -
            reportPdf.getMargins(reportPdf.getCurrentPage).bottom
    );
    WriteLinesAndSetPosition(
        lines,
        reportPdf,
        data,
        language,
        true,
        footerConfig.LineSpacing ?? 4
    );
    reportPdf.horizontalRule();
    reportPdf.setFooterHeight(
        reportPdf.getPageHeight() - reportPdf.getCurrentLinePosition()
    );
};

const getFirstLineNumber = (lines) =>
    Math.min(...lines.map((item) => item.Line));
const getLastLineNumber = (lines) =>
    Math.max(...lines.map((item) => item.Line));

/**
 * Moves to a new page when a layout section row will not fit in the space left above the footer.
 * @param {object} reportPdf - The report PDF state object.
 * @param {number} rowHeight - Estimated height of the row in points.
 * @param {number} footerHeight - Footer height in points.
 */
const breakPageIfRowOverflows = (reportPdf, rowHeight, footerHeight) => {
    const footerTopPadding = reportPdf.spacing.md;
    const rowEndYPx = rowHeight + reportPdf.getCurrentLinePosition();
    const footerYStartPx =
        reportPdf.getPageHeight() - (footerHeight + footerTopPadding);

    if (rowEndYPx > footerYStartPx) {
        reportPdf.addPage();
        reportPdf.setCurrentLinePosition(reportPdf.getHeaderHeight() + 10);
    }
};

/**
 * Renders the areas of one layout section row, or a single entry when the row holds only one area.
 * @param {Array<object>} entries - The entries sharing the row.
 * @param {object} entryData - Report data payload for the current specimen or organism group.
 * @param {object} lines - Full printable report definition.
 * @param {object} reportPdf - The report PDF state object.
 * @param {number} footerHeight - Footer height in points.
 * @param {string} language - Active report language code.
 * @returns {Promise<void>}
 */
const writeLayoutRow = async (entries, entryData, lines, reportPdf, footerHeight, language) =>
    renderLayoutRow({
        entries,
        reportPdf,
        data: entryData,
        contents: lines.Contents,
        renderEntry: (entry, extraOptions) =>
            generateSection(
                entryData,
                entry,
                footerHeight,
                reportPdf,
                lines,
                true,
                language,
                extraOptions
            ),
        ensureRoomForRow: (rowHeight) =>
            breakPageIfRowOverflows(reportPdf, rowHeight, footerHeight),
    });

const writeContent = async (lines, reportPdf, data, language) => {
    if (!lines || !lines.Contents || !Array.isArray(lines.Contents)) {
        return;
    }
    const headerHeight = reportPdf.getHeaderHeight();
    const footerHeight = reportPdf.getFooterHeight();
    let currentGroup = '';
    reportPdf.setCurrentLinePosition(headerHeight + 10);

    // Entries are walked a row at a time so the areas a section placed side by side render together. A
    // section with no arrangement yields one group per entry, which is the original entry-by-entry loop.
    for (const layoutRow of groupContentsIntoLayoutRows(lines.Contents)) {
        const section = layoutRow.entries[0];

        if (currentGroup !== section.Group && section.Multiple) {
            currentGroup = section.Group;
            if (currentGroup && data.Groups) {
                const sectionData = data.Groups.find(
                    (f) =>
                        f.Key.toLowerCase() ===
                        section.MultipleName.toLowerCase()
                );
                const groupSections = lines.Contents.filter(
                    (f) =>
                        f.Group &&
                        f.Group.toLowerCase() === currentGroup.toLowerCase()
                );
                if (!sectionData || !Array.isArray(sectionData.Multiple)) {
                    continue;
                }
                for (const groupData of sectionData.Multiple) {
                    for (const groupRow of groupContentsIntoLayoutRows(groupSections)) {
                        await writeLayoutRow(
                            groupRow.entries,
                            groupData,
                            lines,
                            reportPdf,
                            footerHeight,
                            language
                        );
                    }
                    reportPdf.incrementCurrentLinePositionBy(15);
                }
            }
        } else {
            await writeLayoutRow(
                layoutRow.entries,
                data,
                lines,
                reportPdf,
                footerHeight,
                language
            );
        }
    }
};

/**
 * Defers a mixed section heading for linked grid ContentsConfigs that follow.
 * Always overwrites any stale saved heading left by a prior section.
 * @param {object} section - The main layout ContentsConfig being rendered.
 * @param {object} reportPdf - The report PDF state object.
 */
const deferSectionHeadingForLinkedGrids = (section, reportPdf) => {
    if (section.LinkedSections <= 0) {
        return;
    }

    const deferredHeading = section.Heading?.[0]?.Text?.trim()
        ? section.Heading
        : undefined;

    reportPdf.setLinkedSectionsRemaining(section.LinkedSections);
    if (deferredHeading) {
        reportPdf.setSavedSectionHeading(deferredHeading);
    }
};

/**
 * Completes linked-grid processing for a grid table ContentsConfig.
 * Decrements the deferred-heading countdown and clears unconsumed saved headings
 * after the last linked grid in the group is processed.
 * @param {object} reportPdf - The report PDF state object.
 */
const finalizeLinkedGridHeading = (reportPdf) => {
    const linkedRemaining = reportPdf.getLinkedSectionsRemaining();
    if (linkedRemaining > 0) {
        const remaining = reportPdf.decrementLinkedSectionsRemaining();
        if (remaining === 0 && reportPdf.getSavedSectionHeading()) {
            reportPdf.setSavedSectionHeading(undefined);
        }
        return;
    }

    reportPdf.setSavedSectionHeading(undefined);
};

/**
 * Renders one printable ContentsConfig section and advances the PDF cursor.
 * @param {object} data - Report data payload for the current specimen or organism group.
 * @param {object} section - ContentsConfig entry to render.
 * @param {number} footerHeight - Footer height in points for page-break calculations.
 * @param {object} reportPdf - The report PDF state object.
 * @param {object} config - Full printable report definition including all Contents entries.
 * @param {boolean} showHeading - Whether inline section headings should be rendered for field data.
 * @param {string} language - Active report language code.
 */
const generateSection = async (
    data,
    section,
    footerHeight,
    reportPdf,
    config,
    showHeading = true,
    language,
    options = {}
) => {
    // Field and grid sections render their data even when ContentsConfig.Heading is absent.
    const sectionName = section.Name.toLowerCase();
    const sectionType = section.Type.toLowerCase();
    const contents = config?.Contents || config?.contents || [];

    //TODO: We need to check if there are images and if there are check if they span over the page then factor that into the newsectioncalc

    const sectionHeight = NewSectionHeight(section, data, contents);
    reportPdf.setCurrentLine(0);

    if (!options.skipPageBreak) {
        const footerTopPadding = reportPdf.spacing.md;
        const newSectionEndYPx =
            sectionHeight.singleHeight + reportPdf.getCurrentLinePosition();
        const footerYStartPx =
            reportPdf.getPageHeight() - (footerHeight + footerTopPadding);

        const isNewSectionOffPage = newSectionEndYPx > footerYStartPx;
        if (isNewSectionOffPage && sectionType !== 'table') {
            reportPdf.addPage();
            reportPdf.setCurrentLinePosition(reportPdf.getHeaderHeight() + 10);
        }
    }

    deferSectionHeadingForLinkedGrids(section, reportPdf);

    if (isImagesInSection(section)) {
        const newImages = section.Images.map((image) => ({
            ...image,
            X: image.X ? image.X : reportPdf.getMargins().left,
            Y: image.Y
                ? image.Y + reportPdf.getCurrentLinePosition()
                : reportPdf.getCurrentLinePosition(),
        }));
        await PlaceImages(newImages, reportPdf);
    }

    switch (sectionType) {
        case 'lines':
            WriteLinesAndSetPosition(
                section.Lines,
                reportPdf,
                data,
                language,
                false,
                section.LineSpacing ?? 2
            );
            break;
        case 'reportimage':
            PlaceSectionColumnFieldImages(section, reportPdf);
            break;
        case 'singlefieldcolumn':
            SingleFieldColumn(section, data, reportPdf, options);
            break;
        case 'singlefieldcolumnwithseparateheading':
            SingleFieldColumnWithSeparateHeading(section, data, reportPdf, options);
            break;
        case 'singlefieldcolumnwithrowheading':
            SingleFieldColumnWithRowHeading(section, data, reportPdf, options);
            break;
        case 'doublefieldcolumn': {
            const hasDoubleFieldData = options.designerPreview
                || DoesDoubleFieldContainData(section, data);
            if (hasDoubleFieldData) {
                if (showHeading && section.Heading?.[0]?.Text?.trim()) {
                    SectionHeading(section.Heading, reportPdf);
                }
                DoubleFieldColumn(section, data, reportPdf, options);
                reportPdf.setSavedSectionHeading(undefined);
            }
            break;
        }
        case 'doublefieldcolumnwithrowheading':
            DoubleFieldColumnWithRowHeading(section, data, reportPdf, options);
            break;
        case 'dynamicsinglecolumnone':
            SingleFieldColumnWithDynamicHeader(section, data, reportPdf, options);
            break;
        case 'absolute':
            // Render absolute section with lines and images
            if (section.Lines && section.Lines.length > 0) {
                WriteLinesAndSetPosition(
                    section.Lines,
                    reportPdf,
                    data,
                    language,
                    false,
                    4  // LINE_SPACING constant from LineWriter.js
                );
            }
            // Images are already handled above at line 232-240
            break;
        case 'table':
            if (data.Tables?.length > 0) {
                const useSingleReportTable =
                    section.Theme !== 'grid' &&
                    (
                        sectionName === 'organismlisttable' ||
                        sectionName === 'alerts' ||
                        sectionName === 'commentstable' ||
                        sectionName === 'specimencommentstable'
                    );

                if (useSingleReportTable) {
                    SingleReportTable(section, reportPdf, data);
                } else {
                    MultipleReportTables(section, reportPdf, data);
                    if (section.Theme === 'grid') {
                        finalizeLinkedGridHeading(reportPdf);
                    } else if (section.LinkedSections < 1) {
                        reportPdf.setSavedSectionHeading(undefined);
                    }
                }
            } else if (
                sectionType === 'table' &&
                section.Theme === 'grid' &&
                reportPdf.getLinkedSectionsRemaining() > 0
            ) {
                finalizeLinkedGridHeading(reportPdf);
            }
            break;
        default:
            break;
    }

    if (section.Separator) {
        reportPdf.horizontalRule();
        reportPdf.incrementCurrentLinePositionBy(10);
    }
};

const isImagesInSection = (section) =>
    Array.isArray(section.Images) && section.Images.length > 0;

const hasLinesOrImages = (headerFooterConfig) => {
    return (headerFooterConfig?.Images?.length > 0) || (headerFooterConfig?.Lines?.length > 0);
};

export {
    writeReport as WriteReport,
    writeAndDownloadReport as WriteAndDownloadReport,
    writeMultipleReports as WriteMultipleReports,
    generateSection as renderReportSection,
};
