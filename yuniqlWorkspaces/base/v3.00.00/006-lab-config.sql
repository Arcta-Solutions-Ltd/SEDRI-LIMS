CREATE TABLE
    laboratoryconfigs (
        id SERIAL PRIMARY KEY,
        laboratoryid INT NOT NULL,
        configname VARCHAR(60),
        contents JSONB,
        lastmodifieddate TIMESTAMPTZ NOT NULL
    );

ALTER TABLE laboratory Add defaultworkflowid INT;

ALTER TABLE organisationuser ADD lastmodifieddate TIMESTAMPTZ NOT NULL;

ALTER TABLE reporthistory ADD approvaldate timestamp
with
    time zone,
    ADD approvedby varchar(40),
    ADD reportapprovalid INT;

ALTER TABLE organisationuser
DROP COLUMN IF EXISTS defaultorganisation;

INSERT INTO
    List (
        id,
        name,
        grouping,
        parentId,
        common,
        description,
        lastmodifieddate,
        deleted
    ) OVERRIDING SYSTEM VALUE
VALUES
    (
        130,
        'TestCategory',
        'System',
        null,
        true,
        'Test Category',
        now (),
        false
    ),
    (
        131,
        'CultureTypeCategory',
        'System',
        null,
        true,
        'Culture Type Category',
        now (),
        false
    );

delete from Configs
where
    id in (105, 106, 107, 108, 109, 110);

INSERT INTO
    configs (
        id,
        configname,
        configtypeid,
        lastmodifieddate,
        contents
    ) OVERRIDING SYSTEM VALUE
