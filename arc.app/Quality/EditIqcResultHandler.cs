using arc.common.Models;
using arc.domain.Configuration.QueryFiltersConfig;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Quality
{
    public class EditIqcResultHandler : IEditIqcResultHandler
    {
        private readonly IQualityRepository _qualityAssuranceRepository;

        public EditIqcResultHandler(IQualityRepository qualityAssuranceRepository)
        {
            _qualityAssuranceRepository = qualityAssuranceRepository;
        }

        public async Task<JustCraftedPages> GetInitialDataAsync(QueryFilterConfig queryFilters)
        {
            var result = await _qualityAssuranceRepository.EditIqcResultQueryAsync(queryFilters);
            var resultAsArray = new List<object> { new { Key = "Id", value = result.Id }, new { Key = "QcOrganismsWithIqcResults", value = result.QcOrganismWithIqcResults } };

            var resultAsString = JsonConvert.SerializeObject(resultAsArray);
            return new JustCraftedPages { Crafted = new List<CraftedModel> { new CraftedModel { Name = "editiqcresultpage", Contents = resultAsString } } };
        }
    }
}
