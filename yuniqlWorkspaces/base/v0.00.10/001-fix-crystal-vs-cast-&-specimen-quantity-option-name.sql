/* Swap crystal & cast options */

update listitem set listid = 32 where id >= 300 and id <= 305;
update listitem set listid = 31 where id >= 306 and id <= 310;


/* Correct name of growth field */

update list set description = 'Culture Quantity' where id = 12;
