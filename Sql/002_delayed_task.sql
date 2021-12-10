-- index any foreign keys!
CREATE TABLE snittlistan.delayed_task (
    delayed_task_id INT PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
    tenant_id INT NOT NULL REFERENCES snittlistan.tenant,
    correlation_id UUID NOT NULL,
    causation_id UUID NULL,
    message_id UUID NOT NULL,
    business_key VARCHAR(255) NOT NULL,
    data JSONB NOT NULL,
    publish_date TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    published_date TIMESTAMP WITHOUT TIME ZONE NULL,
    created_by VARCHAR(255) NOT NULL,
    created_date TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP
);
CREATE INDEX delayed_task_tenant_idx ON
snittlistan.delayed_task(tenant_id);
CREATE UNIQUE INDEX delayed_task_published_date_uk ON
snittlistan.delayed_task(business_key)
WHERE
published_date IS NULL;
