using System.Collections.Generic;
using System.Linq;
using arc.domain.Configuration.UIEventsConfig;
using arc.domain.Configuration.ViewConfig.Common;

namespace arc.domain.Configuration.ViewConfig.RecordViewConfig;

/// <summary>
/// Represents the configuration for a record view, including metadata and regions.
/// </summary>
public class RecordViewConfig : BaseViewConfig
{
    /// <summary>
    /// Gets or sets the unique identifier for the record view configuration.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the record view.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the name of a single item in the record view.
    /// </summary>
    public string SingleItemName { get; set; }

    /// <summary>
    /// Gets or sets the type of the record view.
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Gets or sets the single query associated with the record view.
    /// </summary>
    public string SingleQuery { get; set; }

    /// <summary>
    /// When true, refreshes the record view's standard regions when an embedded form saves with OnFinish: 'embeddedrefresh'.
    /// Use when embedded list actions can update parent record data (e.g. breakpoint Enabled when rejection is saved).
    /// </summary>
    public bool RefreshRecordOnEmbeddedSave { get; set; }

    /// <summary>
    /// Gets or sets the list of region configurations in the record view.
    /// </summary>
    public List<RegionConfig> Regions { get; set; }

    /// <summary>
    /// Removes buttons from the view configuration that are not included in the given UI event list.
    /// The test record view Edit button is kept: its UI event is resolved from <c>TestName</c> on the client (same as <c>DirectTestsList</c>).
    /// </summary>
    /// <param name="uiEvents">The list of allowed UI events.</param>
    public void RemoveButtonsNotInUIEventList(List<UIEventConfig> uiEvents)
    {
        if (Buttons == null)
            return;

        var newButtonList = new List<ButtonConfig>();
        foreach (var button in Buttons)
        {
            if (Name != null
                && Name.Equals("testrecordview", System.StringComparison.OrdinalIgnoreCase)
                && button.Key != null
                && button.Key.Equals("edittest", System.StringComparison.OrdinalIgnoreCase))
            {
                newButtonList.Add(button);
                continue;
            }

            if (button.UIEvent != null)
            {
                bool allowed = uiEvents.Any(s => s.Name.ToLower() == button.UIEvent.ToLower());
                if (allowed)
                {
                    newButtonList.Add(button);
                }
            }
        }
        Buttons = newButtonList;
    }
}

