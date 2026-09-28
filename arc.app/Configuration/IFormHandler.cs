using arc.common.Models;
using System.Threading.Tasks;

namespace arc.app.Configuration
{
    public interface IFormHandler
    {
        Task<string> GetFormListAsync(bool testonly = false, string testType = "tests", TokenInfoModel token = null);
    }
}
