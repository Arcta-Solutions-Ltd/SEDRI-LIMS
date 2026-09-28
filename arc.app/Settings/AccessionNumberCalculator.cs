using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using arc.app.SystemConfig;
using arc.common.Models.Specimen;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Settings;
using Newtonsoft.Json;

namespace arc.app.Settings
{
    public class AccessionNumberCalculator : IAccessionNumberCalculator
    {
        private readonly IConfigRepository _configRepository;

        public AccessionNumberCalculator(IConfigRepository configRepository)
        {
            _configRepository = configRepository;
        }

        public async Task<string> GetAccessionNumberAsync(string category, CreateSpecimenEventModel data)
        {
            var queryFilter = new QueryFilterConfig();
            queryFilter.AddString("configname", category);
            var config = await _configRepository.SingleConfigByNameAsync(queryFilter);

            var settingConfig = JsonConvert.DeserializeObject<List<SettingConfig>>(config.Contents);

            var completePrefixValue = "";
            var completeSeedMaskValue = "";
            var numberLength = 0;
            foreach (var configItem in settingConfig)
            {
                var idList = configItem.Id.Split("|");
                var currentPrefixValue = "";
                var currentSeedMaskValue = "";
                if (configItem.Enabled)
                {
                    switch (idList[1])
                    {
                        case "year":
                            currentPrefixValue = DateTime.Now.Year.ToString();
                            if (configItem.Value == "598")
                            {
                                currentPrefixValue = currentPrefixValue.Substring(2, 2);
                            }
                            break;
                        case "monthnumber":
                            currentPrefixValue = DateTime.Now.ToString("MM");
                            break;
                        case "daynumber":
                            currentPrefixValue = DateTime.Now.ToString("dd");
                            break;
                        case "specimentype":
                            var specimenTypeConfig = configItem.MappingValues.FirstOrDefault(s => s.Type == data.SpecimenTypeId);
                            if (specimenTypeConfig != null)
                            {
                                currentPrefixValue = specimenTypeConfig.Value;
                            }
                            break;
                        case "sequence":
                            numberLength = int.Parse(configItem.Value);
                            break;
                        default:
                            currentPrefixValue = configItem.Value.Trim();
                            break;
                    }
                    completePrefixValue += currentPrefixValue;
                }
                if (configItem.Enabled && configItem.Enabled2) // Reset sequence on change?
                {
                    switch (idList[1])
                    {
                        case "year":
                            currentSeedMaskValue = DateTime.Now.Year.ToString();
                            if (configItem.Value == "598")
                            {
                                currentSeedMaskValue = currentSeedMaskValue.Substring(2, 2);
                            }
                            break;
                        case "monthnumber":
                            currentSeedMaskValue = DateTime.Now.ToString("MM");
                            break;
                        case "daynumber":
                            currentSeedMaskValue = DateTime.Now.ToString("dd");
                            break;
                        case "specimentype":
                            var specimenTypeConfig = configItem.MappingValues.FirstOrDefault(s => s.Type == data.SpecimenTypeId);
                            if (specimenTypeConfig != null)
                            {
                                currentSeedMaskValue = specimenTypeConfig.Value;
                            }
                            break;
                        default:
                            currentSeedMaskValue = configItem.Value.Trim();
                            break;
                    }
                    completeSeedMaskValue += currentSeedMaskValue;
                }
            }

            completePrefixValue = string.IsNullOrEmpty(completePrefixValue) ? "<:sequence:>" : completePrefixValue;
            completeSeedMaskValue = string.IsNullOrEmpty(completeSeedMaskValue) ? "<:sequence:>" : completeSeedMaskValue;
            return await _configRepository.GetNextAccessionNumberAsync(completePrefixValue, completeSeedMaskValue, numberLength, category);
        }
    }
}
