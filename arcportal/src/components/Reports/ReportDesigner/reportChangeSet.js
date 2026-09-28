/**
 * Baseline diff for the report designer.
 *
 * The designer used to decide what to save from two React `Set`s of names that were rebuilt, and
 * sometimes cleared, by effects reacting to prop changes. A newly created format could therefore be
 * dropped from the payload while the section that referenced it was still saved, leaving the section
 * pointing at a format that was never written.
 *
 * This module is pure: it compares the current state against the baseline captured when the report
 * was loaded, so nothing in the React lifecycle can clear it. Records are keyed on `ConfigId` first
 * and `Name` second, matching the way the backend matches configuration records.
 */

const CHANGE_STATE = Object.freeze({
    UNCHANGED: 'Unchanged',
    ADDED: 'Added',
    MODIFIED: 'Modified',
    DELETED: 'Deleted',
});

/**
 * The collections a section definition can live in, and the scope each one implies.
 *
 * The scope travels with a section so the backend can tell a header from an ordinary section when the
 * section is new and has no configs record to take a type from. Scopes are names rather than
 * ConfigTypeId numbers so the mapping stays in one place on the backend.
 */
const SECTION_COLLECTIONS = [
    { key: 'SectionDefinitions', scope: 'Section' },
    { key: 'HeaderSectionDefinitions', scope: 'Header' },
    { key: 'FooterSectionDefinitions', scope: 'Footer' },
];

/** Keys that only exist to drive the save and must never be compared or persisted. */
const TRANSIENT_KEYS = ['State', 'IsNew', 'ConfigId', 'ConfigTypeId', 'Scope', 'SaveScope', 'LinkedReportCount'];

/**
 * Normalises a configuration name the same way the backend does: trimmed and lower case.
 * @param {string} name
 * @returns {string} The normalised name, or an empty string.
 */
const normaliseName = (name) => (typeof name === 'string' ? name.trim().toLowerCase() : '');

/**
 * Finds a format by name the way the backend matches configuration names: trimmed and case
 * insensitive. A section's stored `Format` and a format record's stored `Name` are normalised
 * independently of each other, so an exact comparison drops a section's format and leaves the
 * section editor unable to save.
 *
 * The stored record is returned rather than a boolean so callers can adopt its own `Name` as the
 * canonical spelling, which is what the format dropdown selects on.
 * @param {object[]} formats - The formats to search.
 * @param {string} name - The name to look for, in any casing.
 * @returns {object|null} The matching format, or null when there is none.
 */
const findFormatByName = (formats, name) => {
    const wanted = normaliseName(name);
    if (!wanted) return null;
    return (formats || []).find(format => normaliseName(format?.Name) === wanted) || null;
};

/**
 * Builds the key used to match a record between the baseline and the current state.
 * Prefers the configs identity, because a record can be renamed but keeps its identity.
 * @param {object} record
 * @returns {string} A stable lookup key.
 */
const keyFor = (record) => {
    if (!record) return '';
    if (record.ConfigId !== undefined && record.ConfigId !== null && record.ConfigId !== 0) {
        return `id:${record.ConfigId}`;
    }
    return `name:${normaliseName(record.Name)}`;
};

/**
 * Produces a copy of a record with transient designer keys removed, so comparisons only look at
 * data that is actually stored.
 * @param {object} record
 * @returns {object} The comparable copy.
 */
const withoutTransientKeys = (record) => {
    if (!record || typeof record !== 'object') return record;
    const copy = { ...record };
    TRANSIENT_KEYS.forEach((key) => delete copy[key]);
    return copy;
};

/**
 * Stable stringify so key ordering differences between a loaded record and an edited record do not
 * register as a change.
 * @param {*} value
 * @returns {string}
 */
const stableStringify = (value) => {
    if (value === null || value === undefined) return 'null';
    if (Array.isArray(value)) return `[${value.map(stableStringify).join(',')}]`;
    if (typeof value === 'object') {
        return `{${Object.keys(value)
            .sort()
            .map((key) => `${JSON.stringify(key)}:${stableStringify(value[key])}`)
            .join(',')}}`;
    }
    return JSON.stringify(value);
};

/**
 * Deep value comparison that ignores transient designer keys and property ordering.
 * @param {object} left
 * @param {object} right
 * @returns {boolean} True when both records would be stored identically.
 */
const isSameRecord = (left, right) =>
    stableStringify(withoutTransientKeys(left)) === stableStringify(withoutTransientKeys(right));

/**
 * Indexes a list of records by both identity and name so a record can be found either way.
 * @param {object[]} records
 * @returns {Map<string, object>}
 */
