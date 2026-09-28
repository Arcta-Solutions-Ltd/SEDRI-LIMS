import { useState, useEffect } from 'react';

const DEFAULT_DELAY_MS = 200;

/**
 * Returns whether delayed content should be shown after `visible` has been true for `delayMs`.
 * Hides immediately when `visible` becomes false (no delay on hide).
 * @param {boolean} visible
 * @param {number} [delayMs=200]
 * @returns {boolean}
 */
export function useDelayedVisibility(visible, delayMs = DEFAULT_DELAY_MS) {
    const [show, setShow] = useState(false);

    useEffect(() => {
        if (!visible) {
            setShow(false);
            return undefined;
        }
        if (delayMs <= 0) {
            setShow(true);
            return undefined;
        }
        const id = window.setTimeout(() => setShow(true), delayMs);
        return () => window.clearTimeout(id);
    }, [visible, delayMs]);

    return show;
}
