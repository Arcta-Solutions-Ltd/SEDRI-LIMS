using arc.common.Models.Specimen;
using System.Threading.Tasks;

namespace arc.app.Settings
{
    public interface IAccessionNumberCalculator
    {
        Task<string> GetAccessionNumberAsync(string category, CreateSpecimenEventModel data);
    }
}
