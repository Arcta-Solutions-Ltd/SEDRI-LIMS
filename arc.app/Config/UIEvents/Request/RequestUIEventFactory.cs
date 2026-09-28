using arc.app.Common;

namespace arc.app.Config.UIEvents.Request;

/// <summary>
/// Resolves request UI event configuration definitions by name.
/// </summary>
internal class RequestUIEventFactory : IDefinitionFactory
{
    /// <summary>
    /// Creates the request UI event definition matching the supplied name.
    /// </summary>
    /// <param name="definitionName">The UI event definition name.</param>
    /// <returns>The matching definition, or null when this factory does not own the name.</returns>
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "viewrequestrecorduievent" => new ViewRequestRecordUIEventConfig(),
            "editrequestuievent" => new EditRequestUIEventConfig(),
            "deleterequestuievent" => new DeleteRequestUIEventConfig(),
            "managerequestattachmentsuievent" => new ManageRequestAttachmentsUIEventConfig(),
            _ => null,
        };
    }
}
