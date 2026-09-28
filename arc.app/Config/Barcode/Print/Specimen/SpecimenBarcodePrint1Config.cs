using arc.app.Common;

namespace arc.app.Config.Barcode
{
    internal class SpecimenBarcodePrint1Config : IDefinition
    {
        public string Get()
        {
            return """
            {
              "barcodePadding": "0.2cm",
              "border": true,
              "bottomPadding": "0.2cm",
              "displayCode": true,
              "fieldFontSize": "8pt",
              "fieldNameWidth": "3cm",
              "FieldQuery": "SingleSpecimenForSpecimenLabel",
              "fieldTotalWidth": "6cm",
              "id": 1,
              "itemHeight": "auto",
              "itemsPerRow": 2,
              "itemWidth": "auto",
              "labelFields": "AccessionNumber,FirstName,Surname,CollectionDate,ReceivedDate",
              "LabelCaptions": {
                "accessionnumber": "@SpeAcc@",
                "firstname": "@PatFir@",
                "surname": "@PatSurA@",
                "collectiondate": "@SpeColC@",
                "receiveddate": "@SpeRecD@"
              },
              "leftMargin": "0.5cm",
              "linear": true,
              "linearHeight": "60",
              "name": "SpecimenBarcodePrint1",
              "numberOfRows": 2,
              "qr": true,
              "QRSize": 64,
              "ReferenceForm": "createspecimenreceivedform",
              "sideBySide": false,
              "title": "Specimen Label 1",
              "topMargin": "0.5cm"
            }
            """;
        }
    }
}
