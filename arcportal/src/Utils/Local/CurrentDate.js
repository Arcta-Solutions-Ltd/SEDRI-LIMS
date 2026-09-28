/**
 * Gets the current date as a formatted string.
 * 
 * @returns {string} The current date in a human-readable format.
 */
const CurrentDate = () => {
    const today = new Date();
    return today.toDateString();
};

export default CurrentDate;

