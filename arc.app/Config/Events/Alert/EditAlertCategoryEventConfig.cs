using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class EditAlertCategoryEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'editAlertCategory', 
                        Description: '@AleEdiC@',
                        EventType : 'editdata', 
                        Topic : 'Alert', 
                        TableName: 'AlertType',
                        ValidationRules: [
                            { field: 'Name', rule: 'required', message: '@AleAleE@'},
                            { field: 'AlertCategoryId', rule: 'required', message: '@AleAleF@'},
                            { field: 'Colour', rule: 'required', message: '@AleAleG@'}
                        ]
                    }";
        }
    }
}
