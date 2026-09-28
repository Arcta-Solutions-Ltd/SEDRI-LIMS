using System.Collections.Generic;

namespace arc.domain.Configuration.ViewConfig.Common;

/// <summary>
/// Represents the base configuration for a view.
/// This class provides a structure for defining a view's name, workflow, and associated buttons.
/// </summary>
public abstract class BaseViewConfig
{
    /// <summary>
    /// The name of the view. This is typically used to identify different views in the application.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The workflow associated with the view.
    /// Defines how the view operates within the application’s process.
    /// </summary>
    public string Workflow { get; set; }

    /// <summary>
    /// A collection of button configurations related to this view.
    /// These buttons might define available actions for the user within the view.
    /// </summary>
    public List<ButtonConfig> Buttons { get; set; }
}
