using System;

namespace arc.app.Instruments;

/// <summary>
/// Maps instrument profile InterfaceTypeId (InstrumentEvent list items on SingleInstrumentConfig) to <see cref="IInstrumentFactory"/> keys.
/// </summary>
internal static class InstrumentProcessorTypeResolver
{
    /// <summary>List item id for &quot;Organism Id and AST&quot; (InstrumentEvent list).</summary>
    internal const string InterfaceTypeIdOrganismIdAndAst = "9";

    /// <summary>
    /// List item id for the InstrumentEvent list item historically named &quot;Direct Test&quot; and now displayed as &quot;Custom&quot;.
    /// The id is unchanged (10) so existing profiles keep working and everything is matched by id, not the display value.
    /// This interface type now drives the flexible <see cref="Processors.CustomInstrumentProcessor"/> which reverse-maps
    /// an uploaded file against the profile's linked export profile mapping (replacing the legacy MTBPCR-only direct-test load).
    /// </summary>
    internal const string InterfaceTypeIdCustom = "10";

    /// <summary>Factory key for <see cref="Processors.AstProcessor"/>.</summary>
    internal const string FactoryKeyAst = "ast";

    /// <summary>
    /// Factory key for the superseded <see cref="Processors.DirectTestInstrumentProcessor"/> (MTBPCR-only direct-test load).
    /// Retained for reference; interface type id 10 now routes to <see cref="FactoryKeyCustomInstrument"/>.
    /// </summary>
    internal const string FactoryKeyDirectTestInstrument = "directtestinstrument";

    /// <summary>Factory key for <see cref="Processors.CustomInstrumentProcessor"/> (Custom interface type, id 10).</summary>
    internal const string FactoryKeyCustomInstrument = "custominstrument";

    /// <summary>
    /// Returns the factory key for the given interface type id. Custom (id 10) resolves to the flexible custom importer;
    /// unknown or empty values default to AST for backward compatibility.
    /// </summary>
    public static string ResolveFactoryKey(string interfaceTypeId)
    {
        if (string.IsNullOrWhiteSpace(interfaceTypeId))
            return FactoryKeyAst;

        var t = interfaceTypeId.Trim();
        if (string.Equals(t, InterfaceTypeIdCustom, StringComparison.Ordinal))
            return FactoryKeyCustomInstrument;

        return FactoryKeyAst;
    }
}
