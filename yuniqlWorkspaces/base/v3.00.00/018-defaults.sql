INSERT INTO laboratoryconfigs (Id, LaboratoryId, Configname, Contents, lastmodifieddate)
OVERRIDING SYSTEM VALUE
VALUES 
    (1, 1, 'specimentypeculturetypedefault', '{"Id": 1, "GroupId": "808", "AssociatedListId": "1085"}', now()),
    (2, 1, 'specimentypeculturetypedefault', '{"Id": 2, "GroupId": "810", "AssociatedListId": "978"}', now()),
    (3, 1, 'specimentypeculturetypedefault', '{"Id": 3, "GroupId": "811", "AssociatedListId": "979"}', now()),
    (4, 1, 'specimentypeculturetypedefault', '{"Id": 4, "GroupId": "812", "AssociatedListId": "980"}', now()),
    (5, 1, 'specimentypeculturetypedefault', '{"Id": 5, "GroupId": "813", "AssociatedListId": "981"}', now()),
    (6, 1, 'specimentypeculturetypedefault', '{"Id": 6, "GroupId": "817", "AssociatedListId": "978, 979"}', now()),
    (7, 1, 'specimentypeculturetypedefault', '{"Id": 7, "GroupId": "818", "AssociatedListId": "981, 982"}', now()),
    (8, 1, 'specimentypeculturetypedefault', '{"Id": 8, "GroupId": "820", "AssociatedListId": "978"}', now()),

    (9, 1, 'specimentypedirecttestdefault', '{"Id": 1, "GroupId": "810", "AssociatedListId": "gramstaintestform"}', now()),
    (10, 1, 'specimentypedirecttestdefault', '{"Id": 2, "GroupId": "811", "AssociatedListId": "gramstaintestform"}', now()),
    (11, 1, 'specimentypedirecttestdefault', '{"Id": 3, "GroupId": "812", "AssociatedListId": "microscopytestform"}', now()),
    (12, 1, 'specimentypedirecttestdefault', '{"Id": 4, "GroupId": "813", "AssociatedListId": "gramstaintestform"}', now()),
    (13, 1, 'specimentypedirecttestdefault', '{"Id": 5, "GroupId": "817", "AssociatedListId": "gramstaintestform,znstaintestform"}', now()),
    (14, 1, 'specimentypedirecttestdefault', '{"Id": 6, "GroupId": "818", "AssociatedListId": "gramstaintestform"}', now()),
    (15, 1, 'specimentypedirecttestdefault', '{"Id": 7, "GroupId": "820", "AssociatedListId": "microscopytestform,dipsticktestform"}', now());

ALTER SEQUENCE laboratoryconfigs_id_seq RESTART WITH 10000;