using System.Collections.Generic;
using Newtonsoft.Json;

namespace arc.common.Models.Barcodes
{
    /// <summary>
    /// Editing model for barcode print configuration (camelCase-compatible JSON where appropriate).
    /// </summary>
    public class BarcodePrintModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public int NumberOfRows { get; set; }
        public int ItemsPerRow { get; set; }
        public string Linear { get; set; }
        public string QR { get; set; }
        public string DisplayCode { get; set; }
        public string LeftMargin { get; set; }
        public string TopMargin { get; set; }
        public string ItemWidth { get; set; }
        public string ItemHeight { get; set; }
        public string Border { get; set; }
        public string SideBySide { get; set; }
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
        public string UseAccessionNumberForBarcode { get; set; }

        /// <summary>
        /// Optional map of lowercase field ids to caption language tags (<c>@...@</c>).
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Dictionary<string, string> LabelCaptions { get; set; }
    }
}
