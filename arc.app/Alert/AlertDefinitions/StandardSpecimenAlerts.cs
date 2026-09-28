using arc.app.Alert.StandardSpecimenAlerts;
using arc.app.Common;
using arc.common.Models.Alert;
using arc.common.Models.Specimen;
using arc.domain.Alert;
using arc.domain.Tests;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Alert.AlertDefinitions
{
    public class StandardSpecimenAlerts : IStandardSpecimenAlerts
    {
        private readonly ISpecimenAlertRepository _specimenAlertRepository;
        private readonly ILogWriter _logWriter;

        public StandardSpecimenAlerts(ISpecimenAlertRepository specimenAlertRepository, ILogWriter logWriter)
        {
            _specimenAlertRepository = specimenAlertRepository;
            _logWriter = logWriter;
        }

        public async Task<List<SpecimenAlert>> GetAsync(List<CultureListModel> cultures, List<Test> tests, SpecimenModel specimen)
        {
            var returnList = new List<SpecimenAlert>();

            _logWriter.LogInfo($"Get standard specimen alerts for specimen : {specimen.Id}", "StandardSpecimenAlerts", "Get");
            var standardAlerts = await _specimenAlertRepository.StandardSpecimenAlertQueryAsync();

            foreach (var alert in standardAlerts)
            {
                var alertDetails = JsonConvert.DeserializeObject<StandardSpecimenAlertModel>(alert.MoreData);

                var foundAlert = false;

                var testCountCheck = (alertDetails.NoTests && tests.Count() == 0) || !alertDetails.NoTests;
                var cultureCountCheck = (alertDetails.NoCultures && cultures.Count() == 0) || !alertDetails.NoCultures;

                if (testCountCheck && cultureCountCheck)
                {
                    if (alertDetails.Type == "testinspecimentypes")
                    {
                        var alertObject = new TestInSpecimenTypesAlert();
                        foundAlert = alertObject.CheckAlertConditionsMatch(alertDetails, specimen, tests);
                    }
                    if (alertDetails.Type == "quantityinculturetypes")
                    {
                        var alertObject = new QuantityInCultureTypesAlert();
                        foundAlert = alertObject.CheckAlertConditionsMatch(alertDetails, cultures);
                    }
                }

                if (foundAlert)
                {
                    var newAlert = new SpecimenAlert { AlertId = alert.Id, SpecimenId = specimen.Id, AlertTypeId = alert.AlertTypeId, TagId = alert.TagId };
                    returnList.Add(newAlert);
                }
            }

            return returnList;
        }
    }
}
