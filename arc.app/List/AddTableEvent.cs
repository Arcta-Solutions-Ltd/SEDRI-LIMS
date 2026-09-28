using arc.app.Common;
using arc.common;
using arc.common.Models.Lists;
using arc.domain.Configuration.EventsConfig;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.List
{
    internal class AddTableEvent : IRun
    {
        private readonly IListRepository _listRepository;

        /// <summary>
        /// Initialises a new instance of <see cref="AddTableEvent"/>.
        /// </summary>
        /// <param name="listRepository">Repository used to persist the new list (table).</param>
        public AddTableEvent(IListRepository listRepository)
        {
            _listRepository = listRepository;
        }

        /// <summary>
        /// Handles the add-table event. Derives the list Name from the Description value
        /// entered by the user using <see cref="FormatNameFromDescription"/>, then persists
        /// the new list via the repository.
        /// </summary>
        /// <param name="dataToSave">JSON payload from the form submission, deserialised as a <see cref="TableModel"/>.</param>
        /// <param name="id">Not used for add operations; pass empty string or "0".</param>
        /// <param name="command">Metadata about the triggering event.</param>
        /// <param name="eventData">Optional event configuration context; defaults to <c>null</c>.</param>
        /// <returns>The Id of the newly created or revived list.</returns>
        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var sourceData = JsonConvert.DeserializeObject<TableModel>(dataToSave);

            var tableModel = new AddTableModel
            {
                Name = FormatNameFromDescription(sourceData.Description),
                Description = sourceData.Description,
                ParentId = string.IsNullOrWhiteSpace(sourceData.ParentId) ? 0 : int.Parse(sourceData.ParentId)
            };

            var data = JsonConvert.SerializeObject(tableModel);

            return await _listRepository.AddTableAsync(data);
        }

        /// <summary>
        /// Formats a user-entered description into the normalised Name stored in the database,
        /// applying the same rules used when the Name field was entered directly: leading and
        /// trailing whitespace is removed.
        /// </summary>
        /// <param name="description">The raw description value entered by the user.</param>
        /// <returns>The trimmed string to be stored as the list Name, or an empty string if <paramref name="description"/> is <c>null</c>.</returns>
        private static string FormatNameFromDescription(string description)
            => description?.Trim() ?? string.Empty;
    }
}
