using arc.app.ExpertRule;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Common.Display
{
    /// <summary>
    /// Resolves <c>expertrule.id</c> values to expert rule names.
    /// </summary>
    public class ExpertRuleDisplayValueResolver : IdDisplayValueResolver
    {
        private readonly IExpertRuleRepository _expertRuleRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpertRuleDisplayValueResolver"/> class.
        /// </summary>
        /// <param name="expertRuleRepository">Repository used to read expert rule names.</param>
        public ExpertRuleDisplayValueResolver(IExpertRuleRepository expertRuleRepository)
        {
            _expertRuleRepository = expertRuleRepository;
        }

        /// <inheritdoc />
        public override string Name => "expertrule";

        /// <inheritdoc />
        protected override async Task<IReadOnlyDictionary<int, string>> LookupAsync(IReadOnlyList<int> ids)
            => await _expertRuleRepository.GetExpertRuleNamesByIdsAsync(ids);
    }
}
