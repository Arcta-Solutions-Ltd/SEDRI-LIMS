using arc.domain.Configuration.FormStructureConfig;

namespace arc.app.Configuration
{
    public interface IResetNamesInForm
    {
        FullFormConfig Reset(FullFormConfig form, string oldField, string newField, string type);
    }
}
