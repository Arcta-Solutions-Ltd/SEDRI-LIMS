using arc.common.Models.Specimen;
using arc.domain.Alert;
using arc.domain.Configuration.ListsConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Alert.AlertDefinitions
{
    public interface IOrganismASTTestAlerts
    {
        Task<List<SpecimenAlert>> GetAsync(List<CultureListModel> cultures, int specimenId, List<OptionsConfig> antibioticList);
    }
}
