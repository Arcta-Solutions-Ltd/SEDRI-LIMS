using arc.common.Models.Settings;
using arc.domain.Settings;

namespace arc.app.Settings.SettingProviders
{
    internal class YearListProvider : ISettingProvider
    {
        public SettingConfig SetValue(SettingConfig currentSetting, SettingsModel data)
        {
            currentSetting.Value = data.YearSetting;
            return currentSetting;
        }

        public string Validate(SettingConfig currentSetting, SettingsModel data)
        {
            return "";
        }
    }
}
