using arc.app.Common;

namespace arc.app.Config.Queries;

/// <summary>
/// Provides a query definition for retrieving a list of users.
/// </summary>
public class UserListQuery : IDefinition
{
    /// <summary>
    /// Retrieves the definition of the UserList query as a JSON string.
    /// </summary>
    /// <returns>
    /// A JSON string representing the query, where 'Query' is set to 'UserList'
    /// and 'Type' is set to 'Special'.
    /// </returns>
    public string Get()
    {
        return @"{ 'Query': 'UserList', 'Type': 'Special'}";
    }
}
