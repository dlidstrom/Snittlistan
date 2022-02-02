-- index any foreign keys!
CREATE TABLE snittlistan.published_task (
    published_task_id INT PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
    tenant_id INT NOT NULL REFERENCES snittlistan.tenant,
    correlation_id UUID NOT NULL,
    causation_id UUID NULL,
    message_id UUID NULL, -- set if command came from task, not set if came from controller
    business_key VARCHAR(1024) NOT NULL,
    data JSONB NOT NULL,
    publish_date TIMESTAMP WITHOUT TIME ZONE NOT NULL, -- when to publish (DateTime.Now if immediately)
    published_date TIMESTAMP WITHOUT TIME ZONE NULL, -- when it was published
    handled_date TIMESTAMP WITHOUT TIME ZONE NULL, -- when it was handled
    created_by VARCHAR(255) NOT NULL,
    created_date TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP -- when it was created
);
CREATE INDEX published_task_tenant_idx ON
snittlistan.published_task(tenant_id);
