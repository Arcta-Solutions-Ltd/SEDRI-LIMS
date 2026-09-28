using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class PregnancyTestFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'pregnancytestform',
                        uievent: 'pregnancytestuievent',
                        title: '@TesPre@',
                        saveEvent: 'pregnancytest',
                        initialQuery: 'pregnancytestbyid',
                        suppressRecordView: true,
                        formtype: 'directtest',
                        datasection: 'PregnancyDataSection',
                        configurable: 'Yes',
                        pages: ['pregnancytestpage']
                    }";

            return form;
        }
    }
}
