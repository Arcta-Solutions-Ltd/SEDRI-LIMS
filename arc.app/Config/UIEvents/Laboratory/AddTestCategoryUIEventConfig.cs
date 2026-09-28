using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Configuration for the "Add Test Category" UI event.
/// </summary>
internal class AddTestCategoryUIEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Add Test Category" UI event.
    /// </summary>
    /// <remarks>
    /// This configuration specifies the event's name, description, type, and action.
    /// It is used to define the UI event for adding a new test category.
    /// </remarks>
    /// <returns>
    /// A string representation of the event configuration, detailing its attributes and behavior.
    /// </returns>
    public string Get()
    {
        var newEvent = @"{
                        name: 'addtestcategoryuievent',
                        description: 'Add test category',
                        type: 'form',
                        action: 'addtestcategoryform'
                    }";

        return newEvent;
    }
}
