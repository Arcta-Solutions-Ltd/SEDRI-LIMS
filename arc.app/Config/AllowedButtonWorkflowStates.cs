using arc.app.Configuration;
using arc.common.ExtensionMethods;
using arc.common.Models.Laboratory;
using arc.domain.Configuration.FormsConfig;
using arc.domain.Configuration.UIEventsConfig;
using arc.domain.Configuration.ViewConfig.Common;
using arc.domain.Configuration.ViewConfig.ListViewConfig;
using arc.domain.Configuration.WorkflowsConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Config;

/// <summary>
/// Manages allowed workflow states for buttons within a view.
/// Ensures buttons are configured according to UI events, forms, and laboratory settings.
/// </summary>
public class AllowedButtonWorkflowStates : IAllowedButtonWorkflowStates
{
    private readonly IWorkflowHandler _workflowHandler;

    /// <summary>
    /// Initializes a new instance of the AllowedButtonWorkflowStates class.
    /// </summary>
    /// <param name="workflowHandler">An instance of IWorkflowHandler for managing workflow operations.</param>
    public AllowedButtonWorkflowStates(IWorkflowHandler workflowHandler)
    {
        _workflowHandler = workflowHandler;
    }

    /// <summary>
    /// Adds allowed workflow states to buttons within the specified views.
    /// For views named "specimens", "cultures", or "specimenrecordview", all registered workflows
    /// are iterated and their entry states are applied to both the regular button list and the
    /// <see cref="ListViewConfig.GroupMenu"/> items so that group-header actions (e.g. Add Isolate,
    /// Edit Culture, Delete Culture) are hidden by the client when the specimen is not in an
    /// eligible workflow state.
    /// </summary>
    /// <typeparam name="T">
    /// A type constrained to <see cref="BaseViewConfig"/>. Pass <see cref="ListViewConfig"/> when
    /// the views contain a <c>GroupMenu</c> that should also be workflow-gated.
    /// </typeparam>
    /// <param name="views">The list of view configurations whose buttons need workflow state population.</param>
    /// <param name="allowedUIEvents">
    /// The UI events that are permitted for the current user. Only buttons whose
    /// <c>UIEvent</c> matches an entry in this list will receive workflow entry states.
    /// </param>
    /// <param name="allowedForms">
    /// The forms available to the current user. Each form's <c>SaveEvent</c> is matched
    /// against the workflow steps to resolve the correct entry states for the button.
    /// </param>
    /// <returns>A task that resolves to the mutated list of views with populated <c>WorkflowEntryStates</c>.</returns>
    public async Task<List<T>> AddAllowedStatesToButtonsInView<T>(
        List<T> views,
        List<UIEventConfig> allowedUIEvents,
        List<FormConfig> allowedForms) where T : BaseViewConfig
    {
        foreach (var view in views)
        {
            if (view.Name == "specimens" || view.Name == "cultures" || view.Name.IsSameAs("specimenrecordview"))
            {
                var workflows = await _workflowHandler.GetWorkflowListAsync();

                foreach (var workflow in workflows)
                {
                    view.Buttons = UpdateButtons(view.Buttons, allowedUIEvents, allowedForms, workflow.Steps, workflow.Id);

                    // Also populate WorkflowEntryStates for GroupMenu items so that the client-side
                    // FilterGroupMenuOnState utility can hide group-header buttons (e.g. Add Isolate,
                    // Edit Culture, Delete Culture) when the specimen is in a post-submission state.
                    if (view is ListViewConfig listView && listView.GroupMenu?.Any() == true)
                    {
                        listView.GroupMenu = UpdateButtons(
                            listView.GroupMenu, allowedUIEvents, allowedForms, workflow.Steps, workflow.Id);
                    }
                }
            }
            else
            {
                // If a workflow is explicitly defined for the view, retrieve and update buttons
                if (!string.IsNullOrEmpty(view.Workflow))
                {
                    var workflow = await _workflowHandler.GetSingleWorkflowAsync(view.Workflow);
                    if (workflow != null)
                    {
                        view.Buttons = UpdateButtons(view.Buttons, allowedUIEvents, allowedForms, workflow.Steps, 1);

                        if (view is ListViewConfig listView && listView.GroupMenu?.Any() == true)
                        {
                            listView.GroupMenu = UpdateButtons(
                                listView.GroupMenu, allowedUIEvents, allowedForms, workflow.Steps, 1);
                        }
                    }
                }
            }
        }

        return views;
    }

    /// <summary>
    /// Populates <see cref="ButtonConfig.WorkflowEntryStates"/> for every button in
    /// <paramref name="buttons"/> that has <see cref="ButtonConfig.Workflow"/> set to <c>true</c>.
    /// Buttons without a matching UI event or form are excluded from the result so that the client
    /// only receives actions the user is authorised to perform.
    /// </summary>
    /// <param name="buttons">
    /// The list of button configurations to process. This may be the view's regular
    /// <c>Buttons</c> collection or its <c>GroupMenu</c> collection.
    /// </param>
    /// <param name="allowedUIEvents">
    /// The UI events permitted for the current user. A button's <c>UIEvent</c> must appear here
    /// for the button to be included in the returned list.
    /// </param>
    /// <param name="allowedForms">
    /// The forms available to the current user. The form's <c>SaveEvent</c> is matched against
    /// workflow steps to determine which specimen states permit the button's action.
    /// </param>
    /// <param name="steps">
    /// The ordered list of workflow step definitions for the active workflow. Each step carries an
    /// <c>EntryState</c> (comma-separated state IDs) that becomes the button's allowed state list.
    /// </param>
    /// <param name="workflowId">
    /// The numeric identifier of the workflow being processed. Stored on each
    /// <see cref="EntryStates"/> record so the client can select the correct state list when
    /// multiple workflows are in use for a specimen type.
    /// </param>
    /// <returns>
    /// A new list containing only the buttons that are either not workflow-controlled, or are
    /// workflow-controlled and have a matching UI event, form, and workflow step.
    /// </returns>
    private List<ButtonConfig> UpdateButtons(
        List<ButtonConfig> buttons,
        List<UIEventConfig> allowedUIEvents,
        List<FormConfig> allowedForms,
        List<StepItemConfig> steps,
        int workflowId)
    {
        // Filter buttons that are NOT controlled by workflow
        var returnButtonList = buttons.Where(b => !b.Workflow).ToList();

        // Process workflow-controlled buttons
        var buttonsControlledByWorkflowList = buttons.Where(b => b.Workflow).ToList();

        foreach (var button in buttonsControlledByWorkflowList)
        {
            var allowedUIEvent = allowedUIEvents.FirstOrDefault(e => e.Name.Equals(button.UIEvent, StringComparison.CurrentCultureIgnoreCase));

            if (allowedUIEvent != null)
            {
                var form = allowedForms.FirstOrDefault(f => f.Name == allowedUIEvent.Action);
                if (form != null)
                {
                    var stepResult = steps.Where(s => s.Event.Equals(form.SaveEvent, StringComparison.OrdinalIgnoreCase));
                    if (stepResult.Any())
                    {
                        var step = stepResult.First();
                        var newEntryStates = new EntryStates { WorkflowId = workflowId, States = step.EntryState };
                        button.WorkflowEntryStates.Add(newEntryStates);
                    }
                    returnButtonList.Add(button);
                }
            }
        }

        return returnButtonList;
    }
}
