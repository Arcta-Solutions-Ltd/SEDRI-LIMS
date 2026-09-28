using arc.app.Specification;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Common.Display
{
    /// <summary>
    /// Resolves <c>breakpoint.id</c> values to the specification the breakpoint came from, using the same
    /// composite text as the specification dropdown (<c>Guidelines - Document (Version, Year)</c>). A stored id
    /// of zero means no breakpoint was applied and resolves to an empty string.
    /// </summary>
    public class BreakpointSpecificationDisplayValueResolver : IdDisplayValueResolver
    {
        private readonly ISpecificationRepository _specificationRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="BreakpointSpecificationDisplayValueResolver"/> class.
        /// </summary>
        /// <param name="specificationRepository">Repository used to read specification display text.</param>
        public BreakpointSpecificationDisplayValueResolver(ISpecificationRepository specificationRepository)
        {
            _specificationRepository = specificationRepository;
        }

        /// <inheritdoc />
        public override string Name => "breakpointspecification";

        /// <inheritdoc />
        protected override async Task<IReadOnlyDictionary<int, string>> LookupAsync(IReadOnlyList<int> ids)
            => await _specificationRepository.GetSpecificationTextByBreakpointIdsAsync(ids);
    }
}
