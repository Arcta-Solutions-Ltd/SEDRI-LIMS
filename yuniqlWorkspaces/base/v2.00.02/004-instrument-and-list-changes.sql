insert into list(id, name, grouping, common, description, lastmodifieddate, deleted)
values 
	(10, 'InstrumentEvent', 'Instrument', false, 'Instrument event list', now(), false),
	(129, 'InstrumentErrorStatus', 'System', false, 'Instrument Error Status', now(), false);

insert into listitem(id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate, parentid)
values 
	(9, 10, 'Organism Id and AST', true, true, 1, false, now(),null),
	(10, 10, 'Direct Test', true, true, 1, false, now(),null),
	(11, 129, 'Failed', true, true, 1, false, now(),null),
	(12, 129, 'Resolved', true, true, 1, false, now(),null),
	(13, 74, 'AddExportProfile', true, true,1,false, now(),969),
	(14, 74, 'EditExportProfile', true, true,1,false, now(),969),
	(15, 74, 'DeleteExportProfile', true, true,1,false, now(),969),
	(16, 74, 'SpecimenComment', true, true,1,false, now(),620),
	(17, 74, 'CultureComment', true, true,1,false, now(),668),
	(18, 74, 'EditComment', true, true,1,false, now(),620),
	(22, 74, 'DeleteComment', true, true,1,false, now(),620),
	(23, 74, 'SpecimenPrintPreview', true, true,1,false, now(),620);
	
alter table instrumentresults
add column RawResult JSONB;

alter table instrumenterrors
add column errorstatusid INT;