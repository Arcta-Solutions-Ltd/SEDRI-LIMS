update configs
set contents = '{
    "name": "editcultureform",
    "pages": [
        "editculturepage"
    ],
    "title": "Edit Culture",
    "newItem": true,
    "saveEvent": "editculture",
    "startstate": "",
    "configurable": "Yes",
    "initialQuery": "CultureById",
    "singleItemName": "culture",
    "configureactions": [
        "edit"
    ],
    "suppressRecordView": true
}'
where id = 82;
