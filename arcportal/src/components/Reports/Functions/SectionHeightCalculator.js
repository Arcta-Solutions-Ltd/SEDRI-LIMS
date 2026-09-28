import {
    ABSOLUTE_DEFAULT_FONT_SIZE,
    ABSOLUTE_HORIZONTAL_RULE_SPACING,
    calculateAbsoluteSectionHeight,
} from './AbsoluteSectionLineLayout';

/**
 * Calculates the height a header or footer section will occupy.
 * Mirrors the logic in writeHeader/writeFooter from ReportWriter.js.
 * @param {Object} sectionConfig - Section configuration with Lines and Images.
 * @param {number} [startingMargin=20] - Starting position (e.g., top margin).
 * @returns {number} Total height in points.
 */
export const calculateHeaderFooterHeight = (sectionConfig, startingMargin = 20) => {
    if (!sectionConfig) {
        return 0;
    }

    return calculateAbsoluteSectionHeight(
        sectionConfig.Lines || [],
        sectionConfig.Images || [],
        {
            startingY: startingMargin,
            lineSpacing: sectionConfig.LineSpacing ?? 4,
            marginBottom: 7.5,
            horizontalRuleSpacing: ABSOLUTE_HORIZONTAL_RULE_SPACING,
            bottomPadding: 0,
            defaultFontSize: ABSOLUTE_DEFAULT_FONT_SIZE,
        }
    );
};

/**
 * Calculates available height for content sections based on page and header/footer.
 * @param {number} [pageHeight=842] - Page height in points (default A4).
 * @param {number} [topMargin=20] - Top margin in points.
 * @param {number} [bottomMargin=12] - Bottom margin in points.
 * @param {number} [headerHeight=0] - Header height in points.
 * @param {number} [footerHeight=0] - Footer height in points.
 * @param {number} [contentPadding=10] - Padding around content.
 * @returns {number} Available height for content in points.
 */
export const calculateAvailableContentHeight = (
    pageHeight = 842,
    topMargin = 20,
    bottomMargin = 12,
    headerHeight = 0,
    footerHeight = 0,
    contentPadding = 10
) => {
    const available = pageHeight - topMargin - bottomMargin -
        headerHeight - footerHeight - (contentPadding * 2);
    return Math.max(100, available);
};
