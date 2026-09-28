using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class EditPatientBarcodeFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'editpatientbarcodeform',
                        viewTitle: 'Edit Patient Barcode',
                        initialQuery: 'editpatientbarcode',
                        saveEvent: 'editbarcodeprintconfig',
                        suppressRecordView: true,
                        pages: ['editpatientbarcodecontentpage', 'edititemdimensionspage', 'editlabeldimensionspage']
                    }";

            return form;
        }
    }
}
