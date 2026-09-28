using arc.domain.Configuration.PagesConfig;
using System.Collections.Generic;
using System.Linq;

namespace arc.domain.Configuration.ViewConfig.Common;

/// <summary>
/// Represents a configuration model for UI buttons.
/// Defines properties that control button behavior, appearance, and workflow interactions.
/// </summary>
public class ButtonConfig
{
    /// <summary>
    /// Unique key to identify the button.
    /// Typically used for mapping actions within the system.
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// The display text shown on the button.
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// The icon associated with the button.
    /// Represents the visual cue for the button's function.
    /// </summary>
    public string Icon { get; set; }

    /// <summary>
    /// Indicates whether the button should trigger an action when selected.
    /// </summary>
    public bool OnSelect { get; set; }

    /// <summary>
    /// Determines if the button applies to batch operations.
    /// Used for actions affecting multiple items at once.
    /// </summary>
    public bool OnBatch { get; set; }

    /// <summary>
    /// Defines behavior when an operation finishes.
    /// Could be used to trigger post-action events or transitions.
    /// </summary>
    public string OnFinish { get; set; }

    /// <summary>
    /// Specifies the primary action associated with the button.
    /// This could link to a predefined workflow step.
    /// </summary>
    public int PrimaryAction { get; set; }

    /// <summary>
    /// UI event associated with the button.
    /// Defines interactions tied to user interface triggers.
    /// </summary>
    public string UIEvent { get; set; }

    /// <summary>
    /// Determines whether the button is part of a workflow.
    /// If true, it interacts with predefined workflow processes.
    /// </summary>
    public bool Workflow { get; set; }

    /// <summary>
    /// Defines workflow entry states associated with the button.
    /// These states help manage button availability based on workflow progression.
    /// </summary>
    public List<EntryStates> WorkflowEntryStates { get; set; } = new List<EntryStates>();

    /// <summary>
    /// List of rules governing button behavior.
    /// These may enforce validation or restrictions on button usage.
    /// </summary>
    public List<RuleConfig> Rules { get; set; }

    /// <summary>
    /// Collection of nested buttons associated with this configuration.
    /// Enables hierarchical button structures.
    /// </summary>
    public IEnumerable<ButtonConfig> Buttons { get; set; }

    /// <summary>
    /// When true, the selected record is used as parent context when opening the form.
    /// Used with PrefillFormFields to pre-fill form fields from the selected record.
    /// </summary>
    public bool AddChildContext { get; set; }

    /// <summary>
    /// Map of form field name to record field name. When AddChildContext is true and a record is selected,
    /// each form field is populated from the corresponding record field. Enables hierarchy views to
    /// pre-fill parent fields (e.g. ParentOrganisationId, ParentOrganisation) without hardcoding.
    /// </summary>
    public Dictionary<string, string> PrefillFormFields { get; set; }

    /// <summary>
    /// Retrieves the UI event list for this button and any nested buttons.
    /// Ensures all relevant UI events are gathered for processing.
    /// </summary>
    /// <returns>A list of UI events linked to this button.</returns>
    internal List<string> GetUIEventList()
    {
        var eventList = new List<string>();

        if (!string.IsNullOrEmpty(UIEvent))
        {
            eventList.Add(UIEvent);
        }

        if (Buttons != null)
        {
            var subEventList = Buttons.Select(b => b.UIEvent).Where(b => b != null).ToList();
            eventList.AddRange(subEventList);
        }

        return eventList;
    }
}

/// <summary>
/// Represents entry states for workflow configurations.
/// Helps manage button availability based on workflow state transitions.
/// </summary>
public class EntryStates
{
    /// <summary>
    /// Unique identifier for the workflow associated with the entry state.
    /// </summary>
    public int WorkflowId { get; set; }

    /// <summary>
    /// Defines the states related to the workflow entry.
    /// Helps determine button actions based on workflow conditions.
    /// </summary>
    public string States { get; set; }
}