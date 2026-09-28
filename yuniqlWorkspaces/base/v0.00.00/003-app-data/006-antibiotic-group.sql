INSERT INTO antibioticgroup
(id, name, lastmodifieddate)
VALUES 
(100001, 'Antimycobacterials', now()),
(100002, 'Antifungals/antimycotics', now()),
(100003, 'Macrolides/lincosamides', now()),
(100004, 'Other antibacterials', now()),
(100005, 'Aminoglycosides', now()),
(100006, 'Beta-lactams/penicillins', now()),
(100007, 'Glycopeptides', now()),
(100008, 'Quinolones', now()),
(100009, 'Carbapenems', now()),
(100010, 'Trimethoprims', now()),
(100011, 'Cephalosporins (3rd gen.)', now()),
(100012, 'Cephalosporins (1st gen.)', now()),
(100013, 'Cephalosporins (2nd gen.)', now()),
(100014, 'Cephalosporins (4th gen.)', now()),
(100015, 'Cephalosporins (unclassified gen.)', now()),
(100016, 'Cephalosporins (5th gen.)', now()),
(100017, 'Tetracyclines', now()),
(100018, 'Amphenicols', now()),
(100019, 'Polymyxins', now()),
(100020, 'Oxazolidinones', now());

ALTER SEQUENCE antibioticgroup_id_seq RESTART WITH 10021;