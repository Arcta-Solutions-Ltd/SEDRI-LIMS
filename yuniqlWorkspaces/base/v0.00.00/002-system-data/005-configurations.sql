INSERT INTO
	ConfigType (Id, Name, Type) OVERRIDING SYSTEM VALUE
VALUES
	(1, 'DirectTestForm', 'Form'),
	(2, 'CultureTestForm', 'Form'),
	(3, 'PatientBarcode', 'Form'),
	(4, 'SpecimenBarcode', 'Form'),
	(5, 'View', 'List'),
	(6, 'UIEvent', 'UIEvent'),
	(7, 'Event', 'Event'),
	(8, 'Query', 'Query'),
	(9, 'Page', 'Page'),
	(10, 'ParameterMapper', 'Mapper'),
	(11, 'ResultMapper', 'Mapper'),
	(12, 'EventMapper', 'Mapper'),
	(13, 'Reports', 'Report'),
	(14, 'Specimen', 'ReportSection'),
	(15, 'Culture', 'ReportSection'),
	(16, 'Specimen', 'DataSource'),
	(17, 'Form', 'Form'),
	(18, 'Workflow', 'Workflow'),
	(19, 'Settings', 'Settings'),
	(20, 'AccessionNumber', 'Settings'),
	(21, 'ReportStatusMap', 'Settings');

ALTER SEQUENCE ConfigType_id_seq RESTART WITH 22;

INSERT INTO
	Configs (
		Id,
		configname,
		configtypeid,
		lastmodifieddate,
		contents
	)
