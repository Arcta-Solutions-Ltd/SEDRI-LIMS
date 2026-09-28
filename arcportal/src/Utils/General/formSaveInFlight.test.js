import { tryBeginFormSave, clearFormSaveInFlight } from './formSaveInFlight';

describe('formSaveInFlight', () => {
    it('allows the first save and blocks a second until cleared', () => {
        const ref = { current: false };

        expect(tryBeginFormSave(ref)).toBe(true);
        expect(tryBeginFormSave(ref)).toBe(false);

        clearFormSaveInFlight(ref);

        expect(tryBeginFormSave(ref)).toBe(true);
    });
});
