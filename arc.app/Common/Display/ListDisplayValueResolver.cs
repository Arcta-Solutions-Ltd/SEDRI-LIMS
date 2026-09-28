using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Common.Display
{
    /// <summary>
    /// Resolves <c>listitem</c> ids to their values. This is the resolver behind a display row configured with
    /// <c>List: 'Yes'</c>, and can also be selected by name for a field that stores list ids.
    /// </summary>
    public class ListDisplayValueResolver : IdDisplayValueResolver
    {
        private readonly IListRepository _listRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListDisplayValueResolver"/> class.
        /// </summary>
        /// <param name="listRepository">Repository used to read list item values.</param>
        public ListDisplayValueResolver(IListRepository listRepository)
        {
            _listRepository = listRepository;
        }

        /// <inheritdoc />
        public override string Name => "list";

        /// <inheritdoc />
        protected override async Task<IReadOnlyDictionary<int, string>> LookupAsync(IReadOnlyList<int> ids)
        {
            var found = new Dictionary<int, string>();

            foreach (var id in ids)
            {
                found[id] = await _listRepository.GetValueFromIdAsync(id);
            }

            return found;
        }
    }
}
