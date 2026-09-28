CREATE TABLE SpecimenTypeTestPattern(
	Id SERIAL PRIMARY KEY,
	SpecimenTypeId INT,
	TestPatternId INT,
	LastModifiedDate TIMESTAMPTZ NOT NULL
);