VALUES
	(
		1,
		'cellcounttestform',
		1,
		now (),
		'{
			"name": "cellcounttestform",
			"uievent": "cellcounttestuievent",
			"title": "@TesCel@",
			"initialQuery": "cellcounttestbyid",
			"saveEvent": "cellcounttest",
			"suppressRecordView": true,
			"formtype": "directtest",
			"datasection": "CellCountDataSection",
			"configurable": "Yes",
			"pages": ["cellcounttestpage"]
		}'
	),
	(
		2,
		'gramstaintestform',
		1,
		now (),
		'{
			"name": "gramstaintestform",
			"uievent": "gramstaintestuievent",
			"title": "@TesGra@",
			"saveEvent": "gramstaintest",
			"initialQuery": "gramstaintestbyid",
			"suppressRecordView": true,
			"formtype": "directtest",
			"datasection": "GramStainDataSection",
			"configurable": "Yes",
			"pages": ["gramstaintestpage"]
		}'
	),
	(
		3,
		'indiainktestform',
		1,
		now (),
		'{
			"name": "indiainktestform",
			"uievent": "indiainktestuievent",
			"title": "@TesInd@",
			"saveEvent": "indiainktest",
			"initialQuery": "indiainktestbyid",
			"suppressRecordView": true,
			"formtype": "directtest",
			"datasection": "IndiaInkDataSection",
			"configurable": "Yes",
			"pages": ["indiainktestpage"]
		}'
	),
	(
		4,
		'wetpreptestform',
		1,
		now (),
		'{
			"name": "wetpreptestform",
			"uievent": "wetpreptestuievent",
			"saveEvent": "wetpreptest",
			"initialQuery": "wetpreptestbyid",
			"title": "@TesWet@",
			"suppressRecordView": true,
			"formtype": "directtest",
			"datasection": "WetPrepDataSection",
			"configurable": "Yes",
			"pages": ["wetpreptestpage"]
		}'
	),
	(
		5,
		'znstaintestform',
		1,
		now (),
		'{
			"name": "znstaintestform",
			"uievent": "znstaintestuievent",
			"saveEvent": "znstaintest",
			"initialQuery": "znstaintestbyid",
			"title": "@TesZnsA@",
			"suppressRecordView": true,
			"formtype": "directtest",
			"datasection": "ZnStainDataSection",
			"configurable": "Yes",
			"pages": ["znstaintestpage"]
		}'
	),
	(
		6,
		'pregnancytestform',
		1,
		now (),
		'{
			"name": "pregnancytestform",
			"uievent": "pregnancytestuievent",
			"title": "@TesPre@",
			"saveEvent": "pregnancytest",
			"initialQuery": "pregnancytestbyid",
			"suppressRecordView": true,
			"formtype": "directtest",
			"datasection": "PregnancyDataSection",
			"configurable": "Yes",
			"pages": ["pregnancytestpage"]
		}'
	),
	(
		7,
		'auraminetestform',
		1,
		now (),
		'{
			"name": "auraminetestform",
			"uievent": "auraminetestuievent",
			"title": "@TesAur@",
			"saveEvent": "auraminetest",
			"initialQuery": "auraminetestbyid",
			"suppressRecordView": true,
			"formtype": "directtest",
			"datasection": "AuramineDataSection",
			"configurable": "Yes",
			"pages": ["auraminetestpage"]
		}'
	),
	(
		8,
		'kohpreptestform',
		1,
		now (),
		'{
			"name": "kohpreptestform",
			"uievent": "kohpreptestuievent",
			"title": "@TesFun@",
			"saveEvent": "kohpreptest",
			"initialQuery": "kohpreptestbyid",
			"suppressRecordView": true,
			"formtype": "directtest",
			"datasection": "KohPrepDataSection",
			"configurable": "Yes",
			"pages": ["kohpreptestpage"]
		}'
	),
	(
		9,
		'microscopytestform',
		1,
		now (),
		'{
			"name": "microscopytestform",
			"uievent": "microscopytestuievent",
			"title": "@TesMic@",
			"saveEvent": "microscopytest",
			"initialQuery": "microscopytestbyid",
			"suppressRecordView": true,
			"formtype": "directtest",
			"datasection": "MicroscopyDataSection",
			"configurable": "Yes",
			"pages": ["microscopytestpage"]
		}'
	),
	(
		10,
		'biochemistrytestform',
		1,
		now (),
		'{
			"name": "biochemistrytestform",
			"uievent": "biochemistrytestuievent",
			"title": "@TesBio@",   
			"saveEvent": "biochemistrytest",
			"initialQuery": "biochemistrytestbyid",
			"suppressRecordView": true,
			"formtype": "directtest",
			"datasection": "BiochemistryDataSection",
			"configurable": "Yes",
			"pages": ["biochemistrytestpage"]
		}'
	),
	(
		11,
		'wrightsstaintestform',
		1,
		now (),
		'{
			"name": "wrightsstaintestform",
			"uievent": "wrightsstaintestuievent",
			"title": "@TesWri@",
			"saveEvent": "wrightsstaintest",
			"initialQuery": "wrightsstaintestbyid",
			"suppressRecordView": true,
			"formtype": "directtest",
			"datasection": "WrightsStainDataSection",
			"configurable": "Yes",
			"pages": ["wrightsstaintestpage"]
		}'
	),
	(
		12,
		'dipsticktestform',
		1,
		now (),
		'{
			"name": "dipsticktestform",
			"uievent": "dipsticktestuievent",
			"title": "@TesDip@",
			"saveEvent": "dipsticktest",
			"initialQuery": "dipsticktestbyid",
			"suppressRecordView": true,
			"formtype": "directtest",
			"datasection": "DipstickDataSection",
			"configurable": "Yes",
			"pages": ["dipsticktestpage"]
		}'
	),
	(
		13,
		'hpyloriantigentestform',
		1,
		now (),
		'{
			"name": "hpyloriantigentestform",
			"uievent": "hpyloriantigentestuievent",
			"title": "@TesHpy@",
			"saveEvent": "hpyloriantigentest",
			"initialQuery": "hpyloriantigentestbyid",
			"suppressRecordView": true,
			"formtype": "directtest",
			"datasection": "HpyloriAntigenDataSection",
			"configurable": "Yes",
			"pages": ["hpyloriantigentestpage"]
		}'
	),
	(
		14,
		'jevserologytestform',
		1,
		now (),
		'{
			"name": "jevserologytestform",
			"uievent": "jevserologytestuievent",
			"title": "@TesJev@",
			"saveEvent": "jevserologytest",
			"initialQuery": "jevserologytestbyid",
			"suppressRecordView": true,
			"formtype": "directtest",
			"datasection": "JevSerologyDataSection",
			"configurable": "Yes",
			"pages": ["jevserologytestpage"]
		}'
	),
	(
		15,
		'esbltestform',
		2,
		now (),
		'{
			"name": "esbltestform",
			"uievent": "esbltestuievent",
			"title": "@TesEsb@",
			"saveEvent": "esbltest",
			"initialQuery": "esbltestbyid",
			"suppressRecordView": true,
			"formtype": "culturetest",
			"datasection": "EsblDataSection",
			"configurable": "Yes",
			"pages": ["esbltestpage"]
		}'
	),
	(
		16,
		'betalactamasetestform',
		2,
		now (),
		'{
			"name": "betalactamasetestform",
			"uievent": "betalactamasetestuievent",
			"title": "@TesBet@",
			"saveEvent": "betalactamasetest",
			"initialQuery": "betalactamasetestbyid",
			"suppressRecordView": true,
			"formtype": "culturetest",
			"datasection": "BetalactamaseDataSection",
			"configurable": "Yes",
			"pages": ["betalactamasetestpage"]
		}'
	),
	(
		17,
		'carbapenemasetestform',
		2,
		now (),
		'{
			"name": "carbapenemasetestform",
			"uievent": "carbapenemasetestuievent",
			"title": "@TesCarB@",
			"saveEvent": "carbapenemasetest",
			"initialQuery": "carbapenemasetestbyid",
			"suppressRecordView": true,
			"formtype": "culturetest",
			"datasection": "CarbapenemaseDataSection",
			"configurable": "Yes",
			"pages": ["carbapenemasetestpage"]
		}'
	),
	(
		18,
		'apipaneltestform',
		2,
		now (),
		'{
			"name": "apipaneltestform",
			"uievent": "apipaneltestuievent",
			"title": "@TesApi@",
			"saveEvent": "apipaneltest",
			"initialQuery": "apipaneltestbyid",
			"suppressRecordView": true,
			"formtype": "culturetest",
			"datasection": "ApiPanelDataSection",
			"pages": ["apipaneltestpage"],
			"enabled": "Yes",
			"configurable": "Yes"
		}'
	),
	(19, 'patientbarcodeprint1', 3, now (), '{}'),
	(20, 'patientbarcodeprint2', 3, now (), '{}'),
	(21, 'specimenbarcodeprint1', 4, now (), '{}'),
	(22, 'specimenbarcodeprint2', 4, now (), '{}'),
	(23, 'specimens', 5, now (), '{}'),
	(26, 'defaultspecimenreport', 13, now (), '{}'),
	(27, 'apipanelsection', 14, now (), '{}'),
	(28, 'approvalsection', 14, now (), '{}'),
	(29, 'astsection', 15, now (), '{}'),
	(30, 'auraminesection', 14, now (), '{}'),
	(31, 'betalactamasesection', 15, now (), '{}'),
	(32, 'biochemistrysection', 14, now (), '{}'),
	(33, 'carbapenemasesection', 15, now (), '{}'),
	(34, 'cellcountsection', 14, now (), '{}'),
	(35, 'cultureresultsection', 15, now (), '{}'),
	(36, 'dipsticksection', 14, now (), '{}'),
	(37, 'esblsection', 15, now (), '{}'),
	(38, 'gramstainsection', 14, now (), '{}'),
	(39, 'hpyloriantigensection', 14, now (), '{}'),
	(40, 'indiainksection', 14, now (), '{}'),
	(41, 'jevserologysection', 14, now (), '{}'),
	(42, 'kohprepsection', 14, now (), '{}'),
	(43, 'locationsection', 14, now (), '{}'),
	(44, 'microscopysection', 14, now (), '{}'),
	(45, 'organismlistsection', 15, now (), '{}'),
	(46, 'patientdetailssection', 14, now (), '{}'),
	(47, 'precultureresultssection', 14, now (), '{}'),
	(48, 'pregnancysection', 14, now (), '{}'),
	(49, 'wetprepsection', 14, now (), '{}'),
	(50, 'wrightsstainsection', 14, now (), '{}'),
	(51, 'znstainsection', 14, now (), '{}'),
	(52, 'apipaneldatasection', 16, now (), '{}'),
	(53, 'approvaldatasection', 16, now (), '{}'),
	(54, 'astdatasection', 16, now (), '{}'),
	(55, 'auraminedatasection', 16, now (), '{}'),
	(56, 'betalactamasedatasection', 16, now (), '{}'),
	(57, 'biochemistrydatasection', 16, now (), '{}'),
	(58, 'carbapenemasedatasection', 16, now (), '{}'),
	(59, 'cellcountdatasection', 16, now (), '{}'),
	(60, 'cultureresultdatasection', 16, now (), '{}'),
	(61, 'dipstickdatasection', 16, now (), '{}'),
	(62, 'esbldatasection', 16, now (), '{}'),
	(63, 'gramstaindatasection', 16, now (), '{}'),
	(64, 'hpyloriantigendatasection', 16, now (), '{}'),
	(65, 'indiainkdatasection', 16, now (), '{}'),
	(66, 'jevserologydatasection', 16, now (), '{}'),
	(67, 'kohprepdatasection', 16, now (), '{}'),
	(68, 'locationdatasection', 16, now (), '{}'),
	(69, 'microscopydatasection', 16, now (), '{}'),
	(70, 'organismlistdatasection', 16, now (), '{}'),
	(71, 'patientdetailsdatasection', 16, now (), '{}'),
	(
		72,
		'precultureresultsdatasection',
		16,
		now (),
		'{}'
	),
	(73, 'pregnancydatasection', 16, now (), '{}'),
	(74, 'wetprepdatasection', 16, now (), '{}'),
	(75, 'wrightsstaindatasection', 16, now (), '{}'),
	(76, 'znstaindatasection', 16, now (), '{}'),
	(
		77,
		'ackreceiptform',
		17,
		now (),
		'{
		"name": "ackreceiptform",
		"title": "@SpeRecG@",
		"formtype": "singlepage",
		"saveEvent": "ACKReceipt",
		"recordView": "specimens",
		"suppressRecordView": false,
	    "startstate": "validspecimen",
		"initialQuery": "SpecimenByIdForACK",
		"pages": [ "ackreceipt","testselectionpage", "culturetypeselectionpage" ],
		"configurable": "Yes",
		"configureactions": ["edit"],
		"rules":
		[
			{ "Outcome": "visible", "page": "testselectionpage", "state": "validspecimen" },
			{ "Outcome": "visible", "page": "culturetypeselectionpage", "state": "validspecimen" }
		]
	}'
	),
	(
		78,
		'rejectspecimenform',
		17,
		now (),
		'{
		"name": "rejectspecimenform",
		"title": "@SpeRej@",
		"formtype": "singlepage",
		"saveEvent": "rejectspecimen",
		"recordView": "specimens",
		"suppressRecordView": false,
		"pages": [ "rejectspecimen" ],
		"configurable": "Yes",
		"configureactions": ["edit"]
	}'
	),
	(
		79,
		'specimencancelrequestform',
		17,
		now (),
		'{
		"name": "specimencancelrequestform",
		"title": "@SpeCan@",
		"formtype": "singlepage",
		"saveEvent": "specimencancelrequest",
		"recordView": "specimens",
		"suppressRecordView": false,
		"initialQuery": "specimenbyidforcancelrequest",
		"pages": [ "specimencancelrequest" ],
		"configurable": "Yes",
		"configureactions": ["edit"]
	}'
	),
	(
		80,
		'editspecimenform',
		17,
		now (),
		'{
		"name": "editspecimenform",
		"title": "@SpeEdiB@",
		"singleItemName": "specimen",
		"startstate": "",
		"saveevent": "editspecimen",
		"recordView": "specimens",
		"initialQuery": "SpecimenByIdForEdit",
		"pages": ["specimenpatientdetails","specimenattributeswhenreceived","specimentimingswhenreceived"],
		"configurable": "Yes",
		"configureactions": ["edit"]
	}'
	),
	(
		81,
		'addcultureform',
		17,
		now (),
		'{
		"name": "addcultureform",
		"title": "@SpeAddD@",
		"newItem": true,
		"singleItemName": "culture",
		"startstate": "",
		"saveEvent": "addculture",
		"recordView": "specimens",
		"suppressRecordView": false,
		"pages": ["specimengrowthdetails","cultureorganismpage","selectorganismpage","organismlistpage","specimenadditionalguidance", "specimenotherinformationpage" ],
		"rules":
		[
			{ "Outcome": "visible", "page": "selectorganismpage", "state": "organismsearch" },
			{ "Outcome": "visible", "page": "organismlistpage", "state": "organismsearch" },
			{ "Outcome": "visible", "page": "cultureorganismpage", "state": "organismselect" }
		],
		"configurable": "Yes",
		"configureactions": ["edit"]
	}'
	),
	(
		82,
		'editcultureform',
		17,
		now (),
		'{
		"name": "editcultureform",
		"title": "Edit Culture",
		"newItem": true,
		"singleItemName": "culture",
		"initialQuery": "CultureById",
	 	"suppressRecordView": true,
		"startstate": "",
		"saveEvent": "editculture",
		"pages": ["specimengrowthdetails","cultureorganismpage","selectorganismpage","organismlistpage","specimenadditionalguidance", "specimenotherinformationpage" ],
		"rules":
		[
			{ "Outcome": "visible", "page": "selectorganismpage", "state": "organismsearch" },
			{ "Outcome": "visible", "page": "organismlistpage", "state": "organismsearch" },
			{ "Outcome": "visible", "page": "cultureorganismpage", "state": "organismselect" }
		],
		"configurable": "Yes",
		"configureactions": ["edit"]
	}'
	),
	(
		83,
		'submitconfirmationform',
		17,
		now (),
		'{
		"name": "submitconfirmationform",
		"formtype": "confirmation",
		"title": "@SpeSubA@",
		"text": "You are about to submit a specimen record for approval.  Please confirm",
		"saveevent": "submitspecimen",
		"recordView": "specimens",
		"suppressRecordView": false,
		"displaySettings": "fullScreenDefault",
		"pages": ["submitconfirmationpage"],
		"configurable": "Yes",
		"configureactions": ["edit"]
	}'
	),
	(
		84,
		'specimenapprovaloneform',
		17,
		now (),
		'{
		"name": "specimenapprovaloneform",
		"title": "@SpeFir@",
		"text": "Level one approval",
		"saveEvent": "specimenapprovalone",
		"recordView": "specimens",
		"suppressRecordView": false,
		"displaySettings": "fullScreenDefault",
		"pages": ["specimenapproval"],
		"configurable": "Yes",
		"configureactions": ["edit"]
	}'
	),
	(
		85,
		'specimenapprovaltwoform',
		17,
		now (),
		'{
		"name": "specimenapprovaltwoform",
		"title": "@SpeSec@",
		"text": "Level two approval",
		"saveEvent": "specimenapprovaltwo",
		"recordView": "specimens",
		"suppressRecordView": false,
		"displaySettings": "fullScreenDefault",
		"pages": ["specimenapprovaltwopage"],
		"configurable": "Yes",
		"configureactions": ["edit"]
	}'
	),
	(
		86,
		'day0benchreadform',
		17,
		now (),
		'{
		"name": "day0benchreadform",
		"title": "@SpeBat@",
		"formtype": "singlepage",
		"saveEvent": "day0benchread",
		"recordView": "specimens",
		"suppressRecordView": false,
		"displaySettings": "fullScreenDefault",
		"pages": [ "day0benchread" ],
		"configurable": "Yes",
		"configureactions": ["edit"]
	}'
	),
	(
		87,
		'day1benchreadform',
		17,
		now (),
		'{
		"name": "day1benchreadform",
		"title": "@SpeBatA@",
		"formtype": "singlepage",
		"saveEvent": "day1benchread",
		"recordView": "specimens",
		"suppressRecordView": false,
		"displaySettings": "fullScreenDefault",
		"pages": [ "day1benchread" ],
		"configurable": "Yes",
		"configureactions": ["edit"]
	}'
	),
	(
		88,
		'gramculturetestform',
		2,
		now (),
		'{
		"name": "gramculturetestform",
		"uievent": "gramculturetestuievent",
		"title": "@TesGra@",
		"saveEvent": "gramculturetest",
		"initialQuery": "gramculturetestbyid",
		"suppressRecordView": true,
		"formtype": "culturetest",
		"datasection": "GramCultureDataSection",
		"configurable": "Yes",
		"pages": ["gramculturetestpage"]
	}'
	),
	(
		89,
		'oxidasetestform',
		2,
		now (),
		'{
		"name": "oxidasetestform",
		"uievent": "oxidasetestuievent",
		"title": "@TesOxi@",
		"saveEvent": "oxidasetest",
		"initialQuery": "oxidasetestbyid",
		"suppressRecordView": true,
		"formtype": "culturetest",
		"datasection": "OxidaseDataSection",
		"configurable": "Yes",
		"pages": ["oxidasetestpage"]
	}'
	),
	(
		90,
		'catalasetestform',
		2,
		now (),
		'{
		"name": "catalasetestform",
		"uievent": "catalasetestuievent",
		"title": "@TesCat@",
		"saveEvent": "catalasetest",
		"initialQuery": "catalasetestbyid",
		"suppressRecordView": true,
		"formtype": "culturetest",
		"datasection": "CatalaseDataSection",
		"configurable": "Yes",
		"pages": ["catalasetestpage"]
	}'
	),
	(94, 'addorganisationform', 17, now (), '{}'),
	(95, 'createspecimenreceivedform', 17, now (), '{}'),
	(96, 'createspecimenrequestform', 17, now (), '{}'),
	(
		97,
		'generalsettings',
		19,
		now (),
		'[
			{ "Id": "generalsettings|timeout", "Text": "@ConScr@", "Value": "10", "Enabled": "true", "Type": "number", "Min": "5", "Max": "1000", "ErrorMessage": "@SetScr@" }
		]'
	),
	(
		98,
		'accessionnumber',
		20,
		now (),
		'[
		{ "Id": "accessionnumber|year", "Text": "@GenYeaB@", "Value": "599", "Enabled": "false", "Type": "yearlist" },
	 	{ "Id": "accessionnumber|monthnumber", "Text": "@GenMonB@", "Value": "2", "Enabled": "false", "Type": "toggle" },
	 	{ "Id": "accessionnumber|specimentype", "Text": "@GraSpeB@", "Enabled": "false", "Type": "mapping",
	 	"MappingValues" : [
	 		{ "Type": "808", "Value": "bl"}, { "Type": "809", "Value": "cf"}, { "Type": "810", "Value": "ea"}, 
	 		{ "Type": "811", "Value": "ey"}, { "Type": "812", "Value": "st"}, { "Type": "813", "Value": "gu"},
	 	 	{ "Type": "814", "Value": "rt"}, { "Type": "815", "Value": "sf"}, { "Type": "816", "Value": "pf"}, 
	 		{ "Type": "818", "Value": "sw"}, { "Type": "819", "Value": "ps"}, { "Type": "820", "Value": "ur"}
	 	], "ErrorMessage": "@SetAll@"},
	 	{ "Id": "accessionnumber|sequence", "Text": "@SetNum@", "Value": "13", "Enabled": "true", "Type": "number", "Min": "3", "Max": "13", "ErrorMessage": "@SetNumA@" }
		]'
	),
	(99, 'instrumentinfo', 19, now (), '{}'),
	(100, 'gramculturesection', 15, now (), '{}'),
	(101, 'apipanelsection', 15, now (), '{}'),
	(102, 'oxidasesection', 15, now (), '{}'),
	(103, 'catalasesection', 15, now (), '{}'),
	(104, 'reportstatus', 21, now(), '[
	{ "Id": "reportstatus|specimenstatus", "Text": "@RepSta@", "Enabled": "false", "Type": "mapping",
	"MappingValues" : [
		{ "Type": "525", "Text": "Preliminary Report", "Colour": "red"}, { "Type": "526", "Text": "Preliminary Report", "Colour": "red"}, 
	 	{ "Type": "527", "Text": "Preliminary Report", "Colour": "red"}, { "Type": "528", "Text": "Preliminary Report", "Colour": "red"},
		{ "Type": "529", "Text": "Preliminary Report", "Colour": "red"}, { "Type": "530", "Text": "Preliminary Report", "Colour": "red"}, 
	 	{ "Type": "531", "Text": "Preliminary Report", "Colour": "red"}, { "Type": "532", "Text": "Preliminary Report", "Colour": "red"},
		{ "Type": "533", "Text": "Preliminary Report", "Colour": "red"}, { "Type": "535", "Text": "Preliminary Report", "Colour": "red"},
	 	{ "Type": "536", "Text": "Preliminary Report", "Colour": "red"}, { "Type": "537", "Text": "Preliminary Report", "Colour": "red"}
	]}
]');

