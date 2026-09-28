using arc.app.Common;
using arc.common.Models.Specimen;
using arc.domain.Alert;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Alert.AlertDefinitions
{
    public class OrganismExistsAlerts : IOrganismExistsAlerts
    {
        private readonly ISpecimenAlertRepository _specimenAlertRepository;
        private readonly ILogWriter _logWriter;

        public OrganismExistsAlerts(ISpecimenAlertRepository specimenAlertRepository, ILogWriter logWriter)
        {
            _specimenAlertRepository = specimenAlertRepository;
            _logWriter = logWriter;
        }

        public async Task<List<SpecimenAlert>> GetAsync(List<CultureListModel> cultures, int specimenId)
        {
            var returnList = new List<SpecimenAlert>();

            foreach (var culture in cultures)
            {
                if (culture.SpecimenOrganismId != 0)
                {
                    _logWriter.LogInfo($"Get organism exists alerts for organism : {culture.SpecimenOrganismId}", "AlertHandler", "RaiseAlert");
                    var parameters = new QueryFilterConfig { Parameters = new List<QueryValuesConfig> { new QueryValuesConfig { Key = "OrganismId", Value = culture.SpecimenOrganismId.ToString() } } };
                    var organismAlerts = await _specimenAlertRepository.OnlyOrganismExistsAlertQueryAsync(parameters);

                    foreach (var alert in organismAlerts)
                    {
                        var cultureAlerts = new List<CultureAlert> { new CultureAlert { CultureId = int.Parse(culture.Id) } };
                        var newAlert = new SpecimenAlert { AlertId = alert.Id, SpecimenId = specimenId, AlertTypeId = alert.AlertTypeId, TagId = alert.TagId, CultureId = int.Parse(culture.Id), CultureAlerts = cultureAlerts };
                        returnList.Add(newAlert);
                    }
                }
            }

            return returnList;
        }
    }
}
