using System;
using System.Collections.Generic;

namespace arc.common.Data;

/// <summary>
/// Keys the client adds to a save payload as system metadata rather than record data.
/// These are consumed by event routing and must never be written as database columns
/// or stored in a MoreData JSONB field.
/// </summary>
public static class ClientPayloadMetadataFields
{
    private static readonly string[] MetadataNames =
    [
        "ApplyDefaults",
        "Action",
        "Crafted",
        "Event",
        "View",
        "FormName",
        "ReportFilter",
        "SelectedItems",
        "PatientAgeDisplay"
    ];

    private static readonly HashSet<string> MetadataNameLookup =
        new(MetadataNames, StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Gets the metadata key names, in the order they are stripped from a payload.
    /// </summary>
    public static IReadOnlyList<string> Names => MetadataNames;

    /// <summary>
    /// Returns true when the supplied payload key is client metadata.
    /// </summary>
    /// <param name="fieldName">Payload key taken from the save message.</param>
    /// <returns>True when the key is system metadata rather than record data.</returns>
    public static bool Contains(string fieldName)
    {
        return !string.IsNullOrWhiteSpace(fieldName) && MetadataNameLookup.Contains(fieldName);
    }
}
