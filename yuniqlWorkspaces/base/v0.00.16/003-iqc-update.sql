-- QC target and range rows (CLSI M100 / EUCAST QC tables) are not distributed with this repository.

delete from iqctestprofileqcantibiotics;
delete from iqctestprofileqcorganisms;
delete from iqctestprofiles;
delete from qcantibiotics;
delete from qcorganisms;

ALTER SEQUENCE iqctestprofileqcantibiotics_id_seq RESTART WITH 1;
ALTER SEQUENCE iqctestprofileqcorganisms_id_seq RESTART WITH 1;
ALTER SEQUENCE iqctestprofiles_id_seq RESTART WITH 1;
ALTER SEQUENCE qcantibiotics_id_seq RESTART WITH 1;
ALTER SEQUENCE qcorganisms_id_seq RESTART WITH 1;

INSERT INTO qcorganisms
(organismid, standardsbody, primarystrain, otherstrains, lastmodifieddate)
VALUES 
(21443, 'CLSI', 'ATCC 25922', NULL, now()),
(51148, 'CLSI', 'ATCC 27853', NULL, now()),
(58880, 'CLSI', 'ATCC 25923', NULL, now()),
(21443, 'CLSI', 'ATCC 35218', NULL, now()),
(29381, 'CLSI', 'ATCC 700603', NULL, now()),
(21443, 'CLSI', 'NCTC 13353', NULL, now()),
(29381, 'CLSI', 'ATCC BAA-1705', NULL, now()),
(29381, 'CLSI', 'ATCC BAA-2814', NULL, now()),
(728, 'CLSI', 'NCTC 13304', NULL, now()),
(25985, 'CLSI', 'ATCC 49247', NULL, now()),
(25985, 'CLSI', 'ATCC 49766', NULL, now()),
(39665, 'CLSI', 'ATCC 49226', NULL, now()),
(59751, 'CLSI', 'ATCC 49619', NULL, now()),
(58880, 'CLSI', 'ATCC 29213', NULL, now()),
(20808, 'CLSI', 'ATCC 29212', NULL, now()),
(21443, 'EUCAST', 'ATCC 25922', 'NCTC 12241, CIP 76.24, DSM 1103, CCUG 17620, CECT 434', now()),
(51148, 'EUCAST', 'ATCC 27853', 'NCTC 12903, CIP 76.110, DSM 1117, CCUG 17619, CECT 108', now()),
(58880, 'EUCAST', 'ATCC 29213', 'NCTC 12973, CIP 103429, DSM 2569, CCUG 15915, CECT 794', now()),
(20808, 'EUCAST', 'ATCC 29212', 'NCTC 12697, CIP 103214, DSM 2570, CCUG 9997, CECT 795', now()),
(59751, 'EUCAST', 'ATCC 49619', 'NCTC 12977, CIP 104340, DSM 11967, CCUG 33638', now()),
(25985, 'EUCAST', 'ATCC 49766', 'NCTC 12975, CIP 103570, DSM 11970, CCUG 29539', now()),
(10993, 'EUCAST', 'ATCC 33560', 'NCTC 11351, CIP 70.2T, DSM 4688, CCUG 11284', now()),
(34094, 'EUCAST', 'ATCC 33396', 'NCTC 9380, DSM 10531, CCUG 12392T', now()),
(8385, 'EUCAST', 'ATCC 25285', 'NCTC 9343, DSM 2151, CCUG 4856T', now()),
(14463, 'EUCAST', 'ATCC 13124', 'NCTC 8237, CIP 103409, DSM 756, CCUG 1795T, CECT 376 T', now()),
(21443, 'EUCAST', 'ATCC 35218', 'NCTC 11954, CIP 102181, DSM 5923, CCUG 30600, CECT 943', now()),
(29381, 'EUCAST', 'ATCC 700603', 'NCTC 13368, CCUG 45421, CECT 7787', now()),
(29381, 'EUCAST', 'ATCC BAA-2814', '', now()),
(58880, 'EUCAST', 'ATCC 29213', 'NCTC 12973, CIP 103429, DSM 2569, CCUG 15915, CECT 794', now());

ALTER SEQUENCE qcorganisms_id_seq RESTART WITH 1000000;

ALTER SEQUENCE qcantibiotics_id_seq RESTART WITH 1000000;

INSERT INTO iqctestprofiles VALUES (1, 'Master', 681, now(), NULL);
INSERT INTO iqctestprofiles VALUES (2, 'Master', 680, now(), NULL);

ALTER SEQUENCE iqctestprofiles_id_seq RESTART WITH 1000000;

INSERT INTO iqctestprofileqcorganisms 
(iqctestprofileid, qcorganismid, usebydefault, lastmodifieddate)
VALUES 
(1, 1, false, now()),
(1, 2, false, now()),
(1, 3, false, now()),
(1, 4, false, now()),
(1, 5, false, now()),
(1, 6, false, now()),
(1, 7, false, now()),
(1, 8, false, now()),
(1, 9, false, now()),
(1, 10, false, now()),
(1, 11, false, now()),
(1, 12, false, now()),
(1, 13, false, now()),
(1, 16, false, now()),
(1, 17, false, now()),
(1, 18, false, now()),
(1, 19, false, now()),
(1, 20, false, now()),
(1, 21, false, now()),
(1, 22, false, now()),
(1, 24, false, now()),
(1, 25, false, now()),
(1, 26, false, now()),
(1, 27, false, now()),
(1, 28, false, now()),
(1, 29, false, now()),
(2, 1, false, now()),
(2, 2, false, now()),
(2, 4, false, now()),
(2, 5, false, now()),
(2, 6, false, now()),
(2, 7, false, now()),
(2, 8, false, now()),
(2, 9, false, now()),
(2, 10, false, now()),
(2, 11, false, now()),
(2, 13, false, now()),
(2, 14, false, now()),
(2, 15, false, now()),
(2, 16, false, now()),
(2, 17, false, now()),
(2, 18, false, now()),
(2, 19, false, now()),
(2, 20, false, now()),
(2, 21, false, now()),
(2, 23, false, now()),
(2, 24, false, now()),
(2, 25, false, now()),
(2, 26, false, now()),
(2, 27, false, now()),
(2, 28, false, now());

ALTER SEQUENCE iqctestprofileqcorganisms_id_seq RESTART WITH 1000000;

ALTER SEQUENCE iqctestprofileqcantibiotics_id_seq RESTART WITH 1000000;
