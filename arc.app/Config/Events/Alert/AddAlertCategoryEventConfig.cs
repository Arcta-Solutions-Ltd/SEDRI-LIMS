using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class AddAlertCategoryEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'addAlertCategory', 
                        Description: '@AleAddD@',
                        EventType : 'adddata', 
                        Topic : 'Alert', 
                        TableName: 'AlertType',
                        ValidationRules: [
                            { field: 'Name', rule: 'required', message: '@AleAleE@'},
                            { field: 'AlertCategoryId', rule: 'required', message: '@AleAleF@'},
                            { field: 'Colour', rule: 'required', message: '@AleAleG@'},
                            { field: 'PositionId', rule: 'required', message: '@AleAleH@'}
                        ]
                    }";
        }
    }
}
