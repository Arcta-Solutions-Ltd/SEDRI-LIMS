using arc.app.Coding;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Common.Display
{
    /// <summary>
    /// Resolves <c>antibiotic.id</c> values to antibiotic names.
    /// </summary>
    public class AntibioticDisplayValueResolver : IdDisplayValueResolver
    {
        private readonly IAntibioticRepository _antibioticRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AntibioticDisplayValueResolver"/> class.
        /// </summary>
        /// <param name="antibioticRepository">Repository used to read antibiotic names.</param>
        public AntibioticDisplayValueResolver(IAntibioticRepository antibioticRepository)
        {
            _antibioticRepository = antibioticRepository;
        }

        /// <inheritdoc />
        public override string Name => "antibiotic";

        /// <inheritdoc />
        protected override async Task<IReadOnlyDictionary<int, string>> LookupAsync(IReadOnlyList<int> ids)
            => await _antibioticRepository.GetAntibioticNamesByIdsAsync(ids);
    }
}
