using arc.domain.Configuration.ListsConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Configuration.Queries
{
    public interface ISpecimenEventListQuery
    {
        Task<List<OptionsConfig>> GetListAsync();
    }
}
