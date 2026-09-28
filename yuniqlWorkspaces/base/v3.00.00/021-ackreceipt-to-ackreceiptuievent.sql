-- Changes any configs referencing the ackreceipt UI Event name for buttons to ackreceiptuievent
UPDATE configs 
SET contents = jsonb_set( 
    contents, 
    ARRAY['Buttons', (button_index - 1)::text, 'UIEvent'], 
    '"ackreceiptuievent"'::jsonb 
)
FROM ( 
    SELECT  
        id, 
        idx as button_index 
    FROM configs, 
    jsonb_array_elements(contents->'Buttons') WITH ORDINALITY arr(elem, idx) 
    WHERE elem->>'Key' = 'ackreceipt' 
) AS subquery 
WHERE configs.id = subquery.id; 