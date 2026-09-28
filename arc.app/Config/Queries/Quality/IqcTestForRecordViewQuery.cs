using arc.app.Common;

namespace arc.app.Config.Queries;

internal class IqcTestForRecordViewQuery : IDefinition
{
    public string Get()
    {
        return """
            {
                "Query": "iqctestforrecordview",
                "TableName": "iqctests",
                "Type": "Single",
                "Translate": true,
                "ResultMapping": "iqctestviewmapper",
                "Fields": [
                    {
                        "Name": "id",
                        "Type": "int"
                    },
                    {
                        "Name": "stateid",
                        "Type": "int"
                    },
                    {
                        "Name": "createddate",
                        "Type": "datetime"
                    },
                    {
                        "Name": "completeddate",
                        "Type": "datetime"
                    }
                ],
                "Where": [
                    {
                        "Field": "Id",
                        "Comparison": "="
                    }
                ],
                "Joins": [
                    {
                        "Table": "iqctestprofiles",
                        "Type": "Left",
                        "On": "iqctestprofileid",
                        "From": "Id",
                        "Fields": [
                            {
                                "Name": "testmethodlistitemid",
                                "Type": "int",
                                "KnownAs": "testmethodid"
                            },
                            {
                                "Name": "name",
                                "Type": "string",
                                "KnownAs": "profilename"
                            }
                        ]
                    },
                    {
                        "Table": "listitem",
                        "Type": "Left",
                        "On": "stateid",
                        "From": "Id",
                        "Fields": [
                            {
                                "Name": "value",
                                "Type": "string",
                                "KnownAs": "state"
                            }
                        ]
                    },
                    {
                        "Table": "listitem",
                        "Type": "Left",
                        "On": "a.testmethodlistitemid  ",
                        "From": "Id",
                        "Fields": [
                            {
                                "Name": "value",
                                "Type": "string",
                                "KnownAs": "method"
                            }
                        ]
                    }
                ]
            }
            """;
    }
}
