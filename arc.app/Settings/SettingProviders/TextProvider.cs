using arc.common.Models.Settings;
using arc.domain.Settings;

namespace arc.app.Settings.SettingProviders
{
    internal class TextProvider : ISettingProvider
    {
        public SettingConfig SetValue(SettingConfig currentSetting, SettingsModel data)
        {
            currentSetting.Value = data.TextValue;
            return currentSetting;
        }

        public string Validate(SettingConfig currentSetting, SettingsModel data)
        {
            return string.IsNullOrEmpty(data.TextValue) ? currentSetting.ErrorMessage : "";
        }
    }
}
