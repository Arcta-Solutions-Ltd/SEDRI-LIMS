using arc.domain.Security.User;
using Microsoft.AspNetCore.Identity;

namespace arc.identity
{
    /// <summary>
    /// Provides utilities for generating password hashes for application users.
    /// </summary>
    public class PasswordUtils : IPasswordUtils
    {
        /// <summary>
        /// Generates a hashed password for the specified user using the ASP.NET Core identity password hasher.
        /// </summary>
        /// <param name="user">The user whose password will be hashed. The plain-text password is read from <see cref="User.Password"/>.</param>
        /// <returns>The hashed password string.</returns>
        public string GetPassword(User user)
        {
            var hasher = new PasswordHasher<User>();
            return hasher.HashPassword(user, user.Password);
        }
    }
}
