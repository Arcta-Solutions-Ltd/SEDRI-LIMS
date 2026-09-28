using arc.common;
using arc.common.Models;
using System.Threading.Tasks;

namespace arc.app
{
    public interface IHandleEvent
    {
        Task<IdModel> HandleAsync(string message, TokenInfoModel token);
        string GetValidationMessage();
    }
}
