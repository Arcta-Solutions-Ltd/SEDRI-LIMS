using arc.app.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace arc.app.Config.Events.Export
{
    internal class RunExportProfileEventConfig:IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'runexportprofile', 
                        Description: '@RunExpT@',
                        EventType : 'special', 
                        Topic : 'Export'
                    }";
        }
    }
}
