CREATE TABLE SpecimenStateHistory (
    Id SERIAL PRIMARY KEY,
	StateId INT NOT NULL,
	SpecimenId INT NOT NULL,
	LastModifiedDate TIMESTAMPTZ NOT NULL
);

CREATE TABLE AccessionNumber (
    Id SERIAL PRIMARY KEY,
	Prefix VARCHAR(30) NOT NULL,
	Counter INT NOT NULL
);

CREATE TABLE Topic (
    Id SERIAL PRIMARY KEY,
	Name VARCHAR(20) NOT NULL
);

CREATE TABLE NameList (
    Id SERIAL PRIMARY KEY,
	Name VARCHAR(40) NOT NULL
);

CREATE TABLE Event(
	Id SERIAL PRIMARY KEY,
	TopicId INT REFERENCES Topic (Id) NOT NULL,
	Name VARCHAR(40) NOT NULL,
	TableName VARCHAR(40) NOT NULL,
	Type VARCHAR(20) NOT NULL
);

CREATE TABLE Organisation (
	Id SERIAL PRIMARY KEY,
	OrganisationName VARCHAR(50) NOT NULL,
	FullyQualifiedName VARCHAR(100),
	Code VARCHAR(15),
	ParentOrganisationId INT,
	LanguageId INT,
	LocationId INT,
	MoreData JSONB,
	Enabled CHAR(3) NOT NULL,
	LastModifiedDate TIMESTAMPTZ NOT NULL
);

CREATE TABLE Users (
	Id SERIAL PRIMARY KEY,
	Enabled CHAR(3) NOT NULL,
	FirstName VARCHAR(30),
	LastName VARCHAR(30),
	Password VARCHAR(100) NOT NULL,
	Username VARCHAR(30) NOT NULL,
	Email VARCHAR(60),
	OrganisationId INT,
	MoreData JSONB,
	LastModifiedDate TIMESTAMPTZ NOT NULL
);

CREATE TABLE OrganisationUser(
	Id SERIAL PRIMARY KEY,
	OrganisationId INT REFERENCES Organisation (Id) NOT NULL,
	UserId INT REFERENCES Users (Id) NOT NULL,
	DefaultOrganisation BOOLEAN NOT NULL
);

CREATE TABLE Laboratory (
	Id SERIAL PRIMARY KEY,
	LaboratoryName VARCHAR(30) NOT NULL,
	LanguageId INT,
	CodingListId VARCHAR(50),
	LastModifiedDate TIMESTAMPTZ NOT NULL
);

CREATE TABLE LaboratoryUser(
	Id SERIAL PRIMARY KEY,
	LaboratoryId INT REFERENCES Laboratory (Id) NOT NULL,
	UserId INT REFERENCES Users (Id) NOT NULL
);

CREATE TABLE Location(
	Id SERIAL PRIMARY KEY,
	ParentLocationId INT,
	Name VARCHAR(50) NOT NULL,
	FullyQualifiedName VARCHAR(100),
	Code VARCHAR(15),
	Latitude numeric(10,5),
	Longitude numeric (10,5),
	MoreData JSONB,
	Enabled CHAR(3) NOT NULL,
	LastModifiedDate TIMESTAMPTZ NOT NULL
);

CREATE TABLE List(
	Id SERIAL PRIMARY KEY,
	Name VARCHAR(30) NOT NULL,
	Grouping VARCHAR(10) NOT NULL,
	ParentId INT,
	Common BOOL,
	Description VARCHAR(100) NOT NULL,
	LastModifiedDate TIMESTAMPTZ NOT NULL
);

CREATE TABLE ListItem (
	Id SERIAL PRIMARY KEY,
	ListId INT REFERENCES List (Id) NOT NULL,
	Value VARCHAR(200) NOT NULL,
	ParentId INT,
	Fixed BOOL,
	Enabled BOOL,
	DisplayOrder INT,
	Deleted BOOL,
	LastModifiedDate TIMESTAMPTZ NOT NULL
);
	
CREATE TABLE Patient (
	Id SERIAL PRIMARY KEY,
	PatientRef VARCHAR(20),
	FirstName VARCHAR(50),
	Surname VARCHAR(50) NOT NULL,
	Age VARCHAR(5),
	DateOfBirth DATE,
	TelephoneNumber VARCHAR(30),
	StateId INT,
	GenderId INT,
	LocationId INT,
	AddressLine1 VARCHAR(50),
	AddressLine2 VARCHAR(50),
	ZipCode VARCHAR(50),
	Barcode VARCHAR(30),
	MoreData JSONB,
	LastModifiedDate TIMESTAMPTZ NOT NULL
);


