import CheckRule from './CheckRule';

describe('CheckRule', () => {
    describe('contains', () => {
        it('matches an option chosen in a multi select value', () => {
            const rule = { field: 'BirthDateTimeKnown', rule: 'contains', value: '1947' };
            expect(CheckRule(rule, { BirthDateTimeKnown: '1947,1948' })).toBe(true);
        });

        it('does not match an option that was not chosen', () => {
            const rule = { field: 'BirthDateTimeKnown', rule: 'contains', value: '1948' };
            expect(CheckRule(rule, { BirthDateTimeKnown: '1947' })).toBe(false);
        });

        it('ignores spacing around the options', () => {
            const rule = { field: 'WeightKnownId', rule: 'contains', value: '2002' };
            expect(CheckRule(rule, { WeightKnownId: '2001, 2002' })).toBe(true);
        });

        it('matches when any of several rule options is chosen', () => {
            const rule = { field: 'WeightKnownId', rule: 'contains', value: '2001,2002' };
            expect(CheckRule(rule, { WeightKnownId: '2002' })).toBe(true);
        });

        it('is false when the field is absent, so a dependent group stays hidden', () => {
            const rule = { field: 'WeightKnownId', rule: 'contains', value: '2001' };
            expect(CheckRule(rule, {})).toBe(false);
        });

        it('is false when the field is empty', () => {
            const rule = { field: 'WeightKnownId', rule: 'contains', value: '2001' };
            expect(CheckRule(rule, { WeightKnownId: '' })).toBe(false);
        });

        it('does not treat a longer id as a match for a shorter one', () => {
            const rule = { field: 'AntibioticAgentsId', rule: 'contains', value: '194' };
            expect(CheckRule(rule, { AntibioticAgentsId: '1947' })).toBe(false);
        });

        it('reads the field name case insensitively', () => {
            const rule = { Field: 'weightknownid', Rule: 'contains', Value: '2001' };
            expect(CheckRule(rule, { WeightKnownId: '2001' })).toBe(true);
        });
    });

    describe('existing operators still behave', () => {
        it('= matches a single value', () => {
            expect(CheckRule({ field: 'CotAvailableId', rule: '=', value: '1940' }, { CotAvailableId: '1940' })).toBe(true);
        });

        it('= matches any of a comma separated rule value', () => {
            expect(CheckRule({ field: 'CotAvailableId', rule: '=', value: '1940,1941' }, { CotAvailableId: '1941' })).toBe(true);
        });

        it('= does not match a multi select value as a whole', () => {
            expect(CheckRule({ field: 'WeightKnownId', rule: '=', value: '2001' }, { WeightKnownId: '2001,2002' })).toBe(false);
        });

        it('!= is true when the value differs', () => {
            expect(CheckRule({ field: 'SpecimenTypeId', rule: '!=', value: '808' }, { SpecimenTypeId: '810' })).toBe(true);
        });

        it('isnotempty is false for a missing field', () => {
            expect(CheckRule({ field: 'CotId', rule: 'isnotempty' }, {})).toBe(false);
        });

        it('isempty is true for a missing field', () => {
            expect(CheckRule({ field: 'CotId', rule: 'isempty' }, {})).toBe(true);
        });
    });
});
