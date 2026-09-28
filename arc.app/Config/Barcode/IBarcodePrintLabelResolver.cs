using System.Collections.Generic;
using System.Threading.Tasks;
using arc.domain.Configuration.BarcodeConfig;

namespace arc.app.Config.Barcode;

/// <summary>
/// Fills missing <see cref="BarcodePrintConfig.LabelCaptions"/> entries from authoritative form/page definitions without user form permissions.
/// </summary>
public interface IBarcodePrintLabelResolver
{
    /// <summary>
    /// Ensures each id in <see cref="BarcodePrintConfig.LabelFields"/> has a caption. Existing captions are retained (caller dictionary is merged upward).
    /// </summary>
    /// <param name="configs">Barcode layouts returned to clients.</param>
    /// <param name="username">For diagnostics only.</param>
    Task EnrichBarcodePrintConfigsAsync(IReadOnlyList<BarcodePrintConfig> configs, string username);
}
