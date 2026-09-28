using arc.app.Common;
using arc.app.Configuration.Export;
using arc.common.Models.Config;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Configuration
{
    /// <summary>
    /// Handler for the event which transfers configuration information into a file.
    /// </summary>
    public class ConfigTransferHandler : IConfigTransferHandler
    {
        public readonly IExportConfigurationProcessor _processor;
        private readonly ILogWriter _logWriter;
        private readonly Dictionary<string, IEnumerable<string>> _configDefinitions = new Dictionary<string, IEnumerable<string>>();

        public ConfigTransferHandler(IExportConfigurationProcessor processor, ILogWriter logWriter)
        {
            _processor = processor;
            _logWriter = logWriter;
        }

        /// <summary>
        /// Gets all of the configuration information for the selected categories.
        /// </summary>
        /// <param name="settings">Which configuration categories information should be obtained for</param>
        /// <returns>All the data from all of the configuration categories requested</returns>
        public async Task<string> GetConfig(ConfigurationExportModel settings)
        {
            var returnList = new List<string>();

            var processors = new Dictionary<string, string> { 
                { "Configuration", settings.Configuration }, 
                { "ListItems", settings.ListItems }, 
                { "Language", settings.Language },
                { "Coding", settings.Coding },
                { "QualityControl", settings.QualityControl },
                { "ExportProfile", settings.Exportprofile }
            };

            var configMapping = new Dictionary<string, List<string>>
            {
                { "Configuration", new List<string> { "configs" } },
                { "ListItems", new List<string> { "listitem", "list" } },
                { "Language", new List<string> { "language" } },
                { "Coding", new List<string> { "iqctestprofiles", "iqctestprofileqcantibiotics", "iqctestprofileqcorganisms", "qcorganisms","qcantibiotics","organism", "ordercat", "family",
                    "genus", "species", "subspecies", "additional", "organismcoding", "organismsynonyms", "breakpoint","testpattern", "testpatternline", "alert", "alertlines","alerttestlines", 
                    "antibiotic", "antibioticcoding" } },
                { "ExportProfile", new List<string> { "exportprofile", "exportprofilerecord" } }
            };

            _logWriter.LogInfo($"Load the configuration definitions", "ConfigTransferHandler", "GetConfig");
            foreach (var processor in processors)
            {
                if (processor.Value == "Yes" && configMapping.TryGetValue(processor.Key, out List<string> value))
                {
                    _configDefinitions.Add(processor.Key, value);
                }
            }

            _logWriter.LogInfo($"Start getting the configuration information from the database", "ConfigTransferHandler", "GetConfig");
            if (_configDefinitions.Count > 0)
            {
                returnList = await _processor.GetDataAsync(_configDefinitions);
            }

            _logWriter.LogInfo($"All configuration information obtained from the database", "ConfigTransferHandler", "GetConfig");
            return JsonConvert.SerializeObject(returnList);
        }
    }
}
