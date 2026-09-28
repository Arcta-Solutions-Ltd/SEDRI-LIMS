namespace arc.app.Common
{
    /// <summary>
    /// Implemented by an event that produces a richer result than the row count
    /// <see cref="IRun.RunAsync"/> can return, so the controller can send that result to the client.
    /// </summary>
    /// <typeparam name="T">The type of the result produced by the event.</typeparam>
    public interface IProvideSaveResult<out T>
    {
        /// <summary>
        /// Gets the result of the most recent run, or the default value when the event has not run.
        /// </summary>
        T SaveResult { get; }
    }
}
