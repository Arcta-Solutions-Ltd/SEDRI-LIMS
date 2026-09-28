using arc.app.AST;
using arc.app.Common;
using arc.common.Models.Specimen;
using arc.domain.Alert;
using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Alert.AlertDefinitions
{
    public class OrganismASTTestAlerts : IOrganismASTTestAlerts
    {
        private readonly ISpecimenAlertRepository _specimenAlertRepository;
        private readonly IAlertProcessor _alertProcessor;
        private readonly IASTRepository _astRepository;
        private readonly ILogWriter _logWriter;

        public OrganismASTTestAlerts(ISpecimenAlertRepository specimenAlertRepository, IAlertProcessor alertProcessor, IASTRepository astRepository, ILogWriter logWriter)
        {
            _specimenAlertRepository = specimenAlertRepository;
            _alertProcessor = alertProcessor;
            _astRepository = astRepository;
            _logWriter = logWriter;
        }

        public async Task<List<SpecimenAlert>> GetAsync(List<CultureListModel> cultures, int specimenId, List<OptionsConfig> antibioticList)
        {
            var returnList = new List<SpecimenAlert>();

            foreach (var culture in cultures)
            {
                if (culture.SpecimenOrganismId != 0)
                {
                    _logWriter.LogInfo($"Get AST Test alerts for organism : {culture.SpecimenOrganismId}", "AlertHandler", "RaiseAlert");
                    var parameters = new QueryFilterConfig { Parameters = new List<QueryValuesConfig> { new QueryValuesConfig { Key = "OrganismId", Value = culture.SpecimenOrganismId.ToString() } } };

                    var allAlerts = await _specimenAlertRepository.OrganismASTTestAlertQueryAsync(parameters);

                    if (allAlerts.Count() > 0)
                    {
                        var astTestList = (await _astRepository.GetASTTestResultsAsync(culture.Id)).ToList();
                        foreach (var astRow in astTestList)
                        {
                            var antibioticOption = antibioticList.Where((o) => o.Key == astRow.AntibioticId.ToString());
                            astRow.AntibioticGroupId = int.Parse(antibioticOption.First().ParentKey);
                        }

                        var astAlerts = _alertProcessor.ApplyASTAlerts(allAlerts, astTestList, specimenId, int.Parse(culture.Id));

                        foreach (var testAlert in astAlerts)
                        {
                            var match = astAlerts.Where((t) => t.AlertId == testAlert.AlertId);
                            if (match.Count() > 0)
                            {
                                var cultureAlerts = testAlert.CultureAlerts;
                                if (cultureAlerts.Count > 0)
                                {
                                    testAlert.CultureAlerts.First().ASTTestAlerts = match.First().CultureAlerts.First().ASTTestAlerts;
                                }
                                returnList.Add(testAlert);
                            }
                        }

                    }
                }
            }

            return returnList;
        }
    }
}
