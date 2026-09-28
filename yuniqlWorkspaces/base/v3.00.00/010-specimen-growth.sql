INSERT INTO List (id, name, grouping, parentId, common, description, lastmodifieddate, deleted ) 
OVERRIDING SYSTEM VALUE
VALUES
    (133, 'Specimengrowth', 'System', null, true, 'Specimen Growth', now (), false);
	
update listitem set listid = 133 where id in (177,178,179,180,181,182,1050,1086,1087);

alter table culture 
add column GrowthId int;

update culture set growthid = specimenquantityid, specimenquantityid = null where specimenquantityid in (177,178,179,180,181,182,1050,1086,1087);
update culture set growthid = 1087 where specimenquantityid > 0;

update listitem set value = 'Growth' where id = 1087;
update listitem set value = 'Mixed flora' where id = 182;
update listitem set value = 'Contaminant' where id = 1050;
update listitem set value = 'Insignificant growth' where id = 1086;

INSERT INTO ListItem
(Id, ListId, Value, ParentId, LastModifiedDate, Fixed, Enabled, DisplayOrder, Deleted)
VALUES 
( 125, 133, 'Overgrown',null, now(), false, true, 1, false),
( 126, 133, 'Significant growth',null, now(), false, true, 1, false),
( 127, 12, 'Rare',null, now(), false, true, 1, false),
( 128, 12, 'Moderate',null, now(), false, true, 1, false),
( 129, 12, 'Abundant',null, now(), false, true, 1, false),
( 130, 12, 'Dominant',null, now(), false, true, 1, false),
( 131, 12, 'Trace',null, now(), false, true, 1, false),
( 132, 12, 'Overgrown',null, now(), false, true, 1, false),
( 133, 12, 'Not quantified',null, now(), false, true, 1, false);

UPDATE listitem SET enabled = false, deleted = true
WHERE id >=183 AND id <= 188;

INSERT INTO listitem(id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
    OVERRIDING SYSTEM VALUE 
	VALUES 
    (2076, 12, '10² CFU/mL', false, true, 1, false, now()),
    (2077, 12, '10³ CFU/mL', false, true, 1, false, now()),
    (2078, 12, '10⁴ CFU/mL', false, true, 1, false, now()),
    (2079, 12, '10⁵ CFU/mL', false, true, 1, false, now()),
    (2080, 12, '>10⁵ CFU/mL', false, true, 1, false, now());
