-- Queue taxonomy: missing Topic list (73) / Event list (74) rows referenced by arc.app event configs.
-- TopicTranslation + Topic listitems for Report and Asset; Event listitems + listitemparentchild links.

-- TopicTranslation for Report / Asset (listitemname matches Topic.Name for display)
INSERT INTO TopicTranslation (Id, listitemname, displayedtopic)
SELECT 37, 'Report', 'Report'
WHERE NOT EXISTS (SELECT 1 FROM TopicTranslation WHERE listitemname = 'Report');

INSERT INTO TopicTranslation (Id, listitemname, displayedtopic)
SELECT 38, 'Asset', 'Asset'
WHERE NOT EXISTS (SELECT 1 FROM TopicTranslation WHERE listitemname = 'Asset');

-- Report / Asset Topic rows (list 73)
INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1600, 73, 'Report', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 73 AND LOWER(value) = LOWER('Report'));

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1601, 73, 'Asset', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 73 AND LOWER(value) = LOWER('Asset'));

-- Topic: Report (1600)
INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1701, 74, 'approvereportevent', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('approvereportevent')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 1600, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('approvereportevent')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 1600 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1702, 74, 'batchapprovereportevent', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('batchapprovereportevent')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 1600, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('batchapprovereportevent')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 1600 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1703, 74, 'batchrejectreportevent', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('batchrejectreportevent')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 1600, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('batchrejectreportevent')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 1600 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1704, 74, 'unapprovereportevent', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('unapprovereportevent')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 1600, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('unapprovereportevent')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 1600 AND childid = li.id);

-- Topic: Asset (1601)
INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1705, 74, 'addsupplierevent', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('addsupplierevent')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 1601, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('addsupplierevent')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 1601 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1706, 74, 'editsupplierevent', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('editsupplierevent')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 1601, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('editsupplierevent')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 1601 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1707, 74, 'deletesupplierevent', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('deletesupplierevent')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 1601, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('deletesupplierevent')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 1601 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1708, 74, 'addstorageevent', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('addstorageevent')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 1601, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('addstorageevent')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 1601 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1709, 74, 'editstorageevent', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('editstorageevent')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 1601, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('editstorageevent')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 1601 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1710, 74, 'deletestorageevent', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('deletestorageevent')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 1601, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('deletestorageevent')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 1601 AND childid = li.id);

-- Topic: Billing (152)
INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1711, 74, 'addbillingrule', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('addbillingrule')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 152, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('addbillingrule')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 152 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1712, 74, 'editbillingrule', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('editbillingrule')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 152, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('editbillingrule')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 152 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1713, 74, 'deletebillingrule', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('deletebillingrule')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 152, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('deletebillingrule')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 152 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1714, 74, 'editbillingrecord', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('editbillingrecord')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 152, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('editbillingrecord')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 152 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1715, 74, 'deletebillingrecord', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('deletebillingrecord')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 152, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('deletebillingrecord')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 152 AND childid = li.id);

-- Topic: ExpertRules (147)
INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1716, 74, 'addExpertRule', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('addExpertRule')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 147, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('addExpertRule')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 147 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1717, 74, 'editExpertRule', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('editExpertRule')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 147, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('editExpertRule')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 147 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1718, 74, 'deleteExpertRule', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('deleteExpertRule')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 147, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('deleteExpertRule')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 147 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1719, 74, 'addexpertruleapproval', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('addexpertruleapproval')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 147, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('addexpertruleapproval')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 147 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1720, 74, 'addexpertruleaction', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('addexpertruleaction')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 147, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('addexpertruleaction')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 147 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1721, 74, 'editexpertruleaction', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('editexpertruleaction')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 147, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('editexpertruleaction')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 147 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1722, 74, 'addexpertrulecondition', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('addexpertrulecondition')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 147, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('addexpertrulecondition')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 147 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1723, 74, 'editexpertrulecondition', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('editexpertrulecondition')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 147, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('editexpertrulecondition')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 147 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1724, 74, 'addexpertruletestcondition', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('addexpertruletestcondition')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 147, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('addexpertruletestcondition')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 147 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1725, 74, 'editexpertruletestcondition', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('editexpertruletestcondition')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 147, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('editexpertruletestcondition')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 147 AND childid = li.id);

-- Topic: Specification (148)
INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1726, 74, 'addspecification', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('addspecification')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 148, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('addspecification')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 148 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1727, 74, 'editspecification', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('editspecification')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 148, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('editspecification')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 148 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1728, 74, 'deletespecification', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('deletespecification')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 148, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('deletespecification')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 148 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1729, 74, 'adddocument', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('adddocument')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 148, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('adddocument')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 148 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1730, 74, 'deletedocument', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('deletedocument')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 148, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('deletedocument')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 148 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1731, 74, 'addversion', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('addversion')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 148, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('addversion')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 148 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1732, 74, 'deleteversion', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('deleteversion')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 148, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('deleteversion')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 148 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1733, 74, 'addyear', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('addyear')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 148, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('addyear')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 148 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1734, 74, 'deleteyear', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('deleteyear')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 148, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('deleteyear')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 148 AND childid = li.id);

