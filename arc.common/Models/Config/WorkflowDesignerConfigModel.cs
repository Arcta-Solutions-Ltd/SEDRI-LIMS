using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace arc.common.Models.Config;

/// <summary>
/// Everything the workflow designer needs to render one workflow: the stored workflow document and
/// the read-only lookups used to turn the ids inside it into something a human can read.
/// </summary>
/// <remarks>
/// The workflow itself is carried as a <see cref="JObject"/> rather than a typed
/// <c>WorkflowConfig</c> for two reasons. First, arc.common cannot reference arc.domain without a
/// circular project reference. Second, and more importantly, a typed round trip would rewrite the
/// stored document: property casing would change, defaulted properties would be added, and anything
/// the runtime model does not know about would be silently dropped. The designer only edits the
/// steps, so the document is loaded and saved verbatim apart from the parts the user changed.
/// </remarks>
public class WorkflowDesignerConfigModel
{
    /// <summary>
    /// Gets or sets the configs table identity of the workflow record. This is what the save matches on.
    /// </summary>
    public int ConfigId { get; set; }

    /// <summary>
    /// Gets or sets the configuration name of the workflow record, used only for logging and as a
    /// fallback when no identity is available.
    /// </summary>
    public string ConfigName { get; set; }

    /// <summary>
    /// Gets or sets the workflow document exactly as stored in configs.contents.
    /// </summary>
    public JObject Workflow { get; set; }

    /// <summary>
    /// Gets or sets the reference data the designer needs to display the workflow. Never written back.
    /// </summary>
    public WorkflowSupportingConfigModel SupportingConfig { get; set; } = new();
}

/// <summary>
/// Read-only lookups supplied to the workflow designer so it can display state ids, event names and
/// condition values as text. The designer never sends these back, so it cannot corrupt listitem or
/// the event configuration.
/// </summary>
public class WorkflowSupportingConfigModel
{
    /// <summary>
    /// Gets or sets the states of the workflow, taken from the listitem rows of the list named by the
    /// workflow's StatesList. Key is the listitem id; Value is its text.
    /// </summary>
    public List<WorkflowOptionModel> StateConfig { get; set; } = [];

    /// <summary>
    /// Gets or sets the events referenced by the workflow, with the fields their payloads carry so
    /// transition conditions can be built against real field ids.
    /// </summary>
    public List<WorkflowEventConfigModel> EventConfig { get; set; } = [];

    /// <summary>
    /// Gets or sets the option lists referenced by any event field, so a condition value can be
    /// picked from a list rather than typed.
    /// </summary>
    public List<WorkflowListConfigModel> ListConfig { get; set; } = [];
}

/// <summary>
/// A single id and its display text. Matching is always on <see cref="Key"/>; <see cref="Value"/> is
/// display only and may have been translated.
/// </summary>
public class WorkflowOptionModel
{
    /// <summary>
    /// Gets or sets the stable identifier, for example a listitem id.
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// Gets or sets the display text for the key.
    /// </summary>
    public string Value { get; set; }
}

/// <summary>
/// One event that appears in the workflow, with the fields available to build conditions from.
/// </summary>
public class WorkflowEventConfigModel
{
    /// <summary>
    /// Gets or sets the event name as it appears on a workflow step. Matched case-insensitively.
    /// </summary>
    public string EventName { get; set; }

    /// <summary>
    /// Gets or sets the human readable description of the event.
    /// </summary>
    public string EventDescription { get; set; }

    /// <summary>
    /// Gets or sets the fields carried by the event payload.
    /// </summary>
    public List<WorkflowEventFieldModel> Fields { get; set; } = [];
}

/// <summary>
/// One field of an event payload, as offered to the designer when building a transition condition.
/// </summary>
public class WorkflowEventFieldModel
{
    /// <summary>
    /// Gets or sets the field id as it appears in the stored event payload. This is what a condition
    /// is written against, so it must be the id and never the label.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the display label for the field, which may be a @Tag@ language token.
    /// </summary>
    public string Label { get; set; }

    /// <summary>
    /// Gets or sets the editor type hint: list, date or text.
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Gets or sets the name of the option list supplying this field's values, when it has one.
    /// </summary>
    public string OptionsName { get; set; }
}

/// <summary>
/// An option list referenced by an event field.
/// </summary>
public class WorkflowListConfigModel
{
    /// <summary>
    /// Gets or sets the list name referenced by <see cref="WorkflowEventFieldModel.OptionsName"/>.
    /// </summary>
    public string OptionsName { get; set; }

    /// <summary>
    /// Gets or sets the options in the list.
    /// </summary>
    public List<WorkflowOptionModel> Contents { get; set; } = [];
}
