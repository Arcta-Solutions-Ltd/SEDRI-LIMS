namespace arc.app.Common
{
    /// <summary>
    /// Service for computing and validating hash chain values for the Queue table.
    /// The hashing algorithm is kept in the application layer to prevent tampering via direct database queries.
    /// </summary>
    public interface IQueueHashService
    {
        /// <summary>
        /// Computes the SHA256 hash for a queue record in the chain.
        /// </summary>
        /// <param name="previousMessage">The Message column value of the previous record (by Id order). Use empty string for the first record.</param>
        /// <param name="currentMessage">The Message column value of the current record.</param>
        /// <returns>A 64-character hexadecimal hash string.</returns>
        string ComputeHash(string previousMessage, string currentMessage);

        /// <summary>
        /// Validates that the stored hash matches the expected hash for the given record.
        /// </summary>
        /// <param name="previousMessage">The Message column value of the previous record (by Id order). Use empty string for the first record.</param>
        /// <param name="currentMessage">The Message column value of the current record.</param>
        /// <param name="storedHash">The Hash value stored in the database.</param>
        /// <returns>True if the stored hash matches the computed hash; false otherwise (including when storedHash is null).</returns>
        bool ValidateHash(string previousMessage, string currentMessage, string storedHash);
    }
}
