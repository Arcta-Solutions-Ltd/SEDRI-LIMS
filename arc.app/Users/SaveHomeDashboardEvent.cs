using arc.app.Common;
using arc.app.Security;
using arc.common;
using arc.common.Models;
using arc.domain;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace arc.app.Users;

/// <summary>
/// Persists the user's home dashboard configuration into <c>users.moredata</c> as <c>HomeDashboard</c> JSON.
/// </summary>
internal class SaveHomeDashboardEvent : IRun
{
    private readonly IServiceProvider _serviceProvider;
    private readonly TokenInfoModel _token;
    private readonly ILogWriter _logWriter;

    public SaveHomeDashboardEvent(IServiceProvider serviceProvider, TokenInfoModel token, ILogWriter logWriter)
    {
        _serviceProvider = serviceProvider;
        _token = token;
        _logWriter = logWriter;
    }

    /// <summary>
    /// Saves home dashboard JSON. Expects <paramref name="dataToSave"/> as JSON with a <c>HomeDashboard</c> property (object or stringified JSON).
    /// </summary>
    public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
    {
        var userRepository = _serviceProvider.GetService<IUserRepository>();

        JObject root;
        try
        {
            root = JObject.Parse(dataToSave ?? "{}");
        }
        catch (Exception ex)
        {
            _logWriter.LogError($"SaveHomeDashboardEvent: invalid JSON: {ex.Message}", nameof(SaveHomeDashboardEvent), nameof(RunAsync));
            return 0;
        }

        var homeToken = root["HomeDashboard"];
        if (homeToken == null || homeToken.Type == JTokenType.Null)
        {
            _logWriter.LogInfo("SaveHomeDashboardEvent: HomeDashboard missing, skipping", nameof(SaveHomeDashboardEvent), nameof(RunAsync));
            return 0;
        }

        var userId = int.Parse(_token.Id);
        _logWriter.LogInfo($"SaveHomeDashboardEvent: saving for userId={userId}", nameof(SaveHomeDashboardEvent), nameof(RunAsync));

        try
        {
            var currentMoreData = await userRepository.GetUserMoreDataByIdAsync(userId);
            var moreDataObj = string.IsNullOrWhiteSpace(currentMoreData)
                ? new JObject()
                : JObject.Parse(currentMoreData);

            moreDataObj["HomeDashboard"] = homeToken.Type == JTokenType.String
                ? JToken.Parse(homeToken.Value<string>() ?? "{}")
                : homeToken;

            var preference = new Preference
            {
                Id = userId,
                MoreData = moreDataObj.ToString()
            };
            await userRepository.EditUserPreferenceAsync(preference);

            _logWriter.LogInfo("SaveHomeDashboardEvent: saved successfully", nameof(SaveHomeDashboardEvent), nameof(RunAsync));
            return int.Parse(id ?? "0");
        }
        catch (Exception ex)
        {
            _logWriter.LogError($"SaveHomeDashboardEvent: failed: {ex.Message}", nameof(SaveHomeDashboardEvent), nameof(RunAsync));
            throw;
        }
    }
}
