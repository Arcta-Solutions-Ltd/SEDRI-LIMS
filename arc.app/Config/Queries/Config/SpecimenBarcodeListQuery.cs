using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class SpecimenBarcodeListQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'SpecimenBarcodeList', 'Type': 'Special'}";
        }
    }
}
