using arc.app.Config.Forms;
using arc.app.Config.Pages;
using arc.app.Config.UIEvents;
using arc.domain.Configuration.FormsConfig;
using arc.domain.Configuration.PagesConfig;
using arc.domain.Configuration.UIEventsConfig;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Config
{
    /// <summary>
    /// Represents a list of form configurations, including associated page configurations and UI events.
    /// </summary>
    public class FormConfigList : IFormConfigList
    {
        private readonly IFormConfigAdapter _formConfigAdapter;
        private readonly IPageConfigAdapter _pageConfigAdapter;
        private readonly IUIEventConfigAdapter _eventConfigAdapter;
        private readonly IPageConfigAdapter _pageAdapter;
        private readonly ILogger<FormConfigList> _logger;

        private List<string> _pageList;
        private List<UIEventConfig> _uiEventsToDisplay;
        private List<FormConfig> _formsToDisplay;

        // Tracks which pages have already been built so repeated GetPagesAsync calls within a
        // request only process newly-added pages instead of re-loading every page each time.
        private readonly HashSet<string> _processedPages = new(StringComparer.OrdinalIgnoreCase);
        private readonly List<PageConfig> _pagesToDisplay = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="FormConfigList"/> class with the specified adapters.
        /// </summary>
        /// <param name="formConfigAdapter">The adapter for form configurations.</param>
        /// <param name="pageConfigAdapter">The adapter for page configurations.</param>
        /// <param name="eventConfigAdapter">The adapter for UI event configurations.</param>
        /// <param name="logger">Logger for diagnostic messages.</param>
        public FormConfigList(IFormConfigAdapter formConfigAdapter, IPageConfigAdapter pageConfigAdapter, IUIEventConfigAdapter eventConfigAdapter, IPageConfigAdapter pageAdapter, ILogger<FormConfigList> logger)
        {
            _formConfigAdapter = formConfigAdapter;
            _pageConfigAdapter = pageConfigAdapter;
            _eventConfigAdapter = eventConfigAdapter;
            _logger = logger;

            _pageList = new List<string>();
            _uiEventsToDisplay = new List<UIEventConfig>();
            _formsToDisplay = new List<FormConfig>();
            _pageAdapter = pageAdapter;
        }

        /// <summary>
        /// Loads and processes forms based on the provided UI event list and allowed event list.
        /// </summary>
        /// <param name="uiEventList">The list of UI event names to load.</param>
        /// <param name="allowedEventList">The list of allowed event names.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task LoadFormsAsync(List<string> uiEventList, List<string> allowedEventList)
        {
            var uiEvents = LoadUiEvents(uiEventList);

            foreach (var ev in uiEvents)
            {
                try
                {
                    if (ev.Type.Equals("form", StringComparison.OrdinalIgnoreCase))
                    {
                        var newForm = await _formConfigAdapter.GetFormAsync(ev.Action);
                        if (newForm?.SaveEvent != null)
                        {
                            if (IsAllowedFormForUser(newForm, allowedEventList))
                            {
                                if (_formsToDisplay.Exists(f => string.Equals(f.Name, newForm.Name, StringComparison.OrdinalIgnoreCase)))
                                {
                                    continue;
                                }
                                _pageList.AddRange(newForm.Pages);
                                _formsToDisplay.Add(newForm);
                                _uiEventsToDisplay.Add(ev);
                            }
                        }
                        else
                        {
                            _logger.LogWarning(
                                "Config load: skipped uievent '{UiEventName}' — linked form '{FormName}' not found",
                                ev.Name,
                                ev.Action);
                        }
                    }
                    else
                    {
                        // Check permissions for view-record type UI events
                        if (ev.Type.Equals("view-record", StringComparison.OrdinalIgnoreCase) &&
                            ev.Name.Equals("viewspecimenrecord", StringComparison.OrdinalIgnoreCase))
                        {
                            // Only add if permission is granted
                            if (allowedEventList.Any(s => s.Equals("viewspecimenrecord", StringComparison.OrdinalIgnoreCase)))
                            {
                                _uiEventsToDisplay.Add(ev);
                            }
                        }
                        else
                        {
                            // Add other non-form types as before (diary, printpreview, etc.)
                            _uiEventsToDisplay.Add(ev);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Config load: exception loading form for uievent '{UiEventName}'",
                        ev.Name);
                }
            }
        }

        /// <summary>
        /// Whether the user may load this form: direct save-event permission, or expert-rule embedded add forms when user can edit expert rules.
        /// </summary>
        private static bool IsAllowedFormForUser(FormConfig newForm, List<string> allowedEventList)
        {
            if (newForm?.SaveEvent == null)
            {
                return false;
            }

            // Barcode label field titles use GetFieldsForForm(ReferenceForm); these forms must be in client config
            // for every user who can print labels, not only users with newreceivedspecimen (workflow remains gated elsewhere).
            if (string.Equals(newForm.Name, "createspecimenreceivedform", StringComparison.OrdinalIgnoreCase)
                || string.Equals(newForm.Name, "createspecimenreceivedforpatientform", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (allowedEventList.Any(s => s.Equals(newForm.SaveEvent, StringComparison.OrdinalIgnoreCase)))
            {
                return true;
            }

            // Embedded expert-rule record lists (conditions, actions, test conditions, approvals) use add* save events
            // that are often not granted separately; users with editexpertrule should still open these add forms.
            var save = newForm.SaveEvent;
            var embeddedExpertRuleAdds = new[]
            {
                "addexpertrulecondition",
                "addexpertruleaction",
                "addexpertruletestcondition",
                "addexpertruleapproval"
            };
            if (!embeddedExpertRuleAdds.Any(x => x.Equals(save, StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }

            return allowedEventList.Any(s => s.Equals("editexpertrule", StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Asynchronously retrieves the complete list of form configurations based on the provided UI event list.
        /// </summary>
        /// <param name="uiEventList">The list of UI event names to load forms for.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the list of form configurations.</returns>
        public async Task<List<FormConfig>> GetCompleteFormListAsync(List<string> uiEventList)
        {
            _uiEventsToDisplay = LoadUiEvents(uiEventList);
            var formList = new List<FormConfig>();

            var eventList = _uiEventsToDisplay.Where(f => f.Type.Equals("form", StringComparison.OrdinalIgnoreCase));
            foreach (var ev in eventList)
            {
                var newForm = await _formConfigAdapter.GetFormAsync(ev.Action);
                if (newForm != null)
                {
                    formList.Add(newForm);
                }

                if (newForm?.Pages != null)
                {
                    foreach (var page in newForm.Pages)
                    {
                        var nextPage = await _pageAdapter.GetPageAsync(page);
                        if (nextPage != null)
                        {
                            formList.AddRange(await GetCompleteFormListAsync(nextPage.GetUIEvents()));
                        }
                        else
                        {
                            _logger.LogWarning("Page config is missing for page '{PageName}' referenced by form '{FormName}'", page, newForm.Name ?? ev.Action);
                        }
                    }
                }
            }

            return formList;
        }

        /// <summary>
        /// Loads UI events based on the provided UI event list, skipping names that cannot be resolved.
        /// Unresolvable or failing uievents are logged and omitted from the result.
        /// </summary>
        /// <param name="uiEventList">The list of UI event names to load.</param>
        /// <returns>A list of UIEventConfig objects representing the loaded UI events.</returns>
        private List<UIEventConfig> LoadUiEvents(List<string> uiEventList)
        {
            var uiEventsToDisplay = new List<UIEventConfig>();
            var listOfEvents = uiEventList.Distinct().OrderBy(e => e).ToArray();

            foreach (var ev in listOfEvents)
            {
                try
                {
                    var newEvent = _eventConfigAdapter.GetEventAsync(ev).Result;
                    if (newEvent != null)
                    {
                        uiEventsToDisplay.Add(newEvent);
                    }
                    else
                    {
                        _logger.LogWarning("Config load: skipped uievent '{UiEventName}' — not resolvable in code or DB", ev);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Config load: exception resolving uievent '{UiEventName}'", ev);
                }
            }

            return uiEventsToDisplay;
        }

        /// <summary>
        /// Gets the list of UI events to display.
        /// </summary>
        /// <returns>A list of UIEventConfig objects representing the UI events to display.</returns>
        public List<UIEventConfig> GetUiEvents()
        {
            return _uiEventsToDisplay;
        }

        /// <summary>
        /// Gets the list of forms to display.
        /// </summary>
        /// <returns>A list of FormConfig objects representing the forms to display.</returns>
        public List<FormConfig> GetForms()
        {
            return _formsToDisplay;
        }

        /// <summary>
        /// Asynchronously retrieves the list of pages to display.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the list of PageConfig objects representing the pages to display.</returns>
        public async Task<(List<PageConfig>, List<string>)> GetPagesAsync()
        {
            var uiEvents = new List<string>();

            foreach (var page in _pageList.Distinct())
            {
                // Skip pages already built on a previous call so the work is not repeated.
                if (!_processedPages.Add(page))
                {
                    continue;
                }

                var newPage = await _pageConfigAdapter.GetPageAsync(page);
                if (newPage == null)
                {
                    continue;
                }

                _pagesToDisplay.Add(newPage);

                var newUIEvents = newPage.GetUIEvents();
                uiEvents.AddRange(newUIEvents);
                _uiEventsToDisplay.AddRange(LoadUiEvents(newUIEvents));
            }

            return (_pagesToDisplay.ToList(), uiEvents);
        }
    }
}



/// <summary>
/// Loads and processes forms based on the provided UI event list and allowed event list.
/// </summary>
/// <param name="uiEventList">The list of UI event names to load.</param>
/// <param name="allowedEventList">The list of allowed event names.</param>
/// <returns>A task representing the asynchronous operation.</returns>
//public async Task LoadForms(List<string> uiEventList, List<string> allowedEventList)
//{
//    var uiEvents = LoadUiEvents(uiEventList);

//    foreach (var ev in uiEvents)
//    {
//        if (ev.Type.Equals("form", StringComparison.OrdinalIgnoreCase))
//        {
//            var newForm = await _formConfigAdapter.GetFormAsync(ev.Action);
//            if (newForm?.SaveEvent != null)
//            {
//                if (allowedEventList.Any(s => s.Equals(newForm.SaveEvent, StringComparison.OrdinalIgnoreCase)))
//                {
//                    _pageList.AddRange(newForm.Pages);
//                    _formsToDisplay.Add(newForm);
//                    _uiEventsToDisplay.Add(ev);
//                }
//            }
//        }
//        else
//        {
//            _uiEventsToDisplay.Add(ev);
//        }
//    }
//}