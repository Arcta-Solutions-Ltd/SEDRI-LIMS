using arc.app.Common;

namespace arc.app.Config.Queries;

internal class QcAntimicrobialListByQcOrganismIdQuery : IDefinition
{
    public string Get()
    {
        return """
            {
                "Query": "qcantimicrobiallistbyqcorganismid",
                "TableName": "iqctestprofileqcantibiotics",
                "Type": "Select",
                "Translate": true,
                "Fields": [
                    {
                        "Name": "id",
                        "Type": "integer"
                    },
                    {
                        "Name": "iqctestprofileqcorganismid",
                        "Type": "integer"
                    },
                    {
                        "Name": "enabled",
                        "Type": "boolean",
                        "TrueValue": "@GenYesA@",
                        "FalseValue": "@GenNo@"
                    }
                ],
                "Joins": [
                    {
                        "Table": "qcantibiotics",
                        "Type": "Left",
                        "On": "qcantibioticid",
                        "From": "Id",
                        "Fields": [
                            {
                                "Name": "qcorganismid",
                                "Type": "integer"
                            },
                            {
                                "Name": "mictargetlower",
                                "Type": "decimal"
                            },
                            {
                                "Name": "mictargetupper",
                                "Type": "decimal"
                            },
                            {
                                "Name": "micrangelower",
                                "Type": "decimal"
                            },
                            {
                                "Name": "micrangeupper",
                                "Type": "decimal"
                            },
                            {
                                "Name": "diskcontent",
                                "Type": "string"
                            },
                            {
                                "Name": "inhibitionzonediametertargetlower",
                                "Type": "decimal"
                            },
                            {
                                "Name": "inhibitionzonediametertargetupper",
                                "Type": "decimal"
                            },
                            {
                                "Name": "inhibitionzonediameterrangelower",
                                "Type": "decimal"
                            },
                            {
                                "Name": "inhibitionzonediameterrangeupper",
                                "Type": "decimal"
                            }
                        ]
                    },
                    {
                        "Table": "antibiotic",
                        "Type": "Left",
                        "On": "a.antibioticid",
                        "From": "Id",
                        "Fields": [
                            {
                                "Name": "antibioticname",
                                "Type": "string"
                            }
                        ]
                    }
                ],
                "Where": [
                    {
                        "Field": "iqctestprofileqcorganismid",
                        "Comparison": "="
                    }
                ],
                "OrderBy": "antibioticname"
            }
            """;
    }
}
