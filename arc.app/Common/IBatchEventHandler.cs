using arc.common.Models;
using System.Threading.Tasks;

namespace arc.app.Common
{
    public interface IBatchEventHandler
    {
        Task Handle(string message, TokenInfoModel token);
        string GetValidationMessage();
    }
}
