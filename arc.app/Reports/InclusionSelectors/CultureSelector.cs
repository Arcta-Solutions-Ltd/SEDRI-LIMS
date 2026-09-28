using arc.app.Specimen;
using arc.common.Models.Reports;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Reports.InclusionSelectors
{
    /// <summary>
    /// Report selector for cultures.
    /// </summary>
    public class CultureSelector : ICultureSelector
    {
        private readonly ICultureRepository _cultureRepository;

        /// <summary>
        /// Initializes a new instance of the CultureSelector class.
        /// </summary>
        /// <param name="cultureRepository">The culture repository instance.</param>
        public CultureSelector(ICultureRepository cultureRepository)
        {
            _cultureRepository = cultureRepository;
        }

        /// <summary>
        /// Asynchronously gets the contents for the culture selector.
        /// </summary>
        /// <param name="specimenId">The ID of the specimen.</param>
        /// <returns>A list of CultureSelectorListModel objects.</returns>
        public async Task<List<CultureSelectorListModel>> GetContentsAsync(int specimenId)
        {
            var queryFilters = new QueryFilterConfig { Parameters = new List<QueryValuesConfig> { new QueryValuesConfig { Key = "specimenid", Value = specimenId.ToString() } } };
            var cultures = await _cultureRepository.GetCultureListBySpecimenIdAsync(queryFilters);

            var returnList = new List<CultureSelectorListModel>();
            foreach (var culture in cultures)
            {
                var connector = string.IsNullOrWhiteSpace(culture.SpecimenOrganism) ? "" : " - ";
                var newModel = new CultureSelectorListModel
                {
                    Id = culture.Id,
                    Name = culture.Type + connector + culture.SpecimenOrganism,
                    PrintOnReport = culture.DisplayOnReport,
                };
                returnList.Add(newModel);
            }

            return returnList;
        }
    }
}
