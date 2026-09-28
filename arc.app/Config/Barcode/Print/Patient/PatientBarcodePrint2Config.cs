using arc.app.Common;

namespace arc.app.Config.Barcode
{
    internal class PatientBarcodePrint2Config : IDefinition
    {
        public string Get()
        {
            return """
            {
              "barcodePadding": "0.2cm",
              "border": false,
              "bottomPadding": "0.2cm",
              "displayCode": true,
              "fieldFontSize": "8pt",
              "fieldNameWidth": "3cm",
              "FieldQuery": "SinglePatientForPatientLabel",
              "fieldTotalWidth": "6cm",
              "id": 2,
              "itemHeight": "auto",
              "ItemsPerRow": 2,
              "itemWidth": "auto",
              "labelFields": "FirstName,Surname",
              "LabelCaptions": {
                "firstname": "@PatFir@",
                "surname": "@PatSurA@"
              },
              "leftMargin": "0.5cm",
              "linear": true,
              "linearHeight": "60",
              "name": "PatientBarcodePrint2",
              "NumberOfRows": 2,
              "qr": false,
              "QRSize": 64,
              "ReferenceForm": "createspecimenreceivedform",
              "sideBySide": true,
              "title": "Patient Label 2",
              "topMargin": "0.5cm"
            }
            """;
        }
    }
}
