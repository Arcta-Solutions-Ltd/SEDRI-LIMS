using System.Collections.Generic;
using Newtonsoft.Json;

namespace arc.common.Models.Instruments;

/// <summary>
/// Root JSON shape for instrument file payloads loaded into a direct test (e.g. GeneXpert export).
/// </summary>
public class DirectTestInstrumentFileModel
{
    [JsonProperty("AccessionNumber")]
    public string AccessionNumber { get; set; } = "";

    [JsonProperty("CompletionDateTime")]
    public string CompletionDateTime { get; set; } = "";

    [JsonProperty("Instrument")]
    public string Instrument { get; set; } = "";

    [JsonProperty("Status")]
    public string Status { get; set; } = "";

    /// <summary>
    /// Instrument payload / mapper key (e.g. <c>MTBPCR</c>). This selects which pipeline runs; the <c>tests.testname</c> row is resolved from the instrument profile’s <c>DirectTestId</c> in <c>DirectTestInstrumentProcessor</c>, not from this field.
    /// </summary>
    [JsonProperty("TestType")]
    public string TestType { get; set; } = "";

    [JsonProperty("Assays")]
    public List<DirectTestInstrumentAssayModel> Assays { get; set; } = new();
}

/// <summary>
/// One assay block (e.g. MTB or RIF Resistance) from the instrument file.
/// </summary>
public class DirectTestInstrumentAssayModel
{
    [JsonProperty("AssayName")]
    public string AssayName { get; set; } = "";

    [JsonProperty("MainResult")]
    public string MainResult { get; set; } = "";

    [JsonProperty("AnalyteResults")]
    public List<DirectTestInstrumentAnalyteResultModel> AnalyteResults { get; set; } = new();
}

/// <summary>
/// Single analyte row from the instrument file.
/// </summary>
public class DirectTestInstrumentAnalyteResultModel
{
    [JsonProperty("Analyte")]
    public string Analyte { get; set; } = "";

    [JsonProperty("CtValue")]
    public string CtValue { get; set; } = "";

    [JsonProperty("EndPtValue")]
    public string EndPtValue { get; set; } = "";

    [JsonProperty("Result")]
    public string Result { get; set; } = "";
}
