using arc.app.Common;
using arc.app.Config.Barcode;
using arc.app.Config.Sidebar;
using arc.app.Configuration;
using arc.app.Security;
using arc.app.SystemConfig;
using arc.common.ExtensionMethods;
using arc.common.Models;
using arc.common.Models.User;
using arc.common.Utils;
using arc.data.model.Configuration;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Configuration.ReportsConfig;
using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace arc.app.Config;

/// <summary>
/// Handles the configuration process, including retrieving and processing various configuration details.
/// </summary>
/// <param name="permissionHandler">The handler for permission-related operations.</param>
/// <param name="jsonMapper">The mapper for converting objects to JSON.</param>
/// <param name="getListsConfiguration">The configuration handler for lists.</param>
/// <param name="configListUtils">Utility methods for list configurations.</param>
/// <param name="allowedButtonWorkflowStates">Handler for allowed button workflow states.</param>
/// <param name="removeButtonsWithoutPermission">Handler for removing buttons without permission.</param>
/// <param name="languageHandler">Handler for language translation.</param>
/// <param name="userRepository">Repository for user-related operations.</param>
/// <param name="logWriter">Logger for writing log information.</param>
/// <param name="configCache">Cache for configuration data.</param>
/// <param name="configRepository">Repository for configuration data.</param>
/// <param name="barcodePrintLabelResolver">Resolver that enriches barcode print configurations with labels.</param>
/// <param name="configuration">Application configuration, used to resolve <c>Files:StorageRoot</c> for placeholder substitution.</param>
public class HandleConfig(IPermissionHandler permissionHandler, IMapObjectArrayToJson jsonMapper, IGetListsConfiguration getListsConfiguration, IConfigListUtils configListUtils,
    IAllowedButtonWorkflowStates allowedButtonWorkflowStates, IRemoveButtonsWithoutPermission removeButtonsWithoutPermission, ILanguageHandler languageHandler,
    IUserRepository userRepository, ILogWriter logWriter, IConfigCache configCache, IConfigRepository configRepository,
    IBarcodePrintLabelResolver barcodePrintLabelResolver, IConfiguration configuration) : IHandleConfig
{
    /// <summary>
    /// Token used in page configuration placeholders that is substituted with the configured
    /// <c>Files:StorageRoot</c> value (forward-slashed) when the configuration is assembled.
    /// </summary>
    private const string StorageRootToken = "{STORAGE_ROOT}";

    /// <summary>
    /// Asynchronously retrieves the configuration for the specified user and language.
    /// </summary>
    /// <param name="token">The token information for the user.</param>
    /// <param name="language">The language for the configuration.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the configuration as a JSON string.</returns>
    public async Task<string> GetConfigurationAsync(TokenInfoModel token, string language)
    {
        var userName = token.Username;

        // Retrieve role permissions
        logWriter.LogInfo("Get Role Permissions", nameof(HandleConfig), nameof(GetConfigurationAsync));
        var roleWithPermissions = await permissionHandler.GetPermissionsAsync(userName);
        var allowedSidebarItemsBeforeMandatory = roleWithPermissions.MenuPermissionDetails.AllowedSidebarItems.OrEmpty();
        var allowedSidebarItems = allowedSidebarItemsBeforeMandatory.EnsureMandatorySidebarItems();

        if (allowedSidebarItemsBeforeMandatory.Length != allowedSidebarItems.Length ||
            !allowedSidebarItemsBeforeMandatory.Any(k => string.Equals(k, "home", StringComparison.OrdinalIgnoreCase)))
        {
            logWriter.LogInfo(
                $"Mandatory sidebar key 'home' injected at login for user {userName}. Stored keys=[{string.Join(",", allowedSidebarItemsBeforeMandatory)}].",
                nameof(HandleConfig),
                nameof(GetConfigurationAsync));
        }

        // Get left sidebar configuration
        logWriter.LogInfo("Get Left Side Bar Config", nameof(HandleConfig), nameof(GetConfigurationAsync));
        var sidebarConfig = new SidebarSetup().GetLeftSidebarConfig();
        sidebarConfig.OnlyKeepTheseSidebarItems(allowedSidebarItems);

        logWriter.LogInfo(
            $"Sidebar filtered for user {userName}: allowedKeys=[{string.Join(",", allowedSidebarItems)}], remainingTopLevelKeys=[{string.Join(",", sidebarConfig.Links.Select(l => l.Key))}]",
            nameof(HandleConfig),
            nameof(GetConfigurationAsync));

        if (allowedSidebarItems.Length == 0 &&
            sidebarConfig.Links.Any(l => l.Key is "specimens" or "patients"))
        {
            logWriter.LogWarning(
                $"Zero-permission user {userName} still has specimen or patient sidebar links after filtering.",
                nameof(HandleConfig),
                nameof(GetConfigurationAsync));
        }

        await configCache.LoadConfig();

        var recordViewsToAddFromMenu = new List<string> { "formconfig", "settings", "testrecordview", "instrumentresultrecordview", "admissionrecordview", "requestrecordview" };

        // Retrieve list view configuration
        logWriter.LogInfo("Get List View Config", nameof(HandleConfig), nameof(GetConfigurationAsync));
        var listViewsToDisplay = configListUtils.GetListViewConfigList(allowedSidebarItems.ToList(), recordViewsToAddFromMenu);

        // Retrieve record view configuration
        logWriter.LogInfo("Get Record View Config", nameof(HandleConfig), nameof(GetConfigurationAsync));
        var recordViewsToDisplay = configListUtils.GetRecordViewConfigList(listViewsToDisplay, recordViewsToAddFromMenu);

        // Retrieve reporting grid configuration
        logWriter.LogInfo("Get Reporting Grid Config", nameof(HandleConfig), nameof(GetConfigurationAsync));
        var reportingGridsToDisplay = await configListUtils.GetReportingGridsAsync(allowedSidebarItems.ToList());

        // Retrieve barcode print configuration
        logWriter.LogInfo("Get Barcode Config", nameof(HandleConfig), nameof(GetConfigurationAsync));
        var barcodePrintConfigsToDisplay = await configListUtils.GetBarcodePrintConfigListAsync();
        await barcodePrintLabelResolver.EnrichBarcodePrintConfigsAsync(barcodePrintConfigsToDisplay, userName);

        // Retrieve diary configuration
        logWriter.LogInfo("Get Diary Config", nameof(HandleConfig), nameof(GetConfigurationAsync));
        var diariesToDisplay = await configListUtils.GetDiaryConfigListAsync();

        // Retrieve form configuration
        logWriter.LogInfo("Get Form Config", nameof(HandleConfig), nameof(GetConfigurationAsync));
        var formsToDisplay = await configListUtils.GetFormConfigListAsync(roleWithPermissions.EventPermissionDetails.AllowedEvents.ToList());

        // Retrieve pages configuration
        logWriter.LogInfo("Get Pages", nameof(HandleConfig), nameof(GetConfigurationAsync));
        var (pagesToDisplay, additionalUIEvents) = await configListUtils.GetPagesAsync();
        formsToDisplay.AddRange(await configListUtils.GetAdditonalFormConfigListAsync(additionalUIEvents,roleWithPermissions.EventPermissionDetails.AllowedEvents.ToList()));
        (pagesToDisplay, additionalUIEvents) = await configListUtils.GetPagesAsync();

        // Retrieve UI events
        logWriter.LogInfo("Get UIEvents", nameof(HandleConfig), nameof(GetConfigurationAsync));
        var uiEventsToDisplay = configListUtils.GetUiEvents();

        // Retrieve graph configuration
        logWriter.LogInfo("Get Graph Config", nameof(HandleConfig), nameof(GetConfigurationAsync));
        var graphsToDisplay = configListUtils.GetGraphConfig(allowedSidebarItems);

        var homeDashboardGraphs = configListUtils.GetHomeDashboardGraphCatalog();
        var graphsForListResolution = new List<GraphConfig>(graphsToDisplay);
        foreach (var g in homeDashboardGraphs)
        {
            if (graphsForListResolution.TrueForAll(x => !string.Equals(x.Name, g.Name, StringComparison.OrdinalIgnoreCase)))
            {
                graphsForListResolution.Add(g);
            }
        }

        // Retrieve list configuration
        logWriter.LogInfo("Get List Configuration", nameof(HandleConfig), nameof(GetConfigurationAsync));
        var listsToDisplay = getListsConfiguration.GetConfiguration(listViewsToDisplay, pagesToDisplay, graphsForListResolution, reportingGridsToDisplay, token);

        // Get laboratory details
        logWriter.LogInfo("Get laboratory details", nameof(HandleConfig), nameof(GetConfigurationAsync));
        var laboratoriesToDisplay = await configListUtils.GetLaboratoryConfigAsync(token);

        // Get workflow details
        logWriter.LogInfo("Get workflow details", nameof(HandleConfig), nameof(GetConfigurationAsync));
        var workflowsToDisplay = configListUtils.GetWorkflowConfigList();

        // Remove buttons based on permission
        logWriter.LogInfo("Remove Buttons based on permission", nameof(HandleConfig), nameof(GetConfigurationAsync));
        listViewsToDisplay = removeButtonsWithoutPermission.RemoveButtonsFromListView(listViewsToDisplay, uiEventsToDisplay);
        recordViewsToDisplay = removeButtonsWithoutPermission.RemoveButtonsFromRecordView(recordViewsToDisplay, uiEventsToDisplay);

        // Remove buttons based on workflow
        logWriter.LogInfo("Remove Buttons based on workflow", nameof(HandleConfig), nameof(GetConfigurationAsync));
        listViewsToDisplay = await allowedButtonWorkflowStates.AddAllowedStatesToButtonsInView(listViewsToDisplay, uiEventsToDisplay, formsToDisplay);
        recordViewsToDisplay = await allowedButtonWorkflowStates.AddAllowedStatesToButtonsInView(recordViewsToDisplay, uiEventsToDisplay, formsToDisplay);

        // Remove add buttons from list views
        logWriter.LogInfo("Remove add buttons from list views", nameof(HandleConfig), nameof(GetConfigurationAsync));
        listViewsToDisplay = configListUtils.RemoveAddButtonsFromViewsIfNoPermission(listViewsToDisplay, uiEventsToDisplay);

        // Retrieve user preferences
        logWriter.LogInfo("Get user preferences", nameof(HandleConfig), nameof(GetConfigurationAsync));
        var preferences = await userRepository.PreferenceByUsernameAsync(userName);

        // Apply user-specific filter presets to list views and graphs
        ApplyUserFilterPresetsToConfig(listViewsToDisplay, graphsToDisplay, preferences);

        // Apply user-specific column layouts to list views
        ApplyUserColumnLayoutsToConfig(listViewsToDisplay, preferences, logWriter);

        var preferenceConfig = JsonSerializer.Serialize(preferences);

        var newSidebarConfig = JsonSerializer.Serialize(sidebarConfig).Replace("null", "\"\"");
        var rightSidebarConfig = """
            [{ "links": [ { "name": "Log Out", "key": "logout" } ] } ]
            """;

        var listViewStartsWith = """
            {"Name": "login", "Type": "login"}, {"Name": "home", "Type": "home"}
            """;
        var listViewConfig = jsonMapper.Map(listViewsToDisplay, listViewStartsWith);
        var recordViewConfig = jsonMapper.Map(recordViewsToDisplay);
        var barcodePrintConfig = jsonMapper.Map(barcodePrintConfigsToDisplay);
        var uiEventConfig = jsonMapper.Map(uiEventsToDisplay);
        var formConfig = jsonMapper.Map(formsToDisplay);
        var pagesConfig = SubstituteStorageRootToken(jsonMapper.Map(pagesToDisplay));
        var listConfig = jsonMapper.Map(listsToDisplay);
        var diaryViewConfig = jsonMapper.Map(diariesToDisplay);
        var graphConfig = jsonMapper.Map(graphsToDisplay);
        var homeDashboardGraphConfig = jsonMapper.Map(homeDashboardGraphs);
        var laboratoryConfig = ArcJson.Serialize(laboratoriesToDisplay);
        var workflowConfig = ArcJson.Serialize(workflowsToDisplay);
        var reportingGridConfig = jsonMapper.Map(reportingGridsToDisplay);

        var parameters = new QueryFilterConfig().AddString("configname", "generalsettings");
        var generalSettingsConfig = await configRepository.SingleConfigByNameAsync(parameters);
        var settingsConfig = GeneralSettingsConfigToJson(generalSettingsConfig);

        var jsonConfig = $$"""
            {
                "sidebar": [
                {{newSidebarConfig}}
                ],
                "rightSidebar": {{rightSidebarConfig}},
                "views": {{listViewConfig}},
                "recordviews": {{recordViewConfig}},
                "barcodeprintconfigs": {{barcodePrintConfig}},
                "uievents": {{uiEventConfig}},
                "forms": {{formConfig}},
                "pages": {{pagesConfig}},
                "lists": {{listConfig}},
                "diaries": {{diaryViewConfig}},
                "graphs": {{graphConfig}},
                "homeDashboardGraphs": {{homeDashboardGraphConfig}},
                "preferences": {{preferenceConfig}},
                "reportinggrids": {{reportingGridConfig}},
                "settings": {{settingsConfig}},
                "laboratory": {{laboratoryConfig}},
                "workflow": {{workflowConfig}}
            }
            """;

        // Carry out translation
        logWriter.LogInfo("Carry out translation", nameof(HandleConfig), nameof(GetConfigurationAsync));
        var returnConfig = await languageHandler.TranslateAsync(jsonConfig, language);

        // Add language tags to the config response
        logWriter.LogInfo("Get front end language tags", nameof(HandleConfig), nameof(GetConfigurationAsync));
        var languageConfig = await languageHandler.GetFrontEndTagsAsync(language);
        returnConfig = returnConfig.Remove(returnConfig.LastIndexOf('\n'));
        returnConfig += ",\n" + $$"""
                "language": {{languageConfig}}
            }
            """;

        return returnConfig;
    }

    /// <summary>
    /// Replaces the <see cref="StorageRootToken"/> in the supplied pages configuration JSON with the
    /// configured <c>Files:StorageRoot</c> value (normalized to forward slashes). This drives the
    /// helpful default-path placeholder on the export schedule output directory field.
    /// </summary>
    /// <param name="pagesConfig">The serialized pages configuration JSON.</param>
    /// <returns>The pages configuration with the storage-root token substituted.</returns>
    private string SubstituteStorageRootToken(string pagesConfig)
    {
        if (string.IsNullOrEmpty(pagesConfig) || !pagesConfig.Contains(StorageRootToken))
        {
            return pagesConfig;
        }

        var storageRoot = configuration["Files:StorageRoot"];
        if (string.IsNullOrWhiteSpace(storageRoot))
        {
            // Logged as an error so an installed system (where we cannot attach a debugger) shows why
            // the output directory placeholder is empty, then fall back to clearing the token so the
            // rendered JSON stays valid.
            logWriter.LogError("Files:StorageRoot is missing or blank; export schedule output directory placeholder will be empty.", nameof(HandleConfig), nameof(SubstituteStorageRootToken));
            return pagesConfig.Replace(StorageRootToken, "");
        }

        var forwardSlashRoot = storageRoot.ToForwardSlashPath();
        logWriter.LogInfo($"Substituting export schedule storage root placeholder with '{forwardSlashRoot}'", nameof(HandleConfig), nameof(SubstituteStorageRootToken));
        return pagesConfig.Replace(StorageRootToken, forwardSlashRoot);
    }

    /// <summary>
    /// Applies user-specific filter presets to list views and graphs. When a user has saved filter
    /// presets for a view, those presets replace the default presets for that view.
    /// </summary>
    /// <param name="listViews">List view configurations to update.</param>
    /// <param name="graphs">Graph configurations to update.</param>
    /// <param name="preferences">User preferences containing FilterPresets JSON.</param>
    private static void ApplyUserFilterPresetsToConfig(List<ListViewConfig> listViews, List<GraphConfig> graphs, PreferenceConfigModel preferences)
    {
        if (string.IsNullOrWhiteSpace(preferences?.FilterPresets) || preferences.FilterPresets == "{}")
        {
            return;
        }

        Dictionary<string, List<FilterPresetConfig>> userPresets;
        try
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            userPresets = JsonSerializer.Deserialize<Dictionary<string, List<FilterPresetConfig>>>(preferences.FilterPresets, options);
        }
        catch
        {
            return;
        }

        if (userPresets == null || userPresets.Count == 0)
        {
            return;
        }

        foreach (var view in listViews ?? Enumerable.Empty<ListViewConfig>())
        {
            if (view?.Name != null && userPresets.TryGetValue(view.Name, out var presets) && presets != null && presets.Count > 0)
            {
                view.FilterPresets = presets;
            }
        }

        foreach (var graph in graphs ?? Enumerable.Empty<GraphConfig>())
        {
            if (graph?.Name != null && userPresets.TryGetValue(graph.Name, out var presets) && presets != null && presets.Count > 0)
            {
                graph.FilterPresets = presets;
            }
        }
    }

    /// <summary>
    /// Applies user-specific column layouts to list views. When a user has saved column layout
    /// preferences for a view, those are set on the view's ColumnLayout property.
    /// </summary>
    /// <param name="listViews">List view configurations to update.</param>
    /// <param name="preferences">User preferences containing ColumnLayouts JSON.</param>
    /// <param name="logWriter">Logger for writing log information.</param>
    private static void ApplyUserColumnLayoutsToConfig(List<ListViewConfig> listViews, PreferenceConfigModel preferences, ILogWriter logWriter)
    {
        if (string.IsNullOrWhiteSpace(preferences?.ColumnLayouts) || preferences.ColumnLayouts == "{}")
        {
            return;
        }

        Dictionary<string, ColumnLayoutConfig> userLayouts;
        try
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            userLayouts = JsonSerializer.Deserialize<Dictionary<string, ColumnLayoutConfig>>(preferences.ColumnLayouts, options);
        }
        catch
        {
            return;
        }

        if (userLayouts == null || userLayouts.Count == 0)
        {
            return;
        }

        var viewNames = string.Join(", ", userLayouts.Keys);
        logWriter.LogInfo($"ApplyUserColumnLayoutsToConfig: Applying column layouts for {userLayouts.Count} view(s): {viewNames}", nameof(HandleConfig), nameof(ApplyUserColumnLayoutsToConfig));

        foreach (var view in listViews ?? Enumerable.Empty<ListViewConfig>())
        {
            if (view?.Name != null && userLayouts.TryGetValue(view.Name, out var layout) && layout != null)
            {
                view.ColumnLayout = layout;
            }
        }
    }

    /// <summary>
    /// Converts the general settings configuration to a JSON string.
    /// </summary>
    /// <param name="generalSettingsConfig">The general settings configuration data model.</param>
    /// <returns>A JSON string representing the general settings configuration.</returns>
    private static string GeneralSettingsConfigToJson(ConfigsDataModel generalSettingsConfig)
    {
        var settingsObject = JsonSerializer.Deserialize<JsonArray>(generalSettingsConfig.Contents).Select(a =>
        {
            // Extract and process the setting name
            string settingName = a["Id"].ToString();
            int pipeIndex = settingName.IndexOf('|');
            if (pipeIndex > -1)
            {
                settingName = settingName.Substring(pipeIndex + 1);
            }

            // Return an anonymous object with name and value properties
            return new
            {
                name = settingName,
                value = a["Value"]
            };
        });

        // Serialize the processed settings object to a JSON string
        return JsonSerializer.Serialize(settingsObject);
    }
}
