using arc.common.Models;
using arc.common.Models.Coding;
using arc.domain.Configuration.QueryFiltersConfig;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Coding
{
    public class TestPatternHandler : ITestPatternHandler
    {
        private readonly ITestPatternRepository _testPatternRepository;
        private readonly IOrganismRepository _organismRepository;

        public TestPatternHandler(ITestPatternRepository testPatternRepository, IOrganismRepository organismRepository)
        {
            _testPatternRepository = testPatternRepository;
            _organismRepository = organismRepository;
        }

        public async Task<string> GetTestPatternAsync(QueryFilterConfig queryFilters)
        {
            var testPattern = await _testPatternRepository.EditTestPatternQueryAsync(queryFilters);

            var craftedKeyValuePairs = new List<JsonKeyValuePairModel>
            {
                new JsonKeyValuePairModel { Key = "orderid", value = testPattern.OrderId.ToString() },
                new JsonKeyValuePairModel { Key = "familyid", value = testPattern.FamilyId.ToString() },
                new JsonKeyValuePairModel { Key = "orggroupcodingid", value = testPattern.OrgGroupCodingId.ToString() }
            };

            var organismPairs = new List<JsonKeyValuePairModel>
            {
                new JsonKeyValuePairModel { Key = "order", value = testPattern.Order },
                new JsonKeyValuePairModel { Key = "family", value = testPattern.Family },
            };

            if (testPattern.OrganismId > 0)
            {
                var organismFilter = new QueryFilterConfig();
                organismFilter.AddInteger("id", testPattern.OrganismId);
                var organism = await _organismRepository.GetOrganismListEntrByIdAsync(organismFilter);
                organismPairs.Add(new JsonKeyValuePairModel { Key = "organism", value = organism.Description });
                organismPairs.Add(new JsonKeyValuePairModel { Key = "organismid", value = organism.Id.ToString() });
                var hierarchy = await _organismRepository.GetOrganismHierarchyAsync(testPattern.OrganismId);
                craftedKeyValuePairs.Add(new JsonKeyValuePairModel { Key = "genusid", value = hierarchy.GenusId.ToString() });
                craftedKeyValuePairs.Add(new JsonKeyValuePairModel { Key = "speciesid", value = hierarchy.SpeciesId.ToString() });
                craftedKeyValuePairs.Add(new JsonKeyValuePairModel { Key = "subspeciesid", value = hierarchy.SubSpeciesId.ToString() });
                craftedKeyValuePairs.Add(new JsonKeyValuePairModel { Key = "serotypeid", value = hierarchy.SerotypeId.ToString() });
            }

            if (!string.IsNullOrEmpty(testPattern.OrganismGroup))
            {
                organismPairs.Add(new JsonKeyValuePairModel { Key = "orggroup", value = testPattern.OrganismGroup });
            }
            organismPairs.AddRange(craftedKeyValuePairs);

            var craftedModels = new List<CraftedModel>
            {
                new CraftedModel { Name = "editorganismscopepage", Contents = JsonConvert.SerializeObject(craftedKeyValuePairs) },
                new CraftedModel { Name = "changeorganismselectorpage", Contents = JsonConvert.SerializeObject(organismPairs) }
            };

            TestPatternModel testPatternModel = new TestPatternModel
            {
                Id = testPattern.Id,
                OrderId = testPattern.OrderId,
                FamilyId = testPattern.FamilyId,
                GenusId = testPattern.GenusId,
                SpeciesId = testPattern.SpeciesId,
                OrganismId = testPattern.OrganismId,
                OrgGroupCodingId = testPattern.OrgGroupCodingId,
                TestPatternName = testPattern.TestPatternName,
                MetafCodingId = testPattern.MetafCodingId,
                HostId = testPattern.HostId,
                MakeDefault = testPattern.MakeDefault,
                Crafted = craftedModels,
                SpecimenTypeId = testPattern.SpecimenTypeId
            };

            testPatternModel.AntibioticGrid = new List<TestPatternLineModel>();

            foreach (var line in testPattern.AntibioticGrid)
            {
                var testPatternLine = new TestPatternLineModel
                {
                    Id = line.Id,
                    testOrder = line.testOrder,
                    AntibioticId = line.AntibioticId,
                    Dosage = line.Dosage,
                    TestMethodId = line.TestMethodId,
                    GuidelinesId = line.GuidelinesId,
                    CategoryId = line.CategoryId,
                    PrintOnReport = line.PrintOnReport ? "Yes" : "No"
                };

                testPatternModel.AntibioticGrid.Add(testPatternLine);
            }

            return JsonConvert.SerializeObject(testPatternModel);
        }
    }
}
