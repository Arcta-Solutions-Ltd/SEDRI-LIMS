using arc.common.Models.Settings;
using arc.domain.Settings;

namespace arc.app.Settings.SettingProviders
{
    public interface ISettingProvider
    {
        SettingConfig SetValue(SettingConfig currentSetting, SettingsModel data);
        string Validate(SettingConfig currentSetting, SettingsModel data);
    }
}