CREATE TABLE PatientComment (
	Id SERIAL PRIMARY KEY,
	PatientId INT REFERENCES Patient (Id),
	Comment TEXT NOT NULL,
	LastModifiedDate TIMESTAMPTZ NOT NULL
);

CREATE TABLE Queue (
	Id SERIAL PRIMARY KEY,
	RecordId INT,
	Message JSONB NOT NULL,
	UserName VARCHAR(30) NOT NULL,
	Added TIMESTAMPTZ NOT NULL,
	TopicId INT,
	EventId INT,
	EventStatusId INT,
	SpecimenId INT,
	PatientId INT,
	StateId INT,
	Error TEXT
);

CREATE TABLE Role (
	Id SERIAL PRIMARY KEY,
	RoleName VARCHAR(30) NOT NULL,
	RoleDescription VARCHAR(100) NOT NULL,
	LastModifiedDate TIMESTAMPTZ NOT NULL,
	Enabled VARCHAR(3) NOT NULL,
	MoreData JSONB,
	MenuPermission JSONB,
	EventPermission JSONB
);
	
CREATE TABLE Specimen(
	Id SERIAL PRIMARY KEY,
	PatientId INT REFERENCES Patient (Id),
	PatientLocationId INT,
	AdmissionDate DATE,
	DiagnosisId INT,
	ClinicalContactNo VARCHAR(20),
	AccessionNumber VARCHAR(30),
	Barcode VARCHAR(30),
	ExistingBarcode VARCHAR(30),
	SpecimenTypeId INT,
	SpecimenSiteId INT,
	ReceivedConditionId INT,
	SpecimenAppearanceId INT,
	SpecimenWeight INT,
	BottleOnlyWeight NUMERIC(7,3),
	BloodAndBottleWeight NUMERIC(7,3),
	CollectionDate DATE NOT NULL,
	CollectionTime VARCHAR(10),
	ReceivedDate DATE,
	ReceivedTime VARCHAR(10),
	StateId INT,
	GrowthId INT,
	RejectionReason TEXT,
	ApprovalCommentL1Id INT,
	ApprovalCommentL2Id INT,
	ReasonOne TEXT,
	ReasonTwo TEXT,
	LastModifiedDate TIMESTAMPTZ NOT NULL,
	LaboratoryId INT,
	OrganisationId INT,
	MoreData JSONB,
	AlertTypeId int
);

CREATE TABLE SpecimenComment (
	Id SERIAL PRIMARY KEY,
	SpecimenId INT REFERENCES Specimen (Id),
	Comment TEXT NOT NULL,
	LastModifiedDate TIMESTAMPTZ NOT NULL
);

CREATE TABLE Tests(
	Id SERIAL PRIMARY KEY,
	SpecimenId INT REFERENCES Specimen(Id),
	TestName VARCHAR(60) NOT NULL,
	TestResults JSONB,
	Status VARCHAR(20) NOT NULL,
	Requested TIMESTAMPTZ NOT NULL,
	Completed TIMESTAMPTZ,
	AlertTypeId int,
	LastModifiedDate TIMESTAMPTZ NOT NULL
);

CREATE TABLE Culture(
	Id SERIAL PRIMARY KEY,
	TypeId INT,
	SpecimenId INT REFERENCES Specimen (Id),
	SpecimenOrganismId INT,
	OrgGroupCodingId INT,
	SpecimenQuantityId INT,
	PositiveDate DATE,
	PositiveTime varchar(10),
	SpecimenApiIdPanelId INT,
	IdProfile VARCHAR(30),
	IdPercentage VARCHAR(4),
	CommentOneId INT,
	CommentTwoId INT,
	ASTCommentOneId INT,
	ASTCommentTwoId INT,
	AdditionalNotes VARCHAR(5000),
	ASTAdditionalNotes VARCHAR(5000),
	AloquatId VARCHAR(30),
	DisplayOnReport VARCHAR(3),
	MoreData JSONB,
	AlertTypeId int,
	LastModifiedDate TIMESTAMPTZ NOT NULL
);

