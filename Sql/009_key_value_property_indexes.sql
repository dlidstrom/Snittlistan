ALTER TABLE snittlistan.key_value_property
    DROP CONSTRAINT IF EXISTS key_value_property_key_key;
CREATE UNIQUE INDEX published_task_tenant_uk ON
snittlistan.key_value_property(tenant_id, key);
