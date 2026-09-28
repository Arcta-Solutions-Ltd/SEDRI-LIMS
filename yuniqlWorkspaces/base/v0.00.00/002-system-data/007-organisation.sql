INSERT INTO organisation
(id, organisationname, fullyqualifiedname, code, parentorganisationid, languageid, locationid, moredata, enabled, lastmodifieddate)
VALUES 
(1, 'Default Organisation', 'Default Organisation', null, null, 669, null, null, 'Yes', now());

ALTER SEQUENCE organisation_id_seq restart with 2;
