using arc.app.Configuration;
using arc.domain.Configuration.PagesConfig;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Config.Pages;

/// <summary>
/// Adapter for retrieving and managing page configurations.
/// </summary>
public class PageConfigAdapter : IPageConfigAdapter
{
    /// <summary>
    /// Factory for creating page configurations.
    /// </summary>
    private readonly IPageConfigFactory _pageConfigFactory;

    /// <summary>
    /// Cache for storing and retrieving configuration data.
    /// </summary>
    private readonly IConfigCache _configCache;

    private readonly ILogger<PageConfigAdapter> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="PageConfigAdapter"/> class.
    /// </summary>
    /// <param name="formConfigFactory">The page configuration factory to use.</param>
    /// <param name="configCache">The configuration cache to use.</param>
    /// <param name="logger">Logger for diagnostic output when stored page configs are loaded.</param>
    public PageConfigAdapter(IPageConfigFactory formConfigFactory, IConfigCache configCache, ILogger<PageConfigAdapter> logger)
    {
        _pageConfigFactory = formConfigFactory;
        _configCache = configCache;
        _logger = logger;
    }

    /// <summary>
    /// Asynchronously retrieves a page configuration based on the specified page name.
    /// </summary>
    /// <param name="pageName">The name of the page to retrieve.</param>
    /// <returns>
    /// A task representing the asynchronous operation, containing the page configuration,
    /// or null if no valid configuration record is found.
    /// </returns>
    public async Task<PageConfig> GetPageAsync(string pageName)
    {
        var configRecord = await _configCache.GetConfigRecordAsync(pageName);

        var useFactory = configRecord == null || configRecord.Contents == null || configRecord.Contents == "{}";
        var pageDef = useFactory
            ? _pageConfigFactory.GetPage(pageName)
            : JsonConvert.DeserializeObject<PageConfig>(configRecord.Contents);

        ApplyFactoryGroupMetadata(pageName, pageDef, fromDatabase: !useFactory);

        LogSpecimenTimingsCollectionDefaults(pageName, pageDef, fromDatabase: !useFactory);
        LogDependentFieldDefaults(pageName, pageDef, fromDatabase: !useFactory);
        LogCulturePageConfigureActions(pageName, pageDef, fromDatabase: !useFactory);

        return pageDef;
    }

    /// <summary>
    /// Forces the page group and anchor position of a built-in page to match its factory definition.
    /// Group membership is a system concept that is never editable through the configuration UI, so a
    /// stored override must not be able to drop or change it.
    /// </summary>
    /// <param name="pageName">The page configuration name.</param>
    /// <param name="pageDef">The loaded page definition to update.</param>
    /// <param name="fromDatabase">True when the definition came from a stored database override.</param>
    private void ApplyFactoryGroupMetadata(string pageName, PageConfig pageDef, bool fromDatabase)
    {
        if (pageDef == null)
        {
            return;
        }

        var factoryDefault = _pageConfigFactory.GetPage(pageName);
        if (factoryDefault == null || string.IsNullOrWhiteSpace(factoryDefault.PageGroup))
        {
            return;
        }

        var alreadyCorrect = string.Equals(pageDef.PageGroup, factoryDefault.PageGroup, StringComparison.OrdinalIgnoreCase)
            && pageDef.GroupAnchor == factoryDefault.GroupAnchor;
        if (alreadyCorrect)
        {
            return;
        }

        pageDef.PageGroup = factoryDefault.PageGroup;
        pageDef.GroupAnchor = factoryDefault.GroupAnchor;

        if (fromDatabase)
        {
            _logger.LogInformation(
                "Page config {PageName} loaded from database: applied factory page group {PageGroup} anchor {GroupAnchor}",
                pageName,
                factoryDefault.PageGroup,
                factoryDefault.GroupAnchor);
        }
    }

    /// <summary>
    /// Logs DefaultToNow flags for collection date/time when a stored DB page config is used (support diagnostics).
    /// </summary>
    private void LogSpecimenTimingsCollectionDefaults(string pageName, PageConfig pageDef, bool fromDatabase)
    {
        if (!fromDatabase || pageDef?.Columns == null)
        {
            return;
        }

        if (!string.Equals(pageName, "specimentimings", StringComparison.OrdinalIgnoreCase)
            && !string.Equals(pageName, "specimentimingsreceived", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        foreach (var fieldId in new[] { "CollectionDate", "CollectionTime" })
        {
            var field = pageDef.Columns
                .SelectMany(c => c.FormGroups ?? Enumerable.Empty<FormGroupConfig>())
                .SelectMany(fg => fg.Fields ?? Enumerable.Empty<FieldConfig>())
                .FirstOrDefault(f => string.Equals(f.Id, fieldId, StringComparison.OrdinalIgnoreCase));

            _logger.LogInformation(
                "Page config {PageName} loaded from database: {FieldId} DefaultToNow={DefaultToNow}",
                pageName,
                fieldId,
                field?.DefaultToNow ?? false);
        }
    }

    /// <summary>
    /// Logs dependent DefaultValue placeholders (<:fieldId:>) when a stored DB page config is loaded.
    /// </summary>
    private void LogDependentFieldDefaults(string pageName, PageConfig pageDef, bool fromDatabase)
    {
        if (!fromDatabase || pageDef?.Columns == null)
        {
            return;
        }

        foreach (var field in pageDef.Columns
            .SelectMany(c => c.FormGroups ?? Enumerable.Empty<FormGroupConfig>())
            .SelectMany(fg => fg.Fields ?? Enumerable.Empty<FieldConfig>()))
        {
            var defaultValue = field.DefaultValue;
            if (string.IsNullOrWhiteSpace(defaultValue)
                || !defaultValue.Contains("<:", StringComparison.Ordinal)
                || !defaultValue.Contains(":>", StringComparison.Ordinal))
            {
                continue;
            }

            var sourceFieldId = defaultValue
                .Replace("<:", string.Empty, StringComparison.Ordinal)
                .Replace(":>", string.Empty, StringComparison.Ordinal)
                .Trim();

            _logger.LogInformation(
                "Page config {PageName} loaded from database: {FieldId} dependent default from {SourceFieldId}",
                pageName,
                field.Id,
                sourceFieldId);
        }
    }

    /// <summary>
    /// Logs when a culture-form page loaded from the database lacks <c>add</c> in <c>ConfigureActions</c>,
    /// which would prevent Add field and Add subsection in Define Page Contents on installed systems.
    /// </summary>
    /// <param name="pageName">The page configuration name.</param>
    /// <param name="pageDef">The loaded page definition.</param>
    /// <param name="fromDatabase">True when the definition came from a stored database override.</param>
    private void LogCulturePageConfigureActions(string pageName, PageConfig pageDef, bool fromDatabase)
    {
        if (pageDef == null)
        {
            return;
        }

        if (!string.Equals(pageName, "specimengrowthdetails", StringComparison.OrdinalIgnoreCase)
            && !string.Equals(pageName, "specimenadditionalguidance", StringComparison.OrdinalIgnoreCase)
            && !string.Equals(pageName, "specimenotherinformationpage", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        var configureActions = pageDef.ConfigureActions ?? string.Empty;
        if (configureActions.Contains("add", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        _logger.LogInformation(
            "Page config {PageName} loaded from {Source}: ConfigureActions={ConfigureActions} (missing add — Define Page Contents add field/subsection disabled)",
            pageName,
            fromDatabase ? "database" : "factory",
            string.IsNullOrWhiteSpace(configureActions) ? "(none)" : configureActions);
    }
}