-- Topic: User (626)
INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1735, 74, 'deleteUser', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('deleteUser')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 626, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('deleteUser')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 626 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1736, 74, 'changepassword', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('changepassword')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 626, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('changepassword')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 626 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1737, 74, 'mypassword', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('mypassword')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 626, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('mypassword')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 626 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1738, 74, 'preference', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('preference')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 626, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('preference')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 626 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1739, 74, 'savefilterpresetsevent', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('savefilterpresetsevent')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 626, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('savefilterpresetsevent')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 626 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1740, 74, 'savecolumnlayoutsevent', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('savecolumnlayoutsevent')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 626, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('savecolumnlayoutsevent')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 626 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1741, 74, 'savehomedashboardevent', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('savehomedashboardevent')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 626, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('savehomedashboardevent')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 626 AND childid = li.id);

-- Topic: Specimen (620)
INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1742, 74, 'cultureprintselector', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('cultureprintselector')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 620, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('cultureprintselector')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 620 AND childid = li.id);

-- Topic: Laboratory (621) — definitions per docs/BusinessRules.md / LaboratoryEventFactory order
INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1743, 74, 'addculturetypecategoryevent', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('addculturetypecategoryevent')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 621, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('addculturetypecategoryevent')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 621 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1744, 74, 'deleteculturetypecategoryevent', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('deleteculturetypecategoryevent')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 621, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('deleteculturetypecategoryevent')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 621 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1745, 74, 'editculturetypecategoryevent', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('editculturetypecategoryevent')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 621, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('editculturetypecategoryevent')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 621 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1746, 74, 'addculturetypeculturetestoptionevent', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('addculturetypeculturetestoptionevent')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 621, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('addculturetypeculturetestoptionevent')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 621 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1747, 74, 'deleteculturetypeculturetestoptionevent', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('deleteculturetypeculturetestoptionevent')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 621, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('deleteculturetypeculturetestoptionevent')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 621 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1748, 74, 'editculturetypeculturetestoptionevent', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('editculturetypeculturetestoptionevent')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 621, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('editculturetypeculturetestoptionevent')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 621 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1749, 74, 'addorganismscopeculturetestoptionevent', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('addorganismscopeculturetestoptionevent')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 621, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('addorganismscopeculturetestoptionevent')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 621 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1750, 74, 'deleteorganismscopeculturetestoptionevent', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('deleteorganismscopeculturetestoptionevent')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 621, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('deleteorganismscopeculturetestoptionevent')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 621 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1751, 74, 'editorganismscopeculturetestoptionevent', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('editorganismscopeculturetestoptionevent')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 621, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('editorganismscopeculturetestoptionevent')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 621 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1752, 74, 'addspecimentypeculturetypeoptionevent', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('addspecimentypeculturetypeoptionevent')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 621, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('addspecimentypeculturetypeoptionevent')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 621 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1753, 74, 'deletespecimentypeculturetypeoptionevent', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('deletespecimentypeculturetypeoptionevent')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 621, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('deletespecimentypeculturetypeoptionevent')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 621 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1754, 74, 'editspecimentypeculturetypeoptionevent', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('editspecimentypeculturetypeoptionevent')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 621, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('editspecimentypeculturetypeoptionevent')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 621 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1755, 74, 'addspecimentypedirecttestoptionevent', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('addspecimentypedirecttestoptionevent')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 621, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('addspecimentypedirecttestoptionevent')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 621 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1756, 74, 'deletespecimentypedirecttestoptionevent', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('deletespecimentypedirecttestoptionevent')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 621, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('deletespecimentypedirecttestoptionevent')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 621 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1757, 74, 'editspecimentypedirecttestoptionevent', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('editspecimentypedirecttestoptionevent')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 621, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('editspecimentypedirecttestoptionevent')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 621 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1758, 74, 'addspecimentypeworkflowevent', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('addspecimentypeworkflowevent')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 621, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('addspecimentypeworkflowevent')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 621 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1759, 74, 'deletespecimentypeworkflowevent', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('deletespecimentypeworkflowevent')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 621, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('deletespecimentypeworkflowevent')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 621 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1760, 74, 'editspecimentypeworkflowevent', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('editspecimentypeworkflowevent')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 621, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('editspecimentypeworkflowevent')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 621 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1761, 74, 'addtestcategoryevent', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('addtestcategoryevent')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 621, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('addtestcategoryevent')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 621 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1762, 74, 'deletetestcategoryevent', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('deletetestcategoryevent')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 621, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('deletetestcategoryevent')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 621 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1763, 74, 'edittestcategoryevent', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('edittestcategoryevent')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 621, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('edittestcategoryevent')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 621 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1764, 74, 'updateturnaroundtimeconfig', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('updateturnaroundtimeconfig')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 621, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('updateturnaroundtimeconfig')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 621 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1765, 74, 'addturnaroundtimerange', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('addturnaroundtimerange')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 621, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('addturnaroundtimerange')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 621 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1766, 74, 'editturnaroundtimerange', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('editturnaroundtimerange')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 621, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('editturnaroundtimerange')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 621 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1767, 74, 'adddirecttestoverride', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('adddirecttestoverride')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 621, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('adddirecttestoverride')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 621 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1768, 74, 'editdirecttestoverride', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('editdirecttestoverride')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 621, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('editdirecttestoverride')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 621 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1769, 74, 'addculturetestoverride', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('addculturetestoverride')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 621, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('addculturetestoverride')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 621 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1770, 74, 'editculturetestoverride', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('editculturetestoverride')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 621, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('editculturetestoverride')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 621 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1771, 74, 'deletelaboratory', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('deletelaboratory')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 621, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('deletelaboratory')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 621 AND childid = li.id);

