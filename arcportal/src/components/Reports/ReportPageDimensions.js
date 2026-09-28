/**

 * A4 report page dimensions in points.

 * Layout coordinates (Left, page width) map 1 numeric unit = 1pt = 1 CSS px for position.

 * Font sizes in the absolute section preview must use CSS pt units to match jsPDF.

 * Matches ReportPdf / jsPDF defaults and ReportWriter margin configuration.

 */

export const ReportPageDimensions = {

    PAGE_WIDTH: 595,

    PAGE_HEIGHT: 842,

    MARGIN_LEFT: 20,

    MARGIN_RIGHT: 20,

    MARGIN_TOP: 20,

    MARGIN_BOTTOM: 12,

    CONTENT_WIDTH: 555,



    /** jsPDF font used by ReportPdf for all report text. */

    PDF_FONT_FAMILY: 'Times',



    /** CSS font stack matching jsPDF Times rendering in the browser preview. */

    PREVIEW_FONT_FAMILY: '"Times New Roman", Times, serif',



    /** Default font size (pt) when a line element has no FontSize — matches LineWriter.DEFAULT_FONT_SIZE. */

    PDF_DEFAULT_FONT_SIZE: 11,

};

