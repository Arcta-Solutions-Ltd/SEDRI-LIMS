/**
 * Parses a date string that may be in locale format (DD/MM/YYYY) or ISO (YYYY-MM-DD).
 * @param {string} str - Date string to parse.
 * @returns {Date|null} Parsed Date or null if invalid.
 */
const parseDateString = (str) => {
    if (!str || typeof str !== 'string') return null;
    let d = new Date(str);
    if (!isNaN(d.getTime())) return d;
    const slashParts = str.trim().split('/');
    if (slashParts.length === 3) {
        const a = slashParts.map(p => parseInt(p, 10));
        if (a.every(n => !isNaN(n))) {
            if (a[0] > 12) {
                d = new Date(a[2], a[1] - 1, a[0]);
            } else if (a[1] > 12) {
                d = new Date(a[2], a[0] - 1, a[1]);
            } else {
                d = new Date(a[2], a[1] - 1, a[0]);
            }
            if (!isNaN(d.getTime())) return d;
        }
    }
    return null;
};

/**
 * Calculates age in years, months, days, and hours from a date of birth to a reference date.
 * @param {string|Date} dateOfBirth - The date of birth (ISO string, locale DD/MM/YYYY, or Date object).
 * @param {string|Date} [referenceDate] - The reference date (defaults to now). Use CollectionDate/ReceivedDate when available.
 * @returns {{Years: number, Months: number, Days: number, Hours: number}} Age components.
 */
const CalculateAgeFromDateOfBirth = (dateOfBirth, referenceDate) => {
    if (!dateOfBirth) return { Years: 0, Months: 0, Days: 0, Hours: 0 };
    const dob = typeof dateOfBirth === 'string' ? (parseDateString(dateOfBirth) ?? new Date(dateOfBirth)) : dateOfBirth;
    const ref = referenceDate
        ? (typeof referenceDate === 'string' ? (parseDateString(referenceDate) ?? new Date(referenceDate)) : referenceDate)
        : new Date();
    if (!dob || !(dob instanceof Date) || isNaN(dob.getTime()) || isNaN(ref.getTime())) return { Years: 0, Months: 0, Days: 0, Hours: 0 };

    let years = ref.getFullYear() - dob.getFullYear();
    let months = ref.getMonth() - dob.getMonth();
    let days = ref.getDate() - dob.getDate();
    let hours = ref.getHours() - dob.getHours();

    if (hours < 0) {
        hours += 24;
        days -= 1;
    }
    if (days < 0) {
        const prevMonth = new Date(ref.getFullYear(), ref.getMonth(), 0);
        days += prevMonth.getDate();
        months -= 1;
    }
    if (months < 0) {
        months += 12;
        years -= 1;
    }
    if (years < 0) return { Years: 0, Months: 0, Days: 0, Hours: 0 };

    return { Years: years, Months: months, Days: days, Hours: hours };
};

export default CalculateAgeFromDateOfBirth;