const indexRecords = (records) => {
    const index = new Map();
    (records || []).forEach((record) => {
        if (!record || !record.Name) return;
        index.set(keyFor(record), record);
        index.set(`name:${normaliseName(record.Name)}`, record);
    });
    return index;
};

/**
 * Collects every section definition held anywhere in a report configuration, de-duplicated by name.
 *
 * Each section is stamped with the scope of the collection it came from, unless it already carries
 * one from the backend. A name held in more than one collection keeps the scope of the first
 * collection it appears in, which is the same record the de-duplication keeps.
 *
 * @param {object} config
 * @returns {object[]} The section definitions.
 */
const collectSections = (config) => {
    const sections = [];
    const seen = new Set();

    SECTION_COLLECTIONS.forEach(({ key, scope }) => {
        (config?.[key] || []).forEach((section) => {
            if (!section || typeof section !== 'object' || !section.Name) return;
            const name = normaliseName(section.Name);
            if (seen.has(name)) return;
            seen.add(name);
            sections.push(section.Scope ? section : { ...section, Scope: scope });
        });
    });

    return sections;
};

/**
 * Diffs one collection of records against its baseline.
 *
 * `ConfigTypeId` and `Scope` are carried across from the baseline the same way `ConfigId` is, because
 * the backend needs them to write a record back to the type it was loaded from. A record with neither
 * is new, and the backend infers its type from the category that owns it.
 *
 * @param {object[]} baselineRecords
 * @param {object[]} currentRecords
 * @returns {{changed: object[], deleted: {ConfigId: (number|null), ConfigTypeId: (number|null), Scope: (string|null), Name: string}[]}}
 */
const diffRecords = (baselineRecords, currentRecords) => {
    const baselineIndex = indexRecords(baselineRecords);
    const changed = [];
    const stillPresent = new Set();

    (currentRecords || []).forEach((record) => {
        if (!record || !record.Name) return;

        const baseline = baselineIndex.get(keyFor(record)) || baselineIndex.get(`name:${normaliseName(record.Name)}`);

        if (baseline) {
            stillPresent.add(keyFor(baseline));
            stillPresent.add(`name:${normaliseName(baseline.Name)}`);

            if (!isSameRecord(baseline, record)) {
                changed.push({
                    ...record,
                    ConfigId: record.ConfigId ?? baseline.ConfigId ?? null,
                    ConfigTypeId: record.ConfigTypeId ?? baseline.ConfigTypeId ?? null,
                    Scope: record.Scope ?? baseline.Scope ?? null,
                    State: CHANGE_STATE.MODIFIED,
                });
            }
            return;
        }

        // No ConfigId, because an added record must be inserted rather than matched to an existing
        // one. The type is kept when the record has one, so the insert still lands on the right type.
        changed.push({
            ...record,
            ConfigId: null,
            ConfigTypeId: record.ConfigTypeId ?? null,
            Scope: record.Scope ?? null,
            State: CHANGE_STATE.ADDED,
        });
    });

    const deleted = [];
    const seenDeletions = new Set();

    (baselineRecords || []).forEach((record) => {
        if (!record || !record.Name) return;
        const key = keyFor(record);
        if (stillPresent.has(key) || stillPresent.has(`name:${normaliseName(record.Name)}`)) return;
        if (seenDeletions.has(key)) return;
        seenDeletions.add(key);
        deleted.push({
            ConfigId: record.ConfigId ?? null,
            ConfigTypeId: record.ConfigTypeId ?? null,
            Scope: record.Scope ?? null,
            Name: record.Name,
        });
    });

    return { changed, deleted };
};

/**
 * Builds the complete change set for a save by diffing the current designer state against the
 * baseline captured when the report was loaded.
 *
 * @param {object} baseline - The report configuration exactly as it was loaded from the backend.
 * @param {object} currentConfig - The report configuration as it stands in the designer.
 * @param {object[]} currentFormats - The custom formats as they stand in the designer.
 * @returns {{changedSections: object[], changedFormats: object[], deletedSections: object[], deletedFormats: object[]}}
 *   Records to write, each carrying its `ConfigId` and `State`, and identity references for records to delete.
 */
const buildChangeSet = (baseline, currentConfig, currentFormats) => {
    const sectionDiff = diffRecords(collectSections(baseline), collectSections(currentConfig));
    const formatDiff = diffRecords(baseline?.CustomFormats || [], currentFormats || []);

    return {
        changedSections: sectionDiff.changed,
        changedFormats: formatDiff.changed,
        deletedSections: sectionDiff.deleted,
        deletedFormats: formatDiff.deleted,
    };
};

export default buildChangeSet;
export { buildChangeSet, collectSections, normaliseName, findFormatByName, CHANGE_STATE };
