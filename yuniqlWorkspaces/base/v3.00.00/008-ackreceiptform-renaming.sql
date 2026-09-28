alter table laboratory
add column ApproveReports varchar(3);
update laboratory set ApproveReports = 'No';

update configs 
set contents = '{
    "name": "ackreceiptform",
    "pages": [
        "ackreceiptpage",
        "testselectionpage",
        "culturetypeselectionpage"
    ],
    "rules": [
        {
            "page": "testselectionpage",
            "state": "notrejected",
            "Outcome": "visible"
        },
        {
            "page": "culturetypeselectionpage",
            "state": "notrejected",
            "Outcome": "visible"
        }
    ],
    "title": "@SpeRecG@",
    "formtype": "singlepage",
    "saveEvent": "ACKReceipt",
    "recordView": "specimenrecordview",
    "startstate": "notrejected",
    "configurable": "Yes",
    "initialQuery": "SpecimenByIdForACK",
    "configureactions": [
        "edit"
    ],
    "suppressRecordView": false
}'
where configname = 'ackreceiptform'
