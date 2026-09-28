-- Add Specification and Expert Rules views, and additional events to OrganisationAdmin role (id 2).
-- Views: specifications, expertrules (AllowedSidebarItems)
-- Events: Alert approval, Configuration form/grid, Expert Rules, Export, Laboratory TAT, Specification

-- 1. Add specifications to AllowedSidebarItems (if not present)
UPDATE role
SET menupermission = jsonb_set(
    menupermission,
    '{AllowedSidebarItems}',
    (menupermission->'AllowedSidebarItems') || '["specifications"]'::jsonb
)
WHERE id = 2
  AND NOT (menupermission->'AllowedSidebarItems') @> '["specifications"]'::jsonb;

-- 2. Add expertrules to AllowedSidebarItems (if not present)
UPDATE role
SET menupermission = jsonb_set(
    menupermission,
    '{AllowedSidebarItems}',
    (menupermission->'AllowedSidebarItems') || '["expertrules"]'::jsonb
)
WHERE id = 2
  AND NOT (menupermission->'AllowedSidebarItems') @> '["expertrules"]'::jsonb;

-- 3. Add new events to AllowedEvents (merge without duplicates)
UPDATE role
SET eventpermission = jsonb_set(
    eventpermission,
    '{AllowedEvents}',
    (
        SELECT jsonb_agg(elem)
        FROM (
            SELECT jsonb_array_elements_text(eventpermission->'AllowedEvents') AS elem
            UNION
            SELECT unnest(ARRAY[
                'addalertapproval',
                'adddocument',
                'addfieldgridcolumn',
                'addformgroup',
                'addspecification',
                'addturnaroundtimerange',
                'addversion',
                'addyear',
                'addexpertrule',
                'addexpertruleaction',
                'addexpertrulecondition',
                'addexpertruletestcondition',
                'addculturetestoverride',
                'adddirecttestoverride',
                'deletedocument',
                'deleteexpertrule',
                'deleteformgroup',
                'deletespecification',
                'deleteversion',
                'deleteyear',
                'editculturetestoverride',
                'editdirecttestoverride',
                'editexpertrule',
                'editexpertruleaction',
                'editexpertrulecondition',
                'editfieldgridcolumn',
                'editformgroup',
                'editspecification',
                'editturnaroundtimerange',
                'movefield',
                'reorderformgroups',
                'updateturnaroundtimeconfig',
                'viewspecimenrecord'
            ]) AS elem
        ) sub
    )::jsonb
)
WHERE id = 2;
