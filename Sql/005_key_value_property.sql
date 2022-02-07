-- index any foreign keys!
CREATE TABLE snittlistan.key_value_property (
    key_value_property_id INT PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
    tenant_id INT NOT NULL REFERENCES snittlistan.tenant,
    key varchar(100) NOT NULL UNIQUE, -- for example, user-1:logon_mail, feature_toggles
    value JSONB NOT NULL,
    updated_date TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    created_date TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP
);
CREATE INDEX key_value_property_tenant_idx ON
snittlistan.key_value_property(tenant_id);
