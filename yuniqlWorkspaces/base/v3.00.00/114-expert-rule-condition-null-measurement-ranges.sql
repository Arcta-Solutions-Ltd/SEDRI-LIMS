-- Legacy "no range" rows stored as 0,0 become NULL so explicit user-entered 0 is distinguishable.
UPDATE expertrulecondition
SET startval = NULL, endval = NULL
WHERE startval = 0 AND endval = 0;
