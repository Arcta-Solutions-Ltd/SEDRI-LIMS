using arc.app.Common;
using arc.app.Security;
using arc.common;
using arc.common.Models;
using arc.domain;
using arc.domain.Configuration.EventsConfig;
using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Users
{
    /// <summary>
    /// Handles saving user filter presets to MoreData. Merges the new FilterPresets for the given view
    /// with existing MoreData and persists to the Users table.
    /// </summary>
    internal class SaveFilterPresetsEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TokenInfoModel _token;
        private readonly ILogger<SaveFilterPresetsEvent> _logger;

        public SaveFilterPresetsEvent(IServiceProvider serviceProvider, TokenInfoModel token)
        {
            _serviceProvider = serviceProvider;
            _token = token;
            _logger = serviceProvider.GetRequiredService<ILogger<SaveFilterPresetsEvent>>();
        }

        /// <summary>
        /// Saves filter presets for a view, merging with existing MoreData.
        /// Expects dataToSave as JSON: { "ViewName": "specimens", "FilterPresets": [...] }
        /// </summary>
        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var userRepository = _serviceProvider.GetService<IUserRepository>();

            var payload = JsonConvert.DeserializeObject<SaveFilterPresetsPayload>(dataToSave);
            if (payload?.ViewName == null || payload.FilterPresets == null)
            {
                _logger.LogWarning(
                    "SaveFilterPresets rejected: missing ViewName or FilterPresets for user {UserId}.",
                    _token.Id);
                return 0;
            }

            var userId = int.Parse(_token.Id);
            var presetKeys = payload.FilterPresets
                .Where(p => p != null && !string.IsNullOrWhiteSpace(p.Key))
                .Select(p => p.Key)
                .ToList();

            var currentMoreData = await userRepository.GetUserMoreDataByIdAsync(userId);
            var moreDataObj = string.IsNullOrWhiteSpace(currentMoreData)
                ? new JObject()
                : JObject.Parse(currentMoreData);

            var filterPresetsObj = moreDataObj["FilterPresets"] as JObject;
            if (filterPresetsObj == null)
            {
                filterPresetsObj = new JObject();
                moreDataObj["FilterPresets"] = filterPresetsObj;
            }

            filterPresetsObj[payload.ViewName] = JArray.FromObject(payload.FilterPresets);

            var preference = new Preference
            {
                Id = userId,
                MoreData = moreDataObj.ToString()
            };
            await userRepository.EditUserPreferenceAsync(preference);

            _logger.LogInformation(
                "Saved {PresetCount} filter preset(s) for user {UserId}, view {ViewName}; preset keys: {PresetKeys}.",
                payload.FilterPresets.Count,
                userId,
                payload.ViewName,
                presetKeys.Count > 0 ? string.Join(", ", presetKeys) : "(none)");

            return int.Parse(id ?? "0");
        }

        private class SaveFilterPresetsPayload
        {
            public string ViewName { get; set; }
            public List<FilterPresetConfig> FilterPresets { get; set; }
        }
    }
}