VALUES
    (
        106,
        'SpecimenDefault',
        18,
        now (),
        '{ 
		"Name": "SpecimenDefault", 
		"Description": "Double Approval Workflow",
		"Table": "specimen",
		"Field": "stateid",
		"EntryConditions": [
			{
				"Events": "remotespecimen",
				"Default": "525"
			},
			{
				"Events": "newreceivedspecimen",
				"Default": "526",
				"options": [ 
					{ "newstate": "527", "conditiontype": "and", "conditions": [ { "field": "action", "value": "509" } ] },
					{ "newstate": "528", "conditiontype": "and", "conditions": [ { "field": "action", "value": "507" } ] },
					{ "newstate": "535", "conditiontype": "and", "conditions": [ { "field": "action", "value": "508" } ] }
				]
			}
		],
		"StatesList" : "524",
		"StartState": "525",
		"Steps": [
			{
				"entrystate": "",
				"event": "newreceivedspecimen",
				"actions": {
					"options": [
						{ "action": "PublishReport", "newState": "528", "conditions": [ { "field": "action", "value": "507" } ],
						  "parameters": "DefaultSpecimenReport,@RepFin@",
						  "conditiontype": "and"
						}
					]
				}
			},
			{ 
				"entrystate": "525", 
				"event": "ACKReceipt", 
				"exitstate": { 
					"default": "526",
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "field": "action", "value": "509" } ] },
						{ "newstate": "528", "conditiontype": "and", "conditions": [ { "field": "action", "value": "507" } ] },
						{ "newstate": "535", "conditiontype": "and", "conditions": [ { "field": "action", "value": "508" } ] }
					]
				},
				"actions": {
					"options": [
						{ "action": "PublishReport", "newState": "528", "parameters": "DefaultSpecimenReport,@RepFin@" }
					]
				}
			},
			{ 
				"entrystate": "526",
				"event": "RejectSpecimen",
				"exitstate": { "default": "528" },
				"actions": {
					"options": [
						{ "action": "PublishReport", "newState": "528", "parameters": "DefaultSpecimenReport,@RepFin@" }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "TestSelection",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "directtestentry",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "525",
				"event": "SpecimenCancelRequest",
				"exitstate": { "default": "537" }
			},
			{
				"entryState": "525, 526, 527, 529, 532, 533, 535",
				"event": "EditSpecimen"
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "addCulture",
				"exitstate": {
					"default": "535",
					"options": [ 
						{ "newstate": "532", "conditiontype": "and", "conditions": [ { "currentstate": "532" } ] },
						{ "newstate": "533", "conditiontype": "and", "conditions": [ { "currentstate": "533" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "currentstate": "529" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "178" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "179" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "180" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "181" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "182" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "183" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "184" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "185" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "186" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "187" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "188" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "1087" } ] },
						{ "newstate": "526", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "177" }, { "currentstate": "526" } ] },
						{ "newstate": "526", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "1050" }, { "currentstate": "526" } ] },
						{ "newstate": "526", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "1086" }, { "currentstate": "526" } ] },
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "177" }, { "currentstate": "527" } ] },
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "1050" }, { "currentstate": "527" } ] },
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "1086" }, { "currentstate": "527" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "addisolateevent",
				"exitstate": {
					"default": "535",
					"options": [ 
						{ "newstate": "532", "conditiontype": "and", "conditions": [ { "currentstate": "532" } ] },
						{ "newstate": "533", "conditiontype": "and", "conditions": [ { "currentstate": "533" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "currentstate": "529" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "178" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "179" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "180" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "181" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "182" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "183" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "184" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "185" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "186" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "187" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "188" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "1087" } ] },
						{ "newstate": "526", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "177" }, { "currentstate": "526" } ] },
						{ "newstate": "526", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "1050" }, { "currentstate": "526" } ] },
						{ "newstate": "526", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "1086" }, { "currentstate": "526" } ] },
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "177" }, { "currentstate": "527" } ] },
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "1050" }, { "currentstate": "527" } ] },
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "1086" }, { "currentstate": "527" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "editCulture",
				"exitstate": {
					"default": "535",
					"options": [ 
						{ "newstate": "532", "conditiontype": "and", "conditions": [ { "currentstate": "532" } ] },
						{ "newstate": "533", "conditiontype": "and", "conditions": [ { "currentstate": "533" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "currentstate": "529" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "178" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "179" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "180" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "181" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "182" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "183" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "184" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "185" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "186" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "187" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "188" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "1087" } ] },
						{ "newstate": "526", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "177" }, { "currentstate": "526" } ] },
						{ "newstate": "526", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "1050" }, { "currentstate": "526" } ] },
						{ "newstate": "526", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "1086" }, { "currentstate": "526" } ] },
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "177" }, { "currentstate": "527" } ] },
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "1050" }, { "currentstate": "527" } ] },
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "1086" }, { "currentstate": "527" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "editisolateevent",
				"exitstate": {
					"default": "535",
					"options": [ 
						{ "newstate": "532", "conditiontype": "and", "conditions": [ { "currentstate": "532" } ] },
						{ "newstate": "533", "conditiontype": "and", "conditions": [ { "currentstate": "533" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "currentstate": "529" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "178" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "179" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "180" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "181" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "182" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "183" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "184" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "185" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "186" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "187" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "188" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "1087" } ] },
						{ "newstate": "526", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "177" }, { "currentstate": "526" } ] },
						{ "newstate": "526", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "1050" }, { "currentstate": "526" } ] },
						{ "newstate": "526", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "1086" }, { "currentstate": "526" } ] },
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "177" }, { "currentstate": "527" } ] },
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "1050" }, { "currentstate": "527" } ] },
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "1086" }, { "currentstate": "527" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "cultureTestEntry"
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "deleteCulture",
				"exitstate": { "default": "529" }
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "deleteisolateevent",
				"exitstate": { "default": "529" }
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "cultureTestSelection"
			},
			{
				"entryState": "529, 532, 533, 535",
				"event": "updateast"
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "submitspecimen",
				"exitstate": { 
					"default": "530",
					"options": [ 
						{ "newstate": "534", "conditiontype": "and", "conditions": [ { "field": "growthid", "value": "521" } ] },
						{ "newstate": "535", "conditiontype": "and", "conditions": [ { "field": "growthid", "value": "536" } ] }
					]
				}
			},
			{
				"entryState": "530",
				"event": "specimenapprovalone",
				"exitstate": { 
					"default": "531", 
					"options": [ 
						{ "newstate": "532", "conditiontype": "and", "conditions": [ { "field": "decision", "value": "523" } ] }
					]
				}
			},
			{
				"entryState": "531",
				"event": "specimenapprovaltwo",
				"exitstate": { 
					"default": "534", 
					"options": [ 
						{ "newstate": "533", "conditiontype": "and", "conditions": [ { "field": "decision", "value": "523" } ] }
					]
				},
				"actions": {
					"options": [ 
						{ "action": "PublishReport", "parameters": "DefaultSpecimenReport,@RepFin@", "newState": "534"}
					]
				}
			},
			{
				"entryState": "526, 527",
				"event": "day0benchread",
				"exitstate": { 
					"options": [
						{ "newstate": "528", "conditiontype": "and", "conditions": [ { "field": "BenchReadDay0Action", "value": "590" } ] },
						{ "newstate": "535", "conditiontype": "and", "conditions": [ { "field": "BenchReadDay0Action", "value": "591" } ] },
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "field": "BenchReadDay0Action", "value": "592" } ] }
					]
				}
			},
			{
				"entryState": "535",
				"event": "day1benchread",
				"exitstate": { 
					"options": [
						{ "newstate": "530", "conditiontype": "and", "conditions": [ { "field": "BenchReadDay1Action", "value": "593" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "BenchReadDay1Action", "value": "594" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "BenchReadDay1Action", "value": "595" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "CellCountTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "GramStainTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "IndiaInkTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "WetPrepTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "ZnStainTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "PregnancyTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "AuramineTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "MicroscopyTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "WrightsStainTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "BiochemistryTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "JevSerologyTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "KOHPrepTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "DipstickTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "HPyloriAntigenTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "534",
				"event": "editaliquotevent"
			}
		]
	}'
    ),
    (
        107,
        'SingleApprovalWorkflow',
        18,
        now (),
        '{ 
		"Name": "SingleApprovalWorkflow", 
		"Description": "Single Approval Workflow",
		"Table": "specimen",
		"Field": "stateid",
		"EntryConditions": [
			{
				"Events": "remotespecimen",
				"Default": "525"
			},
			{
				"Events": "newreceivedspecimen",
				"Default": "526",
				"options": [ 
					{ "newstate": "527", "conditiontype": "and", "conditions": [ { "field": "action", "value": "509" } ] },
					{ "newstate": "528", "conditiontype": "and", "conditions": [ { "field": "action", "value": "507" } ] },
					{ "newstate": "535", "conditiontype": "and", "conditions": [ { "field": "action", "value": "508" } ] }
				]
			}
		],
		"StatesList" : "524",
		"StartState": "525",
		"Steps": [
			{
				"entrystate": "",
				"event": "newreceivedspecimen",
				"actions": {
					"options": [
						{ "action": "PublishReport", "newState": "528", "conditions": [ { "field": "action", "value": "507" } ],
						  "parameters": "DefaultSpecimenReport,@RepFin@",
						  "conditiontype": "and"
						}
					]
				}
			},
			{ 
				"entrystate": "525", 
				"event": "ACKReceipt", 
				"exitstate": { 
					"default": "526",
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "field": "action", "value": "509" } ] },
						{ "newstate": "528", "conditiontype": "and", "conditions": [ { "field": "action", "value": "507" } ] },
						{ "newstate": "535", "conditiontype": "and", "conditions": [ { "field": "action", "value": "508" } ] }
					]
				},
				"actions": {
					"options": [
						{ "action": "PublishReport", "newState": "528", "parameters": "DefaultSpecimenReport,@RepFin@" }
					]
				}
			},
			{ 
				"entrystate": "526",
				"event": "RejectSpecimen",
				"exitstate": { "default": "528" },
				"actions": {
					"options": [
						{ "action": "PublishReport", "newState": "528", "parameters": "DefaultSpecimenReport,@RepFin@" }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "TestSelection",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "directtestentry",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "525",
				"event": "SpecimenCancelRequest",
				"exitstate": { "default": "537" }
			},
			{
				"entryState": "525, 526, 527, 529, 532, 533, 535",
				"event": "EditSpecimen"
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "addCulture",
				"exitstate": {
					"default": "535",
					"options": [ 
						{ "newstate": "532", "conditiontype": "and", "conditions": [ { "currentstate": "532" } ] },
						{ "newstate": "533", "conditiontype": "and", "conditions": [ { "currentstate": "533" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "currentstate": "529" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "178" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "179" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "180" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "181" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "182" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "183" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "184" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "185" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "186" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "187" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "188" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "1087" } ] },
						{ "newstate": "526", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "177" }, { "currentstate": "526" } ] },
						{ "newstate": "526", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "1050" }, { "currentstate": "526" } ] },
						{ "newstate": "526", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "1086" }, { "currentstate": "526" } ] },
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "177" }, { "currentstate": "527" } ] },
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "1050" }, { "currentstate": "527" } ] },
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "1086" }, { "currentstate": "527" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "addisolateevent",
				"exitstate": {
					"default": "535",
					"options": [ 
						{ "newstate": "532", "conditiontype": "and", "conditions": [ { "currentstate": "532" } ] },
						{ "newstate": "533", "conditiontype": "and", "conditions": [ { "currentstate": "533" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "currentstate": "529" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "178" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "179" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "180" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "181" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "182" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "183" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "184" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "185" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "186" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "187" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "188" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "1087" } ] },
						{ "newstate": "526", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "177" }, { "currentstate": "526" } ] },
						{ "newstate": "526", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "1050" }, { "currentstate": "526" } ] },
						{ "newstate": "526", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "1086" }, { "currentstate": "526" } ] },
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "177" }, { "currentstate": "527" } ] },
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "1050" }, { "currentstate": "527" } ] },
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "1086" }, { "currentstate": "527" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "editCulture",
				"exitstate": {
					"default": "535",
					"options": [ 
						{ "newstate": "532", "conditiontype": "and", "conditions": [ { "currentstate": "532" } ] },
						{ "newstate": "533", "conditiontype": "and", "conditions": [ { "currentstate": "533" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "currentstate": "529" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "178" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "179" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "180" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "181" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "182" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "183" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "184" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "185" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "186" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "187" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "188" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "1087" } ] },
						{ "newstate": "526", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "177" }, { "currentstate": "526" } ] },
						{ "newstate": "526", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "1050" }, { "currentstate": "526" } ] },
						{ "newstate": "526", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "1086" }, { "currentstate": "526" } ] },
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "177" }, { "currentstate": "527" } ] },
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "1050" }, { "currentstate": "527" } ] },
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "1086" }, { "currentstate": "527" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "editisolateevent",
				"exitstate": {
					"default": "535",
					"options": [ 
						{ "newstate": "532", "conditiontype": "and", "conditions": [ { "currentstate": "532" } ] },
						{ "newstate": "533", "conditiontype": "and", "conditions": [ { "currentstate": "533" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "currentstate": "529" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "178" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "179" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "180" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "181" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "182" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "183" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "184" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "185" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "186" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "187" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "188" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "1087" } ] },
						{ "newstate": "526", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "177" }, { "currentstate": "526" } ] },
						{ "newstate": "526", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "1050" }, { "currentstate": "526" } ] },
						{ "newstate": "526", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "1086" }, { "currentstate": "526" } ] },
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "177" }, { "currentstate": "527" } ] },
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "1050" }, { "currentstate": "527" } ] },
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "field": "Quantity", "value": "1086" }, { "currentstate": "527" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "cultureTestEntry"
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "deleteCulture",
				"exitstate": { "default": "529" }
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "deleteisolateevent",
				"exitstate": { "default": "529" }
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "cultureTestSelection"
			},
			{
				"entryState": "529, 532, 533, 535",
				"event": "updateast"
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "submitspecimen",
				"exitstate": { 
					"default": "531",
					"options": [ 
						{ "newstate": "534", "conditiontype": "and", "conditions": [ { "field": "growthid", "value": "521" } ] },
						{ "newstate": "535", "conditiontype": "and", "conditions": [ { "field": "growthid", "value": "536" } ] }
					]
				}
			},
			{
				"entryState": "530, 531",
				"event": "specimenapprovaltwo",
				"exitstate": { 
					"default": "534", 
					"options": [ 
						{ "newstate": "533", "conditiontype": "and", "conditions": [ { "field": "decision", "value": "523" } ] }
					]
				},
				"actions": {
					"options": [ 
						{ "action": "PublishReport", "parameters": "DefaultSpecimenReport,@RepFin@", "newState": "534"}
					]
				}
			},
			{
				"entryState": "526, 527",
				"event": "day0benchread",
				"exitstate": { 
					"options": [
						{ "newstate": "528", "conditiontype": "and", "conditions": [ { "field": "BenchReadDay0Action", "value": "590" } ] },
						{ "newstate": "535", "conditiontype": "and", "conditions": [ { "field": "BenchReadDay0Action", "value": "591" } ] },
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "field": "BenchReadDay0Action", "value": "592" } ] }
					]
				}
			},
			{
				"entryState": "535",
				"event": "day1benchread",
				"exitstate": { 
					"options": [
						{ "newstate": "530", "conditiontype": "and", "conditions": [ { "field": "BenchReadDay1Action", "value": "593" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "BenchReadDay1Action", "value": "594" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "BenchReadDay1Action", "value": "595" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "CellCountTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "GramStainTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "IndiaInkTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "WetPrepTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "ZnStainTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "PregnancyTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "AuramineTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "MicroscopyTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "WrightsStainTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "BiochemistryTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "JevSerologyTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "KOHPrepTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "DipstickTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "HPyloriAntigenTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "534",
				"event": "editaliquotevent"
			}
		]
	}'
    ),
    (
        108,
        'NoCultureSingleApproval',
        18,
        now (),
        '{ 
		"Name": "NoCultureSingleApproval", 
		"Description": "No Culture Single Approval Workflow",
		"Table": "specimen",
		"Field": "stateid",
	 	"IncludeCulture": false,
		"EntryConditions": [
			{
				"Events": "remotespecimen",
				"Default": "525"
			},
			{
				"Events": "newreceivedspecimen",
				"Default": "526",
				"options": [ 
					{ "newstate": "527", "conditiontype": "and", "conditions": [ { "field": "action", "value": "509" } ] },
					{ "newstate": "528", "conditiontype": "and", "conditions": [ { "field": "action", "value": "507" } ] },
					{ "newstate": "535", "conditiontype": "and", "conditions": [ { "field": "action", "value": "508" } ] }
				]
			}
		],
		"StatesList" : "524",
		"StartState": "525",
		"Steps": [
			{
				"entrystate": "",
				"event": "newreceivedspecimen",
				"actions": {
					"options": [
						{ "action": "PublishReport", "newState": "528", "conditions": [ { "field": "action", "value": "507" } ],
						  "parameters": "DefaultSpecimenReport,@RepFin@",
						  "conditiontype": "and"
						}
					]
				}
			},
			{ 
				"entrystate": "525", 
				"event": "ACKReceipt", 
				"exitstate": { 
					"default": "526",
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "field": "action", "value": "509" } ] },
						{ "newstate": "528", "conditiontype": "and", "conditions": [ { "field": "action", "value": "507" } ] },
						{ "newstate": "535", "conditiontype": "and", "conditions": [ { "field": "action", "value": "508" } ] }
					]
				},
				"actions": {
					"options": [
						{ "action": "PublishReport", "newState": "528", "parameters": "DefaultSpecimenReport,@RepFin@" }
					]
				}
			},
			{ 
				"entrystate": "526",
				"event": "RejectSpecimen",
				"exitstate": { "default": "528" },
				"actions": {
					"options": [
						{ "action": "PublishReport", "newState": "528", "parameters": "DefaultSpecimenReport,@RepFin@" }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "TestSelection",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "directtestentry",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "525",
				"event": "SpecimenCancelRequest",
				"exitstate": { "default": "537" }
			},
			{
				"entryState": "525, 526, 527, 529, 532, 533, 535",
				"event": "EditSpecimen"
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "cultureTestEntry"
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "submitspecimen",
				"exitstate": { 
					"default": "530, 531",
					"options": [ 
						{ "newstate": "534", "conditiontype": "and", "conditions": [ { "field": "growthid", "value": "521" } ] },
						{ "newstate": "535", "conditiontype": "and", "conditions": [ { "field": "growthid", "value": "536" } ] }
					]
				}
			},
			{
				"entryState": "531",
				"event": "specimenapprovaltwo",
				"exitstate": { 
					"default": "534", 
					"options": [ 
						{ "newstate": "533", "conditiontype": "and", "conditions": [ { "field": "decision", "value": "523" } ] }
					]
				},
				"actions": {
					"options": [ 
						{ "action": "PublishReport", "parameters": "DefaultSpecimenReport,@RepFin@", "newState": "534"}
					]
				}
			},
			{
				"entryState": "526, 527",
				"event": "day0benchread",
				"exitstate": { 
					"options": [
						{ "newstate": "528", "conditiontype": "and", "conditions": [ { "field": "BenchReadDay0Action", "value": "590" } ] },
						{ "newstate": "535", "conditiontype": "and", "conditions": [ { "field": "BenchReadDay0Action", "value": "591" } ] },
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "field": "BenchReadDay0Action", "value": "592" } ] }
					]
				}
			},
			{
				"entryState": "535",
				"event": "day1benchread",
				"exitstate": { 
					"options": [
						{ "newstate": "530", "conditiontype": "and", "conditions": [ { "field": "BenchReadDay1Action", "value": "593" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "BenchReadDay1Action", "value": "594" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "BenchReadDay1Action", "value": "595" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "CellCountTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "GramStainTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "IndiaInkTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "WetPrepTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "ZnStainTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "PregnancyTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "AuramineTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "MicroscopyTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "WrightsStainTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "BiochemistryTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "JevSerologyTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "KOHPrepTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "DipstickTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "HPyloriAntigenTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "534",
				"event": "editaliquotevent"
			}
		]
	}'
    ),
    (
        109,
        'NoCultureDoubleApproval',
        18,
        now (),
        '{ 
		"Name": "NoCultureDoubleApproval", 
		"Description": "No Culture Double Approval Workflow",
		"Table": "specimen",
		"Field": "stateid",
	 	"IncludeCulture": false,
		"EntryConditions": [
			{
				"Events": "remotespecimen",
				"Default": "525"
			},
			{
				"Events": "newreceivedspecimen",
				"Default": "526",
				"options": [ 
					{ "newstate": "527", "conditiontype": "and", "conditions": [ { "field": "action", "value": "509" } ] },
					{ "newstate": "528", "conditiontype": "and", "conditions": [ { "field": "action", "value": "507" } ] },
					{ "newstate": "535", "conditiontype": "and", "conditions": [ { "field": "action", "value": "508" } ] }
				]
			}
		],
		"StatesList" : "524",
		"StartState": "525",
		"Steps": [
			{
				"entrystate": "",
				"event": "newreceivedspecimen",
				"actions": {
					"options": [
						{ "action": "PublishReport", "newState": "528", "conditions": [ { "field": "action", "value": "507" } ],
						  "parameters": "DefaultSpecimenReport,@RepFin@",
						  "conditiontype": "and"
						}
					]
				}
			},
			{ 
				"entrystate": "525", 
				"event": "ACKReceipt", 
				"exitstate": { 
					"default": "526",
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "field": "action", "value": "509" } ] },
						{ "newstate": "528", "conditiontype": "and", "conditions": [ { "field": "action", "value": "507" } ] },
						{ "newstate": "535", "conditiontype": "and", "conditions": [ { "field": "action", "value": "508" } ] }
					]
				},
				"actions": {
					"options": [
						{ "action": "PublishReport", "newState": "528", "parameters": "DefaultSpecimenReport,@RepFin@" }
					]
				}
			},
			{ 
				"entrystate": "526",
				"event": "RejectSpecimen",
				"exitstate": { "default": "528" },
				"actions": {
					"options": [
						{ "action": "PublishReport", "newState": "528", "parameters": "DefaultSpecimenReport,@RepFin@" }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "TestSelection",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "directtestentry",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "525",
				"event": "SpecimenCancelRequest",
				"exitstate": { "default": "537" }
			},
			{
				"entryState": "525, 526, 527, 529, 532, 533, 535",
				"event": "EditSpecimen"
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "cultureTestEntry"
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "submitspecimen",
				"exitstate": { 
					"default": "530",
					"options": [ 
						{ "newstate": "534", "conditiontype": "and", "conditions": [ { "field": "growthid", "value": "521" } ] },
						{ "newstate": "535", "conditiontype": "and", "conditions": [ { "field": "growthid", "value": "536" } ] }
					]
				}
			},
			{
				"entryState": "530",
				"event": "specimenapprovalone",
				"exitstate": { 
					"default": "531", 
					"options": [ 
						{ "newstate": "532", "conditiontype": "and", "conditions": [ { "field": "decision", "value": "523" } ] }
					]
				}
			},
			{
				"entryState": "531",
				"event": "specimenapprovaltwo",
				"exitstate": { 
					"default": "534", 
					"options": [ 
						{ "newstate": "533", "conditiontype": "and", "conditions": [ { "field": "decision", "value": "523" } ] }
					]
				},
				"actions": {
					"options": [ 
						{ "action": "PublishReport", "parameters": "DefaultSpecimenReport,@RepFin@", "newState": "534"}
					]
				}
			},
			{
				"entryState": "526, 527",
				"event": "day0benchread",
				"exitstate": { 
					"options": [
						{ "newstate": "528", "conditiontype": "and", "conditions": [ { "field": "BenchReadDay0Action", "value": "590" } ] },
						{ "newstate": "535", "conditiontype": "and", "conditions": [ { "field": "BenchReadDay0Action", "value": "591" } ] },
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "field": "BenchReadDay0Action", "value": "592" } ] }
					]
				}
			},
			{
				"entryState": "535",
				"event": "day1benchread",
				"exitstate": { 
					"options": [
						{ "newstate": "530", "conditiontype": "and", "conditions": [ { "field": "BenchReadDay1Action", "value": "593" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "BenchReadDay1Action", "value": "594" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "BenchReadDay1Action", "value": "595" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "CellCountTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "GramStainTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "IndiaInkTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "WetPrepTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "ZnStainTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "PregnancyTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "AuramineTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "MicroscopyTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "WrightsStainTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "BiochemistryTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "JevSerologyTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "KOHPrepTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "DipstickTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "HPyloriAntigenTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "534",
				"event": "editaliquotevent"
			}
		]
	}'
    ),
    (
        110,
        'NoCultureNoInstrumentDoubleApproval',
        18,
        now (),
        '{ 
		"Name": "NoCultureNoInstrumentDoubleApproval", 
		"Description": "No Culture, No Instrument Double Approval Workflow",
		"Table": "specimen",
		"Field": "stateid",
	 	"IncludeCulture": false,
	    "IncludeInstrument": false,
		"EntryConditions": [
			{
				"Events": "remotespecimen",
				"Default": "525"
			},
			{
				"Events": "newreceivedspecimen",
				"Default": "526",
				"options": [ 
					{ "newstate": "527", "conditiontype": "and", "conditions": [ { "field": "action", "value": "509" } ] },
					{ "newstate": "528", "conditiontype": "and", "conditions": [ { "field": "action", "value": "507" } ] },
					{ "newstate": "535", "conditiontype": "and", "conditions": [ { "field": "action", "value": "508" } ] }
				]
			}
		],
		"StatesList" : "524",
		"StartState": "525",
		"Steps": [
			{
				"entrystate": "",
				"event": "newreceivedspecimen",
				"actions": {
					"options": [
						{ "action": "PublishReport", "newState": "528", "conditions": [ { "field": "action", "value": "507" } ],
						  "parameters": "DefaultSpecimenReport,@RepFin@",
						  "conditiontype": "and"
						}
					]
				}
			},
			{ 
				"entrystate": "525", 
				"event": "ACKReceipt", 
				"exitstate": { 
					"default": "526",
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "field": "action", "value": "509" } ] },
						{ "newstate": "528", "conditiontype": "and", "conditions": [ { "field": "action", "value": "507" } ] },
						{ "newstate": "535", "conditiontype": "and", "conditions": [ { "field": "action", "value": "508" } ] }
					]
				},
				"actions": {
					"options": [
						{ "action": "PublishReport", "newState": "528", "parameters": "DefaultSpecimenReport,@RepFin@" }
					]
				}
			},
			{ 
				"entrystate": "526",
				"event": "RejectSpecimen",
				"exitstate": { "default": "528" },
				"actions": {
					"options": [
						{ "action": "PublishReport", "newState": "528", "parameters": "DefaultSpecimenReport,@RepFin@" }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "TestSelection",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "directtestentry",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "525",
				"event": "SpecimenCancelRequest",
				"exitstate": { "default": "537" }
			},
			{
				"entryState": "525, 526, 527, 529, 532, 533, 535",
				"event": "EditSpecimen"
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "cultureTestEntry"
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "submitspecimen",
				"exitstate": { 
					"default": "530",
					"options": [ 
						{ "newstate": "534", "conditiontype": "and", "conditions": [ { "field": "growthid", "value": "521" } ] },
						{ "newstate": "535", "conditiontype": "and", "conditions": [ { "field": "growthid", "value": "536" } ] }
					]
				}
			},
			{
				"entryState": "530",
				"event": "specimenapprovalone",
				"exitstate": { 
					"default": "531", 
					"options": [ 
						{ "newstate": "532", "conditiontype": "and", "conditions": [ { "field": "decision", "value": "523" } ] }
					]
				}
			},
			{
				"entryState": "531",
				"event": "specimenapprovaltwo",
				"exitstate": { 
					"default": "534", 
					"options": [ 
						{ "newstate": "533", "conditiontype": "and", "conditions": [ { "field": "decision", "value": "523" } ] }
					]
				},
				"actions": {
					"options": [ 
						{ "action": "PublishReport", "parameters": "DefaultSpecimenReport,@RepFin@", "newState": "534"}
					]
				}
			},
			{
				"entryState": "526, 527",
				"event": "day0benchread",
				"exitstate": { 
					"options": [
						{ "newstate": "528", "conditiontype": "and", "conditions": [ { "field": "BenchReadDay0Action", "value": "590" } ] },
						{ "newstate": "535", "conditiontype": "and", "conditions": [ { "field": "BenchReadDay0Action", "value": "591" } ] },
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "field": "BenchReadDay0Action", "value": "592" } ] }
					]
				}
			},
			{
				"entryState": "535",
				"event": "day1benchread",
				"exitstate": { 
					"options": [
						{ "newstate": "530", "conditiontype": "and", "conditions": [ { "field": "BenchReadDay1Action", "value": "593" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "BenchReadDay1Action", "value": "594" } ] },
						{ "newstate": "529", "conditiontype": "and", "conditions": [ { "field": "BenchReadDay1Action", "value": "595" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "CellCountTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "GramStainTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "IndiaInkTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "WetPrepTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "ZnStainTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "PregnancyTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "AuramineTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "MicroscopyTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "WrightsStainTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "BiochemistryTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "JevSerologyTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "KOHPrepTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "DipstickTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "526, 527, 529, 532, 533, 535",
				"event": "HPyloriAntigenTest",
				"exitstate": {
					"options": [ 
						{ "newstate": "527", "conditiontype": "and", "conditions": [ { "currentstate": "526" } ] }
					]
				}
			},
			{
				"entryState": "534",
				"event": "editaliquotevent"
			}
		]
	}'
    );

INSERT INTO
    configs (
        id,
        configname,
        configtypeid,
        lastmodifieddate,
        contents
    ) OVERRIDING SYSTEM VALUE
VALUES
    (
        105,
        'patientreference',
        20,
        now (),
        '[
		{ "Id": "patientreference|year", "Text": "@GenYeaB@", "Value": "599", "Enabled": "false", "Type": "yearlist" },
		{ "Id": "patientreference|monthnumber", "Text": "@GenMonB@", "Value": "2", "Enabled": "false", "Type": "toggle" },
		{ "Id": "patientreference|daynumber", "Text": "@GenDay@", "Type": "toggle", "Value": "2", "Enabled": "false" },
		{ "Id": "patientreference|sequence", "Text": "@SetNum@", "Value": "6", "Enabled": "true", "Type": "number", "Min": "3", "Max": "13", "ErrorMessage": "@SetNumA@" }
	]'
    );

UPDATE configtype
SET
    NAME = 'PrefixConfig'
WHERE
    id = 20;

ALTER TABLE accessionnumber ADD category varchar(30);

UPDATE accessionnumber
SET
    category = 'accessionnumber';

ALTER TABLE patient
ALTER patientref TYPE varchar(30);

UPDATE configs 
SET contents = contents || '{ "Id": "accessionnumber|daynumber", "Text": "@GenDay@", "Type": "toggle", "Value": "2", "Enabled": "false" }' :: jsonb 
WHERE id = 98;

UPDATE laboratory SET defaultworkflowid = 106;