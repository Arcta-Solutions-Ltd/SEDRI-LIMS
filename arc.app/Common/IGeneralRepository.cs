using arc.common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Common
{
    /// <summary>
    /// Defines the methods that a general repository must implement.
    /// </summary>
    public interface IGeneralRepository
    {
        /// <summary>
        /// Calls the query the will return all the rows in a table in the database.
        /// </summary>
        /// <param name="tableName">The name of the table in the database you want to extract the rows from</param>
        /// <typeparam name="T">The type of item that represents a row in the database.</typeparam>
        /// <returns>IEnumerable of all the rows in a table in the database</returns>
        Task<IEnumerable<T>> GetAllAsync<T>(string tableName);
        /// <summary>
        /// Retrieves a single record by its ID from the specified table.
        /// </summary>
        /// <typeparam name="T">The type of the entity to retrieve.</typeparam>
        /// <param name="entity">An instance of the entity used as a template for the query.</param>
        /// <param name="tableName">The name of the database table to query.</param>
        /// <param name="id">The unique identifier of the record to retrieve.</param>
        /// <returns>A task representing the asynchronous operation, containing the retrieved entity.</returns>
        Task<T> GetByIdAsync<T>(string tableName, int id) where T : new();
        /// <summary>
        /// Updates a record in the database.
        /// </summary>
        /// <param name="entity">Model to use to update the record</param>
        /// <param name="tableName">Name of the table to update</param>
        /// <param name="keyColumn">Name of the property to select the field on</param>
        /// <returns>IEnumerable of all the rows in a table in the database</returns>
        Task<int> UpdateAsync<T>(T entity, string tableName, string keyColumn);
        /// <summary>
        /// Calls the query the will return a single row of a table in the database.
        /// </summary>
        /// <param name="entity">Model to use to update the record</param>
        /// <param name="tableName">Name of the table to update</param>
        /// <param name="keyColumn">Name of the property to select the field on</param>
        /// <typeparam name="T">The type of item that represents a row in the database.</typeparam>
        /// <returns>Object representing the row in the database</returns>
        Task<T> GetSingleRecordAsync<T>(T entity, string tableName, string keyColumn, string moredataValue = "") where T : new();
        /// <summary>
        /// Calls the query that will return a single row of a table in the database.
        /// </summary>
        /// <param name="entity">Model to use to update the record</param>
        /// <param name="tableName">Name of the table to update</param>
        /// <param name="keyValues">List of parameters for the query where Key holds the fields name and Value the value to find in the field</param>
        /// <typeparam name="T">The type of item that represents a row in the database.</typeparam>
        /// <returns>Object representing the row in the database</returns>
        Task<T> GetSingleRecordWithMultipleParametersAsync<T>(T entity, string tableName, List<KeyValueModel> keyValues) where T : new();
        /// <summary>
        /// Adds a record to the database
        /// </summary>
        /// <param name="entity">Model to use to update the record</param>
        /// <param name="tableName">Name of the table to update</param>
        /// <returns>Number of rows affected in the database</returns>
        Task<int> AddAsync<T>(T entity, string tableName);

    }
}




