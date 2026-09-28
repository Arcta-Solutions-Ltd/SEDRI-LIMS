using arc.app.Common;

namespace arc.app.Config.Reports;

/// <summary>
/// Provides configuration for the specimen record report format.
/// Implements IDefinition to return a JSON string that defines the report's structure and layout.
/// </summary>
internal class SpecimenRecordReportConfig : IDefinition
{
    /// <summary>
    /// Returns a JSON configuration string that defines the specimen record report layout.
    /// The configuration includes header, content sections, and footer definitions.
    /// </summary>
    /// <returns>A JSON string containing the complete report configuration including:
    /// - Header with laboratory and specimen information
    /// - Content sections for alerts, patient details, test results, and culture findings
    /// - Footer with printing information and page numbers</returns>
    public string Get()
    {
        return """
            {
                "Header": [
                    {
                        "Line": 1,
                        "Left": 20,
                        "Field": "LaboratoryName",
                        "FontSize": 16
                    },
                    {
                        "Line": 2,
                        "Left": 20,
                        "Text": "@RepMic@",
                        "FontSize": 20,
                        "Bold": true
                    },
                    {
                        "Line": 2,
                        "Left": 400,
                        "Field": "State",
                        "FontSize": 16
                    },
                    {
                        "Line": 3,
                        "Left": 20,
                        "Text": "@SpeSpeB@:"
                    },
                    {
                        "Line": 3,
                        "Left": 120,
                        "Field": "SpecimenType"
                    },
                    {
                        "Line": 3,
                        "Left": 400,
                        "Text": "@SpeColC@:"
                    },
                    {
                        "Line": 3,
                        "Left": 480,
                        "Field": "CollectionDate"
                    },
                    {
                        "Line": 4,
                        "Left": 20,
                        "Text": "@SpeAcc@:"
                    },
                    {
                        "Line": 4,
                        "Left": 120,
                        "Field": "AccessionNumber"
                    }
                ],
                "Contents": [
                    {
                        "Name": "TopAlertTable",
                        "Head": [
                            "@GenAleA@"
                        ],
                        "Type": "Table",
                        "Data": "TopAlerts",
                        "Left": 20,
                        "Separator": "line",
                        "Width": "500",
                        "Colour": "red"
                    },
                    {
                        "Name": "PatientDetails",
                        "Heading": [
                            {
                                "Line": 1,
                                "Left": 20,
                                "Text": "@PatDet@",
                                "FontSize": 14,
                                "Bold": true
                            }
                        ],
                        "Type": "DoubleFieldColumn",
                        "Column1": {
                            "Left": 20,
                            "Width": 240,
                            "LabelWidth": 80,
                            "Fields": [
                                {
                                    "Label": "@RepRef@",
                                    "Value": "PatientRef"
                                },
                                {
                                    "Label": "@PatFir@",
                                    "Value": "FirstName"
                                },
                                {
                                    "Label": "@PatDat@",
                                    "Value": "DateOfBirth"
                                }
                            ]
                        },
                        "Column2": {
                            "Left": 300,
                            "Width": 240,
                            "LabelWidth": 80,
                            "Fields": [
                                {
                                    "Label": "@PatGenA@",
                                    "Value": "Gender"
                                },
                                {
                                    "Label": "@PatSurA@",
                                    "Value": "Surname"
                                },
                                {
                                    "Label": "@PatAgeA@",
                                    "Value": "Age"
                                },
                                {
                                    "Label": "@PatAgeB@",
                                    "Value": "AgeMonths"
                                }
                            ]
                        }
                    },
                    {
                        "Name": "location",
                        "Heading": [
                            {
                                "Line": 1,
                                "Left": 0,
                                "Text": "",
                                "FontSize": 14,
                                "Bold": true
                            }
                        ],
                        "Type": "SingleFieldColumnWithRowHeading",
                        "Group": "Organism",
                        "Column1": {
                            "Left": 20,
                            "Width": 520,
                            "LabelWidth": 80,
                            "Fields": [
                                {
                                    "Label": "@GenLoc@",
                                    "Value": "fullyqualifiedname"
                                }
                            ]
                        }
                    },
                    {
                        "Name": "PrecultureResults",
                        "Heading": [
                            {
                                "Line": 1,
                                "Left": 20,
                                "Text": "@SpeDir@",
                                "FontSize": 14,
                                "Bold": true
                            }
                        ],
                        "Type": "DoubleFieldColumn",
                        "Column1": {
                            "Left": 20,
                            "Width": 240,
                            "LabelWidth": 80,
                            "Fields": [
                                {
                                    "Label": "@RepApp@",
                                    "Value": "SpecimenAppearance"
                                }
                            ]
                        },
                        "Column2": {
                            "Left": 300,
                            "Width": 240,
                            "LabelWidth": 80,
                            "Fields": [
                            ]
                        }
                    },
                    {
                        "Name": "CellCount",
                        "Heading": [
                            {
                                "Line": 1,
                                "Left": 20,
                                "Text": "@RepCel@",
                                "FontSize": 12,
                                "Bold": true
                            }
                        ],
                        "Type": "DoubleFieldColumnWithRowHeading",
                        "Column1": {
                            "Left": 100,
                            "Width": 220,
                            "LabelWidth": 100,
                            "Fields": [
                                {
                                    "Label": "@TesWbc@",
                                    "Value": "CCWbc"
                                },
                                {
                                    "Label": "@TesRbc@",
                                    "Value": "CCRbc"
                                },
                                {
                                    "Label": "@TesWbcA@",
                                    "Value": "WbcQualitative"
                                }
                            ]
                        },
                        "Column2": {
                            "Left": 330,
                            "Width": 220,
                            "LabelWidth": 100,
                            "Fields": [
                                {
                                    "Label": "@TesRbcA@",
                                    "Value": "RbcQualitative"
                                },
                                {
                                    "Label": "@TesPol@",
                                    "Value": "Polymorphonuclear"
                                },
                                {
                                    "Label": "@TesMon@",
                                    "Value": "Mononuclear"
                                }
                            ]
                        }
                    },
                    {
                        "Name": "GramStain",
                        "Heading": [
                            {
                                "Line": 1,
                                "Left": 20,
                                "Text": "@RepGra@",
                                "FontSize": 12,
                                "Bold": true
                            }
                        ],
                        "Type": "DoubleFieldColumnWithRowHeading",
                        "LinkedSections": 1,
                        "Column1": {
                            "Left": 100,
                            "Width": 180,
                            "LabelWidth": 100,
                            "Fields": [
                                {
                                    "Label": "@TesWbcB@",
                                    "Value": "wbc"
                                }
                            ]
                        },
                        "Column2": {
                            "Left": 300,
                            "Width": 180,
                            "LabelWidth": 100,
                            "Fields": [
                                {
                                    "Label": "@TesEpi@",
                                    "Value": "Epicells"
                                }
                            ]
                        }
                    },
                    {
                        "Name": "GramStainTable",
                        "Type": "Table",
                        "Data": "GramStain",
                        "Theme": "grid",
                        "Left": 100,
                        "Width": "300|80"
                    },
                    {
                        "Name": "ZNStain",
                        "Heading": [
                            {
                                "Line": 1,
                                "Left": 20,
                                "Text": "@RepZns@",
                                "FontSize": 12,
                                "Bold": true
                            }
                        ],
                        "Type": "SingleFieldColumnWithRowHeading",
                        "Column1": {
                            "Left": 100,
                            "Width": 180,
                            "LabelWidth": 100,
                            "Fields": [
                                {
                                    "Label": "@TesAfb@",
                                    "Value": "AFBQuantity"
                                }
                            ]
                        }
                    },
                    {
                        "Name": "IndiaInk",
                        "Heading": [
                            {
                                "Line": 1,
                                "Left": 20,
                                "Text": "@RepInd@",
                                "FontSize": 12,
                                "Bold": true
                            }
                        ],
                        "Type": "DoubleFieldColumnWithRowHeading",
                        "Column1": {
                            "Left": 100,
                            "Width": 180,
                            "LabelWidth": 100,
                            "Fields": [
                                {
                                    "Label": "@RepRes@",
                                    "Value": "IndiaInkResult"
                                }
                            ]
                        },
                        "Column2": {
                            "Left": 300,
                            "Width": 180,
                            "LabelWidth": 100,
                            "Fields": [
                                {
                                    "Label": "@TesPos@",
                                    "Value": "PositiveResult"
                                }
                            ]
                        }
                    },
                    {
                        "Name": "WetPrep",
                        "Heading": [
                            {
                                "Line": 1,
                                "Left": 20,
                                "Text": "@RepWet@",
                                "FontSize": 12,
                                "Bold": true
                            }
                        ],
                        "Type": "DoubleFieldColumnWithRowHeading",
                        "LinkedSections": 1,
                        "Column1": {
                            "Left": 100,
                            "Width": 180,
                            "LabelWidth": 100,
                            "Fields": [
                                {
                                    "Label": "@TesWbcB@",
                                    "Value": "wbcwetprep"
                                }
                            ]
                        },
                        "Column2": {
                            "Left": 300,
                            "Width": 180,
                            "LabelWidth": 100,
                            "Fields": [
                                {
                                    "Label": "@TesRbcB@",
                                    "Value": "rbcwetprep"
                                }
                            ]
                        }
                    },
                    {
                        "Name": "WetPrepTable",
                        "Type": "Table",
                        "Data": "WetPrep",
                        "Left": 100,
                        "Theme": "grid",
                        "Width": "150|150|80"
                    },
                    {
                        "Name": "Auramine",
                        "Heading": [
                            {
                                "Line": 1,
                                "Left": 20,
                                "Text": "@RepAur@",
                                "FontSize": 12,
                                "Bold": true
                            }
                        ],
                        "Type": "SingleFieldColumnWithRowHeading",
                        "Column1": {
                            "Left": 100,
                            "Width": 180,
                            "LabelWidth": 100,
                            "Fields": [
                                {
                                    "Label": "@TesTesA@",
                                    "Value": "AuramineId"
                                }
                            ]
                        }
                    },
                    {
                        "Name": "Dipstick",
                        "Heading": [
                            {
                                "Line": 1,
                                "Left": 20,
                                "Text": "@RepDip@",
                                "FontSize": 12,
                                "Bold": true
                            }
                        ],
                        "Type": "DoubleFieldColumnWithRowHeading",
                        "Column1": {
                            "Left": 100,
                            "Width": 180,
                            "LabelWidth": 100,
                            "Fields": [
                                {
                                    "Label": "@TesPh@",
                                    "Value": "phId"
                                },
                                {
                                    "Label": "@TesProA@",
                                    "Value": "proteinId"
                                },
                                {
                                    "Label": "@TesGluA@",
                                    "Value": "glucoseId"
                                },
                                {
                                    "Label": "@TesLeu@",
                                    "Value": "leucocytesId"
                                }
                            ]
                        },
                        "Column2": {
                            "Left": 300,
                            "Width": 180,
                            "LabelWidth": 100,
                            "Fields": [
                                {
                                    "Label": "@TesSpe@",
                                    "Value": "specificGravityId"
                                },
                                {
                                    "Label": "@TesKet@",
                                    "Value": "ketonesId"
                                },
                                {
                                    "Label": "@GenBlo@",
                                    "Value": "bloodId"
                                },
                                {
                                    "Label": "@TesNit@",
                                    "Value": "nitritesId"
                                }
                            ]
                        }
                    },
                    {
                        "Name": "hpyloriantigen",
                        "Heading": [
                            {
                                "Line": 1,
                                "Left": 20,
                                "Text": "@TesHpyA@",
                                "FontSize": 12,
                                "Bold": true
                            }
                        ],
                        "Type": "SingleFieldColumnWithRowHeading",
                        "Column1": {
                            "Left": 100,
                            "Width": 180,
                            "LabelWidth": 100,
                            "Fields": [
                                {
                                    "Label": "@TesTesA@",
                                    "Value": "antResultId"
                                }
                            ]
                        }
                    },
                    {
                        "Name": "jevserology",
                        "Heading": [
                            {
                                "Line": 1,
                                "Left": 20,
                                "Text": "@TesJevA@",
                                "FontSize": 12,
                                "Bold": true
                            }
                        ],
                        "Type": "SingleFieldColumnWithRowHeading",
                        "Column1": {
                            "Left": 100,
                            "Width": 180,
                            "LabelWidth": 100,
                            "Fields": [
                                {
                                    "Label": "@TesTesA@",
                                    "Value": "jevserologyResultId"
                                }
                            ]
                        }
                    },
                    {
                        "Name": "KohPrep",
                        "Heading": [
                            {
                                "Line": 1,
                                "Left": 20,
                                "Text": "@TesFunA@",
                                "FontSize": 12,
                                "Bold": true
                            }
                        ],
                        "Type": "DoubleFieldColumnWithRowHeading",
                        "Column1": {
                            "Left": 100,
                            "Width": 180,
                            "LabelWidth": 100,
                            "Fields": [
                                {
                                    "Label": "@RepRes@",
                                    "Value": "kohResultId"
                                }
                            ]
                        },
                        "Column2": {
                            "Left": 300,
                            "Width": 180,
                            "LabelWidth": 100,
                            "Fields": [
                                {
                                    "Label": "@TesPos@",
                                    "Value": "KohFungalId"
                                }
                            ]
                        }
                    },
                    {
                        "Name": "Microscopy",
                        "Heading": [
                            {
                                "Line": 1,
                                "Left": 20,
                                "Text": "@TesMicA@",
                                "FontSize": 12,
                                "Bold": true
                            }
                        ],
                        "Type": "DoubleFieldColumnWithRowHeading",
                        "LinkedSections": 1,
                        "Column1": {
                            "Left": 100,
                            "Width": 180,
                            "LabelWidth": 100,
                            "Fields": [
                                {
                                    "Label": "@TesEpiA@",
                                    "Value": "epitheliumId"
                                },
                                {
                                    "Label": "@GenYea@",
                                    "Value": "yeastId"
                                }
                            ]
                        },
                        "Column2": {
                            "Left": 300,
                            "Width": 180,
                            "LabelWidth": 100,
                            "Fields": [
                                {
                                    "Label": "@GenBacA@",
                                    "Value": "bacteriaId"
                                }
                            ]
                        }
                    },
                    {
                        "Name": "CrystalTable",
                        "Type": "Table",
                        "Data": "Crystal",
                        "Theme": "grid",
                        "Left": 100,
                        "Width": "300|80"
                    },
                    {
                        "Name": "pregnancy",
                        "Heading": [
                            {
                                "Line": 1,
                                "Left": 20,
                                "Text": "@TesPreA@",
                                "FontSize": 12,
                                "Bold": true
                            }
                        ],
                        "Type": "SingleFieldColumnWithRowHeading",
                        "Column1": {
                            "Left": 100,
                            "Width": 180,
                            "LabelWidth": 100,
                            "Fields": [
                                {
                                    "Label": "@TesTesA@",
                                    "Value": "pregnancyId"
                                }
                            ]
                        }
                    },
                    {
                        "Name": "Biochemistry",
                        "Heading": [
                            {
                                "Line": 1,
                                "Left": 20,
                                "Text": "@TesBioA@",
                                "FontSize": 12,
                                "Bold": true
                            }
                        ],
                        "Type": "DoubleFieldColumnWithRowHeading",
                        "Column1": {
                            "Left": 100,
                            "Width": 180,
                            "LabelWidth": 100,
                            "Fields": [
                                {
                                    "Label": "@TesGlu@",
                                    "Value": "glucose"
                                }
                            ]
                        },
                        "Column2": {
                            "Left": 300,
                            "Width": 180,
                            "LabelWidth": 100,
                            "Fields": [
                                {
                                    "Label": "@TesPro@",
                                    "Value": "protein"
                                }
                            ]
                        }
                    },
                    {
                        "Name": "wrightsstain",
                        "Heading": [
                            {
                                "Line": 1,
                                "Left": 20,
                                "Text": "@TesWriA@",
                                "FontSize": 12,
                                "Bold": true
                            }
                        ],
                        "Type": "SingleFieldColumnWithRowHeading",
                        "Column1": {
                            "Left": 100,
                            "Width": 180,
                            "LabelWidth": 100,
                            "Fields": [
                                {
                                    "Label": "@TesTesA@",
                                    "Value": "wrightsstainresultId"
                                }
                            ]
                        }
                    },
                    {
                        "Name": "CultureResult",
                        "Heading": [
                            {
                                "Line": 1,
                                "Left": 20,
                                "Text": "@RepCul@",
                                "FontSize": 14,
                                "Bold": true
                            }
                        ],
                        "Type": "DoubleFieldColumn",
                        "Group": "Organism",
                        "Multiple": true,
                        "MultipleName": "Organisms",
                        "LinkedSections": 5,
                        "Column1": {
                            "Left": 20,
                            "Width": 240,
                            "LabelWidth": 80,
                            "Fields": [
                                {
                                    "Label": "@GenOrgA@",
                                    "Value": "SpecimenOrganism"
                                }
                            ]
                        },
                        "Column2": {
                            "Left": 300,
                            "Width": 240,
                            "LabelWidth": 80,
                            "Fields": [
                                {
                                    "Label": "@GenQua@",
                                    "Value": "SpecimenQuantity"
                                }
                            ]
                        }
                    },
                    {
                        "Name": "esbl",
                        "Heading": [
                            {
                                "Line": 1,
                                "Left": 20,
                                "Text": "@TesEsbA@",
                                "FontSize": 12,
                                "Bold": true
                            }
                        ],
                        "Type": "SingleFieldColumnWithRowHeading",
                        "Group": "Organism",
                        "Column1": {
                            "Left": 100,
                            "Width": 180,
                            "LabelWidth": 100,
                            "Fields": [
                                {
                                    "Label": "@TesTesA@",
                                    "Value": "esblresultId"
                                }
                            ]
                        }
                    },
                    {
                        "Name": "betalactamase",
                        "Heading": [
                            {
                                "Line": 1,
                                "Left": 20,
                                "Text": "@TesBetA@",
                                "FontSize": 12,
                                "Bold": true
                            }
                        ],
                        "Type": "SingleFieldColumnWithRowHeading",
                        "Group": "Organism",
                        "Column1": {
                            "Left": 100,
                            "Width": 180,
                            "LabelWidth": 100,
                            "Fields": [
                                {
                                    "Label": "@TesTesA@",
                                    "Value": "betalactamaseresultId"
                                }
                            ]
                        }
                    },
                    {
                        "Name": "carbapenemase",
                        "Heading": [
                            {
                                "Line": 1,
                                "Left": 20,
                                "Text": "@TesCarC@",
                                "FontSize": 12,
                                "Bold": true
                            }
                        ],
                        "Type": "SingleFieldColumnWithRowHeading",
                        "Group": "Organism",
                        "Column1": {
                            "Left": 100,
                            "Width": 180,
                            "LabelWidth": 100,
                            "Fields": [
                                {
                                    "Label": "@TesTesA@",
                                    "Value": "carbapenemaseresultId"
                                }
                            ]
                        }
                    },
                    {
                        "Name": "ApiPanel",
                        "Heading": [
                            {
                                "Line": 1,
                                "Left": 20,
                                "Text": "@TesApiB@",
                                "FontSize": 12,
                                "Bold": true
                            }
                        ],
                        "Type": "DoubleFieldColumnWithRowHeading",
                        "Group": "Organism",
                        "Column1": {
                            "Left": 100,
                            "Width": 180,
                            "LabelWidth": 100,
                            "Fields": [
                                {
                                    "Label": "@SpeApi@",
                                    "Value": "APIIDPanel"
                                },
                                {
                                    "Label": "@SpeIdA@",
                                    "Value": "PercentageID"
                                }
                            ]
                        },
                        "Column2": {
                            "Left": 300,
                            "Width": 180,
                            "LabelWidth": 100,
                            "Fields": [
                                {
                                    "Label": "@SpeId@",
                                    "Value": "IdProfile"
                                }
                            ]
                        }
                    },
                    {
                        "Name": "OrganismListTable",
                        "Head": [
                            "@RepAnt@",
                            "@RepSen@",
                            "@RepAnt@",
                            "@RepSen@"
                        ],
                        "Type": "Table",
                        "Group": "Organism",
                        "Data": "OrganismList",
                        "Left": 20,
                        "Width": "150|150|150"
                    },
                    {
                        "Name": "astadditionalnotes",
                        "Heading": [
                            {
                                "Line": 1,
                                "Left": 20,
                                "Text": "",
                                "FontSize": 12,
                                "Bold": false
                            }
                        ],
                        "Type": "SingleFieldColumnWithRowHeading",
                        "Group": "Organism",
                        "Separator": "line",
                        "Column1": {
                            "Left": 20,
                            "Width": 520,
                            "LabelWidth": 80,
                            "Fields": [
                                {
                                    "Label": "@GenNot@",
                                    "Value": "astadditionalnotes"
                                }
                            ]
                        }
                    },
                    {
                        "Name": "BottomAlertTable",
                        "Head": [
                            "@GenAleB@"
                        ],
                        "Type": "Table",
                        "Data": "BottomAlerts",
                        "Left": 20,
                        "Separator": "line",
                        "Width": "500",
                        "Colour": "lightblue"
                    },
                    {
                        "Name": "Approvals",
                        "Heading": [
                            {
                                "Line": 1,
                                "Left": 20,
                                "Text": "@SpeAppA@",
                                "FontSize": 14,
                                "Bold": true
                            }
                        ],
                        "Type": "DoubleFieldColumn",
                        "Column1": {
                            "Left": 20,
                            "Width": 240,
                            "LabelWidth": 80,
                            "Fields": [
                                {
                                    "Label": "@SpeAppB@",
                                    "Value": "SubmittedBy"
                                },
                                {
                                    "Label": "@SpeAppC@",
                                    "Value": "ApprovedBy"
                                }
                            ]
                        },
                        "Column2": {
                            "Left": 300,
                            "Width": 240,
                            "LabelWidth": 80,
                            "Fields": [
                                {
                                    "Label": "@SpeSubDat@",
                                    "Value": "SubmittedDate"
                                },
                                {
                                    "Label": "@SpeAppDat@",
                                    "Value": "ApprovedDate"
                                }
                            ]
                        }
                    }
                ],
                "Footer": [
                    {
                        "Line": 1,
                        "Left": 20,
                        "Text": "@RepIfy@"
                    },
                    {
                        "Line": 2,
                        "Left": 20,
                        "Calc": "PrintedDate"
                    },
                    {
                        "Line": 2,
                        "Left": 530,
                        "Calc": "Pages"
                    }
                ]
            }
            """;
    }
}
