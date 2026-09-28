using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Configuration for the "Delete Culture Type Category" UI event.
/// </summary>
internal class DeleteCultureTypeCategoryUIEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Delete Culture Type Category" UI event.
    /// </summary>
    /// <remarks>
    /// This configuration defines the event's name, description, type, and associated action. 
    /// It specifies the form used for deleting a culture type category.
    /// </remarks>
    /// <returns>
    /// A string representation of the event configuration, detailing metadata such as 
    /// the event name, description, and associated form action.
    /// </returns>
    public string Get()
    {
        var newEvent = @"{
                        name: 'deleteculturetypecategoryuievent',
                        description: 'Delete culture type category',
                        type: 'form',
                        action: 'deleteculturetypecategoryform'
                    }";

        return newEvent;
    }
}
