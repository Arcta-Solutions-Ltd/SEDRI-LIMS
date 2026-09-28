using arc.common.Models.Instruments;
using System.Threading.Tasks;

namespace arc.app.Instruments
{
    public interface IInstrumentErrorRepository
    {
        Task<int> AddAsync(InstrumentErrorModel dataToSave);
    }
}
