using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class BarcodeEditQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'BarcodeEdit', 'Type': 'Special'}";
        }
    }
}
