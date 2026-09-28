using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class OrganisationForValidationQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'OrganisationForValidation',
                        'TableName': 'Organisation',
                        'Type': 'Special'
                    }";
        }
    }
}
