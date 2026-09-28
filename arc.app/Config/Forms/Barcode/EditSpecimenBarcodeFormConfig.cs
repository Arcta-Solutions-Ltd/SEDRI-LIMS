using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class EditSpecimenBarcodeFormConfig : IDefinition
    {
        public string Get()
        {
            return @"{
                name: 'editspecimenbarcodeform',
                viewTitle: 'Edit Specimen Barcode',
                initialQuery: 'editspecimenbarcode',
                saveEvent: 'editbarcodeprintconfig',
                suppressRecordView: true,
                pages: ['editspecimenbarcodecontentpage', 'edititemdimensionspage', 'editlabeldimensionspage']
            }";
        }
    }
}