CREATE TABLE CultureTests(
	Id SERIAL PRIMARY KEY,
	CultureId INT REFERENCES Culture(Id),
	TestName VARCHAR(30) NOT NULL,
	TestResults JSONB,
	Status VARCHAR(20) NOT NULL,
	Requested TIMESTAMPTZ NOT NULL,
	Completed TIMESTAMPTZ,
	AlertTypeId int,
	LastModifiedDate TIMESTAMPTZ NOT NULL
);

CREATE TABLE AST (
	Id SERIAL PRIMARY KEY,
	CultureId INT REFERENCES Culture (Id),
	TestType VARCHAR(10),
	EntryType VARCHAR(10),
	TestMethodId INT,
	AntibioticId INT,
	Dosage INT,
	Measurement NUMERIC(7,3),
	SusceptibilityId INT,
	DisplayOnReport VARCHAR(3),
	MicComparison VARCHAR(3),
	AppliedBreakpointId INT,
	GuidelinesId INT,
	Categoryid INT,
	MoreData JSONB,
	AlertTypeId int,
	LastModifiedDate TIMESTAMPTZ NOT NULL
);

CREATE TABLE SpecialASTRow(
	Id SERIAL PRIMARY KEY,
	ASTId INT,
	SpecialTypeId INT,
	SusceptibilityId INT,
	DisplayOnReport VARCHAR(3),
	BreakpointId INT,
	LastModifiedDate TIMESTAMPTZ NOT NULL
);

CREATE TABLE TestType (
	Id SERIAL PRIMARY KEY,
	Name VARCHAR(30)
);

CREATE TABLE SpecimenTest (
	Id SERIAL PRIMARY KEY,
	SpecimenId INT REFERENCES Specimen (Id),
	TestTypeId INT REFERENCES TestType (Id),
	Results JSONB
);

CREATE TABLE UserRole (
	Id SERIAL PRIMARY KEY,
	UserId INT REFERENCES Users (Id),
	RoleId INT REFERENCES Role (Id),
	LastModifiedDate TIMESTAMPTZ NOT NULL
);
	
CREATE TABLE Language(
	Id SERIAL PRIMARY KEY,
	TranslationId INT,
	SourceId INT,
	Pack JSONB,
	LastModifiedDate TIMESTAMPTZ NOT NULL
);

CREATE TABLE OrderCat(
	Id SERIAL PRIMARY KEY,
	Name VARCHAR(30),
	LastModifiedDate TIMESTAMPTZ NOT NULL
);

CREATE TABLE Family(
	Id SERIAL PRIMARY KEY,
	OrderId INT,
	Name VARCHAR(30),
	LastModifiedDate TIMESTAMPTZ NOT NULL
);

CREATE TABLE Genus(
	Id SERIAL PRIMARY KEY,
	FamilyId INT,
	Name VARCHAR(30),
	LastModifiedDate TIMESTAMPTZ NOT NULL
);

CREATE TABLE Species(
	Id SERIAL PRIMARY KEY,
	GenusId INT,
	Name VARCHAR(30),
	LastModifiedDate TIMESTAMPTZ NOT NULL
);

CREATE TABLE SubSpecies(
	Id SERIAL PRIMARY KEY,
	SpeciesId INT,
	Name VARCHAR(30),
	LastModifiedDate TIMESTAMPTZ NOT NULL
);

CREATE TABLE Serotype(
	Id SERIAL PRIMARY KEY,
	SpeciesId INT,
	SubSpeciesId INT,
	Name VARCHAR(30),
	LastModifiedDate TIMESTAMPTZ NOT NULL
);

CREATE TABLE Additional(
	Id SERIAL PRIMARY KEY,
	Name VARCHAR(50),
	FamilyId INT,
	LastModifiedDate TIMESTAMPTZ NOT NULL
);

CREATE TABLE Organism(
	Id SERIAL PRIMARY KEY,
	GenusId INT,
	SpeciesId INT, 
	SubSpeciesId INT, 
	SerotypeId INT,
	AdditionalId INT,
	GramId INT,
	LastModifiedDate TIMESTAMPTZ NOT NULL
);

CREATE TABLE OrganismSynonyms(
	Id SERIAL PRIMARY KEY,
	OrganismId INT,
	Synonym varchar(60), 
	PreferredName BOOL, 
	LastModifiedDate TIMESTAMPTZ NOT NULL
);

