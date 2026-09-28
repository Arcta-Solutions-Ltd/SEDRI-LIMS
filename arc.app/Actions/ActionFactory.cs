using arc.common.Models;
using System;

namespace arc.app.Actions
{
    public class ActionFactory : IActionFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ActionFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IAction Get(string actionName, TokenInfoModel token)
        {
            return actionName.ToLower() switch
            {
                "publishreport" => new PublishReportAction(_serviceProvider),
                _ => null,
            };
        }
    }
}
