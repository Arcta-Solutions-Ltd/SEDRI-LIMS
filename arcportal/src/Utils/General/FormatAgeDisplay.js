import CalculateAgeFromDateOfBirth from './CalculateAgeFromDateOfBirth';
import TranslateTag from '../Local/TranslateTag';

const unitTag = (count, singularTag, pluralTag) => (count === 1 ? singularTag : pluralTag);

/**
 * Formats age components as a display string with embedded language tags for server-side translation.
 *
 * @param {{Years: number, Months: number, Days: number, Hours: number}} age - Age components.
 * @returns {string} Display string with singular/plural @GenYea@, @GenMon@, @GenDay@, or @GenHou@ tags.
 */
export const formatAgeDisplayWithTags = (age) => {
    if (!age) return '';

    if (age.Years > 0 || age.Months > 0) {
        const parts = [];
        if (age.Years > 0) {
            parts.push(`${age.Years} ${unitTag(age.Years, '@GenYeaC@', '@GenYeaA@')}`);
        }
        if (age.Months > 0) {
            parts.push(`${age.Months} ${unitTag(age.Months, '@GenMonC@', '@GenMonA@')}`);
        }
        return parts.join(' ');
    }

    if (age.Days > 0 || (age.Days === 0 && age.Hours >= 24)) {
        const dayCount = age.Days > 0 ? age.Days : 1;
        return `${dayCount} ${unitTag(dayCount, '@GenDayB@', '@GenDayA@')}`;
    }

    if (age.Hours > 0) {
        return `${age.Hours} ${unitTag(age.Hours, '@GenHouB@', '@GenHouA@')}`;
    }

    return '';
};

/**
 * Formats age from date of birth to a reference date using language tags (for storage / server translation).
 *
 * @param {string|Date} dateOfBirth - Date of birth.
 * @param {string|Date} [referenceDate] - Reference date (defaults to now).
 * @returns {string} Tag-embedded display string.
 */
export const formatAgeFromDateOfBirthWithTags = (dateOfBirth, referenceDate) => {
    const age = CalculateAgeFromDateOfBirth(dateOfBirth, referenceDate);
    return formatAgeDisplayWithTags(age);
};

/**
 * Replaces embedded @Tag@ tokens in a string with translated values for the current language.
 *
 * @param {string} value - String that may contain language tags.
 * @param {Array<{Key: string, Value: string}>} language - Language catalogue from Redux.
 * @returns {string} Translated display string.
 */
export const translateEmbeddedLanguageTags = (value, language) => {
    if (!value || typeof value !== 'string' || !language) {
        return value ?? '';
    }

    return value.replace(/@[A-Za-z0-9]+@/g, (tag) => {
        const translated = TranslateTag(tag, language);
        return translated || tag;
    });
};

export default formatAgeDisplayWithTags;
