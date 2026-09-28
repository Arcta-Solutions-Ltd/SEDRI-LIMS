INSERT INTO
    configs (
        id,
        configname,
        configtypeid,
        contents,
        lastmodifieddate
    )
VALUES
    (
        111,
        'doublecolumnone',
        21,
        '{
    "Name": "DoubleColumnOne",
    "Type": "DoubleFieldColumn",
    "Description": "@RepTwo@",
    "Heading": [
        {
            "Line": 1,
            "Left": 20,
            "Text": "@PatDet@",
            "FontSize": 14,
            "Bold": true
        }
    ],
    "Columns": [
        {
            "left": 20,
            "width": 240,
            "labelwidth": 80
        },
        {
            "left": 300,
            "width": 240,
            "labelwidth": 80
        }
    ]
}',
        NOW ()
    ),
    (
        112,
        'doublecolumntwo',
        21,
        '{
    "Name": "DoubleColumnTwo",
    "Type": "DoubleFieldColumnWithRowHeading",
    "Description": "@RepTwoA@",
    "Heading": [
        {
            "Line": 1,
            "Left": 20,
            "Text": "@RepCel@",
            "FontSize": 12,
            "Bold": true
        }
    ],
    "Columns": [
        {
            "left": 100,
            "width": 220,
            "labelwidth": 100
        },
        {
            "left": 330,
            "width": 220,
            "labelwidth": 100
        }
    ]
}',
        NOW ()
    ),
    (
        113,
        'doublecolumnthree',
        21,
        '{
    "Name": "DoubleColumnThree",
    "Type": "DoubleFieldColumnWithRowHeading",
    "Description": "@RepTwoC@",
    "Heading": [
        {
            "Line": 1,
            "Left": 20,
            "Text": "@PatDet@",
            "FontSize": 14,
            "Bold": true
        }
    ],
    "Columns": [
        {
            "left": 100,
            "width": 180,
            "labelwidth": 100
        },
        {
            "left": 300,
            "width": 180,
            "labelwidth": 100
        }
    ]
}',
        NOW ()
    ),
    (
        114,
        'doublecolumnfour',
        21,
        '{
    "Name": "DoubleColumnFour",
    "Type": "DoubleFieldColumn",
    "Description": "@RepTwoE@",
    "Heading": [
        {
            "Line": 1,
            "Left": 20,
            "Text": "@PatDet@",
            "FontSize": 14,
            "Bold": true
        }
    ],
    "Columns": [
        {
            "left": 20,
            "width": 255,
            "labelwidth": 120
        },
        {
            "left": 315,
            "width": 255,
            "labelwidth": 120
        }
    ]
}',
        NOW ()
    ),
    (
        115,
        'doublecolumnwithgridone',
        21,
        '{
    "Name": "DoubleColumnWithGridOne",
    "Type": "DoubleFieldColumn",
    "Description": "@RepTwoB@",
    "Heading": [
        {
            "Line": 1,
            "Left": 20,
            "Text": "@RepGra@",
            "FontSize": 12,
            "Bold": true
        }
    ],
    "Columns": [
        {
            "left": 100,
            "width": 180,
            "labelwidth": 100
        },
        {
            "left": 300,
            "width": 180,
            "labelwidth": 100
        }
    ],
    "Grids": [
        {
            "left": 100,
            "width": "150|150|80"
        }
    ]
}',
        NOW ()
    ),
    (
        116,
        'doublecolumnwithgridtwo',
        21,
        '{
    "Name": "DoubleColumnWithGridTwo",
    "Type": "DoubleFieldColumn",
    "Description": "@RepTwoD@",
    "Heading": [
        {
            "Line": 1,
            "Left": 20,
            "Text": "@RepGra@",
            "FontSize": 12,
            "Bold": true
        }
    ],
    "Columns": [
        {
            "left": 100,
            "width": 180,
            "labelwidth": 100
        },
        {
            "left": 300,
            "width": 180,
            "labelwidth": 100
        }
    ],
    "Grids": [
        {
            "left": 100,
            "width": "120|100|100|100"
        }
    ]
}',
        NOW ()
    ),
    (
        117,
        'singlecolumnone',
        21,
        '{
    "Name": "SingleColumnOne",
    "Type": "SingleFieldColumnWithSeparateHeading",
    "Description": "@RepSin@",
    "Heading": [
        {
            "Line": 1,
            "Left": 20,
            "Text": "@PatDet@",
            "FontSize": 14,
            "Bold": true
        }
    ],
    "Columns": [
        {
            "left": 20,
            "width": 520,
            "labelwidth": 80
        }
    ]
}',
        NOW ()
    ),
    (
        118,
        'singlecolumntwo',
        21,
        '{
    "Name": "SingleColumnTwo",
    "Type": "SingleFieldColumnWithSeparateHeading",
    "Description": "@RepSin@",
    "Heading": [
        {
            "Line": 1,
            "Left": 20,
            "Text": "@PatDet@",
            "FontSize": 14,
            "Bold": true
        }
    ],
    "Columns": [
        {
            "left": 20,
            "width": 440,
            "labelwidth": 160
        }
    ]
}',
        NOW ()
    ),
    (
        119,
        'tableone',
        21,
        '{
    "Name": "TableOne",
    "Type": "Table",
    "Grids": [
        {
            "left": 20,
            "width": "200|100|140|80"
        }
    ]
}',
        NOW ()
    ),
    (
        120,
        'tabletwo',
        21,
        '{
    "Name": "TableTwo",
    "Type": "Table",
    "Grids": [
        {
            "left": 20,
            "width": "183|183|184"
        }
    ]
}',
        NOW ()
    ),
    (
        121,
        'dynamicsinglecolumnone',
        21,
        '{
    "Name": "DynamicSingleColumnOne",
    "Type": "DynamicSingleColumnOne",
    "Description": "@RepDyn@",
    "Columns": [
        {
            "left": 100,
            "width": 440,
            "labelwidth": 100
        }
    ]
}',
        NOW ()
    ),
    (
        122,
        'imageone',
        21,
        '{
    "Name": "ImageOne",
    "Type": "ReportImage",
    "Columns": [
        {
            "left": 160,
            "width": 255,
            "labelwidth": 80
        }
    ]
}',
        NOW ()
    ),
    (
        123,
        'absolute',
        21,
        '{
    "Name": "Absolute",
    "Type": "Lines"
}',
        NOW ()
    );