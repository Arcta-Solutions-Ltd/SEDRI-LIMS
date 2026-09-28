import ResetFormForRepeat from './ResetFormForRepeat';

const buildPage = (name, fieldIds, extra = {}) => ({
    Name: name,
    Visible: true,
    Columns: [
        {
            FormGroups: [
                { Fields: fieldIds.map((id) => ({ Id: id, value: 'entered' })) }
            ]
        }
    ],
    ...extra
});

const buildNeoshieldState = () => ({
    currentPage: { Name: 'neoshieldbottlepage' },
    currentState: 'newrequest,bloodspecimen',
    fieldChanges: ['CollectionDate'],
    rootElements: [{ id: 'something' }],
    formDef: {
        Rules: [
            { Outcome: 'visible', Page: 'neoshieldbottlepage', State: 'bloodspecimen' }
        ],
        Pages: [
            buildPage('neoshieldrequestheaderpage', ['WardId', 'UrgencyId']),
            buildPage('neoshieldspecimenpage', ['SpecimenTypeId', 'CollectionDate'], {
                NextButton: { OnClickState: { State: 'bloodspecimen' } }
            }),
            buildPage('neoshieldbottlepage', ['BottleTypeId'])
        ],
        data: {
            PatientRef: 'NEO-1',
            WardId: '1971',
            UrgencyId: '1981',
            SpecimenTypeId: '808',
            CollectionDate: '2026-08-18',
            BottleTypeId: '2070',
            Id: '4321',
            AccessionNumber: 'SP0004321'
        }
    }
});

const buildGenericState = () => ({
    currentPage: { Name: 'pageC' },
    currentState: 'extraStep',
    fieldChanges: ['fieldB'],
    rootElements: [{ id: 'root' }],
    formDef: {
        Rules: [
            { Outcome: 'visible', Page: 'pageC', State: 'extraStep' }
        ],
        Pages: [
            buildPage('pageA', ['FieldA']),
            buildPage('pageB', ['FieldB'], {
                NextButton: { OnClickState: { State: 'extraStep' } }
            }),
            buildPage('pageC', ['FieldC'])
        ],
        data: {
            FieldA: 'keep-me',
            FieldB: 'clear-me',
            FieldC: 'clear-me-too',
            Id: '99',
            accessionNumber: 'ACC-99'
        }
    }
});

