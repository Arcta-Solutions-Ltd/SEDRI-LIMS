using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Utils
{
    /// <summary>
    /// Repository for reading and merging JSON more-data payloads stored per-entity.
    /// </summary>
    public interface IMoreDataRepository
    {
        /// <summary>
        /// Merges a new more-data JSON fragment with the existing stored field on a given row.
        /// When invoked inside a <see cref="System.Transactions.TransactionScope"/>, pass the caller's
        /// open connection so all operations enlist in a single local transaction.
        /// </summary>
        /// <param name="newData">The new more-data JSON fragment to merge.</param>
        /// <param name="tableName">The table containing the target row.</param>
        /// <param name="id">The primary key of the target row.</param>
        /// <param name="connect">Optional open connection; when null a new connection is opened and disposed.</param>
        /// <returns>The merged JSON object as a string.</returns>
        Task<string> CombineWithExistingFieldAsync(string newData, string tableName, string id, NpgsqlConnection connect = null);
    }
}