CREATE TABLE OrganismCoding(
	Id SERIAL PRIMARY KEY,
	Code VARCHAR(10),
	OrganismId INT,
	CodingId INT, 
	LastModifiedDate TIMESTAMPTZ NOT NULL
);

CREATE TABLE ResultLine(
	Id SERIAL PRIMARY KEY,
	ResultId INT,
	BreakpointId INT,
	StartVal NUMERIC(7,3),
	EndVal NUMERIC(7,3),
	LastModifiedDate TIMESTAMPTZ NOT NULL
);

CREATE TABLE Antibiotic(
	Id SERIAL PRIMARY KEY,
	Code VARCHAR(10),
	AntibioticName VARCHAR(50),
	GroupId INT,
	Atc VARCHAR(12),
	Cid VARCHAR(12),
	Loinc VARCHAR(200),
	LastModifiedDate TIMESTAMPTZ NOT NULL
);

CREATE TABLE AntibioticGroup(
	Id SERIAL PRIMARY KEY,
	Name VARCHAR(50),
	LastModifiedDate TIMESTAMPTZ NOT NULL
);

CREATE TABLE Breakpoint(
	Id SERIAL PRIMARY KEY,
	OrderId INT,
	FamilyId INT,
	OrganismId INT,
	OrgGroupCodingId INT,
	AntibioticId INT,
	Dosage VARCHAR(5),
	TestMethodId INT,
	SourceId INT,
	SpecialConsiderId INT,
	HostId INT,
	Enabled VARCHAR(3),
	MakeDefault VARCHAR(3),
	LastModifiedDate TIMESTAMPTZ NOT NULL
);

CREATE TABLE SpecimenTypeBreakpoint(
	Id SERIAL PRIMARY KEY,
	SpecimenTypeId INT,
	BreakpointId INT,
	LastModifiedDate TIMESTAMPTZ NOT NULL
);

CREATE TABLE TestPattern(
	Id SERIAL PRIMARY KEY,
	TestPatternName VARCHAR(80),
	OrderId INT,
	FamilyId INT,
	OrganismId INT,
	OrgGroupCodingId INT,
	HostId INT,
	MakeDefault VARCHAR(3),
	LastModifiedDate TIMESTAMPTZ NOT NULL
);

CREATE TABLE TestPatternLine(
	Id SERIAL PRIMARY KEY,
	TestPatternId INT,
	TestOrder INT,
	AntibioticId INT,
	Dosage VARCHAR(5),
	TestMethodId INT,
	GuidelinesId INT,
	Categoryid INT,
	PrintOnReport BOOL,
	LastModifiedDate TIMESTAMPTZ NOT NULL
);

CREATE TABLE BarcodeCounter(
	Id SERIAL PRIMARY KEY,
	BarcodeCount VARCHAR(30),
	LastModifiedDate TIMESTAMPTZ NOT NULL
);

CREATE TABLE ReportHistory(
	Id SERIAL PRIMARY KEY,
	ReportId INT,
	Name VARCHAR(50),
	ReportConfig VARCHAR(50),
	SpecimenId INT,
	PrintStatusId INT,
	Contents JSONB,
	LastModifiedDate TIMESTAMPTZ NOT NULL
);

CREATE TABLE Configs(
	Id SERIAL PRIMARY KEY,
	ConfigName VARCHAR(60),
	ConfigTypeId INT,
	Contents JSONB,
	LastModifiedDate TIMESTAMPTZ NOT NULL
);

CREATE TABLE ConfigType(
	Id SERIAL PRIMARY KEY,
	Name VARCHAR(30),
	Type VARCHAR(20)
);
	
CREATE TABLE AlertType(
	Id SERIAL PRIMARY KEY,
	Colour VARCHAR(30),
	Name VARCHAR(30),
	AlertCategoryId INT,
	PositionId INT,
	ReportPositionId INT,
	LastModifiedDate TIMESTAMPTZ NOT NULL
);

CREATE TABLE Alert(
	Id SERIAL PRIMARY KEY,
	AlertName VARCHAR(100),
	OrderId INT,
	FamilyId INT,
	OrganismId INT,
	OrgGroupCodingId INT,
	DoesExist VARCHAR(3),
	AlertMessage VARCHAR(500),
	TestAndOr VARCHAR(3),
	SusceptibilityAndOr VARCHAR(3),
	Enabled VARCHAR(3),
	AlertTypeId INT,
	TagId VARCHAR(50),
	SourceId INT,
	MoreData JSONB,
	LastModifiedDate TIMESTAMPTZ NOT NULL
);

