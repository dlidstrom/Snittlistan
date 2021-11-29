-- index any foreign keys!
CREATE TABLE snittlistan.tenant (
    tenant_id INT PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
    club_id INT NOT NULL UNIQUE,
    team_full_name VARCHAR(255) NOT NULL,
    database_name VARCHAR(255) NOT NULL,
    hostname VARCHAR(255) NOT NULL UNIQUE,
    favicon VARCHAR(255) NOT NULL,
    apple_touch_icon VARCHAR(255) NOT NULL,
    apple_touch_icon_size VARCHAR(255) NOT NULL,
    web_app_title VARCHAR(255) NOT NULL,
    created_date TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    created_by VARCHAR(255) NOT NULL,
    handled_date TIMESTAMP WITHOUT TIME ZONE NULL
);
