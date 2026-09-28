using arc.app.Common;
using arc.app.Config.Forms;
using arc.app.Config.UIEvents;
using arc.domain.Configuration.ViewConfig.ListViewConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Config.Validation;

/// <summary>
/// Validates list view <c>tests[]</c> uievent names and removes entries that cannot be resolved at login.
/// </summary>
public class ListViewTestsValidator : IListViewTestsValidator
{
    private readonly IUIEventConfigAdapter _uiEventConfigAdapter;
    private readonly IFormConfigAdapter _formConfigAdapter;
    private readonly ILogWriter _logWriter;

    /// <summary>
    /// Initializes a new instance of the <see cref="ListViewTestsValidator"/> class.
    /// </summary>
    /// <param name="uiEventConfigAdapter">Resolves uievent definitions from code or DB.</param>
    /// <param name="formConfigAdapter">Resolves form definitions linked from form-type uievents.</param>
    /// <param name="logWriter">Logger for production diagnostics.</param>
    public ListViewTestsValidator(
        IUIEventConfigAdapter uiEventConfigAdapter,
        IFormConfigAdapter formConfigAdapter,
        ILogWriter logWriter)
    {
        _uiEventConfigAdapter = uiEventConfigAdapter;
        _formConfigAdapter = formConfigAdapter;
        _logWriter = logWriter;
    }

    /// <inheritdoc />
    public async Task<ListViewTestUiEventResolution> ValidateTestUiEventAsync(string uiEventName)
    {
        if (string.IsNullOrWhiteSpace(uiEventName))
        {
            return new ListViewTestUiEventResolution(uiEventName, ListViewTestUiEventResolutionStatus.InvalidUiEvent, "Empty uievent name");
        }

        try
        {
            var uiEvent = await _uiEventConfigAdapter.GetEventAsync(uiEventName).ConfigureAwait(false);
            if (uiEvent == null)
            {
                return new ListViewTestUiEventResolution(uiEventName, ListViewTestUiEventResolutionStatus.MissingUiEvent);
            }

            if (!uiEvent.Type.Equals("form", StringComparison.OrdinalIgnoreCase))
            {
                return new ListViewTestUiEventResolution(uiEventName, ListViewTestUiEventResolutionStatus.Resolved);
            }

            if (string.IsNullOrWhiteSpace(uiEvent.Action))
            {
                return new ListViewTestUiEventResolution(
                    uiEventName,
                    ListViewTestUiEventResolutionStatus.MissingForm,
                    "Form-type uievent has no action");
            }

            var form = await _formConfigAdapter.GetFormAsync(uiEvent.Action).ConfigureAwait(false);
            if (form == null || string.IsNullOrWhiteSpace(form.SaveEvent))
            {
                return new ListViewTestUiEventResolution(
                    uiEventName,
                    ListViewTestUiEventResolutionStatus.MissingForm,
                    $"Linked form '{uiEvent.Action}' not found or has no saveEvent");
            }

            return new ListViewTestUiEventResolution(uiEventName, ListViewTestUiEventResolutionStatus.Resolved);
        }
        catch (Exception ex)
        {
            return new ListViewTestUiEventResolution(
                uiEventName,
                ListViewTestUiEventResolutionStatus.InvalidUiEvent,
                ex.Message);
        }
    }

    /// <inheritdoc />
    public async Task SanitizeViewTestsAsync(ListViewConfig view)
    {
        if (view?.Tests == null || view.Tests.Count == 0)
        {
            return;
        }

        var kept = new List<string>();
        var removed = new List<string>();

        foreach (var uiEventName in view.Tests)
        {
            var resolution = await ValidateTestUiEventAsync(uiEventName).ConfigureAwait(false);
            if (resolution.Status == ListViewTestUiEventResolutionStatus.Resolved)
            {
                kept.Add(uiEventName);
                continue;
            }

            removed.Add(uiEventName);
            LogRemoval(view.Name, resolution);
        }

        view.Tests = kept;

        if (removed.Count > 0)
        {
            _logWriter.LogInfo(
                $"List view '{view.Name}': sanitized tests[] — removed {removed.Count} orphan uievent(s): {string.Join(", ", removed)}",
                nameof(ListViewTestsValidator),
                nameof(SanitizeViewTestsAsync));
        }
    }

    private void LogRemoval(string viewName, ListViewTestUiEventResolution resolution)
    {
        var viewLabel = string.IsNullOrWhiteSpace(viewName) ? "(unknown view)" : viewName;

        switch (resolution.Status)
        {
            case ListViewTestUiEventResolutionStatus.MissingUiEvent:
                _logWriter.LogInfo(
                    $"List view '{viewLabel}': removed orphan test uievent '{resolution.UiEventName}' — not resolvable in code or DB.",
                    nameof(ListViewTestsValidator),
                    nameof(SanitizeViewTestsAsync));
                break;
            case ListViewTestUiEventResolutionStatus.MissingForm:
                _logWriter.LogInfo(
                    $"List view '{viewLabel}': removed orphan test uievent '{resolution.UiEventName}' — linked form not found{(resolution.Detail != null ? $" ({resolution.Detail})" : string.Empty)}.",
                    nameof(ListViewTestsValidator),
                    nameof(SanitizeViewTestsAsync));
                break;
            default:
                _logWriter.LogError(
                    $"List view '{viewLabel}': removed orphan test uievent '{resolution.UiEventName}' — exception during resolution{(resolution.Detail != null ? $": {resolution.Detail}" : string.Empty)}.",
                    nameof(ListViewTestsValidator),
                    nameof(SanitizeViewTestsAsync));
                break;
        }
    }
}
