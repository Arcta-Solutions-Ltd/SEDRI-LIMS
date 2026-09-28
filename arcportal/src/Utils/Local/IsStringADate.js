/**
 * Checks if a given string is in a valid date format.
 * 
 * This function verifies if the provided string follows a valid date format by:
 * 1. Ensuring the string is not undefined or null and is of type 'string'.
 * 2. Counting the number of dashes in the string.
 * 3. Checking if the string can be parsed into a Date object.
 * 4. Ensuring the string does not contain any alphabet characters outside of the date format.
 * 
 * @param {string} stringToCheck - The string to check if it is a date.
 * @returns {boolean} - Returns true if the string is a date, false otherwise.
 */
const IsStringADate = (stringToCheck) => {
    let returnValue = false;
    if (stringToCheck !== undefined && typeof(stringToCheck) == 'string' && stringToCheck !== null) {
        var numberOfDashes = 0;
        Array.from(stringToCheck).forEach(element => {
            if (element === '-') {numberOfDashes++}
        });
        returnValue = (numberOfDashes === 2) && (Date.parse(stringToCheck) !== NaN) && (stringToCheck.search(/[A-SU-Ya-su-y]/) === -1);
    }
    return returnValue;
}

export default IsStringADate;
