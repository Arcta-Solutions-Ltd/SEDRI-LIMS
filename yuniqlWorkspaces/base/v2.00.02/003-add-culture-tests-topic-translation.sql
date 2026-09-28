insert into TopicTranslation(Id, listitemname, displayedtopic)
overriding system value
values 
( 25, 'CultureTests', 'Culture Tests');

INSERT INTO ListItem
(Id, ListId, Value, ParentId, LastModifiedDate, Fixed, Enabled, DisplayOrder, Deleted)
VALUES 
( 1248, 73, 'CultureTests',null, now(), false, true, 1, false);

UPDATE ListItem
SET ParentId = 1248
WHERE Id in (799, 804, 805, 806, 807, 976, 977, 1077, 1078, 1079);
