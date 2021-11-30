-- index any foreign keys!
CREATE TABLE snittlistan.published_task (
    published_task_id INT PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
    tenant_id INT NOT NULL REFERENCES tenant,
    correlation_id UUID NOT NULL,
    causation_id UUID NULL,
    message_id UUID NOT NULL,
    business_key VARCHAR(255) NOT NULL,
    data JSONB NOT NULL,
    publish_date TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    handled_date TIMESTAMP WITHOUT TIME ZONE NULL,
    created_date TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    created_by VARCHAR(255) NOT NULL
);
CREATE INDEX published_task_tenant_idx ON snittlistan.published_task(tenant_id);
CREATE UNIQUE INDEX published_task_business_key_uk ON snittlistan.published_task(business_key) WHERE handled_date IS NULL;
