using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class UserChangePasswordQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'UserChangePassword', 'TableName': 'Users', 'Type': 'Single', 
                        'Fields': [
                            {'Name': 'UserName', 'Type': 'string'},
                            {'Name': 'Id', 'Type': 'int'}
                        ],
                        'Where' : [
                            {'Field': 'Id', 'Comparison': '=' } 
                        ]
                    }";
        }
    }
}
