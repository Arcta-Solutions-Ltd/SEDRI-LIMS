using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class EditSpecimenBarcodeContentPageConfig : IDefinition
    {
        public string Get()
        {
            return """
                {
                  "columns": [
                    {
                      "fieldWidth": "wide",
                      "formGroups": [
                        {
                          "fields": [
                            {
                              "id": "title",
                              "label": "@CfgLabR@",
                              "type": "text"
                            },
                            {
                              "id": "Linear",
                              "label": "@CfgLabS@",
                              "type": "toggle"
                            },
                            {
                              "id": "QR",
                              "label": "@CfgLabT@",
                              "type": "toggle"
                            },
                            {
                              "id": "UseAccessionNumberForBarcode",
                              "label": "@CfgLabUseAcc@",
                              "type": "toggle"
                            },
                            {
                              "id": "DisplayCode",
                              "label": "@CfgLabU@",
                              "type": "toggle"
                            },
                            {
                              "id": "LabelFields",
                              "label": "@CfgLabV@",
                              "parameter1": "specimenlabelavailablefields",
                              "parameter2": "createspecimenreceivedform",
                              "type": "crafted"
                            },
                            {
                              "id": "Border",
                              "label": "@CfgLabW@",
                              "type": "toggle"
                            },
                            {
                              "id": "ItemsPerRow",
                              "label": "@CfgLabX@",
                              "required": true,
                              "type": "singleline"
                            },
                            {
                              "id": "NumberOfRows",
                              "label": "@CfgLabY@",
                              "required": true,
                              "type": "singleline"
                            },
                            {
                              "id": "ReferenceForm",
                              "type": "hidden"
                            },
                            {
                              "id": "FieldQuery",
                              "type": "hidden"
                            }
                          ],
                          "key": "fg1"
                        }
                      ],
                      "itemWidth": "wide",
                      "key": "col1"
                    }
                  ],
                  "name": "editspecimenbarcodecontentpage",
                  "pageTitle": "@CfgLabP@",
                  "text": "@CfgLabQ@."
                }
                """;
        }
    }
}

