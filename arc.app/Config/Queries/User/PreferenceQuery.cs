using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class PreferenceQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'Preference', 'TableName': 'Users', 'Type': 'Special'
                    }";
        }
    }
}
