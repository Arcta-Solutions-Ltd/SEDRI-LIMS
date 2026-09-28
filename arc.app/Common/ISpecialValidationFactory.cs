using arc.common;
using arc.common.Models;
using System.Threading.Tasks;

namespace arc.app.Common
{
    public interface ISpecialValidationFactory
    {
        string ValidateMessage(string eventName, string message);
        Task<string> ValidateMessageAsync(string eventName, string message, TokenInfoModel token = null);
    }
}
