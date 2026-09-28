/**
 * Parses a date-only value as a calendar date.
 * yyyy-MM-dd strings must NOT go through new Date(string) — that attaches UTC midnight.
 *
 * @param {Date|string|undefined|null} value - A Date instance or date string.
 * @returns {Date|undefined} The parsed calendar date, or undefined when invalid.
 */
export const parseLocalDate = (value) => {
    if (value instanceof Date) {
        return value;
    }
    if (typeof value !== 'string') {
        return undefined;
    }

    const match = /^(\d{4})-(\d{2})-(\d{2})$/.exec(value.trim());
    if (match) {
        return new Date(+match[1], +match[2] - 1, +match[3]);
    }

    const parsed = new Date(value);
    return isNaN(parsed.getTime()) ? undefined : parsed;
};

export default parseLocalDate;
