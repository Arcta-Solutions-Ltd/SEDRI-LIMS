using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class SpecimenInstrumentResultsQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'SpecimenInstrumentResultsQuery',
                        'TableName': 'InstrumentResults',
                        'Translate': false,
                        'Type': 'special'
                    }";
        }
    }
}
