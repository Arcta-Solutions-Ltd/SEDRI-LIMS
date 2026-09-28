using arc.domain.Alert;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Alert.AlertDefinitions
{
    public interface IDirectTestOnlyAlerts
    {
        Task<List<SpecimenAlert>> GetAsync(int specimenId);
    }
}
