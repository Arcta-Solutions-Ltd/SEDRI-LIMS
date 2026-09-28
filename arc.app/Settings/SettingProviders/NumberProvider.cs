using arc.common.Models.Settings;
using arc.domain.Settings;

namespace arc.app.Settings.SettingProviders
{
    internal class NumberProvider : ISettingProvider
    {
        public SettingConfig SetValue (SettingConfig currentSetting, SettingsModel data)
        {
            currentSetting.Value = data.Value;
            return currentSetting;
        }

        public string Validate (SettingConfig currentSetting, SettingsModel data)
        {
            var returnMessage = "";
            if (! string.IsNullOrEmpty(currentSetting.Min))
            {
                var min = int.Parse(currentSetting.Min);
                returnMessage = string.IsNullOrEmpty(data.Value) || int.Parse(data.Value) < min ? currentSetting.ErrorMessage : ""; 
            }

            if (returnMessage == "" && !string.IsNullOrEmpty(currentSetting.Max))
            {
                var max = int.Parse(currentSetting.Max);
                returnMessage = string.IsNullOrEmpty(data.Value) || int.Parse(data.Value) > max ? currentSetting.ErrorMessage : "";
            }

            return returnMessage;
        }
    }
}
