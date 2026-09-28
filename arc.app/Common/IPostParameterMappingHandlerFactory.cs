namespace arc.app.Common
{
    /// <summary>
    /// Factory for obtaining post-parameter-mapping handlers by query name.
    /// </summary>
    public interface IPostParameterMappingHandlerFactory
    {
        /// <summary>
        /// Gets a handler for the specified query name, or null if none is registered.
        /// </summary>
        /// <param name="queryName">The name of the query.</param>
        /// <returns>An optional handler, or null.</returns>
        IPostParameterMappingHandler GetHandler(string queryName);
    }
}
