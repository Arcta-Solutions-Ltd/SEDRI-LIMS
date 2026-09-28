using arc.app.Common;
using arc.app.Config.Forms;
using arc.app.Config.Pages;
using arc.common.Data;
using arc.common.ExtensionMethods;
using arc.common.Models.Config;
using arc.domain.Configuration.PagesConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Configuration;

/// <summary>
/// Derives save routing from page <c>TableName</c> settings and data model column definitions.
/// </summary>
public sealed class FormPageTableRoutingService : IFormPageTableRoutingService
{
    private const string PrimaryTable = "specimen";
    private readonly IFormConfigAdapter _formAdapter;
    private readonly IPageConfigAdapter _pageAdapter;
    private readonly IDataModelColumnResolver _columnResolver;
    private readonly ILogWriter _logWriter;

    /// <summary>
    /// Initializes a new instance of the <see cref="FormPageTableRoutingService"/> class.
    /// </summary>
    public FormPageTableRoutingService(
        IFormConfigAdapter formAdapter,
        IPageConfigAdapter pageAdapter,
        IDataModelColumnResolver columnResolver,
        ILogWriter logWriter)
    {
        _formAdapter = formAdapter;
        _pageAdapter = pageAdapter;
        _columnResolver = columnResolver;
        _logWriter = logWriter;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<TableExceptionModel>> BuildRoutingAsync(string formName)
    {
        if (string.IsNullOrWhiteSpace(formName))
        {
            _logWriter.LogInfo("Form page routing skipped: FormName is missing", nameof(FormPageTableRoutingService), nameof(BuildRoutingAsync));
            return Array.Empty<TableExceptionModel>();
        }

        var formDef = await _formAdapter.GetFormAsync(formName);
        if (!FormPageTargetTableExtensions.IsSpecimenRecordForm(formDef?.SingleItemName))
        {
            _logWriter.LogInfo(
                $"Form page routing skipped: form '{formName}' is not a specimen record form",
                nameof(FormPageTableRoutingService),
                nameof(BuildRoutingAsync));
            return Array.Empty<TableExceptionModel>();
        }

        if (formDef?.Pages == null || formDef.Pages.Count == 0)
        {
            _logWriter.LogInfo($"Form page routing: no pages loaded for form '{formName}'", nameof(FormPageTableRoutingService), nameof(BuildRoutingAsync));
            return Array.Empty<TableExceptionModel>();
        }

        var routing = new List<TableExceptionModel>();
        var countsByTable = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        var seenFields = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var pageName in formDef.Pages)
        {
            if (string.IsNullOrWhiteSpace(pageName))
            {
                continue;
            }

            var page = await _pageAdapter.GetPageAsync(pageName);
            if (page == null)
            {
                continue;
            }

            AppendPageRouting(formName, page, routing, countsByTable, seenFields);
        }

        _logWriter.LogInfo(
            $"Form '{formName}' routing complete: {routing.Count} exception(s) [{string.Join(", ", countsByTable.Select(kvp => $"{kvp.Key}={kvp.Value}"))}]",
            nameof(FormPageTableRoutingService),
            nameof(BuildRoutingAsync));

        return routing;
    }

    private void AppendPageRouting(
        string formName,
        PageConfig page,
        List<TableExceptionModel> routing,
        Dictionary<string, int> countsByTable,
        HashSet<string> seenFields)
    {
        var target = FormPageTargetTableExtensions.ResolvePageTarget(page.TableName, PrimaryTable);
        if (target.Equals(PrimaryTable, StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        if (!FormPageTargetTableExtensions.IsSpecimenFormTarget(target))
        {
            _logWriter.LogInfo(
                $"Form '{formName}' page '{page.Name}' has unsupported TableName '{page.TableName}'; treated as specimen",
                nameof(FormPageTableRoutingService),
                nameof(BuildRoutingAsync));
            return;
        }

        var fields = page.GetFieldList();
        var moreDataCount = 0;
        foreach (var field in fields)
        {
            if (field == null || string.IsNullOrWhiteSpace(field.Id))
            {
                continue;
            }

            if (!_columnResolver.IsMoreDataField(target, field.Id))
            {
                continue;
            }

            var dedupeKey = $"{target}|{field.Id}";
            if (!seenFields.Add(dedupeKey))
            {
                continue;
            }

            routing.Add(new TableExceptionModel { Field = field.Id, Table = target });
            moreDataCount++;
        }

        countsByTable[target] = countsByTable.GetValueOrDefault(target) + moreDataCount;
        _logWriter.LogInfo(
            $"Form '{formName}' page '{page.Name}' target '{target}': {moreDataCount} MoreData field(s)",
            nameof(FormPageTableRoutingService),
            nameof(BuildRoutingAsync));
    }
}
