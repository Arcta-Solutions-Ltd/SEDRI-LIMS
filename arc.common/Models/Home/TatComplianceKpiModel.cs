namespace arc.common.Models.Home;

/// <summary>
/// Home dashboard TAT Compliance KPI payload: primary window compliance %, counts, optional rolling-average %, and RAG band.
/// </summary>
public class TatComplianceKpiModel
{
    /// <summary>Compliance percentage for the primary (dashboard timerange) window, or null when there are no qualifying specimens.</summary>
    public double? PrimaryPercent { get; set; }

    /// <summary>Specimens in the window with TAT within the configured target.</summary>
    public int OnTimeCount { get; set; }

    /// <summary>Specimens finalised in the window with a usable received timestamp.</summary>
    public int TotalCount { get; set; }

    /// <summary>Specimens in the window with TAT strictly greater than the late threshold (hours).</summary>
    public int LateCount { get; set; }

    /// <summary>Optional compliance % over a longer rolling window (e.g. 7 days).</summary>
    public double? RollingAveragePercent { get; set; }

    /// <summary>Display band for the headline colour: Green, Amber, or Red.</summary>
    public string Rag { get; set; }
}
