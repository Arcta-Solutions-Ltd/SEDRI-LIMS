using arc.app.Coding;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Common.Display
{
    /// <summary>
    /// Resolves <c>organism.id</c> values to organism descriptions. Applied automatically to a field named
    /// <c>organismid</c> so configurations written before resolvers existed keep working.
    /// </summary>
    public class OrganismDisplayValueResolver : IdDisplayValueResolver
    {
        private readonly IOrganismRepository _organismRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrganismDisplayValueResolver"/> class.
        /// </summary>
        /// <param name="organismRepository">Repository used to read organism descriptions.</param>
        public OrganismDisplayValueResolver(IOrganismRepository organismRepository)
        {
            _organismRepository = organismRepository;
        }

        /// <inheritdoc />
        public override string Name => "organism";

        /// <inheritdoc />
        protected override async Task<IReadOnlyDictionary<int, string>> LookupAsync(IReadOnlyList<int> ids)
        {
            var found = new Dictionary<int, string>();

            foreach (var id in ids)
            {
                var queryFilter = new QueryFilterConfig().AddString("id", id.ToString());
                var organism = await _organismRepository.OrganismByIdQueryAsync(queryFilter);
                found[id] = organism?.Description;
            }

            return found;
        }
    }
}
