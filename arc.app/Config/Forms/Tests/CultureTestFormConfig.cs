using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class CultureTestFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'culturetestform',
                        title: 'Culture Tests',
                        initialQuery: 'testlistforculturelite',
                        saveEvent: 'culturetestentry',
                        SuppressRecordView: true,
                        pages: ['culturetestpage']
                    }";

            return form;
        }
    }
}
