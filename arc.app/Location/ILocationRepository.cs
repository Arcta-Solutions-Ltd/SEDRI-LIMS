using arc.domain.Configuration.ListsConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Location
{
    public interface ILocationRepository
    {
        Task<int> AddAsync(string dataToSave);
        Task<int> EditAsync(string dataToSave);
        Task<IEnumerable<OptionsConfig>> GetLocationsForListAsync();
        Task<string> GetLocationHierarchyAsync(string id);
    }
}
