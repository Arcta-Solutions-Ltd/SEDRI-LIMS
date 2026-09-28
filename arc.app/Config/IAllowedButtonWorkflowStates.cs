using arc.common.Models.Laboratory;
using arc.domain.Configuration.FormsConfig;
using arc.domain.Configuration.UIEventsConfig;
using arc.domain.Configuration.ViewConfig.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Config;

/// <summary>
/// Defines an interface for managing allowed workflow states for buttons within a view.
/// This ensures buttons have proper configurations based on UI events, forms, and laboratory settings.
/// </summary>
public interface IAllowedButtonWorkflowStates
{
    /// <summary>
    /// Adds allowed workflow states to buttons within the given views.
    /// This method ensures that each button's state is properly configured based on the provided UI events,
    /// allowed forms, and laboratory configurations.
    /// </summary>
    /// <typeparam name="T">A type parameter constrained to BaseViewConfig, representing the views to be updated.</typeparam>
    /// <param name="views">A list of views that require workflow state modifications.</param>
    /// <param name="allowedUIEvents">A list of UI event configurations that define allowed interactions.</param>
    /// <param name="allowedForms">A list of form configurations that determine associated workflows.</param>
    /// <param name="laboratoryConfigs">Configuration details for the laboratory, influencing button behavior.</param>
    /// <returns>A task that resolves to a list of updated views with the allowed button states applied.</returns>
    Task<List<T>> AddAllowedStatesToButtonsInView<T>(
        List<T> views,
        List<UIEventConfig> allowedUIEvents,
        List<FormConfig> allowedForms
    ) where T : BaseViewConfig;
}
