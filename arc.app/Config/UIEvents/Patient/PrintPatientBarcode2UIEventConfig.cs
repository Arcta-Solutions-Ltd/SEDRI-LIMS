using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class PrintPatientBarcode2UIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'printpatientbarcode2uievent',
                        description: 'Print patient barcode',
                        type: 'print-barcode',
                        action: 'PatientBarcodePrint2'
                    }";

            return newEvent;
        }
    }
}
