using arc.app.Common;
using arc.common.Models.Coding;
using Newtonsoft.Json;

namespace arc.app.Coding;

/// <summary>
/// Validates the relationship between OrderId and FamilyId in a deserialized CustomEntryModel.
/// Ensures that if an OrderId is provided, a FamilyId must also be supplied.
/// </summary>
internal class CustomEntryValidator : ISpecialValidator
{
    private readonly string _message;

    /// <summary>
    /// Initializes a new instance of <see cref="CustomEntryValidator"/> with the raw JSON message.
    /// </summary>
    /// <param name="message">
    /// A JSON string representing a <see cref="CustomEntryModel"/> to validate.
    /// </param>
    public CustomEntryValidator(string message)
    {
        _message = message;
    }

    /// <summary>
    /// Deserializes the JSON message and checks that if <see cref="CustomEntryModel.OrderId"/> is non-zero,
    /// then <see cref="CustomEntryModel.FamilyId"/> must also be non-zero.
    /// </summary>
    /// <returns>
    /// Returns the error code <c>"@CodVal@"</c> if the validation rule fails; otherwise, returns an empty string.
    /// </returns>
    public string ValidateMessage()
    {
        var customEntry = JsonConvert.DeserializeObject<CustomEntryModel>(_message);

        if (customEntry.OrderId != 0 && customEntry.FamilyId == 0)
        {
            return "@CodVal@";
        }

        return string.Empty;
    }
}
