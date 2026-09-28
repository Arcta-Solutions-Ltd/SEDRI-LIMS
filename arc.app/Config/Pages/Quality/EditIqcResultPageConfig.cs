using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class EditIqcResultPageConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                name: 'editiqcresultpage',
                pageTitle: '@QuaEdiTesRes@',
                text: '@QuaEdiTesRes@.',
                crafted: true
            }";
        }
    }
}
