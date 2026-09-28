using arc.app.Config;
using arc.app.Config.Events;
using arc.app.Config.Forms;
using arc.app.Config.UIEvents;
using arc.domain.Configuration.EventsConfig;
using arc.domain.Configuration.ListsConfig;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Configuration.Queries
{
    public  class SpecimenEventListQuery : ISpecimenEventListQuery
    {
        private readonly IListViewConfigFactory _listViewFactory;
        private readonly IUIEventConfigAdapter _uiEventConfigAdapter;
        private readonly IFormConfigAdapter _formConfigAdapter;
        private readonly IEventAdapter _eventAdapter;

        public SpecimenEventListQuery(IListViewConfigFactory listViewFactory, IUIEventConfigAdapter uiEventConfigAdapter, IFormConfigAdapter formConfigAdapter, IEventAdapter eventAdapter)
        {
            _listViewFactory = listViewFactory;
            _uiEventConfigAdapter = uiEventConfigAdapter;
            _formConfigAdapter = formConfigAdapter;
            _eventAdapter = eventAdapter;
        }

        public  async Task<List<OptionsConfig>> GetListAsync()
        {
            var view = await _listViewFactory.GetViewAsync("specimens");

            var uiEventList = view.GetUIEventList();

            var eventList = new List<EventConfig>();
            foreach (var uiEvent in uiEventList)
            {
                var uiEventConfig = await _uiEventConfigAdapter.GetEventAsync(uiEvent);
                if (uiEventConfig.Type.ToLower() == "form")
                {
                    var formConfig = await _formConfigAdapter.GetFormAsync(uiEventConfig.Action);
                    var eventConfig = await _eventAdapter.GetEventAsync(formConfig.SaveEvent);

                    if ( ! string.IsNullOrEmpty(eventConfig.Description))
                    {
                        if (eventConfig.Topic.ToLower() != "tests" && eventConfig.Topic.ToLower() != "culturetests" && eventConfig.EventName.ToLower() != "specimenreport")
                        {
                            eventList.Add(eventConfig);
                        }
                    }
                }
            }

            return  eventList.Select(x => new OptionsConfig { Key = x.EventName, Text = x.Description }).Distinct().ToList();
        }
    }
}