CREATE TABLE AlertLines(
	Id SERIAL PRIMARY KEY,
	AlertId INT,
	AntibioticId VARCHAR(50),
	SusceptibilityId INT,
	LastModifiedDate TIMESTAMPTZ NOT NULL
);

CREATE TABLE AlertTestLines(
	Id SERIAL PRIMARY KEY,
	AlertId INT,
	TestName VARCHAR(30),
	FieldName VARCHAR(30),
	Comparison VARCHAR(10),
	CompValue VARCHAR(30),
	LastModifiedDate TIMESTAMPTZ NOT NULL
);

CREATE TABLE SpecimenAlert(
	Id SERIAL PRIMARY KEY,
	SpecimenId INT,
	CultureId INT,
	AlertId INT,
	AlertTypeId INT,
	LastModifiedDate TIMESTAMPTZ NOT NULL
);

CREATE TABLE CultureAlert(
	Id SERIAL PRIMARY KEY,
	CultureId INT,
	AlertId INT,
	AlertTypeId INT,
	LastModifiedDate TIMESTAMPTZ NOT NULL
);

CREATE TABLE SpecimenTag(
	Id SERIAL PRIMARY KEY,
	SpecimenId INT,
	ListItemId INT,
	LastModifiedDate TIMESTAMPTZ NOT NULL
);

CREATE TABLE OrganismAlias(
	Id SERIAL PRIMARY KEY,
	Name VARCHAR(50),
	OrganismId INT,
	LastModifiedDate TIMESTAMPTZ NOT NULL
);

CREATE TABLE InstrumentResults(
	Id SERIAL PRIMARY KEY,
	InstrumentName VARCHAR(40),
	SpecimenId INT,
	CultureId INT,
	Barcode VARCHAR(30),
	DateResultReceived TIMESTAMPTZ,
	Status INT,
	MoreData JSONB,
	LastModifiedDate TIMESTAMPTZ NOT NULL
);

CREATE TABLE IF NOT EXISTS qcorganisms (
	id SERIAL PRIMARY KEY, 
	organismid integer NOT NULL,  
	standardsbody character varying(50) NOT NULL, 
	primarystrain character varying(50) NOT NULL,
	otherstrains character varying(100),
	lastmodifieddate timestamp with time zone NOT NULL, 
	CONSTRAINT qcorganisms_organismid_fkey 
		FOREIGN KEY (organismid) 
		REFERENCES organism (id) 
		MATCH SIMPLE 
		ON UPDATE NO ACTION
		ON DELETE NO ACTION
);

