import { formatAgeDisplayWithTags, translateEmbeddedLanguageTags } from './FormatAgeDisplay';

const englishAgeUnits = [
    { Key: '@GenYeaA@', Value: 'years' },
    { Key: '@GenYeaC@', Value: 'year' },
    { Key: '@GenMonA@', Value: 'months' },
    { Key: '@GenMonC@', Value: 'month' },
    { Key: '@GenDayA@', Value: 'days' },
    { Key: '@GenDayB@', Value: 'day' },
    { Key: '@GenHouA@', Value: 'hours' },
    { Key: '@GenHouB@', Value: 'hour' },
];

describe('formatAgeDisplayWithTags', () => {
    it('uses singular year tag when years is 1', () => {
        expect(formatAgeDisplayWithTags({ Years: 1, Months: 0, Days: 0, Hours: 0 })).toBe('1 @GenYeaC@');
    });

    it('uses plural year tag when years is not 1', () => {
        expect(formatAgeDisplayWithTags({ Years: 2, Months: 0, Days: 0, Hours: 0 })).toBe('2 @GenYeaA@');
    });

    it('uses singular month tag when months is 1', () => {
        expect(formatAgeDisplayWithTags({ Years: 0, Months: 1, Days: 0, Hours: 0 })).toBe('1 @GenMonC@');
    });

    it('uses plural month tag when months is not 1', () => {
        expect(formatAgeDisplayWithTags({ Years: 0, Months: 3, Days: 0, Hours: 0 })).toBe('3 @GenMonA@');
    });

    it('uses singular tags for each unit when years and months are both 1', () => {
        expect(formatAgeDisplayWithTags({ Years: 1, Months: 1, Days: 0, Hours: 0 })).toBe('1 @GenYeaC@ 1 @GenMonC@');
    });

    it('uses singular day tag when days is 1', () => {
        expect(formatAgeDisplayWithTags({ Years: 0, Months: 0, Days: 1, Hours: 0 })).toBe('1 @GenDayB@');
    });

    it('uses plural day tag when days is not 1', () => {
        expect(formatAgeDisplayWithTags({ Years: 0, Months: 0, Days: 5, Hours: 0 })).toBe('5 @GenDayA@');
    });

    it('uses singular hour tag when hours is 1', () => {
        expect(formatAgeDisplayWithTags({ Years: 0, Months: 0, Days: 0, Hours: 1 })).toBe('1 @GenHouB@');
    });

    it('uses plural hour tag when hours is not 1', () => {
        expect(formatAgeDisplayWithTags({ Years: 0, Months: 0, Days: 0, Hours: 4 })).toBe('4 @GenHouA@');
    });
});

describe('translateEmbeddedLanguageTags for age units', () => {
    it('translates singular age labels to lowercase English', () => {
        expect(translateEmbeddedLanguageTags('1 @GenMonC@', englishAgeUnits)).toBe('1 month');
        expect(translateEmbeddedLanguageTags('1 @GenHouB@', englishAgeUnits)).toBe('1 hour');
    });

    it('translates plural age labels to lowercase English', () => {
        expect(translateEmbeddedLanguageTags('2 @GenMonA@', englishAgeUnits)).toBe('2 months');
        expect(translateEmbeddedLanguageTags('4 @GenHouA@', englishAgeUnits)).toBe('4 hours');
    });
});