-- Topic: Instruments (1080)
INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1772, 74, 'batchacceptresultsevent', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('batchacceptresultsevent')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 1080, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('batchacceptresultsevent')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 1080 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1773, 74, 'batchrejectresultsevent', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('batchrejectresultsevent')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 1080, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('batchrejectresultsevent')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 1080 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1774, 74, 'batchreplayinstrumenterror', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('batchreplayinstrumenterror')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 1080, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('batchreplayinstrumenterror')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 1080 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1775, 74, 'replayinstrumenterror', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('replayinstrumenterror')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 1080, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('replayinstrumenterror')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 1080 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1776, 74, 'viewinstrumenterrordetails', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('viewinstrumenterrordetails')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 1080, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('viewinstrumenterrordetails')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 1080 AND childid = li.id);

-- Topic: Lists (720)
INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1777, 74, 'addtable', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('addtable')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 720, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('addtable')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 720 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1778, 74, 'addtableentry', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('addtableentry')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 720, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('addtableentry')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 720 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1779, 74, 'edittable', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('edittable')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 720, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('edittable')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 720 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1780, 74, 'edittableentry', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('edittableentry')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 720, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('edittableentry')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 720 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1781, 74, 'deletetable', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('deletetable')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 720, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('deletetable')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 720 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1782, 74, 'deletetableentry', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('deletetableentry')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 720, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('deletetableentry')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 720 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1783, 74, 'ordertable', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('ordertable')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 720, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('ordertable')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 720 AND childid = li.id);

-- Topic: Patient (623)
INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1784, 74, 'mergepatient', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('mergepatient')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 623, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('mergepatient')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 623 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1785, 74, 'movepatient', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('movepatient')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 623, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('movepatient')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 623 AND childid = li.id);

-- Topic: Organisation (622)
INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1786, 74, 'deleteOrganisation', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('deleteOrganisation')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 622, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('deleteOrganisation')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 622 AND childid = li.id);

-- Topic: Configuration (402) — instrument image events
INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1787, 74, 'uploadimageevent', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('uploadimageevent')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 402, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('uploadimageevent')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 402 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1788, 74, 'updateImageEvent', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('updateImageEvent')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 402, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('updateImageEvent')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 402 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1789, 74, 'deleteImageEvent', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('deleteImageEvent')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 402, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('deleteImageEvent')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 402 AND childid = li.id);

-- Topic: Alert (983)
INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1790, 74, 'addalertapproval', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('addalertapproval')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 983, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('addalertapproval')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 983 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1791, 74, 'addorganismalert', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('addorganismalert')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 983, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('addorganismalert')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 983 AND childid = li.id);

-- Topic: Monitoring (1190)
INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1792, 74, 'vieweventdetails', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('vieweventdetails')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 1190, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('vieweventdetails')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 1190 AND childid = li.id);

-- Topic: Tags (151)
INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1793, 74, 'addspecimentag', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('addspecimentag')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 151, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('addspecimentag')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 151 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1794, 74, 'addpatienttag', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('addpatienttag')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 151, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('addpatienttag')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 151 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1795, 74, 'batchaddspecimentag', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('batchaddspecimentag')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 151, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('batchaddspecimentag')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 151 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1796, 74, 'batchaddpatienttag', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('batchaddpatienttag')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 151, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('batchaddpatienttag')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 151 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1797, 74, 'addtag', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('addtag')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 151, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('addtag')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 151 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1798, 74, 'edittag', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('edittag')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 151, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('edittag')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 151 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1799, 74, 'deletetag', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('deletetag')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 151, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('deletetag')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 151 AND childid = li.id);
