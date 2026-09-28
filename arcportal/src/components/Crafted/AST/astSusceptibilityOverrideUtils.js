/**
 * Helpers for manual AST susceptibility override audit state.
 */

const DISK_TEST_METHOD = 681;

/** @param {Object|undefined|null} row */
export function isRowManuallySetSusceptibility(row) {
    return row?.SusceptibilityOverride?.IsManuallySet === true;
}

/** @param {Object|undefined|null} override */
export function hasOverrideReason(override) {
    if (!override) {
        return false;
    }
    const canned = Number(override.CannedCommentId ?? override.cannedCommentId) || 0;
    const freeText = (override.FreeTextComment ?? override.freeTextComment ?? '').trim();
    return canned > 0 || freeText.length > 0;
}

/**
 * @param {Object|undefined|null} row
 * @returns {Object|undefined}
 */
export function mapSusceptibilityOverrideForSave(row) {
    const override = row?.SusceptibilityOverride ?? row?.susceptibilityOverride;
    if (!override) {
        return undefined;
    }
    if (override.ClearOverride === true) {
        return { ClearOverride: true, IsManuallySet: false };
    }
    if (!override.IsManuallySet && override.isManuallySet !== true) {
        return undefined;
    }
    return {
        IsManuallySet: true,
        SetByUsername: override.SetByUsername ?? override.setByUsername ?? '',
        SetAt: override.SetAt ?? override.setAt ?? null,
        CannedCommentId: Number(override.CannedCommentId ?? override.cannedCommentId) || 0,
        FreeTextComment: override.FreeTextComment ?? override.freeTextComment ?? '',
        OverriddenFromSusceptibilityId:
            Number(override.OverriddenFromSusceptibilityId ?? override.overriddenFromSusceptibilityId) || 0,
    };
}

/**
 * @param {Array<{key:string,text:string}>|undefined} cannedOptions
 * @param {Object|undefined|null} override
 * @returns {string}
 */
export function resolveOverridePreviewText(cannedOptions, override) {
    if (!override) {
        return '';
    }
    const cannedId = String(override.CannedCommentId ?? override.cannedCommentId ?? '');
    if (cannedId && cannedId !== '0') {
        const found = (cannedOptions ?? []).find((o) => String(o.key) === cannedId);
        if (found?.text) {
            return found.text;
        }
    }
    return (override.FreeTextComment ?? override.freeTextComment ?? '').trim();
}

/**
 * @param {'disk'|'mic'} type
 * @param {number} index
 * @param {number|undefined} specialIndex
 */
export function buildManualSusceptibilityDomIds(type, index, specialIndex) {
    const section = type === 'disk' ? 'disk' : 'mic';
    const base = specialIndex === undefined || specialIndex === null
        ? `ast-${section}-row-${index}`
        : `ast-${section}-row-${index}-embed-${specialIndex}`;
    return {
        iconId: `${base}-manual-susceptibility-icon`,
        trashId: `${base}-manual-susceptibility-trash`,
    };
}

/** @param {number} testMethodId @param {number} dosage */
export function normalizeDosageForLineKey(testMethodId, dosage) {
    return Number(testMethodId) === DISK_TEST_METHOD ? (Number(dosage) || 0) : 0;
}
