CREATE TABLE
    supplier (
        id SERIAL PRIMARY KEY,
        code VARCHAR(30) NULL,
        name VARCHAR(100) NOT NULL,
        addressline1 VARCHAR(50) NULL,
        addressline2 VARCHAR(50) NULL,
        locationid INT,
        zipcode VARCHAR(50) NULL,
        supplierstatusid INT,
        moredata JSONB,
        lastmodifieddate TIMESTAMPTZ NOT NULL
    );

CREATE TABLE
    suppliercontact (
        id SERIAL PRIMARY KEY,
        supplierid INT NOT NULL,
        contactid INT NOT NULL,
        lastmodifieddate TIMESTAMPTZ NOT NULL
    );

CREATE TABLE
    contact (
        id SERIAL PRIMARY KEY,
        name VARCHAR(100) NOT NULL,
        email VARCHAR(100),
        telephone VARCHAR(30),
        mobile VARCHAR(30),
        moredata JSONB,
        LASTMODIFIEDDATE TIMESTAMPTZ NOT NULL
    );

CREATE TABLE
    storage (
        id SERIAL PRIMARY KEY,
        storagename VARCHAR(50) NOT NULL,
        fullyqualifiedname VARCHAR(100),
        description VARCHAR(500),
        storagetypeid INT,
        temperature NUMERIC(10, 2),
        code VARCHAR(15),
        parentstorageid INT,
        moredata JSONB,
        lastmodifieddate TIMESTAMPTZ NOT NULL,
        enabled CHARACTER(3),
        laboratoryid INT
    );

CREATE TABLE
    storagecontents (
        id SERIAL PRIMARY KEY,
        storageid INT NOT NULL,
        inventoryid INT NOT NULL,
        number INT,
        moredata JSONB,
        lastmodifieddate TIMESTAMPTZ NOT NULL
    );

CREATE TABLE
    storageitems (
        id SERIAL PRIMARY KEY,
        storagecontentsid INT NOT NULL,
        specimenid INT,
        cultureid INT,
        number INT,
        barcode VARCHAR(50),
        moredata JSONB,
        lastmodifieddate TIMESTAMPTZ NOT NULL
    );

CREATE TABLE
    inventory (
        id SERIAL PRIMARY KEY,
        inventoryname VARCHAR(100) NOT NULL,
        inventorytypeid INT,
        laboratoryid INT,
        moredata JSONB,
        lastmodifieddate TIMESTAMPTZ NOT NULL
    );

CREATE TABLE
    supplierinventory (
        id SERIAL PRIMARY KEY,
        supplierid INT,
        inventoryid INT,
        lastmodifieddate TIMESTAMPTZ NOT NULL
    );

CREATE TABLE
    billingprofile (
        id SERIAL PRIMARY KEY,
        profilename VARCHAR(100) NOT NULL,
        testname VARCHAR(100) NOT NULL,
        testrule JSONB,
        specimenrule JSONB,
        culturerule JSONB,
        patientrule JSONB,
        moredata JSONB,
        laboratoryid INT,
        lastmodifieddate TIMESTAMPTZ NOT NULL
    );

delete from ListItem
where
    id in (
        45,
        46,
        47,
        48,
        49,
        50,
        51,
        52,
        53,
        54,
        55,
        56,
        57,
        58,
        191,
        192,
        193,
        194
    );

INSERT INTO
    list (
        id,
        name,
        grouping,
        parentid,
        common,
        description,
        lastmodifieddate,
        deleted
    ) OVERRIDING SYSTEM VALUE
VALUES
    (
        34,
        'SupplierStatus',
        'System',
        null,
        true,
        'Supplier Status',
        now (),
        false
    ),
    (
        35,
        'StorageType',
        'System',
        null,
        true,
        'Storage Type',
        now (),
        false
    );

INSERT INTO
    listitem (
        id,
        listid,
        value,
        parentid,
        lastmodifieddate,
        fixed,
        enabled,
        displayorder,
        deleted
    ) OVERRIDING SYSTEM VALUE
VALUES
    (
        45,
        34,
        'Active',
        null,
        now (),
        false,
        true,
        1,
        false
    ),
    (
        46,
        34,
        'InActive',
        null,
        now (),
        false,
        true,
        1,
        false
    ),
    (
        47,
        34,
        'Pending Approval',
        null,
        now (),
        false,
        true,
        1,
        false
    ),
    (
        48,
        34,
        'Suspended',
        null,
        now (),
        false,
        true,
        1,
        false
    ),
    (
        49,
        34,
        'Terminated',
        null,
        now (),
        false,
        true,
        1,
        false
    ),
    (
        50,
        34,
        'On Probation',
        null,
        now (),
        false,
        true,
        1,
        false
    ),
    (
        51,
        35,
        'Refrigerated',
        null,
        now (),
        false,
        true,
        1,
        false
    ),
    (
        52,
        35,
        'Freezer',
        null,
        now (),
        false,
        true,
        1,
        false
    ),
    (
        53,
        35,
        'Chemical',
        null,
        now (),
        false,
        true,
        1,
        false
    ),
    (
        54,
        35,
        'Dry',
        null,
        now (),
        false,
        true,
        1,
        false
    ),
    (
        55,
        35,
        'Secure',
        null,
        now (),
        false,
        true,
        1,
        false
    ),
    (
        56,
        35,
        'Ambient',
        null,
        now (),
        false,
        true,
        1,
        false
    ),
    (
        57,
        35,
        'Cryogenic',
        null,
        now (),
        false,
        true,
        1,
        false
    ),
    (
        58,
        35,
        'Specialized',
        null,
        now (),
        false,
        true,
        1,
        false
    );

INSERT INTO
    listitem (
        id,
        listid,
        value,
        parentid,
        lastmodifieddate,
        fixed,
        enabled,
        displayorder,
        deleted
    ) OVERRIDING SYSTEM VALUE
VALUES
    (
        191,
        74,
        'Asset',
        null,
        now (),
        false,
        true,
        1,
        false
    ),
    (
        192,
        74,
        'addsupplier',
        191,
        now (),
        false,
        true,
        1,
        false
    ),
    (
        193,
        74,
        'editsupplier',
        191,
        now (),
        false,
        true,
        1,
        false
    ),
    (
        194,
        74,
        'deletesupplier',
        191,
        now (),
        false,
        true,
        1,
        false
    );

INSERT INTO
    topictranslation (id, listitemname, displayedtopic) OVERRIDING SYSTEM VALUE
VALUES
    (26, 'Asset', 'Asset Tracking');
