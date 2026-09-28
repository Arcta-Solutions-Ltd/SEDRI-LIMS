using arc.app.Config.Barcode;
using arc.app.Config.Pages;
using arc.common;
using arc.common.ExtensionMethods;
using arc.common.Models.Barcodes;
using arc.domain.Configuration.BarcodeConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace arc.app.Barcodes
{
    public class BarcodesHandler : IBarcodesHandler
    {
        private readonly IGenericRepository _genericRepository;
        private readonly IBarcodePrintConfigAdapter _barcodePrintConfigAdapter;
        private readonly BarcodeAssignmentConfig _barcodeConfig;
        private readonly string _format;

        public BarcodesHandler(IGenericRepository genericRepository, IBarcodePrintConfigAdapter barcodePrintConfigAdapter)
        {
            _genericRepository = genericRepository;
            _barcodePrintConfigAdapter = barcodePrintConfigAdapter;

            var configJson = new BarcodeAssignment().Get();
            _barcodeConfig = JsonConvert.DeserializeObject<BarcodeAssignmentConfig>(configJson);

            _format = "";
            for (int i = 0; i < _barcodeConfig.Width; i++)
            {
                _format += '0';
            }
        }

        public async Task<string> GetBarcodeAsync()
        {
            if (!_barcodeConfig.Enabled) { return ""; }

            string barcodeCount;
            string dataToSave;

            _genericRepository.AddConfiguration("BarcodeCounter");
            var result = await _genericRepository.GetFirstValueAsync();

            if (string.IsNullOrEmpty(result))
            {
                barcodeCount = _barcodeConfig.SeedValue.ToString(_format);
                barcodeCount = barcodeCount.Substring(barcodeCount.Length - _barcodeConfig.Width, _barcodeConfig.Width);
                dataToSave = $"{{\"barcodecount\": {barcodeCount}}}";
                await _genericRepository.AddAsync(dataToSave);
            }
            else
            {
                var recordId = JsonConvert.DeserializeObject<IdModel>(result);
                barcodeCount = JsonConvert.DeserializeObject<BarcodeCountModel>(result).BarcodeCount;
                barcodeCount = (long.Parse(barcodeCount) + 1).ToString(_format);
                dataToSave = $"{{\"barcodecount\": {barcodeCount}}}";
                await _genericRepository.EditAsync(dataToSave, recordId.Id);
            }

            return _barcodeConfig.Discriminator + barcodeCount;
        }

        public async Task<string> GetBarcodeListAsync(string labelType)
        {
            var barcodePrintConfigsToDisplay = await _barcodePrintConfigAdapter.GetBarcodeConfigByTypeAsync(labelType);
            return JsonConvert.SerializeObject(barcodePrintConfigsToDisplay);
        }

        public async Task<string> GetBarcodeToEditAsync(string type, QueryFilterConfig queryFilters)
        {
            var id = queryFilters.GetIntegerValue("id");

            string barcodeName = $"{type}BarcodePrint{id}";
            var barcodeToEdit = await _barcodePrintConfigAdapter.GetBarcodePrintConfigAsync(barcodeName);

            var barcodeConfig = new BarcodePrintModel()
            {
                Id = barcodeToEdit.Id,
                Name = barcodeToEdit.Name,
                Title = barcodeToEdit.Title,
                NumberOfRows = barcodeToEdit.NumberOfRows,
                ItemsPerRow = barcodeToEdit.ItemsPerRow,
                LeftMargin = barcodeToEdit.LeftMargin,
                TopMargin = barcodeToEdit.TopMargin,
                ItemWidth = barcodeToEdit.ItemWidth,
                ItemHeight = barcodeToEdit.ItemHeight,
                BarcodePadding = barcodeToEdit.BarcodePadding,
                BottomPadding = barcodeToEdit.BottomPadding,
                FieldNameWidth = barcodeToEdit.FieldNameWidth,
                FieldTotalWidth = barcodeToEdit.FieldTotalWidth,
                LinearHeight = barcodeToEdit.LinearHeight,
                QRSize = barcodeToEdit.QRSize,
                FieldFontSize = barcodeToEdit.FieldFontSize,
                Linear = barcodeToEdit.Linear.ToYesNo(),
                QR = barcodeToEdit.QR.ToYesNo(),
                DisplayCode = barcodeToEdit.DisplayCode.ToYesNo(),
                Border = barcodeToEdit.Border.ToYesNo(),
                SideBySide = barcodeToEdit.SideBySide.ToYesNo(),
                LabelFields = barcodeToEdit.LabelFields,
                ReferenceForm = barcodeToEdit.ReferenceForm,
                FieldQuery = barcodeToEdit.FieldQuery,
                SuppressFieldLabels = barcodeToEdit.SuppressFieldLabels,
                MaxBarcodeHeight = barcodeToEdit.MaxBarcodeHeight,
                UseAccessionNumberForBarcode = barcodeToEdit.UseAccessionNumberForBarcode.ToYesNo(),
                LabelCaptions = barcodeToEdit.LabelCaptions
            };

            return JsonConvert.SerializeObject(barcodeConfig);
        }

        public async Task EditBarcodePrintConfigAsync(string type, string dataToSave)
        {
            var barcodePrintModel = JsonConvert.DeserializeObject<BarcodePrintModel>(dataToSave);

            var barcodeConfig = new BarcodePrintConfig()
            {
                Id = barcodePrintModel.Id,
                Name = barcodePrintModel.Name,
                Title = barcodePrintModel.Title,
                NumberOfRows = barcodePrintModel.NumberOfRows,
                ItemsPerRow = barcodePrintModel.ItemsPerRow,
                LeftMargin = barcodePrintModel.LeftMargin,
                TopMargin = barcodePrintModel.TopMargin,
                ItemWidth = barcodePrintModel.ItemWidth,
                ItemHeight = barcodePrintModel.ItemHeight,
                BarcodePadding = barcodePrintModel.BarcodePadding,
                BottomPadding = barcodePrintModel.BottomPadding,
                FieldNameWidth = barcodePrintModel.FieldNameWidth,
                FieldTotalWidth = barcodePrintModel.FieldTotalWidth,
                LinearHeight = barcodePrintModel.LinearHeight,
                QRSize = barcodePrintModel.QRSize,
                FieldFontSize = barcodePrintModel.FieldFontSize,
                Linear = barcodePrintModel.Linear.IsYes(),
                QR = barcodePrintModel.QR.IsYes(),
                DisplayCode = barcodePrintModel.DisplayCode.IsYes(),
                Border = barcodePrintModel.Border.IsYes(),
                SideBySide = barcodePrintModel.SideBySide.IsYes(),
                LabelFields = barcodePrintModel.LabelFields,
                ReferenceForm = barcodePrintModel.ReferenceForm,
                FieldQuery = barcodePrintModel.FieldQuery,
                SuppressFieldLabels = barcodePrintModel.SuppressFieldLabels,
                MaxBarcodeHeight = barcodePrintModel.MaxBarcodeHeight,
                UseAccessionNumberForBarcode = barcodePrintModel.UseAccessionNumberForBarcode.IsYes(),
                LabelCaptions = barcodePrintModel.LabelCaptions
            };

            var data = JsonConvert.SerializeObject(barcodeConfig);

            await _barcodePrintConfigAdapter.UpdateBarcodePrintConfigAsync(data);
        }
    }
}
