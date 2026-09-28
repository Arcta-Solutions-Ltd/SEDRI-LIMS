using arc.common.Models.Specimen;
using arc.domain.Alert;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Alert.AlertDefinitions
{
    public interface IOrganismExistsAlerts
    {
        Task<List<SpecimenAlert>> GetAsync(List<CultureListModel> cultures, int specimenId);
    }
}
