using arc.common.Models;
using arc.common.Models.Coding;
using arc.domain.Configuration.QueryFiltersConfig;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Coding
{
    public class BreakpointHandler : IBreakpointHandler
    {
        private readonly IBreakpointRepository _breakpointRepository;
        private readonly IOrganismRepository _organismRepository;

        public BreakpointHandler(IBreakpointRepository breakpointRepository, IOrganismRepository organismRepository)
        {
            _breakpointRepository = breakpointRepository;
            _organismRepository = organismRepository;
        }

        public async Task<string> GetBreakpoint(QueryFilterConfig queryFilters)
        {
            var breakpoint = await _breakpointRepository.EditBreakpointQueryAsync(queryFilters);

            var craftedKeyValuePairs = new List<JsonKeyValuePairModel>
            {
                new JsonKeyValuePairModel { Key = "orderid", value = breakpoint.OrderId.ToString() },
                new JsonKeyValuePairModel { Key = "familyid", value = breakpoint.FamilyId.ToString() },
                new JsonKeyValuePairModel { Key = "orggroupcodingid", value = breakpoint.OrgGroupCodingId.ToString() },
            };

            var organismPairs = new List<JsonKeyValuePairModel>   
            {
                new JsonKeyValuePairModel { Key = "order", value = breakpoint.Order },
                new JsonKeyValuePairModel { Key = "family", value = breakpoint.Family },
            };

            if (breakpoint.OrganismId > 0)
            {
                var organismFilter = new QueryFilterConfig();
                organismFilter.AddInteger("id", breakpoint.OrganismId);
                var organism = await _organismRepository.GetOrganismListEntrByIdAsync(organismFilter);
                organismPairs.Add(new JsonKeyValuePairModel { Key = "organism", value = organism.Description });
                organismPairs.Add(new JsonKeyValuePairModel { Key = "organismid", value = organism.Id.ToString() });
                var hierarchy = await _organismRepository.GetOrganismHierarchyAsync(breakpoint.OrganismId);
                craftedKeyValuePairs.Add(new JsonKeyValuePairModel { Key = "genusid", value = hierarchy.GenusId.ToString() });
                craftedKeyValuePairs.Add(new JsonKeyValuePairModel { Key = "speciesid", value = hierarchy.SpeciesId.ToString() });
                craftedKeyValuePairs.Add(new JsonKeyValuePairModel { Key = "subspeciesid", value = hierarchy.SubSpeciesId.ToString() });
                craftedKeyValuePairs.Add(new JsonKeyValuePairModel { Key = "serotypeid", value = hierarchy.SerotypeId.ToString() });
            }

            if (! string.IsNullOrEmpty(breakpoint.OrganismGroup))
            {
                organismPairs.Add(new JsonKeyValuePairModel { Key = "orggroup", value = breakpoint.OrganismGroup });
            }
            organismPairs.AddRange(craftedKeyValuePairs);

            var craftedModels = new List<CraftedModel>
            {
                new CraftedModel { Name = "editorganismscopepage", Contents = JsonConvert.SerializeObject(craftedKeyValuePairs) },
                new CraftedModel { Name = "changeorganismselectorpage", Contents = JsonConvert.SerializeObject(organismPairs) }
            };

            BreakpointModel breakpointModel = new BreakpointModel
            {
                Id = breakpoint.Id,
                OrderId = breakpoint.OrderId,
                FamilyId = breakpoint.FamilyId,
                GenusId = breakpoint.GenusId,
                SpeciesId = breakpoint.SpeciesId,
                OrganismId = breakpoint.OrganismId,
                OrgGroupCodingId = breakpoint.OrgGroupCodingId,
                AntibioticId = breakpoint.AntibioticId,
                SpecimenTypeId = breakpoint.SpecimenTypeId,
                TestMethodId = breakpoint.TestMethodId,
                SpecificationId = breakpoint.SpecificationId,
                SpecialConsiderId = breakpoint.SpecialConsiderId,
                HostId = breakpoint.HostId,
                Dosage = breakpoint.Dosage,
                Enabled = breakpoint.Enabled,
                MetafCodingId = breakpoint.MetafCodingId,
                Crafted = craftedModels
            };

            breakpointModel.BreakpointGrid = new List<BreakpointLineModel>();

            foreach (var line in breakpoint.BreakpointGrid)
            {
                var breakpointLine = new BreakpointLineModel
                {
                    ResultId = line.ResultId,
                    StartVal = line.StartVal,
                    EndVal = line.EndVal
                };

                breakpointModel.BreakpointGrid.Add(breakpointLine);
            }

            return JsonConvert.SerializeObject(breakpointModel);
        }
    }
}
