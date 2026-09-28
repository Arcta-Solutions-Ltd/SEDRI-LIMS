using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class PrintSpecimenBarcode1UIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'printspecimenbarcode1uievent',
                        description: 'Print specimen barcode',
                        type: 'print-barcode',
                        action: 'SpecimenBarcodePrint1'
                    }";

            return newEvent;
        }
    }
}
