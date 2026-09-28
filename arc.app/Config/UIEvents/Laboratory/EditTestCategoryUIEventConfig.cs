using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Configuration for the "Edit Test Category" UI event.
/// </summary>
internal class EditTestCategoryUIEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Edit Test Category" UI event.
    /// </summary>
    /// <remarks>
    /// This configuration specifies the event's name, description, type, and associated action.
    /// It defines the form used for editing test category rules.
    /// </remarks>
    /// <returns>
    /// A string representation of the event configuration, including metadata such as 
    /// the event name, description, and the form action associated with the event.
    /// </returns>
    public string Get()
    {
        var newEvent = @"{
                        name: 'edittestcategoryuievent',
                        description: 'Edit test category rules',
                        type: 'form',
                        action: 'edittestcategoryform'
                    }";

        return newEvent;
    }
}

