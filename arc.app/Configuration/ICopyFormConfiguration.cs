using System.Threading.Tasks;

namespace arc.app.Configuration
{
    public interface ICopyFormConfiguration
    {
        Task Copy(string dataToSave, string formType, int formTypeKey);
    }
}
