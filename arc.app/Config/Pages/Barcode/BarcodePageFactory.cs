using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class BarcodePageFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "editspecimenbarcodecontentpage" => new EditSpecimenBarcodeContentPageConfig(),
                "editpatientbarcodecontentpage" => new EditPatientBarcodeContentPageConfig(),
                "edititemdimensionspage" => new EditItemDimensionsPageConfig(),
                "editlabeldimensionspage" => new EditLabelDimensionsPageConfig(),
                _ => null,
            };
        }
    }
}