describe('ResetFormForRepeat', () => {
    describe('Neoshield-shaped fixture', () => {
        it('clears the repeated pages and keeps the earlier answers', () => {
            const state = ResetFormForRepeat(buildNeoshieldState(), { FromPage: 'neoshieldspecimenpage' }, {});

            expect(state.formDef.data.WardId).toBe('1971');
            expect(state.formDef.data.UrgencyId).toBe('1981');
            expect(state.formDef.data.SpecimenTypeId).toBeUndefined();
            expect(state.formDef.data.CollectionDate).toBeUndefined();
            expect(state.formDef.data.BottleTypeId).toBeUndefined();
        });

        it('positions the form back on the repeat page', () => {
            const state = ResetFormForRepeat(buildNeoshieldState(), { FromPage: 'neoshieldspecimenpage' }, {});

            expect(state.currentPage.Name).toBe('neoshieldspecimenpage');
            expect(state.fieldChanges).toEqual([]);
            expect(state.rootElements).toEqual([]);
        });

        it('drops the identifiers of the record just saved', () => {
            const state = ResetFormForRepeat(buildNeoshieldState(), { FromPage: 'neoshieldspecimenpage' }, {});

            expect(state.formDef.data.Id).toBeUndefined();
            expect(state.formDef.data.AccessionNumber).toBeUndefined();
        });

        it('carries the created parents over from a camel cased save response', () => {
            const created = { id: '4321', patientId: '77', admissionId: '12', requestId: '34' };
            const state = ResetFormForRepeat(buildNeoshieldState(), { FromPage: 'neoshieldspecimenpage' }, created);

            expect(state.formDef.data.PatientId).toBe('77');
            expect(state.formDef.data.AdmissionId).toBe('12');
            expect(state.formDef.data.RequestId).toBe('34');
        });

        it('writes parent ids when placeholders were zero on the first run', () => {
            const initial = buildNeoshieldState();
            initial.formDef.data.PatientId = '0';
            initial.formDef.data.AdmissionId = '0';
            initial.formDef.data.RequestId = '0';

            const created = { patientId: '77', admissionId: '12', requestId: '34' };
            const state = ResetFormForRepeat(initial, { FromPage: 'neoshieldspecimenpage' }, created);

            expect(state.formDef.data.PatientId).toBe('77');
            expect(state.formDef.data.AdmissionId).toBe('12');
            expect(state.formDef.data.RequestId).toBe('34');
        });

        it('leaves a parent alone when the save did not return one', () => {
            const initial = buildNeoshieldState();
            initial.formDef.data.AdmissionId = '55';

            const created = { patientId: '77', admissionId: '', requestId: null };
            const state = ResetFormForRepeat(initial, { FromPage: 'neoshieldspecimenpage' }, created);

            expect(state.formDef.data.PatientId).toBe('77');
            expect(state.formDef.data.AdmissionId).toBe('55');
            expect(state.formDef.data.RequestId).toBeUndefined();
        });

        it('removes workflow state tokens contributed by cleared pages', () => {
            const state = ResetFormForRepeat(buildNeoshieldState(), { FromPage: 'neoshieldspecimenpage' }, {});

            expect(state.currentState).toBe('newrequest');
            expect(state.formDef.Pages.find((p) => p.Name === 'neoshieldbottlepage').Visible).toBe(false);
        });

        it('resolves fromPage in camelCase', () => {
            const state = ResetFormForRepeat(buildNeoshieldState(), { fromPage: 'neoshieldspecimenpage' }, {});

            expect(state.currentPage.Name).toBe('neoshieldspecimenpage');
        });

        it('resets the choices made on a repeated crafted page', () => {
            const initial = buildNeoshieldState();
            initial.formDef.Pages.splice(2, 0, { Name: 'testselectionpage', Crafted: true, Visible: true });
            initial.formDef.data.Crafted = [
                { Name: 'testselectionpage', Contents: [{ Id: '1', Allowed: 'Yes' }, { Id: '2', Allowed: 'No' }] }
            ];

            const state = ResetFormForRepeat(initial, { FromPage: 'neoshieldspecimenpage' }, {});

            expect(state.formDef.data.Crafted[0].Contents.every((item) => item.Allowed === 'No')).toBe(true);
        });

        it('returns the state untouched when the repeat page is not in the form', () => {
            const initial = buildNeoshieldState();
            const state = ResetFormForRepeat(initial, { FromPage: 'nosuchpage' }, {});

            expect(state).toBe(initial);
            expect(state.formDef.data.SpecimenTypeId).toBe('808');
        });
    });

    describe('generic fixture', () => {
        it('clears tail pages and keeps head page data', () => {
            const state = ResetFormForRepeat(buildGenericState(), { FromPage: 'pageB' }, {});

            expect(state.formDef.data.FieldA).toBe('keep-me');
            expect(state.formDef.data.FieldB).toBeUndefined();
            expect(state.formDef.data.FieldC).toBeUndefined();
        });

        it('lands on the configured from page', () => {
            const state = ResetFormForRepeat(buildGenericState(), { fromPage: 'pageB' }, {});

            expect(state.currentPage.Name).toBe('pageB');
        });

        it('clears server-stamped record keys case-insensitively', () => {
            const state = ResetFormForRepeat(buildGenericState(), { FromPage: 'pageB' }, {});

            expect(state.formDef.data.Id).toBeUndefined();
            expect(state.formDef.data.accessionNumber).toBeUndefined();
        });

        it('removes workflow state from cleared pages only', () => {
            const state = ResetFormForRepeat(buildGenericState(), { FromPage: 'pageB' }, {});

            expect(state.currentState).toBe('');
            expect(state.formDef.Pages.find((p) => p.Name === 'pageC').Visible).toBe(false);
        });
    });
});
