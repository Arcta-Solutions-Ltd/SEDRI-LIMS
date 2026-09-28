using arc.app.Tests;
using arc.common.Models.Specimen;
using arc.domain.Alert;
using arc.domain.Tests;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Alert.AlertDefinitions;

public class DirectTestAndCultureTestAlerts : IDirectTestAndCultureTestAlerts
{
    private readonly ISpecimenAlertRepository _specimenAlertRepository;
    private readonly ITestRepository _testRepository;
    private readonly IAlertProcessor _alertProcessor;

    public DirectTestAndCultureTestAlerts(ISpecimenAlertRepository specimenAlertRepository, ITestRepository testRepository, IAlertProcessor alertProcessor)
    {
        _specimenAlertRepository = specimenAlertRepository;
        _testRepository = testRepository;
        _alertProcessor = alertProcessor;
    }

    public async Task<List<SpecimenAlert>> GetAsync(List<CultureListModel> cultures, int specimenId)
    {
        var returnList = new List<SpecimenAlert>();

        var directTestAlerts = await _specimenAlertRepository.DirectTestAlertsQueryAsync(null);

        if (directTestAlerts.Count() > 0)
        {
            var directTestList = await _testRepository.GetTestsForSpecimenAsync(specimenId);

            var cultureTestList = new List<Test>();
            foreach (var culture in cultures)
            {
                var testList = await _testRepository.GetTestsForCultureAsync(culture.Id);
                cultureTestList.AddRange(testList);
            }
            var newAlerts = _alertProcessor.ProcessTestList(directTestList.ToList(), cultureTestList.ToList(), directTestAlerts, specimenId);
            returnList.AddRange(newAlerts);
        }

        return returnList;
    }
}
