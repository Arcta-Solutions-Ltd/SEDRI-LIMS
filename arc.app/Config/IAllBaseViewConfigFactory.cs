using arc.domain.Configuration.ViewConfig.Common;

namespace arc.app.Config
{
    public interface IAllBaseViewConfigFactory
    {
        BaseViewConfig Get(string viewName);
    }
}
