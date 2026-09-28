using arc.app.Common;
using arc.app.Security;
using arc.common;
using arc.common.Models;
using arc.common.Models.User;
using arc.domain;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Users
{
    /// <summary>
    /// Handles saving user column layout preferences to MoreData. Merges the new ColumnLayout for the given view
    /// with existing MoreData and persists to the Users table.
    /// </summary>
    internal class SaveColumnLayoutsEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TokenInfoModel _token;
        private readonly ILogWriter _logWriter;

        public SaveColumnLayoutsEvent(IServiceProvider serviceProvider, TokenInfoModel token, ILogWriter logWriter)
        {
            _serviceProvider = serviceProvider;
            _token = token;
            _logWriter = logWriter;
        }

        /// <summary>
        /// Saves or clears column layout for a view, merging with existing MoreData.
        /// Expects dataToSave as JSON: { "ViewName": "specimens", "ColumnLayout": { ... } } or { "ViewName": "specimens", "ClearLayout": true } to reset.
        /// </summary>
        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var userRepository = _serviceProvider.GetService<IUserRepository>();

            var payload = JsonConvert.DeserializeObject<SaveColumnLayoutsPayload>(dataToSave);
            if (payload?.ViewName == null)
            {
                _logWriter.LogInfo("SaveColumnLayoutsEvent: Skipping save - ViewName is null", nameof(SaveColumnLayoutsEvent), nameof(RunAsync));
                return 0;
            }
            if (payload.ClearLayout != true && payload.ColumnLayout == null)
            {
                _logWriter.LogInfo("SaveColumnLayoutsEvent: Skipping save - ColumnLayout is null and ClearLayout is false", nameof(SaveColumnLayoutsEvent), nameof(RunAsync));
                return 0;
            }

            var userId = int.Parse(_token.Id);
            _logWriter.LogInfo($"SaveColumnLayoutsEvent: Saving column layout for view '{payload.ViewName}', userId={userId}", nameof(SaveColumnLayoutsEvent), nameof(RunAsync));

            try
            {
                var currentMoreData = await userRepository.GetUserMoreDataByIdAsync(userId);
                var moreDataObj = string.IsNullOrWhiteSpace(currentMoreData)
                    ? new JObject()
                    : JObject.Parse(currentMoreData);

                var columnLayoutsObj = moreDataObj["ColumnLayouts"] as JObject;
                if (columnLayoutsObj == null)
                {
                    columnLayoutsObj = new JObject();
                    moreDataObj["ColumnLayouts"] = columnLayoutsObj;
                }

                if (payload.ClearLayout == true)
                {
                    columnLayoutsObj.Remove(payload.ViewName);
                }
                else
                {
                    columnLayoutsObj[payload.ViewName] = JObject.FromObject(payload.ColumnLayout);
                }

                var preference = new Preference
                {
                    Id = userId,
                    MoreData = moreDataObj.ToString()
                };
                await userRepository.EditUserPreferenceAsync(preference);

                _logWriter.LogInfo($"SaveColumnLayoutsEvent: Successfully saved column layout for view '{payload.ViewName}'", nameof(SaveColumnLayoutsEvent), nameof(RunAsync));
                return int.Parse(id ?? "0");
            }
            catch (Exception ex)
            {
                _logWriter.LogError($"SaveColumnLayoutsEvent: Failed to save column layout for view '{payload.ViewName}': {ex.Message}", nameof(SaveColumnLayoutsEvent), nameof(RunAsync));
                throw;
            }
        }

        private class SaveColumnLayoutsPayload
        {
            public string ViewName { get; set; }
            public ColumnLayoutConfig ColumnLayout { get; set; }
            public bool ClearLayout { get; set; }
        }
    }
}
