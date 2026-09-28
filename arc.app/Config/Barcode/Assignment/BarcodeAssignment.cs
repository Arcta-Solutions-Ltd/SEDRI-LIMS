using arc.app.Common;

namespace arc.app.Config.Barcode
{
    public class BarcodeAssignment : IDefinition
    {
        public string Get()
        {
            return @"{
                enabled: true,  
                width: 8,
                discriminator: 'SEDRI',
                seedValue: 12340001
            }";
        }
    }
}
