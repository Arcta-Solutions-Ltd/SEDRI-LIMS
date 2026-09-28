using arc.common.Models;
using System.Threading.Tasks;

namespace arc.app.Security
{
    public interface IRegisterHandler
    {
        void Handle(string message);
        Task UpdateUser(string message, bool edit);
        string GetValidationMessage();
        Task ChangePassword(string message);
        Task ChangeMyPassword(string message, TokenInfoModel token);
    }
}
