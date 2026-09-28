-- Reports sidebar menu labels: View Reports → Released Reports, Report Approval → Awaiting Approval.
-- Keys @GenVieG@ (approvedreportview) and @RepRepD@ (unapprovedreportview / list title).
UPDATE language
SET pack = (
    SELECT COALESCE(jsonb_agg(
        CASE
            WHEN elem->>'Key' = '@GenVieG@' THEN jsonb_set(elem, '{Value}', '"Released Reports"'::jsonb)
            WHEN elem->>'Key' = '@RepRepD@' THEN jsonb_set(elem, '{Value}', '"Awaiting Approval"'::jsonb)
            ELSE elem
        END
    ), '[]'::jsonb)
    FROM jsonb_array_elements(COALESCE(pack, '[]'::jsonb)) AS elem
),
lastmodifieddate = now()
WHERE translationid = 669
  AND EXISTS (
    SELECT 1
    FROM jsonb_array_elements(COALESCE(pack, '[]'::jsonb)) AS elem
    WHERE elem->>'Key' IN ('@GenVieG@', '@RepRepD@')
  );
