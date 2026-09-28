using arc.app.Config.Barcode;
using arc.app.SystemConfig;
using arc.common.Models.SystemConfig;
using arc.data.model.Configuration;
using arc.domain.Configuration.BarcodeConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Config.Pages
{
    public class BarcodePrintConfigAdapter : IBarcodePrintConfigAdapter
    {
        private readonly IBarcodePrintConfigFactory _barcodePrintConfigFactory;
        private readonly IConfigRepository _configRepository;

        public BarcodePrintConfigAdapter(IBarcodePrintConfigFactory barcodePrintConfigFactory, IConfigRepository configRepository)
        {
            _barcodePrintConfigFactory = barcodePrintConfigFactory;
            _configRepository = configRepository;
        }

        public async Task<List<BarcodePrintConfig>> GetBarcodeConfigByTypeAsync(string barcodeConfigType)
        {
            var parameters = new QueryFilterConfig().AddString("ConfigTypeId", barcodeConfigType);
            var configRecords = await _configRepository.GetConfigListAsync(parameters);

            var configList = new List<BarcodePrintConfig>();
            foreach (var record in configRecords)
            {
                var configDef = record.Contents == null || record.Contents == "{}" ? _barcodePrintConfigFactory.GetBarcodePrintConfig(record.ConfigName) : JsonConvert.DeserializeObject<BarcodePrintConfig>(record.Contents);
                configList.Add(configDef);
            }
            return configList;
        }

        public async Task<BarcodePrintConfig> GetBarcodePrintConfigAsync(string barcodeConfigName)
        {
            var parameters = new QueryFilterConfig().AddString("ConfigName", barcodeConfigName);
            var configRecord = await _configRepository.SingleConfigByNameAsync(parameters);

            return configRecord.Contents == null || configRecord.Contents == "{}" ? _barcodePrintConfigFactory.GetBarcodePrintConfig(barcodeConfigName) : JsonConvert.DeserializeObject<BarcodePrintConfig>(configRecord.Contents);
        }

        public async Task UpdateBarcodePrintConfigAsync(string dataToSave)
        {
            var barcodePrintConfig = JsonConvert.DeserializeObject<BarcodePrintConfig>(dataToSave);

            var data = new ConfigsDataModel()
            {
                Id = barcodePrintConfig.Id,
                ConfigName = barcodePrintConfig.Name.ToLower(),
                Contents = dataToSave
            };

            await _configRepository.EditCustomEntryAsync(data);
        }
    }
}
