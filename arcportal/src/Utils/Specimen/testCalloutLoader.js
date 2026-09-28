import Post from '../../Data/Post';
import FindCaseInsensitiveProperty from '../General/FindCaseInsensitiveProperty';

/** @type {Map<string, Object>} */
const calloutCacheByTestId = new Map();

/**
 * Removes cached callout data for a test row (e.g. after edit).
 * @param {number|string} testRowId - Tests.Id or CultureTests.Id.
 */
export const invalidateCalloutCacheForTest = (testRowId) => {
    if (testRowId !== undefined && testRowId !== null) {
        calloutCacheByTestId.delete(String(testRowId));
    }
};

/**
 * Returns true when the row may have hover callout content but formatted results are not loaded yet.
 * @param {Object} item - Grid row.
 * @returns {boolean}
 */
export const rowNeedsLazyCalloutFetch = (item) => {
    if (!item) {
        return false;
    }
    const stateKey = FindCaseInsensitiveProperty(item, 'stateid');
    const stateId = stateKey ? String(item[stateKey] ?? '') : '';
    if (stateId.includes('incomplete')) {
        return false;
    }
    const resultsKey = FindCaseInsensitiveProperty(item, 'TestResults');
    const testResults = resultsKey ? item[resultsKey] : undefined;
    if (testResults === undefined || testResults === null || testResults === '' || testResults === '[]') {
        return true;
    }
    if (typeof testResults === 'string') {
        try {
            const parsed = JSON.parse(testResults);
            return parsed === null || (typeof parsed === 'object' && Object.keys(parsed).length === 0);
        } catch (e) {
            return true;
        }
    }
    return false;
};

/**
 * Fetches formatted TestResults for a single test row, with in-memory cache keyed by test id.
 * @param {'specimen'|'culture'} parentType
 * @param {number|string} testRowId
 * @param {Function} onSuccess
 * @param {Function} onError
 */
export const fetchFormattedTestResultsForCallout = (parentType, testRowId, onSuccess, onError) => {
    const cacheKey = String(testRowId);
    if (calloutCacheByTestId.has(cacheKey)) {
        onSuccess(calloutCacheByTestId.get(cacheKey));
        return;
    }

    const queryName = parentType === 'culture'
        ? 'activeculturetestbyidfortestlistquery'
        : 'activetestbyidfortestlistquery';

    Post(
        'query/filteredget',
        { Name: queryName, Parameters: [{ Key: 'id', Value: testRowId }] },
        (row) => {
            calloutCacheByTestId.set(cacheKey, row);
            onSuccess(row);
        },
        onError
    );
};
