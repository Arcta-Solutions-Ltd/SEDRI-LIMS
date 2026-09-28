using arc.common.Models;
using arc.common.Models.QualityAssurance;
using arc.common.Models.Role;
using arc.domain.Configuration.QueryFiltersConfig;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Quality
{
    public class IqcTestProfileQueryHandler : IIqcTestProfileQueryHandler
    {
        private readonly IQualityRepository _qualityRepository;

        public IqcTestProfileQueryHandler(IQualityRepository qualityRepository)
        {
            _qualityRepository = qualityRepository;
        }

        public async Task<string> GetProfileAsync(QueryFilterConfig queryFilter)
        {
            var antibioticList = await _qualityRepository.GetQcAntibioticsForIqcTestProfileQcOrganismAsync(queryFilter);
            var useByDefault = await _qualityRepository.IsIqcTestProfileOrganismUsedByDefaultAsync(queryFilter);

            var antibitoicListToReturn = antibioticList.Select(t => new CraftedSelectionsModel { Name = t.AntibioticName, Key = t.Id.ToString(), Allowed = t.Enabled ? "Yes" : "No" }).ToList();

            return JsonConvert.SerializeObject(CreateReturnModel(antibitoicListToReturn, "editiqctestprofileantibioticspage", useByDefault ? "Yes" : "No"));
        }

        // TODO: Review and move about default parameters
        private JustCraftedPages CreateReturnModel(List<CraftedSelectionsModel> antibioticList, string queryName, string useByDefault = "No value", int id = 0)
        {
            var craftedModels = new List<CraftedModel>
            {
                new CraftedModel { Name = queryName, Contents = JsonConvert.SerializeObject(antibioticList.OrderBy(o => o.Name)) }
            };
            return new EditIqcTestProfileQcOrganismModel { Crafted = craftedModels, Default = useByDefault, Id = id };
        }

        public async Task<string> GetQcOrganismsProfileAsync(QueryFilterConfig queryFilter)
        {
            var qcOrganisms = await _qualityRepository.GetIqcTestProfileListQueryAsync(queryFilter);

            var displayList = qcOrganisms.Where(x => x.Enabled == "@GenYesA@").OrderByDescending(y => y.UseByDefault).ThenBy(z => z.Organism);
            var organismListToReturn = displayList.Select(t => new CraftedSelectionsModel { Name = FormatName(t.Organism, t.StandardsBody, t.PrimaryStrain), Key = t.Id.ToString(), Allowed = t.UseByDefault }).ToList();

            return JsonConvert.SerializeObject(CreateReturnModel(organismListToReturn, "selectqcorganismspage"));
        }

        public async Task<string> GetQcOrganismsForEditIqcTestQcOrganismsPageAsync(QueryFilterConfig queryFilter)
        {
            var qcOrganisms = await _qualityRepository.GetQcOrganismsForIqcTestAsync(queryFilter);
            var organismListToReturn = qcOrganisms.Select(t => new CraftedSelectionsModel { Name = FormatName(t.Organism, t.StandardsBody, t.PrimaryStrain), Key = t.Id.ToString(), Allowed = t.PresentInTest ? "Yes" : "No" }).ToList();
            var iqcTestId = int.Parse(queryFilter.Parameters.Where(p => p.Key.ToLower() == "id").First().Value);
            return JsonConvert.SerializeObject(CreateReturnModel(organismListToReturn, "editiqctestqcorganismspage", "No Value", iqcTestId));
        }

        private string FormatName(string organism, string standardsBody, string primaryStrain)
        {
            return $"{organism} ({standardsBody} {primaryStrain})";
        }
    }
}
