using arc.app.Security;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Common.Display
{
    /// <summary>
    /// Resolves <c>organisation.id</c> values to organisation names. Applied automatically to a field named
    /// <c>organisationid</c> so configurations written before resolvers existed keep working.
    /// </summary>
    public class OrganisationDisplayValueResolver : IdDisplayValueResolver
    {
        private readonly IOrganisationRepository _organisationRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrganisationDisplayValueResolver"/> class.
        /// </summary>
        /// <param name="organisationRepository">Repository used to read organisation names.</param>
        public OrganisationDisplayValueResolver(IOrganisationRepository organisationRepository)
        {
            _organisationRepository = organisationRepository;
        }

        /// <inheritdoc />
        public override string Name => "organisation";

        /// <inheritdoc />
        protected override async Task<IReadOnlyDictionary<int, string>> LookupAsync(IReadOnlyList<int> ids)
        {
            var found = new Dictionary<int, string>();

            foreach (var id in ids)
            {
                var organisation = await _organisationRepository.GetOrganisationsForNameByIdAsync(id);
                found[id] = organisation?.Text;
            }

            return found;
        }
    }
}
