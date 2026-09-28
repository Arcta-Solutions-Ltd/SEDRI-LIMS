import { findFormatByName } from './reportChangeSet';

const DOUBLE_COLUMN = { Name: 'DoubleColumnOne', Description: 'Two Column', Grids: [] };
const WITH_GRIDS = { Name: 'doublecolumnwithtwogrids', Description: 'Two Grids', Grids: [{}, {}] };

const formats = [DOUBLE_COLUMN, WITH_GRIDS];

describe('findFormatByName', () => {
    it('matches a name spelled exactly as the format stores it', () => {
        expect(findFormatByName(formats, 'DoubleColumnOne')).toBe(DOUBLE_COLUMN);
    });

    it('matches a name that only differs in case', () => {
        // A format saved through the designer used to be stored lower cased while the sections
        // referencing it kept their original casing, which left the section with no format at all.
        expect(findFormatByName(formats, 'doublecolumnone')).toBe(DOUBLE_COLUMN);
        expect(findFormatByName(formats, 'DOUBLECOLUMNONE')).toBe(DOUBLE_COLUMN);
        expect(findFormatByName(formats, 'DoubleColumnWithTwoGrids')).toBe(WITH_GRIDS);
    });

    it('ignores surrounding whitespace on either side of the comparison', () => {
        expect(findFormatByName(formats, '  DoubleColumnOne  ')).toBe(DOUBLE_COLUMN);
        expect(findFormatByName([{ Name: ' SingleColumnOne ' }], 'singlecolumnone')).not.toBeNull();
    });

    it('returns the stored record so callers can adopt its own name', () => {
        expect(findFormatByName(formats, 'doublecolumnone').Name).toBe('DoubleColumnOne');
    });

    it('returns null when no format matches', () => {
        expect(findFormatByName(formats, 'nosuchformat')).toBeNull();
    });

    it('returns null for a missing or empty name', () => {
        expect(findFormatByName(formats, undefined)).toBeNull();
        expect(findFormatByName(formats, null)).toBeNull();
        expect(findFormatByName(formats, '')).toBeNull();
        expect(findFormatByName(formats, '   ')).toBeNull();
    });

    it('returns null for a missing or empty format list', () => {
        expect(findFormatByName(undefined, 'DoubleColumnOne')).toBeNull();
        expect(findFormatByName(null, 'DoubleColumnOne')).toBeNull();
        expect(findFormatByName([], 'DoubleColumnOne')).toBeNull();
    });

    it('skips entries that have no name of their own', () => {
        expect(findFormatByName([null, {}, { Name: null }, DOUBLE_COLUMN], 'doublecolumnone')).toBe(DOUBLE_COLUMN);
    });
});
