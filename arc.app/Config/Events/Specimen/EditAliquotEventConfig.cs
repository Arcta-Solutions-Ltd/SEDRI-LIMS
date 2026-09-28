using arc.app.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arc.app.Config.Events.Specimen
{
    internal class EditAliquotEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{
                        EventName: 'editaliquotevent',
                        Description: '@SpeAliA@',
                        EventType : 'editdata',
                        Topic : 'Culture',
                        TableName: 'Culture',
                        Mapping: 'editaliquotmapper',
                        Display: [
                            { Label: 'aliquotid', Translation: '', List: 'No' },
                        ]
                    }";
        }
    }
}
