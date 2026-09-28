using arc.domain.Configuration.FormsConfig;

namespace arc.app.Config.Forms
{
    public interface IFormConfigFactory
    {
        FormConfig GetForm(string formName);
    }
}
