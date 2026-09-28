import { SectionHeading } from './Headings';
import { NewSectionHeight } from './FieldHeights';

/**
 * Section types whose renderers pull the cursor up by 4pt before drawing their own row heading.
 * Their leading offset has to be known here so every area on a row can be aligned to one top edge.
 */
const ROW_HEADING_SECTION_TYPES = ['doublefieldcolumnwithrowheading', 'singlefieldcolumnwithrowheading'];

/** Points the row heading renderers pull the cursor up by before drawing. */
const ROW_HEADING_LEAD = -4;

/**
 * Groups printable entries so that the areas sharing a layout section row are rendered together.
 * @param {Array<object>} contents - Printable ContentsConfig entries in document order.
 * @returns {Array<{rowKey: string|null, entries: Array<object>}>} One group per rendered row. Entries with
 * no LayoutRowKey each become a group of one, which is how every section behaved before area rows existed.
 */
export const groupContentsIntoLayoutRows = (contents) => {
    const groups = [];

    for (const entry of contents || []) {
        const rowKey = entry?.LayoutRowKey || null;
        const previousGroup = groups[groups.length - 1];

        if (rowKey && previousGroup?.rowKey === rowKey) {
            previousGroup.entries.push(entry);
            continue;
        }

        groups.push({ rowKey, entries: [entry] });
    }

    return groups;
};

/**
 * Reads how far past the cursor an entry's own renderer starts drawing.
 * @param {object} entry - A printable ContentsConfig entry.
 * @param {object} reportPdf - The report PDF state object, read for its spacing scale.
 * @returns {number} The entry's leading offset in points, which may be negative.
 * @remarks
 * Grid tables add a gap before their first row, 10pt when they have a header row and 5pt when they do not,
 * while field blocks start exactly on the cursor and the row heading formats start 4pt above it. Left
 * uncorrected, a field block and a grid placed on one row would have their first rows 10pt apart.
 */
export const getLayoutRowLeadingOffset = (entry, reportPdf) => {
    const sectionType = String(entry?.Type || '').toLowerCase();

    if (sectionType === 'table') {
        return entry?.Head ? reportPdf.spacing.md : reportPdf.spacing.sm;
    }

    return ROW_HEADING_SECTION_TYPES.includes(sectionType) ? ROW_HEADING_LEAD : 0;
};

/**
 * Renders the areas that share one layout section row, side by side and aligned on a shared top edge.
 * @param {object} params - Row rendering parameters.
 * @param {Array<object>} params.entries - The entries sharing the row, ordered left to right.
 * @param {object} params.reportPdf - The report PDF state object.
 * @param {object} params.data - Report data payload for the current specimen or organism group.
 * @param {Array<object>} params.contents - Every printable entry, needed for height estimation.
 * @param {Function} params.renderEntry - Renders one entry: (entry, extraOptions) => Promise<void>.
 * @param {Function} [params.ensureRoomForRow] - Given the row's height in points, moves to a new page when
 * the row will not fit. Omitted for the designer preview, which never paginates.
 * @returns {Promise<void>}
 * @remarks
 * The page and cursor are reset to the row's top before each area, and set once afterwards to the bottom of
 * the tallest area, which is how a double field column has always aligned its two columns. Any section heading
 * is drawn once above the whole row rather than by an individual area, because an area drawing its own
 * heading would push its content down relative to its neighbours.
 */
export const renderLayoutRow = async ({ entries, reportPdf, data, contents, renderEntry, ensureRoomForRow }) => {
    if (!entries || entries.length === 0) {
        return;
    }

    if (entries.length === 1) {
        await renderEntry(entries[0], {});
        return;
    }

    const suppressedHeadings = entries
        .filter((entry) => entry.Heading)
        .map((entry) => ({ entry, heading: entry.Heading }));
    const rowHeadingLines = suppressedHeadings
        .find(({ heading }) => heading?.[0]?.Text?.trim())?.heading;

    // Headings are removed for the duration of the row so no area draws one, and restored afterwards
    // because organism sections render the same entries once per organism.
    suppressedHeadings.forEach(({ entry }) => {
        entry.Heading = undefined;
    });
    reportPdf.setSavedSectionHeading(undefined);

    try {
        const areaHeights = entries.map((entry) => NewSectionHeight(entry, data, contents).singleHeight);

        if (areaHeights.every((height) => height <= 0)) {
            return;
        }

        if (ensureRoomForRow) {
            ensureRoomForRow(Math.max(...areaHeights));
        }

        if (rowHeadingLines) {
            reportPdf.setCurrentLine(0);
            SectionHeading(rowHeadingLines, reportPdf);
        }

        const leadingOffsets = entries.map((entry) => getLayoutRowLeadingOffset(entry, reportPdf));
        const rowLeading = Math.max(...leadingOffsets);
        const rowPage = reportPdf.getCurrentPageNumber();
        const rowTop = reportPdf.getCurrentLinePosition();
        let bottomPage = rowPage;
        let bottomY = rowTop;

        for (let index = 0; index < entries.length; index++) {
            // The page is restored as well as the cursor. An area tall enough to spill onto a new page leaves
            // the document on that page, and without this the next area would draw there instead of alongside.
            reportPdf.setPage(rowPage);
            reportPdf.setCurrentLinePosition(rowTop + rowLeading - leadingOffsets[index]);
            await renderEntry(entries[index], { skipPageBreak: true });

            const endPage = reportPdf.getCurrentPageNumber();
            const endY = reportPdf.getCurrentLinePosition();

            if (endPage > bottomPage || (endPage === bottomPage && endY > bottomY)) {
                bottomPage = endPage;
                bottomY = endY;
            }
        }

        reportPdf.setPage(bottomPage);
        reportPdf.setCurrentLinePosition(bottomY);
    } finally {
        suppressedHeadings.forEach(({ entry, heading }) => {
            entry.Heading = heading;
        });
    }
};
