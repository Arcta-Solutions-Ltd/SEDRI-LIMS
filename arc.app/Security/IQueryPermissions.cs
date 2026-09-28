using arc.common.Models;
using System.Threading.Tasks;

namespace arc.app.Security;
 
public interface IQueryPermissions
{
    Task<string> GetQueryPermssionStringAsync(TokenInfoModel token, string language);
}
