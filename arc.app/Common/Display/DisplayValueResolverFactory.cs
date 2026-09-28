using arc.common.ExtensionMethods;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace arc.app.Common.Display
{
    /// <inheritdoc />
    public class DisplayValueResolverFactory : IDisplayValueResolverFactory
    {
        private readonly List<IDisplayValueResolver> _resolvers;
        private readonly ILogger<DisplayValueResolverFactory> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DisplayValueResolverFactory"/> class.
        /// </summary>
        /// <param name="resolvers">Every resolver registered with the container.</param>
        /// <param name="logger">Logger used to report display rows asking for a resolver that is not registered.</param>
        public DisplayValueResolverFactory(IEnumerable<IDisplayValueResolver> resolvers, ILogger<DisplayValueResolverFactory> logger)
        {
            _resolvers = resolvers?.ToList() ?? new List<IDisplayValueResolver>();
            _logger = logger;
        }

        /// <inheritdoc />
        public IDisplayValueResolver Get(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            var resolver = _resolvers.FirstOrDefault(r => r.Name.IsSameAs(name.Trim()));

            if (resolver == null)
            {
                _logger.LogWarning(
                    "Display resolver '{ResolverName}' is not registered, so the stored id will be shown as-is. Registered resolvers: {RegisteredResolvers}",
                    name,
                    string.Join(", ", _resolvers.Select(r => r.Name)));
            }

            return resolver;
        }
    }
}
