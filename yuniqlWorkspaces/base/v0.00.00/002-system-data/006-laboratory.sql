INSERT INTO laboratory(id, laboratoryname, languageid, lastmodifieddate)
VALUES 
(1, 'Default Laboratory', 669, now());

ALTER SEQUENCE laboratory_id_seq restart with 2;