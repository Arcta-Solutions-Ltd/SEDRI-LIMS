using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class CultureTypeSelectionPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'culturetypeselectionpage',
                            pageTitle: '@SpeSelK@',
                            text: '@SpeSelM@.',
                            configureActions: 'nofields, delete',
                            tableName: 'None',
                            crafted: true
                        }";

            return page;
        }
    }
}
