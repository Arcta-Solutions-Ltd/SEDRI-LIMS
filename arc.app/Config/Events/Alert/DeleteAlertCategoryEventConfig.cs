using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class DeleteAlertCategoryEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'deleteAlertCategory', 
                        Description: '@AleDelE@',
                        EventType : 'deletedata', 
                        Topic : 'Alert', 
                        TableName: 'AlertType',
                        ValidationRules: [
                            { field: 'Name', rule: 'required', message: '@AleAleE@'}
                        ],
                        DataRules: [
                            { type: 'NoRecord', query: 'alerttypeusedinalert', message: '@AleAleI@' }
                        ]
                    }";
        }
    }
}
