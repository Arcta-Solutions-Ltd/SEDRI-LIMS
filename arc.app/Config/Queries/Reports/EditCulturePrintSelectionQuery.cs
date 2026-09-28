using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class EditCulturePrintSelectionQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'editcultureprintselectionquery', 'Type': 'Report', 'Translate': true}";
        }
    }
}
