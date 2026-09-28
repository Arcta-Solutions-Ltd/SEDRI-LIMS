/** Stable sidebar keys that must always remain enabled (match backend MenuPermissionKeys). */
export const MANDATORY_SIDEBAR_KEYS = ['home'];

/**
 * @param {string} key Sidebar menu key.
 * @returns {boolean} True when the key is mandatory and cannot be disabled.
 */
export const isMandatorySidebarKey = (key) =>
    MANDATORY_SIDEBAR_KEYS.some((mandatoryKey) => mandatoryKey === key);
