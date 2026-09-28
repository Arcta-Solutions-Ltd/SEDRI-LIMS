using System.Threading.Tasks;

namespace arc.app.Common.Display
{
    /// <summary>
    /// Turns a stored identifier into the text a user should see. Resolvers always take an id rather than a
    /// display value, so a diary entry still resolves correctly after tags or list items have been translated
    /// into another language.
    /// </summary>
    public interface IDisplayValueResolver
    {
        /// <summary>
        /// Name this resolver is selected by, matched case-insensitively against
        /// <see cref="arc.domain.Configuration.EventsConfig.DisplayConfig.Resolver"/>.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Resolves a stored value to display text.
        /// </summary>
        /// <param name="rawValue">Stored value, normally an id or a comma separated list of ids.</param>
        /// <returns>The display text, or an empty string when the value is absent or resolves to nothing.</returns>
        Task<string> ResolveAsync(string rawValue);
    }
}
