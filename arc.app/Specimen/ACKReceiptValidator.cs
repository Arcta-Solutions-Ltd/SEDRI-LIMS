using arc.app.Common;
using arc.common.Models.Specimen;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace arc.app.Specimen;

/// <summary>
/// Validates acknowledgment receipt events by ensuring the received date
/// is not earlier than the specimen’s collection date.
/// </summary>
internal class ACKReceiptValidator : ISpecialValidatorAsync
{
    private readonly ISpecimenRepository _specimenRepository;
    private readonly string _message;

    /// <summary>
    /// Initializes a new instance of the <see cref="ACKReceiptValidator"/> class.
    /// </summary>
    /// <param name="specimenRepository">
    /// Repository used to retrieve specimen collection dates.
    /// </param>
    /// <param name="message">
    /// JSON string representing the ACK receipt event payload.
    /// </param>
    public ACKReceiptValidator(ISpecimenRepository specimenRepository, string message)
    {
        _specimenRepository = specimenRepository;
        _message = message;
    }

    /// <summary>
    /// Deserializes the ACK receipt event message, compares the received date
    /// to the collection date, and returns a validation error if received
    /// precedes collection.
    /// </summary>
    /// <returns>
    /// The validation error code (“@SpeVal@”) if the received date is before
    /// the collection date; otherwise an empty string.
    /// </returns>
    public async Task<string> ValidateMessageAsync()
    {
        var data = JsonConvert.DeserializeObject<ACKReceiptEventModel>(_message);
        var receivedDate = Convert.ToDateTime(data.ReceivedDate);
        var collectionDate = await _specimenRepository.GetCollectionDateAsync(data.Id);

        if (receivedDate.CompareTo(collectionDate) < 0)
        {
            return "@SpeVal@";
        }

        return string.Empty;
    }
}
