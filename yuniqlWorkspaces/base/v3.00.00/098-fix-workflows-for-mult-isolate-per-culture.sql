-- The add/edit culture event handling was broken with the introduction of multiple isolates per culture,
-- and the add//edit isolate event handling was also correct.  The following workflow configs fix these
-- issues for the two impacted built-in workflows.
UPDATE configs
SET    contents = '
{
  "Name": "SpecimenDefault",
  "Field": "stateid",
  "Steps": [
    {
      "event": "newreceivedspecimen",
      "actions": {
        "options": [
          {
            "action": "PublishReport",
            "newState": "528",
            "conditions": [
              {
                "field": "action",
                "value": "507"
              }
            ],
            "parameters": {
              "reportname": "@RepFin@",
              "reportconfig": "DefaultSpecimenReport",
              "doreportsneedapproval": "no"
            },
            "conditiontype": "and"
          }
        ]
      },
      "entrystate": ""
    },
    {
      "event": "ACKReceipt",
      "actions": {
        "options": [
          {
            "action": "PublishReport",
            "newState": "528",
            "parameters": {
              "reportname": "@RepFin@",
              "reportconfig": "DefaultSpecimenReport",
              "doreportsneedapproval": "no"
            }
          }
        ]
      },
      "exitstate": {
        "default": "526",
        "options": [
          {
            "newstate": "527",
            "conditions": [
              {
                "field": "action",
                "value": "509"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "528",
            "conditions": [
              {
                "field": "action",
                "value": "507"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "535",
            "conditions": [
              {
                "field": "action",
                "value": "508"
              }
            ],
            "conditiontype": "and"
          }
        ]
      },
      "entrystate": "525"
    },
    {
      "event": "RejectSpecimen",
      "actions": {
        "options": [
          {
            "action": "PublishReport",
            "newState": "528",
            "parameters": {
              "reportname": "@RepFin@",
              "reportconfig": "DefaultSpecimenReport",
              "doreportsneedapproval": "no"
            }
          }
        ]
      },
      "exitstate": {
        "default": "528"
      },
      "entrystate": "526"
    },
    {
      "event": "TestSelection",
      "exitstate": {
        "options": [
          {
            "newstate": "527",
            "conditions": [
              {
                "currentstate": "526"
              }
            ],
            "conditiontype": "and"
          }
        ]
      },
      "entryState": "526, 527, 529, 532, 533, 535"
    },
    {
      "event": "directtestentry",
      "exitstate": {
        "options": [
          {
            "newstate": "527",
            "conditions": [
              {
                "currentstate": "526"
              }
            ],
            "conditiontype": "and"
          }
        ]
      },
      "entryState": "526, 527, 529, 532, 533, 535"
    },
    {
      "event": "SpecimenCancelRequest",
      "exitstate": {
        "default": "537"
      },
      "entryState": "525"
    },
    {
      "event": "EditSpecimen",
      "entryState": "525, 526, 527, 529, 532, 533, 535"
    },
    {
      "event": "addCulture",
      "exitstate": {
        "default": "535",
        "options": [
          {
            "newstate": "532",
            "conditions": [
              {
                "currentstate": "532"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "533",
            "conditions": [
              {
                "currentstate": "533"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "529",
            "conditions": [
              {
                "currentstate": "529"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "529",
            "conditions": [
              {
                "field": "growthid",
                "value": "178"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "529",
            "conditions": [
              {
                "field": "growthid",
                "value": "179"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "529",
            "conditions": [
              {
                "field": "growthid",
                "value": "180"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "529",
            "conditions": [
              {
                "field": "growthid",
                "value": "181"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "529",
            "conditions": [
              {
                "field": "growthid",
                "value": "126"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "529",
            "conditions": [
              {
                "field": "growthid",
                "value": "1087"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "535",
            "conditions": [
              {
                "field": "growthid",
                "value": "125"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "535",
            "conditions": [
              {
                "field": "growthid",
                "value": "177"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "535",
            "conditions": [
              {
                "field": "growthid",
                "value": "182"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "535",
            "conditions": [
              {
                "field": "growthid",
                "value": "1050"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "535",
            "conditions": [
              {
                "field": "growthid",
                "value": "1086"
              }
            ],
            "conditiontype": "and"
          }
        ]
      },
      "entryState": "526, 527, 529, 532, 533, 535"
    },
    {
      "event": "addisolateevent",
      "exitstate": {
        "default": "535",
        "options": [
          {
            "newstate": "532",
            "conditions": [
              {
                "currentstate": "532"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "533",
            "conditions": [
              {
                "currentstate": "533"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "529",
            "conditions": [
              {
                "currentstate": "529"
              }
            ],
            "conditiontype": "and"
          }
        ]
      },
      "entryState": "526, 527, 529, 532, 533, 535"
    },
    {
      "event": "editCulture",
      "exitstate": {
        "default": "535",
        "options": [
          {
            "newstate": "532",
            "conditions": [
              {
                "currentstate": "532"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "533",
            "conditions": [
              {
                "currentstate": "533"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "529",
            "conditions": [
              {
                "currentstate": "529"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "529",
            "conditions": [
              {
                "field": "growthid",
                "value": "178"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "529",
            "conditions": [
              {
                "field": "growthid",
                "value": "179"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "529",
            "conditions": [
              {
                "field": "growthid",
                "value": "180"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "529",
            "conditions": [
              {
                "field": "growthid",
                "value": "181"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "529",
            "conditions": [
              {
                "field": "growthid",
                "value": "126"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "529",
            "conditions": [
              {
                "field": "growthid",
                "value": "1087"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "535",
            "conditions": [
              {
                "field": "growthid",
                "value": "125"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "535",
            "conditions": [
              {
                "field": "growthid",
                "value": "177"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "535",
            "conditions": [
              {
                "field": "growthid",
                "value": "182"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "535",
            "conditions": [
              {
                "field": "growthid",
                "value": "1050"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "535",
            "conditions": [
              {
                "field": "growthid",
                "value": "1086"
              }
            ],
            "conditiontype": "and"
          }
        ]
      },
      "entryState": "526, 527, 529, 532, 533, 535"
    },
    {
      "event": "editisolateevent",
      "exitstate": {
        "default": "535",
        "options": [
          {
            "newstate": "532",
            "conditions": [
              {
                "currentstate": "532"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "533",
            "conditions": [
              {
                "currentstate": "533"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "529",
            "conditions": [
              {
                "currentstate": "529"
              }
            ],
            "conditiontype": "and"
          }
        ]
      },
      "entryState": "526, 527, 529, 532, 533, 535"
    },
    {
      "event": "cultureTestEntry",
      "entryState": "526, 527, 529, 532, 533, 535"
    },
    {
      "event": "deleteCulture",
      "exitstate": {
        "default": "529"
      },
      "entryState": "526, 527, 529, 532, 533, 535"
    },
    {
      "event": "deleteisolateevent",
      "exitstate": {
        "default": "529"
      },
      "entryState": "526, 527, 529, 532, 533, 535"
    },
    {
      "event": "cultureTestSelection",
      "entryState": "526, 527, 529, 532, 533, 535"
    },
    {
      "event": "updateast",
      "entryState": "529, 532, 533, 535"
    },
    {
      "event": "submitspecimen",
      "exitstate": {
        "default": "530",
        "options": [
          {
            "newstate": "534",
            "conditions": [
              {
                "field": "growthid",
                "value": "521"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "535",
            "conditions": [
              {
                "field": "growthid",
                "value": "536"
              }
            ],
            "conditiontype": "and"
          }
        ]
      },
      "entryState": "526, 527, 529, 532, 533, 535"
    },
    {
      "event": "specimenapprovalone",
      "exitstate": {
        "default": "531",
        "options": [
          {
            "newstate": "532",
            "conditions": [
              {
                "field": "decision",
                "value": "523"
              }
            ],
            "conditiontype": "and"
          }
        ]
      },
      "entryState": "530"
    },
    {
      "event": "specimenapprovaltwo",
      "actions": {
        "options": [
          {
            "action": "PublishReport",
            "newState": "534",
            "parameters": {
              "reportname": "@RepFin@",
              "reportconfig": "DefaultSpecimenReport",
              "doreportsneedapproval": "no"
            }
          }
        ]
      },
      "exitstate": {
        "default": "534",
        "options": [
          {
            "newstate": "533",
            "conditions": [
              {
                "field": "decision",
                "value": "523"
              }
            ],
            "conditiontype": "and"
          }
        ]
      },
      "entryState": "531"
    },
    {
      "event": "day0benchread",
      "exitstate": {
        "options": [
          {
            "newstate": "528",
            "conditions": [
              {
                "field": "BenchReadDay0Action",
                "value": "590"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "535",
            "conditions": [
              {
                "field": "BenchReadDay0Action",
                "value": "591"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "527",
            "conditions": [
              {
                "field": "BenchReadDay0Action",
                "value": "592"
              }
            ],
            "conditiontype": "and"
          }
        ]
      },
      "entryState": "526, 527"
    },
    {
      "event": "day1benchread",
      "exitstate": {
        "options": [
          {
            "newstate": "530",
            "conditions": [
              {
                "field": "BenchReadDay1Action",
                "value": "593"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "529",
            "conditions": [
              {
                "field": "BenchReadDay1Action",
                "value": "594"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "529",
            "conditions": [
              {
                "field": "BenchReadDay1Action",
                "value": "595"
              }
            ],
            "conditiontype": "and"
          }
        ]
      },
      "entryState": "535"
    },
    {
      "event": "CellCountTest",
      "exitstate": {
        "options": [
          {
            "newstate": "527",
            "conditions": [
              {
                "currentstate": "526"
              }
            ],
            "conditiontype": "and"
          }
        ]
      },
      "entryState": "526, 527, 529, 532, 533, 535"
    },
    {
      "event": "GramStainTest",
      "exitstate": {
        "options": [
          {
            "newstate": "527",
            "conditions": [
              {
                "currentstate": "526"
              }
            ],
            "conditiontype": "and"
          }
        ]
      },
      "entryState": "526, 527, 529, 532, 533, 535"
    },
    {
      "event": "IndiaInkTest",
      "exitstate": {
        "options": [
          {
            "newstate": "527",
            "conditions": [
              {
                "currentstate": "526"
              }
            ],
            "conditiontype": "and"
          }
        ]
      },
      "entryState": "526, 527, 529, 532, 533, 535"
    },
    {
      "event": "WetPrepTest",
      "exitstate": {
        "options": [
          {
            "newstate": "527",
            "conditions": [
              {
                "currentstate": "526"
              }
            ],
            "conditiontype": "and"
          }
        ]
      },
      "entryState": "526, 527, 529, 532, 533, 535"
    },
    {
      "event": "ZnStainTest",
      "exitstate": {
        "options": [
          {
            "newstate": "527",
            "conditions": [
              {
                "currentstate": "526"
              }
            ],
            "conditiontype": "and"
          }
        ]
      },
      "entryState": "526, 527, 529, 532, 533, 535"
    },
    {
      "event": "PregnancyTest",
      "exitstate": {
        "options": [
          {
            "newstate": "527",
            "conditions": [
              {
                "currentstate": "526"
              }
            ],
            "conditiontype": "and"
          }
        ]
      },
      "entryState": "526, 527, 529, 532, 533, 535"
    },
    {
      "event": "AuramineTest",
      "exitstate": {
        "options": [
          {
            "newstate": "527",
            "conditions": [
              {
                "currentstate": "526"
              }
            ],
            "conditiontype": "and"
          }
        ]
      },
      "entryState": "526, 527, 529, 532, 533, 535"
    },
    {
      "event": "MicroscopyTest",
      "exitstate": {
        "options": [
          {
            "newstate": "527",
            "conditions": [
              {
                "currentstate": "526"
              }
            ],
            "conditiontype": "and"
          }
        ]
      },
      "entryState": "526, 527, 529, 532, 533, 535"
    },
    {
      "event": "WrightsStainTest",
      "exitstate": {
        "options": [
          {
            "newstate": "527",
            "conditions": [
              {
                "currentstate": "526"
              }
            ],
            "conditiontype": "and"
          }
        ]
      },
      "entryState": "526, 527, 529, 532, 533, 535"
    },
    {
      "event": "BiochemistryTest",
      "exitstate": {
        "options": [
          {
            "newstate": "527",
            "conditions": [
              {
                "currentstate": "526"
              }
            ],
            "conditiontype": "and"
          }
        ]
      },
      "entryState": "526, 527, 529, 532, 533, 535"
    },
    {
      "event": "JevSerologyTest",
      "exitstate": {
        "options": [
          {
            "newstate": "527",
            "conditions": [
              {
                "currentstate": "526"
              }
            ],
            "conditiontype": "and"
          }
        ]
      },
      "entryState": "526, 527, 529, 532, 533, 535"
    },
    {
      "event": "KOHPrepTest",
      "exitstate": {
        "options": [
          {
            "newstate": "527",
            "conditions": [
              {
                "currentstate": "526"
              }
            ],
            "conditiontype": "and"
          }
        ]
      },
      "entryState": "526, 527, 529, 532, 533, 535"
    },
    {
      "event": "DipstickTest",
      "exitstate": {
        "options": [
          {
            "newstate": "527",
            "conditions": [
              {
                "currentstate": "526"
              }
            ],
            "conditiontype": "and"
          }
        ]
      },
      "entryState": "526, 527, 529, 532, 533, 535"
    },
    {
      "event": "HPyloriAntigenTest",
      "exitstate": {
        "options": [
          {
            "newstate": "527",
            "conditions": [
              {
                "currentstate": "526"
              }
            ],
            "conditiontype": "and"
          }
        ]
      },
      "entryState": "526, 527, 529, 532, 533, 535"
    },
    {
      "event": "editaliquotevent",
      "entryState": "534"
    }
  ],
  "Table": "specimen",
  "StartState": "525",
  "StatesList": "524",
  "Description": "Double Approval Workflow",
  "EntryConditions": [
    {
      "Events": "remotespecimen",
      "Default": "525"
    },
    {
      "Events": "newreceivedspecimen",
      "Default": "526",
      "options": [
        {
          "newstate": "527",
          "conditions": [
            {
              "field": "action",
              "value": "509"
            }
          ],
          "conditiontype": "and"
        },
        {
          "newstate": "528",
          "conditions": [
            {
              "field": "action",
              "value": "507"
            }
          ],
          "conditiontype": "and"
        },
        {
          "newstate": "535",
          "conditions": [
            {
              "field": "action",
              "value": "508"
            }
          ],
          "conditiontype": "and"
        }
      ]
    }
  ]
}'
WHERE  id = 106;

UPDATE configs
SET    contents = '
{
  "Name": "SingleApprovalWorkflow",
  "Field": "stateid",
  "Steps": [
    {
      "event": "newreceivedspecimen",
      "actions": {
        "options": [
          {
            "action": "PublishReport",
            "newState": "528",
            "conditions": [
              {
                "field": "action",
                "value": "507"
              }
            ],
            "parameters": {
              "reportname": "@RepFin@",
              "reportconfig": "DefaultSpecimenReport",
              "doreportsneedapproval": "no"
            },
            "conditiontype": "and"
          }
        ]
      },
      "entrystate": ""
    },
    {
      "event": "ACKReceipt",
      "actions": {
        "options": [
          {
            "action": "PublishReport",
            "newState": "528",
            "parameters": {
              "reportname": "@RepFin@",
              "reportconfig": "DefaultSpecimenReport",
              "doreportsneedapproval": "no"
            }
          }
        ]
      },
      "exitstate": {
        "default": "526",
        "options": [
          {
            "newstate": "527",
            "conditions": [
              {
                "field": "action",
                "value": "509"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "528",
            "conditions": [
              {
                "field": "action",
                "value": "507"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "535",
            "conditions": [
              {
                "field": "action",
                "value": "508"
              }
            ],
            "conditiontype": "and"
          }
        ]
      },
      "entrystate": "525"
    },
    {
      "event": "RejectSpecimen",
      "actions": {
        "options": [
          {
            "action": "PublishReport",
            "newState": "528",
            "parameters": {
              "reportname": "@RepFin@",
              "reportconfig": "DefaultSpecimenReport",
              "doreportsneedapproval": "no"
            }
          }
        ]
      },
      "exitstate": {
        "default": "528"
      },
      "entrystate": "526"
    },
    {
      "event": "TestSelection",
      "exitstate": {
        "options": [
          {
            "newstate": "527",
            "conditions": [
              {
                "currentstate": "526"
              }
            ],
            "conditiontype": "and"
          }
        ]
      },
      "entryState": "526, 527, 529, 532, 533, 535"
    },
    {
      "event": "directtestentry",
      "exitstate": {
        "options": [
          {
            "newstate": "527",
            "conditions": [
              {
                "currentstate": "526"
              }
            ],
            "conditiontype": "and"
          }
        ]
      },
      "entryState": "526, 527, 529, 532, 533, 535"
    },
    {
      "event": "SpecimenCancelRequest",
      "exitstate": {
        "default": "537"
      },
      "entryState": "525"
    },
    {
      "event": "EditSpecimen",
      "entryState": "525, 526, 527, 529, 532, 533, 535"
    },
    {
      "event": "addCulture",
      "exitstate": {
        "default": "535",
        "options": [
          {
            "newstate": "532",
            "conditions": [
              {
                "currentstate": "532"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "533",
            "conditions": [
              {
                "currentstate": "533"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "529",
            "conditions": [
              {
                "currentstate": "529"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "529",
            "conditions": [
              {
                "field": "growthid",
                "value": "178"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "529",
            "conditions": [
              {
                "field": "growthid",
                "value": "179"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "529",
            "conditions": [
              {
                "field": "growthid",
                "value": "180"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "529",
            "conditions": [
              {
                "field": "growthid",
                "value": "181"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "529",
            "conditions": [
              {
                "field": "growthid",
                "value": "126"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "529",
            "conditions": [
              {
                "field": "growthid",
                "value": "1087"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "535",
            "conditions": [
              {
                "field": "growthid",
                "value": "125"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "535",
            "conditions": [
              {
                "field": "growthid",
                "value": "177"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "535",
            "conditions": [
              {
                "field": "growthid",
                "value": "182"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "535",
            "conditions": [
              {
                "field": "growthid",
                "value": "1050"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "535",
            "conditions": [
              {
                "field": "growthid",
                "value": "1086"
              }
            ],
            "conditiontype": "and"
          }
        ]
      },
      "entryState": "526, 527, 529, 532, 533, 535"
    },
    {
      "event": "addisolateevent",
      "exitstate": {
        "default": "535",
        "options": [
          {
            "newstate": "532",
            "conditions": [
              {
                "currentstate": "532"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "533",
            "conditions": [
              {
                "currentstate": "533"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "529",
            "conditions": [
              {
                "currentstate": "529"
              }
            ],
            "conditiontype": "and"
          }
        ]
      },
      "entryState": "526, 527, 529, 532, 533, 535"
    },
    {
      "event": "editCulture",
      "exitstate": {
        "default": "535",
        "options": [
          {
            "newstate": "532",
            "conditions": [
              {
                "currentstate": "532"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "533",
            "conditions": [
              {
                "currentstate": "533"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "529",
            "conditions": [
              {
                "currentstate": "529"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "529",
            "conditions": [
              {
                "field": "growthid",
                "value": "178"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "529",
            "conditions": [
              {
                "field": "growthid",
                "value": "179"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "529",
            "conditions": [
              {
                "field": "growthid",
                "value": "180"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "529",
            "conditions": [
              {
                "field": "growthid",
                "value": "181"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "529",
            "conditions": [
              {
                "field": "growthid",
                "value": "126"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "529",
            "conditions": [
              {
                "field": "growthid",
                "value": "1087"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "535",
            "conditions": [
              {
                "field": "growthid",
                "value": "125"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "535",
            "conditions": [
              {
                "field": "growthid",
                "value": "177"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "535",
            "conditions": [
              {
                "field": "growthid",
                "value": "182"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "535",
            "conditions": [
              {
                "field": "growthid",
                "value": "1050"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "535",
            "conditions": [
              {
                "field": "growthid",
                "value": "1086"
              }
            ],
            "conditiontype": "and"
          }
        ]
      },
      "entryState": "526, 527, 529, 532, 533, 535"
    },
    {
      "event": "editisolateevent",
      "exitstate": {
        "default": "535",
        "options": [
          {
            "newstate": "532",
            "conditions": [
              {
                "currentstate": "532"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "533",
            "conditions": [
              {
                "currentstate": "533"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "529",
            "conditions": [
              {
                "currentstate": "529"
              }
            ],
            "conditiontype": "and"
          }
        ]
      },
      "entryState": "526, 527, 529, 532, 533, 535"
    },
    {
      "event": "cultureTestEntry",
      "entryState": "526, 527, 529, 532, 533, 535"
    },
    {
      "event": "deleteCulture",
      "exitstate": {
        "default": "529"
      },
      "entryState": "526, 527, 529, 532, 533, 535"
    },
    {
      "event": "deleteisolateevent",
      "exitstate": {
        "default": "529"
      },
      "entryState": "526, 527, 529, 532, 533, 535"
    },
    {
      "event": "cultureTestSelection",
      "entryState": "526, 527, 529, 532, 533, 535"
    },
    {
      "event": "updateast",
      "entryState": "529, 532, 533, 535"
    },
    {
      "event": "submitspecimen",
      "exitstate": {
        "default": "531",
        "options": [
          {
            "newstate": "534",
            "conditions": [
              {
                "field": "growthid",
                "value": "521"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "535",
            "conditions": [
              {
                "field": "growthid",
                "value": "536"
              }
            ],
            "conditiontype": "and"
          }
        ]
      },
      "entryState": "526, 527, 529, 532, 533, 535"
    },
    {
      "event": "specimenapprovaltwo",
      "actions": {
        "options": [
          {
            "action": "PublishReport",
            "newState": "534",
            "parameters": {
              "reportname": "@RepFin@",
              "reportconfig": "DefaultSpecimenReport",
              "doreportsneedapproval": "no"
            }
          }
        ]
      },
      "exitstate": {
        "default": "534",
        "options": [
          {
            "newstate": "533",
            "conditions": [
              {
                "field": "decision",
                "value": "523"
              }
            ],
            "conditiontype": "and"
          }
        ]
      },
      "entryState": "530, 531"
    },
    {
      "event": "day0benchread",
      "exitstate": {
        "options": [
          {
            "newstate": "528",
            "conditions": [
              {
                "field": "BenchReadDay0Action",
                "value": "590"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "535",
            "conditions": [
              {
                "field": "BenchReadDay0Action",
                "value": "591"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "527",
            "conditions": [
              {
                "field": "BenchReadDay0Action",
                "value": "592"
              }
            ],
            "conditiontype": "and"
          }
        ]
      },
      "entryState": "526, 527"
    },
    {
      "event": "day1benchread",
      "exitstate": {
        "options": [
          {
            "newstate": "530",
            "conditions": [
              {
                "field": "BenchReadDay1Action",
                "value": "593"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "529",
            "conditions": [
              {
                "field": "BenchReadDay1Action",
                "value": "594"
              }
            ],
            "conditiontype": "and"
          },
          {
            "newstate": "529",
            "conditions": [
              {
                "field": "BenchReadDay1Action",
                "value": "595"
              }
            ],
            "conditiontype": "and"
          }
        ]
      },
      "entryState": "535"
    },
    {
      "event": "CellCountTest",
      "exitstate": {
        "options": [
          {
            "newstate": "527",
            "conditions": [
              {
                "currentstate": "526"
              }
            ],
            "conditiontype": "and"
          }
        ]
      },
      "entryState": "526, 527, 529, 532, 533, 535"
    },
    {
      "event": "GramStainTest",
      "exitstate": {
        "options": [
          {
            "newstate": "527",
            "conditions": [
              {
                "currentstate": "526"
              }
            ],
            "conditiontype": "and"
          }
        ]
      },
      "entryState": "526, 527, 529, 532, 533, 535"
    },
    {
      "event": "IndiaInkTest",
      "exitstate": {
        "options": [
          {
            "newstate": "527",
            "conditions": [
              {
                "currentstate": "526"
              }
            ],
            "conditiontype": "and"
          }
        ]
      },
      "entryState": "526, 527, 529, 532, 533, 535"
    },
    {
      "event": "WetPrepTest",
      "exitstate": {
        "options": [
          {
            "newstate": "527",
            "conditions": [
              {
                "currentstate": "526"
              }
            ],
            "conditiontype": "and"
          }
        ]
      },
      "entryState": "526, 527, 529, 532, 533, 535"
    },
    {
      "event": "ZnStainTest",
      "exitstate": {
        "options": [
          {
            "newstate": "527",
            "conditions": [
              {
                "currentstate": "526"
              }
            ],
            "conditiontype": "and"
          }
        ]
      },
      "entryState": "526, 527, 529, 532, 533, 535"
    },
    {
      "event": "PregnancyTest",
      "exitstate": {
        "options": [
          {
            "newstate": "527",
            "conditions": [
              {
                "currentstate": "526"
              }
            ],
            "conditiontype": "and"
          }
        ]
      },
      "entryState": "526, 527, 529, 532, 533, 535"
    },
    {
      "event": "AuramineTest",
      "exitstate": {
        "options": [
          {
            "newstate": "527",
            "conditions": [
              {
                "currentstate": "526"
              }
            ],
            "conditiontype": "and"
          }
        ]
      },
      "entryState": "526, 527, 529, 532, 533, 535"
    },
    {
      "event": "MicroscopyTest",
      "exitstate": {
        "options": [
          {
            "newstate": "527",
            "conditions": [
              {
                "currentstate": "526"
              }
            ],
            "conditiontype": "and"
          }
        ]
      },
      "entryState": "526, 527, 529, 532, 533, 535"
    },
    {
      "event": "WrightsStainTest",
      "exitstate": {
        "options": [
          {
            "newstate": "527",
            "conditions": [
              {
                "currentstate": "526"
              }
            ],
            "conditiontype": "and"
          }
        ]
      },
      "entryState": "526, 527, 529, 532, 533, 535"
    },
    {
      "event": "BiochemistryTest",
      "exitstate": {
        "options": [
          {
            "newstate": "527",
            "conditions": [
              {
                "currentstate": "526"
              }
            ],
            "conditiontype": "and"
          }
        ]
      },
      "entryState": "526, 527, 529, 532, 533, 535"
    },
    {
      "event": "JevSerologyTest",
      "exitstate": {
        "options": [
          {
            "newstate": "527",
            "conditions": [
              {
                "currentstate": "526"
              }
            ],
            "conditiontype": "and"
          }
        ]
      },
      "entryState": "526, 527, 529, 532, 533, 535"
    },
    {
      "event": "KOHPrepTest",
      "exitstate": {
        "options": [
          {
            "newstate": "527",
            "conditions": [
              {
                "currentstate": "526"
              }
            ],
            "conditiontype": "and"
          }
        ]
      },
      "entryState": "526, 527, 529, 532, 533, 535"
    },
    {
      "event": "DipstickTest",
      "exitstate": {
        "options": [
          {
            "newstate": "527",
            "conditions": [
              {
                "currentstate": "526"
              }
            ],
            "conditiontype": "and"
          }
        ]
      },
      "entryState": "526, 527, 529, 532, 533, 535"
    },
    {
      "event": "HPyloriAntigenTest",
      "exitstate": {
        "options": [
          {
            "newstate": "527",
            "conditions": [
              {
                "currentstate": "526"
              }
            ],
            "conditiontype": "and"
          }
        ]
      },
      "entryState": "526, 527, 529, 532, 533, 535"
    },
    {
      "event": "editaliquotevent",
      "entryState": "534"
    }
  ],
  "Table": "specimen",
  "StartState": "525",
  "StatesList": "524",
  "Description": "Single Approval Workflow",
  "EntryConditions": [
    {
      "Events": "remotespecimen",
      "Default": "525"
    },
    {
      "Events": "newreceivedspecimen",
      "Default": "526",
      "options": [
        {
          "newstate": "527",
          "conditions": [
            {
              "field": "action",
              "value": "509"
            }
          ],
          "conditiontype": "and"
        },
        {
          "newstate": "528",
          "conditions": [
            {
              "field": "action",
              "value": "507"
            }
          ],
          "conditiontype": "and"
        },
        {
          "newstate": "535",
          "conditions": [
            {
              "field": "action",
              "value": "508"
            }
          ],
          "conditiontype": "and"
        }
      ]
    }
  ]
}'
WHERE  id = 107;
