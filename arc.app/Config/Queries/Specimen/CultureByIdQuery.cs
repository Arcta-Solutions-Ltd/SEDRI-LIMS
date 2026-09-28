using arc.app.Common;

namespace arc.app.Config.Queries.Specimen
{
    internal class CultureByIdQuery : IDefinition
    {
        public string Get()
        {
            return @"{
                'Query': 'CultureById',
                        'TableName': 'Culture',
                        'Type': 'Special',
                        'Tags': 'SP'
                    }
            ";
        }
    }
}


            //return @"{
            //            'Query': 'CultureById',
            //            'TableName': 'Culture',
            //            'Type': 'Single',
            //            'ResultMapping': 'culturebyidmapper',
            //            'Fields': [
            //                { 'Name': 'SpecimenOrganismId', 'Type': 'string'},
            //                { 'Name': 'SpecimenQuantityId', 'Type': 'string'},
            //                { 'Name': 'SpecimenApiIdPanelId', 'Type': 'string'},
            //                { 'Name': 'IdProfile', 'Type': 'string'},
            //                { 'Name': 'IdPercentage', 'Type': 'string'},
            //                { 'Name': 'PositiveDate', 'Type': 'date'},
            //                { 'Name': 'PositiveTime', 'Type': 'string'},
            //                { 'Name': 'CommentOneId', 'Type': 'string'},
            //                { 'Name': 'CommentTwoId', 'Type': 'string'},
            //                { 'Name': 'AdditionalNotes', 'Type': 'string'},
            //                { 'Name': 'AloquatId', 'Type': 'string'},
            //                { 'Name': 'DisplayOnReport', 'Type': 'string'}
            //            ],
            //            'Where' : [
            //                {'Field': 'Id', 'Comparison': '=' }
            //            ]
            //        }";





// 'ListItems' should be declared as follows, but these fields are currently strings in the database and named
// 'commentone' & 'commenttwo' instead of 'commentoneid' & 'commenttwoid'.
//
//                       'ListItems': 'SpecimenOrganism, SpecimenQuantity, SpecimenApiIdPanel, CommentOne, CommentTwo',

