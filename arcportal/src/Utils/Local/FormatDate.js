/**
 * Formats a date as a string.
 * 
 * @param {Date} dateToFormat - The date to format.
 * @param {boolean} dateOnly - If true, returns only the date part.
 * @returns {string} The formatted date string.
 */
const FormatDate = (dateToFormat, dateOnly = false) => {
    // Format the date to locale string
    let formattedDate = dateToFormat.toLocaleString();

    // If only the date part is needed, extract it
    if (dateOnly) {
        formattedDate = formattedDate.split(',')[0];
    }

    // Return the formatted date without commas
    return formattedDate.replace(',', '');
};

export default FormatDate;

