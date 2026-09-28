ALTER TABLE SpecimenComment
Add Column commenttypeid integer,
Add Column displayonreport varchar(3),
Add Column cultureid integer,
Add Column addeddate date,
Add Column addedby varchar(30),
Add Column cannedcommentid integer,
Add Column commentorder integer,
Add Column fieldid varchar(255);

ALTER TABLE specimencomment ALTER COLUMN comment DROP NOT NULL;

insert into list (id, name, grouping, common, description, lastmodifieddate, deleted)
values (126, 'CommentType', 'System', true, 'Comment Type', now(), false);
 
insert into list (id, name, grouping, parentid, common, description, lastmodifieddate, deleted)
values (127, 'CannedComments', 'System', 126, true, 'Standard Comments', now(), false);

insert into listitem (id, listid, value, parentid, fixed, enabled, displayorder, deleted, lastmodifieddate)
values (1231, 126, 'Specimen', null, true, true, 1, false, now()),
		(1232, 126, 'Culture', null, true, true, 1, false, now()),
		(1233, 126, 'AST', null, true, true, 1, false, now());

update specimencomment set commenttypeid = 1231, displayonreport = 'Yes', addedby = 'Transfer';

insert into listitem(listid, value, parentid, fixed, enabled, displayorder, deleted, lastmodifieddate)
select 127, li.value, 1232, true, true, 1, false, now() from listitem li where li.listid = 7;

insert into listitem(listid, value, parentid, fixed, enabled, displayorder, deleted, lastmodifieddate)
select 127, li.value, 1233, true, true, 1, false, now() from listitem li where li.listid = 122;

insert into specimencomment(specimenId, lastmodifieddate, commenttypeid, displayonreport, cultureid, addedby, cannedcommentid, fieldid)
select c.specimenid, c.lastmodifieddate, 1232, 'Yes', c.id, 'Transfer', li2.id, 
'comment1' from culture c
left outer join listitem li on c.commentoneid = li.id
left outer join listitem li2 on li.value = li2.value and li2.listid = 127
where c.commentoneid > 0;

insert into specimencomment(specimenId, lastmodifieddate, commenttypeid, displayonreport, cultureid, addedby, cannedcommentid, fieldid)
select c.specimenid, c.lastmodifieddate, 1232, 'Yes', c.id, 'Transfer', li2.id, 
'comment2' from culture c
left outer join listitem li on c.commenttwoid = li.id
left outer join listitem li2 on li.value = li2.value and li2.listid = 127
where c.commenttwoid > 0;

insert into specimencomment(specimenId, comment, lastmodifieddate, commenttypeid, displayonreport, cultureid, addedby, fieldid)
select c.specimenid, c.additionalnotes, c.lastmodifieddate, 1232, 'Yes', c.id, 'Transfer',
'additionalnotes' from culture c where c.additionalnotes IS NOT NULL OR c.additionalnotes != '';

insert into specimencomment(specimenId, lastmodifieddate, commenttypeid, displayonreport, cultureid, addedby, cannedcommentid, fieldid)
select c.specimenid, c.lastmodifieddate, 1233, 'Yes', c.id, 'Transfer', li2.id, 
'astcommentoneid' from culture c
left outer join listitem li on c.astcommentoneid = li.id
left outer join listitem li2 on li.value = li2.value and li2.listid = 127
where c.astcommentoneid > 0;

insert into specimencomment(specimenId, lastmodifieddate, commenttypeid, displayonreport, cultureid, addedby, cannedcommentid, fieldid)
select c.specimenid, c.lastmodifieddate, 1233, 'Yes', c.id, 'Transfer', li2.id, 
'astcommenttwoid' from culture c
left outer join listitem li on c.astcommenttwoid = li.id
left outer join listitem li2 on li.value = li2.value and li2.listid = 127
where c.astcommenttwoid > 0;

insert into specimencomment(specimenId, comment, lastmodifieddate, commenttypeid, displayonreport, cultureid, addedby, fieldid)
select c.specimenid, c.astadditionalnotes, c.lastmodifieddate, 1233, 'Yes', c.id, 'Transfer', 
'astadditionalnotes' from culture c where c.additionalnotes IS NOT NULL OR c.additionalnotes != '';
