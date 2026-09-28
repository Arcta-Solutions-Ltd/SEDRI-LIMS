using arc.app.Tests;
using arc.domain.Alert;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Tests;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Alert.AlertDefinitions;

public class DirectTestOnlyAlerts : IDirectTestOnlyAlerts
{
    private readonly ISpecimenAlertRepository _specimenAlertRepository;
    private readonly ITestRepository _testRepository;
    private readonly IAlertProcessor _alertProcessor;

    public DirectTestOnlyAlerts(ISpecimenAlertRepository specimenAlertRepository, ITestRepository testRepository, IAlertProcessor alertProcessor)
    {
        _specimenAlertRepository = specimenAlertRepository;
        _testRepository = testRepository;
        _alertProcessor = alertProcessor;
    }

    public async Task<List<SpecimenAlert>> GetAsync(int specimenId)
    {
        var returnList = new List<SpecimenAlert>();

        var directTestAlerts = await _specimenAlertRepository.DirectTestAlertsQueryAsync(new QueryFilterConfig().AddString("OrganismId", "0"));
        if (directTestAlerts.Count > 0)
        {
            var testList = await _testRepository.GetTestsForSpecimenAsync(specimenId);

            if (testList.Any())
            {
                returnList = _alertProcessor.ProcessTestList(testList.ToList(), new List<Test>(), directTestAlerts, specimenId);
            }
        }

        return returnList;
    }

}
