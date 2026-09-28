using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class BlankAllTestSelectionWithPatientRefQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'blankalltestselectionwithpatientref', 'Type': 'Special', 'Translate': true}";
        }
    }
}
