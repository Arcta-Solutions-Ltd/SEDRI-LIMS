INSERT INTO alerttype
(id, colour, name, alertcategoryid, positionid, reportpositionid, lastmodifieddate)
VALUES 
(1, '#fde7e9', 'Notification', 991, 998, 999, now()),
(2, '#ffffff', 'Invisible', 991, 999, 999, now());

ALTER SEQUENCE alerttype_id_seq RESTART WITH 10000;
