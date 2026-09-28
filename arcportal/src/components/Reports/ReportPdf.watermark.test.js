import ReportPdf from './ReportPdf';
import autoTable from 'jspdf-autotable';

// A 1x1 transparent png - jsdom has no canvas implementation, so the watermark
// image has to be stubbed for the drawing commands to be emitted at all.
const TRANSPARENT_PNG =
    'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z8BQDwAEhQGAhKmMIQAAAABJRU5ErkJggg==';

describe('ReportPdf watermark', () => {
    beforeAll(() => {
        HTMLCanvasElement.prototype.getContext = () => ({
            clearRect: () => {},
            translate: () => {},
            rotate: () => {},
            fillText: () => {},
            measureText: () => ({ width: 100 }),
        });
        HTMLCanvasElement.prototype.toDataURL = () => TRANSPARENT_PNG;
    });

    test('watermark ops are present and appear before table drawing ops on the same page', () => {
        // Arrange: create a pdf with a watermark
        const pdf = new ReportPdf({ left: 20, right: 20, top: 20, bottom: 12 }, 'a4', 'NOT APPROVED');

        // Act: draw a table (this is the scenario that used to obscure the watermark)
        autoTable(pdf, {
            head: [['A', 'B']],
            body: [
                ['1', '2'],
                ['3', '4'],
            ],
            startY: 60,
        });

        // Re-insert watermarks (as ReportWriter does)
        pdf.redrawWatermarksOnAllPages();

        // Assert: page content exists
        const pageNum = 1;
        const page = pdf.internal.pages[pageNum];
        expect(Array.isArray(page)).toBe(true);

        const content = page.join('\n');
        // watermark command should exist
        expect(content).toContain(' Do');

        // Heuristic: watermark ops should be at/near the beginning of the page stream
        // autoTable typically emits rectangle / fill ops and text later.
        const firstDoIndex = content.indexOf(' Do');
        const firstRectIndex = content.indexOf(' re');
        if (firstRectIndex !== -1) {
            expect(firstDoIndex).toBeLessThan(firstRectIndex);
        }
    });
});

