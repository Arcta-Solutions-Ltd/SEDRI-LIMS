namespace arc.app.Common.Display
{
    /// <summary>
    /// Looks up a registered <see cref="IDisplayValueResolver"/> by the name an event's display row asks for.
    /// </summary>
    public interface IDisplayValueResolverFactory
    {
        /// <summary>
        /// Returns the resolver registered under a name.
        /// </summary>
        /// <param name="name">Resolver name from the display configuration.</param>
        /// <returns>The matching resolver, or null when the name is empty or unregistered.</returns>
        IDisplayValueResolver Get(string name);
    }
}
