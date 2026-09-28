/**
 * Reads a single value out of the key/value array a crafted page receives as its input data. Entries arrive
 * with either casing depending on whether they came from the page structure or from the form payload.
 *
 * @param {Array<{key?: string, Key?: string, value?: *, Value?: *}>|undefined|null} inputData - Crafted page input.
 * @param {string} name - Lower case name of the value to read.
 * @returns {*} The value, or undefined when the name is not present.
 */
const ReadCraftedInputValue = (inputData, name) => {
    const match = (inputData || []).find((item) => (item.key ?? item.Key)?.toLowerCase() === name);
    return match === undefined ? undefined : (match.value ?? match.Value);
};

export default ReadCraftedInputValue;
