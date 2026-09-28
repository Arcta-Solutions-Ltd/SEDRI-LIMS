namespace arc.app.Config.Validation;

/// <summary>
/// Result of validating a single entry from a list view <c>tests[]</c> array.
/// </summary>
public class ListViewTestUiEventResolution
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ListViewTestUiEventResolution"/> class.
    /// </summary>
    /// <param name="uiEventName">The stable uievent name from <c>tests[]</c>.</param>
    /// <param name="status">The resolution outcome.</param>
    /// <param name="detail">Optional diagnostic detail (e.g. exception message).</param>
    public ListViewTestUiEventResolution(string uiEventName, ListViewTestUiEventResolutionStatus status, string detail = null)
    {
        UiEventName = uiEventName;
        Status = status;
        Detail = detail;
    }

    /// <summary>Gets the stable uievent name that was validated.</summary>
    public string UiEventName { get; }

    /// <summary>Gets the resolution outcome.</summary>
    public ListViewTestUiEventResolutionStatus Status { get; }

    /// <summary>Gets optional diagnostic detail when resolution failed.</summary>
    public string Detail { get; }
}
