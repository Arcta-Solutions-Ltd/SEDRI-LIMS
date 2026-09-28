using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class PrintPatientBarcode1UIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'printpatientbarcode1uievent',
                        description: 'Print patient barcode',
                        type: 'print-barcode',
                        action: 'PatientBarcodePrint1'
                    }";

            return newEvent;
        }
    }
}
