using arc.app.Common;

namespace arc.app.Config.Barcode
{
    internal class SpecimenBarcodePrint2Config : IDefinition
    {
        public string Get()
        {
            return """
            {
              "barcodePadding": "0.2cm",
              "border": false,
              "bottomPadding": "0.2cm",
              "displayCode": true,
              "fieldFontSize": "6pt",
              "fieldNameWidth": "3cm",
              "FieldQuery": "SingleSpecimenForSpecimenLabel",
              "fieldTotalWidth": "6cm",
              "id": 2,
              "itemHeight": "auto",
              "itemsPerRow": 3,
              "itemWidth": "auto",
              "labelFields": "AccessionNumber,CollectionDate,ReceivedDate",
              "LabelCaptions": {
                "accessionnumber": "@SpeAcc@",
                "collectiondate": "@SpeColC@",
                "receiveddate": "@SpeRecD@"
              },
              "leftMargin": "0.5cm",
              "linear": true,
              "linearHeight": "60",
              "name": "SpecimenBarcodePrint2",
              "numberOfRows": 3,
              "qr": false,
              "QRSize": 64,
              "ReferenceForm": "createspecimenreceivedform",
              "sideBySide": false,
              "title": "Specimen Label 2",
              "topMargin": "0.5cm"
            }
            """;
        }
    }
}
