using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class PrintSpecimenBarcode2UIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'printspecimenbarcode2uievent',
                        description: 'Print specimen barcode',
                        type: 'print-barcode',
                        action: 'SpecimenBarcodePrint2'
                    }";

            return newEvent;
        }
    }
}
