import enStrings, { loginStrings as enLoginStrings } from '../i18n/en';
import nlStrings, { loginStrings as nlLoginStrings } from '../i18n/nl';
import ruStrings, { loginStrings as ruLoginStrings } from '../i18n/ru';
import esStrings, { loginStrings as esLoginStrings } from '../i18n/es';

class LanguageService {
  constructor() {
    this.translations = {
      ENGLISH: enStrings,
      DUTCH: nlStrings,
      RUSSIAN: ruStrings,
      SPANISH: esStrings
    };
    this.loginTranslations = {
      ENGLISH: enLoginStrings,
      DUTCH: nlLoginStrings,
      RUSSIAN: ruLoginStrings,
      SPANISH: esLoginStrings
    };
  }

  /**
   * Get DayPicker strings based on the current language setting
   * @returns {Object} DayPicker strings for the current language
   */
  getDayPickerStrings() {
    const language = process.env.REACT_APP_LANGUAGE || 'ENGLISH';
    return this.translations[language] || this.translations.ENGLISH;
  }

  /**
   * Get DayPicker strings for a specific language
   * @param {string} language - Language code (ENGLISH, DUTCH, RUSSIAN, SPANISH)
   * @returns {Object} DayPicker strings for the specified language
   */
  getDayPickerStringsByLanguage(language) {
    return this.translations[language] || this.translations.ENGLISH;
  }

  /**
   * Get the current language setting
   * @returns {string} Current language code
   */
  getCurrentLanguage() {
    return process.env.REACT_APP_LANGUAGE || 'ENGLISH';
  }

  /**
   * Get login strings based on the current language setting
   * @returns {Object} Login strings for the current language
   */
  getLoginStrings() {
    const language = process.env.REACT_APP_LANGUAGE || 'ENGLISH';
    return this.loginTranslations[language] || this.loginTranslations.ENGLISH;
  }

  /**
   * Get login strings for a specific language
   * @param {string} language - Language code (ENGLISH, DUTCH, RUSSIAN, SPANISH)
   * @returns {Object} Login strings for the specified language
   */
  getLoginStringsByLanguage(language) {
    return this.loginTranslations[language] || this.loginTranslations.ENGLISH;
  }
}

// Export a singleton instance
const languageService = new LanguageService();
export default languageService;
