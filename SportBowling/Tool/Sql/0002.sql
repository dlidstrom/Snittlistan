CREATE TABLE IF NOT EXISTS "Test" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK_Test" PRIMARY KEY ("MigrationId")
);
