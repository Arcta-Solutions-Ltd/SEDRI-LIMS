using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class DoesPatientContainSpecimensCheckQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'DoesPatientContainSpecimensCheckQuery', 
                        'TableName': 'Specimen', 
                        'Type': 'Count', 
                        'Fields': [
                            { 'Name': 'Id', 'Type': 'int'}
                        ],
                        'Where' : [
                            {'Field': 'Id', 'Comparison': '=', 'FieldToMatch': 'PatientId' } 
                        ]
                    }";
        }
    }
}
