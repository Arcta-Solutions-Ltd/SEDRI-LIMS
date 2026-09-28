using arc.domain.Security.User;

namespace arc.identity
{
    public interface IPasswordUtils
    {
        string GetPassword(User user);
    }
}
