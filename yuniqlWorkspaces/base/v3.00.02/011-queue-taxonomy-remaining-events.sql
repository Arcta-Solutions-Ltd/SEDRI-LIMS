-- Queue taxonomy: remaining Event list (74) rows + listitemparentchild links.
-- Fixes mismatched event names and adds events missing from Monitoring log display.
-- Ids 1819-1886 (125 uses 1815-1818 for AST canned comments list).

-- Align existing listitem values with EventConfig.EventName.
UPDATE listitem SET value = 'acceptinstrumentresults', lastmodifieddate = now() WHERE id = 1082 AND listid = 74 AND LOWER(value) = LOWER('instrumentresultsaccepted');
UPDATE listitem SET value = 'WrightsStainTest', lastmodifieddate = now() WHERE id = 747 AND listid = 74 AND LOWER(value) = LOWER('WrightStainTest');
UPDATE listitem SET value = 'HPyloriantigenTest', lastmodifieddate = now() WHERE id = 752 AND listid = 74 AND LOWER(value) = LOWER('HPyloriAntigen');

-- Topic: Breakpoints (721)
INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1819, 74, 'addbreakpointapproval', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('addbreakpointapproval')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 721, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('addbreakpointapproval')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 721 AND childid = li.id);

-- Topic: Coding (715)
INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1820, 74, 'addantibiotic', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('addantibiotic')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 715, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('addantibiotic')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 715 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1821, 74, 'addAntibioticEntry', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('addAntibioticEntry')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 715, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('addAntibioticEntry')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 715 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1822, 74, 'AddAntibioticGroup', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('AddAntibioticGroup')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 715, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('AddAntibioticGroup')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 715 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1823, 74, 'addrulecategory', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('addrulecategory')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 715, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('addrulecategory')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 715 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1824, 74, 'addSource', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('addSource')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 715, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('addSource')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 715 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1825, 74, 'deleteAntibiotic', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('deleteAntibiotic')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 715, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('deleteAntibiotic')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 715 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1826, 74, 'deleteAntibioticEntry', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('deleteAntibioticEntry')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 715, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('deleteAntibioticEntry')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 715 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1827, 74, 'deleteAntibioticGroup', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('deleteAntibioticGroup')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 715, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('deleteAntibioticGroup')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 715 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1828, 74, 'deleterulecategory', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('deleterulecategory')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 715, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('deleterulecategory')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 715 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1829, 74, 'deletesource', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('deletesource')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 715, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('deletesource')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 715 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1830, 74, 'editAntibiotic', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('editAntibiotic')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 715, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('editAntibiotic')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 715 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1831, 74, 'editrulecategory', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('editrulecategory')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 715, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('editrulecategory')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 715 AND childid = li.id);

-- Topic: Configuration (402)
INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1832, 74, 'addField', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('addField')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 402, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('addField')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 402 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1833, 74, 'addfieldgridcolumn', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('addfieldgridcolumn')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 402, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('addfieldgridcolumn')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 402 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1834, 74, 'addmappingevent', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('addmappingevent')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 402, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('addmappingevent')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 402 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1835, 74, 'addPage', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('addPage')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 402, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('addPage')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 402 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1836, 74, 'addReportConfig', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('addReportConfig')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 402, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('addReportConfig')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 402 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1837, 74, 'addSection', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('addSection')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 402, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('addSection')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 402 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1838, 74, 'addworkflowevent', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('addworkflowevent')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 402, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('addworkflowevent')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 402 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1839, 74, 'deleteField', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('deleteField')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 402, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('deleteField')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 402 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1840, 74, 'deletemappingevent', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('deletemappingevent')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 402, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('deletemappingevent')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 402 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1841, 74, 'deletePage', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('deletePage')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 402, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('deletePage')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 402 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1842, 74, 'deleteReportConfig', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('deleteReportConfig')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 402, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('deleteReportConfig')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 402 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1843, 74, 'deleteSection', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('deleteSection')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 402, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('deleteSection')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 402 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1844, 74, 'deleteworkflowevent', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('deleteworkflowevent')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 402, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('deleteworkflowevent')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 402 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1845, 74, 'disableCultureTest', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('disableCultureTest')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 402, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('disableCultureTest')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 402 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1846, 74, 'disableDirectTest', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('disableDirectTest')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 402, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('disableDirectTest')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 402 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1847, 74, 'editField', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('editField')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 402, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('editField')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 402 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1848, 74, 'editfieldgridcolumn', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('editfieldgridcolumn')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 402, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('editfieldgridcolumn')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 402 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1849, 74, 'editmappingevent', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('editmappingevent')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 402, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('editmappingevent')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 402 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1850, 74, 'editPage', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('editPage')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 402, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('editPage')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 402 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1851, 74, 'editpages', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('editpages')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 402, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('editpages')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 402 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1852, 74, 'editReportConfig', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('editReportConfig')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 402, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('editReportConfig')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 402 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1853, 74, 'editReportSection', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('editReportSection')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 402, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('editReportSection')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 402 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1854, 74, 'editSection', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('editSection')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 402, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('editSection')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 402 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1855, 74, 'editworkflowevent', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('editworkflowevent')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 402, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('editworkflowevent')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 402 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1856, 74, 'exportconfiguration', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('exportconfiguration')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 402, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('exportconfiguration')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 402 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1857, 74, 'importconfiguration', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('importconfiguration')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 402, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('importconfiguration')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 402 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1858, 74, 'reportSectionMove', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('reportSectionMove')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 402, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('reportSectionMove')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 402 AND childid = li.id);

