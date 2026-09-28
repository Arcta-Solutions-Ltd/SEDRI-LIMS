using arc.app.Common;
using arc.app.Tests;
using arc.common.Models.Specimen;
using arc.domain.Alert;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Alert.AlertDefinitions;

public class OrganismTestAlerts : IOrganismTestAlerts
{
    private readonly ISpecimenAlertRepository _specimenAlertRepository;
    private readonly ITestRepository _testRepository;
    private readonly IAlertProcessor _alertProcessor;
    private readonly ILogWriter _logWriter;

    public OrganismTestAlerts(ISpecimenAlertRepository specimenAlertRepository, ITestRepository testRepository, IAlertProcessor alertProcessor, ILogWriter logWriter)
    {
        _specimenAlertRepository = specimenAlertRepository;
        _testRepository = testRepository;
        _alertProcessor = alertProcessor;
        _logWriter = logWriter;
    }

    public async Task<List<SpecimenAlert>> GetAsync(List<CultureListModel> cultures, int specimenId)
    {
        var returnList = new List<SpecimenAlert>();

        foreach (var culture in cultures)
        {
            if (culture.SpecimenOrganismId > 0)
            {
                _logWriter.LogInfo($"Get Organism + Direct Test alerts for organism : {culture.SpecimenOrganismId}", "AlertHandler", "RaiseAlert");
                var parameters = new QueryFilterConfig { Parameters = new List<QueryValuesConfig> { new() { Key = "OrganismId", Value = culture.SpecimenOrganismId.ToString() } } };

                var allAlerts = await _specimenAlertRepository.OrganismTestsAlertQueryAsync(parameters);

                if (allAlerts.Count > 0)
                {
                    var directTestList = await _testRepository.GetTestsForSpecimenAsync(specimenId);
                    var cultureTestList = await _testRepository.GetTestsForCultureAsync(culture.Id);

                    directTestList.ToList().AddRange(cultureTestList);

                    var testAlerts = _alertProcessor.ProcessTestList(directTestList.ToList(), cultureTestList.ToList(), allAlerts, specimenId);

                    returnList.AddRange(testAlerts);
                }
            }
        }

        return returnList;
    }
}
