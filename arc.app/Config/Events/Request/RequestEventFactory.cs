using arc.app.Common;

namespace arc.app.Config.Events.Request;

/// <summary>
/// Resolves request event configuration definitions by name.
/// </summary>
internal class RequestEventFactory : IDefinitionFactory
{
    /// <summary>
    /// Creates the request event definition matching the supplied name.
    /// </summary>
    /// <param name="definitionName">The event definition name.</param>
    /// <returns>The matching definition, or null when this factory does not own the name.</returns>
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "editrequest" => new EditRequestEventConfig(),
            "deleterequest" => new DeleteRequestEventConfig(),
            "viewrequestrecord" => new ViewRequestRecordEventConfig(),
            "managerequestattachments" => new ManageRequestAttachmentsEventConfig(),
            _ => null,
        };
    }
}
