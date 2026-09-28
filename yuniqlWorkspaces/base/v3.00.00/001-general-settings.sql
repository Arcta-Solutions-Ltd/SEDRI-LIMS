delete from ListItem where id in (885);

INSERT INTO ListItem(Id, ListId, Value, ParentId, LastModifiedDate, Fixed, Enabled, DisplayOrder, Deleted)
OVERRIDING SYSTEM VALUE
VALUES 
( 885, 123, 'Awaiting Acceptance',null, now(), false, true,1,false);



