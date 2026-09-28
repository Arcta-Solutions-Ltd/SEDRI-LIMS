using arc.app.Common;
using arc.common.Models.Specimen;
using arc.domain.Configuration.QueryFiltersConfig;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace arc.app.Specimen;

/// <summary>
/// Validates edits to a culture's growth assignment by comparing the existing GrowthId
/// against the new value and enforcing that a growth entry cannot be removed if it was present.
/// </summary>
internal class EditIsolateValidator : ISpecialValidatorAsync
{
    private readonly string _message;
    private readonly ICultureRepository _cultureRepository;
    private readonly ILogWriter _logWriter;

    /// <summary>
    /// Initializes a new instance of <see cref="EditIsolateValidator"/>.
    /// </summary>
    /// <param name="cultureRepository">
    /// Repository used to fetch the current culture data by ID.
    /// </param>
    /// <param name="logWriter">
    /// Logger used to record validation decisions for field support.
    /// </param>
    /// <param name="message">
    /// A JSON string representing an <see cref="EditCultureValidatorModel"/> with the target Id and optional GrowthId.
    /// </param>
    public EditIsolateValidator(ICultureRepository cultureRepository, ILogWriter logWriter, string message)
    {
        _message = message;
        _cultureRepository = cultureRepository;
        _logWriter = logWriter;
    }

    /// <summary>
    /// Deserializes the incoming message, retrieves the existing culture by ID,
    /// and ensures that if the original GrowthId indicated a growth entry, the new GrowthId does as well.
    /// Null or omitted GrowthId on edit is valid when the culture has no existing growth entry.
    /// </summary>
    /// <returns>
    /// Returns the error code "@SpeCanD@" if an existing growth entry is removed; otherwise, returns an empty string.
    /// </returns>
    public async Task<string> ValidateMessageAsync()
    {
        var data = JsonConvert.DeserializeObject<EditCultureValidatorModel>(_message);

        if (!data.GrowthId.HasValue)
        {
            _logWriter.LogInfo(
                $"EditIsolateValidator: culture Id={data.Id} edit payload has null GrowthId",
                "EditIsolateValidator", "ValidateMessageAsync");
        }

        var queryFilter = new QueryFilterConfig().AddInteger("id", data.Id);
        var currentIsolate = await _cultureRepository.GetCultureByIdAsync(queryFilter);

        if (string.IsNullOrEmpty(currentIsolate.GrowthId))
        {
            _logWriter.LogInfo(
                $"EditIsolateValidator: culture Id={data.Id} has no existing growth; edit allowed (GrowthId={(data.GrowthId?.ToString() ?? "null")})",
                "EditIsolateValidator", "ValidateMessageAsync");
            return string.Empty;
        }

        if (!int.TryParse(currentIsolate.GrowthId, out var currentGrowthId))
        {
            _logWriter.LogInfo(
                $"EditIsolateValidator: culture Id={data.Id} has non-numeric GrowthId '{currentIsolate.GrowthId}'; skipping growth downgrade check",
                "EditIsolateValidator", "ValidateMessageAsync");
            return string.Empty;
        }

        if (isGrowthEntry(currentGrowthId)
            && (!data.GrowthId.HasValue || !isGrowthEntry(data.GrowthId.Value)))
        {
            _logWriter.LogInfo(
                $"EditIsolateValidator: blocked downgrade from growth entry {currentGrowthId} to {(data.GrowthId?.ToString() ?? "null")} for culture Id={data.Id}",
                "EditIsolateValidator", "ValidateMessageAsync");
            return "@SpeCanD@";
        }

        return string.Empty;
    }

    /// <summary>
    /// Determines whether the specified growth ID corresponds to a predefined growth entry.
    /// </summary>
    /// <param name="growthid">The growth ID to evaluate.</param>
    /// <returns>
    /// <c>true</c> if <paramref name="growthid"/> matches one of the known growth entry IDs; otherwise, <c>false</c>.
    /// </returns>
    private bool isGrowthEntry(int growthid)
    {
        return growthid == 178 || growthid == 179 || growthid == 180
            || growthid == 181 || growthid == 126 || growthid == 1087;
    }
}
