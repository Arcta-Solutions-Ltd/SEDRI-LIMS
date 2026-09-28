using arc.app.Coding;
using arc.domain.Configuration.ListsConfig;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Common.Display
{
    /// <summary>
    /// Resolves <c>testpattern.id</c> values to test pattern names.
    /// </summary>
    public class TestPatternDisplayValueResolver : IdDisplayValueResolver
    {
        private readonly ITestPatternRepository _testPatternRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestPatternDisplayValueResolver"/> class.
        /// </summary>
        /// <param name="testPatternRepository">Repository used to read test pattern names.</param>
        public TestPatternDisplayValueResolver(ITestPatternRepository testPatternRepository)
        {
            _testPatternRepository = testPatternRepository;
        }

        /// <inheritdoc />
        public override string Name => "testpattern";

        /// <inheritdoc />
        protected override async Task<IReadOnlyDictionary<int, string>> LookupAsync(IReadOnlyList<int> ids)
        {
            var names = await _testPatternRepository.GetTestPatternNamesListAsync();
            var found = new Dictionary<int, string>();

            foreach (var option in names ?? Enumerable.Empty<OptionsConfig>())
            {
                if (int.TryParse(option.Key, out var id) && ids.Contains(id) && !found.ContainsKey(id))
                {
                    found[id] = option.Text;
                }
            }

            return found;
        }
    }
}
