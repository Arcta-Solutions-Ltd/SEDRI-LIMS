using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Configuration for the "Delete Test Category" UI event.
/// </summary>
internal class DeleteTestCategoryUIEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Delete Test Category" UI event.
    /// </summary>
    /// <remarks>
    /// This configuration defines the event's name, description, type, and action. 
    /// It specifies the form used for deleting test category rules.
    /// </remarks>
    /// <returns>
    /// A string representation of the event configuration, including metadata such as 
    /// the event name, description, and associated form action.
    /// </returns>
    public string Get()
    {
        var newEvent = @"{
                        name: 'deletetestcategoryuievent',
                        description: 'Delete test category rules',
                        type: 'form',
                        action: 'deletetestcategoryform'
                    }";

        return newEvent;
    }
}
