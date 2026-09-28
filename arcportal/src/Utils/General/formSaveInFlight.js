/**
 * Synchronous in-flight guard for form saves. React {@code saveEnabled} state updates
 * asynchronously; a ref blocks double-clicks before re-render.
 */

/**
 * @param {React.MutableRefObject<boolean>} saveInFlightRef - Ref tracking an active save POST.
 * @returns {boolean} True when the caller may proceed with save; false when already saving.
 */
const tryBeginFormSave = (saveInFlightRef) => {
    if (saveInFlightRef.current) {
        return false;
    }
    saveInFlightRef.current = true;
    return true;
};

/**
 * @param {React.MutableRefObject<boolean>} saveInFlightRef - Ref to clear after save completes or fails.
 */
const clearFormSaveInFlight = (saveInFlightRef) => {
    saveInFlightRef.current = false;
};

export { tryBeginFormSave, clearFormSaveInFlight };
