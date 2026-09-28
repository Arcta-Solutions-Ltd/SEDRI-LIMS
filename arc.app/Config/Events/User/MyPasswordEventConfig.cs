using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class MyPasswordEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'mypassword', 
                        Description: '@UseMy@',
                        EventType : 'editdata', 
                        Topic : 'User', 
                        TableName: 'Users',
                        ValidationRules: [
                            { field: 'Password', rule: 'required', message: '@UsePas@'}
                        ]
                    }";
        }
    }
}