CREATE TABLE IF NOT EXISTS qcantibiotics (
	id SERIAL PRIMARY KEY, 
	qcorganismid integer NOT NULL, 
	antibioticId int NOT NULL, 
	mictargetlower decimal, 
	mictargetupper decimal, 
	micrangelower decimal, 
	micrangeupper decimal, 
	diskcontent character varying(20), 
	inhibitionzonediametertargetlower decimal, 
	inhibitionzonediametertargetupper decimal, 
	inhibitionzonediameterrangelower decimal, 
	inhibitionzonediameterrangeupper decimal, 
	lastmodifieddate timestamp with time zone NOT NULL, 
	CONSTRAINT qcantibiotics_qcorganismid_fkey 
		FOREIGN KEY (qcorganismid) 
		REFERENCES qcorganisms (id) 
		MATCH SIMPLE 
		ON UPDATE CASCADE
		ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS iqctests (
	id SERIAL PRIMARY KEY, 
	accessionnumber character varying(30), 
	stateid integer NOT NULL, 
	iqctestprofileid integer NOT NULL, 
	comments character varying(200), 
	createddate timestamp with time zone NOT NULL, 
	completeddate timestamp with time zone, 
	lastmodifieddate timestamp with time zone NOT NULL
);

CREATE TABLE IF NOT EXISTS iqcresults (
	id SERIAL PRIMARY KEY, 
	iqctestid integer NOT NULL, 
	qcantibioticid integer NOT NULL, 
	alerttypeid integer, 
	value decimal, 
	lastmodifieddate timestamp with time zone NOT NULL, 
	CONSTRAINT iqcresults_iqctestid_fkey 
		FOREIGN KEY (iqctestid) 
		REFERENCES iqctests (id) 
		MATCH SIMPLE 
		ON UPDATE CASCADE 
		ON DELETE CASCADE,
	CONSTRAINT iqcresults_qcantibioticid_fkey 
		FOREIGN KEY (qcantibioticid) 
		REFERENCES qcantibiotics (id) 
		MATCH SIMPLE 
		ON UPDATE NO ACTION 
		ON DELETE NO ACTION
);

CREATE TABLE iqctestprofiles(
	id SERIAL PRIMARY KEY,
	Name VARCHAR(50),
	testmethodlistitemid integer NOT NULL,
	lastmodifieddate timestamp with time zone NOT NULL,
	deleteddate timestamp with time zone
);

CREATE TABLE iqctestprofileqcorganisms(
	id SERIAL PRIMARY KEY,
	iqctestprofileid integer NOT NULL,
	qcorganismid integer NOT NULL,
	usebydefault boolean DEFAULT false, 
	lastmodifieddate timestamp with time zone NOT NULL,
	CONSTRAINT iqctestprofileqcorganisms_iqctestprofileid_fkey 
		FOREIGN KEY (iqctestprofileid) 
		REFERENCES iqctestprofiles (id) 
		MATCH SIMPLE 
		ON UPDATE CASCADE
		ON DELETE CASCADE,
	CONSTRAINT iqctestprofileqcorganisms_qcorganismid_fkey 
		FOREIGN KEY (qcorganismid) 
		REFERENCES qcorganisms (id) 
		MATCH SIMPLE 
		ON UPDATE NO ACTION
		ON DELETE NO ACTION
);

CREATE TABLE iqctestprofileqcantibiotics(
	id SERIAL PRIMARY KEY,
	iqctestprofileqcorganismid integer NOT NULL,
	qcantibioticid integer, 
	enabled boolean DEFAULT false, 
	lastmodifieddate timestamp with time zone NOT NULL,
	CONSTRAINT iqctestprofileqcantibiotics_iqctestprofileqcorganismid_fkey 
		FOREIGN KEY (iqctestprofileqcorganismid) 
		REFERENCES iqctestprofileqcorganisms (id) 
		MATCH SIMPLE 
		ON UPDATE CASCADE
		ON DELETE CASCADE,
	CONSTRAINT iqctestprofileqcantibiotics_qcantibioticsid_fkey 
		FOREIGN KEY (qcantibioticid) 
		REFERENCES qcantibiotics (id) 
		MATCH SIMPLE 
		ON UPDATE NO ACTION
		ON DELETE NO ACTION
);

CREATE TABLE exportprofile(
	id SERIAL PRIMARY KEY,
	name character varying(50) NOT NULL, 
	description character varying(250) NOT NULL, 
	modifieddate TIMESTAMPTZ NOT NULL
);

CREATE TABLE exportprofilerecord(
	id SERIAL PRIMARY KEY,
	exportprofileid integer NOT NULL,
	tablename character varying(50) NOT NULL, 
	fieldname character varying(50) NOT NULL,
	headername character varying(100),
	formname character varying(100),
	labelname character varying(150),
	modifieddate TIMESTAMPTZ NOT NULL,
	ordernumber integer NOT NULL,
	CONSTRAINT exportprofilerecord_exportprofileid_fkey 
		FOREIGN KEY (exportprofileid) 
		REFERENCES exportprofile (id) 
		MATCH SIMPLE 
		ON UPDATE CASCADE
		ON DELETE CASCADE
);

CREATE TABLE ExportRunHistory(
	id SERIAL PRIMARY KEY,
	exportprofileid integer NOT NULL,
	filter JSONB,
	runat TIMESTAMPTZ NOT NULL,
	CONSTRAINT exportrunhistory_exportprofileid_fkey 
		FOREIGN KEY (exportprofileid) 
		REFERENCES exportprofile (id) 
		MATCH SIMPLE 
		ON UPDATE CASCADE
		ON DELETE CASCADE
);

CREATE TABLE TopicTranslation(
	id SERIAL PRIMARY KEY,
	listitemname character varying(200) NOT NULL,
	displayedtopic character varying(200) NOT NULL
);

