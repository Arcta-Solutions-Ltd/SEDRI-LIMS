INSERT INTO iqctestprofiles 
(id, name, testmethodlistitemid, lastmodifieddate, deleteddate) 
VALUES 
(1, 'Master', 681, now(), NULL),
(2, 'Master', 680, now(), NULL);

ALTER SEQUENCE iqctestprofiles_id_seq RESTART WITH 1000000;
