using arc.app.Common;
using arc.app.SystemConfig;
using arc.data.Common;
using arc.data.model.Configuration;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.data.SystemConfig;
public class LaboratoryConfigRepository(ISqlCommand sqlCommand, ISqlQuery sqlQuery, ILogWriter logWriter) : GeneralRepository(sqlQuery, logWriter, sqlCommand), ILaboratoryConfigRepository
{
    public async Task<List<LaboratoryConfigsDataModel>> GetLaboratoryConfigListAsync(QueryFilterConfig queryFilter)
    {
        _logWriter.LogInfo("Run laboratory config query", "LaboratoryConfigRepository", "GetLaboratoryConfigListAsync");
        return await _sqlQuery.QueryReturningTypeAsync(new GetLaboratoryConfigListQuery(), "Get Laboratory Config list", queryFilter);
    }
}
