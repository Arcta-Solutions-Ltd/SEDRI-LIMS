using arc.domain.Configuration.FormsConfig;
using arc.domain.Configuration.PagesConfig;
using arc.domain.Configuration.UIEventsConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Config
{
    public interface IFormConfigList
    {
        Task LoadFormsAsync(List<string> uiEventList, List<string> allowedEventList);
        List<UIEventConfig> GetUiEvents();
        Task<(List<PageConfig>, List<string>)> GetPagesAsync();
        List<FormConfig> GetForms();
        Task<List<FormConfig>> GetCompleteFormListAsync(List<string> uiEventList);

    }
}
