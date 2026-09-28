-- Add organismselect requirement for organism pages so they are hidden when no growth is specified.
-- When growth is empty, organismselect is never added, so selectorganismpage and organismlistpage stay hidden.
update configs set contents = '{
    "name": "addcultureform",
    "pages": [
        "specimengrowthdetails",
        "cultureorganismpage",
        "selectorganismpage",
        "organismlistpage",
        "specimenadditionalguidance",
        "specimenotherinformationpage"
    ],
    "rules": [
        {"page": "selectorganismpage", "state": "organismsearch", "Outcome": "visible"},
        {"page": "selectorganismpage", "state": "organismselect", "Outcome": "visible"},
        {"page": "organismlistpage", "state": "organismsearch", "Outcome": "visible"},
        {"page": "organismlistpage", "state": "organismselect", "Outcome": "visible"},
        {"page": "cultureorganismpage", "state": "organismselect", "Outcome": "visible"}
    ],
    "title": "@SpeAddD@",
    "newItem": true,
    "saveEvent": "addculture",
    "recordView": "specimenrecordview",
    "startstate": "",
    "configurable": "Yes",
    "singleItemName": "culture",
    "configureactions": ["edit"],
    "suppressRecordView": false
}'::jsonb
where configname = 'addcultureform';
