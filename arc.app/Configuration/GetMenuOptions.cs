using arc.domain.Configuration.QueryConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using arc.app.SystemConfig;
using arc.app.Config;
using Newtonsoft.Json;
using arc.app.Config.UIEvents;
using arc.app.Config.Forms;
using System.Collections.Generic;

namespace arc.app.Configuration
{
    internal class GetMenuOptions : ISingleConfig
    {
        private readonly IServiceProvider _serviceProvider;

        internal GetMenuOptions(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<string> GetAsync(QueryFilterConfig queryFilters, QueryConfig queryData)
        {
            var configRepository = _serviceProvider.GetService<IConfigRepository>();
            queryFilters.Parameters[0].Key = "id";
            var viewRecord = await configRepository.SingleConfigByIdAsync(queryFilters);

            var listViewFactory = _serviceProvider.GetService<IListViewConfigFactory>();
            var view = await listViewFactory.GetViewAsync(viewRecord.ConfigName);

            var uieventAdapter = _serviceProvider.GetService<IUIEventConfigAdapter>();
            var formAdapter = _serviceProvider.GetService<IFormConfigAdapter>();

            var returnList = new List<object>();
            foreach (var button in view.Buttons)
            {
                if (button.UIEvent == null)
                {
                    returnList.Add(new { Text = button.Text, Form = "" });
                }
                else
                {
                    var uievent = await uieventAdapter.GetEventAsync(button.UIEvent);
                    var formName = "";

                    if (uievent.Type.ToLower() == "form")
                    {
                        var form = await formAdapter.GetFormAsync(uievent.Action);
                        formName = form.Title;
                    }

                    var newButton = new { Text = button.Text, Form = formName };
                    returnList.Add(newButton);

                    if (button.Buttons != null)
                    {
                        foreach (var subbutton in button.Buttons)
                        {
                            uievent = await uieventAdapter.GetEventAsync(button.UIEvent);
                            var subFormName = "";
                            if (uievent.Type.ToLower() == "form")
                            {
                                var form = await formAdapter.GetFormAsync(uievent.Action);
                                subFormName = form.Title;
                            }

                            var newSubButton = new { Text = subbutton.Text, Form = subFormName, ParentButton = button.Text };
                            returnList.Add(newSubButton);
                        }
                    }

                }
            }


            return JsonConvert.SerializeObject(returnList);
        }
    }
}
