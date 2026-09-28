update configs 
set contents = '{
    "name": "specimenapprovaloneform",
    "text": "Level one approval",
    "pages": [
        "specimenapproval"
    ],
    "title": "@SpeFir@",
    "saveEvent": "specimenapprovalone",
    "recordView": "specimenrecordview",
    "configurable": "Yes",
    "displaySettings": "fullScreenDefault",
    "configureactions": [
        "edit"
    ],
    "suppressRecordView": false
}'
where id = 84;

update configs set contents = '{
    "name": "specimenapprovaltwoform",
    "text": "Level two approval",
    "pages": [
        "specimenapprovaltwopage"
    ],
    "title": "@SpeSec@",
    "saveEvent": "specimenapprovaltwo",
    "recordView": "specimenrecordview",
    "configurable": "Yes",
    "displaySettings": "fullScreenDefault",
    "configureactions": [
        "edit"
    ],
    "suppressRecordView": false
}'
where id = 85;

update configs set contents = '{
    "name": "rejectspecimenform",
    "pages": [
        "rejectspecimen"
    ],
    "title": "@SpeRej@",
    "formtype": "singlepage",
    "saveEvent": "rejectspecimen",
    "recordView": "specimenrecordview",
    "configurable": "Yes",
    "configureactions": [
        "edit"
    ],
    "suppressRecordView": false
}'
where id = 78;

update configs set contents = '{
    "name": "specimencancelrequestform",
    "pages": [
        "specimencancelrequest"
    ],
    "title": "@SpeCan@",
    "formtype": "singlepage",
    "saveEvent": "specimencancelrequest",
    "recordView": "specimenrecordview",
    "configurable": "Yes",
    "initialQuery": "specimenbyidforcancelrequest",
    "configureactions": [
        "edit"
    ],
    "suppressRecordView": false
}'
where id = 79;

update configs set contents = '{
    "name": "editspecimenform",
    "pages": [
        "specimenpatientdetails",
        "specimenattributeswhenreceived",
        "specimentimingswhenreceived"
    ],
    "title": "@SpeEdiB@",
    "saveevent": "editspecimen",
    "recordView": "specimenrecordview",
    "startstate": "",
    "configurable": "Yes",
    "initialQuery": "SpecimenByIdForEdit",
    "singleItemName": "specimen",
    "configureactions": [
        "edit"
    ]
}'
where id = 80;

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
        {
            "page": "selectorganismpage",
            "state": "organismsearch",
            "Outcome": "visible"
        },
        {
            "page": "organismlistpage",
            "state": "organismsearch",
            "Outcome": "visible"
        },
        {
            "page": "cultureorganismpage",
            "state": "organismselect",
            "Outcome": "visible"
        }
    ],
    "title": "@SpeAddD@",
    "newItem": true,
    "saveEvent": "addculture",
    "recordView": "specimenrecordview",
    "startstate": "",
    "configurable": "Yes",
    "singleItemName": "culture",
    "configureactions": [
        "edit"
    ],
    "suppressRecordView": false
}'
where id = 81;

update configs set contents = '{
    "name": "day0benchreadform",
    "pages": [
        "day0benchread"
    ],
    "title": "@SpeBat@",
    "formtype": "singlepage",
    "saveEvent": "day0benchread",
    "recordView": "specimenrecordview",
    "configurable": "Yes",
    "displaySettings": "fullScreenDefault",
    "configureactions": [
        "edit"
    ],
    "suppressRecordView": false
}'
where id = 86;

update configs set contents = '{
    "Name": "submitconfirmationform",
    "Pages": [
        "submitconfirmationpage"
    ],
    "Title": "@SpeSubA@",
    "NewItem": false,
    "Expanded": false,
    "Formtype": "confirmation",
    "SaveEvent": "submitspecimen",
    "RecordView": "specimenrecordview",
    "Collapsible": false,
    "UseListData": false,
    "Configurable": "Yes",
    "DisplaySettings": "fullScreenDefault",
    "ConfigureActions": [
        "edit"
    ],
    "SuppressRecordView": false
}'
where id = 83;

update configs set contents = '{
    "name": "day1benchreadform",
    "pages": [
        "day1benchread"
    ],
    "title": "@SpeBatA@",
    "formtype": "singlepage",
    "saveEvent": "day1benchread",
    "recordView": "specimenrecordview",
    "configurable": "Yes",
    "displaySettings": "fullScreenDefault",
    "configureactions": [
        "edit"
    ],
    "suppressRecordView": false
}'
where id = 87;

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
    "recordView": "specimenrecordview",
    "startstate": "notrejected",
    "configurable": "Yes",
    "initialQuery": "SpecimenByIdForACK",
    "configureactions": [
        "edit"
    ],
    "suppressRecordView": false
}'
where id = 77;