using System;

namespace arc.domain.Instruments
{
    /// <summary>
    /// Thrown by the Custom-interface import when a business rule prevents a record from being saved (for example, a new
    /// specimen whose patient does not exist and whose profile does not create patients). Carries a language
    /// <see cref="ErrorTag"/> so the caller can raise a meaningful instrument error. Because the whole record is saved in
    /// a single transaction, throwing this rolls back any partial writes.
    /// </summary>
    public class CustomImportException : Exception
    {
        /// <summary>Language tag describing the failure (e.g. <c>@InsCusPat@</c>).</summary>
        public string ErrorTag { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomImportException"/> class.
        /// </summary>
        /// <param name="errorTag">Language tag describing the failure.</param>
        /// <param name="message">Human-readable description of the failure.</param>
        public CustomImportException(string errorTag, string message) : base(message)
        {
            ErrorTag = errorTag;
        }
    }
}
