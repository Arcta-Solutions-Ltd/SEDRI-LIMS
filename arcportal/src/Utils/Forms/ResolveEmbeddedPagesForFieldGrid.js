/**
 * Collects page definitions referenced by a fieldgrid's subform UI events so list-based display
 * resolution (FieldFormat / GetTextForListItemsInGrid) can map field keys to OptionsName.
 *
 * Looks up `FormUIEvent` and `AddFormUIEvent` on the fieldgrid field config, resolves each to a
 * form via `uievents` → `Action`, then loads each page named in `form.Pages` from `allPages`.
 *
 * @param {Object} fieldGridConfig - Field definition with Type fieldgrid; may include FormUIEvent, AddFormUIEvent (camelCase or PascalCase).
 * @param {Array<{Name?: string, name?: string, Action?: string, action?: string}>} uievents - UI event definitions from config.
 * @param {Array<{Name?: string, name?: string, Pages?: string[]}>} forms - Form definitions from config.
 * @param {Array<{Name?: string, name?: string}>} allPages - Full page catalog from config (e.g. Redux `state.config.pages`).
 * @returns {Array<Object>} Deduped page objects, in discovery order.
 */
const ResolveEmbeddedPagesForFieldGrid = (fieldGridConfig, uievents, forms, allPages) => {
    if (!fieldGridConfig || !Array.isArray(allPages) || allPages.length === 0) {
        return [];
    }

    const seen = new Set();
    const out = [];

    const formName = (f) => (f.Name ?? f.name ?? '').toString();
    const pageName = (p) => (p.Name ?? p.name ?? '').toString();

    const addFromUiEventName = (uiEventName) => {
        if (!uiEventName || typeof uiEventName !== 'string' || !Array.isArray(uievents)) {
            return;
        }
        const uie = uievents.find(
            (a) => (a.Name ?? a.name ?? '').toString().toLowerCase() === uiEventName.toLowerCase()
        );
        if (!uie) {
            return;
        }
        const actionFormName = uie.Action ?? uie.action;
        if (!actionFormName || !Array.isArray(forms)) {
            return;
        }
        const form = forms.find((f) => formName(f).toLowerCase() === String(actionFormName).toLowerCase());
        if (!form) {
            return;
        }
        const formPageNames = form.Pages ?? form.pages;
        if (!Array.isArray(formPageNames)) {
            return;
        }
        for (const pn of formPageNames) {
            const page = allPages.find((p) => pageName(p).toLowerCase() === String(pn).toLowerCase());
            if (!page) {
                continue;
            }
            const key = pageName(page);
            if (seen.has(key)) {
                continue;
            }
            seen.add(key);
            out.push(page);
        }
    };

    addFromUiEventName(fieldGridConfig.FormUIEvent ?? fieldGridConfig.formUIEvent);
    addFromUiEventName(fieldGridConfig.AddFormUIEvent ?? fieldGridConfig.addFormUIEvent);

    return out;
};

export default ResolveEmbeddedPagesForFieldGrid;
