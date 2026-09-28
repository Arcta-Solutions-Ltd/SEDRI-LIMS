import ReportPdf from './ReportPdf';
import {
    SplitScriptSegments,
    ToPdfSafeText,
} from './Functions/PdfTextEncoding';

// jsPDF signals "I gave up and wrote this string as UCS-2 BE" by padding every
// character with a NUL byte, which viewers draw as a blank.
const NUL = '\u0000';

const newPdf = () =>
    new ReportPdf({ left: 20, right: 20, top: 20, bottom: 12 }, 'a4');

const pageContent = (pdf) => pdf.internal.pages[1].join('\n');

const drawnStrings = (pdf) =>
    Array.from(pageContent(pdf).matchAll(/\((.*)\) Tj/g)).map(
        (match) => match[1]
    );

describe('PdfTextEncoding', () => {
    test('leaves text the standard fonts can encode alone', () => {
        expect(ToPdfSafeText('Count > 10 ml/cfm')).toBe('Count > 10 ml/cfm');
    });

    test('transliterates characters outside Latin-1', () => {
        expect(ToPdfSafeText('β-lactamase – ‘high’ level')).toBe(
            "beta-lactamase - 'high' level"
        );
        expect(ToPdfSafeText('≥ 10⁵ CFU/ml')).toBe('>= 105 CFU/ml');
    });

    test('splits superscript and subscript runs out of the text', () => {
        expect(SplitScriptSegments('Count > 10⁵ ml/cfm')).toEqual([
            { text: 'Count > 10', kind: 'normal' },
            { text: '5', kind: 'superscript' },
            { text: ' ml/cfm', kind: 'normal' },
        ]);

        expect(SplitScriptSegments('H₂O')).toEqual([
            { text: 'H', kind: 'normal' },
            { text: '2', kind: 'subscript' },
            { text: 'O', kind: 'normal' },
        ]);
    });
});

describe('ReportPdf.text', () => {
    test('does not UCS-2 encode a title containing a superscript', () => {
        const pdf = newPdf();

        pdf.text(20, 40, 'Count > 10⁵ ml/cfm');

        expect(pageContent(pdf)).not.toContain(NUL);
        expect(drawnStrings(pdf)).toEqual(['Count > 10', '5', ' ml/cfm']);
    });

    test('draws the superscript smaller and above the baseline', () => {
        const pdf = newPdf();
        pdf.setFontSize(12);

        pdf.text(20, 40, '10⁵');

        const positioned = Array.from(
            pageContent(pdf).matchAll(/([\d.]+) ([\d.]+) Td\n\((.*)\) Tj/g)
        ).map((match) => ({
            x: Number(match[1]),
            y: Number(match[2]),
            text: match[3],
        }));
        const fontSizes = Array.from(
            pageContent(pdf).matchAll(/\/F\d+ ([\d.]+) Tf/g)
        ).map((match) => Number(match[1]));

        expect(positioned.map((entry) => entry.text)).toEqual(['10', '5']);
        // y is measured from the bottom of the page, so the raised superscript
        // has the larger value, and it starts to the right of the '10'.
        expect(positioned[1].y).toBeGreaterThan(positioned[0].y);
        expect(positioned[1].x).toBeGreaterThan(positioned[0].x);
        expect(Math.min(...fontSizes)).toBeLessThan(12);
        // The font size is restored for whatever is written next.
        expect(pdf.getFontSize()).toBe(12);
    });

    test('keeps writing plain text through jsPDF unchanged', () => {
        const pdf = newPdf();

        pdf.text(20, 40, 'Specimen: Blood culture');

        expect(drawnStrings(pdf)).toEqual(['Specimen: Blood culture']);
    });

    test('transliterates text it cannot draw as segments', () => {
        const pdf = newPdf();

        pdf.text('β-lactamase', 20, 40, { align: 'center' });

        expect(pageContent(pdf)).not.toContain(NUL);
        expect(drawnStrings(pdf)).toEqual(['beta-lactamase']);
    });

    test('handles an array of lines', () => {
        const pdf = newPdf();

        pdf.text(['10⁵ ml/cfm', 'plain'], 20, 40);

        expect(pageContent(pdf)).not.toContain(NUL);
    });
});
