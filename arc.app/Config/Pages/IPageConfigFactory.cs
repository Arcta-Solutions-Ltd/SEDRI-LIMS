using arc.domain.Configuration.PagesConfig;

namespace arc.app.Config.Pages
{
    public interface IPageConfigFactory
    {
        PageConfig GetPage(string pageName);
    }
}
