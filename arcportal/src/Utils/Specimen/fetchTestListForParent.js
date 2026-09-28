import Post from '../../Data/Post';

/**
 * Builds filteredget criteria for a test list query scoped to a parent record id.
 * @param {string} queryName - Query config name (e.g. testlistforspecimenlite).
 * @param {number|string} parentId - Specimen or culture id.
 * @returns {{ Name: string, Parameters: Array<{ Key: string, Value: * }> }}
 */
const buildTestListCriteria = (queryName, parentId) => ({
    Name: queryName,
    Parameters: [{ Key: 'id', Value: parentId }],
});

/**
 * Fetches the direct test list for a specimen via testlistforspecimenlite.
 * @param {number|string} specimenId - Specimen id.
 * @param {Function} onSuccess - Called with the query result array.
 * @param {Function} onError - Called with the error response.
 */
export const fetchDirectTestList = (specimenId, onSuccess, onError) => {
    Post(
        'query/filteredget',
        buildTestListCriteria('testlistforspecimenlite', specimenId),
        onSuccess,
        onError
    );
};

/**
 * Fetches the isolate test list for a culture via testlistforculturelite.
 * @param {number|string} cultureId - Culture (isolate) id.
 * @param {Function} onSuccess - Called with the query result array.
 * @param {Function} onError - Called with the error response.
 */
export const fetchCultureTestList = (cultureId, onSuccess, onError) => {
    Post(
        'query/filteredget',
        buildTestListCriteria('testlistforculturelite', cultureId),
        onSuccess,
        onError
    );
};

/**
 * Fetches the test list for the given parent type using lite list queries.
 * @param {'specimen'|'culture'} parentType - Whether the parent is a specimen or culture.
 * @param {number|string} parentId - Specimen or culture id.
 * @param {Function} onSuccess - Called with the query result array.
 * @param {Function} onError - Called with the error response.
 */
export const fetchTestListForParent = (parentType, parentId, onSuccess, onError) => {
    if (parentType === 'culture') {
        fetchCultureTestList(parentId, onSuccess, onError);
    } else {
        fetchDirectTestList(parentId, onSuccess, onError);
    }
};
