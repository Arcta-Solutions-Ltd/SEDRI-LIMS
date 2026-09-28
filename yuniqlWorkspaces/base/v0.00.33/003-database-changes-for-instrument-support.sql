delete from ListItem where id in (878,879,880,881,882,883,884, 19,20,21);
delete from list where id = 123 or id = 125;

INSERT INTO List(Id, Name, Grouping, ParentId, Common, Description, LastModifiedDate)
OVERRIDING SYSTEM VALUE
VALUES 
( 123, 'InstrumentStatus', 'Instrument', null, true,'Instrument Status', now()),
( 125, 'InstrumentDirection', 'System', null, true, 'Instrument Direction', now());

INSERT INTO ListItem(Id, ListId, Value, ParentId, LastModifiedDate, Fixed, Enabled, DisplayOrder, Deleted)
OVERRIDING SYSTEM VALUE
VALUES 
( 878, 74, 'AddInstrumentProfile',877, now(), false, true,1,false),
( 879, 74, 'EditInstrumentProfile',877, now(), false, true,1,false),
( 880, 74, 'DeleteInstrumentProfile',877, now(), false, true,1,false),
( 881, 74, 'Synonym',983, now(), false, true,1,false),
( 882, 123, 'Pending',null, now(), false, true,1,false),
( 883, 123, 'Requested',null, now(), false, true,1,false),
( 884, 123, 'Received',null, now(), false, true,1,false),
( 19, 125, 'Inbound',null, now(), true, true,1,false),
( 20, 125, 'Outbound',null, now(), true, true,1,false),
( 21, 82, 'Master',null, now(), true, true, 1, false);

DROP TABLE IF EXISTS 
	InstrumentResults,
	InstrumentErrors;

CREATE TABLE InstrumentResults(
	Id SERIAL PRIMARY KEY,
	InstrumentProfile VARCHAR(30),
	SpecimenId INT,
	CultureId INT,
	Barcode VARCHAR(30),
	RequestMade TIMESTAMPTZ,
	ResultReceived TIMESTAMPTZ,
	StatusId INT,
	MoreData JSONB,
	LastModifiedDate TIMESTAMPTZ NOT NULL
);

CREATE TABLE InstrumentErrors(
	Id SERIAL PRIMARY KEY,
	ProfileName VARCHAR(40),
	InstrumentResultId INT,
	InstrumentDirectionId INT,
 	ErrorText varchar(2000),
	Message JSONB,
	LastModifiedDate TIMESTAMPTZ NOT NULL
);

CREATE TABLE AntibioticCoding(
	Id SERIAL PRIMARY KEY,
	Code VARCHAR(10),
	AntibioticId INT,
	CodingId INT, 
	LastModifiedDate TIMESTAMPTZ NOT NULL
);

CREATE TABLE configshistory
(
	Id SERIAL PRIMARY KEY,
    configsid integer,
    newcontents jsonb,
    oldcontents jsonb,
    lastmodifieddate timestamp with time zone NOT NULL
);

with grouplist as (select ag.id as agid, li.id as liid, ag.name from antibioticgroup ag
inner join listitem li on li.value = ag.name)
insert into antibioticcoding(code, antibioticid, codingid, lastmodifieddate)
select ant.code, ant.id, gl.liid, now() from antibiotic ant
inner join grouplist gl on gl.agid = ant.groupid;

delete from antibioticcoding; 
insert into antibioticcoding(code, antibioticid, codingid, lastmodifieddate)
select code,id, 21, now() from antibiotic;
