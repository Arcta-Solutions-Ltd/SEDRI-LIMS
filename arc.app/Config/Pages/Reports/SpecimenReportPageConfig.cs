using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class SpecimenReportPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'specimenreportpage',
                            pageTitle: '@RepSpe@',
                            text: '@RepPri@.',
                            crafted: true,
                            nextButton: { show: false }
                        }";

            return page;
        }
    }
}