ALTER SEQUENCE configs_id_seq
RESTART WITH 10000;

-- Used to keep field ids unique on forms
INSERT INTO
	NameList (Id, Name)
VALUES
	(1, 'afbquantity'),
	(2, 'antresultid'),
	(3, 'apiidpanel'),
	(4, 'auramineid'),
	(5, 'bacteriaid'),
	(6, 'betalactamaseresultid'),
	(7, 'bloodid'),
	(8, 'carbapenemaseresultid'),
	(9, 'cast'),
	(10, 'castgrid'),
	(11, 'castseen'),
	(12, 'ccrbc'),
	(13, 'ccwbc'),
	(14, 'crystal'),
	(15, 'crystalgrid'),
	(16, 'crystalseen'),
	(17, 'epicells'),
	(18, 'epitheliumid'),
	(19, 'esblresultid'),
	(20, 'foundparasite'),
	(21, 'glucose'),
	(22, 'glucoseid'),
	(23, 'idprofile'),
	(24, 'indiainkresult'),
	(25, 'jevserologyresultid'),
	(26, 'ketonesid'),
	(27, 'kohfungalid'),
	(28, 'kohresultid'),
	(29, 'leucocytesId'),
	(30, 'mononuclear'),
	(31, 'nitritesid'),
	(32, 'organism'),
	(33, 'organismgrid'),
	(34, 'parasite'),
	(35, 'parasitegrid'),
	(36, 'parasitetype'),
	(37, 'percentageid'),
	(38, 'phid'),
	(39, 'polymorphonuclear'),
	(40, 'positiveresult'),
	(41, 'pregnancyid'),
	(42, 'protein'),
	(43, 'proteinid'),
	(44, 'rbcqualitative'),
	(45, 'rbcwetprep'),
	(46, 'resultid'),
	(47, 'specificgravityid'),
	(48, 'wbc'),
	(49, 'wbclist'),
	(50, 'wbcqualitative'),
	(51, 'wbcwetprep'),
	(52, 'wrightsstainresultid'),
	(53, 'yeastid');

ALTER SEQUENCE namelist_id_seq
RESTART WITH 10000;
