ALTER TABLE ExportProfileRecord
Add Column moredata jsonb;

INSERT INTO List(Id, Name, Grouping, ParentId, Common, Description, LastModifiedDate, Deleted)
OVERRIDING SYSTEM VALUE
VALUES 
( 128, 'CommentFormat', 'System', null, false,'Comment Formats', now(), false);

INSERT INTO ListItem
(Id, ListId, Value, ParentId, LastModifiedDate, Fixed, Enabled, DisplayOrder, Deleted)
VALUES 
( 1246, 128, 'Free Text',null, now(), true, true, 1, false),
( 1247, 128, 'Standard',null, now(), true, true, 1, false);

