using arc.app.Tests;
using arc.common.Models.Alert;
using arc.common.Models.Specimen;
using arc.common.Utils;
using arc.domain.Alert;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Tests;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Alert.AlertDefinitions
{
    public class CultureTestOnlyAlerts : ICultureTestOnlyAlerts
    {
        private readonly ISpecimenAlertRepository _specimenAlertRepository;
        private readonly ITestRepository _testRepository;
        private readonly IAlertProcessor _alertProcessor;

        public CultureTestOnlyAlerts(ISpecimenAlertRepository specimenAlertRepository, ITestRepository testRepository, IConvertJsonStructureToKeyValuePair pairConverter, IAlertProcessor alertProcessor)
        {
            _specimenAlertRepository = specimenAlertRepository;
            _testRepository = testRepository;
            _alertProcessor = alertProcessor;
        }

        public async Task<List<SpecimenAlert>> GetAsync(List<CultureListModel> cultures, int specimenId)
        {
            var returnList = new List<SpecimenAlert>();

            var parameters = new QueryFilterConfig { Parameters = new List<QueryValuesConfig> { new QueryValuesConfig { Key = "OrganismId", Value = "0" } } };
            var directTestAlerts = await _specimenAlertRepository.DirectTestAlertsQueryAsync(parameters);

            if (directTestAlerts.Count() > 0)
            {
                var cultureTestList = new List<Test>();
                foreach (var culture in cultures)
                {
                    var testList = await _testRepository.GetTestsForCultureAsync(culture.Id);
                    cultureTestList.AddRange(testList);
                }
                var newAlerts = _alertProcessor.ProcessTestList(new List<Test>(), cultureTestList.ToList(), directTestAlerts, specimenId);
                returnList.AddRange(newAlerts);
            }

            return returnList;
        }

    }
}
