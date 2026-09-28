using arc.domain.Configuration.EventsConfig;
using System;
using System.Threading.Tasks;

namespace arc.app.Exports
{
    public class ExportHandler : IExportHandler
    {
        private readonly IGenericRepository _genericRepository;
        public ExportHandler(IGenericRepository genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<int> HandleAsync(string message, EventConfig eventData)
        {
            var exportType = "";
            switch (eventData.EventName)
            {
                case "whonetexport":
                    exportType = "WHONET";
                    break;
                case "dhis2export":
                    exportType = "DHIS2";
                    break;
                default:
                    break;
            }

            var dataToSave = @"{'exporttype':'" + exportType + "','issueddate':'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'}";

            _genericRepository.AddConfiguration("export");
            return await _genericRepository.AddAsync(dataToSave);
        }
    }
}
