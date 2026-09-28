using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class SpecimenByIdForCancelRequestQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'SpecimenByIdForCancelRequest', 
                        'TableName': 'Specimen', 
                        'Type': 'Single', 
                        'Fields': [
                            { 'Name': 'AccessionNumber', 'Type': 'string'}
                        ],
                        'Where' : [
                            {'Field': 'Id', 'Comparison': '=' } 
                        ]
                    }";
        }
    }
}
