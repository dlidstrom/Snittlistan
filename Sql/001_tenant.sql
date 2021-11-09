-- index any foreign keys!
CREATE TABLE snittlistan.tenant (
    tenant_id INT PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
    hostname VARCHAR(255) NOT NULL,
    favicon VARCHAR(255) NOT NULL,
    apple_touch_icon VARCHAR(255) NOT NULL,
    apple_touch_icon_size VARCHAR(255) NOT NULL,
    web_app_title VARCHAR(255) NOT NULL,
    club_id INT NOT NULL,
    created_date TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    created_by VARCHAR(255) NOT NULL
);
