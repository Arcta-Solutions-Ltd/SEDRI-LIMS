using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class EditWordEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'editWord', 
                        Description: '@LanEdiA@',
                        Topic: 'Language',
                        TableName: 'Language',
                        EventType : 'special',
                        ValidationRules: [
                            { field: 'Value', rule: 'required', message: '@LanAB@'}
                        ],
                    }";
        }
    }
}
