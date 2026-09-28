using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class ChangePasswordEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'changepassword', 
                        Description: '@UseCha@',
                        EventType : 'editdata', 
                        Topic : 'User', 
                        TableName: 'User',
                        ValidationRules: [
                            { field: 'UserId', rule: 'required', message: '@UseUseD@'},
                            { field: 'Password', rule: 'required', message: '@UsePas@'}
                        ]
                    }";
        }
    }
}
