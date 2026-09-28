using arc.app.Config;
using arc.app.Config.Events;
using arc.app.Config.Sidebar;
using arc.app.Configuration;
using arc.common.Models;
using arc.common.Models.Role;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Configuration.SidebarConfig;
using arc.domain.Security.Permission;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Roles
{
    /// <summary>
    /// Handles event permissions for roles.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="EventPermissionHandler"/> class with the specified dependencies.
    /// </remarks>
    /// <param name="roleRepository">The repository for role data.</param>
    /// <param name="topicRepository">The repository for topic data.</param>
    /// <param name="configListUtils">Utilities for handling configuration lists.</param>
    /// <param name="eventAdapter">The adapter for event configurations.</param>
    /// <param name="logger">The logger for logging information.</param>
    public class EventPermissionHandler(
        IRoleRepository roleRepository,
        ITopicRepository topicRepository,
        IConfigListUtils configListUtils,
        IEventAdapter eventAdapter,
        ILogger logger) : IEventPermissionHandler
    {
        private readonly IRoleRepository _roleRepository = roleRepository;
        private readonly ITopicRepository _topicRepository = topicRepository;
        private readonly IConfigListUtils _configListUtils = configListUtils;
        private readonly IEventAdapter _eventAdapter = eventAdapter;
        private readonly ILogger _logger = logger;
        private static readonly string[] _collection = ["accessionnumberlistview", "generalsettingslistview", "patientreferencelistview"];

        /// <summary>
        /// Asynchronously retrieves event permissions for a role based on the specified query filters.
        /// </summary>
        /// <param name="queryFilters">The query filters to use for retrieving the role details.</param>
        /// <returns>A JSON string representing the event permissions for the role.</returns>
        public async Task<string> GetEventPermissionsForRoleAsync(QueryFilterConfig queryFilters)
        {
            var roleDetails = await _roleRepository.GetRoleByIdAsync(int.Parse(queryFilters.Parameters[0].Value));

            var sidebarConfig = new SidebarSetup().GetLeftSidebarConfig();
            var eventsToUse = await PopulateEventPermissionModelAsync(sidebarConfig, roleDetails);

            var craftedModels = new List<CraftedModel>
        {
            new CraftedModel { Name = "eventpermission", Contents = JsonConvert.SerializeObject(eventsToUse.OrderBy(o => o.Event)) }
        };

            return JsonConvert.SerializeObject(new JustCraftedPages { Crafted = craftedModels });
        }

        /// <summary>
        /// Asynchronously populates the event permission model based on the sidebar configuration and role details.
        /// </summary>
        /// <param name="sidebarConfig">The sidebar configuration to use.</param>
        /// <param name="roleDetails">The details of the role to populate event permissions for.</param>
        /// <returns>A list of <see cref="EventPermissionModel"/> objects representing the event permissions for the role.</returns>
        public async Task<List<EventPermissionModel>> PopulateEventPermissionModelAsync(SidebarConfig sidebarConfig, Role roleDetails)
        {
            var sidebarList = sidebarConfig.GetListOfViews();
            var topicTranslations = await _topicRepository.GetTopicsForTranslationAsync();
            var viewList = sidebarList.Select(s => s.Key).ToList();

            if (viewList.Contains("settings"))
            {
                viewList.Remove("settings");
                viewList.AddRange(_collection);
            }

            var listViews = _configListUtils.GetListViewConfigList(viewList, new List<string>());
            var formList = await _configListUtils.GetCompleteFormConfigListAsync();

            var events = formList.Where(f => f?.SaveEvent != null).Select(f => f.SaveEvent).ToList();

            events.AddRange(new[] { "instrumentculture", "specimenprintpreview", "viewspecimenrecord" });

            var eventsToUse = new List<EventPermissionModel>();

            foreach (var ev in events)
            {
                var newEvent = await _eventAdapter.GetEventAsync(ev);

                if (newEvent == null)
                {
                    _logger.LogError("Could not find event {0} in configurations", ev);
                    continue;
                }

                var found = roleDetails.EventPermissionDetails?.AllowedEvents?
                    .Any(r => r.Equals(newEvent.EventName, StringComparison.OrdinalIgnoreCase)) ?? false;

                var translatedTopicName = topicTranslations.FirstOrDefault(t => t.listitemname == newEvent.Topic)?.displayedtopic;
                var newItem = new EventPermissionModel
                {
                    Key = newEvent.EventName.ToLower(),
                    Event = newEvent.EventName.ToLower(),
                    Description = newEvent.Description,
                    Topic = newEvent.Topic,
                    TranslatedTopic = translatedTopicName,
                    Allowed = found ? "Yes" : "No"
                };

                if (eventsToUse.All(e => e.Event != newItem.Event))
                {
                    eventsToUse.Add(newItem);
                }
            }

            return eventsToUse;
        }
    }
}
