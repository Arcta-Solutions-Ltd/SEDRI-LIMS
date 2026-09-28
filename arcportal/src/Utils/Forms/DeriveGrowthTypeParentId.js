/**
 * Derives growthTypeParentId from growthid using the SpecimenGrowth list (from Redux/form config).
 * Used when growth options have a hierarchy (Growth/No Growth parents); rules reference growthTypeParentId.
 * @param {Array} lists - Lists loaded in form context (from Redux or form config)
 * @param {string|number} growthId - Selected growth list item id
 * @returns {string|undefined} Parent list item id (e.g. 427=Growth, 428=No Growth), or undefined if not found
 */
const DeriveGrowthTypeParentId = (lists, growthId) => {
    if (growthId == null || growthId === '') return undefined;
    const growthIdStr = String(growthId).trim();
    if (!growthIdStr || !lists?.length) return undefined;

    const specimenGrowthList = lists.find(l => (l.Name || l.name || '').toLowerCase() === 'specimengrowth');
    const opt = (specimenGrowthList?.Options || []).find(o => String(o.key ?? o.Key) === growthIdStr);
    const parentKey = opt?.ParentKey ?? opt?.parentkey ?? opt?.parentKey;
    return parentKey != null && parentKey !== '' ? String(parentKey) : undefined;
};

export default DeriveGrowthTypeParentId;
