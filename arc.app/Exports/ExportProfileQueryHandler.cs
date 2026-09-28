using arc.domain.Configuration.QueryFiltersConfig;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Exports
{
    public class ExportProfileQueryHandler : IExportProfileQueryHandler
    {
        private readonly IExportProfileFieldRepository _exportProfileFieldRepository;

        public ExportProfileQueryHandler(IExportProfileFieldRepository exportProfileFieldRepository)
        {
            _exportProfileFieldRepository = exportProfileFieldRepository;
        }
        public async Task<string> GetProfileFieldsForEditAsync(QueryFilterConfig queryFilters)
        {
            var fields = await _exportProfileFieldRepository.GetByProfileIdAsync(queryFilters);

            var selectorValues = fields.Select(f => new { id = f.HeaderName, label = f.HeaderName, value = f.FieldName });

            var result = new { Id = fields.First().ExportProfileId, FieldList = selectorValues };

            return JsonConvert.SerializeObject(result);
        }
    }
}
