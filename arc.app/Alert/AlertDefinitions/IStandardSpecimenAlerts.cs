using arc.common.Models.Specimen;
using arc.domain.Alert;
using arc.domain.Tests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Alert.AlertDefinitions
{
    public interface IStandardSpecimenAlerts
    {
        Task<List<SpecimenAlert>> GetAsync(List<CultureListModel> cultures, List<Test> tests, SpecimenModel specimen);
    }
}
