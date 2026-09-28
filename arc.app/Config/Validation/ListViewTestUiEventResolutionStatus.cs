namespace arc.app.Config.Validation;

/// <summary>
/// Outcome of resolving a list-view <c>tests[]</c> uievent name against code and DB configuration.
/// </summary>
public enum ListViewTestUiEventResolutionStatus
{
    /// <summary>The uievent and linked form (when required) resolve successfully.</summary>
    Resolved,

    /// <summary>No uievent configuration exists in code or DB.</summary>
    MissingUiEvent,

    /// <summary>The uievent exists but the linked form is missing or has no save event.</summary>
    MissingForm,

    /// <summary>An exception occurred while resolving the uievent or form.</summary>
    InvalidUiEvent
}
