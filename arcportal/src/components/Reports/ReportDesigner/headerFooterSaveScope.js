import { normaliseName } from './reportChangeSet';

/** Save scope sent to the backend when updating a shared header or footer. */
export const HEADER_FOOTER_SAVE_SCOPE = Object.freeze({
    SHARED: 'Shared',
    CURRENT_REPORT_ONLY: 'CurrentReportOnly',
});

/**
 * Reads linked report count from a section model regardless of JSON casing.
 * @param {object|null|undefined} section
 * @returns {number}
 */
const getLinkedReportCount = (section) =>
    section?.LinkedReportCount ?? section?.linkedReportCount ?? 0;

/**
 * Finds the current header or footer definition for a changed section.
 * @param {object} reportConfig - The live designer configuration.
 * @param {object} section - A changed section from the save payload.
 * @returns {object|null} The matching section definition, if any.
 */
const findSectionDefinition = (reportConfig, section) => {
    if (!reportConfig || !section?.Name) {
        return null;
    }

    const collectionKey = section.Scope === 'Footer'
        ? 'FooterSectionDefinitions'
        : section.Scope === 'Header'
            ? 'HeaderSectionDefinitions'
            : null;

    if (!collectionKey) {
        return null;
    }

    const wanted = normaliseName(section.Name);
    return (reportConfig[collectionKey] || []).find(
        (candidate) => normaliseName(candidate?.Name) === wanted
    ) || null;
};

/**
 * Determines whether a changed section is a shared header or footer edit.
 * @param {object} section - A changed section from the save payload.
 * @param {object} reportConfig - The live designer configuration.
 * @returns {boolean} True when the section is a header or footer linked to more than one report.
 */
export const isSharedHeaderFooterChange = (section, reportConfig) => {
    if (!section?.Scope || (section.Scope !== 'Header' && section.Scope !== 'Footer')) {
        return false;
    }

    const current = findSectionDefinition(reportConfig, section);
    const linkedCount = Math.max(getLinkedReportCount(current), getLinkedReportCount(section));
    return linkedCount > 1;
};

/**
 * Returns changed header or footer sections that require a save-scope choice.
 * @param {object[]} changedSections - Sections from the save payload.
 * @param {object} reportConfig - The live designer configuration.
 * @returns {object[]} Sections that need prompting.
 */
export const getSharedHeaderFooterSectionsNeedingPrompt = (changedSections, reportConfig) =>
    (changedSections || []).filter((section) => isSharedHeaderFooterChange(section, reportConfig));

/**
 * Determines whether the save-scope panel should appear before posting.
 * @param {object[]} changedSections - Sections from the save payload.
 * @param {object} reportConfig - The live designer configuration.
 * @returns {boolean} True when at least one shared header or footer changed.
 */
export const needsHeaderFooterSaveScopePrompt = (changedSections, reportConfig) =>
    getSharedHeaderFooterSectionsNeedingPrompt(changedSections, reportConfig).length > 0;

/**
 * Annotates shared header or footer changes with the chosen save scope.
 * @param {object} saveData - The SaveReportDesignerConfigModel payload.
 * @param {string} saveScope - One of the HEADER_FOOTER_SAVE_SCOPE values.
 * @param {object} reportConfig - The live designer configuration.
 * @returns {object} A copy of the payload with SaveScope set on matching sections.
 */
export const applyHeaderFooterSaveScope = (saveData, saveScope, reportConfig) => {
    if (!saveData) {
        return saveData;
    }

    const changedSections = saveData.ChangedSectionDefinitions || [];

    return {
        ...saveData,
        ChangedSectionDefinitions: changedSections.map((section) => {
            if (!isSharedHeaderFooterChange(section, reportConfig)) {
                return section;
            }

            const current = findSectionDefinition(reportConfig, section);

            return {
                ...section,
                SaveScope: saveScope,
                LinkedReportCount: getLinkedReportCount(current) || getLinkedReportCount(section) || null,
            };
        }),
    };
};
