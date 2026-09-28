using arc.common.Models;
using System.Collections.Generic;
using System.Linq;

namespace arc.common.Utils;

/// <summary>
/// This class converts a JSON structure to a list of key-value pairs.
/// Implements the IConvertJsonStructureToKeyValuePair interface.
/// </summary>
public class ConvertJsonStructureToKeyValuePair : IConvertJsonStructureToKeyValuePair
{
    private readonly IJsonWholeStructureFieldsCollector _jsonWholeStructureFieldsCollector;

    /// <summary>
    /// Initializes a new instance of the ConvertJsonStructureToKeyValuePair class.
    /// </summary>
    /// <param name="jsonWholeStructureFieldsCollector">The JSON structure fields collector.</param>
    public ConvertJsonStructureToKeyValuePair(IJsonWholeStructureFieldsCollector jsonWholeStructureFieldsCollector)
    {
        _jsonWholeStructureFieldsCollector = jsonWholeStructureFieldsCollector;
    }

    /// <summary>
    /// Converts the given JSON string to a list of key-value pairs.
    /// </summary>
    /// <param name="json">The JSON string to convert.</param>
    /// <param name="removeId">A boolean flag indicating whether to remove the "id" field.</param>
    /// <returns>An enumerable of KeyValueModel representing the key-value pairs.</returns>
    public IEnumerable<KeyValueModel> Convert(string json, bool removeId)
    {
        var jsonStructure = _jsonWholeStructureFieldsCollector.GetStructure(json);
        if (removeId)
        {
            jsonStructure = jsonStructure.Where((item) => item.Label.ToLower() != "id").ToList();
        }
        var result = jsonStructure.Where((item) => item.Contents != null).Select((item) => new KeyValueModel { Key = item.Label, Value = item.Contents });

        return result;
    }
}
