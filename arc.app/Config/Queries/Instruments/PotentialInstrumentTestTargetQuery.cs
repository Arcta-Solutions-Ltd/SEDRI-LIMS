//using arc.app.Common;

//namespace arc.app.Config.Queries
//{
//    internal class PotentialInstrumentTestTargetQuery : IDefinition
//    {
//        public string Get()
//        {
//            return @"{
//              'Query': 'PotentialInstrumentTestTarget',
//              'TableName': 'Culture',
//              'Type': 'Select',
//              'Fields': [
//                {
//                  'Name': 'Id',
//                  'Type': 'int'
//                },
//                {
//                  'Name': 'ManufacturersBarcode',
//                  'Type': 'string'
//                },
//                {
//                  'Name': 'TypeId',
//                  'Type': 'int'
//                }
//              ],
//              'Joins': [
//                {
//                  'Table': 'Specimen',
//                  'On': 'SpecimenId',
//                  'Fields': [
//                    {
//                      'Name': 'AccessionNumber',
//                      'Type': 'string'
//                    },
//                    {
//                      'Name': 'StateId',
//                      'Type': 'string'
//                    },
//                    {
//                      'Name': 'CollectionDate',
//                      'Type': 'date'
//                    }
//                  ]
//                },
//                {
//                  'Table': 'Patient',
//                  'On': 'a.PatientId',
//                  'Fields': [
//                    {
//                      'Name': 'PatientRef',
//                      'Type': 'string'
//                    },
//                    {
//                      'Name': 'FirstName',
//                      'Type': 'string'
//                    },
//                    {
//                      'Name': 'Surname',
//                      'Type': 'string'
//                    }
//                  ]
//                }
//              ],
//              'Where': [
//                {
//                  'Field': 'StateId',
//                  'Comparison': 'oneof'
//                },
//                {
//                  'Field': 'LastModifiedDate',
//                  'Comparison': 'withinlast',
//                  'Units': 'seconds'
//                }
//              ]
//            }";
//        }
//    }
//}

