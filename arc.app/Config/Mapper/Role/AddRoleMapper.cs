using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class AddRoleMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'addrolemapper', 'Type': 'Special'
                     }";
        }
    }
}

