using arc.app.Common;

namespace arc.app.Config.Barcode
{
    internal class PatientBarcodePrint1Config : IDefinition
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
              "FieldQuery": "SinglePatientForPatientLabel",
              "fieldTotalWidth": "6cm",
              "id": 1,
              "itemHeight": "auto",
              "itemsPerRow": 2,
              "itemWidth": "auto",
              "labelFields": "",
              "LabelCaptions": {},
              "leftMargin": "0.5cm",
              "linear": true,
              "linearHeight": "60",
              "name": "PatientBarcodePrint1",
              "numberOfRows": 2,
              "qr": false,
              "QRSize": 64,
              "ReferenceForm": "createspecimenreceivedform",
              "sideBySide": false,
              "title": "Patient Label 1",
              "topMargin": "0.5cm"
            }
            """;
        }
    }
}