-- Topic: Culture (668)
INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1859, 74, 'addisolateevent', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('addisolateevent')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 668, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('addisolateevent')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 668 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1860, 74, 'culturecommentevent', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('culturecommentevent')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 668, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('culturecommentevent')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 668 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1861, 74, 'DeleteIsolateEvent', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('DeleteIsolateEvent')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 668, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('DeleteIsolateEvent')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 668 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1862, 74, 'editaliquotevent', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('editaliquotevent')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 668, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('editaliquotevent')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 668 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1863, 74, 'editisolateevent', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('editisolateevent')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 668, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('editisolateevent')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 668 AND childid = li.id);

-- Topic: CultureTests (1248)
INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1864, 74, 'removeculturetest', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('removeculturetest')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 1248, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('removeculturetest')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 1248 AND childid = li.id);

-- Topic: Export (969)
INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1865, 74, 'addexportprofilefield', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('addexportprofilefield')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 969, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('addexportprofilefield')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 969 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1866, 74, 'deleteExportProfileField', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('deleteExportProfileField')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 969, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('deleteExportProfileField')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 969 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1867, 74, 'dhis2export', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('dhis2export')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 969, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('dhis2export')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 969 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1868, 74, 'editExportProfileFields', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('editExportProfileFields')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 969, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('editExportProfileFields')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 969 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1869, 74, 'runexportprofile', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('runexportprofile')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 969, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('runexportprofile')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 969 AND childid = li.id);

-- Topic: Instruments (1080)
INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1870, 74, 'instrumentculture', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('instrumentculture')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 1080, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('instrumentculture')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 1080 AND childid = li.id);

-- Topic: Laboratory (621)
INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1871, 74, 'addCultureTypeCultureTest', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('addCultureTypeCultureTest')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 621, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('addCultureTypeCultureTest')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 621 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1872, 74, 'deleteculturetypeculturetest', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('deleteculturetypeculturetest')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 621, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('deleteculturetypeculturetest')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 621 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1873, 74, 'editCultureTypeCultureTest', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('editCultureTypeCultureTest')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 621, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('editCultureTypeCultureTest')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 621 AND childid = li.id);

-- Topic: Patient (623)
INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1874, 74, 'deletepatient', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('deletepatient')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 623, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('deletepatient')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 623 AND childid = li.id);

-- Topic: Settings (596)
INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1875, 74, 'addaccessionnumbertext', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('addaccessionnumbertext')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 596, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('addaccessionnumbertext')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 596 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1876, 74, 'addpatientreferencetext', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('addpatientreferencetext')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 596, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('addpatientreferencetext')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 596 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1877, 74, 'deletesetting', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('deletesetting')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 596, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('deletesetting')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 596 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1878, 74, 'editaccessionnumber', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('editaccessionnumber')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 596, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('editaccessionnumber')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 596 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1879, 74, 'editpatientreference', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('editpatientreference')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 596, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('editpatientreference')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 596 AND childid = li.id);

-- Topic: Specimen (620)
INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1880, 74, 'batchspecimenapprovalone', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('batchspecimenapprovalone')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 620, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('batchspecimenapprovalone')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 620 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1881, 74, 'batchspecimenapprovaltwo', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('batchspecimenapprovaltwo')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 620, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('batchspecimenapprovaltwo')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 620 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1882, 74, 'batchsubmitconfirmation', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('batchsubmitconfirmation')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 620, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('batchsubmitconfirmation')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 620 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1883, 74, 'editcommentforselector', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('editcommentforselector')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 620, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('editcommentforselector')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 620 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1884, 74, 'ordercomments', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('ordercomments')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 620, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('ordercomments')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 620 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1885, 74, 'viewspecimenrecord', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('viewspecimenrecord')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 620, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('viewspecimenrecord')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 620 AND childid = li.id);

-- Topic: Tests (625)
INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1886, 74, 'removedirecttest', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('removedirecttest')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 625, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('removedirecttest')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 625 AND childid = li.id);
