using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class DirectTestFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'directtestform',
                        title: '@TesDir@',
                        initialQuery: 'testlistforspecimenlite',
                        saveEvent: 'directtestentry',
                        SuppressRecordView: true,
                        pages: ['directtestpage']
                    }";

            return form;
        }
    }
}

