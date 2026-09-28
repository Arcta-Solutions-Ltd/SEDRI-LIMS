using arc.domain.Configuration.ViewConfig.Common;
using System.Collections.Generic;

namespace arc.domain.Configuration.ViewConfig.DiaryViewConfig
{
    public class DiaryConfig : BaseViewConfig
    {
        public string Title { get; set; }
        public string HeaderText { get; set; }
        public string InitialQuery { get; set; }
        public List<TextConfig> Text { get; set; }

        public List<string> GetUIEventList()
        {
            var eventList = new List<string>();
            foreach (var button in Buttons)
            {
                eventList.AddRange(button.GetUIEventList());
            }
            return eventList;
        }
    }
}
