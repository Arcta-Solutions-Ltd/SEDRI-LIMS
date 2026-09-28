using arc.app.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arc.app.Config.Pages.Specimen
{
    internal class EditAliquotPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{
                              name: 'editaliquotpage',
                              text: '@SpeAliA@.',
                              columns: [
                                {
                                  key: 'col1',
                                  itemWidth: 'wide',
                                  fieldWidth: 'wide',
                                  formGroups: [
                                    {
                                      key: 'fg1',
                                      fields: [
                                        {
                                          id: 'AliquotID',
                                          Max: 30,
                                          type: 'singleline',
                                          label: '@SpeAli@',
                                          required: false,
                                          placeholder: '@SpeProH@'
                                        }
                                      ]
                                    }
                                  ]
                                }
                              ],
                              pageTitle: '@SpeAliA@'                   
                        }";

            return page;
        }
    }
}
