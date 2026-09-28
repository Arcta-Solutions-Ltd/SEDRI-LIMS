alter table reporthistory
add column requesteddate TIMESTAMPTZ;

update reporthistory set requesteddate = now();

INSERT INTO List (id, name, grouping, parentId, common, description, lastmodifieddate, deleted ) 
OVERRIDING SYSTEM VALUE
VALUES
    (132, 'ReportApproval', 'System', null, false, 'Report Approval', now (), false);
	
INSERT INTO listitem (id, listid, value, parentid, lastmodifieddate, fixed, enabled, displayorder, deleted ) 
OVERRIDING SYSTEM VALUE
VALUES
    (122, 132, 'Needs Approval', null, now(), true, true, 1, false),
	(123, 132, 'Approved', null, now(), true, true, 1, false),
	(124, 132, 'Rejected', null, now(), true, true, 1, false);

update reporthistory set reportapprovalid = 123;