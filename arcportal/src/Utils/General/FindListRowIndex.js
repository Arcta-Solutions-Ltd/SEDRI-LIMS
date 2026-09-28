/**
 * Finds the index of a list row by record id, using string comparison so numeric and
 * string ids (and id vs Id casing) match consistently.
 *
 * @param {Array|undefined} listData - List rows from ManageList.
 * @param {string|number|undefined|null} id - Record id to locate.
 * @returns {number} Row index, or -1 when not found or listData is not an array.
 */
const findListRowIndex = (listData, id) => {
    if (!Array.isArray(listData) || id === undefined || id === null) {
        return -1;
    }
    const idStr = String(id);
    return listData.findIndex(item => String(item.id ?? item.Id) === idStr);
};

export default findListRowIndex;
