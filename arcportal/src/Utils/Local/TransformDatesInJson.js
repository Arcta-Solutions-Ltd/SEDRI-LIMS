import IsStringADate from './IsStringADate';
import FormatDate from './FormatDate';
import parseLocalDate from '../General/ParseLocalDate';

/**
 * Formats an ISO-style date string to the browser locale, or returns null when the value is not a date.
 *
 * @param {string} value - The string to format.
 * @param {boolean|null} forceDateOnly - When set, overrides date-only detection by string length.
 * @returns {string|null} The formatted date string, or null when the value is not a date.
 */
const formatDateStringToLocale = (value, forceDateOnly = null) => {
    if (!IsStringADate(value)) {
        return null;
    }
    const dateOnly = forceDateOnly !== null ? forceDateOnly : value.length < 11;
    let dateValue = value;
    if (!dateOnly) {
        dateValue = value.replace(' ', 'T');
        const hasTimezone = /[zZ]$|[+-]\d{2}(:?\d{2})?$/.test(dateValue);
        if (!hasTimezone) {
            dateValue += 'Z';
        }
    }
    const newDate = dateOnly ? parseLocalDate(dateValue) : new Date(dateValue);
    if (newDate === undefined || isNaN(newDate.getTime())) {
        return null;
    }
    return FormatDate(newDate, dateOnly);
};

/**
 * Transforms date strings in a JSON object to a formatted date string.
 *
 * @param {Object|Array} json - The JSON object or array to transform.
 */
const TransformDatesInJson = (json) => {
    /**
     * Formats dates to locale strings within a JSON object.
     *
     * @param {Object} jsonObj - The JSON object to format.
     */
    const FormatDatesToLocale = (jsonObj) => {
        for (const key in jsonObj) {
            const value = jsonObj[key];
            const formatted = formatDateStringToLocale(value);
            if (formatted !== null) {
                jsonObj[key] = formatted;
            } else if (key.toLowerCase() === "testresults" && value) {
                const regExp = /\d{4}-\d{2}-\d{2}/;
                let pos = value.search(regExp);
                while (pos > 0) {
                    const dateAsString = value.slice(pos, pos + 10);
                    const newDate = parseLocalDate(dateAsString);
                    if (newDate === undefined || isNaN(newDate.getTime())) {
                        pos = jsonObj[key].indexOf(dateAsString, pos + 1);
                        continue;
                    }
                    const newValue = FormatDate(newDate, true);
                    jsonObj[key] = value.slice(0, pos) + newValue + value.slice(pos + 10);
                    pos = jsonObj[key].search(regExp);
                }
            }
        }
    };

    if (Array.isArray(json)) {
        json.forEach(element => FormatDatesToLocale(element));
    } else {
        FormatDatesToLocale(json);
    }
};

/**
 * Recursively transforms ISO-style date strings anywhere in a parsed JSON value tree.
 *
 * @param {*} value - A parsed JSON value (object, array, string, etc.).
 * @returns {*} The same value with date strings replaced by locale-formatted strings.
 */
const TransformDatesInJsonDeep = (value) => {
    if (typeof value === 'string') {
        return formatDateStringToLocale(value, true) ?? value;
    }
    if (Array.isArray(value)) {
        return value.map(TransformDatesInJsonDeep);
    }
    if (value && typeof value === 'object') {
        for (const key of Object.keys(value)) {
            value[key] = TransformDatesInJsonDeep(value[key]);
        }
    }
    return value;
};

/**
 * Transforms ISO-style date strings in leaf XML element text to locale-formatted strings.
 *
 * @param {string} xmlString - The serialised XML document.
 * @returns {string} The XML document with localised date values.
 */
const TransformDatesInXml = (xmlString) => {
    const parser = new DOMParser();
    const doc = parser.parseFromString(xmlString, 'application/xml');
    const hadDeclaration = xmlString.trimStart().startsWith('<?xml');

    const walk = (element) => {
        if (element.children.length === 0) {
            const text = (element.textContent || '').trim();
            if (text) {
                const formatted = formatDateStringToLocale(text, true);
                if (formatted !== null) {
                    element.textContent = formatted;
                }
            }
            return;
        }
        for (const child of element.children) {
            walk(child);
        }
    };

    if (doc.documentElement) {
        walk(doc.documentElement);
    }

    let output = new XMLSerializer().serializeToString(doc);
    if (hadDeclaration && !output.trimStart().startsWith('<?xml')) {
        output = '<?xml version="1.0" encoding="utf-8"?>' + output;
    }
    return output;
};

/**
 * Formats date strings to locale strings within an array for export.
 *
 * @param {Array<string>} json - The JSON array to format.
 * @returns {Array<string>} The formatted JSON array.
 */
const FormatDatesToLocaleForExport = (json) => {
    return json.map(line => {
        const elements = line.split("|");
        return elements.map((element, index) => {
            const formatted = formatDateStringToLocale(element, true) ?? element;
            return index > 0 ? "|" + formatted : formatted;
        }).join('');
    });
};

export default TransformDatesInJson;
export {
    FormatDatesToLocaleForExport,
    TransformDatesInJsonDeep,
    TransformDatesInXml
};
