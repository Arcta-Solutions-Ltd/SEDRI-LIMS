INSERT INTO
    AccessionNumber (prefix, counter)
VALUES
    ('<:sequence:>', 0);

INSERT INTO
    Topic (Id, Name)
VALUES
    (1, 'User'),
    (2, 'Role');

ALTER SEQUENCE Topic_id_seq RESTART WITH 3;

INSERT INTO
    TopicTranslation (Id, listitemname, displayedtopic)
VALUES
    (1, 'Configuration', 'Configuration'),
    (2, 'Specimen', 'Specimen'),
    (3, 'Laboratory', 'Laboratory'),
    (4, 'Organisation', 'Organisation'),
    (5, 'Patient', 'Patient'),
    (6, 'Role', 'Role'),
    (7, 'Tests', 'Direct Tests'),
    (8, 'User', 'User'),
    (9, 'Culture', 'Culture'),
    (10, 'Language', 'Language'),
    (11, 'Coding', 'Coding'),
    (12, 'Lists', 'Lists'),
    (13, 'Breakpoints', 'Breakpoints'),
    (14, 'Organism', 'Organism'),
    (15, 'TestPattern', 'Test Patterns'),
    (16, 'Export', 'Export'),
    (17, 'Alert', 'Alert'),
    (18, 'Config', 'Config'),
    (19, 'Location', 'Location'),
    (20, 'Quality', 'Quality'),
    (21, 'Instruments', 'Instruments'),
    (22, 'Monitoring', 'Monitoring'),
    (23, 'Settings', 'Settings');
    
ALTER SEQUENCE TopicTranslation_id_seq RESTART WITH 24;
