using arc.app.Security;
using arc.common;
using arc.common.Data;
using arc.common.Models;
using arc.domain;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using arc.app.Common;
using System;

namespace arc.app.Users
{
    /// <summary>
    /// Handles saving user preferences from the preference form. Merges new preference values
    /// with existing MoreData to preserve FilterPresets and other keys not in the form.
    /// MoreData is part of the Users table and is accessed via UserRepository.
    /// </summary>
    internal class EditUserPreferenceEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TokenInfoModel _token;

        public EditUserPreferenceEvent(IServiceProvider serviceProvider, TokenInfoModel token)
        {
            _serviceProvider = serviceProvider;
            _token = token;
        }

        /// <summary>
        /// Saves user preferences, merging form data with existing MoreData to preserve FilterPresets.
        /// </summary>
        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var userRepository = _serviceProvider.GetService<IUserRepository>();
            var moreDataGenerator = _serviceProvider.GetService<IGenerateMoreData>();

            var preference = JsonConvert.DeserializeObject<Preference>(dataToSave);
            var formMoreData = moreDataGenerator.GetMoreDataJsonString("users", dataToSave);

            var userId = int.Parse(_token.Id);
            var currentMoreData = await userRepository.GetUserMoreDataByIdAsync(userId);
            var existingObj = string.IsNullOrWhiteSpace(currentMoreData) ? new JObject() : JObject.Parse(currentMoreData);
            var formObj = string.IsNullOrWhiteSpace(formMoreData) ? new JObject() : JObject.Parse(formMoreData);

            foreach (var prop in formObj.Properties())
            {
                existingObj[prop.Name] = prop.Value;
            }

            preference.MoreData = existingObj.ToString();
            preference.Id = userId;
            await userRepository.EditUserPreferenceAsync(preference);
            return int.Parse(id);
        }
    }
}
