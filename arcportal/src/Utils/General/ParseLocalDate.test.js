import parseLocalDate from './ParseLocalDate';
import ParseDate from '../../components/Forms/ArcDate/ParseDate';

describe('parseLocalDate', () => {
    it('parses yyyy-MM-dd as a calendar date regardless of environment TZ', () => {
        const d = parseLocalDate('2026-09-01');
        expect(d.getFullYear()).toBe(2026);
        expect(d.getMonth()).toBe(8);
        expect(d.getDate()).toBe(1);
    });

    it('returns the same Date instance when passed a Date', () => {
        const original = new Date(2026, 8, 1);
        expect(parseLocalDate(original)).toBe(original);
    });

    it('round-trips ISO strings without shifting the calendar day', () => {
        const roundTripped = parseLocalDate('2026-09-01');
        expect(roundTripped.getDate()).toBe(1);
        expect(roundTripped.getMonth()).toBe(8);
        expect(roundTripped.getFullYear()).toBe(2026);
    });

    it('returns undefined for non-date input', () => {
        expect(parseLocalDate(undefined)).toBeUndefined();
        expect(parseLocalDate(null)).toBeUndefined();
        expect(parseLocalDate('')).toBeUndefined();
    });
});

describe('ParseDate', () => {
    const assertSameCalendarDate = (a, b) => {
        expect(a.getFullYear()).toBe(b.getFullYear());
        expect(a.getMonth()).toBe(b.getMonth());
        expect(a.getDate()).toBe(b.getDate());
    };

    it('parses DD/MM/YYYY and yyyy-MM-dd to the same calendar date', () => {
        const fromSlash = ParseDate('01/09/2026');
        const fromDash = ParseDate('2026-09-01');
        assertSameCalendarDate(fromSlash, fromDash);
        expect(fromSlash.getDate()).toBe(1);
        expect(fromSlash.getMonth()).toBe(8);
        expect(fromSlash.getFullYear()).toBe(2026);
    });
});
