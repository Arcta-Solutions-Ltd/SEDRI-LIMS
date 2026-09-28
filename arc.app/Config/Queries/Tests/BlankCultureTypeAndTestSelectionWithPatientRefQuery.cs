using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class BlankCultureTypeAndTestSelectionWithPatientRefQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'blankculturetypeandtestselectionwithpatientref', 'Type': 'Special', 'Translate': true}";
        }
    }
}
