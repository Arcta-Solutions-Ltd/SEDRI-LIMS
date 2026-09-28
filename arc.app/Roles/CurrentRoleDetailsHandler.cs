using arc.app.Config.Sidebar;
using arc.app.Configuration;
using arc.common.Models.Viewer;
using arc.domain.Configuration.QueryFiltersConfig;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Roles
{
    public class CurrentRoleDetailsHandler : ICurrentRoleDetailsHandler
    {
        private readonly IRoleRepository _roleRepository;
        private readonly ITopicRepository _topicRepository;
        private readonly IMenuPermissionHandler _menuPermissionHandler;
        private readonly IEventPermissionHandler _eventPermissionHandler;

        public CurrentRoleDetailsHandler(IRoleRepository roleRepository, ITopicRepository topicRepository, IMenuPermissionHandler menuPermissionHandler, IEventPermissionHandler eventPermissionHandler)
        {
            _roleRepository = roleRepository;
            _topicRepository = topicRepository;
            _menuPermissionHandler = menuPermissionHandler;
            _eventPermissionHandler = eventPermissionHandler;
        }

        public async Task<string> HandleAsync(QueryFilterConfig queryFilters)
        {
            var roleDetails = await _roleRepository.GetRoleByIdAsync(int.Parse(queryFilters.Parameters[0].Value));

            var topicTranslations = await _topicRepository.GetTopicsForTranslationAsync();

            var sidebarConfig = new SidebarSetup().GetLeftSidebarConfig();

            var menuListToReturn = _menuPermissionHandler.PopulatePermissionModelList(sidebarConfig, roleDetails);

            var eventsToUse = await _eventPermissionHandler.PopulateEventPermissionModelAsync(sidebarConfig, roleDetails);

            var roleSectionOne = new ViewerSectionModel
            {
                Id = "general",
                Title = "@GenGenA@",
                Fields = new List<ViewerFieldModel>
                {
                    new ViewerFieldModel { Id = "rolename", Label = "@RolRolA@", Value = roleDetails.RoleName },
                    new ViewerFieldModel { Id = "roledescription", Label = "@RolRol@", Value = roleDetails.RoleDescription },
                    new ViewerFieldModel { Id = "enabled", Label = "@GenEna@", Value = roleDetails.Enabled }
                }
            };

            var fieldList = new List<ViewerFieldModel>();
            foreach(var item in menuListToReturn)
            {
                var newField = new ViewerFieldModel { Id = item.Key, Label = item.Name, Value = item.Allowed };
                fieldList.Add(newField);
            }
            var roleSectionTwo = new ViewerSectionModel
            {
                Id = "menuperms",
                Title = "@RolMen@",
                Fields = fieldList
            };

            var fieldList2 = new List<ViewerFieldModel>();
            var roleSectionThree = new ViewerSectionModel { Id = "eventparms", Title = "@RolEve@", SubSections = new List<ViewerSubSectionModel>() };
            var topic = "";
            foreach (var item in eventsToUse.OrderBy(o => o.Topic).ThenBy(o => o.Description))
            {
                var newField = new ViewerFieldModel { Id = item.Key, Label = item.Description, Value = item.Allowed };

                if (topic != item.Topic)
                {
                    var subSectionFieldList = new List<ViewerFieldModel>();
                    foreach (var field in fieldList2)
                    {
                        var targetField = field.Copy();
                        subSectionFieldList.Add(targetField);
                    }
                    var translatedTopicName = topicTranslations.Where(t => t.listitemname == topic).FirstOrDefault();
                    var newSubSection = new ViewerSubSectionModel { Id = topic, Title = topic != ""  ? translatedTopicName.displayedtopic : "", Fields = subSectionFieldList };
                    if (topic != "")
                    {
                        roleSectionThree.SubSections.Add(newSubSection);
                        fieldList2.Clear();
                    }
                    topic = item.Topic;
                }
                fieldList2.Add(newField);
            }

            if (fieldList2.Count() > 0)
            {
                var translatedTopicName = topicTranslations.Where(t => t.listitemname == topic).FirstOrDefault();
                var newSubSection = new ViewerSubSectionModel { Id = topic, Title = translatedTopicName.displayedtopic, Fields = fieldList2 };
                roleSectionThree.SubSections.Add(newSubSection);
            }

            var newViewer = new ViewerModel { Sections = new List<ViewerSectionModel>() };
            newViewer.Sections.Add(roleSectionOne);
            newViewer.Sections.Add(roleSectionTwo);
            newViewer.Sections.Add(roleSectionThree);

                return JsonConvert.SerializeObject(newViewer);
        }
    }
}
