using arc.domain.Configuration.ViewConfig.DiaryViewConfig;

namespace arc.app.Config.Views.DiaryViews
{
    public interface IDiaryFactory
    {
        DiaryConfig Create(string definitionName);
    }
}
