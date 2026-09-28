using System.Collections.Generic;
using Newtonsoft.Json;

namespace arc.domain.Configuration.BarcodeConfig
{
    /// <summary>
    /// Client-facing barcode layout and caption metadata bundled for printing (captions merged server-side when missing).
    /// </summary>
    public class BarcodePrintConfig
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public int NumberOfRows { get; set; }
        public int ItemsPerRow { get; set; }
        public bool Linear { get; set; }
        public bool QR { get; set; }
        public bool DisplayCode { get; set; }
        public string LeftMargin { get; set; }
        public string TopMargin { get; set; }
        public string ItemWidth { get; set; }
        public string ItemHeight { get; set; }
        public bool Border { get; set; }
        public bool SideBySide { get; set; }
        public string BarcodePadding { get; set; }
        public string BottomPadding { get; set; }
        public string FieldNameWidth { get; set; }
        public string FieldTotalWidth { get; set; }
        public int LinearHeight { get; set; }
        public int QRSize { get; set; }
        public string FieldFontSize { get; set; }
        public string LabelFields { get; set; }
        public string ReferenceForm { get; set; }
        public string FieldQuery { get; set; }
        public bool SuppressFieldLabels { get; set; }
        public string MaxBarcodeHeight { get; set; }
        public bool UseAccessionNumberForBarcode { get; set; }

        /// <summary>
        /// Maps stable field id (same tokens as comma-separated <see cref="LabelFields"/>, lowercased) to caption token (typically <c>@...@</c>), not transient list translations.
        /// </summary>
        [JsonProperty("LabelCaptions", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, string> LabelCaptions { get; set; }
    }
}
