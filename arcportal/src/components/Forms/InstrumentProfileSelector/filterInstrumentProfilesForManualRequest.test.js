import { filterInstrumentProfilesForManualRequest, toComboOptions } from './filterInstrumentProfilesForManualRequest';

describe('filterInstrumentProfilesForManualRequest', () => {
    const baseProfile = {
        LaboratoryId: 'lab1',
        SpecimenTypeId: '',
        CultureTypeId: '',
        DirectTestId: '',
        CultureTestId: '',
        OrganismGroupId: '',
        IsEnabled: 'Yes',
    };

    it('specimen: excludes isolate-linked profiles (CultureTestId)', () => {
        const ctx = { ListKind: 'specimen', SpecimenTypeId: 'st1' };
        const profiles = [
            { ...baseProfile, Id: '1', CultureTestId: '1' },
            { ...baseProfile, Id: '2', CultureTestId: '' },
        ];
        const r = filterInstrumentProfilesForManualRequest(ctx, profiles);
        expect(r.map((p) => p.Id)).toEqual(['2']);
    });

    it('specimen: keeps profiles that require a culture type when ctx has no CultureTypeId', () => {
        const ctx = { ListKind: 'specimen', SpecimenTypeId: 'st1' };
        const profiles = [{ ...baseProfile, Id: '1', CultureTypeId: 'ct1' }];
        const r = filterInstrumentProfilesForManualRequest(ctx, profiles);
        expect(r).toHaveLength(1);
    });

    it('culture: excludes direct-only profiles', () => {
        const ctx = { ListKind: 'culture', CultureTypeId: 'ct1', SpecimenTypeId: 'st1' };
        const profiles = [
            { ...baseProfile, Id: '1', DirectTestId: '1', CultureTestId: '' },
            { ...baseProfile, Id: '2', DirectTestId: '', CultureTestId: '1' },
            { ...baseProfile, Id: '3', DirectTestId: '1', CultureTestId: '2' },
        ];
        const r = filterInstrumentProfilesForManualRequest(ctx, profiles);
        expect(r.map((p) => p.Id).sort()).toEqual(['2', '3']);
    });

    it('culture: requires culture type to match when profile specifies CultureTypeId', () => {
        const ctx = { ListKind: 'culture', CultureTypeId: 'ct1', SpecimenTypeId: 'st1' };
        const profiles = [
            { ...baseProfile, Id: '1', CultureTypeId: 'ct1' },
            { ...baseProfile, Id: '2', CultureTypeId: 'ct2' },
        ];
        const r = filterInstrumentProfilesForManualRequest(ctx, profiles);
        expect(r.map((p) => p.Id)).toEqual(['1']);
    });

    it('isolate context: organism group wildcard when context organism not set (matches server matcher)', () => {
        const ctxCulture = { ListKind: 'culture', CultureTypeId: 'ct1', OrgGroupCodingId: '' };
        const profiles = [{ ...baseProfile, Id: '1', OrganismGroupId: 'og1' }];
        expect(filterInstrumentProfilesForManualRequest(ctxCulture, profiles)).toHaveLength(1);
        const ctxOk = { ...ctxCulture, OrgGroupCodingId: 'og1' };
        expect(filterInstrumentProfilesForManualRequest(ctxOk, profiles)).toHaveLength(1);
    });

    it('isolate context: excludes profile when organism group does not match context organism', () => {
        const ctx = { ListKind: 'culture', CultureTypeId: 'ct1', OrgGroupCodingId: 'og2' };
        const profiles = [{ ...baseProfile, Id: '1', OrganismGroupId: 'og1' }];
        expect(filterInstrumentProfilesForManualRequest(ctx, profiles)).toHaveLength(0);
    });

    it('specimen: allows organism group profile when isolate organism not set (wildcard)', () => {
        const ctx = { ListKind: 'specimen', SpecimenTypeId: 'st1', OrgGroupCodingId: '' };
        const profiles = [{ ...baseProfile, Id: '1', OrganismGroupId: 'og1' }];
        expect(filterInstrumentProfilesForManualRequest(ctx, profiles)).toHaveLength(1);
    });

    it('test direct: requires MatchesDirectTestContext true', () => {
        const ctx = { ListKind: 'test', Source: 'direct', SpecimenTypeId: 'st1' };
        const profiles = [
            { ...baseProfile, Id: '1', MatchesDirectTestContext: true },
            { ...baseProfile, Id: '2', MatchesDirectTestContext: false },
            { ...baseProfile, Id: '3' },
        ];
        const r = filterInstrumentProfilesForManualRequest(ctx, profiles);
        expect(r.map((p) => p.Id)).toEqual(['1']);
    });

    it('test culture: requires MatchesCultureTestContext true', () => {
        const ctx = { ListKind: 'test', Source: 'culture', CultureTypeId: 'ct1', OrgGroupCodingId: 'og1' };
        const profiles = [
            { ...baseProfile, Id: '1', MatchesCultureTestContext: true, OrganismGroupId: 'og1' },
            { ...baseProfile, Id: '2', MatchesCultureTestContext: false },
        ];
        const r = filterInstrumentProfilesForManualRequest(ctx, profiles);
        expect(r.map((p) => p.Id)).toEqual(['1']);
    });

    it('respects specimen type on profile', () => {
        const ctx = { ListKind: 'specimen', SpecimenTypeId: 'st1' };
        const profiles = [
            { ...baseProfile, Id: '1', SpecimenTypeId: 'st1' },
            { ...baseProfile, Id: '2', SpecimenTypeId: 'st2' },
        ];
        const r = filterInstrumentProfilesForManualRequest(ctx, profiles);
        expect(r.map((p) => p.Id)).toEqual(['1']);
    });
});

describe('toComboOptions', () => {
    it('maps id and name', () => {
        const o = toComboOptions([{ Id: 'a', InstrumentName: 'N' }]);
        expect(o).toEqual([{ key: 'a', text: 'N' }]);
    });
});
