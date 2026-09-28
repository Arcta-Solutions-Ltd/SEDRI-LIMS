using arc.app.Common;
using arc.common.Models.Alert;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Alert
{
    public class AlertMessageHandler : IAlertMessageHandler
    {
        private readonly ISpecimenAlertRepository _specimenAlertRepository;
        private readonly ILogWriter _logWriter;

        public AlertMessageHandler(ISpecimenAlertRepository specimenAlertRepository, ILogWriter logWriter)
        {
            _specimenAlertRepository = specimenAlertRepository;
            _logWriter = logWriter;
        }

        public async Task<List<AlertMessageModel>> GetMessagesAsync(QueryFilterConfig parameters)
        {
            var viewName = parameters.Parameters.Where(p => p.Key.ToLower() == "view").First();

            if (viewName.Value.ToLower() == "specimenrecordview")
            {
                _logWriter.LogInfo("Get Specimen alert messages", "AlertMessageHandler", "GetMessages");
                return await _specimenAlertRepository.SpecimenMessageListQueryAsync(parameters);
            } else
            {
                _logWriter.LogInfo("Get Culture alert messages", "AlertMessageHandler", "GetMessages");
                return await _specimenAlertRepository.CultureMessageListQueryAsync(parameters);
            }
        }
    }
}
