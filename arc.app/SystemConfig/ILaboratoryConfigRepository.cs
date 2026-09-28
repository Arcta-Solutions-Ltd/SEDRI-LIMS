using arc.app.Common;
using arc.data.model.Configuration;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.SystemConfig;
public interface ILaboratoryConfigRepository : IGeneralRepository
{
    Task<List<LaboratoryConfigsDataModel>> GetLaboratoryConfigListAsync(QueryFilterConfig queryFilter);
}
