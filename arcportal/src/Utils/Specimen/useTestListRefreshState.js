import { useCallback, useRef, useState } from 'react';

/**
 * Tracks in-flight test list refresh for card panels and embedded grids.
 * @returns {{
 *   listRefreshInFlight: boolean,
 *   beginListRefresh: () => void,
 *   endListRefresh: () => void
 * }}
 */
const useTestListRefreshState = () => {
    const [listRefreshInFlight, setListRefreshInFlight] = useState(false);
    const refreshCountRef = useRef(0);

    /**
     * Marks the test list refresh as started. Supports nested/overlapping calls via a counter.
     */
    const beginListRefresh = useCallback(() => {
        refreshCountRef.current += 1;
        setListRefreshInFlight(true);
    }, []);

    /**
     * Marks one test list refresh as complete. Clears in-flight state when the counter reaches zero.
     */
    const endListRefresh = useCallback(() => {
        refreshCountRef.current = Math.max(0, refreshCountRef.current - 1);
        if (refreshCountRef.current === 0) {
            setListRefreshInFlight(false);
        }
    }, []);

    return {
        listRefreshInFlight,
        beginListRefresh,
        endListRefresh,
    };
};

export default useTestListRefreshState;
