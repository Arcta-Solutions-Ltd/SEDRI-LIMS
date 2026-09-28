
/* Modified form rules to suppress final direct test & culture selection pages if the specimen is rejected */

update configs set contents = '{
    "name": "ackreceiptform",
    "pages": [
        "ackreceipt",
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
    "recordView": "specimens",
    "startstate": "notrejected",
    "configurable": "Yes",
    "initialQuery": "SpecimenByIdForACK",
    "configureactions": [
        "edit"
    ],
    "suppressRecordView": false
}'
where id = 77;
