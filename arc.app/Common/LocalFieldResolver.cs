using arc.app.Barcodes;
using arc.common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace arc.app.Common
{
    public class LocalFieldResolver : ILocalFieldResolver
    {
        private readonly IBarcodesHandler _barcodesHandler;

        public LocalFieldResolver(IBarcodesHandler barcodesHandler)
        {
            _barcodesHandler = barcodesHandler;
        }

        public async Task<List<JsonFieldModel>> GetFieldValuesAsync(List<JsonFieldModel> requiredLocalFields)
        {
            var localFields = requiredLocalFields;

            foreach (var field in localFields)
            {
                switch (field.Key.ToLower())
                {
                    case "barcode":
                        field.Value = await _barcodesHandler.GetBarcodeAsync();
                        break;
                }
            }

            return localFields;
        }
    }
}
