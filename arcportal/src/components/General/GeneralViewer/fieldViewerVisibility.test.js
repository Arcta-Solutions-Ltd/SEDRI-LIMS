import {
    fieldIsVisibleInViewer,
    hasValidUploadValue,
    sectionHasVisibleContent,
    subsectionHasVisibleContent,
} from './fieldViewerVisibility';

describe('fieldViewerVisibility', () => {
    describe('hasValidUploadValue', () => {
        it('returns false for empty', () => {
            expect(hasValidUploadValue(undefined)).toBe(false);
            expect(hasValidUploadValue(null)).toBe(false);
            expect(hasValidUploadValue('')).toBe(false);
        });
        it('returns true for positive id', () => {
            expect(hasValidUploadValue(42)).toBe(true);
            expect(hasValidUploadValue('7')).toBe(true);
        });
        it('returns true for comma-separated ids', () => {
            expect(hasValidUploadValue('1, 2, 3')).toBe(true);
        });
        it('returns false for zero or invalid tokens', () => {
            expect(hasValidUploadValue('0')).toBe(false);
            expect(hasValidUploadValue(',')).toBe(false);
        });
    });

    describe('fieldIsVisibleInViewer', () => {
        it('hides empty upload', () => {
            expect(fieldIsVisibleInViewer({ Type: 'upload', Value: '' })).toBe(false);
        });
        it('shows upload with ids', () => {
            expect(fieldIsVisibleInViewer({ Type: 'upload', Value: '99' })).toBe(true);
        });
        it('hides non-upload with empty value', () => {
            expect(fieldIsVisibleInViewer({ Type: 'text', Value: '' })).toBe(false);
        });
        it('shows non-upload with value', () => {
            expect(fieldIsVisibleInViewer({ Type: 'text', Value: 'x' })).toBe(true);
        });
    });

    describe('sectionHasVisibleContent', () => {
        it('false when only empty upload field', () => {
            expect(
                sectionHasVisibleContent({
                    Id: 'attachments',
                    Fields: [{ Id: 'f', Type: 'upload', Value: '' }],
                })
            ).toBe(false);
        });
        it('true when upload has id', () => {
            expect(
                sectionHasVisibleContent({
                    Id: 'attachments',
                    Fields: [{ Id: 'f', Type: 'upload', Value: '1' }],
                })
            ).toBe(true);
        });
        it('true when subsection has visible field', () => {
            expect(
                sectionHasVisibleContent({
                    Id: 's',
                    Fields: [],
                    SubSections: [{ Id: 'sub', Fields: [{ Type: 'text', Value: 'a' }] }],
                })
            ).toBe(true);
        });
    });

    describe('subsectionHasVisibleContent', () => {
        it('false when no visible fields', () => {
            expect(subsectionHasVisibleContent({ Fields: [{ Type: 'upload', Value: '' }] })).toBe(false);
        });
    });
});
