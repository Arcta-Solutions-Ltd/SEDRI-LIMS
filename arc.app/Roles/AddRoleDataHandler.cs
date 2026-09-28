using arc.app.Config;
using arc.app.Config.Events;
using arc.app.Config.Sidebar;
using arc.app.Configuration;
using arc.common.Models;
using arc.common.Models.Role;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Roles
{
    public class AddRoleDataHandler : IAddRoleDataHandler
    {
        private readonly IConfigListUtils _configListUtils;
        private readonly IEventAdapter _eventAdapter;
        private readonly ITopicRepository _topicRepository;

        public AddRoleDataHandler(IConfigListUtils configListUtils, IEventAdapter eventAdapter, ITopicRepository topicRepository)
        {
            _configListUtils = configListUtils;
            _eventAdapter = eventAdapter;
            _topicRepository = topicRepository;
        }

        public async Task<string> GetInitialDataAsync()
        {
            var roleDetails = new AddRoleQueryModel { Enabled = "No"};

            var sidebarConfig = new SidebarSetup().GetLeftSidebarConfig();
            var topicTranslations = await _topicRepository.GetTopicsForTranslationAsync();

            var returnConfig = sidebarConfig.GetListOfViews().Select(s => new { s.Name, s.Key });

            var menuListToReturn = new List<CraftedSelectionsModel>();
            foreach (var config in returnConfig)
            {
                var newItem = new CraftedSelectionsModel { Key = config.Key, Name = config.Name, Allowed = "No" };
                menuListToReturn.Add(newItem);
            }

            var sidebarList = sidebarConfig.GetListOfViews();
            var viewList = sidebarList.Select(s => s.Key).ToList();
            viewList.AddRange(["accessionnumberlistview", "generalsettingslistview", "patientreferencelistview"]);
            var listViews = _configListUtils.GetListViewConfigList(viewList, new List<string>());
            var formList = await _configListUtils.GetCompleteFormConfigListAsync();
            var events = formList.Where(f => f != null && f.SaveEvent != null).Select(f => f.SaveEvent).ToList();
            events.AddRange(new[] { "instrumentculture", "specimenprintpreview", "viewspecimenrecord" });

            var eventsToUse = new List<EventPermissionModel>();
            foreach (var ev in events)
            {
                var newEvent = await _eventAdapter.GetEventAsync(ev);
                if (newEvent == null)
                    continue;

                var translatedTopicName = topicTranslations.Where(t => t.listitemname == newEvent.Topic).FirstOrDefault();
                var newItem = new EventPermissionModel { Key = newEvent.EventName.ToLower(), Event = newEvent.EventName.ToLower(), Description = newEvent.Description, TranslatedTopic = translatedTopicName?.displayedtopic, Topic = newEvent.Topic, Allowed = "No" };
                if (!eventsToUse.Exists((e) => e.Event == newItem.Event))
                {
                    eventsToUse.Add(newItem);
                }
            }
            var craftedModels = new List<CraftedModel>();
            craftedModels.Add( new CraftedModel { Name = "menupermission", Contents = JsonConvert.SerializeObject(menuListToReturn.OrderBy(o => o.Name)) });
            craftedModels.Add(new CraftedModel { Name = "eventpermission", Contents = JsonConvert.SerializeObject(eventsToUse.OrderBy(o => o.Event)) });
            roleDetails.Crafted = craftedModels;

            return JsonConvert.SerializeObject(roleDetails);
        }
    }
}
