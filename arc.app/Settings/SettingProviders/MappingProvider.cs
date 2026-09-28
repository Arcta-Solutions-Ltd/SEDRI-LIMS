using arc.common.Models.Settings;
using arc.domain.Settings;
using System.Linq;

namespace arc.app.Settings.SettingProviders
{
    internal class MappingProvider : ISettingProvider
    {
        public SettingConfig SetValue(SettingConfig currentSetting, SettingsModel data)
        {
            currentSetting.MappingValues = data.MappingGrid.Where(i => !string.IsNullOrEmpty(i.MapValue)).Select(s => new SettingMappingConfig { Type = int.Parse(s.Key), Value = s.MapValue }).ToList(); ;
            return currentSetting;
        }

        public string Validate(SettingConfig currentSetting, SettingsModel data)
        {
            var blankMappings = data.MappingGrid.Where(d => string.IsNullOrEmpty(d.MapValue));
            var returnMessage = blankMappings.Count() > 0 ? currentSetting.ErrorMessage : "";
            return returnMessage;
        }
    }
}
