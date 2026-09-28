using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Configuration.Export
{
    public interface IExportConfigurationProcessor
    {
        public Task<List<string>> GetDataAsync(Dictionary<string, IEnumerable<string>> _configDefinitions);
    }
}
