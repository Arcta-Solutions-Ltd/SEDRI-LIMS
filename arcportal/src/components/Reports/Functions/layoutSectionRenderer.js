import { renderReportSection } from '../ReportWriter';
import { groupContentsIntoLayoutRows, renderLayoutRow } from './layoutRowRenderer';

/**
 * Renders ContentsConfig entries onto a ReportPdf instance for designer preview.
 * Skips header/footer, watermark handling, and organism multiple-group expansion.
 * @param {object} reportPdf - The report PDF state object.
 * @param {object[]} contents - Printable ContentsConfig entries to render.
 * @param {object} data - Synthetic preview data with Standard and Tables arrays.
 * @param {Array} language - Language catalogue for tag translation.
 * @param {number} [startingY=20] - Initial Y position in points.
 * @returns {Promise<void>}
 */
export const renderContentsPreview = async (reportPdf, contents, data, language, startingY = 20) => {
    if (!contents || !Array.isArray(contents) || contents.length === 0) {
        return;
    }

    if (typeof renderReportSection !== 'function') {
        throw new Error('renderReportSection is not available for layout section preview');
    }

    // Copied up front so the row renderer, which temporarily removes headings while it aligns a row, only
    // ever touches preview copies and never the translated contents the editor holds on to.
    const previewSections = contents.map((section) => ({
        ...section,
        Dynamic: false,
        Column1: section.Column1
            ? {
                ...section.Column1,
                Fields: [...(section.Column1.Fields || [])],
            }
            : section.Column1,
        Column2: section.Column2
            ? {
                ...section.Column2,
                Fields: [...(section.Column2.Fields || [])],
            }
            : section.Column2,
    }));

    const config = { Contents: previewSections };
    reportPdf.setCurrentLinePosition(startingY);

    for (const layoutRow of groupContentsIntoLayoutRows(previewSections)) {
        await renderLayoutRow({
            entries: layoutRow.entries,
            reportPdf,
            data,
            contents: previewSections,
            renderEntry: (entry, extraOptions) =>
                renderReportSection(
                    data,
                    entry,
                    0,
                    reportPdf,
                    config,
                    true,
                    language,
                    {
                        skipPageBreak: true,
                        designerPreview: true,
                        skipDynamicFieldRemoval: true,
                        ...extraOptions,
                    }
                ),
        });
    }
};
