using arc.domain.Configuration.QueryConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using arc.app.SystemConfig;
using arc.app.Config;
using System.Linq;
using arc.app.Config.Forms;
using arc.app.Config.UIEvents;
using System.Collections.Generic;
using arc.domain.Configuration.FormsConfig;
using Newtonsoft.Json;
using arc.app.Config.Events;

namespace arc.app.Configuration
{
    internal class GetFormConfigList : ISingleConfig
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly string _type;

        internal GetFormConfigList(IServiceProvider serviceProvider, string type)
        {
            _serviceProvider = serviceProvider;
            _type = type;
        }

        public async Task<string> GetAsync(QueryFilterConfig queryFilters, QueryConfig queryData)
        {
            var configRepository = _serviceProvider.GetService<IConfigRepository>();
            queryFilters.Parameters[0].Key = "id";
            var id = queryFilters.Parameters[0].Value;
            var viewRecord = await configRepository.SingleConfigByIdAsync(queryFilters);

            var listViewFactory = _serviceProvider.GetService<IListViewConfigFactory>();
            var view = await listViewFactory.GetViewAsync(viewRecord.ConfigName);

            var uieventAdapter = _serviceProvider.GetService<IUIEventConfigAdapter>();
            var uiEventList = _type == "form" ? view.GetThisLevelOnlyUIEventList() : view.GetUIEventList();
            var formList = new List<string>();
            foreach (var ev in uiEventList)
            {
                var newEvent = await uieventAdapter.GetEventAsync(ev);
                if (newEvent != null && newEvent.Type.ToLower() == "form")
                {
                    formList.Add(newEvent.Action);
                }
            }

            var formAdapter = _serviceProvider.GetService<IFormConfigAdapter>();
            var eventAdapter = _serviceProvider.GetService<IEventAdapter>();
            var returnFormList = new List<FormConfig>();
            foreach ( var form in formList)
            {
                var newForm = await formAdapter.GetFormAsync(form);

                if (newForm != null)
                {
                    var formtype = newForm.Formtype == null ? null : newForm.Formtype.ToLower();
                    if ((_type == "directtest" && formtype == "directtest") || (_type == "culturetest" && formtype == "culturetest") || (_type.ToLower() != "directtest" && _type.ToLower() != "culturetest"))
                    {
                        if (newForm.Configurable != null && newForm.Configurable.ToLower() == "yes")
                        {
                            var formEvent = await eventAdapter.GetEventAsync(newForm.SaveEvent);
                            newForm.SaveEvent = formEvent.Description;
                            returnFormList.Add(newForm);
                        }
                    }
                }
            }

            var returnList = returnFormList.Where(f => f != null).Select(f => new { Event = f.SaveEvent, Description = f.Title, Id = id + "|" + f.Name });

            return JsonConvert.SerializeObject(returnList);

        }
    }
}
