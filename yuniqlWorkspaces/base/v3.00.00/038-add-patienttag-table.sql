-- Create PatientTag table to link patients to tags (ListItem where ListId=105)
CREATE TABLE PatientTag(
    Id SERIAL PRIMARY KEY,
    PatientId INT,
    ListItemId INT,
    LastModifiedDate TIMESTAMPTZ NOT NULL
);
