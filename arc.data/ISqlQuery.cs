using arc.domain.Configuration.QueryFiltersConfig;
using System.Threading.Tasks;

namespace arc.data
{
    /// <summary>
    /// Defines classes used to wrap a sql query with a connection in a common way.
    /// </summary>
    public interface ISqlQuery
    {
        /// <summary>
        /// Carries out a query against the database returning an integer.
        /// </summary>
        /// <param name="query">The method which represents the query to be carried out</param>
        /// <param name="errorMessage">The error message to be displayed if the query fails</param>
        /// <param name="queryFilters">Set of filter conditions which are passed to the query</param>
        /// <returns>Returns an integer</returns>
        Task<int> QueryReturningIntegerAsync(IQueryReturningInteger query, string errorMessage, QueryFilterConfig queryFilters);
        /// <summary>
        /// Carries out a query against the database returning a string.
        /// </summary>
        /// <param name="query">The method which represents the query to be carried out</param>
        /// <param name="errorMessage">The error message to be displayed if the query fails</param>
        /// <param name="queryFilters">Set of filter conditions which are passed to the query</param>
        /// <returns>Returns an string</returns>
        Task<T> QueryReturningTypeAsync<T>(IQueryReturningType<T> query, string errorMessage, QueryFilterConfig queryFilters) where T : new();
        /// <summary>
        /// Carries out a query against the database returning a type.
        /// </summary>
        /// <param name="query">The method which represents the query to be carried out</param>
        /// <param name="errorMessage">The error message to be displayed if the query fails</param>
        /// <param name="queryFilters">Set of filter conditions which are passed to the query</param>
        /// <returns>Returns an instance of a type</returns>
        Task<string> QueryReturningStringAsync(IQueryReturningString query, string errorMessage, QueryFilterConfig queryFilters);
        /// <summary>
        /// Carries out a query against the database returning a type.
        /// </summary>
        /// <param name="query">The method which represents the query to be carried out</param>
        /// <param name="errorMessage">The error message to be displayed if the query fails</param>
        /// <param name="entity">Instance of the type, normally reprsenting the model for the table</param>
        /// <param name="queryFilters">Set of filter conditions which are passed to the query</param>
        /// <returns>Returns an instance of a type representing the results of the query</returns>
        Task<T> QuerySendingAndReturningTypeAsync<T>(IQuerySendingAndReturningType<T> query, string errorMessage, T entity, QueryFilterConfig queryFilters) where T : new();
    }
}
